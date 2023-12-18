using ClosedXML.Excel;
using Rocadeira.Interface;

namespace Rocadeira.Entities.BetNacional
{
    public class BetNacionalFolderManager : IFolderManager
    {
        const string projectPath = @"C:\Users\LucasHergessel\source\repos\EstudoAutomacaoLote\EstudoAutomacaoLote\Files";
        public const string sourcePath = @"C:\Users\LucasHergessel\Exato Digital\Exato Digital - Development\Clientes\BetNacional\Lotes\Modelo\";
        const string newBatchPath = @"C:\Users\LucasHergessel\Exato Digital\Exato Digital - Development\Clientes\BetNacional\Lotes";

        public static void CreateFolders(string fileName)
        {
            Console.WriteLine($"Este é o nome do arquivo atual: {fileName}");

            Console.WriteLine("Insira agora o nome do lote:");
            string newBatchName = Console.ReadLine();

            string futureBatchFolder = Path.Combine(newBatchPath, newBatchName);

            if (!Directory.Exists(futureBatchFolder))
            {
                Console.WriteLine("Este lote ainda não existe. Criando pastas para o lote.");

                Directory.CreateDirectory(futureBatchFolder);
                Directory.CreateDirectory(futureBatchFolder + @"\Entrada");
                Directory.CreateDirectory(futureBatchFolder + @"\Saída");

                
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