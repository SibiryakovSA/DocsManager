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

        public async Task UploadFile(IFormFile file, string relativeFolderPath, string description = "")
        {
            using (var stream = new FileStream(folderPath + relativeFolderPath + file.FileName, FileMode.Create))
            {
                File.WriteAllText(folderPath + relativeFolderPath + file.FileName.Remove(file.FileName.LastIndexOf('.')) + file.FileName.Split('.').Last() + ".descr", description);
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
                var descrList = Directory.GetFiles(folder.Path.Remove(folder.Path.LastIndexOf(folder.Name)), folder.Name + ".descr");
                if (descrList.Length == 1)
                {
                    var descr = File.ReadAllText(descrList[0]);
                    folder.SetDescription(descr);
                }
                list.Add(folder);
            }

            foreach (var filePath in files)
            {
                var file = new DocFile(filePath);
                if (file.Extention == "doc" || file.Extention == "xls")
                {
                    var descrList = Directory.GetFiles(file.Path.Remove(file.Path.LastIndexOf(file.Name + "." + file.Extention)), file.Name + file.Extention + ".descr");
                    if (descrList.Length == 1)
                    {
                        var descr = File.ReadAllText(descrList[0]);
                        file.SetDescription(descr);
                    }
                    list.Add(file);
                }
            }

            return list;
        }
    }
}
