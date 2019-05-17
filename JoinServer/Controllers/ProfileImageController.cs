using DataAccessLayer;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Hosting;
using System.Web.Http;

namespace JoinServer.Controllers
{
    [Authorize]
    public class ProfileImageController : ApiController
    {
        [Route("ProfileImage/{deviceid}")]
        [HttpGet]
        public HttpResponseMessage GetProfileImage([FromUri] string deviceId)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                string profileImagePath;
                using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
                {
                    profileImagePath = GetProfileImagePath(deviceId, dataLayer);
                }
                Image image = Image.FromFile(HostingEnvironment.MapPath("~") + ConfigurationManager.AppSettings["ProfileImagesPath"] + profileImagePath);
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                    result = new HttpResponseMessage(HttpStatusCode.OK);
                    result.Content = new ByteArrayContent(ms.ToArray());
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                }
                //}
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        [HttpPost, Route("ProfileImage/{deviceId}")]
        public IHttpActionResult PostProfileImageAsync([FromUri] string deviceId)
        {
            try
            {
                if (Request.Content.IsMimeMultipartContent())
                {
                    //For larger files, this might need to be added:
                    Request.Content.LoadIntoBufferAsync().Wait();
                    Request.Content.ReadAsMultipartAsync<MultipartMemoryStreamProvider>(
                            new MultipartMemoryStreamProvider()).ContinueWith((task) =>
                            {
                                try
                                {
                                    MultipartMemoryStreamProvider provider = task.Result;
                                    foreach (HttpContent content in provider.Contents)
                                    {
                                        string fileName = Guid.NewGuid() + ".jpeg";
                                        string fileFolder = HostingEnvironment.MapPath("~") + ConfigurationManager.AppSettings["ProfileImagesPath"] + $"{deviceId}\\";
                                        if (!Directory.Exists(fileFolder))
                                        {
                                            Directory.CreateDirectory(fileFolder);
                                        }
                                        string filePath = fileFolder + fileName;
                                        using (Stream stream = content.ReadAsStreamAsync().Result)
                                        {
                                            using (Image image = Image.FromStream(stream))
                                            {
                                                image.Save(filePath);
                                            }
                                        }
                                        var testName = content.Headers.ContentDisposition.Name;
                                        using (IDataLayer dataLayer = DataLayer.GetInstance(DatabaseTypes.MSSql, false))
                                        {
                                            SavePicture(deviceId, fileName, dataLayer);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            });
                }
                else
                {
                    throw new HttpResponseException(Request.CreateResponse(
                            HttpStatusCode.NotAcceptable,
                            "This request is not properly formatted"));
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

            return Ok();
        }

        private static void SavePicture(string deviceId, string fileName, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = string.Format("update profile set imagepath = @imagePath where deviceid =  @deviceid");
            dataLayer.AddParameter("@deviceid", deviceId);
            dataLayer.AddParameter("@imagePath", deviceId + "\\" + fileName);
            dataLayer.ExecuteNonQuery();
        }

        private string GetProfileImagePath(string deviceID, IDataLayer dataLayer)
        {
            Image returnImage = null;
            byte[] image = new byte[0];
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = "select top 1 imagepath from profile where deviceid = @deviceID";
            dataLayer.AddParameter("@deviceID", deviceID);
            return dataLayer.ExecuteScalar().ToString();
         }
    }
}
