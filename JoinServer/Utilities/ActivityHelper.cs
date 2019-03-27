using DataAccessLayer;
using JoinServer.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace JoinServer.Utilities
{
    public class ActivityHelper
    {
        public static Activity GetActivity(string activityId, IDataLayer dataLayer)
        {
            Activity activity = null;
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = @"select deviceId, what, [when] activityTime, description, lat, long, imagepath from activity where id=@activityid";
            dataLayer.AddParameter("@activityid", activityId);
            DataTable table = dataLayer.ExecuteDataTable();
            if (table.Rows != null && table.Rows.Count > 0)
            {
                activity = new Activity()
                {
                    description = table.Rows[0]["description"].ToString(),
                    DeviceID = table.Rows[0]["deviceId"].ToString(),
                    What = table.Rows[0]["what"].ToString(),
                    When = DateTime.Parse(table.Rows[0]["activityTime"].ToString()),
                    Lat = double.Parse(table.Rows[0]["lat"].ToString()),
                    Long = double.Parse(table.Rows[0]["long"].ToString())
                };
            }
            return activity;
        }

        public static bool IsActivityFound(ActivitySettings activitySetting, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = "select count(*) from Activity where activityid = @activityid";
            dataLayer.AddParameter("@activityid", activitySetting.ActivityId);
            return ((int)dataLayer.ExecuteScalar() > 0);
        }

        public static void InsertActivitySettings(ActivitySettings activity, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = @"INSERT INTO [dbo].[activitysettings]
                                   ([activityid]
                                   ,[starttime]
                                   ,[endtime]
                                   ,[activtyType]
                                   ,[activitystatus]
                                   ,[comments])
                             VALUES
                                   (@activityid
                                   ,@starttime
                                   ,@endtime
                                   ,@activtyType
                                   ,@activitystatus
                                   ,@comments)";
            dataLayer.AddParameter("@activityid", activity.ActivityId);
            dataLayer.AddParameter("@starttime", activity.StartTime);
            dataLayer.AddParameter("@endtime", activity.EndTime);
            dataLayer.AddParameter("@activtyType", activity.ActivityType);
            dataLayer.AddParameter("@activitystatus", activity.ActivityStatus);
            dataLayer.AddParameter("@comments", activity.Comments ?? "");
            dataLayer.ExecuteNonQuery();
        }

        public static void UpdateUpdateActivity(ActivitySettings activity, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = @"UPDATE [dbo].[activitysettings]
                                SET [starttime] = @starttime
                                    ,[endtime] = @endtime
                                    ,[activtyType] = @activtyType
                                    ,[activitystatus] = @activitystatus
                                    ,[activityreviews] = @activityreviews
                                    ,[activityviews] = @activityviews
                                    ,[comments] = @comments
                                WHERE activityid = @activityid";
            dataLayer.AddParameter("@activityid", activity.ActivityId);
            dataLayer.AddParameter("@starttime", activity.StartTime);
            dataLayer.AddParameter("@endtime", activity.EndTime);
            dataLayer.AddParameter("@activtyType", activity.ActivityType);
            dataLayer.AddParameter("@activitystatus", activity.ActivityStatus);
            dataLayer.AddParameter("@activityreviews", activity.ActivityReviews);
            dataLayer.AddParameter("@activityviews", activity.ActivityViews);
            dataLayer.AddParameter("@comments", activity.Comments);
            dataLayer.ExecuteNonQuery();
        }

        // PUT api/values/5
        public static ActivitySettings getActivitySettingsById(string activityId, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = @"SELECT [activityid]
                                      ,[starttime]
                                      ,[endtime]
                                      ,[activtyType]
                                      ,[activitystatus]
                                      ,[activityreviews]
                                      ,[activityviews]
                                      ,[comments]
                                  FROM [dbo].[activitysettings]
                                  WHERE activityid = @activityid";
            dataLayer.AddParameter("@activityid", activityId);
            ActivitySettings activitySetttings = null;
            DataTable dataTable = dataLayer.ExecuteDataTable();
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                activitySetttings = new ActivitySettings()
                {
                    ActivityId = Guid.Parse(row["activityid"].ToString()),
                    StartTime = DateTime.Parse(row["starttime"].ToString()),
                    EndTime = DateTime.Parse(row["endtime"].ToString()),
                    ActivityType = (ActivityTypes)int.Parse(row["activtyType"].ToString()),
                    ActivityStatus = (ActivityStatuses)int.Parse(row["activitystatus"].ToString()),
                    ActivityReviews = long.Parse(row["activityreviews"].ToString()),
                    ActivityViews = long.Parse(row["activityviews"].ToString()),
                    Comments = row["comments"].ToString()
                };
            }
            return activitySetttings;
        }

        public static bool IsDeviceFound(Activity activity, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = "select count(*) from Activity where deviceid = @deviceid";
            dataLayer.AddParameter("@deviceid", activity.DeviceID);
            return ((int)dataLayer.ExecuteScalar() > 0);
        }

        public static Guid InsertActivity(Activity activity, IDataLayer dataLayer)
        {
            Guid activityId = Guid.NewGuid();
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = "insert into Activity(deviceid, what, [when], lat, long, description, id) values(@deviceId, @what, @when, @lat, @long, @description, @id)";
            dataLayer.AddParameter("@deviceId", activity.DeviceID);
            dataLayer.AddParameter("@What", activity.What);
            dataLayer.AddParameter("@when", activity.When);
            dataLayer.AddParameter("@lat", activity.Lat);
            dataLayer.AddParameter("@long", activity.Long);
            dataLayer.AddParameter("@description", activity.description);
            dataLayer.AddParameter("@id", activityId);
            dataLayer.ExecuteNonQuery();
            return activityId;
        }

        public static void UpdateUpdateActivity(Activity activity, IDataLayer dataLayer)
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

        public static List<CurrentActivity> GetMachingLocations(string activity, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            if (string.IsNullOrEmpty(activity))
            {
                dataLayer.Sql = @"select distinct deviceId, Lat, Long, what, description, imagepath, id, ase.activtyType, ase.starttime, ase.endtime from Activity a inner join activitysettings ase
								on ase.activityid = a.Id and  getdate() between ase.starttime and ase.endtime";
            }
            else
            {
                dataLayer.Sql = @"select distinct deviceId, Lat, Long, what, description, imagepath, id, ase.activtyType, ase.starttime, ase.endtime from Activity a inner join activitysettings ase
								on ase.activityid = a.Id and  getdate() between ase.starttime and ase.endtime
                                 where what like '%'+@activity+'%'";
                dataLayer.AddParameter("@activity", activity);
            }
            List<CurrentActivity> locations = new List<CurrentActivity>();
            foreach (DataRow row in dataLayer.ExecuteDataTable().Rows)
            {
                var duplicateLocation = locations.Find(x => x.Lat == double.Parse(row["lat"].ToString()) && x.Long == double.Parse(row["long"].ToString()));
                //if (duplicateLocation != null)
                //{
                //    double newLat, newLong;
                //    getRandomLocationInCircle(duplicateLocation.Long, duplicateLocation.Lat, 40, out newLat, out newLong);
                //    duplicateLocation.Lat = newLat;
                //    duplicateLocation.Long = newLong;
                //}
                locations.Add(new CurrentActivity()
                {
                    DeviceID = row["deviceid"].ToString(),
                    Lat = double.Parse(row["lat"].ToString()),
                    Long = double.Parse(row["long"].ToString()),
                    Activity = row["what"].ToString(),
                    Description = row["description"].ToString(),
                    ImagePath = row["imagepath"].ToString().Split('.')[0],
                    ActivityId = row["id"].ToString().Split('.')[0],
                    ActivityType = ((ActivityTypes) int.Parse(row["activtyType"].ToString())).ToString(),
                    ActivityStartTime = DateTime.Parse(row["starttime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                    ActivityEndTime = DateTime.Parse(row["endtime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                });
            }
            
            return locations;
        }

        private static void getRandomLocationInCircle(double x0, double y0, int radius, out double foundLatitude, out double foundLongitude)
        {
            Random random = new Random();

            // Convert radius from meters to degrees
            double radiusInDegrees = radius / 111000f;

            double u = random.NextDouble();
            double v = random.NextDouble();
            double w = radiusInDegrees * Math.Sqrt(u);
            double t = 2 * Math.PI * v;
            double x = w * Math.Cos(t);
            double y = w * Math.Sin(t);

            // Adjust the x-coordinate for the shrinking of the east-west distances
            double new_x = x / Math.Cos((Math.PI / 180) * y0);

            foundLongitude = new_x + x0;
            foundLatitude = y + y0;
        }
        
        // PUT api/values/5
        public static CurrentActivity GetLocationByActivityId(string activityId, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = @"select distinct deviceId, Lat, Long, what, description, imagepath, id, ase.activtyType, ase.starttime, ase.endtime from Activity a inner join activitysettings ase
								on ase.activityid = a.Id where id = '" + activityId + "'";
            CurrentActivity location = null;
            DataTable dataTable = dataLayer.ExecuteDataTable();
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                location = new CurrentActivity()
                {
                    DeviceID = row["deviceid"].ToString(),
                    Lat = double.Parse(row["lat"].ToString()),
                    Long = double.Parse(row["long"].ToString()),
                    Activity = row["what"].ToString(),
                    Description = row["description"].ToString(),
                    ImagePath = row["imagepath"].ToString().Split('.')[0],
                    ActivityId = row["id"].ToString().Split('.')[0],
                    ActivityType = ((ActivityTypes)int.Parse(row["activtyType"].ToString())).ToString(),
                    ActivityStartTime = DateTime.Parse(row["starttime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                    ActivityEndTime = DateTime.Parse(row["endtime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss")
                };
            }
            return location;
        }

    }
}