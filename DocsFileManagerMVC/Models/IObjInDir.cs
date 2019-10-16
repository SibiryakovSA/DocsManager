using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocsFileManagerMVC.Models
{
    public interface IObjInDir
    {
        string Name { get; }
        string Path { get; }
        string Extention { get; }
        string RelativeFolderPath { get; }
        void Delete();
    }
}
