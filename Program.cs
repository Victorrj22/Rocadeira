using Rocadeira.Entities.BetNacional;
using Rocadeira.Entities.Bradesco;
using Rocadeira.Entities.EstrelaBet;
using Rocadeira.Entities.Randstad;
using Rocadeira.Enums;
using Rocadeira.Interface;
using Rocadeira.Utilities;
using Rocadeira;

namespace Rocadeira
{
    class Program 
    {
        static async Task Main(string[] args)
        {
            var companyFolderCreators = new Dictionary<CompaniesEnum, IFolderManager>
            {
                { CompaniesEnum.Bradesco, new BradescoFolderManager() },
                { CompaniesEnum.EstrelaBet, new EstrelaBetFolderManager() },
                { CompaniesEnum.BetNacional, new BetNacionalFolderManager() },
                { CompaniesEnum.Randstad, new RandstadFolderManager() }
            };

            int opId = MainProcess.ReadOperationId();

            switch (opId)
            {
                case 1:
                    BradescoBatchProcess.ProcessBatch();
                    break;
                case 2:
                    BetNacionalBatchProcess.ProcessBatch();
                    break;
                case 3:
                    Folder.GetFilesNamesWithoutExtension(BetNacionalFolderManager.sourcePath);
                    break;
                case 4:
                    Folder.GetFilesNamesWithoutExtension(RandstadFolderManager.sourcePath);
                    break;
                default:
                    Console.WriteLine("Invalid operation ID");
                    break;
            }

            static async Task ExecuteAsyncTasks(string fileName)
            {
                var query = new ConsultaContextExtensions();
                var tokenAcesso = await query.CreateToken("Bradesco RH Lote " + fileName);
                await query.InsertQuery(tokenAcesso.TokenAcessoId, "lote_20231207");
            }
            
            await ExecuteAsyncTasks(MainProcess.FileName);
            
            
            
        }

        
    }
}