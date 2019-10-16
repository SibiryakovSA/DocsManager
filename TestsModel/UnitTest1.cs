using DocsFileManagerMVC.Models;
using NUnit.Framework;
using System;

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
            DocsModel docsModel = new DocsModel(@"C:\Users\User\source\repos\DocsFileManagerMVC\DocsFileManagerMVC\bin\Debug\netcoreapp2.1\Files\");
            Console.WriteLine("Количество файлов: " + docsModel.Refresh());
            foreach (var doc in docsModel.docFiles)
            {
                Console.WriteLine(doc.Name);
                Console.WriteLine(doc.Extention);
                Console.WriteLine(doc.Path + "\n");
            }
            //Assert.Pass();
        }
    }
}