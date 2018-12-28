using DataAccessLayer;
using JoinServer.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JoinServer.Controllers
{
    public class ProfileController : ApiController
    {
        // GET: api/Profile
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Profile/5
        [Route("profile/{deviceId}")]
        public Profile Get([FromUri] string deviceId)
        {
            using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
            {
                return GetProfile(deviceId, dataLayer);
            }
        }

        // POST: api/Profile
        [Route("profile")]
        public void Post([FromBody]Profile profile)
        {

            using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
            {
                if (!IsProfileFound(profile.DeviceID, dataLayer))
                {
                    InsertProfile(profile, dataLayer);
                }
                //else
                //{
                //    UpdateProfile(profile, dataLayer);
                //}
            }
        }

        // POST: api/Profile
        [HttpPost, Route("profile/{deviceId}")]
        public void PostImage([FromUri] string deviceId)
        {
            var request = System.Web.HttpContext.Current.Request;
            var filePath = "C:\\windows\\temp\\" + deviceId + ".jpg";
            //if (!File.Exists(filePath)) File.Create(filePath);
            using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.OpenOrCreate))
            {
                request.InputStream.CopyTo(fileStream);
            }

            using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
            {
                if (IsProfileFound(deviceId, dataLayer))
                {
                    SavePicture(deviceId, filePath, dataLayer);
                }
                //else
                //{
                //    UpdateProfile(profile, dataLayer);
                //}
            }
        }

        private static void SavePicture(string deviceId, string filePath, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = string.Format("update activity set image = (select * from openrowset(BULK N'{0}', SINGLE_BLOB) AS CategoryImage) where deviceid =  @deviceid", filePath);
            dataLayer.AddParameter("@deviceid", deviceId);
            dataLayer.ExecuteNonQuery();
        }

        private static bool IsProfileFound(string deviceId, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = "select count(*) from Profile where deviceid = @deviceid";
            dataLayer.AddParameter("@deviceid", deviceId);
            return ((int)dataLayer.ExecuteScalar() > 0);
        }

        private static void InsertProfile(Profile profile, IDataLayer dataLayer)
        {
            try
            {
                dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                dataLayer.Sql = "insert into Profile (deviceid, username, profilename, hobies, about, rating, reviews, views) values(@deviceId, @username, @profilename, @hobies, @about, @rating, @reviews, @views)";
                dataLayer.AddParameter("@deviceId", profile.DeviceID);
                dataLayer.AddParameter("@username", profile.UserName);
                //dataLayer.AddParameter("@profilephoto", profile.ProfilePhoto);
                dataLayer.AddParameter("@profilename", profile.ProfileName);
                dataLayer.AddParameter("@hobies", profile.Hobies);
                dataLayer.AddParameter("@about", profile.About);
                dataLayer.AddParameter("@rating", profile.Rating);
                dataLayer.AddParameter("@reviews", profile.Reviews);
                dataLayer.AddParameter("@views", profile.views);
                dataLayer.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void UpdateProfile(Profile profile, IDataLayer dataLayer)
        {
            try
            {
                dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                dataLayer.Sql = "update Profile set username = @username, profilename = @profilename, hobies = @hobies, about = @about, rating = @rating, reviews = @reviews, views = @reviews where deviceid = @deviceid";
                dataLayer.AddParameter("@deviceid", profile.DeviceID);
                dataLayer.AddParameter("@username", profile.UserName);
                dataLayer.AddParameter("@profilename", profile.ProfileName);
                dataLayer.AddParameter("@hobies", profile.Hobies);
                dataLayer.AddParameter("@about", profile.About);
                dataLayer.AddParameter("@rating", profile.Rating);
                dataLayer.AddParameter("@reviews", profile.Reviews);
                dataLayer.AddParameter("@views", profile.views);
                dataLayer.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static Profile GetProfile(string deviceId, IDataLayer dataLayer)
        {
            Profile profile = null;
            try
            {
                dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                dataLayer.Sql = "select * from profile where deviceid = @deviceid";
                dataLayer.AddParameter("@deviceid", deviceId);
                DataTable dataTable = dataLayer.ExecuteDataTable();
                if (dataTable != null && dataTable.Rows != null && dataTable.Rows.Count == 1)
                {
                    profile = new Profile()
                    {
                        DeviceID = (string)dataTable.Rows[0]["deviceid"],
                        UserName = (string)dataTable.Rows[0]["username"],
                        ProfileName = (string)dataTable.Rows[0]["profilename"],
                        Hobies = (string)dataTable.Rows[0]["hobies"],
                        About = (string)dataTable.Rows[0]["about"],
                        //Rating = (byte)dataTable.Rows[0]["rating"],
                        //Reviews = (long)dataTable.Rows[0]["reviews"],
                        //views = (long)dataTable.Rows[0]["views"]
                    };
                }
                else if (dataTable.Rows.Count > 1)
                {
                    //return new HttpError("More than one profile found");
                }
            }catch(Exception ex)
            {
                //throw ex;
            }
            return profile;
        }

        // PUT: api/Profile/5
        [Route("profile/{deviceId}")]
        public void Put([FromUri] string deviceId, [FromBody]Profile profile)
        {
            using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
            {
                if (!IsProfileFound(deviceId, dataLayer))
                {
                    InsertProfile(profile, dataLayer);
                }
                else
                {
                    profile.DeviceID = deviceId;
                    UpdateProfile(profile, dataLayer);
                }
            }
        }

        // DELETE: api/Profile/5
        public void Delete(int id)
        {
        }
    }
}
