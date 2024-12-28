using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projecto_dip_oficial
{
    public partial class PrincipalTitleBar : Form
    {
        //Variavel
        string usuario = "";

        //Fields
        private Button curretButton;
        private Random random;
        private int tempIndex;
        private Form activeForm;


        //Constructor
        public PrincipalTitleBar(string usuario)
        {
            this.usuario = usuario; 
            InitializeComponent();
            random = new Random();
        }

        //Methods

        private Color SelectThemeColor()
        {
            int index = random.Next(Thecolor.ColorList.Count());
            while (tempIndex == index)
            {
              index =  random.Next(Thecolor.ColorList.Count());
            }

            tempIndex = index;
            string color = Thecolor.ColorList[index];
            return ColorTranslator.FromHtml(color);
        }

        private void ActivateButton(object btnSendr)
        {
            if (btnSendr != null) 
            {
                if (curretButton != (Button)btnSendr) 
                {
                    DisableButton();
                    Color color = SelectThemeColor(); 
                    curretButton= (Button)btnSendr;
                    curretButton.BackColor = color;
                    curretButton.ForeColor = Color.White; 
                    curretButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    painelTitle.BackColor = color;
                    painelLogo.BackColor = Thecolor.ChangeColorBrightness(color, -0.3);
                    Thecolor.PrimaryColor = color;
                    Thecolor.SecondaryColor = Thecolor.ChangeColorBrightness(color, -0.3);
                    btnCloseChildForm.Visible = true;
                  
                }
            }
        }

        private void DisableButton() 
        {
            foreach (Control  previousBtn in panel1Manu.Controls)
            {
                if(previousBtn.GetType() == typeof(Button))
                {
                    previousBtn.BackColor = Color.BlueViolet;
                    previousBtn.BackColor = Color.Gainsboro;
                    previousBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }
        }

        //Para abrir formulários filhos
        public void openChildForm(Form childForm, Object btnSender)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }

            ActivateButton(btnSender);
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            this.panelDesktopPanel.Controls.Add(childForm);
            this.panelDesktopPanel.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            lblTitle.Text = childForm.Text;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            openChildForm(new Forms.FormVenda(usuario), sender);
        }

        private void button6Admin_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
        }

        private void button2Stock_Click(object sender, EventArgs e)
        {
            openChildForm(new Forms.FormStock(), sender);
        }

        private void Realatorio_Click(object sender, EventArgs e)
        {
            openChildForm(new Forms.FormVendas(usuario), sender);
        }

        private void button4Trocar_Click(object sender, EventArgs e)
        {
            exit();
        }

        private void button5Fechar_Click(object sender, EventArgs e)
        {
            openChildForm(new Forms.FormFecharVenda(), sender);
        }

        private void PrincipalTitleBar_Load(object sender, EventArgs e)
        {
            labelUsername.Text = usuario;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnCloseChildForm_Click(object sender, EventArgs e)
        {
            if(activeForm != null)
            {
                activeForm.Close();
                Reset();
            }
        }

        private void Reset()
        {
            DisableButton();
            lblTitle.Text = "HOME";
            painelTitle.BackColor = Color.DodgerBlue;
            painelLogo.BackColor = Color.DodgerBlue;
            curretButton = null;
            btnCloseChildForm.Visible = false;

        }

        private void panelDesktopPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        //Função fechar o formulário 
        public void exit()
        {
            var resultado = MessageBox.Show("desejas trocar de usuário?", "Mensagem", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                Form1 login = new Form1();
                login.Show();
                this.Hide();
            }

        }
    }
}
