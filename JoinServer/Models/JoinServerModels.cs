using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JoinServer.Models
{
    public class CurrentActivity
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "DeviceId")]
        public string DeviceID { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Lattitude")]
        public double Lat { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Longitude")]
        public double Long { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "CurrentActivity")]
        public string Activity { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "description")]
        public string Description { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "imagepath")]
        public string ImagePath { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "activityid")]
        public string ActivityId { get; set; }
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
}