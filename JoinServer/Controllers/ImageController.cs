using DataAccessLayer;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace JoinServer.Controllers
{
    [Authorize]
    public class ImagesController : ApiController
    {
        // GET api/values
        // POST api/values
        [Route("Images/{deviceid}/{filename}")]
        [HttpGet]
        public HttpResponseMessage GetImage([FromUri] string deviceId, [FromUri] string fileName)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                Image image = Image.FromFile(ConfigurationManager.AppSettings["ActivityImagesPath"] + $"{deviceId}\\{fileName}.jpeg");
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

        // POST: api/Profile
        [HttpPost, Route("Images/{deviceId}/{activity}")]
        public IHttpActionResult PostImageAsync([FromUri] string deviceId, [FromUri] string activity)
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
                                        string fileFolder = ConfigurationManager.AppSettings["ActivityImagesPath"] + $"{deviceId}\\";
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
                                            SavePicture(deviceId, activity, fileName, dataLayer);
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

        private static void SavePicture(string deviceId, string activity, string fileName, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = string.Format("update activity set imagePath = @imagePath where deviceid =  @deviceid and [id] = @activityid");
            dataLayer.AddParameter("@deviceid", deviceId);
            dataLayer.AddParameter("@activityid", activity);
            dataLayer.AddParameter("@imagePath", deviceId + "\\" + fileName);
            dataLayer.ExecuteNonQuery();
        }

        private Image GetMachingImages(string activity, IDataLayer dataLayer)
        {
            Image returnImage = null;
            byte[] image = new byte[0];
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = string.Format("select distinct deviceId, Lat, Long, what, description, image from Activity where what = '{0}'", activity);
            DataTable dataTable = dataLayer.ExecuteDataTable();
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                image = ((byte[])dataTable.Rows[0]["image"]);
                MemoryStream ms = new MemoryStream(image);
                try
                {
                    returnImage = Image.FromStream(ms);
                }
                catch (ArgumentException ex)
                {
                }
            }
            return returnImage;
        }
    }
}
