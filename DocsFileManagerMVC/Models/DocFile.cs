using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DocsFileManagerMVC.Models
{
    public class DocFile : IObjInDir
    {
        public string Name { get; set; }
        public string Extention { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }

        public void Delete()
        {
            File.Delete(Path);
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
            Description = description;
        }

        public DocFile(string name, string extention, string path, string description = "")
        {
            Name = name;
            Extention = extention;
            Path = path;
            Description = description;
        }
    }
}
