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
        public IActionResult CreateFolder(string folderName, string relativeFolderPath = "", string description = "")
        {
            var rfp = relativeFolderPath == "" ? "" : Uri.UnescapeDataString(relativeFolderPath);
            Directory.CreateDirectory(docsModel.folderPath + rfp + folderName);
            System.IO.File.WriteAllText(docsModel.folderPath + rfp + folderName + ".descr", description);
            var redirectRelativePath = rfp == "" ? "" : "/default/ViewFolder?relativeFolderPath=" + relativeFolderPath;
            return Redirect(string.Format("{0}://{1}" + redirectRelativePath, Request.Scheme, Request.Host));
        }

        [HttpPost("DeleteFile")]
        public async Task<IActionResult> Delete(string fileNameExtention, string relativeFolderPath = "")
        {
            var rfp = relativeFolderPath == null ? "" : Uri.UnescapeDataString(relativeFolderPath);
            if (fileNameExtention != "")
            {
                var obj = (from f in docsModel.GetElementsInFolder(rfp)
                           where (f.Name + "." + f.Extention == fileNameExtention && f.RelativeFolderPath == rfp)
                           select f).FirstOrDefault();

                if (obj != null)
                {
                    await Task.Run(() => { obj.Delete(); });
                    //return View("Default", docsModel.GetElementsInFolder(obj.RelativeFolderPath));
                }
            }
            //return View("Default", docsModel.GetElementsInFolder());
            var redirectRelativePath = rfp == "" ? "" : "/default/ViewFolder?relativeFolderPath=" + relativeFolderPath;
            return Redirect(string.Format("{0}://{1}" + redirectRelativePath, Request.Scheme, Request.Host));
        }

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Post(List<IFormFile> files, string relativeFolderPath, string description = "")
        {
            foreach (var formFile in files)
            {
                if (formFile.FileName.Split('.').Last() == "doc" || formFile.FileName.Split('.').Last() == "xls")
                {
                    await docsModel.UploadFile(formFile, relativeFolderPath == null ? "" : Uri.UnescapeDataString(relativeFolderPath), description);
                }
            }
            var redirectRelativePath = relativeFolderPath == "" ? "" : "/default/ViewFolder?relativeFolderPath=" + relativeFolderPath;
            return Redirect(string.Format("{0}://{1}" + redirectRelativePath, Request.Scheme, Request.Host)); //View("Default", docsModel.GetElementsInFolder(relativeFolderPath)); // Ok(new { count = files.Count, path = docsModel.folderPath + relativeFolderPath });
        }

        public IActionResult GetFile(string fileNameExtention, string relativeFolderPath = "")
        {
            var rfp = relativeFolderPath == null ? "" : Uri.UnescapeDataString(relativeFolderPath);
            if (fileNameExtention != "")
            {
                var file = (from f in docsModel.GetElementsInFolder(rfp)
                            where (f.Name + "." + f.Extention == fileNameExtention && f.RelativeFolderPath == rfp)
                            select f).FirstOrDefault() as DocFile;

                if (file != null)
                {
                    string filePath = file.Path;
                    string fileType = "application/" + file.Extention;
                    string fileName = file.Name + "." + file.Extention;
                    return PhysicalFile(filePath, fileType, fileName);
                }
            }
            return View("Default", docsModel.GetElementsInFolder(rfp)); // Redirect(string.Format("{0}://{1}", Request.Scheme, Request.Host));
        }

        public IActionResult ViewFolder(string relativeFolderPath)
        {
            return View("Default", docsModel.GetElementsInFolder(relativeFolderPath == null ? "" : Uri.UnescapeDataString(relativeFolderPath)));
        }
    }
}