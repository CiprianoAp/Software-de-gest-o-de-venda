using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projecto_dip_oficial.User
{
    public partial class FrmUsuario : Form
    {
        Conexao conectar = new Conexao();
        string Sql { get; set; }
        int userCodi {  get; set; }
        public FrmUsuario()
        {
            InitializeComponent();
            

        }

        private void FrmUsuario_Load(object sender, EventArgs e)
        {
            
            
        }

        //Pegar codigo do Cliente
        public void codigoUsuario()
        {
            conectar.abriConexao();
            Sql = "select max(CodigoCliente)  from cliente";
            MySqlCommand comando = new MySqlCommand(Sql, conectar.con);

            try
            {
                Int32 n = Convert.ToInt32(comando.ExecuteScalar());
                label3CodigoCliente.Text = n.ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void verificar(string nome, string nif, string endreco, string tel)
        {
            
            if (nome == "" || nif == "" || endreco == "" || tel == "")
            {
                MessageBox.Show("Nenhum campo deve ser vazio", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    conectar.abriConexao();
                    Sql = "INSERT INTO cliente (Nome,Nif,Enderoco,Tel) value('" + nome + "','" + nif + "','" + endreco + "','" + tel + "')";
                    MySqlCommand caixa = new MySqlCommand(Sql, conectar.con);
                    caixa.ExecuteNonQuery();
                    conectar.fecharConexao();
                    MessageBox.Show("Dados salvo com sucesso", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            verificarUserExiste();
            
        }


        //Verificar se o usuário existe
        public void verificarUserExiste()
        {
           

           
            try
            {
                if (textBox2Nif.Text != "" || textBox3Telefone.Text != "")
                {
                    conectar.abriConexao();

                    string query = "SELECT * FROM cliente WHERE Tel = @Tel OR Nif = @Nif LIMIT 1";

                    using (MySqlCommand command = new MySqlCommand(query, conectar.con))
                    {
                        command.Parameters.AddWithValue("@Tel", textBox3Telefone.Text);
                        command.Parameters.AddWithValue("@Nif", textBox2Nif.Text);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Montar o objeto com os dados do usuário

                                label3CodigoCliente.Text = reader.GetInt32("CodigoCliente").ToString();
                                textBox1Nome.Text = reader.GetString("Nome").ToString();
                                textBox2Nif.Text = reader.GetString("Nif").ToString();
                                textBox4Endereco.Text = reader.GetString("Enderoco").ToString();
                                textBox3Telefone.Text = reader.GetString("Tel").ToString();

                                MessageBox.Show("Usuario existente", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                            }
                            else
                            {
                                //Salvar no banco de dados
                                verificar(textBox1Nome.Text, textBox2Nif.Text, textBox4Endereco.Text, textBox3Telefone.Text);
                                codigoUsuario();
                            }
                        }
                    }
                }
                 
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao acessar o banco de dados: " + ex.Message);
            }
        }
    }
}
