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
        DocsModel docsModel = new DocsModel(@"C:\Users\User\source\repos\DocsManager\DocsFileManagerMVC\bin\Debug\netcoreapp2.1\Files\");

        public IActionResult Index()
        {
            return View("Default", docsModel.GetElementsInFolder());
        }


        [HttpPost("CreateFolder")]
        public IActionResult CreateFolder(string folderName, string relativeFolderPath = "")
        {
            Directory.CreateDirectory(docsModel.folderPath + relativeFolderPath + folderName);

            var redirectRelativePath = relativeFolderPath == "" ? "" : "/default/ViewFolder?relativeFolderPath=" + relativeFolderPath;
            return Redirect(string.Format("{0}://{1}" + redirectRelativePath, Request.Scheme, Request.Host));
        }

        [HttpPost("DeleteFile")]
        public async Task<IActionResult> Delete(string fileNameExtention, string relativeFolderPath = "")
        {
            if (fileNameExtention != "")
            {
                var obj = (from f in docsModel.GetElementsInFolder(relativeFolderPath)
                           where (f.Name + "." + f.Extention == fileNameExtention && f.RelativeFolderPath == relativeFolderPath)
                           select f).FirstOrDefault();

                if (obj != null)
                {
                    await Task.Run(() => { obj.Delete(); });
                    //return View("Default", docsModel.GetElementsInFolder(obj.RelativeFolderPath));
                }
            }
            //return View("Default", docsModel.GetElementsInFolder());
            var redirectRelativePath = relativeFolderPath == "" ? "" : "/default/ViewFolder?relativeFolderPath=" + relativeFolderPath;
            return Redirect(string.Format("{0}://{1}" + redirectRelativePath, Request.Scheme, Request.Host));
        }

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Post(List<IFormFile> files, string relativeFolderPath)
        {
            foreach (var formFile in files)
            {
                if (formFile.FileName.Split('.').Last() == "doc" || formFile.FileName.Split('.').Last() == "xls")
                {

                    await docsModel.UploadFile(formFile, relativeFolderPath);
                }
            }
            var redirectRelativePath = relativeFolderPath == "" ? "" : "/default/ViewFolder?relativeFolderPath=" + relativeFolderPath;
            return Redirect(string.Format("{0}://{1}" + redirectRelativePath, Request.Scheme, Request.Host)); //View("Default", docsModel.GetElementsInFolder(relativeFolderPath)); // Ok(new { count = files.Count, path = docsModel.folderPath + relativeFolderPath });
        } //проблема в редиректе кириллицы (в загрузке файлов в папку с русскими символами)

        public IActionResult GetFile(string fileNameExtention, string relativeFolderPath = "")
        {
            if (fileNameExtention != "")
            {
                var file = (from f in docsModel.GetElementsInFolder(relativeFolderPath)
                            where (f.Name + "." + f.Extention == fileNameExtention && f.RelativeFolderPath == relativeFolderPath)
                            select f).FirstOrDefault() as DocFile;

                if (file != null)
                {
                    string filePath = file.Path;
                    string fileType = "application/" + file.Extention;
                    string fileName2 = file.Name + "." + file.Extention;
                    return PhysicalFile(filePath, fileType, fileName2);
                }
            }
            return View("Default", docsModel.GetElementsInFolder(relativeFolderPath)); // Redirect(string.Format("{0}://{1}", Request.Scheme, Request.Host));
        }

        public IActionResult ViewFolder(string relativeFolderPath)
        {
            return View("Default", docsModel.GetElementsInFolder(relativeFolderPath));
        }
    }
}