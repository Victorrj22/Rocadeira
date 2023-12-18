using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstudoAutomacaoLote.Utilities;

namespace EstudoAutomacaoLote.Entities.BetNacional
{
    class BetNacionalBatchProcess
    {
        public static void ProcessBatch()
        {
            List<string> filesNames = Folder.GetFilesNamesWithoutExtension(BetNacionalFolderManager.sourcePath);

            foreach (string file in filesNames)
            {
                BetNacionalFolderManager.CreateFolders(file);
            }
        }

    }
}