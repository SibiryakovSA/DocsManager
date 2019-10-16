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
        public string RelativePath { get; private set; }
        public string Extention => throw new NotImplementedException();
        public string Description { get; private set; }

        public void Delete()
        {
            Directory.Delete(Path, true);
        }

        public Folder(string path, string description = "")
        {
            Path = path;
            RelativePath = path.Split("Files\\").Last();
            Name = path.Split('\\').Last();
            Description = description;
        }
    }
}
