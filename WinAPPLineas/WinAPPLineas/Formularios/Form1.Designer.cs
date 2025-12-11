namespace WinAPPLineas
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.lineasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ddaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.midpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xiaolinWuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.circulosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.midPointCircleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.circleBrenshmanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.círcleParamétricoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rellenoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.boundRayFillToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.floodFillToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scalineFillToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cortarLineasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cSutherlandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cBeckToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lBarskyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cortarPoligonosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gHormannToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sHodgmannToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wAlthertonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.menuStrip1.ForeColor = System.Drawing.Color.White;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lineasToolStripMenuItem,
            this.circulosToolStripMenuItem,
            this.rellenoToolStripMenuItem,
            this.cortarLineasToolStripMenuItem,
            this.cortarPoligonosToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1081, 28);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // lineasToolStripMenuItem
            // 
            this.lineasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ddaToolStripMenuItem,
            this.midpointToolStripMenuItem,
            this.xiaolinWuToolStripMenuItem});
            this.lineasToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.lineasToolStripMenuItem.Name = "lineasToolStripMenuItem";
            this.lineasToolStripMenuItem.Size = new System.Drawing.Size(67, 24);
            this.lineasToolStripMenuItem.Text = "Líneas";
            this.lineasToolStripMenuItem.Click += new System.EventHandler(this.lineasToolStripMenuItem_Click);
            // 
            // ddaToolStripMenuItem
            // 
            this.ddaToolStripMenuItem.Name = "ddaToolStripMenuItem";
            this.ddaToolStripMenuItem.Size = new System.Drawing.Size(168, 26);
            this.ddaToolStripMenuItem.Text = "DDA";
            this.ddaToolStripMenuItem.Click += new System.EventHandler(this.ddaToolStripMenuItem_Click);
            // 
            // midpointToolStripMenuItem
            // 
            this.midpointToolStripMenuItem.Name = "midpointToolStripMenuItem";
            this.midpointToolStripMenuItem.Size = new System.Drawing.Size(168, 26);
            this.midpointToolStripMenuItem.Text = "MidPoint";
            this.midpointToolStripMenuItem.Click += new System.EventHandler(this.midpointToolStripMenuItem_Click);
            // 
            // xiaolinWuToolStripMenuItem
            // 
            this.xiaolinWuToolStripMenuItem.Name = "xiaolinWuToolStripMenuItem";
            this.xiaolinWuToolStripMenuItem.Size = new System.Drawing.Size(168, 26);
            this.xiaolinWuToolStripMenuItem.Text = "Xiaolin Wu";
            this.xiaolinWuToolStripMenuItem.Click += new System.EventHandler(this.xiaolinWuToolStripMenuItem_Click);
            // 
            // circulosToolStripMenuItem
            // 
            this.circulosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.midPointCircleToolStripMenuItem,
            this.circleBrenshmanToolStripMenuItem,
            this.círcleParamétricoToolStripMenuItem});
            this.circulosToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.circulosToolStripMenuItem.Name = "circulosToolStripMenuItem";
            this.circulosToolStripMenuItem.Size = new System.Drawing.Size(78, 24);
            this.circulosToolStripMenuItem.Text = "Circulos";
            // 
            // midPointCircleToolStripMenuItem
            // 
            this.midPointCircleToolStripMenuItem.Name = "midPointCircleToolStripMenuItem";
            this.midPointCircleToolStripMenuItem.Size = new System.Drawing.Size(215, 26);
            this.midPointCircleToolStripMenuItem.Text = "MidPointCircle";
            this.midPointCircleToolStripMenuItem.Click += new System.EventHandler(this.midPointCircleToolStripMenuItem_Click);
            // 
            // circleBrenshmanToolStripMenuItem
            // 
            this.circleBrenshmanToolStripMenuItem.Name = "circleBrenshmanToolStripMenuItem";
            this.circleBrenshmanToolStripMenuItem.Size = new System.Drawing.Size(215, 26);
            this.circleBrenshmanToolStripMenuItem.Text = "CircleBrenshman";
            this.circleBrenshmanToolStripMenuItem.Click += new System.EventHandler(this.circleBrenshmanToolStripMenuItem_Click);
            // 
            // círcleParamétricoToolStripMenuItem
            // 
            this.círcleParamétricoToolStripMenuItem.Name = "círcleParamétricoToolStripMenuItem";
            this.círcleParamétricoToolStripMenuItem.Size = new System.Drawing.Size(215, 26);
            this.círcleParamétricoToolStripMenuItem.Text = "CírcleParamétrico";
            this.círcleParamétricoToolStripMenuItem.Click += new System.EventHandler(this.círcleParamétricoToolStripMenuItem_Click);
            // 
            // rellenoToolStripMenuItem
            // 
            this.rellenoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.boundRayFillToolStripMenuItem,
            this.floodFillToolStripMenuItem,
            this.scalineFillToolStripMenuItem});
            this.rellenoToolStripMenuItem.Name = "rellenoToolStripMenuItem";
            this.rellenoToolStripMenuItem.Size = new System.Drawing.Size(75, 24);
            this.rellenoToolStripMenuItem.Text = "Relleno";
            this.rellenoToolStripMenuItem.Click += new System.EventHandler(this.rellenoToolStripMenuItem_Click);
            // 
            // boundRayFillToolStripMenuItem
            // 
            this.boundRayFillToolStripMenuItem.Name = "boundRayFillToolStripMenuItem";
            this.boundRayFillToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.boundRayFillToolStripMenuItem.Text = "BoundRay Fill";
            this.boundRayFillToolStripMenuItem.Click += new System.EventHandler(this.boundRayFillToolStripMenuItem_Click);
            // 
            // floodFillToolStripMenuItem
            // 
            this.floodFillToolStripMenuItem.Name = "floodFillToolStripMenuItem";
            this.floodFillToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.floodFillToolStripMenuItem.Text = "Flood Fill";
            this.floodFillToolStripMenuItem.Click += new System.EventHandler(this.floodFillToolStripMenuItem_Click);
            // 
            // scalineFillToolStripMenuItem
            // 
            this.scalineFillToolStripMenuItem.Name = "scalineFillToolStripMenuItem";
            this.scalineFillToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.scalineFillToolStripMenuItem.Text = "Scaline Fill";
            this.scalineFillToolStripMenuItem.Click += new System.EventHandler(this.scalineFillToolStripMenuItem_Click);
            // 
            // cortarLineasToolStripMenuItem
            // 
            this.cortarLineasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cSutherlandToolStripMenuItem,
            this.cBeckToolStripMenuItem,
            this.lBarskyToolStripMenuItem});
            this.cortarLineasToolStripMenuItem.Name = "cortarLineasToolStripMenuItem";
            this.cortarLineasToolStripMenuItem.Size = new System.Drawing.Size(115, 24);
            this.cortarLineasToolStripMenuItem.Text = "Cortar Lineas";
            // 
            // cSutherlandToolStripMenuItem
            // 
            this.cSutherlandToolStripMenuItem.Name = "cSutherlandToolStripMenuItem";
            this.cSutherlandToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.cSutherlandToolStripMenuItem.Text = "CSutherland";
            this.cSutherlandToolStripMenuItem.Click += new System.EventHandler(this.cSutherlandToolStripMenuItem_Click_1);
            // 
            // cBeckToolStripMenuItem
            // 
            this.cBeckToolStripMenuItem.Name = "cBeckToolStripMenuItem";
            this.cBeckToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.cBeckToolStripMenuItem.Text = "CBeck";
            this.cBeckToolStripMenuItem.Click += new System.EventHandler(this.cBeckToolStripMenuItem_Click_1);
            // 
            // lBarskyToolStripMenuItem
            // 
            this.lBarskyToolStripMenuItem.Name = "lBarskyToolStripMenuItem";
            this.lBarskyToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.lBarskyToolStripMenuItem.Text = "LBarsky";
            this.lBarskyToolStripMenuItem.Click += new System.EventHandler(this.lBarskyToolStripMenuItem_Click_1);
            // 
            // cortarPoligonosToolStripMenuItem
            // 
            this.cortarPoligonosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gHormannToolStripMenuItem,
            this.sHodgmannToolStripMenuItem,
            this.wAlthertonToolStripMenuItem});
            this.cortarPoligonosToolStripMenuItem.Name = "cortarPoligonosToolStripMenuItem";
            this.cortarPoligonosToolStripMenuItem.Size = new System.Drawing.Size(140, 24);
            this.cortarPoligonosToolStripMenuItem.Text = "Cortar Poligonos";
            // 
            // gHormannToolStripMenuItem
            // 
            this.gHormannToolStripMenuItem.Name = "gHormannToolStripMenuItem";
            this.gHormannToolStripMenuItem.Size = new System.Drawing.Size(178, 26);
            this.gHormannToolStripMenuItem.Text = "GHormann";
            // 
            // sHodgmannToolStripMenuItem
            // 
            this.sHodgmannToolStripMenuItem.Name = "sHodgmannToolStripMenuItem";
            this.sHodgmannToolStripMenuItem.Size = new System.Drawing.Size(178, 26);
            this.sHodgmannToolStripMenuItem.Text = "SHodgmann";
            // 
            // wAlthertonToolStripMenuItem
            // 
            this.wAlthertonToolStripMenuItem.Name = "wAlthertonToolStripMenuItem";
            this.wAlthertonToolStripMenuItem.Size = new System.Drawing.Size(178, 26);
            this.wAlthertonToolStripMenuItem.Text = "WAltherton";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(1081, 405);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Algoritmos de Líneas y Círculos";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem lineasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem circulosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ddaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem midpointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xiaolinWuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem midPointCircleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem circleBrenshmanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem círcleParamétricoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rellenoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem boundRayFillToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem floodFillToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scalineFillToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cortarLineasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cortarPoligonosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cSutherlandToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cBeckToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lBarskyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gHormannToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sHodgmannToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wAlthertonToolStripMenuItem;
    }
}

