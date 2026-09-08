using appwebElizeu.Configs;
using appwebElizeu.Model;
using MySql.Data.MySqlClient;

namespace appwebElizeu.DAO
{
    public class ProcessoDAO
    {
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

                Data = DAOHelper.GetDateTime(
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
    }
}
