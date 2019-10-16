using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace DocsFileManagerMVC.Models
{
    public class DocsModel
    {
        public string folderPath;

        public DocsModel(string folderPath)
        {
            this.folderPath = folderPath;
        }

        public async Task UploadFile(IFormFile file, string relativeFolderPath)
        {
            using (var stream = new FileStream(folderPath + relativeFolderPath + file.FileName, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }

        public List<IObjInDir> GetElementsInFolder(string relativePath = "")
        {
            string[] folders = Directory.GetDirectories(folderPath + relativePath);
            string[] files = Directory.GetFiles(folderPath + relativePath, "*.*");
            var list = new List<IObjInDir> { };

            foreach (var folderPath in folders)
            {
                var folder = new Folder(folderPath);
                list.Add(folder);
            }

            foreach (var filePath in files)
            {
                var file = new DocFile(filePath);
                list.Add(file);
            }

            return list;
        }
    }
}
