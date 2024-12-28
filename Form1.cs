using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;

namespace projecto_dip_oficial
{
    public partial class Form1 : Form
    {
        //Instanciar Classe conexão
        Conexao conectar = new Conexao();
        string sql;
       // MySqlCommand cmd = new MySqlCommand();
        
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void label6_Click(object sender, EventArgs e)
        {
            //Função fechar
            exit();
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            //Dados de entrada
            Verificar();
           
          
        }

        //Função abrir o formúlário pprincipal
        public void abrir_principal(string usuario)
        {
            PrincipalTitleBar principal = new PrincipalTitleBar(usuario);
            principal.Show();
            this.Hide();
        }

        //Função fechar o formulário 
        public void exit()
        {
            var resultado = MessageBox.Show("desejas fechar a aplicação", "Mensagem", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                this.Close();
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        //Verificar
        public void Verificar()
        {
            if (textBox1Nome.Text == "" && textBox2Senha.Text == "")
            {
                MessageBox.Show("Usuário e Senha não podem ser campos vazios!");
                textBox1Nome.Text = "";
                textBox2Senha.Text = null;
                return;
            }
            else
            {
                login(textBox1Nome.Text);
            }
            if (textBox1Nome.Text == "")
            {
                
                MessageBox.Show("Dige o nome do suário!");
                textBox1Nome.Text = "";
                textBox1Nome.Focus();
                return;
            }
            if (textBox2Senha.Text == "")
            {
                MessageBox.Show("Dige a senha!");
                textBox2Senha.Text = "";
                textBox2Senha.Focus();
                return;
            }
           
           
        }

        //Função de login 
        public string login(string usuario)
        {
            string Usuario = usuario;


            try
            {

                sql = "SELECT * FROM usuario WHERE  Email=@Email AND Senha=@Senha";
                conectar.abriConexao();
                MySqlCommand cmdVerificar;
                MySqlDataReader reader;
                cmdVerificar = new MySqlCommand(sql, conectar.con);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmdVerificar;
                //cmdVerificar.Parameters.AddWithValue("@Nome", textBox1Nome.Text);
                cmdVerificar.Parameters.AddWithValue("@Email", textBox1Nome.Text);
                cmdVerificar.Parameters.AddWithValue("@Senha",textBox2Senha.Text);
                reader = cmdVerificar.ExecuteReader();

                if (reader.HasRows) {

                    abrir_principal(usuario);

                }
                else
                {
                    MessageBox.Show("Usuário ou senha Encorreta!","Verifica o seu nome ou senha",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro do banco de dados", ex.Message);
            }
            finally
            {
                conectar.fecharConexao();
            }
            return Usuario;

        }

        //Limpar dados do usuário
        public void limparDados()
        {
            textBox1Nome.Text = "";
            textBox2Senha.Text = "";
        }

        private void label5_Click(object sender, EventArgs e)
        {
            //Limpar dados
            limparDados();
        }
    }
}
