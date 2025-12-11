namespace WinAPPLineas
{
    partial class CPE
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.grbInputs = new System.Windows.Forms.GroupBox();
            this.txt_R = new System.Windows.Forms.TextBox();
            this.lbl_R = new System.Windows.Forms.Label();
            this.txt_Y = new System.Windows.Forms.TextBox();
            this.lbl_Y = new System.Windows.Forms.Label();
            this.txt_X = new System.Windows.Forms.TextBox();
            this.lblX = new System.Windows.Forms.Label();
            this.grbProcess = new System.Windows.Forms.GroupBox();
            this.btnDibujar = new System.Windows.Forms.Button();
            this.grbCanvas = new System.Windows.Forms.GroupBox();
            this.picCanvas = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.grbInputs.SuspendLayout();
            this.grbProcess.SuspendLayout();
            this.grbCanvas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCanvas)).BeginInit();
            this.SuspendLayout();
            // 
            // grbInputs
            // 
            this.grbInputs.BackColor = System.Drawing.Color.Transparent;
            this.grbInputs.Controls.Add(this.txt_R);
            this.grbInputs.Controls.Add(this.lbl_R);
            this.grbInputs.Controls.Add(this.txt_Y);
            this.grbInputs.Controls.Add(this.lbl_Y);
            this.grbInputs.Controls.Add(this.txt_X);
            this.grbInputs.Controls.Add(this.lblX);
            this.grbInputs.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grbInputs.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.grbInputs.Location = new System.Drawing.Point(24, 76);
            this.grbInputs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbInputs.Name = "grbInputs";
            this.grbInputs.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbInputs.Size = new System.Drawing.Size(387, 185);
            this.grbInputs.TabIndex = 0;
            this.grbInputs.TabStop = false;
            this.grbInputs.Text = "Entradas";
            // 
            // txt_R
            // 
            this.txt_R.Location = new System.Drawing.Point(147, 123);
            this.txt_R.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_R.Name = "txt_R";
            this.txt_R.Size = new System.Drawing.Size(212, 27);
            this.txt_R.TabIndex = 5;
            // 
            // lbl_R
            // 
            this.lbl_R.AutoSize = true;
            this.lbl_R.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lbl_R.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.lbl_R.Location = new System.Drawing.Point(13, 127);
            this.lbl_R.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_R.Name = "lbl_R";
            this.lbl_R.Size = new System.Drawing.Size(108, 19);
            this.lbl_R.TabIndex = 4;
            this.lbl_R.Text = "Radio del círculo";
            // 
            // txt_Y
            // 
            this.txt_Y.Location = new System.Drawing.Point(147, 78);
            this.txt_Y.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_Y.Name = "txt_Y";
            this.txt_Y.Size = new System.Drawing.Size(212, 27);
            this.txt_Y.TabIndex = 3;
            // 
            // lbl_Y
            // 
            this.lbl_Y.AutoSize = true;
            this.lbl_Y.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lbl_Y.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.lbl_Y.Location = new System.Drawing.Point(13, 81);
            this.lbl_Y.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_Y.Name = "lbl_Y";
            this.lbl_Y.Size = new System.Drawing.Size(96, 19);
            this.lbl_Y.TabIndex = 2;
            this.lbl_Y.Text = "Coordenada Y";
            // 
            // txt_X
            // 
            this.txt_X.Location = new System.Drawing.Point(147, 32);
            this.txt_X.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_X.Name = "txt_X";
            this.txt_X.Size = new System.Drawing.Size(212, 27);
            this.txt_X.TabIndex = 1;
            // 
            // lblX
            // 
            this.lblX.AutoSize = true;
            this.lblX.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblX.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.lblX.Location = new System.Drawing.Point(13, 36);
            this.lblX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(96, 19);
            this.lblX.TabIndex = 0;
            this.lblX.Text = "Coordenada X";
            // 
            // grbProcess
            // 
            this.grbProcess.Controls.Add(this.btnDibujar);
            this.grbProcess.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grbProcess.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.grbProcess.Location = new System.Drawing.Point(24, 283);
            this.grbProcess.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbProcess.Name = "grbProcess";
            this.grbProcess.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbProcess.Size = new System.Drawing.Size(387, 86);
            this.grbProcess.TabIndex = 1;
            this.grbProcess.TabStop = false;
            this.grbProcess.Text = "Acciones";
            // 
            // btnDibujar
            // 
            this.btnDibujar.BackColor = System.Drawing.Color.MediumVioletRed;
            this.btnDibujar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDibujar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDibujar.ForeColor = System.Drawing.Color.White;
            this.btnDibujar.Location = new System.Drawing.Point(113, 27);
            this.btnDibujar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDibujar.Name = "btnDibujar";
            this.btnDibujar.Size = new System.Drawing.Size(160, 37);
            this.btnDibujar.TabIndex = 0;
            this.btnDibujar.Text = "Dibujar (Paramétrico)";
            this.btnDibujar.UseVisualStyleBackColor = false;
            this.btnDibujar.Click += new System.EventHandler(this.btnDibujar_Click);
            // 
            // grbCanvas
            // 
            this.grbCanvas.Controls.Add(this.picCanvas);
            this.grbCanvas.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grbCanvas.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.grbCanvas.Location = new System.Drawing.Point(429, 15);
            this.grbCanvas.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbCanvas.Name = "grbCanvas";
            this.grbCanvas.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbCanvas.Size = new System.Drawing.Size(853, 640);
            this.grbCanvas.TabIndex = 2;
            this.grbCanvas.TabStop = false;
            this.grbCanvas.Text = "Área de Dibujo";
            // 
            // picCanvas
            // 
            this.picCanvas.BackColor = System.Drawing.Color.White;
            this.picCanvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCanvas.Location = new System.Drawing.Point(13, 27);
            this.picCanvas.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picCanvas.Name = "picCanvas";
            this.picCanvas.Size = new System.Drawing.Size(826, 603);
            this.picCanvas.TabIndex = 0;
            this.picCanvas.TabStop = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.lblTitle.Location = new System.Drawing.Point(19, 15);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(382, 32);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Círculo (Algoritmo Paramétrico)";
            // 
            // CPE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(1304, 661);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.grbCanvas);
            this.Controls.Add(this.grbProcess);
            this.Controls.Add(this.grbInputs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1322, 708);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1322, 708);
            this.Name = "CPE";
            this.Text = "Círculo Paramétrico";
            this.Load += new System.EventHandler(this.CPE_Load);
            this.grbInputs.ResumeLayout(false);
            this.grbInputs.PerformLayout();
            this.grbProcess.ResumeLayout(false);
            this.grbCanvas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picCanvas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grbInputs;
        private System.Windows.Forms.TextBox txt_R;
        private System.Windows.Forms.Label lbl_R;
        private System.Windows.Forms.TextBox txt_Y;
        private System.Windows.Forms.Label lbl_Y;
        private System.Windows.Forms.TextBox txt_X;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.GroupBox grbProcess;
        private System.Windows.Forms.Button btnDibujar;
        private System.Windows.Forms.GroupBox grbCanvas;
        private System.Windows.Forms.PictureBox picCanvas;
        private System.Windows.Forms.Label lblTitle;
    }
}