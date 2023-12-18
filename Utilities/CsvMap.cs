using CsvHelper.Configuration;
using System.Globalization;
using Rocadeira.Entities;


namespace Rocadeira.Utilities
{
    public sealed class CsvMap : ClassMap<Person>
    {
        public CsvMap()
        {
            string format = "dd/MM/yyyy";
            var msMY = CultureInfo.GetCultureInfo("ms-MY");
            
            Map(m => m.linha);
            Map(m => m.analista);
            Map(m => m.nome);
            Map(m => m.cpf);
            Map(m => m.nascimento).TypeConverterOption.Format(format).TypeConverterOption.CultureInfo(msMY).Index(3);
            Map(m => m.email);
        }
    }

}