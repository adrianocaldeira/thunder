using System.IO;

namespace Thunder.Web
{
    /// <summary>
    /// Content type
    /// </summary>
    public static class ContentType
    {
        /// <summary>
        /// Get content type from file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetFromFile(string file)
        {
            var extension = Path.GetExtension(file.Trim());

            if (string.IsNullOrEmpty(extension))
            {
                return null;
            }

            switch (extension.Trim().ToLower())
            {
                case ".pdf":
                    return "application/pdf";
                case ".jpeg":
                case ".jpg":
                    return "image/jpeg";
                case ".gif":
                    return "image/gif";
                case ".png":
                    return "image/png";
                case ".swf":
                    return "application/x-shockwave-flash";
                case ".rtf":
                    return "text/richtext";
                case ".wav":
                    return "audio/wav";
                case ".tiff":
                    return "image/tiff";
                case ".avi":
                    return "video/avi";
                case ".mpeg":
                    return "video/mpeg";
                case ".csv":
                    return "application/vnd.ms-excel";
                case ".doc":
                    return "application/msword";
                case ".docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".xls":
                    return "application/vnd.ms-excel";
                case ".xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".ppt":
                    return "application/vnd.ms-powerpoint";
                case ".pptx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                case ".pps":
                    return "application/vnd.ms-powerpoint";
                case ".ppsx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.slideshow";
                case ".txt":
                    return "text/plain";
                case ".xml":
                    return "application/xml";
                default:
                    return "application/octet-stream";
            }
        }
    }
}