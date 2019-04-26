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
            dataLayer.Sql = @"select id, deviceId, what, [when] activityTime, description, lat, long, imagepath from activity where id=@activityid";
            dataLayer.AddParameter("@activityid", activityId);
            DataTable table = dataLayer.ExecuteDataTable();
            if (table.Rows != null && table.Rows.Count > 0)
            {
                activity = new Activity()
                {
                    ActivityID = table.Rows[0]["id"].ToString(),
                    description = table.Rows[0]["description"].ToString(),
                    DeviceID = table.Rows[0]["deviceId"].ToString(),
                    What = table.Rows[0]["what"].ToString(),
                    When = table.Rows[0]["activityTime"].ToString(),
                    Lat = double.Parse(table.Rows[0]["lat"].ToString()),
                    Long = double.Parse(table.Rows[0]["long"].ToString())
                };
            }
            return activity;
        }

        public static bool IsActivityFound(string activityId, IDataLayer dataLayer)
        {
            if (string.IsNullOrEmpty(activityId))
            {
                return false;
            }
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = "select count(*) from Activity where id = @activityid";
            dataLayer.AddParameter("@activityid", activityId);
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
            dataLayer.AddParameter("@activtyType", Enum.Parse(typeof(ActivityTypes), activity.ActivityType));
            dataLayer.AddParameter("@activitystatus", Enum.Parse(typeof(ActivityStatuses), activity.ActivityStatus));
            dataLayer.AddParameter("@comments", activity.Comments ?? "");
            dataLayer.ExecuteNonQuery();
        }

        public static void UpdateMinActivitySettings(ActivitySettings activitySettings, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = @"UPDATE [dbo].[activitysettings]
                                SET [starttime] = @starttime
                                    ,[endtime] = @endtime
                                    ,[activtyType] = @activtyType
                                WHERE activityid = @activityid";
            dataLayer.AddParameter("@activityid", activitySettings.ActivityId);
            dataLayer.AddParameter("@starttime", activitySettings.StartTime);
            dataLayer.AddParameter("@endtime", activitySettings.EndTime);
            dataLayer.AddParameter("@activtyType", Enum.Parse(typeof(ActivityTypes), activitySettings.ActivityType));
            //dataLayer.AddParameter("@activitystatus", activitySettings.ActivityStatus);
            //dataLayer.AddParameter("@activityreviews", activitySettings.ActivityReviews);
            //dataLayer.AddParameter("@activityviews", activitySettings.ActivityViews);
            //dataLayer.AddParameter("@comments", activitySettings.Comments);
            dataLayer.ExecuteNonQuery();
        }

        public static void DeleteActivitySettings(string activityId, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = @"delete from [dbo].[activitysettings]
                                WHERE activityid = @activityid";
            dataLayer.AddParameter("@activityid", activityId);
            dataLayer.ExecuteNonQuery();
        }

        public static void DeleteActivity(string activityId, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = @"delete from [dbo].[activity]
                                WHERE id = @activityid";
            dataLayer.AddParameter("@activityid", activityId);
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
                    StartTime = row["starttime"].ToString(),
                    EndTime = row["endtime"].ToString(),
                    ActivityType = ((ActivityTypes)int.Parse(row["activtyType"].ToString())).ToString(),
                    ActivityStatus = ((ActivityStatuses)int.Parse(row["activitystatus"].ToString())).ToString(),
                    ActivityReviews = long.Parse(row["activityreviews"].ToString()),
                    ActivityViews = long.Parse(row["activityviews"].ToString()),
                    Comments = row["comments"].ToString()
                };
            }
            return activitySetttings;
        }

        //public static bool IsDeviceFound(Activity activity, IDataLayer dataLayer)
        //{
        //    dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        //    dataLayer.Sql = "select count(*) from Activity where id = @activityid";
        //    dataLayer.AddParameter("@activityid", activity.);
        //    return ((int)dataLayer.ExecuteScalar() > 0);
        //}

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

        public static void UpdateActivity(Activity activity, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = "update Activity set [when] = @when, description = @description where id = @activityid";
            dataLayer.AddParameter("@activityid", activity.ActivityID);
            //dataLayer.AddParameter("@What", activity.What);
            dataLayer.AddParameter("@description", activity.description);
            dataLayer.AddParameter("@when", activity.When);
            //dataLayer.AddParameter("@lat", activity.Lat);
            //dataLayer.AddParameter("@long", activity.Long);
            dataLayer.ExecuteNonQuery();
        }

        public static List<CurrentActivity> GetMachingLocations(string device, string activity, double topLat, double bottomLat, double leftLng, double rightLng, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            if (string.IsNullOrEmpty(activity))
            {
                dataLayer.Sql = @"select distinct a.deviceId, Lat, Long, what, description, imagepath, a.id, ase.activtyType, ase.starttime, ase.endtime, ar.activityrequestid, ar.status, AVG(pr.rating) rating from Activity a 
								 inner join activitysettings ase on ase.activityid = a.Id and  getdate() between ase.starttime and ase.endtime
								left join activityrequests ar on ar.requestfrom = @device and ar.activityid = a.Id
								left join ProfileReviews pr on pr.deviceid = a.deviceid
								where lat between @bottomLat and @topLat and long between @leftLng and @rightLng and a.deviceid = @device
								group by a.deviceId, Lat, Long, what, description, imagepath, a.id, ase.activtyType, ase.starttime, ase.endtime, ar.activityrequestid, ar.status";
                dataLayer.AddParameter("@device", device);
            }
            else
            {
                dataLayer.Sql = @"select distinct a.deviceId, Lat, Long, what, description, imagepath, a.id, ase.activtyType, ase.starttime, ase.endtime, ar.activityrequestid, ar.status, AVG(pr.rating) rating from Activity a inner join activitysettings ase
								on ase.activityid = a.Id and  getdate() between ase.starttime and ase.endtime
								left join activityrequests ar on ar.requestfrom = @device and ar.activityid = a.Id
								left join ProfileReviews pr on pr.deviceid = a.deviceid
                                 where what like '%'+@activity+'%' and lat between @bottomLat and @topLat and long between @leftLng and @rightLng and a.deviceid = @device
								 group by a.deviceId, Lat, Long, what, description, imagepath, a.id, ase.activtyType, ase.starttime, ase.endtime, ar.activityrequestid, ar.status";
                dataLayer.AddParameter("@device", device);
                dataLayer.AddParameter("@activity", activity);
            }
            dataLayer.AddParameter("@topLat", topLat);
            dataLayer.AddParameter("@bottomLat", bottomLat);
            dataLayer.AddParameter("@leftLng", leftLng);
            dataLayer.AddParameter("@rightLng", rightLng);
            List<CurrentActivity> locations = new List<CurrentActivity>();
            foreach (DataRow row in dataLayer.ExecuteDataTable().Rows)
            {
                if (row["status"] != DBNull.Value && (RequestStatus)int.Parse(row["status"].ToString()) == RequestStatus.REJECTED)
                {
                    continue;
                }
                locations.Add(FillLocation(row));
            }

            return locations;
        }

        public static List<Activity> GetMyActivities(string device, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = @"select distinct a.deviceId, Lat, Long, what, [when], description, imagepath, a.id, ase.activtyType, ase.starttime, ase.endtime, null as activityrequestid, null as status, AVG(pr.rating) rating from Activity a 
								 inner join activitysettings ase on ase.activityid = a.Id and  getdate() <= ase.endtime
								left join ProfileReviews pr on pr.deviceid = a.deviceid
								where a.deviceid = @device
								group by a.deviceId, Lat, Long, what, [when], description, imagepath, a.id, ase.activtyType, ase.starttime, ase.endtime";
            dataLayer.AddParameter("@device", device);
            List<Activity> locations = new List<Activity>();
            foreach (DataRow row in dataLayer.ExecuteDataTable().Rows)
            {
                if (row["status"] != DBNull.Value && (RequestStatus)int.Parse(row["status"].ToString()) == RequestStatus.REJECTED)
                {
                    continue;
                }
                locations.Add(FillMyActivities(row));
            }

            return locations;
        }

        private static Activity FillMyActivities(DataRow row)
        {
            return new Activity()
            {
                DeviceID = row["deviceid"].ToString(),
                Lat = double.Parse(row["lat"].ToString()),
                Long = double.Parse(row["long"].ToString()),
                What = row["what"].ToString(),
                When = row["when"].ToString(),
                description = row["description"].ToString(),
                ImagePath = row["imagepath"].ToString().Split('.')[0],
                ActivityID = row["id"].ToString().Split('.')[0],
                ActivitySetting = new ActivitySettings()
                {
                    ActivityType = ((ActivityTypes)int.Parse(row["activtyType"].ToString())).ToString(),
                    StartTime = DateTime.Parse(row["starttime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                    EndTime = DateTime.Parse(row["endtime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                    ActivityStatus = ((RequestStatus)int.Parse(row["status"] == DBNull.Value ? "0" : row["status"].ToString())).ToString(),
                    //                    ProfileRating = float.Parse(row["rating"] == DBNull.Value ? "0.0" : row["rating"].ToString())
                },
            };
        }

        private static CurrentActivity FillLocation(DataRow row)
        {
            return new CurrentActivity()
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
                ActivityEndTime = DateTime.Parse(row["endtime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                ActivityRequestStatus = ((RequestStatus)int.Parse(row["status"] == DBNull.Value ? "0" : row["status"].ToString())).ToString(),
                ProfileRating = float.Parse(row["rating"] == DBNull.Value ? "0.0" : row["rating"].ToString())
            };
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

        public static List<Profile> GetActivitySubscribers(string activityId, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = @"select a.deviceid, p.username, p.profilename, p.about, p.hobies, p.views, count(pr.deviceid) reviews, AVG(pr.rating) rating
                                    from Activity a inner join activityrequests ar on ar.activityid = a.Id and ar.status=4
									 left join  profile p on p.deviceid = ar.requestfrom left join ProfileReviews pr on pr.deviceid = p.deviceid 
									where a.Id=@activityid and p.username is not null
                                    group by pr.deviceid, a.deviceid, p.username, p.profilename, p.about, p.hobies, p.views";
            dataLayer.AddParameter("@activityid", activityId);
            List<Profile> profiles = new List<Profile>();
            DataTable dataTable = dataLayer.ExecuteDataTable();
            foreach (DataRow row in dataTable.Rows)
            {
                profiles.Add(new Profile()
                {
                    DeviceID = (string)row["deviceid"],
                    UserName = (string)row["username"],
                    ProfileName = (string)row["profilename"],
                    Hobies = (string)row["hobies"],
                    About = (string)row["about"],
                    Rating = row["rating"] == DBNull.Value ? 0 : float.Parse(row["rating"].ToString()),
                    Reviews = long.Parse(row["reviews"].ToString()),
                    views = long.Parse(row["views"].ToString())
                });
            }
            return profiles;
        }


        public static List<Device> GetDevicesToNotify(string activityId, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = @"select d.deviceid, d.emailid, d.softwareversion, d.NotificationToken
                                    from Activity a inner join activityrequests ar on ar.activityid = a.Id and ar.status=4
									 inner join Devices d on d.DeviceId = ar.requestfrom
									where a.Id=@activityid";
            dataLayer.AddParameter("@activityid", activityId);
            List<Device> devices = new List<Device>();
            DataTable dataTable = dataLayer.ExecuteDataTable();
            foreach (DataRow row in dataTable.Rows)
            {
                devices.Add(new Device()
                {
                    DeviceID = row["deviceid"].ToString(),
                    EmailID = row["emailid"].ToString(),
                    SoftwareVersion = row["softwareversion"].ToString(),
                    NotificationToken = row["NotificationToken"].ToString()
                });
            }
            return devices;
        }

    }
}
