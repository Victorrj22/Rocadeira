using Rocadeira.Utilities;

namespace Rocadeira.Entities.BetNacional
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