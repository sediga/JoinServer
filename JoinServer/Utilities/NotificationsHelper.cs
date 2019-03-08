using JoinServer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace JoinServer.Utilities
{
    public class NotificationsHelper
    {
        public static async Task<bool> SendNotification(string[] tokens, string title, string body, object data)
        {
            try
            {
                var messageInformation = new Message()
                {
                    notification = new Notification()
                    {
                        title = title,
                        text = body
                    },
                    data = data,
                    registration_ids = tokens,
                };

                //Object to JSON STRUCTURE => using Newtonsoft.Json;
                string jsonMessage = JsonConvert.SerializeObject(messageInformation);
                string FireBasePushNotificationsURL = ConfigurationManager.AppSettings["FirebaseSendURL"].ToString();
                var request = new HttpRequestMessage(HttpMethod.Post, FireBasePushNotificationsURL);
                request.Headers.TryAddWithoutValidation("Authorization", "key =" + ConfigurationManager.AppSettings["FirebaseAPIKey"].ToString());
                request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");
                HttpResponseMessage result;
                using (var client = new HttpClient())
                {
                    result = await client.SendAsync(request);
                }

                return result.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}