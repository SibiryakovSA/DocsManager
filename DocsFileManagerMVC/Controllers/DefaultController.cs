using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocsFileManagerMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocsFileManagerMVC.Controllers
{
    public class DefaultController : Controller
    {
        DocsModel docsModel = new DocsModel(@"C:\Users\User\source\repos\DocsFileManagerMVC\DocsFileManagerMVC\bin\Debug\netcoreapp2.1\Files\");

        public IActionResult Index()
        {
            return View("Default", docsModel);
        }

        [HttpPost("DeleteFile")]
        public async Task<IActionResult> Delete(int id, string relativeFolderPath)
        {
            await Task.Run( () => { docsModel.Remove(docsModel.allFiles[id]); });
            return View("Default", new DocsModel(docsModel.folderPath + relativeFolderPath + "\\"));
        }

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Post(List<IFormFile> files, string relativeFolderPath)
        {
            foreach (var formFile in files)
            {
                if (formFile.FileName.Split('.').Last() == "doc" || formFile.FileName.Split('.').Last() == "xls")
                {

                    await docsModel.UploadFile(formFile, (relativeFolderPath == "" ? "" : relativeFolderPath + "\\"));
                }
            }
            return View("Default", new DocsModel(docsModel.folderPath + relativeFolderPath + "\\"));// Ok(new { count = files.Count, path = docsModel.folderPath + relativeFolderPath });
        }

        public IActionResult GetFile(int id)
        {
            if (id < docsModel.allFiles.Count)
            {
                string filePath = docsModel.allFiles[id].Path;
                string fileType = "application/" + docsModel.allFiles[id].Extention;
                string fileName = docsModel.allFiles[id].Name + "." + docsModel.allFiles[id].Extention;
                return PhysicalFile(filePath, fileType, fileName);
            }
            return View("Default", docsModel); // Redirect(string.Format("{0}://{1}", Request.Scheme, Request.Host));
        }

        public IActionResult ViewFolder(string relativeFolderPath)
        {
            return View("Default", new DocsModel(docsModel.folderPath + relativeFolderPath + "\\", docsModel.allFiles));
        }
    }
}