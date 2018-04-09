using ImageResizer;
using Microsoft.WindowsAzure.MediaServices.Client;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace WorkingWithImagesAndAzure.Models
{
    public class AzureHelper : IDisposable
    {
        #region Constants 
        private static readonly string imgRszeWdth = ConfigurationManager.AppSettings["imgRszeWdth"];
        private static readonly string imgRszeHgth = ConfigurationManager.AppSettings["imgRszeHgth"];
        private static readonly string mediaServicesAccountName = ConfigurationManager.AppSettings["MediaServicesAccountName"];
        private static readonly string mediaServicesAccountKey = ConfigurationManager.AppSettings["MediaServicesAccountKey"];
        private static readonly string myAzureStorageConSetting = ConfigurationManager.AppSettings["myAzureStorageConSetting"];
        private static readonly string myAzureCon = ConfigurationManager.ConnectionStrings["MediaStorage"].ConnectionString;
        #endregion

        #region Private Methods
        private static MemoryStream ResizeImage(Stream downloaded)
        {
            var memoryStream = new MemoryStream();
            var settings = string.Format("mode=crop&width={0}&height={1}", Convert.ToInt32(imgRszeWdth), Convert.ToInt32(imgRszeHgth));
            downloaded.Seek(0, SeekOrigin.Begin);
            var i = new ImageJob(downloaded, memoryStream, new Instructions(settings));
            i.Build();
            memoryStream.Position = 0;
            return memoryStream;
        }

        private string CreateBLOBContainer(string containerName)
        {
            try
            {
                string result = string.Empty;
                CloudMediaContext mediaContext;
                mediaContext = new CloudMediaContext(mediaServicesAccountName, mediaServicesAccountKey);
                IAsset asset = mediaContext.Assets.Create(containerName, AssetCreationOptions.None);
                return asset.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Public Methods 
        public async Task<List<ImageLists>> UploadImages(List<Stream> strmLists, List<string> lstContntTypes)
        {
            string myContainerName = "Test007";
            string assetID = CreateBLOBContainer(myContainerName);
            assetID = assetID.Replace("nb:cid:UUID:", "asset-");
            List<ImageLists> retCollection = new List<ImageLists>();
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(myAzureStorageConSetting);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(assetID);
            container.SetPermissions(
              new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            if (strmLists != null)
            {
                for (int i = 0; i < strmLists.Count; i++)
                {
                    string strExtension = string.Empty;
                    if (lstContntTypes[i] == "image/gif")
                    {
                        strExtension = ".gif";
                    }
                    else if (lstContntTypes[i] == "image/jpeg")
                    {
                        strExtension = ".jpeg";
                    }
                    else if (lstContntTypes[i] == "image/jpg")
                    {
                        strExtension = ".jpg";
                    }
                    else if (lstContntTypes[i] == "image/png")
                    {
                        strExtension = ".png";
                    }
                    ImageLists img = new ImageLists();
                    string imgGUID = Guid.NewGuid().ToString();
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(string.Concat(imgGUID, strExtension));
                    await blockBlob.UploadFromStreamAsync(strmLists[i]);

                    img.ImageID = new Guid(imgGUID);
                    img.Title = string.Concat(imgGUID, strExtension);
                    img.ImageSize = strmLists[i].Length;
                    img.AssetID = assetID;
                    retCollection.Add(img);

                    CloudBlockBlob blockblobthumb = container.GetBlockBlobReference(string.Concat(imgGUID, "_thumb", strExtension));
                    Stream strmThumb = ResizeImage(strmLists[i]);
                    using (strmThumb)
                    {
                        await blockblobthumb.UploadFromStreamAsync(strmThumb);

                        img = new ImageLists();
                        img.ImageID = new Guid(imgGUID);
                        img.Title = string.Concat(imgGUID, "_thumb", strExtension);
                        img.ImageSize = strmThumb.Length;
                        img.AssetID = assetID;
                        retCollection.Add(img);
                    }
                }

            }
            return retCollection;
        }
        #endregion

        void IDisposable.Dispose()
        {

        }
    }
}
