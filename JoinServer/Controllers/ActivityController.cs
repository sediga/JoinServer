using DataAccessLayer;
using JoinServer.Models;
using JoinServer.Utilities;
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
        public Activity PostActivity([FromBody]Activity activity)
        {
            using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
            {
                postActivity(activity, dataLayer);
            }

            return activity;
        }

        [Route("Activity/{device}/{activity}/{toplat}/{bottomlat}/{leftlng}/{rightlng}")]
        public List<CurrentActivity> GetActivity([FromUri] string device, [FromUri] string activity, [FromUri] double toplat, [FromUri] double bottomlat, [FromUri] double leftlng, [FromUri] double rightlng)
        {
            List<CurrentActivity> locations = null;
            try
            {
                //Activity activity = JsonConvert.DeserializeObject<Activity>(value);
                using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
                {
                    locations = ActivityHelper.GetMachingLocations(device, activity, toplat, bottomlat, leftlng, rightlng, dataLayer);
                }
            }
            catch (Exception ex)
            {

            }
            return locations;
        }

        [Route("Activity/{device}/{toplat}/{bottomlat}/{leftlng}/{rightlng}")]
        public List<CurrentActivity> GetAllActivity([FromUri] string device, [FromUri] double toplat, [FromUri] double bottomlat, [FromUri] double leftlng, [FromUri] double rightlng)
        {
            List<CurrentActivity> locations = null;
            try
            {
                //Activity activity = JsonConvert.DeserializeObject<Activity>(value);
                using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
                {
                    locations = ActivityHelper.GetMachingLocations(device, null, toplat, bottomlat, leftlng, rightlng, dataLayer);
                }
            }
            catch (Exception ex)
            {

            }
            return locations;
        }

        [Route("MyActivities/{device}")]
        public List<Activity> GetMyActivities([FromUri] string device)
        {
            List<Activity> locations = null;
            try
            {
                //Activity activity = JsonConvert.DeserializeObject<Activity>(value);
                using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
                {
                    locations = ActivityHelper.GetMyActivities(device, dataLayer);
                }
            }
            catch (Exception ex)
            {

            }
            return locations;
        }

        [Route("ActivityById/{activityid}")]
        public CurrentActivity GetActivityById([FromUri] string activityId)
        {
            CurrentActivity location = null;
            try
            {
                //Activity activity = JsonConvert.DeserializeObject<Activity>(value);
                using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
                {
                    location = ActivityHelper.GetLocationByActivityId(activityId, dataLayer);
                }
            }
            catch (Exception ex)
            {

            }
            return location;
        }

        [Route("ActivityById/{activityid}/Subscribers")]
        public List<Profile> GetActivitySubscribers([FromUri] string activityId)
        {
            List<Profile> profiles = null;
            try
            {
                //Activity activity = JsonConvert.DeserializeObject<Activity>(value);
                using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
                {
                    profiles = ActivityHelper.GetActivitySubscribers(activityId, dataLayer);
                }
            }
            catch (Exception ex)
            {

            }
            return profiles;
        }

        [Route("Activity")]
        public Activity PutActivity(Activity activity)
        {
            using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
            {
                if (!ActivityHelper.IsActivityFound(activity.ActivityID, dataLayer))
                {
                    return postActivity(activity, dataLayer);
                }
                else
                {
                    try
                    {
                        dataLayer.BeginTransaction();
                        ActivityHelper.UpdateActivity(activity, dataLayer);
                        ActivityHelper.UpdateMinActivitySettings(activity.ActivitySetting, dataLayer);
                        dataLayer.CommitTransaction();
                        SendUpdateNotifications(activity.ActivityID, dataLayer);
                    }
                    catch (Exception ex)
                    {
                        dataLayer.RollbackTransaction();
                        return null;
                    }

                }
                return activity;
            }
        }



        // DELETE api/values/5
        [Route("Activity/{activityid}")]
        public void DeleteActivity(string activityId)
        {
            using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
            {
                try
                {
                    SendDeleteNotifications(activityId, dataLayer);
                    dataLayer.BeginTransaction();
                    ActivityRequestHelper.DeleteActivityRequests(activityId, dataLayer);
                    ActivityHelper.DeleteActivitySettings(activityId, dataLayer);
                    ActivityHelper.DeleteActivity(activityId, dataLayer);
                    dataLayer.CommitTransaction();
                }
                catch (Exception ex)
                {
                    dataLayer.RollbackTransaction();
                    throw ex;
                }
            }
        }

        private static void SendDeleteNotifications(string activityId, IDataLayer dataLayer)
        {
            List<string> tokens = ActivityHelper.GetTokensToNotify(activityId, dataLayer);
            if (tokens.Count > 0)
            {
                Activity activity = ActivityHelper.GetActivity(activityId, dataLayer);
                NotificationsHelper.SendNotification(tokens.ToArray(), "Activity Update", $"{activity.What} has been deleted", activity);
            }
        }

        private static void SendUpdateNotifications(string activityId, IDataLayer dataLayer)
        {
            List<string> tokens = ActivityHelper.GetTokensToNotify(activityId, dataLayer);
            if (tokens.Count > 0)
            {
                Activity activity = ActivityHelper.GetActivity(activityId, dataLayer);
                NotificationsHelper.SendNotification(tokens.ToArray(), "Activity Update", $"{activity.What} has been updated", activity);
            }
        }

        private Activity postActivity(Activity activity, IDataLayer dataLayer)
        {
            try
            {
                dataLayer.BeginTransaction();
                //Activity activity = JsonConvert.DeserializeObject<Activity>(value);
                Guid activityId = ActivityHelper.InsertActivity(activity, dataLayer);
                activity.ActivityID = activityId.ToString();
                if (activity.ActivitySetting != null)
                {
                    activity.ActivitySetting.ActivityId = activityId;
                    ActivityHelper.InsertActivitySettings(activity.ActivitySetting, dataLayer);
                }
                dataLayer.CommitTransaction();
            }
            catch (Exception ex)
            {
                dataLayer.RollbackTransaction();
                return null;
            }
            return activity;
        }

    }
}
