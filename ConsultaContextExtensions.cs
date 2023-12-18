using Rocadeira;
using Npgsql;

public class ConsultaContextExtensions
{
    private NpgsqlDataSource _dataSource =
        NpgsqlDataSource.Create("Host=localhost;Port=5430;Database=rocadeira;Username=postgres;Password=password");

    public async Task<TokenAcesso> CreateToken(string tokenName)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();

        var query =
            "INSERT INTO public.token_acesso (cliente_id, token, acesso_total, incluido_em, incluido_por, name, key_type) " +
            "VALUES (1631, translate(uuid_generate_v4()::TEXT, '{{}}-', ''), true, now(), 0, @tokenName, 1)" +
            "RETURNING token_acesso_id;";

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("tokenName", tokenName);

        var tokenAcesso = new TokenAcesso();
        tokenAcesso.TokenAcessoId = (int)await command.ExecuteScalarAsync();

        return tokenAcesso;
    }

    public async Task InsertQuery(int token, string tabela)
    {
        await using var connection = await _dataSource.OpenConnectionAsync();
        var query = @"
    INSERT INTO public.consulta
    (consulta_tipo_id,
    token_acesso_id,
    origem_id,
    uid_base36,
    ip_remoto,
    acesso_negado,
    inicio,
    chave,
    document,
    entrada,
    faturavel,
    armazenar_pdf,
    custo_cred,
    custo_cred_pdf_geracao,
    custo_cred_pdf_armaz,
    custo_cred_total_inc_subcons,
    options,
    hostname,
    input_params,
    remote_ip,
    async,
    async_run_persistent,
    async_attempts)
    SELECT ct.consulta_tipo_id,
        ta.token_acesso_id,
        2                                                                          origem_id,
        doc_normalizado
            || 'B'
            || TO_CHAR(CLOCK_TIMESTAMP(), 'MMDDHH'),
        2130706433                                                                 ip_remoto,
        FALSE                                                                      acesso_negado,
        CLOCK_TIMESTAMP() AT TIME ZONE 'America/Fortaleza'                         inicio,
        doc_normalizado                                                            chave,
        doc_normalizado AS                                                         ""document"",
        CASE
            WHEN nascimento IS NULL THEN 'Cpf:' || doc_normalizado
            ELSE 'Cpf:' || doc_normalizado || ';Nascimento:' || TO_CHAR(nascimento, 'YYYY-MM-DD')
        END                                                                    entrada,
        FALSE                                                                      faturavel,
        FALSE           AS                                                         armazenar_pdf,
        0               AS                                                         custo_cred,
        0               AS                                                         custo_cred_pdf_geracao,
        0               AS                                                         custo_cred_pdf_armaz,
        0               AS                                                         custo_cred_total_inc_subcons,
        '{ ""ExecutionMode"": 1, ""Async"": true, ""AsyncRunPersistent"": true, ""PdfGenerationMode"": 1, ""PdfGenerationSync"": true, ""StorePdf"": false, ""Timeout"": ""48:00:00"", ""CacheMaxAge"": ""60.00:00:00"", ""CacheIncludeNotFound"": null }'::jsonb       AS                                                         options,
        'ROG-LEANDRO'                                                              hostname,
        ('{ ' ||
            '""cpf"": { ""numero"": ' || doc_normalizado::BIGINT || ' }, ' ||
            '""full_name"": ""' || COALESCE(nome_completo, '') || '"", ' ||
            '""emails"": ' || CASE WHEN emails IS NULL THEN 'null' ELSE ' [ ""' || emails || '"" ]' END || ', ' ||
            '""customer_external_id"": ""' || c.external_id || '"", ' ||
            '""credit_analysis"": true, ' ||
            '""birth_date"": ' || CASE WHEN nascimento IS NULL THEN 'null' ELSE '{ ""date"": ""' || TO_CHAR(nascimento, 'YYYY-MM-DD') || '"" }' END || ', ' ||
            '""position_title"": ""' || '' || '"" ' ||
        '}')::JSONB                                                               input_params,
        2130706433                                                                 remote_ip,
        TRUE                                                                       async,
        TRUE                                                                       async_run_persistent,
        0                                                                          async_attempts
    FROM (SELECT *,
                RIGHT(LPAD(doc::BIGINT::TEXT, 11, '0'), 11) doc_normalizado
            FROM (
                    SELECT DISTINCT
                        ON (cpf) linha,
                                NULLIF(LTRIM(REGEXP_REPLACE(TRIM(cpf::TEXT), '[^0-9]', '', 'g'), '0'), '')::BIGINT doc,
                                CASE
                                    WHEN TRIM(TRANSLATE(TRANSLATE(nascimento, e' \r\n', ''), '|.-', '///')) ~* '^(\d{2}\/\d{2}\/\d{4})$'
                                        THEN TO_DATE(TRIM(TRANSLATE(TRANSLATE(nascimento, e' \r\n', ''), '|.-', '///')), 'MM/DD/YYYY')
                                END::date                                                                      nascimento,
                                TRIM(REPLACE(TRANSLATE(nome, e'\r\n', ''), ' ', ' '))                              nome_completo,
                                TRIM(REPLACE(TRANSLATE(email, e'\r\n', ''), ' ', ' '))                             emails,
                                NULL                                                                                    email,
                            @token                                                      token_acesso_id
                    FROM customer_bradesco_rh_jobs." + tabela + @"
                    WHERE LENGTH(NULLIF(LTRIM(REGEXP_REPLACE(TRIM(cpf::TEXT), '[^0-9]', '', 'g'), '0'), '')) <= 14
                    --
                ) t1
            WHERE COALESCE(doc, '0')::BIGINT != 0) docs
                INNER JOIN public.token_acesso ta
                            ON docs.token_acesso_id = ta.token_acesso_id
                INNER JOIN public.cliente c
                            ON c.cliente_id = ta.cliente_id
                INNER JOIN public.consulta_tipo ct
                            ON ct.consulta_tipo_id = 30200
    WHERE NOT EXISTS(
            SELECT 1
            FROM public.consulta c
                    INNER JOIN public.token_acesso t
                                ON c.token_acesso_id = t.token_acesso_id
            WHERE c.document = doc_normalizado
                AND t.cliente_id = ta.cliente_id
                AND t.token_acesso_id = ta.token_acesso_id
                AND c.consulta_tipo_id = ct.consulta_tipo_id
                AND c.consulta_master_id IS NULL
                AND (c.fim IS NULL OR
                    (c.fim IS NOT NULL AND c.consulta_resultado_tipo_id IN (1, 2, 3, 5, 6, 7)))

                AND inicio >= (NOW() - '7 day'::INTERVAL)
                AND (c.consulta_resultado_tipo_id NOT IN (1, 2))
                AND 1 = 1)

    ORDER BY linha DESC
    LIMIT 5000;
";
        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("token", token);
        command.Parameters.AddWithValue("tabela", tabela);
        await command.ExecuteNonQueryAsync();
    }
}