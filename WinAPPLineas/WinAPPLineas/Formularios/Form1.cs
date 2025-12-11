using System;
using System.Windows.Forms;
using WinAPPLineas.Formularios;

namespace WinAPPLineas
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Inicialización al cargar, si aplica
        }

        private void ddaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var child = new DDA { MdiParent = this };
            child.Show();
        }

        private void midpointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var child = new MLA { MdiParent = this };
            child.Show();
        }

        private void xiaolinWuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var child = new XWLA { MdiParent = this };
            child.Show();
        }

        private void midPointCircleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Corregido: abrir el formulario MidPoint Circle como MDI child
            var child = new Circulo { MdiParent = this };
            child.Show();
        }

        private void circleBrenshmanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var child = new CBA { MdiParent = this };
            child.Show();
        }

        private void círcleParamétricoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var child = new CPE { MdiParent = this };
            child.Show();
        }

        // Nuevos formularios: Relleno, Recorte de Líneas, Recorte de Polígonos
        // Menú Relleno
        private void floodFillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FloodFillForm child = new FloodFillForm { MdiParent = this };
            child.Show();
        }

        private void boundaryFillToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void scanlineFillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        // Menú Cortar Líneas
        private void cSutherlandToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void cBeckToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void lBarskyToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        // Menú Cortar Polígonos
        private void sHodgmanToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void wAthertonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void gHormannToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void lineasToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void rellenoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void boundRayFillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BoundaryFillForm child = new BoundaryFillForm { MdiParent = this };
            child.Show();
        }

        private void scalineFillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScanlineFillForm child = new ScanlineFillForm { MdiParent = this }; 
            child.Show();
        }

        private void cSutherlandToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            CohenSutherlandForm child = new CohenSutherlandForm { MdiParent = this };
            child.Show();
        }

        private void cBeckToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            CyrusBeckForm child = new CyrusBeckForm { MdiParent = this };   
            child.Show();
        }

        private void lBarskyToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            LiangBarskyForm child = new LiangBarskyForm { MdiParent = this };
            child.Show();
        }
    }
}
