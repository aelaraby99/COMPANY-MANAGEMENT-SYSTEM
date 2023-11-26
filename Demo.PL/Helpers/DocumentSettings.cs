using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Demo.PL.Helpers
{
    public class DocumentSettings
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            //1. Get Located Folder Path
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName);
            //2. Get FileName and make it unique using Guid
            string fileName = $"{Guid.NewGuid()}{file.FileName}";
            //3. Get File Path (FolderPath + FileName)
            string filePath = Path.Combine(folderPath, fileName);
            //4. Save file using Streaming (Data Per Time)
            using var fileStream = new FileStream(filePath, FileMode.CreateNew);
            file.CopyTo(fileStream);
            return fileName;
        }
        public static void DeleteFile(string fileName, string folderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName, fileName);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
