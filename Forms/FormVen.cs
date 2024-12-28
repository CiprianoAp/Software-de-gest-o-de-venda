using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projecto_dip_oficial.Forms
{
    public partial class FormVendas : Form
    {
        Conexao conectar = new Conexao();

        string id_user;
        public FormVendas(string id_user)
        {
            InitializeComponent();
            this.id_user = id_user;
            quantVendaDiaria();
           
        }

        private void FormVendas_Load(object sender, EventArgs e)
        {
            LoadTheme();
            dadosVendas();
            totalDiario();
        
        }

        private void LoadTheme()
        {
            foreach (Control btns in this.Controls)
            {
                if (btns.GetType() == typeof(Button))
                {
                    Button btn = (Button)btns;
                    btn.BackColor = Thecolor.PrimaryColor;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.BorderColor = Thecolor.SecondaryColor;
                }
            }

            label1.ForeColor = Thecolor.SecondaryColor;
            label1Venda_diaria_quantidade.ForeColor = Thecolor.SecondaryColor;
            label1Venda_diaria_total.ForeColor = Thecolor.SecondaryColor;
            label2.ForeColor = Thecolor.SecondaryColor;
            label3.ForeColor = Thecolor.SecondaryColor;
            label4TotalGeral.ForeColor = Thecolor.SecondaryColor;
        }


        //Pegar codigo do usuário
        public int codiUser()
        {
               conectar.abriConexao();
               string Sql = "select Codigo from usuario where Email = '" + id_user + "'";
                MySqlCommand comando = new MySqlCommand(Sql, conectar.con);
                int Id;

                try
                {
                    Int32 n = Convert.ToInt32(comando.ExecuteScalar());
                    Id = n;
                }
                catch (Exception)
                {

                    throw;
                }
            return Id;
        }


        public void dadosVendas()
        {
            try
            {
                conectar.abriConexao();
                string sql = "SELECT Idvenda,Valor,Tipo_pagamento,Troco,TipoUser, Nome as Nome_operador, Sobrenome as Sobrenome_operador, Nome_cliente, DataVenda FROM caixa  INNER JOIN usuario ON caixa.Codigo_usuario = usuario.Codigo  JOIN cliente ON caixa.Codigo_cliente = cliente.CodigoCliente where Codigo_usuario = '" + codiUser() + "'   order by dataVenda asc";
                MySqlCommand comando = new MySqlCommand(sql, conectar.con);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = comando;
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;



                conectar.fecharConexao();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }


        }

        public void dadosVendasGeral(string codigo)
        {
            conectar.abriConexao();
            string sql = "select CodigoVenda as codigo_fatura,produto.CodigoProduto, Produto,Preco,QuantidadeComprada,Valor as Valor_pago from item_venda inner join produto on item_venda.CodigoProduto = produto.CodigoProduto where CodigoVenda ='" + codigo + "' ";
            MySqlCommand comando = new MySqlCommand(sql, conectar.con);
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = comando;
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
            conectar.fecharConexao();
        }

        public void totalDiario()
        {
            DateTime dataHoje = DateTime.Today;
            string data = dataHoje.ToString("dd/MM/yyyy");

            conectar.abriConexao();
            string sql = "SELECT SUM(valor) AS soma_total FROM caixa where DataVenda = '" + data + "' and Codigo_usuario = '"+ codiUser() + "' ";
            MySqlCommand comando = new MySqlCommand(sql, conectar.con);
      
            try
            {
                Int32 n = Convert.ToInt32(comando.ExecuteScalar());
               
                label1Venda_diaria_total.Text = n.ToString() + " Kz" ;
                
              
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
      
        }

        public void quantVendaDiaria()
        {
            DateTime dataHoje = DateTime.Today;
            string data = dataHoje.ToString("dd/MM/yyyy");

            conectar.abriConexao();
            string sql = "SELECT count(Valor) AS soma_total FROM caixa where DataVenda = '" + data + "' and Codigo_usuario = '" + codiUser() + "' ";
            MySqlCommand comando = new MySqlCommand(sql, conectar.con);

            try
            {
                Int32 n = Convert.ToInt32(comando.ExecuteScalar());

                label1Venda_diaria_quantidade.Text = n.ToString();


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }


        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var item = dataGridView1.Rows[e.RowIndex].Cells[0].Value;
            var total_geral = dataGridView1.Rows[e.RowIndex].Cells[1].Value;
            label4TotalGeral.Text = total_geral.ToString() + " Kz";
            dadosVendasGeral(item.ToString());
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void textBox1_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13)
            {
                //DataTable dt = dataGridView1;
            }
        }
    }
}
