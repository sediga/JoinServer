using DataAccessLayer;
using JoinServer.Models;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace JoinServer.Controllers
{
    [Authorize]
    public class ActivityController : ApiController
    {
        // GET api/values
        // POST api/values
        [Route("Activity")]
        public void PostActivity([FromBody]Activity activity)
        {
            try
            {
                //Activity activity = JsonConvert.DeserializeObject<Activity>(value);
                PutAnActivity(activity);
            }
            catch (Exception ex)
            {

            }
        }

        [Route("Activity/{activity}")]
        public List<CurrentActivity> GetActivity([FromUri] string activity)
        {
            List<CurrentActivity> locations = null;
            try
            {
                //Activity activity = JsonConvert.DeserializeObject<Activity>(value);
                using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
                {
                    locations = GetMachingLocations(activity, dataLayer);
                }
            }
            catch (Exception ex)
            {

            }
            return locations;
        }

        private static void PutAnActivity(Activity activity)
        {
            using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
            {
                //if (!IsDeviceFound(activity, dataLayer))
                //{
                    InsertActivity(activity, dataLayer);
                //}
                //else
                //{
                //    UpdateUpdateActivity(activity, dataLayer);

                //}
            }
        }

        private static bool IsDeviceFound(Activity activity, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = "select count(*) from Activity where deviceid = @deviceid";
            dataLayer.AddParameter("@deviceid", activity.DeviceID);
            return ((int)dataLayer.ExecuteScalar() > 0);
        }

        private static void InsertActivity(Activity activity, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = "insert into Activity(deviceid, what, [when], lat, long, description, id) values(@deviceId, @what, @when, @lat, @long, @description, @id)";
            dataLayer.AddParameter("@deviceId", activity.DeviceID);
            dataLayer.AddParameter("@What", activity.What);
            dataLayer.AddParameter("@when", activity.When);
            dataLayer.AddParameter("@lat", activity.Lat);
            dataLayer.AddParameter("@long", activity.Long);
            dataLayer.AddParameter("@description", activity.description);
            dataLayer.AddParameter("@id", Guid.NewGuid());
            dataLayer.ExecuteNonQuery();
        }

        private static void UpdateUpdateActivity(Activity activity, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = "update Activity set what = @what, [when] = @when, lat = @lat, long = @long where deviceid = @deviceid";
            dataLayer.AddParameter("@deviceId", activity.DeviceID);
            dataLayer.AddParameter("@What", activity.What);
            dataLayer.AddParameter("@when", activity.When);
            dataLayer.AddParameter("@lat", activity.Lat);
            dataLayer.AddParameter("@long", activity.Long);
            dataLayer.ExecuteNonQuery();
        }

        private List<CurrentActivity> GetMachingLocations(string activity, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = "select distinct deviceId, Lat, Long, what, description, imagepath, id from Activity where what like '%" + activity+"%'";
            List<CurrentActivity> locations = new List<CurrentActivity>();
            foreach (DataRow row in dataLayer.ExecuteDataTable().Rows)
            {
                locations.Add(new CurrentActivity()
                { DeviceID = row["deviceid"].ToString(),
                    Lat = double.Parse(row["lat"].ToString()),
                    Long = double.Parse(row["long"].ToString()),
                    Activity = row["what"].ToString(),
                    Description = row["description"].ToString(),
                    ImagePath = row["imagepath"].ToString().Split('.')[0],
                    ActivityId = row["id"].ToString().Split('.')[0]
                });
            }
            return locations;
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
