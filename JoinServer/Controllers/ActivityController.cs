﻿using DataAccessLayer;
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
                try
                {
                    dataLayer.BeginTransaction();
                    //Activity activity = JsonConvert.DeserializeObject<Activity>(value);
                    Guid activityId = ActivityHelper.InsertActivity(activity, dataLayer);
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
            }

            return activity;
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
                    locations = ActivityHelper.GetMachingLocations(activity, dataLayer);
                }
            }
            catch (Exception ex)
            {

            }
            return locations;
        }

        [Route("Activity")]
        public List<CurrentActivity> GetAllActivity()
        {
            List<CurrentActivity> locations = null;
            try
            {
                //Activity activity = JsonConvert.DeserializeObject<Activity>(value);
                using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
                {
                    locations = ActivityHelper.GetMachingLocations(null, dataLayer);
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

        private static void PutAnActivity(Activity activity)
        {
            using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
            {
                //if (!IsDeviceFound(activity, dataLayer))
                //{
                ActivityHelper.InsertActivity(activity, dataLayer);
                //}
                //else
                //{
                //    UpdateUpdateActivity(activity, dataLayer);

                //}
            }
        }

        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
