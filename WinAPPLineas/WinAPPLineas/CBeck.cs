namespace WinAPPLineas
{
    partial class CBeck
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button btnRecortar;
        private System.Windows.Forms.Button btnDefinirVentana;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Button btnAyuda;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnRecortar = new System.Windows.Forms.Button();
            this.btnDefinirVentana = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnAyuda = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelTop.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.btnAyuda);
            this.panelTop.Controls.Add(this.btnLimpiar);
            this.panelTop.Controls.Add(this.btnDefinirVentana);
            this.panelTop.Controls.Add(this.btnRecortar);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Height = 44;
            this.panelTop.Padding = new System.Windows.Forms.Padding(8);
            // 
            // btnRecortar
            // 
            this.btnRecortar.Text = "Recortar (Click Der)";
            this.btnRecortar.Width = 150;
            this.btnRecortar.Left = 8;
            this.btnRecortar.Top = 8;
            // 
            // btnDefinirVentana
            // 
            this.btnDefinirVentana.Text = "Definir Ventana (R)";
            this.btnDefinirVentana.Width = 160;
            this.btnDefinirVentana.Left = 168;
            this.btnDefinirVentana.Top = 8;
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Text = "Limpiar (C)";
            this.btnLimpiar.Width = 120;
            this.btnLimpiar.Left = 336;
            this.btnLimpiar.Top = 8;
            // 
            // btnAyuda
            // 
            this.btnAyuda.Text = "Ayuda";
            this.btnAyuda.Width = 90;
            this.btnAyuda.Left = 464;
            this.btnAyuda.Top = 8;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.toolStripStatusLabel });
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Text = "Cyrus–Beck listo. Izq=p0, Der=p1.";
            // 
            // CBeck
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(900, 650);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.panelTop);
            this.DoubleBuffered = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cortar Líneas - Cyrus–Beck";
            this.panelTop.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}