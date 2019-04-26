using DataAccessLayer;
using JoinServer.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace JoinServer.Utilities
{
    public class ActivityRequestHelper
    {
        public static bool AddRequest(ActivityRequest activityRequest, IDataLayer dataLayer)
        {
            try
            {
                activityRequest.ActivityRequestId = Guid.NewGuid();
                dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                dataLayer.Sql = @"INSERT INTO [dbo].[activityrequests] ([activityrequestid], [requestfrom], [requestto], [requestdate], [status], [activityid], [changedate], [reqesttype])
                                    values(@activityrequestid, @requestfrom, @requestto, @requestdate, @status, @activityid, @changedate, @reqesttype);SELECT SCOPE_IDENTITY();";
                dataLayer.AddParameter("@activityrequestid", activityRequest.ActivityRequestId);
                dataLayer.AddParameter("@requestfrom", activityRequest.RequestFrom);
                dataLayer.AddParameter("@requestto", activityRequest.RequestTo);
                dataLayer.AddParameter("@requestdate", activityRequest.RequestDate);
                dataLayer.AddParameter("@status", (int)activityRequest.RequestStatus);
                dataLayer.AddParameter("@activityid", activityRequest.ActivityId);
                dataLayer.AddParameter("@changedate", activityRequest.RequestStatusChangeDate);
                dataLayer.AddParameter("@reqesttype", (int)activityRequest.RequestType);
                dataLayer.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool HandleRequestChange(ActivityRequest activityRequest, NotificationDetails notificationDetails, IDataLayer dataLayer)
        {
            try
            {
                ActivityRequest existingRequest = null;
                if (activityRequest != null)
                {
                    existingRequest = GetRequestIfExists(activityRequest.ActivityId, activityRequest.RequestFrom, activityRequest.RequestTo, dataLayer);
                }
                if (existingRequest == null)
                {
                    AddRequest(activityRequest, dataLayer);
                    notificationDetails.RequestId = activityRequest.ActivityRequestId.ToString();
                    NotificationsHelper.InsertNotification(notificationDetails, dataLayer);
                }
                else
                {
                    List<NotificationDetails> notifications = NotificationsHelper.GetNotificationsByRequestId(existingRequest.ActivityRequestId.ToString(), dataLayer);
                    if (notifications.Count > 0)
                    {
                        foreach (NotificationDetails notification in notifications)
                        {
                            NotificationsHelper.UpdateNotificationStatus(notification.NotificationId, MessageStatuses.ACTED, dataLayer);
                        }
                    }
                    if (!(existingRequest.RequestFrom == activityRequest.RequestFrom &&
                            existingRequest.RequestTo == activityRequest.RequestTo &&
                            existingRequest.RequestType == activityRequest.RequestType &&
                            existingRequest.RequestStatus == activityRequest.RequestStatus))
                    {
                        activityRequest.ActivityRequestId = existingRequest.ActivityRequestId;
                        UpdateExistingRequest(activityRequest, dataLayer);
                        NotificationsHelper.InsertNotification(notificationDetails, dataLayer);
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool UpdateExistingRequest(ActivityRequest activityRequest, IDataLayer dataLayer)
        {
            try
            {
                dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                dataLayer.Sql = @"update activityrequests set [requestfrom]=@requestfrom, [requestto]=@requestto, [status]=@status, 
                                    [activityid]=@activityid, [changedate]=@changedate, [reqesttype]=@requesttype where activityrequestid=@activityrequestid";
                dataLayer.AddParameter("@activityrequestid", activityRequest.ActivityRequestId);
                dataLayer.AddParameter("@requestfrom", activityRequest.RequestFrom);
                dataLayer.AddParameter("@requestto", activityRequest.RequestTo);
                //dataLayer.AddParameter("@requestdate", activityRequest.RequestDate);
                dataLayer.AddParameter("@status", (int)activityRequest.RequestStatus);
                dataLayer.AddParameter("@activityid", activityRequest.ActivityId);
                dataLayer.AddParameter("@changedate", activityRequest.RequestStatusChangeDate);
                dataLayer.AddParameter("@requesttype", activityRequest.RequestType);
                dataLayer.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool DeleteActivityRequests(string activityId, IDataLayer dataLayer)
        {
            try
            {
                dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                dataLayer.Sql = @"delete from activityrequests where activityrequestid=@activityrequestid";
                dataLayer.AddParameter("@activityid", activityId);
                 dataLayer.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                dataLayer.RollbackTransaction();
                return false;
            }
        }

        public static ActivityRequest GetRequestIfExists(string activityId, string requestFrom, string requestTo, IDataLayer dataLayer)
        {
            ActivityRequest existingRequest = null;
            try
            {
                dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                dataLayer.Sql = @"SELECT [activityrequestid], [requestfrom], [requestto], [requestdate], [status], [activityid], [changedate], [reqesttype] FROM [dbo].[activityrequests]
                                    where activityid=@activityid and requestfrom = @requestfrom and requestto = @requestto";
                dataLayer.AddParameter("@activityid", activityId);
                dataLayer.AddParameter("@requestfrom", requestFrom);
                dataLayer.AddParameter("@requestto", requestTo);
                DataTable table = dataLayer.ExecuteDataTable();
                if (table.Rows != null && table.Rows.Count > 0)
                {
                    existingRequest = new ActivityRequest()
                    {
                        ActivityRequestId = Guid.Parse(table.Rows[0]["activityrequestid"].ToString()),
                        RequestFrom = table.Rows[0]["requestfrom"].ToString(),
                        RequestTo = table.Rows[0]["requestto"].ToString(),
                        RequestDate = DateTime.Parse(table.Rows[0]["requestdate"].ToString()),
                        RequestStatus = ((RequestStatus) table.Rows[0]["status"]),
                        ActivityId = table.Rows[0]["activityid"].ToString(),
                        RequestStatusChangeDate = DateTime.Parse(table.Rows[0]["changedate"].ToString()),
                        RequestType = ((NotificationType)table.Rows[0]["reqesttype"])
                    };
                }
            }
            catch (Exception ex)
            {
            }
            return existingRequest;
        }
    }
}