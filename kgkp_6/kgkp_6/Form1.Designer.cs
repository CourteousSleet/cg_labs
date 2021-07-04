namespace kgkp_6
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.numu = new System.Windows.Forms.NumericUpDown();
            this.numv = new System.Windows.Forms.NumericUpDown();
            this.tp1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tp2 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tp3 = new System.Windows.Forms.TextBox();
            this.tscale = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tp4 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numv)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(135, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(619, 397);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // numu
            // 
            this.numu.Location = new System.Drawing.Point(12, 25);
            this.numu.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numu.Name = "numu";
            this.numu.Size = new System.Drawing.Size(56, 20);
            this.numu.TabIndex = 3;
            this.numu.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numu.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // numv
            // 
            this.numv.Location = new System.Drawing.Point(73, 25);
            this.numv.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numv.Name = "numv";
            this.numv.Size = new System.Drawing.Size(56, 20);
            this.numv.TabIndex = 4;
            this.numv.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numv.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // tp1
            // 
            this.tp1.Location = new System.Drawing.Point(38, 63);
            this.tp1.Name = "tp1";
            this.tp1.Size = new System.Drawing.Size(91, 20);
            this.tp1.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "P1:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "P2:";
            // 
            // tp2
            // 
            this.tp2.Location = new System.Drawing.Point(38, 89);
            this.tp2.Name = "tp2";
            this.tp2.Size = new System.Drawing.Size(91, 20);
            this.tp2.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 118);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(23, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "P3:";
            // 
            // tp3
            // 
            this.tp3.Location = new System.Drawing.Point(38, 115);
            this.tp3.Name = "tp3";
            this.tp3.Size = new System.Drawing.Size(91, 20);
            this.tp3.TabIndex = 10;
            // 
            // tscale
            // 
            this.tscale.Location = new System.Drawing.Point(12, 198);
            this.tscale.Name = "tscale";
            this.tscale.Size = new System.Drawing.Size(114, 20);
            this.tscale.TabIndex = 17;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 182);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(108, 13);
            this.label10.TabIndex = 14;
            this.label10.Text = "Размер кардиоиды:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label11.Location = new System.Drawing.Point(12, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(91, 13);
            this.label11.TabIndex = 19;
            this.label11.Text = "Аппроксимация:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 224);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(114, 23);
            this.button1.TabIndex = 20;
            this.button1.Text = "Применить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 144);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "P4:";
            // 
            // tp4
            // 
            this.tp4.Location = new System.Drawing.Point(38, 141);
            this.tp4.Name = "tp4";
            this.tp4.Size = new System.Drawing.Size(91, 20);
            this.tp4.TabIndex = 21;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(766, 421);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tp4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.tscale);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tp3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tp2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tp1);
            this.Controls.Add(this.numv);
            this.Controls.Add(this.numu);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Вариант 6";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.NumericUpDown numu;
        private System.Windows.Forms.NumericUpDown numv;
        private System.Windows.Forms.TextBox tp1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tp2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tp3;
        private System.Windows.Forms.TextBox tscale;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tp4;
    }
}

