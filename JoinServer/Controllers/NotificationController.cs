using DataAccessLayer;
using JoinServer.Models;
using JoinServer.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace JoinServer.Controllers
{
    public class NotificationController : ApiController
    {
        // GET: api/Notification
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Notification/5
        public string Get(int id)
        {
            return "value";
        }

        [Route("Notification")]
        public void Post([FromBody] NotificationRequest notificationRequest)
        {
            SendNotifications(notificationRequest);
        }

        [Route("Notification/{deviceid}")]
        public List<NotificationDetails> GetNotificationsForDevice([FromUri] string deviceId)
        {
            using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
            {
                return NotificationsHelper.GetMyNotifications(deviceId, dataLayer);
            }
        }

        [Route("Notification/{activityId}/{fromdeviceid}/{todeviceid}")]
        public ActivityRequest GetNotificationsForDevice([FromUri] string activityId, [FromUri] string fromDeviceId, [FromUri] string toDeviceId)
        {
            using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
            {
                return ActivityRequestHelper.GetRequestIfExists(activityId, fromDeviceId, toDeviceId, dataLayer);
            }
        }

        private void SendNotifications(NotificationRequest notificationRequest)
        {
            Activity activity = null;
            Device toDevice = null;
            Device fromDevice = null;
            string deviceIdForNotification = null;
            try
            {
                using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
                {
                    dataLayer.BeginTransaction();
                    toDevice = Getdevice(notificationRequest.ToDeviceId, dataLayer);
                    activity = ActivityHelper.GetActivity(notificationRequest.ActivityId, dataLayer);
                    if (activity != null)
                    {
                        fromDevice = Getdevice(notificationRequest.FromDeviceId, dataLayer);
                        toDevice = Getdevice(notificationRequest.ToDeviceId, dataLayer);
                    }
                }
                if (toDevice != null && fromDevice != null)
                {
                    string[] devices = { toDevice.NotificationToken };
                    string body = null;
                    string title = null;
                    ActivityRequest request = null;
                    switch (notificationRequest.NotificationRequestStatus)
                    {
                        case RequestStatus.NEW:
                            {
                                title = activity.What;
                                body = $"{fromDevice.EmailID} wants in on {title}, what do you say?";

                                request = new ActivityRequest()
                                {
                                    RequestFrom = notificationRequest.FromDeviceId,
                                    RequestTo = notificationRequest.ToDeviceId,
                                    ActivityId = notificationRequest.ActivityId,
                                    RequestDate = DateTime.Now,
                                    RequestStatus = notificationRequest.NotificationRequestStatus,
                                    RequestStatusChangeDate = DateTime.Now,
                                    RequestType = notificationRequest.RequestNotificationType
                                };
                            }
                            break;
                        case RequestStatus.ACCEPTED:
                        case RequestStatus.REJECTED:
                            {
                                title = activity.What;
                                body = $"{fromDevice.EmailID} {notificationRequest.NotificationRequestStatus.ToString()} your request to {title}";

                                request = new ActivityRequest()
                                {
                                    RequestFrom = notificationRequest.ToDeviceId,
                                    RequestTo = notificationRequest.FromDeviceId,
                                    ActivityId = notificationRequest.ActivityId,
                                    RequestDate = DateTime.Now,
                                    RequestStatus = notificationRequest.NotificationRequestStatus,
                                    RequestStatusChangeDate = DateTime.Now,
                                    RequestType = notificationRequest.RequestNotificationType
                                };
                            }
                            break;
                    }

                    string notificationId = Guid.NewGuid().ToString();
                    NotificationDetails notificationDetails = new NotificationDetails()
                    {
                        CreatedOn = DateTime.Now,
                        ActivityId = activity.ActivityID,
                        DeviceId = toDevice.DeviceID,
                        UpdatedOn = DateTime.Now,
                        Dismissed = false,
                        NotificationText = body,
                        MessageObject = notificationRequest,
                        MessageStatus = MessageStatuses.NEW,
                        NotificationId = notificationId
                    };
                    notificationRequest.NotificationId = notificationId;
                    //sendStatus.Wait();
                    //if (sendStatus.Result)
                    //{
                    bool isHandleRequestChangeSuccessfull = false;
                    using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
                    {
                        isHandleRequestChangeSuccessfull = ActivityRequestHelper.HandleRequestChange(request, notificationDetails, dataLayer);
                       // NotificationsHelper.InsertNotification(notificationDetails, dataLayer);
                    }
                    //}
                    if (isHandleRequestChangeSuccessfull)
                    {
                        Task<bool> sendStatus = NotificationsHelper.SendNotification(devices, title, body, notificationRequest);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        // PUT: api/Notification/5
        [Route("Notification/{notificationid}")]
        public void Put([FromUri] string notificationId, [FromBody] NotificationRequest notificationRequest)
        {
        }

        // DELETE: api/Notification/5
        [Route("Notification/{notificationid}")]
        public void Delete([FromUri] string notificationId)
        {
            try
            {
                using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
                {
                    NotificationsHelper.UpdateNotificationStatus(notificationId, MessageStatuses.ACTED, dataLayer);
                }
            }catch(Exception ex)
            {
                
            }
        }

        private Device Getdevice(string deviceId, IDataLayer dataLayer)
        {
            Device device = null;
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = "select EmailId, SoftwareVersion, NotificationToken from Devices where deviceid=@deviceid order by CreatedOn desc";
            dataLayer.AddParameter("@deviceid", deviceId);
            DataTable table = dataLayer.ExecuteDataTable();
            if (table.Rows != null && table.Rows.Count > 0)
            {
                device = new Device()
                {
                    DeviceID = deviceId,
                    EmailID = table.Rows[0]["EmailId"].ToString(),
                    SoftwareVersion = table.Rows[0]["SoftwareVersion"].ToString(),
                    NotificationToken = table.Rows[0]["NotificationToken"].ToString()
                };
            }
            return device;
        }
    }
}
