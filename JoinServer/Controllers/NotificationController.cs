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
            Activity activity = null;
            Device device = null;
            using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
            {
                dataLayer.BeginTransaction();
                try
                {
                    activity = ActivityHelper.GetActivity(notificationRequest.ActivityId, dataLayer);
                    if (activity != null)
                    {
                        device = Getdevice(activity.DeviceID, dataLayer);
                    }
                }
                catch (Exception ex)
                {
                    dataLayer.RollbackTransaction();
                }
            }
            if (device != null)
            {
                string[] devices = { device.NotificationToken };
                string title = activity.What;
                string body = $"{notificationRequest.DeviceId} wants in on {title}, what do you say?";

                Task<bool> sendStatus = NotificationsHelper.SendNotification(devices, title, body, activity);
            }

        }


        // PUT: api/Notification/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Notification/5
        public void Delete(int id)
        {
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
