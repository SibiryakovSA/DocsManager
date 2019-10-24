using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DocsFileManagerMVC.Models
{
    public class DocFile : IObjInDir
    {
        public string Name { get; private set; }
        public string Extention { get; private set; }
        public string Path { get; private set; }
        public string Description { get; private set; }
        public string RelativeFolderPath { get; private set; }

        public void Delete()
        {
            File.Delete(Path);
            File.Delete(Path.Remove(Path.LastIndexOf("." + Extention)) + Extention + ".descr");
        }

        public DocFile(string name, string extention, string  description = "")
        {
            Name = name;
            Extention = extention;
            Path = @"/Files/" + Name + "." + Extention;
            Description = description;
        }

        public DocFile(string path, string description = "")
        {
            Path = path;
            Name = path.Split('\\').Last().Split('.')[0];
            Extention = path.Split('\\').Last().Split('.')[1];
            RelativeFolderPath = path.Split("Files\\").Last();
            RelativeFolderPath = RelativeFolderPath.Remove(RelativeFolderPath.LastIndexOf(Name + "." + Extention));
            Description = description;
        }

        public DocFile(string name, string extention, string path, string description = "")
        {
            Name = name;
            Extention = extention;
            Path = path;
            Description = description;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }
    }
}
