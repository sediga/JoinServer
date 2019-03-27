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
                Device latestDevice = GetLatestDevice(device.DeviceID, dataLayer);
                if (latestDevice == null)
                {
                    InsertDevice(device, dataLayer);
                }
                else
                {
                    UpdateDevice(device, dataLayer);
                }
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

        private static void UpdateDevice(Device device, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            if (!string.IsNullOrEmpty(device.NotificationToken))
            {
                dataLayer.Sql = "update Devices set emailid = @emailid, softwareversion = @softwareversion, notificationtoken = @NotificationToken where deviceid = @deviceId";
                dataLayer.AddParameter("@NotificationToken", device.NotificationToken);
            }
            else
            {
                dataLayer.Sql = "update Devices set emailid = @emailid, softwareversion = @softwareversion where deviceid = @deviceId";
            }
            dataLayer.AddParameter("@deviceId", device.DeviceID);
            dataLayer.AddParameter("@emailid", device.EmailID);
            dataLayer.AddParameter("@softwareversion", device.SoftwareVersion);
            dataLayer.ExecuteNonQuery();
        }

        private static Device GetLatestDevice(string device, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = "select * from devices where deviceid=@deviceid order by CreatedOn desc";
            dataLayer.AddParameter("@deviceId", device);
            Device deviceObj = null;
            using (DataTable table = dataLayer.ExecuteDataTable())
            {
                if (table.Rows != null && table.Rows.Count > 0)
                {
                    deviceObj = new Device()
                    {
                        DeviceID = table.Rows[0]["DeviceId"].ToString(),
                        EmailID = table.Rows[0]["EmailId"].ToString(),
                        NotificationToken = table.Rows[0]["NotificationToken"].ToString(),
                        SoftwareVersion = table.Rows[0]["SoftwareVersion"].ToString()
                    };
                }
            }

            return deviceObj;
        }
    }
}
