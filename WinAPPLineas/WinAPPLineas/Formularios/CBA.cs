using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinAPPLineas
{
    public partial class CBA : Form
    {
        public CBA()
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
            if (e.KeyChar == '-' && txt != null && txt.Text.Contains("-"))
                e.Handled = true;
        }

        private void btnDibujar_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txt_X.Text, out int x) ||
                !int.TryParse(txt_Y.Text, out int y) ||
                !int.TryParse(txt_R.Text, out int r))
            {
                MessageBox.Show("Ingrese valores válidos.", "Entrada inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (r <= 0)
            {
                MessageBox.Show("El radio debe ser mayor que cero.", "Radio inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int cx = picCanvas.Width / 2;
            int cy = picCanvas.Height / 2;

            int xc = cx + x;
            int yc = cy - y;

            CCirculo.DrawCircleBresenham(picCanvas, xc, yc, r, Color.DodgerBlue);
        }

        private void CBA_Load(object sender, EventArgs e)
        {

        }
    }
}
