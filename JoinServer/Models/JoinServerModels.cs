using JoinServer.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JoinServer.Models
{
    public class CurrentActivity
    {
        public string DeviceID { get; set; }

        public double Lat { get; set; }

        public double Long { get; set; }

        public string Activity { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public string ActivityId { get; set; }

        public string ActivityType { get; set; }

        public string ActivityStartTime { get; set; }

        public string ActivityEndTime { get; set; }

        public string ActivityRequestStatus { get; set; }

        public float ProfileRating { get; set; }

    }

    public class Activity
    {
        public string ActivityID { get; set; }

        public string DeviceID { get; set; }

        public string What { get; set; }

        public string When { get; set; }

        public double Lat { get; set; }

        public double Long { get; set; }

        public string description { get; set; }

        public string ImagePath { get; set; }

        public ActivitySettings ActivitySetting { get; set; }
    }

    public class Profile
    {
        public string DeviceID { get; set; }

        public string UserName { get; set; }

        public byte[] ProfilePhoto { get; set; }

        public string ProfileName { get; set; }

        public string Hobies { get; set; }

        public string About { get; set; }

        public float Rating { get; set; }

        public long Reviews { get; set; }

        public long views { get; set; }

    }

    public class ProfileReview
    {
        public string FromDeviceID { get; set; }

        public string DeviceID { get; set; }

        public string UserName { get; set; }

        public string Review { get; set; }

        //        public DateTime ReviewedDate { get; set; }

        public decimal Rating { get; set; }
    }

    public class ActivitySettings
    {
        public Guid ActivityId { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string ActivityType { get; set; }

        public string ActivityStatus { get; set; }

        public long ActivityReviews { get; set; }

        public long ActivityViews { get; set; }

        public string Comments { get; set; }
    }
}