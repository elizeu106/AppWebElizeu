using System.Diagnostics;
using appwebElizeu.Configs;
using appwebElizeu.Model;
using System;
using MySql.Data.MySqlClient;

namespace appwebElizeu.DAO
{
    public class ProcessoDAO
    {
        public static DateOnly? GetDateOnly(MySqlDataReader reader, string columnName)
        {
            DateOnly? value = null;
            if (!reader.IsDBNull(reader.GetOrdinal(columnName)))
                value = DateOnly.FromDateTime(reader.GetDateTime(columnName));

            return value;
        }

        private readonly Conexao _conexao;

        public ProcessoDAO(Conexao conexao)
        {
            _conexao = conexao;
        }

        public List<Processos> Listar()
        {
            var lista = new List<Processos>();

            var comando = _conexao.CreateCommand(
                "SELECT * FROM processos;"
            );

            var leitor =
                (MySqlDataReader)comando.ExecuteReader();

            while (leitor.Read())
            {
                lista.Add(MapearProcesso(leitor));
            }

            return lista;
        }

        private static Processos MapearProcesso(
            MySqlDataReader leitor)
        {
            return new Processos
            {
                Id = leitor.GetInt32("id_pro"),

                Numero = DAOHelper.GetString(
                    leitor,
                    "numero_pro"
                ),

                Data = GetDateOnly(
                    leitor,
                    "data_pro"
                ),

                Interessado = DAOHelper.GetString(
                    leitor,
                    "interessado_pro"
                ),

                Assunto = DAOHelper.GetString(
                    leitor,
                    "assunto_pro"
                ),

                Descricao = DAOHelper.GetString(
                    leitor,
                    "descricao_pro"
                ),

                Situacao = DAOHelper.GetString(
                    leitor,
                    "situacao_pro"
                )
            };
        }

        public void Inserir(Processos processo)
        {
            try
            {
                using var con = _conexao.GetConnection();

                string sql = @"INSERT INTO processos
                    (numero_pro, data_pro, interessado_pro,
                     assunto_pro, descricao_pro, situacao_pro)
                    VALUES
                    (@numero, @data, @interessado,
                     @assunto, @descricao, @situacao)";

                using var comando = con.CreateCommand();

                comando.CommandText = sql;

                comando.Parameters.AddWithValue("@numero", processo.Numero);
                comando.Parameters.AddWithValue("@data", processo.Data!.Value.ToDateTime(TimeOnly.MinValue));
                comando.Parameters.AddWithValue("@interessado", processo.Interessado);
                comando.Parameters.AddWithValue("@assunto", processo.Assunto);
                comando.Parameters.AddWithValue("@descricao", processo.Descricao);
                comando.Parameters.AddWithValue("@situacao", processo.Situacao);

                comando.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
        }
    }
}