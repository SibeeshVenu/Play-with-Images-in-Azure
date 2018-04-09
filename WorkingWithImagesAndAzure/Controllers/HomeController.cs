using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WorkingWithImagesAndAzure.Models;
namespace WorkingWithImagesAndAzure.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Upload()
        {
            return View();
        }

        #region UploadImages
        /// <summary>
        /// Upload images to the cloud and database. User can upload a single image or a collection of images.
        /// </summary>  
        [HttpPost]
        public async Task<JsonResult> UploadFile()
        {
            try
            {
                List<ImageLists> prcssdImgLists = null;
                if (HttpContext.Request.Files.AllKeys.Any())
                {
                    var httpPostedFile = HttpContext.Request.Files;
                    if (httpPostedFile != null)
                    {
                        string result = string.Empty;
                        string returnJson = string.Empty;
                        using (var ah = new AzureHelper())
                        {
                            List<Stream> strmLists = new List<Stream>();
                            List<string> lstContntTypes = new List<string>();
                            for (int i = 0; i < httpPostedFile.Count; i++)
                            {
                                strmLists.Add(httpPostedFile[i].InputStream);
                                lstContntTypes.Add(httpPostedFile[i].ContentType);
                            }
                            prcssdImgLists = await ah.UploadImages(strmLists, lstContntTypes);
                        }

                    }
                }
                return Json(prcssdImgLists, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public ActionResult Index()
        {
            return View();
        }
    }
}