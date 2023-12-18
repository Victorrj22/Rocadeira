using ClosedXML.Excel;
using CsvHelper;
using System.Globalization;
using System.Text;
using Rocadeira.Entities;
using Rocadeira.Utilities;

namespace Rocadeira
{
    class MainProcess
    {
        public static int ReadOperationId()
        {
            Console.WriteLine("Digite o número da operação de acordo com a empresa:");
            Console.WriteLine("1 - BradescoFolderManager");
            Console.WriteLine("2 - EstrelaBetFolderManager");
            Console.WriteLine("3 - BetNacionalFolderManager");
            Console.WriteLine("4 - Randstad");

            int opId = int.Parse(Console.ReadLine());
            return opId;
        }

        /// <summary>
        /// Cria as pastas do novo diretório de Lote
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="newBatchPath"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Tuple<string, string, string> CreateFolders(string fileName, string newBatchPath)
        {
            Console.WriteLine($"Este é o nome do arquivo atual: {fileName}");
            Console.WriteLine();

            Console.WriteLine("Insira agora o nome do lote:");
            string batchName = Console.ReadLine();

            string futureBatchFolder = Path.Combine(newBatchPath, batchName);
            string folderEntrada = "";
            string folderSaida = "";

            if (!Directory.Exists(futureBatchFolder))
            {
                Console.WriteLine("Este lote ainda não existe. Criando pastas para o lote.");

                Directory.CreateDirectory(futureBatchFolder);

                //regra que checa se é Bradesco para criar pasta específica
                if (newBatchPath ==
                    @"C:\Users\JoãoVictorSoaresJord\OneDrive - Exato Digital\Development\Clientes\Bradesco\RH\Lotes")
                {
                    folderEntrada = futureBatchFolder + @"\Entrada_Bradesco";
                    folderSaida = futureBatchFolder + @"\Saída_Bradesco";

                    Directory.CreateDirectory(folderEntrada);
                    Directory.CreateDirectory(folderSaida);
                }
                else
                {
                    folderEntrada = futureBatchFolder + @"\Entrada";
                    folderSaida = futureBatchFolder + @"\Saída";

                    Directory.CreateDirectory(futureBatchFolder + @"\Entrada");
                    Directory.CreateDirectory(futureBatchFolder + @"\Saída");
                }
            }
            else throw new Exception("Este lote já existe. Tente com outro nome.");

            return Tuple.Create(batchName, folderEntrada, folderSaida);
        }

        /// <summary>
        /// Move o documento para o novo diretório 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="batchName"></param>
        /// <param name="newBatchPath"></param>
        /// <param name="sourcePath"></param>
        /// <param name="folderEntrada"></param>
        public static void ManipulateFiles(string fileName, string batchName, string newBatchPath, string sourcePath, string folderEntrada)
        {
            File.Move(sourcePath + fileName + ".xlsx", folderEntrada + $@"\{fileName}.xlsx");

            Console.WriteLine($"Pastas criadas para o Lote {batchName} e arquivos movidos para a pasta Entrada.");
        }

        /// <summary>
        /// Cria nova planilha com os dados copiados da original
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="folderEntrada"></param>
        public static void EditingWorksheet(string fileName, string folderEntrada)
        {
            var originFileData = Reader.ReaderFile(folderEntrada, fileName);

            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Planilha 1");
            
            for (int i = 0; i < originFileData.Count(); i++)
            {
                PopulateLine(ws, i + 2, originFileData[i]);
            }

            WriteHeader(ws);
            PopulateColumnLinha(ws);

            string newFileName = folderEntrada + $@"\{fileName} copy.csv";

        }

        public static void WriteCSV(string fileName, string folderEntrada)
        {
            var originFileData = Reader.ReaderFile(folderEntrada, fileName);

            using (var writer = new StreamWriter(folderEntrada + @"\copy.csv",false, Encoding.UTF8))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<CsvMap>();
                csv.WriteHeader<Person>();
                csv.NextRecord();
                foreach (var data in originFileData)
                {
                    data.linha = "1";
                    csv.WriteRecord(data);
                    csv.NextRecord();
                }

                //csv.WriteRecords(originFileData);
            }
        }

        static void PopulateLine(IXLWorksheet ws, int line, Person person)
        {
            // dados de entrada
            ws.Cells($"A{line}").Value = person.linha;
            ws.Cells($"B{line}").Value = person.analista;
            ws.Cells($"C{line}").Value = person.nome;
            ws.Cells($"D{line}").Value = person.cpf;
            ws.Cells($"E{line}").Value = person.nascimento;
            ws.Cells($"F{line}").Value = person.email;
        }
        static void WriteHeader(IXLWorksheet ws)
        {
            ws.Cell("A1").Value = "linha";
            ws.Cell("B1").Value = "analista";
            ws.Cell("C1").Value = "nome";
            ws.Cell("D1").Value = "cpf";
            ws.Cell("E1").Value = "nascimento";
            ws.Cell("F1").Value = "email";
        }

        public static void PopulateColumnLinha(IXLWorksheet ws)
        {
            for (int i = 2; i < ws.LastRowUsed().RowNumber()+1; i++)
            {
                ws.Cell($"A{i}").Value = i-1;
            }
        }

        static string GetCSVFile(string path)
        {
            var csvFileName = Directory.GetFiles(path, "*.csv").FirstOrDefault();
            if (csvFileName == null)
                throw new Exception($"Nao foi encontrado nenhum arquivo CSV no diretorio {path}");

            var separator = char.Parse(@"\");
            csvFileName = csvFileName.Split(separator)[4];
            return csvFileName;
        }

        static string GetModelFile(string path)
        {
            var modelFile = Directory.GetFiles(path, "Modelo.xlsx").FirstOrDefault();

            if (modelFile == null)
                throw new Exception($"Nao foi possivel encontrar o arquivo Modelo.xlsx no diretorio {path}");

            return System.IO.Path.Combine(path, "Modelo.xlsx");
        }
    }
}


