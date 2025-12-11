namespace WinAPPLineas
{
    partial class DDA
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grbProcess = new System.Windows.Forms.GroupBox();
            this.btnDibujar = new System.Windows.Forms.Button();
            this.grbInputs = new System.Windows.Forms.GroupBox();
            this.txt_Yf = new System.Windows.Forms.TextBox();
            this.txt_Yi = new System.Windows.Forms.TextBox();
            this.txt_Xf = new System.Windows.Forms.TextBox();
            this.lbl_Yi = new System.Windows.Forms.Label();
            this.lbl_Yf = new System.Windows.Forms.Label();
            this.lbl_Xf = new System.Windows.Forms.Label();
            this.txt_Xi = new System.Windows.Forms.TextBox();
            this.lblXi = new System.Windows.Forms.Label();
            this.grbCanvas = new System.Windows.Forms.GroupBox();
            this.picCanvas = new System.Windows.Forms.PictureBox();
            this.grbProcess.SuspendLayout();
            this.grbInputs.SuspendLayout();
            this.grbCanvas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCanvas)).BeginInit();
            this.SuspendLayout();
            // 
            // grbProcess
            // 
            this.grbProcess.Controls.Add(this.btnDibujar);
            this.grbProcess.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grbProcess.Location = new System.Drawing.Point(3, 251);
            this.grbProcess.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbProcess.Name = "grbProcess";
            this.grbProcess.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbProcess.Size = new System.Drawing.Size(365, 78);
            this.grbProcess.TabIndex = 17;
            this.grbProcess.TabStop = false;
            this.grbProcess.Text = "Procesos";
            // 
            // btnDibujar
            // 
            this.btnDibujar.BackColor = System.Drawing.Color.SeaGreen;
            this.btnDibujar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDibujar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDibujar.ForeColor = System.Drawing.Color.White;
            this.btnDibujar.Location = new System.Drawing.Point(124, 27);
            this.btnDibujar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDibujar.Name = "btnDibujar";
            this.btnDibujar.Size = new System.Drawing.Size(133, 37);
            this.btnDibujar.TabIndex = 0;
            this.btnDibujar.Text = "Dibujar DDA";
            this.btnDibujar.UseVisualStyleBackColor = false;
            this.btnDibujar.Click += new System.EventHandler(this.btnDibujar_Click);
            // 
            // grbInputs
            // 
            this.grbInputs.Controls.Add(this.txt_Yf);
            this.grbInputs.Controls.Add(this.txt_Yi);
            this.grbInputs.Controls.Add(this.txt_Xf);
            this.grbInputs.Controls.Add(this.lbl_Yi);
            this.grbInputs.Controls.Add(this.lbl_Yf);
            this.grbInputs.Controls.Add(this.lbl_Xf);
            this.grbInputs.Controls.Add(this.txt_Xi);
            this.grbInputs.Controls.Add(this.lblXi);
            this.grbInputs.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grbInputs.Location = new System.Drawing.Point(3, 15);
            this.grbInputs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbInputs.Name = "grbInputs";
            this.grbInputs.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbInputs.Size = new System.Drawing.Size(367, 212);
            this.grbInputs.TabIndex = 16;
            this.grbInputs.TabStop = false;
            this.grbInputs.Text = "Entradas";
            // 
            // txt_Yf
            // 
            this.txt_Yf.Location = new System.Drawing.Point(149, 150);
            this.txt_Yf.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_Yf.Name = "txt_Yf";
            this.txt_Yf.Size = new System.Drawing.Size(183, 27);
            this.txt_Yf.TabIndex = 8;
            // 
            // txt_Yi
            // 
            this.txt_Yi.Location = new System.Drawing.Point(149, 117);
            this.txt_Yi.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_Yi.Name = "txt_Yi";
            this.txt_Yi.Size = new System.Drawing.Size(183, 27);
            this.txt_Yi.TabIndex = 7;
            // 
            // txt_Xf
            // 
            this.txt_Xf.Location = new System.Drawing.Point(149, 78);
            this.txt_Xf.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_Xf.Name = "txt_Xf";
            this.txt_Xf.Size = new System.Drawing.Size(183, 27);
            this.txt_Xf.TabIndex = 6;
            // 
            // lbl_Yi
            // 
            this.lbl_Yi.AutoSize = true;
            this.lbl_Yi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lbl_Yi.ForeColor = System.Drawing.Color.Gainsboro;
            this.lbl_Yi.Location = new System.Drawing.Point(8, 117);
            this.lbl_Yi.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_Yi.Name = "lbl_Yi";
            this.lbl_Yi.Size = new System.Drawing.Size(124, 20);
            this.lbl_Yi.TabIndex = 5;
            this.lbl_Yi.Text = "Ingrese \"Y\" inicial";
            // 
            // lbl_Yf
            // 
            this.lbl_Yf.AutoSize = true;
            this.lbl_Yf.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lbl_Yf.ForeColor = System.Drawing.Color.Gainsboro;
            this.lbl_Yf.Location = new System.Drawing.Point(8, 150);
            this.lbl_Yf.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_Yf.Name = "lbl_Yf";
            this.lbl_Yf.Size = new System.Drawing.Size(114, 20);
            this.lbl_Yf.TabIndex = 4;
            this.lbl_Yf.Text = "Ingrese \"Y\" final";
            // 
            // lbl_Xf
            // 
            this.lbl_Xf.AutoSize = true;
            this.lbl_Xf.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lbl_Xf.ForeColor = System.Drawing.Color.Gainsboro;
            this.lbl_Xf.Location = new System.Drawing.Point(11, 81);
            this.lbl_Xf.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_Xf.Name = "lbl_Xf";
            this.lbl_Xf.Size = new System.Drawing.Size(115, 20);
            this.lbl_Xf.TabIndex = 3;
            this.lbl_Xf.Text = "Ingrese \"X\" final";
            // 
            // txt_Xi
            // 
            this.txt_Xi.Location = new System.Drawing.Point(149, 42);
            this.txt_Xi.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_Xi.Name = "txt_Xi";
            this.txt_Xi.Size = new System.Drawing.Size(183, 27);
            this.txt_Xi.TabIndex = 2;
            // 
            // lblXi
            // 
            this.lblXi.AutoSize = true;
            this.lblXi.BackColor = System.Drawing.Color.Transparent;
            this.lblXi.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblXi.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblXi.Location = new System.Drawing.Point(8, 46);
            this.lblXi.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblXi.Name = "lblXi";
            this.lblXi.Size = new System.Drawing.Size(125, 20);
            this.lblXi.TabIndex = 0;
            this.lblXi.Text = "Ingrese \"X\" inicial";
            // 
            // grbCanvas
            // 
            this.grbCanvas.Controls.Add(this.picCanvas);
            this.grbCanvas.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grbCanvas.Location = new System.Drawing.Point(376, 4);
            this.grbCanvas.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbCanvas.Name = "grbCanvas";
            this.grbCanvas.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbCanvas.Size = new System.Drawing.Size(809, 576);
            this.grbCanvas.TabIndex = 15;
            this.grbCanvas.TabStop = false;
            this.grbCanvas.Text = "Gráfico";
            // 
            // picCanvas
            // 
            this.picCanvas.BackColor = System.Drawing.Color.White;
            this.picCanvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCanvas.Location = new System.Drawing.Point(8, 23);
            this.picCanvas.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picCanvas.Name = "picCanvas";
            this.picCanvas.Size = new System.Drawing.Size(775, 545);
            this.picCanvas.TabIndex = 0;
            this.picCanvas.TabStop = false;
            // 
            // DDA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.grbProcess);
            this.Controls.Add(this.grbInputs);
            this.Controls.Add(this.grbCanvas);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "DDA";
            this.Text = "Línea DDA";
            this.Load += new System.EventHandler(this.DDA_Load);
            this.grbProcess.ResumeLayout(false);
            this.grbInputs.ResumeLayout(false);
            this.grbInputs.PerformLayout();
            this.grbCanvas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picCanvas)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grbProcess;
        private System.Windows.Forms.Button btnDibujar;
        private System.Windows.Forms.GroupBox grbInputs;
        private System.Windows.Forms.TextBox txt_Yf;
        private System.Windows.Forms.TextBox txt_Yi;
        private System.Windows.Forms.TextBox txt_Xf;
        private System.Windows.Forms.Label lbl_Yi;
        private System.Windows.Forms.Label lbl_Yf;
        private System.Windows.Forms.Label lbl_Xf;
        private System.Windows.Forms.TextBox txt_Xi;
        private System.Windows.Forms.Label lblXi;
        private System.Windows.Forms.GroupBox grbCanvas;
        private System.Windows.Forms.PictureBox picCanvas;
    }
}