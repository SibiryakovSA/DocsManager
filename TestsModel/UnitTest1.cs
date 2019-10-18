using DocsFileManagerMVC.Models;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace TestsModel
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            DocsModel docsModel = new DocsModel(@"C:\Users\User\source\repos\DocsManager\DocsFileManagerMVC\bin\Debug\netcoreapp2.1\Files\");
            //Console.WriteLine("Количество файлов: " + docsModel.GetElementsInFolder().Count);
            //foreach (var folder in (from f in docsModel.GetElementsInFolder() where f is Folder select f as Folder))
            //{
            //    Console.WriteLine(folder.Name);
            //    Console.WriteLine(folder.RelativeFolderPath == null ? "null" : docsModel.folderPath + folder.RelativeFolderPath);
            //}
            //Assert.Pass();

            var relativeFolderPath = "";
            var folderName = "someName";
            Directory.CreateDirectory(relativeFolderPath + folderName);
        }
    }
}