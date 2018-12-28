using DataAccessLayer;
using JoinServer.Models;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
        public List<CurrentLocation> GetActivity([FromUri] string activity)
        {
            List<CurrentLocation> locations = null;
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

        //[Route("Activity/{activity}")]
        public HttpResponseMessage GetImage([FromUri] string activity)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                //Activity activity = JsonConvert.DeserializeObject<Activity>(value);
                using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
                {
                    byte[] image = GetMachingImages(activity, dataLayer);
                    result.Content = new ByteArrayContent(image);
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                }
            }
            catch (Exception ex)
            {

            }
            return result;
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
            dataLayer.Sql = "insert into Activity values(@deviceId, @what, @when, @lat, @long, null, @description, @id)";
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

        private List<CurrentLocation> GetMachingLocations(string activity, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = "select distinct deviceId, Lat, Long, what, description from Activity where what like '%" + activity+"%'";
            List<CurrentLocation> locations = new List<CurrentLocation>();
            foreach (DataRow row in dataLayer.ExecuteDataTable().Rows)
            {
                locations.Add(new CurrentLocation() { DeviceID = row["deviceid"].ToString(), Lat = double.Parse(row["lat"].ToString()), Long = double.Parse(row["long"].ToString()), CurrentActivity = row["what"].ToString(), description = row["description"].ToString() });
            }
            return locations;
        }

        private byte[] GetMachingImages(string activity, IDataLayer dataLayer)
        {
            byte[] image = new byte[0];
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = string.Format("select distinct deviceId, Lat, Long, what, description from Activity where what = '{0}'", activity);
            DataTable dataTable = dataLayer.ExecuteDataTable();
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                image = ((byte[])dataTable.Rows[0]["image"]);
            }
            return image;
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
