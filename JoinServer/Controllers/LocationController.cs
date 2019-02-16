using DataAccessLayer;
using JoinServer.Models;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JoinServer.Controllers
{
    [Authorize]
    public class LocationController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [Route("Location")]
        public void PostLocation([FromBody]CurrentActivity currentLocation)
        {
            try
            {
                //CurrentLocation currentLocation = JsonConvert.DeserializeObject<CurrentLocation>(value);
                PutCurrentLocation(currentLocation);
            }
            catch (Exception ex)
            {

            }
        }

        private static void PutCurrentLocation(CurrentActivity currentLocation)
        {
            using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
            {
                //if (!IsDeviceFound(currentLocation, dataLayer))
                //{
                    InsertCurrentLocation(currentLocation, dataLayer);
                //}
                //else
                //{
                //    UpdateCurrentLocation(currentLocation, dataLayer);
                //}
            }
        }

        private static bool IsDeviceFound(CurrentActivity currentLocation, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = "select count(*) from CurrentLocation where deviceid = @deviceid";
            dataLayer.AddParameter("@deviceid", currentLocation.DeviceID);
            return ((int)dataLayer.ExecuteScalar() > 0);
        }

        private static void InsertCurrentLocation(CurrentActivity currentLocation, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = "insert into CurrentLocation values(@deviceId, @lat, @long)";
            dataLayer.AddParameter("@deviceId", currentLocation.DeviceID);
            dataLayer.AddParameter("@lat", currentLocation.Lat);
            dataLayer.AddParameter("@long", currentLocation.Long);
            dataLayer.ExecuteNonQuery();
        }

        private static void UpdateCurrentLocation(CurrentActivity currentLocation, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = "update CurrentLocation set lat = @lat, long = @long where deviceid = @deviceid";
            dataLayer.AddParameter("@deviceId", currentLocation.DeviceID);
            dataLayer.AddParameter("@lat", currentLocation.Lat);
            dataLayer.AddParameter("@long", currentLocation.Long);
            dataLayer.ExecuteNonQuery();
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
