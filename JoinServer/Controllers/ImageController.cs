using DataAccessLayer;
using JoinServer.Models;
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
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;

namespace JoinServer.Controllers
{
    [Authorize]
    public class ImageController : ApiController
    {
        // GET api/values
        // POST api/values
        [Route("Images/{deviceid}/{filename}")]
        public HttpResponseMessage GetImage([FromUri] string deviceId, [FromUri] string fileName)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                Image image = Image.FromFile(@"D:\Development\Images\" + deviceId + "\\" + fileName + ".jpeg");
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
                                        string fileFolder = @"D:\Development\Images\" + deviceId + "\\";
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

        private static void SavePicture(string deviceId, string what, string fileName, IDataLayer dataLayer)
        {
            dataLayer.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            dataLayer.Sql = string.Format("update activity set image = (select * from openrowset(BULK N'{0}', SINGLE_BLOB) AS CategoryImage) where deviceid =  @deviceid and what = @what", fileName);
            dataLayer.Sql = string.Format("update activity set imagePath = @imagePath where deviceid =  @deviceid and what = @what");
            dataLayer.AddParameter("@deviceid", deviceId);
            dataLayer.AddParameter("@what", what);
            dataLayer.AddParameter("@imagePath", deviceId + "/" + fileName);
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
