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
    public partial class MLA : Form
    {
        public MLA()
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
       
            // Validar datos numéricos
            if (!int.TryParse(txt_Xi.Text, out int x0) ||
                !int.TryParse(txt_Yi.Text, out int y0) ||
                !int.TryParse(txt_Xf.Text, out int x1) ||
                !int.TryParse(txt_Yf.Text, out int y1))
            {
                MessageBox.Show("Ingrese valores numéricos válidos.");
                return;
            }

            // Llamada al algoritmo Midpoint de CLineas
            CLineas.DrawLineMidpoint(picCanvas, x0, y0, x1, y1, Color.Black);
        

    }
}
}
