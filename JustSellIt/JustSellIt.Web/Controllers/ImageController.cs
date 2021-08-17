using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace JustSellIt.Web.Controllers
{
    public class ImageController : BaseController
    {


        public ImageController()
        {

        }

        [HttpPost]
        public IActionResult UploadImage()
        {
            try
            {
                string connectionString = "";
                BlobContainerClient container = new BlobContainerClient(connectionString, "name");
                container.CreateIfNotExists(PublicAccessType.Blob);
                var blockBlob = container.GetBlobClient("mikepic.png");

                using (var fileStream = System.IO.File.OpenRead(@""))
                {
                    blockBlob.Upload(fileStream);
                }


                return Json(new { Success = true });
            }
            catch (Exception ex)
            {
                throw new Exception("Image Upload Failed " + ex);
            }
        }

        [HttpPost]
        public IActionResult DeleteImage()
        {
            try
            {
                string connectionString = "";
                BlobContainerClient container = new BlobContainerClient(connectionString, "name");
                container.CreateIfNotExists(PublicAccessType.Blob);
                var blockBlob = container.GetBlobClient("mikepic.png");


                blockBlob.DeleteIfExists();



                return Json(new { Success = true });
            }
            catch (Exception ex)
            {
                throw new Exception("Image Delete Failed " + ex);
            }
        }
    }
}
