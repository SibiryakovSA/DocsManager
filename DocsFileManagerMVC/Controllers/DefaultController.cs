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
        DocsModel docsModel = new DocsModel(AppContext.BaseDirectory + "Files\\");

        public IActionResult Index()
        {
            TempData["relativeFolderPath"] = "";
            return View("Default", docsModel.GetElementsInFolder());
        }


        [HttpPost("CreateFolder")]
        public IActionResult CreateFolder(string folderName, string relativeFolderPath = "", string description = "")
        {
            relativeFolderPath = TempData["relativeFolderPath"] == null ? "" : TempData["relativeFolderPath"].ToString();

            //var rfp = relativeFolderPath == "" ? "" : Uri.UnescapeDataString(relativeFolderPath);
            Directory.CreateDirectory(docsModel.folderPath + relativeFolderPath + folderName);
            System.IO.File.WriteAllText(docsModel.folderPath + relativeFolderPath + folderName + ".descr", description);
            //var redirectRelativePath = rfp == "" ? "" : "/default/ViewFolder?relativeFolderPath=" + relativeFolderPath;
            //return Redirect(string.Format("{0}://{1}" + redirectRelativePath, Request.Scheme, Request.Host));
            TempData["relativeFolderPath"] = relativeFolderPath;
            return PartialView("~/views/AllElementsPart.cshtml", docsModel.GetElementsInFolder(relativeFolderPath));
        }

        [HttpPost("DeleteFile")]
        public IActionResult Delete(string fileNameExtention, string relativeFolderPath)
        {
            relativeFolderPath = TempData["relativeFolderPath"] == null ? "" : TempData["relativeFolderPath"].ToString();
            //var rfp = relativeFolderPath == null ? "" : Uri.UnescapeDataString(relativeFolderPath);

            if (fileNameExtention != "")
            {
                var obj = (from f in docsModel.GetElementsInFolder(relativeFolderPath)
                           where (f.Name + "." + f.Extention == fileNameExtention && f.RelativeFolderPath == relativeFolderPath)
                           select f).FirstOrDefault();

                if (obj != null)
                {
                    obj.Delete();
                    //return View("Default", docsModel.GetElementsInFolder(obj.RelativeFolderPath));
                }
                // Ok(new { file = obj.RelativeFolderPath + obj.Name, rfp = relativeFolderPath });
            }

            //return View("Default", docsModel.GetElementsInFolder());

            //var redirectRelativePath = rfp == "" ? "" : "/default/ViewFolder?relativeFolderPath=" + relativeFolderPath;
            //return Redirect(string.Format("{0}://{1}" + redirectRelativePath, Request.Scheme, Request.Host));
            //TempData["relativeFolderPath"] = relativeFolderPath;
            //return PartialView("~/views/ttest.cshtml", docsModel.GetElementsInFolder(relativeFolderPath));
            TempData["relativeFolderPath"] = relativeFolderPath;
            return PartialView("~/views/AllElementsPart.cshtml", docsModel.GetElementsInFolder(relativeFolderPath));
        }

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Post(List<IFormFile> files, string description = "")
        {
            var relativeFolderPath = TempData["relativeFolderPath"] == null ? "" : TempData["relativeFolderPath"].ToString();
            foreach (var formFile in files)
            {
                if (formFile.FileName.Split('.').Last() == "doc" || formFile.FileName.Split('.').Last() == "xls")
                {
                    await docsModel.UploadFile(formFile, relativeFolderPath == null ? "" : relativeFolderPath, description);
                }
            }
            TempData["relativeFolderPath"] = relativeFolderPath;
            return PartialView("~/views/AllElementsPart.cshtml", docsModel.GetElementsInFolder(relativeFolderPath));
            //var redirectRelativePath = relativeFolderPath == "" ? "" : "/default/ViewFolder?relativeFolderPath=" + Uri.EscapeDataString(relativeFolderPath);
            //return Redirect(string.Format("{0}://{1}" + redirectRelativePath, Request.Scheme, Request.Host)); //View("Default", docsModel.GetElementsInFolder(relativeFolderPath)); // Ok(new { count = files.Count, path = docsModel.folderPath + relativeFolderPath });
        }

        
        public IActionResult GetFile(string fileNameExtention, string relativeFolderPath = "")
        {
            var rfp = relativeFolderPath == null ? "" : Uri.UnescapeDataString(relativeFolderPath);
            if (fileNameExtention != "" && fileNameExtention != null)
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
            return NotFound(); //View("Default", docsModel.GetElementsInFolder()); // Redirect(string.Format("{0}://{1}", Request.Scheme, Request.Host));
        }

        public IActionResult ViewFolder(string relativeFolderPath)
        {
            TempData["relativeFolderPath"] = relativeFolderPath == null ? "" : Uri.UnescapeDataString(relativeFolderPath);
            return PartialView("~/views/AllElementsPart.cshtml", docsModel.GetElementsInFolder(relativeFolderPath == null ? "" : Uri.UnescapeDataString(relativeFolderPath)));
            //return View("Default", docsModel.GetElementsInFolder(relativeFolderPath == null ? "" : Uri.UnescapeDataString(relativeFolderPath)));
        }
    }
}