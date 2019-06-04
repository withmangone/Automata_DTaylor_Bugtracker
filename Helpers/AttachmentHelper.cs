using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace Automata_DTaylor_Bugtracker.Helpers
{
    public class AttachmentHelper
    {
        public static bool IsWebFriendlyAttachment(HttpPostedFileBase file)
        {
            if (file == null)
                return false;

            if (file.ContentLength > 2 * 1024 * 1024 || file.ContentLength < 1024)
                return false;

            try
            {
                var fileExt = Path.GetExtension(file.FileName);
                return fileExt.Contains (".jpg") ||
                        fileExt.Contains(".png") ||
                        fileExt.Contains(".gif") ||
                        fileExt.Contains(".tiff") ||
                        fileExt.Contains(".bmp") ||
                        fileExt.Contains(".jpeg") ||
                        fileExt.Contains(".doc") ||
                        fileExt.Contains(".docx") ||
                        fileExt.Contains(".pdf") ||
                        fileExt.Contains(".txt") ||
                        fileExt.Contains(".xls") ||
                        fileExt.Contains(".xlsx")||
                        fileExt.Contains(".rtf");
                }
            catch
            {
                return false;
            }
        }

        public static string DisplayImage(string filePath)
        {
            var fileName = filePath;
            switch (Path.GetExtension(filePath))
            {
                case ".doc":
                    fileName = "/Images/File Types/doc.png";
                    break;
                case ".docx":
                    fileName = "/Images/File Types/docx.png";
                    break;
                case ".pdf":
                    fileName = "/Images/File Types/pdf.png";
                    break;
                case ".rtf":
                    fileName = "/Images/File Types/rtf.png";
                    break;
                case ".txt":
                    fileName = "/Images/File Types/txt.png";
                    break;
                case ".xls":
                    fileName = "/Images/File Types/xls.png";
                    break;
                case ".xlsx":
                    fileName = "/Images/File Types/xlsx.png";
                    break;
                default:
                    break;
            }
            return fileName;

        }
    }
}