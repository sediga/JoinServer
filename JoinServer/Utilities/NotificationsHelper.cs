using DataAccessLayer;
using JoinServer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace JoinServer.Utilities
{
    public class NotificationsHelper
    {
        public static async Task<bool> SendNotification(string[] tokens, string title, string body, object data)
        {
            try
            {
                var messageInformation = new Message()
                {
                    notification = new Notification()
                    {
                        title = title,
                        text = body
                    },
                    data = data,
                    registration_ids = tokens,
                };

                //Object to JSON STRUCTURE => using Newtonsoft.Json;
                string jsonMessage = JsonConvert.SerializeObject(messageInformation);
                string FireBasePushNotificationsURL = ConfigurationManager.AppSettings["FirebaseSendURL"].ToString();
                var request = new HttpRequestMessage(HttpMethod.Post, FireBasePushNotificationsURL);
                request.Headers.TryAddWithoutValidation("Authorization", "key =" + ConfigurationManager.AppSettings["FirebaseAPIKey"].ToString());
                request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");
                HttpResponseMessage result;
                using (var client = new HttpClient())
                {
                    result = await client.SendAsync(request);
                }

                return result.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static List<NotificationDetails> GetMyNotifications(string deviceid, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = @"SELECT n.notificationid, n.activityid, n.deviceid, n.notificationtext, n.messagestatus, n.createdon, n.updatedon, n.messageobject, n.messageobjecttype
                              FROM [DeviceNotifications] n inner join activitysettings a on a.activityid = n.activityid and getdate() < a.endtime
                              WHERE n.deviceid = @deviceid and dismissed <> 1 order by createdon asc";
            dataLayer.AddParameter("@deviceid", deviceid);
            List<NotificationDetails> notifications = new List<NotificationDetails>();
            foreach (DataRow row in dataLayer.ExecuteDataTable().Rows)
            {
                notifications.Add(FillMyNotification(row));
            }

            return notifications;
        }

        public static List<NotificationDetails> GetNotificationsByRequestId(string requestId, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = @"SELECT n.notificationid, n.activityid, n.deviceid, n.notificationtext, n.messagestatus, n.createdon, n.updatedon, n.messageobject, n.messageobjecttype
                              FROM [DeviceNotifications] n inner join activitysettings a on a.activityid = n.activityid and getdate() < a.endtime
                              WHERE n.requestid = @requestid and dismissed <> 2 order by createdon asc";
            dataLayer.AddParameter("@requestid", requestId);
            List<NotificationDetails> notifications = new List<NotificationDetails>();
            foreach (DataRow row in dataLayer.ExecuteDataTable().Rows)
            {
                notifications.Add(FillMyNotification(row));
            }

            return notifications;
        }

        public static NotificationDetails GetNotificationById(string notificationId, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = @"SELECT n.notificationid, n.activityid, n.deviceid, n.notificationtext, n.messagestatus, n.createdon, n.updatedon, n.messageobject, n.messageobjecttype
                              FROM [DeviceNotifications] n inner join activitysettings a on a.activityid = n.activityid and getdate() < a.endtime
                              WHERE n.notificationid = @notificationid and dismissed <> 2 order by createdon asc";
            dataLayer.AddParameter("@notificationid", notificationId);
            NotificationDetails notifications = null;
            if (dataLayer.ExecuteDataTable().Rows.Count == 1)
            {
                notifications = FillMyNotification(dataLayer.ExecuteDataTable().Rows[0]);
            }

            return notifications;
        }

        private static NotificationDetails FillMyNotification(DataRow row)
        {
            Type type = Type.GetType(row["messageobjecttype"].ToString());
            return new NotificationDetails()
            {
                ActivityId = row["activityid"].ToString(),
                NotificationId = row["notificationid"].ToString(),
                DeviceId = row["deviceid"].ToString(),
                NotificationText = row["notificationtext"].ToString(),
                MessageStatus = row["messagestatus"] == DBNull.Value ? MessageStatuses.NEW : (MessageStatuses)int.Parse(row["messagestatus"].ToString()),
                CreatedOn = DateTime.Parse(row["createdon"].ToString()),
                UpdatedOn = DateTime.Parse(row["updatedon"].ToString()),
                MessageObject = DeserializeFromXml(row["messageobject"].ToString(), type)
            };
        }

        public static Guid InsertNotification(NotificationDetails notificationDetails, IDataLayer dataLayer)
        {
            Guid activityId = Guid.NewGuid();
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            if (notificationDetails.RequestId != null)
            {
                dataLayer.Sql = "insert into DeviceNotifications(notificationid, deviceid, activityid, notificationtext, messagestatus, createdon, updatedon, dismissed, messageobject, messageobjecttype, requestid) " +
                    "values(@notificationid, @deviceid, @activityid, @notificationtext, @messagestatus, @createdon, @updatedon, @dismissed, @messageobject, @messageobjecttype, @requestid)";
                dataLayer.AddParameter("@requestid", notificationDetails.RequestId);
            }
            else
            {
                dataLayer.Sql = "insert into DeviceNotifications(notificationid, deviceid, activityid, notificationtext, messagestatus, createdon, updatedon, dismissed, messageobject, messageobjecttype) " +
                    "values(@notificationid, @deviceid, @activityid, @notificationtext, @messagestatus, @createdon, @updatedon, @dismissed, @messageobject, @messageobjecttype)";
            }
            dataLayer.AddParameter("@notificationid", notificationDetails.NotificationId);
            dataLayer.AddParameter("@deviceid", notificationDetails.DeviceId);
            dataLayer.AddParameter("@activityid", notificationDetails.ActivityId);
            dataLayer.AddParameter("@notificationtext", notificationDetails.NotificationText);
            dataLayer.AddParameter("@messagestatus", notificationDetails.MessageStatus);
            dataLayer.AddParameter("@createdon", notificationDetails.CreatedOn);
            dataLayer.AddParameter("@updatedon", notificationDetails.UpdatedOn);
            dataLayer.AddParameter("@dismissed", 0);
            dataLayer.AddParameter("@messageobject", SerializeToXml(notificationDetails.MessageObject));
            dataLayer.AddParameter("@messageobjecttype", notificationDetails.MessageObject.GetType().ToString());
            dataLayer.ExecuteNonQuery();
            return activityId;
        }

        public static void UpdateNotificationStatus(string notificationId, MessageStatuses messageStatus, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = "update DeviceNotifications set messagestatus = @messagestatus, updatedon = @updatedon, dismissed = @dismissed where notificationid = @notificationid";
            dataLayer.AddParameter("@messagestatus", messageStatus);
            if(messageStatus == MessageStatuses.ACTED)
            {
                dataLayer.AddParameter("@dismissed", 1);
            }else
            {
                dataLayer.AddParameter("@dismissed", 0);
            }
            dataLayer.AddParameter("@updatedon", DateTime.Now);
            dataLayer.AddParameter("@notificationid", notificationId);
            //dataLayer.AddParameter("@lat", activity.Lat);
            //dataLayer.AddParameter("@long", activity.Long);
            dataLayer.ExecuteNonQuery();
        }

        public static void UpdateNotificationStatusByRequestId(string requestId, MessageStatuses messageStatus, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = "update DeviceNotifications set messagestatus = @messagestatus, updatedon = @updatedon where requestid = @requestid";
            dataLayer.AddParameter("@messagestatus", messageStatus);
            dataLayer.AddParameter("@updatedon", DateTime.Now);
            dataLayer.AddParameter("@requestid", requestId);
            //dataLayer.AddParameter("@lat", activity.Lat);
            //dataLayer.AddParameter("@long", activity.Long);
            dataLayer.ExecuteNonQuery();
        }

        public static string SerializeToXml(object value)
        {
            StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
            XmlSerializer serializer = new XmlSerializer(value.GetType());
            serializer.Serialize(writer, value);
            return writer.ToString();
        }

        public static object DeserializeFromXml(String xml, Type type)
        {
            object returnedXmlClass = null;

            try
            {
                using (TextReader reader = new StringReader(xml))
                {
                    try
                    {
                        returnedXmlClass =
                            new XmlSerializer(type).Deserialize(reader);
                    }
                    catch (Exception ex)
                    {
                        // String passed is not XML, simply return defaultXmlClass
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return returnedXmlClass;
        }
    }
}