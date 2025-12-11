namespace WinAPPLineas
{
    partial class Circulo
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
            this.txt_R = new System.Windows.Forms.TextBox();
            this.lbl_R = new System.Windows.Forms.Label();
            this.txt_Y = new System.Windows.Forms.TextBox();
            this.lbl_Y = new System.Windows.Forms.Label();
            this.txt_X = new System.Windows.Forms.TextBox();
            this.lblX = new System.Windows.Forms.Label();
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
            this.grbProcess.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.grbProcess.Location = new System.Drawing.Point(19, 257);
            this.grbProcess.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbProcess.Name = "grbProcess";
            this.grbProcess.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbProcess.Size = new System.Drawing.Size(387, 86);
            this.grbProcess.TabIndex = 23;
            this.grbProcess.TabStop = false;
            this.grbProcess.Text = "Procesos";
            // 
            // btnDibujar
            // 
            this.btnDibujar.BackColor = System.Drawing.Color.Teal;
            this.btnDibujar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDibujar.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.btnDibujar.ForeColor = System.Drawing.Color.White;
            this.btnDibujar.Location = new System.Drawing.Point(127, 27);
            this.btnDibujar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDibujar.Name = "btnDibujar";
            this.btnDibujar.Size = new System.Drawing.Size(147, 37);
            this.btnDibujar.TabIndex = 0;
            this.btnDibujar.Text = "Dibujar MPC";
            this.btnDibujar.UseVisualStyleBackColor = false;
            this.btnDibujar.Click += new System.EventHandler(this.btnDibujar_Click);
            // 
            // grbInputs
            // 
            this.grbInputs.Controls.Add(this.txt_R);
            this.grbInputs.Controls.Add(this.lbl_R);
            this.grbInputs.Controls.Add(this.txt_Y);
            this.grbInputs.Controls.Add(this.lbl_Y);
            this.grbInputs.Controls.Add(this.txt_X);
            this.grbInputs.Controls.Add(this.lblX);
            this.grbInputs.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.grbInputs.Location = new System.Drawing.Point(11, 38);
            this.grbInputs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbInputs.Name = "grbInputs";
            this.grbInputs.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbInputs.Size = new System.Drawing.Size(387, 185);
            this.grbInputs.TabIndex = 22;
            this.grbInputs.TabStop = false;
            this.grbInputs.Text = "Entradas";
            // 
            // txt_R
            // 
            this.txt_R.Location = new System.Drawing.Point(149, 110);
            this.txt_R.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_R.Name = "txt_R";
            this.txt_R.Size = new System.Drawing.Size(183, 26);
            this.txt_R.TabIndex = 8;
            // 
            // lbl_R
            // 
            this.lbl_R.AutoSize = true;
            this.lbl_R.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.lbl_R.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lbl_R.Location = new System.Drawing.Point(11, 119);
            this.lbl_R.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_R.Name = "lbl_R";
            this.lbl_R.Size = new System.Drawing.Size(117, 17);
            this.lbl_R.TabIndex = 7;
            this.lbl_R.Text = "Ingrese el radio";
            // 
            // txt_Y
            // 
            this.txt_Y.Location = new System.Drawing.Point(149, 78);
            this.txt_Y.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_Y.Name = "txt_Y";
            this.txt_Y.Size = new System.Drawing.Size(183, 26);
            this.txt_Y.TabIndex = 6;
            // 
            // lbl_Y
            // 
            this.lbl_Y.AutoSize = true;
            this.lbl_Y.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.lbl_Y.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lbl_Y.Location = new System.Drawing.Point(11, 81);
            this.lbl_Y.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_Y.Name = "lbl_Y";
            this.lbl_Y.Size = new System.Drawing.Size(75, 17);
            this.lbl_Y.TabIndex = 3;
            this.lbl_Y.Text = "Ingrese Y";
            // 
            // txt_X
            // 
            this.txt_X.Location = new System.Drawing.Point(149, 42);
            this.txt_X.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_X.Name = "txt_X";
            this.txt_X.Size = new System.Drawing.Size(183, 26);
            this.txt_X.TabIndex = 2;
            // 
            // lblX
            // 
            this.lblX.AutoSize = true;
            this.lblX.BackColor = System.Drawing.Color.Transparent;
            this.lblX.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.lblX.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblX.Location = new System.Drawing.Point(8, 46);
            this.lblX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(76, 17);
            this.lblX.TabIndex = 0;
            this.lblX.Text = "Ingrese X";
            // 
            // grbCanvas
            // 
            this.grbCanvas.Controls.Add(this.picCanvas);
            this.grbCanvas.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.grbCanvas.Location = new System.Drawing.Point(405, 15);
            this.grbCanvas.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbCanvas.MaximumSize = new System.Drawing.Size(1328, 719);
            this.grbCanvas.MinimumSize = new System.Drawing.Size(1328, 719);
            this.grbCanvas.Name = "grbCanvas";
            this.grbCanvas.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grbCanvas.Size = new System.Drawing.Size(1328, 719);
            this.grbCanvas.TabIndex = 21;
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
            // Circulo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Honeydew;
            this.ClientSize = new System.Drawing.Size(1304, 661);
            this.Controls.Add(this.grbProcess);
            this.Controls.Add(this.grbInputs);
            this.Controls.Add(this.grbCanvas);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1322, 708);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1322, 708);
            this.Name = "Circulo";
            this.Text = "Círculo Midpoint";
            this.Load += new System.EventHandler(this.Circulo_Load);
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
        private System.Windows.Forms.TextBox txt_Y;
        private System.Windows.Forms.Label lbl_Y;
        private System.Windows.Forms.TextBox txt_X;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.GroupBox grbCanvas;
        private System.Windows.Forms.PictureBox picCanvas;
        private System.Windows.Forms.TextBox txt_R;
        private System.Windows.Forms.Label lbl_R;
    }
}