using DataAccessLayer;
using JoinServer.Models;
using System;
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
    }
}