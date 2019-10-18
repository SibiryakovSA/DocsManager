using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DocsFileManagerMVC.Models
{
    public class Folder : IObjInDir
    {
        public string Name { get; private set; }
        public string Path { get; private set; }
        public string RelativeFolderPath { get; private set; }
        public string Extention => "";
        public string Description { get; private set; }

        public void Delete()
        {
            Directory.Delete(Path, true);
        }

        public Folder(string path, string description = "")
        {
            Path = path;
            Name = path.Split('\\').Last();
            RelativeFolderPath = path.Split("Files\\").Last().Replace(Name, ""); //перепиши через indexOf, то же для DocFile
            Description = description;
        }
    }
}
