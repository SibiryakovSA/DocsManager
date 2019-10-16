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
    public class DocsModel : PageModel
    {
        public List<DocFile> allFiles = new List<DocFile> { };
        public List<IObjInDir> docFiles = new List<IObjInDir> { };
        public string folderPath = @"Files/";

        public DocsModel()
        {
            Refresh();
        }
        public DocsModel(string folderPath)
        {
            this.folderPath = folderPath;
            Refresh();
        }

        public DocsModel(string folderPath, List<DocFile> allFiles)
        {
            this.allFiles = allFiles;
            this.folderPath = folderPath;
            Refresh();
        }

        public void Add(DocFile file)
        {
            docFiles.Add(file);
            allFiles.Add(file);
        }

        public void Remove(DocFile file)
        {
            docFiles.Remove(file);
            allFiles.Remove(allFiles[GetFileIdInAllFiles(file)]);
        }

        public async Task UploadFile(IFormFile file, string relativeFolderPath)
        {
            using (var stream = new FileStream(folderPath + relativeFolderPath + file.FileName, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            DocFile docFile = new DocFile(folderPath + file.FileName);
            Add(docFile);
        }

        public void Refresh()
        {
            string[] folders = Directory.GetDirectories(folderPath);
            string[] files = Directory.GetFiles(folderPath);

            foreach (var folder in folders)
            {
                Folder someFolder = new Folder(folder);
                docFiles.Add(someFolder);
            }

            foreach (var file in files)
            {
                if (file.Split('.').Last() == "doc" || file.Split('.').Last() == "xls")
                {
                    DocFile docFile = new DocFile(file);
                    docFiles.Add(docFile);
                }
            }

            if (allFiles.Count == 0)
            {
                string[] allFiles = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
                foreach (var file in allFiles)
                {
                    if (file.Split('.').Last() == "doc" || file.Split('.').Last() == "xls")
                    {
                        DocFile docFile = new DocFile(file);
                        this.allFiles.Add(docFile);
                    }
                }
            }
        }

        public int GetFileIdInAllFiles(DocFile docFile)
        {
            for (int i = 0; i < allFiles.Count; i++) {
                if (allFiles[i].Path == docFile.Path)
                    return i;
            }
            return -1;
        }
    }
}
