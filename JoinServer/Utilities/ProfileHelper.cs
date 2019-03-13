using DataAccessLayer;
using JoinServer.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace JoinServer.Utilities
{
    public class ProfileHelper
    {
        public static bool IsProfileFound(string deviceId, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = "select count(*) from Profile where deviceid = @deviceid";
            dataLayer.AddParameter("@deviceid", deviceId);
            return ((int)dataLayer.ExecuteScalar() > 0);
        }

        public static void InsertProfile(Profile profile, IDataLayer dataLayer)
        {
            try
            {
                dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                dataLayer.Sql = "insert into Profile (deviceid, username, profilename, hobies, about, reviews, views) values(@deviceId, @username, @profilename, @hobies, @about, @reviews, @views)";
                dataLayer.AddParameter("@deviceId", profile.DeviceID);
                dataLayer.AddParameter("@username", profile.UserName);
                //dataLayer.AddParameter("@profilephoto", profile.ProfilePhoto);
                dataLayer.AddParameter("@profilename", profile.ProfileName);
                dataLayer.AddParameter("@hobies", profile.Hobies);
                dataLayer.AddParameter("@about", profile.About);
                //dataLayer.AddParameter("@rating", profile.Rating);
                dataLayer.AddParameter("@reviews", profile.Reviews);
                dataLayer.AddParameter("@views", profile.views);
                dataLayer.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateProfile(Profile profile, IDataLayer dataLayer)
        {
            try
            {
                dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                dataLayer.Sql = "update Profile set username = @username, profilename = @profilename, hobies = @hobies, " +
                    "about = @about, reviews = @reviews, views = @views where deviceid = @deviceid";
                dataLayer.AddParameter("@deviceid", profile.DeviceID);
                dataLayer.AddParameter("@username", profile.UserName);
                dataLayer.AddParameter("@profilename", profile.ProfileName);
                dataLayer.AddParameter("@hobies", profile.Hobies);
                dataLayer.AddParameter("@about", profile.About);
                //dataLayer.AddParameter("@rating", profile.Rating);
                dataLayer.AddParameter("@reviews", profile.Reviews);
                dataLayer.AddParameter("@views", profile.views);
                dataLayer.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Profile GetProfile(string deviceId, IDataLayer dataLayer)
        {
            Profile profile = null;
            try
            {
                dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                dataLayer.Sql = @"select p.deviceid, p.username, p.profilename, p.about, p.hobies, p.views, count(pr.deviceid) reviews, AVG(pr.rating) rating
                                    from profile p left join ProfileReviews pr on pr.deviceid = p.deviceid 
                                    where p.deviceid = @deviceid
                                    group by pr.deviceid, p.deviceid, p.username, p.profilename, p.about, p.hobies, p.views";
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
                        Rating = dataTable.Rows[0]["rating"] == DBNull.Value ? 0 : float.Parse(dataTable.Rows[0]["rating"].ToString()),
                        Reviews = long.Parse(dataTable.Rows[0]["reviews"].ToString()),
                        views = long.Parse(dataTable.Rows[0]["views"].ToString())
                    };
                }
                else if (dataTable.Rows.Count > 1)
                {
                    //return new HttpError("More than one profile found");
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return profile;
        }

        public static void InsertProfileReview(ProfileReview profile, IDataLayer dataLayer)
        {
            try
            {
                dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                dataLayer.Sql = "insert into ProfileReviews (reviewfrom, deviceid, username, review, revieweddate, rating) " +
                    "values(@reviewfrom, @deviceid, @username, @review, @revieweddate, @rating)";
                dataLayer.AddParameter("@reviewfrom", profile.FromDeviceID);
                dataLayer.AddParameter("@deviceid", profile.DeviceID);
                dataLayer.AddParameter("@username", profile.UserName);
                dataLayer.AddParameter("@review", profile.Review);
                dataLayer.AddParameter("@revieweddate", DateTime.Now);
                dataLayer.AddParameter("@rating", profile.Rating);
                dataLayer.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}