using ClosedXML.Excel;
using EstudoAutomacaoLote.Utilities;

namespace EstudoAutomacaoLote.Entities.Randstad
{
    public class RandstadFolderManager : IFolderManager
    {
        const string projectPath = @"C:\Users\LucasHergessel\source\repos\EstudoAutomacaoLote\EstudoAutomacaoLote\Files";
        public const string sourcePath = @"C:\Users\LucasHergessel\Exato Digital\Exato Digital - Development\Clientes\Randstad\Lotes\Entrada\Modelo\";
        const string newBatchPath = @"C:\Users\LucasHergessel\Exato Digital\Exato Digital - Development\Clientes\Randstad\Lotes\Entrada";

        public void CreateFolders()
        {

            Console.WriteLine("Insira agora o nome do lote:");
            string newBatchName = Console.ReadLine();

            string futureBatchFolder = Path.Combine(newBatchPath, newBatchName);

            if (!Directory.Exists(futureBatchFolder))
            {
                Console.WriteLine("Este lote ainda não existe. Criando pastas para o lote.");

                Directory.CreateDirectory(futureBatchFolder);
                Directory.CreateDirectory(futureBatchFolder + @"\Entrada");
                Directory.CreateDirectory(futureBatchFolder + @"\Saída");


                Console.WriteLine("Coletando nome da planilha original.");

                string fileName = "";
                string[] files = Directory.GetFiles(sourcePath);
                foreach (string file in files)
                {
                    fileName = Path.GetFileNameWithoutExtension(file);
                }

                Console.WriteLine("este é o nome do arquivo: " + fileName);
                Console.WriteLine("Criando nova planilha em branco.");

                using var wbook = new XLWorkbook();
                var ws = wbook.Worksheets.Add("Planilha 1");
                wbook.SaveAs(futureBatchFolder + $@"\Entrada\{fileName} copy.xlsx");


                Console.WriteLine("Movendo a planilha original.");

                File.Move(sourcePath + fileName + ".xlsx", futureBatchFolder + $@"\Entrada\{fileName}.xlsx");

            }
            else throw new Exception("Este lote já existe. Tente com outro nome.");
        }
    }
}
