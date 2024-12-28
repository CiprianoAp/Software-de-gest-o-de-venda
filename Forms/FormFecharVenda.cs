using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projecto_dip_oficial.Forms
{
    public partial class FormFecharVenda : Form
    {
        public FormFecharVenda()
        {
            InitializeComponent();
        }

        private void FormFecharVenda_Load(object sender, EventArgs e)
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
            // label4.ForeColor = ThemeColor.SecondaryColor;
            // label5.ForeColor = ThemeColor.PrimaryColor;
        }
    }
}
