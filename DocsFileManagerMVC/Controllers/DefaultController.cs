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
            return View("Default", docsModel.GetElementsInFolder());
        }

        [HttpPost("DeleteFile")]
        public async Task<IActionResult> Delete(IObjInDir obj)
        {
            await Task.Run( () => { obj.Delete(); });
            return View("Default", docsModel.GetElementsInFolder(obj.RelativeFolderPath));
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
            return View("Default", docsModel.GetElementsInFolder(relativeFolderPath)); // Ok(new { count = files.Count, path = docsModel.folderPath + relativeFolderPath });
        }

        public IActionResult GetFile(string fileName, string relativeFolderPath = "")
        {
            if (fileName != "")
            {
                var file = (from f in docsModel.GetElementsInFolder(relativeFolderPath)
                            where (f.Name == fileName && f.RelativeFolderPath == relativeFolderPath)
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
        } //разобраться почему: если в начальной папке нет файла, то он грузит не в выбранную папку, а в начальную

        public IActionResult ViewFolder(string relativeFolderPath)
        {
            return View("Default", docsModel.GetElementsInFolder(relativeFolderPath));
        }
    }
}