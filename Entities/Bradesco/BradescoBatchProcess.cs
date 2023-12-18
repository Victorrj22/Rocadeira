using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstudoAutomacaoLote.Utilities;

namespace EstudoAutomacaoLote.Entities.Bradesco
{
    class BradescoBatchProcess
    {
        const string projectPath = @"C:\Users\JoãoVictorSoaresJord\Desktop\Roçadeira\Rocadeira\";
        const string sourcePath = @"C:\Users\JoãoVictorSoaresJord\OneDrive - Exato Digital\Development\Clientes\Bradesco\RH\Lotes\Modelo\";
        const string newBatchPath = @"C:\Users\JoãoVictorSoaresJord\OneDrive - Exato Digital\Development\Clientes\Bradesco\RH\Lotes";
        public static void ProcessBatch()
        {
            List<string> filesNames = Folder.GetFilesNamesWithoutExtension(sourcePath);

            foreach (string fileName in filesNames)
            {

                Tuple<string, string, string> foldersTuple = MainProcess.CreateFolders(fileName, newBatchPath);
                
                MainProcess.ManipulateFiles(fileName, foldersTuple.Item1, newBatchPath, sourcePath, foldersTuple.Item2);

                MainProcess.WriteCSV(fileName, foldersTuple.Item2);
                //MainProcess.EditingWorksheet(fileName, foldersTuple.Item2);

                //Reader.ReaderFile(sourcePath);
            }
        }

    }
}