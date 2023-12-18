using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using EstudoAutomacaoLote.Enums;

namespace EstudoAutomacaoLote.Utilities
{
    public interface IFolderManager
    {
        void CreateFolders() { }
        void InsertBatch() {}
    }
}