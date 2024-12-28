using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace projecto_dip_oficial
{
    internal class Conexao
    {
        public string conec = "SERVER=localhost; DATABASE=hause; UID=root; PWD=; PORT=;";
        public MySqlConnection con = null;


        //Abrir conexao
        public void abriConexao()
        {
            //Testar
            try
            {
                con = new MySqlConnection(conec);
                con.Open();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Erro do servidor", ex.Message);
            }

        }

        //Fechar conexao
        public void fecharConexao()
        {
            try
            {
                con = new MySqlConnection(conec);
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro do servidor", ex.Message);                
            }
        }
    }
}
