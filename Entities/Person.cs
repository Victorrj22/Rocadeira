using System.ComponentModel;

namespace EstudoAutomacaoLote;

public class Person
{
    // Dados de entrada

    [Description("LINHA")]
    public string linha { get; set; }

    [Description("ANALISTA")]
    public string analista { get; set; }
    
    [Description("NOME")]
    public string nome { get; set; }
    
    [Description("CPF")]
    public string cpf { get; set; }

    [Description("NASCIMENTO")]
    public DateTime nascimento { get; set; }

    [Description("EMAIL")]
    public string email { get; set; }
    /*
    // Resultados das consultas
    public string indicador { get; set; }
    public string consulta_situacao { get; set; }
    public string consulta_mensagem { get; set; }
    public string nome_entrada_normalizado { get; set; }
    public string nome_receita_federal { get; set; }
    public string receita_federal_data_nascimento { get; set; }
    public string idade_entrada { get; set; }
    public string receita_federal_idade { get; set; }
    public string receita_federal_consta_obito { get; set; }
    public string receita_federal_situacao_cadastral { get; set; }
    public string receita_federal_possivel_outro_cpf { get; set; }
    public string policia_federal_antecedentes_criminais { get; set; }
    public string tst_certidao_negativa_constam_debitos { get; set; }
    public string mte_certidao_negativa_constam_debitos { get; set; }
    public string cgu_pep_principal { get; set; }
    public string cgu_pep_relacionados { get; set; }
    public string sociedades_ativas { get; set; }
    public string quod_total_dividas_nao_imob { get; set; }
    public string indicador_motivos { get; set; }
    public string processos_como_requerente_resumo { get; set; }
    public string processos_como_requerido_resumo { get; set; }

*/
};