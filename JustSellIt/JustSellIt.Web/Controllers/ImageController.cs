using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using JustSellIt.Application.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace JustSellIt.Web.Controllers
{
    public class ImageController : BaseController
    {
        private readonly IConfiguration configuration;

        public ImageController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost]
        public IActionResult Upload(List<string> imageUrls)
        {
            try
            {
                List<string> names = new List<string>();
                string connectionString = configuration.GetConnectionString("AzureConnection");
                BlobContainerClient container = new BlobContainerClient(connectionString, "ProductImages");
                container.CreateIfNotExists(PublicAccessType.Blob);

                foreach (var imageUrl in imageUrls)
                {
                    var name = Guid.NewGuid();
                    names.Add(name.ToString());
                    var blockBlob = container.GetBlobClient(name.ToString());

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(imageUrl);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream receiveStream = response.GetResponseStream();
                    receiveStream.Close();
                    response.Close();

                    blockBlob.Upload(receiveStream);
                }

                return Json(new { Success = true, Names = names });
            }
            catch (Exception ex)
            {
                throw new Exception("Image Upload Failed " + ex);
            }
        }

        [HttpPost]
        public IActionResult Delete(string imageName)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("AzureConnection");
                BlobContainerClient container = new BlobContainerClient(connectionString, "ProductImages");
                container.CreateIfNotExists(PublicAccessType.Blob);
                var blockBlob = container.GetBlobClient(imageName);

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
