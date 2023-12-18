using ClosedXML.Excel;
using System;
using DocumentFormat.OpenXml.Wordprocessing;
using EstudoAutomacaoLote;

public class Reader
{
    public static List<Person> ReaderFile(string folderEntrada, string fileName)
    {
        List<Person> lista = new List<Person>(); 

        var workbook = new XLWorkbook(@$"{folderEntrada}\{fileName}.xlsx");
        
        var nonEmptyDataRows = workbook.Worksheet(1).RowsUsed(); // obtem apenas as linhas que foram utilizadas da planilha

        foreach (var dataRow in nonEmptyDataRows) // percorremos linha a linha da planilha
        {
            if (dataRow.RowNumber() > 1) //obteremos apenas após a linha 1 para não carregar o cabeçalho
            {
                var person = new Person(); // criamos um objeto para popular com os valores obtidos da linha

                person.analista = dataRow.Cell(1).Value.ToString(); // obtemos o valor de cada célula pelo seu nº de coluna
                person.nome = dataRow.Cell(2).Value.ToString();
                person.cpf= dataRow.Cell(3).Value.ToString();
                DateTime.TryParse(dataRow.Cell(4).Value.ToString(), out DateTime datanasct);
                person.nascimento = datanasct;
                person.email = dataRow.Cell(5).Value.ToString();

                lista.Add(person); // adicionamos o objeto criado à lista
            }
        }
        return lista;
    }

}