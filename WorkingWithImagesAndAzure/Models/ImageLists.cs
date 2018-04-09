using System;
using System.IO;

namespace WorkingWithImagesAndAzure.Models
{
    /// <summary>
    /// Image Collection, describes the properties of the image uploaded
    /// </summary>
    public class ImageLists
    {
        #region Private Collections
        private Guid _imageID = Guid.Empty;
        private string _imageTitle = string.Empty;
        private string _imageData = string.Empty;
        private string _assetID = string.Empty;
        private long _imageSize = 0;
        #endregion

        #region Public Properties 
        /// <summary>
        /// The GUID of image
        /// </summary>
        public Guid ImageID
        {
            get
            {
                return _imageID;
            }
            set
            {
                if (value != Guid.Empty && value != _imageID)
                {
                    _imageID = value;
                }
            }
        }
        /// <summary>
        /// The name of the image, a string value
        /// </summary>
        public string Title
        {
            get
            {
                return _imageTitle;
            }
            set
            {
                if (value != string.Empty && value != _imageTitle)
                    _imageTitle = value;
            }
        }
        /// <summary>
        /// AssetID
        /// </summary>
        public string AssetID
        {
            get
            {
                return _assetID;
            }
            set
            {
                if (value != string.Empty && value != _assetID)
                    _assetID = value;
            }
        }
        /// <summary>
        /// The filesteam of the single image uploaded
        /// </summary>
        public string ImageData
        {
            get
            {
                return _imageData;
            }
            set
            {
                if (value != null && value != _imageData)
                    _imageData = value;
            }
        }
        /// <summary>
        /// ImageSize
        /// </summary>
        public long ImageSize
        {
            get
            {
                return _imageSize;
            }
            set
            {
                if (value != 0 && value != _imageSize)
                    _imageSize = value;
            }
        }
        #endregion

    }
}