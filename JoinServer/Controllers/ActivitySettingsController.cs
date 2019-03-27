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
    public class ActivitySettingsController : ApiController
    {
        // GET api/values
        // POST api/values
        [Route("ActivitySettings")]
        public void PostActivityWithSettings([FromBody]Activity activity, [FromBody]ActivitySettings activitySetting)
        {
            using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
            {
                try
                {
                    dataLayer.BeginTransaction();
                    //Activity activity = JsonConvert.DeserializeObject<Activity>(value);
                    activitySetting.ActivityId = ActivityHelper.InsertActivity(activity, dataLayer);
                    ActivityHelper.InsertActivitySettings(activitySetting, dataLayer);
                    dataLayer.CommitTransaction();
                }
                catch (Exception ex)
                {
                    dataLayer.RollbackTransaction();
                }
            }
        }

        [Route("ActivitySettings/{activityid}")]
        public ActivitySettings GetActivitySettingsById([FromUri] string activityId)
        {
            ActivitySettings activitySettings = null;
            try
            {
                //Activity activity = JsonConvert.DeserializeObject<Activity>(value);
                using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
                {
                    activitySettings = ActivityHelper.getActivitySettingsById(activityId, dataLayer);
                }
            }
            catch (Exception ex)
            {

            }
            return activitySettings;
        }

        private static void PutAnActivity(ActivitySettings activity)
        {
            using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
            {
                //if (!IsDeviceFound(activity, dataLayer))
                //{
                ActivityHelper.InsertActivitySettings(activity, dataLayer);
                //}
                //else
                //{
                //    UpdateUpdateActivity(activity, dataLayer);

                //}
            }
        }

    }
}
