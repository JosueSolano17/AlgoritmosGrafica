using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinAPPLineas.WinAPPLineas;

namespace WinAPPLineas
{
    public partial class DDA : Form
    {
       
        public DDA()
        {
            InitializeComponent();
            txt_Xi.KeyPress += SoloNumeros;
            txt_Xf.KeyPress += SoloNumeros;
            txt_Yi.KeyPress += SoloNumeros;
            txt_Yf.KeyPress += SoloNumeros;
            
        }
        private void SoloNumeros(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !char.IsDigit(e.KeyChar) &&
                e.KeyChar != '-')
            {
                e.Handled = true;
            }

            
            TextBox txt = sender as TextBox;
            if (e.KeyChar == '-' && txt.Text.Contains("-"))
                e.Handled = true;
        }

        private void btnDibujar_Click(object sender, EventArgs e)
        {
            if (txt_Xi.Text == "" || txt_Xf.Text == "" || txt_Yi.Text == "" || txt_Yf.Text == "")
            {
                MessageBox.Show("Debes ingresar todos los valores.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int x0 = int.Parse(txt_Xi.Text);
            int x1 = int.Parse(txt_Xf.Text);
            int y0 = int.Parse(txt_Yi.Text);
            int y1 = int.Parse(txt_Yf.Text);


            CLineas.DrawLineDDA(picCanvas, x0, y0, x1, y1, Color.Black);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {

        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
            this.Close();
        }

        private void DDA_Load(object sender, EventArgs e)
        {

        }
    }
}
