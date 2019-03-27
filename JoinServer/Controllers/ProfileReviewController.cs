using DataAccessLayer;
using JoinServer.Models;
using JoinServer.Utilities;
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
    [Authorize]
    public class ProfileReviewController : ApiController
    {
        // GET: api/Profile
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Profile/5
        [Route("profilereview/{deviceId}")]
        public List<ProfileReview> Get([FromUri] string deviceId)
        {
            using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
            {
                return ProfileHelper.GetProfileReviews(deviceId, dataLayer);
            }
            return null;
        }

        // POST: api/Profile
        [Route("profilereview")]
        public void Post([FromBody]ProfileReview profileReview)
        {

            using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
            {
                if (ProfileHelper.IsProfileFound(profileReview.DeviceID, dataLayer))
                {
                    ProfileHelper.InsertProfileReview(profileReview, dataLayer);
                }
                //else
                //{
                //    UpdateProfile(profile, dataLayer);
                //}
            }
        }

        // PUT: api/Profile/5
        [Route("profilereview/{deviceId}")]
        public void Put([FromUri] string deviceId, [FromBody]Profile profile)
        {
            //using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
            //{
            //    if (!IsProfileFound(deviceId, dataLayer))
            //    {
            //        InsertProfile(profile, dataLayer);
            //    }
            //    else
            //    {
            //        profile.DeviceID = deviceId;
            //        UpdateProfile(profile, dataLayer);
            //    }
            //}
        }

        // DELETE: api/Profile/5
        public void Delete(int id)
        {
        }
    }
}
