using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstudoAutomacaoLote.Utilities
{
    class Folder
    {
        public static List<string> GetFilesNamesWithoutExtension(string sourcePath)
        {
            List<string> filesName = new List<string> { };
            List<string> files = Directory.GetFiles(sourcePath).ToList();
            foreach (string file in files)
            {
                filesName.Add(Path.GetFileNameWithoutExtension(file));
            }
            return filesName;
        }

        public static int GetQuantityOfFiles(string sourcePath)
        {
            int quantity = 0;
            List<string> files = Directory.GetFiles(sourcePath).ToList();
            foreach (string file in files)
            {
                quantity ++;
            }
            return quantity;
        }
        
        public static List<string> GetFilesNames(string sourcePath)
        {
            List<string> filesName = new List<string> { };
            List<string> files = Directory.GetFiles(sourcePath).ToList();
            foreach (string file in files)
            {
                filesName.Add(Path.GetFileName(file));
            }
            return filesName;
        }
    }
}