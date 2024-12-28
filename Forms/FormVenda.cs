using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace projecto_dip_oficial.Forms
{
    public partial class FormVenda : Form
    {
        Conexao conectar = new Conexao();

        int quantidade = 0;
        byte[] foto = null;
        int cod;
        string Sql = "";
        float total;
        int QuantVenda = 0;
        int CodigoUsuario = 1;
        int userCodi { get; set; }

        string usuario { get; set; }


       
        public FormVenda(string usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
            pesquisa();
            gerarCod();
            codigoUsuario();
        }

        private void FormVenda_Load(object sender, EventArgs e)
        {
            LoadTheme();
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
            label4.ForeColor = Thecolor.SecondaryColor;
            label5.ForeColor = Thecolor.PrimaryColor;
            label1.ForeColor = Thecolor.PrimaryColor;
            label2.ForeColor = Thecolor.PrimaryColor;
            label3.ForeColor = Thecolor.PrimaryColor;
            label4.ForeColor = Thecolor.PrimaryColor;
            label5.ForeColor = Thecolor.PrimaryColor;
            label6.ForeColor = Thecolor.PrimaryColor;
            label7.ForeColor = Thecolor.PrimaryColor;
            label8.ForeColor = Thecolor.PrimaryColor;
            label9.ForeColor = Thecolor.PrimaryColor;
            label11.ForeColor = Thecolor.PrimaryColor;
            label13Troco.ForeColor = Thecolor.PrimaryColor;
            label10.ForeColor = Thecolor.PrimaryColor;  
        }

        //Função pesquisar
        public void pesquisa()
        {
            try
            {
                conectar.abriConexao();
                MySqlDataAdapter pesquisar = new MySqlDataAdapter("SELECT * FROM produto WHERE Produto LIKE '%" + textBox1Pesquisar.Text + "%' ORDER by Produto", conectar.con);
                DataTable tabela = new DataTable();
                pesquisar.Fill(tabela);
                dataGridView1.DataSource = tabela;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                conectar.fecharConexao();
            }
        }
        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            pesquisa();
        }

        //Pegar codigo do usuario
        public void codigoUsuario()
        {
            conectar.abriConexao();
            Sql = "select Codigo from usuario where Email = '"+usuario+"'";
            MySqlCommand comando = new MySqlCommand(Sql, conectar.con);

            try
            {
                Int32 n = Convert.ToInt32(comando.ExecuteScalar()) ;
                userCodi = n;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Gerador de codigo
        public void gerarCod()
        {
            conectar.abriConexao();
            Sql = "select max(Idvenda)  from caixa";
            MySqlCommand comando = new MySqlCommand(Sql, conectar.con);


            try
            {




                if (comando.ExecuteScalar() == DBNull.Value)
                {
                    label13IdCompra.Text = "1";
                }
                else
                {
                    Int32 n = Convert.ToInt32(comando.ExecuteScalar()) + 1;
                    label13IdCompra.Text = n.ToString();

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            } finally { conectar.fecharConexao(); }

        }

        //DatagreedView pegar dados
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cod = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            textBox2Produto.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            quantidade = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
            textBox4Valor.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();


            if (dataGridView1.Rows[e.RowIndex].Cells[4].Value != null && dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString() != string.Empty)
            {
                // A célula contém um valor. Aqui você pode adicionar a lógica que desejar.
                foto = (byte[])dataGridView1.Rows[e.RowIndex].Cells[4].Value;
                // Realize operações com o valor
            }



            if (foto != null)
            {
                MemoryStream memory = new MemoryStream(foto);
                pictureBox4Foto.Image = Image.FromStream(memory);
            }


        }

        private void textBox3Quantidade_KeyUp(object sender, KeyEventArgs e)
        {

            if (textBox3Quantidade.Text !="")
            {
                label13TotalItem.Text = (float.Parse(textBox3Quantidade.Text) * float.Parse(textBox4Valor.Text)).ToString();
            }
            
        }
        //Adicionar ao carrinho
        public void addCarrinho()
        {


            if (textBox3Quantidade.Text == "")
            {
                MessageBox.Show("Digite o valor da quantidade", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else { QuantVenda = int.Parse(textBox3Quantidade.Text); }


            if (QuantVenda > quantidade && textBox3Quantidade.Text != "")
            {
                MessageBox.Show("Esse produto apenas cotem " + quantidade + " em Stock", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (textBox3Quantidade.Text != "")
            {
                dataGridView2.Rows.Add(cod, textBox2Produto.Text, textBox4Valor.Text, textBox3Quantidade.Text, label13TotalItem.Text);
                total += float.Parse(label13TotalItem.Text);

                label10TotalGeral.Text = total.ToString();

                //Limpar dados
                limpar();
            }
        }

        //Limpar os camposapa
        public void limpar()
        {
            textBox1Pesquisar.Clear();
            textBox2Produto.Clear();
            textBox3Quantidade.Clear();
            textBox4Valor.Clear();
            label13TotalItem.Text = "?";
           

        }
        private void button1_Click(object sender, EventArgs e)
        {
            //Add carrinho
            addCarrinho();
            //Calcular o troco
            calcTroco();
        }

        //Remover do carrinho
        public void RemovCarrinho()
        {
            if (posicao == -1)
            {
                MessageBox.Show("Seleciona uma linha no carrinho que desenha remover", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {


                if (dataGridView2.Rows.Count == 2)
                {
                    label13TrocoResult.Text = "0,000";
                    textBox1ValorApagar.Clear();
                }

                if (dataGridView2.Rows.Count > 1)
                {
                    dataGridView2.Rows.RemoveAt(posicao);
                    total -= valorItem;
                    label10TotalGeral.Text = total.ToString();
                } 
                else
                {
                    MessageBox.Show("Sem dados para eliminar na tabela","Aviso!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
            }
        }
        int posicao = -1;
        float valorItem;
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            posicao = dataGridView2.CurrentRow.Index;
            valorItem = float.Parse(dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString());
        }

        //Verificar o tipo de pagamento se está selecionado

        public void tipoPagamento()
        {
            if (comboBox1TipoPagamento.Text == "")
            {
                MessageBox.Show("Deve selecionar um tipo de pagamento Card ou Caxi","Aviso",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            else
            {
                VendaBD();
            }
        }


        //Salvar venda no banco

        public void VendaBD()
        {
            DateTime dataHoje = DateTime.Today;
            string data = dataHoje.ToString("dd/MM/yyyy");

            if (textBox1CodigoCliente.Text != "")
            {
                CodigoUsuario = int.Parse(textBox1CodigoCliente.Text);
            }
            conectar.abriConexao();
            Sql = "INSERT INTO caixa (Valor,Tipo_pagamento,Codigo_usuario,Troco,Codigo_cliente,DataVenda) value('" + float.Parse(label10TotalGeral.Text) + "','" + comboBox1TipoPagamento.Text + "','" + userCodi + "','" + label13TrocoResult.Text + "','"+ CodigoUsuario + "','"+ data + "')";
            MySqlCommand caixa = new MySqlCommand(Sql, conectar.con);
            caixa.ExecuteNonQuery();
            conectar.fecharConexao();

            conectar.abriConexao();

            for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
            {
                idP = int.Parse(dataGridView2.Rows[i].Cells[0].Value.ToString());
                valorP = int.Parse(dataGridView2.Rows[i].Cells[4].Value.ToString());
                quantP = int.Parse(dataGridView2.Rows[i].Cells[3].Value.ToString());
                MySqlCommand Item_venda = new MySqlCommand("insert into item_venda (CodigoVenda,CodigoProduto,QuantidadeComprada,Valor,Data_hora) value('" + int.Parse(label13IdCompra.Text) + "','" + idP + "','" + quantP + "','" + valorP + "','"+ tempo()+ "')", conectar.con);
                Item_venda.ExecuteNonQuery();

                //Reduzir produto no stock por mei de venda
                MySqlCommand atualizarStok = new MySqlCommand("UPDATE produto SET Quantidade = Quantidade - @quantidadeVendida WHERE CodigoProduto = @idProduto", conectar.con);
                atualizarStok.Parameters.AddWithValue("@quantidadeVendida", quantP);
                atualizarStok.Parameters.AddWithValue("@idProduto", idP);
                atualizarStok.ExecuteNonQuery();

                

            }

            conectar.fecharConexao();


            for (int y = dataGridView2.Rows.Count - 2; y >= 0; y--)
            {
                dataGridView2.Rows.RemoveAt(y);
            }

            MessageBox.Show("Venda finalizada com sucesso");

            limpar();
            pictureBox4Foto.Image = null;
            label10TotalGeral.Text = "0,000";
            label13TrocoResult.Text = "0,000";
            textBox1ValorApagar.Clear();
            textBox1CodigoCliente.Clear();
            pesquisa();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            RemovCarrinho();
            //Calcular o troco
            calcTroco();
        }

        int idP { get; set; }
        int quantP { get; set; }
        float valorP { get; set; }
        float totalP { get; set; }
        private void button3_Click(object sender, EventArgs e)
        {
            var resultado = MessageBox.Show("Desejas Fenalizar essa compra", "Mensagem", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                tipoPagamento();
                gerarCod();
            }
            else
            {
                MessageBox.Show("Venda cancelada");
            }
           

        }

        private void textBox1ValorApagar_KeyUp(object sender, KeyEventArgs e)
        {
            //Calcular o troco
            calcTroco();
        }

        //Função calcular troco
        public void calcTroco()
        {
          
           
                if (textBox1ValorApagar.Text == "" || int.Parse(textBox1ValorApagar.Text) == 0)
                {
                    label13TrocoResult.Text = "0,000";
                }
                else if (int.Parse(label10TotalGeral.Text) > int.Parse(textBox1ValorApagar.Text))
                {
                    label13TrocoResult.Text = "0,000";
                }
                else
                {
                    label13TrocoResult.Text = (int.Parse(textBox1ValorApagar.Text) - int.Parse(label10TotalGeral.Text)).ToString();
                }
            
        }

        //Fução que pega data e hora
       
         // Exibe a data e hora completa
         public string tempo() 
         {
            DateTime dataHoraAtual = DateTime.Now;

            dataHoraAtual = DateTime.Now;
            string dataHoraFormatada = dataHoraAtual.ToString("dddd, dd 'de' MMMM 'de' yyyy 'às' HH:mm:ss");

            return dataHoraFormatada;
         }  


        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox1.Checked)
            {
                User.FrmUsuario usario  = new User.FrmUsuario();
                usario.ShowDialog();
                
            }

            checkBox1.Checked = false;

        }

       
    }
}
