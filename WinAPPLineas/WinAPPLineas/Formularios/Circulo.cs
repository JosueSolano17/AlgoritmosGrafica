using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAPPLineas
{
    public partial class Circulo : Form
    {
        public Circulo()
        {
            InitializeComponent();
            

            txt_X.KeyPress += SoloNumeros;
            txt_Y.KeyPress += SoloNumeros;
            txt_R.KeyPress += SoloNumeros;

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
            if (!int.TryParse(txt_X.Text, out int x) ||
         !int.TryParse(txt_Y.Text, out int y) ||
         !int.TryParse(txt_R.Text, out int r))
            {
                MessageBox.Show("Ingrese valores válidos.");
                return;
            }

            
            int cx = picCanvas.Width / 2;
            int cy = picCanvas.Height / 2;

            
            int xc = cx + x;     
            int yc = cy - y;    

            CCirculo.DrawCircleMidpoint(picCanvas, xc, yc, r, Color.Black);
        }

        private void Circulo_Load(object sender, EventArgs e)
        {

        }
    }
}
