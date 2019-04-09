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
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Device Id")]
        public string DeviceID { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "what")]
        public string What { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "When")]
        public DateTime When { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Lat")]
        public double Lat { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Long")]
        public double Long { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "description")]
        public string description { get; set; }

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

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public ActivityTypes ActivityType { get; set; }

        public ActivityStatuses ActivityStatus { get; set; }

        public long ActivityReviews { get; set; }

        public long ActivityViews { get; set; }

        public string Comments { get; set; }
    }
}