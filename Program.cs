using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EstudoAutomacaoLote.Entities.BetNacional;
using EstudoAutomacaoLote.Entities.Bradesco;
using EstudoAutomacaoLote.Entities.EstrelaBet;
using EstudoAutomacaoLote.Entities.Randstad;
using EstudoAutomacaoLote.Enums;
using EstudoAutomacaoLote.Utilities;

namespace EstudoAutomacaoLote
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

            static async Task ExecuteAsyncTasks()
            {
                var query = new ConsultaContextExtensions();
                await query.CreateToken("Bradesco RH Lote 2023-12-07 (114)");
                await query.InsertQuery(1, "lote_20231207");
            }
            
            await ExecuteAsyncTasks();
            
            
            
        }

        
    }
}