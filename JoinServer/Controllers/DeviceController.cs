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
    public class DeviceController : ApiController
    {
        // GET: api/Device
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Device/5
        public string Get(string id)
        {
            return "value";
        }

        // POST: api/Device
        [Route("Device")]
        public void PostDevice([FromBody]Device device)
        {
            using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
            {
                InsertDevice(device, dataLayer);
            }
        }

        // POST: api/Device
        [Route("Device/Notification/{deviceid}")]
        public void PostNotificationToDevice([FromUri] string deviceId, [FromBody] NotificationType  notificationType, [FromBody] Notification notification)
        {
            Device device = null;
            using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
            {
                device = Getdevice(deviceId, dataLayer);
            }
            if (device != null)
            {
                string[] devices = { device.NotificationToken };
               Task<bool> sendStatus = NotificationsHelper.SendNotification(devices, notification.title, notification.text, null);
            }

        }

        // PUT: api/Device/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Device/5
        public void Delete(int id)
        {
        }

        private static void InsertDevice(Device device, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = "insert into Devices(deviceid, emailid, softwareversion, notificationtoken) values(@deviceId, @emailid, @softwareversion, @NotificationToken)";
            dataLayer.AddParameter("@deviceId", device.DeviceID);
            dataLayer.AddParameter("@emailid", device.EmailID);
            dataLayer.AddParameter("@softwareversion", device.SoftwareVersion);
            dataLayer.AddParameter("@NotificationToken", device.NotificationToken);
            dataLayer.ExecuteNonQuery();
        }

        private Device Getdevice(string deviceId, IDataLayer dataLayer)
        {
            Device device = null;
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = "select EmailId, SoftwareVersion, NotificationToken from Devices where deviceid=@deviceid";
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
