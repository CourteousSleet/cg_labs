namespace kg2_6
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
            this.cbguro = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tka = new System.Windows.Forms.TextBox();
            this.tkd = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tks = new System.Windows.Forms.TextBox();
            this.tis = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tid = new System.Windows.Forms.TextBox();
            this.tia = new System.Windows.Forms.TextBox();
            this.tpower = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tpos = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.nr = new System.Windows.Forms.NumericUpDown();
            this.nh = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.numericB = new System.Windows.Forms.NumericUpDown();
            this.numericA = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.numericH = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericH)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Location = new System.Drawing.Point(176, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(595, 426);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // cbguro
            // 
            this.cbguro.AutoSize = true;
            this.cbguro.Location = new System.Drawing.Point(15, 280);
            this.cbguro.Name = "cbguro";
            this.cbguro.Size = new System.Drawing.Size(49, 17);
            this.cbguro.TabIndex = 1;
            this.cbguro.Text = "Гуро";
            this.cbguro.UseVisualStyleBackColor = true;
            this.cbguro.CheckedChanged += new System.EventHandler(this.cbguro_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(85, 276);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(85, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Задать";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tka
            // 
            this.tka.Location = new System.Drawing.Point(51, 92);
            this.tka.Name = "tka";
            this.tka.Size = new System.Drawing.Size(119, 20);
            this.tka.TabIndex = 4;
            // 
            // tkd
            // 
            this.tkd.Location = new System.Drawing.Point(51, 118);
            this.tkd.Name = "tkd";
            this.tkd.Size = new System.Drawing.Size(119, 20);
            this.tkd.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "k_a:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "k_d:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 147);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "k_s:";
            // 
            // tks
            // 
            this.tks.Location = new System.Drawing.Point(51, 144);
            this.tks.Name = "tks";
            this.tks.Size = new System.Drawing.Size(119, 20);
            this.tks.TabIndex = 13;
            // 
            // tis
            // 
            this.tis.Location = new System.Drawing.Point(51, 222);
            this.tis.Name = "tis";
            this.tis.Size = new System.Drawing.Size(119, 20);
            this.tis.TabIndex = 19;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 225);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "i_s:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 199);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "i_d:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 173);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "i_a:";
            // 
            // tid
            // 
            this.tid.Location = new System.Drawing.Point(51, 196);
            this.tid.Name = "tid";
            this.tid.Size = new System.Drawing.Size(119, 20);
            this.tid.TabIndex = 15;
            // 
            // tia
            // 
            this.tia.Location = new System.Drawing.Point(51, 170);
            this.tia.Name = "tia";
            this.tia.Size = new System.Drawing.Size(119, 20);
            this.tia.TabIndex = 14;
            // 
            // tpower
            // 
            this.tpower.Location = new System.Drawing.Point(51, 248);
            this.tpower.Name = "tpower";
            this.tpower.Size = new System.Drawing.Size(119, 20);
            this.tpower.TabIndex = 21;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 251);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 13);
            this.label7.TabIndex = 20;
            this.label7.Text = "power:";
            // 
            // tpos
            // 
            this.tpos.Location = new System.Drawing.Point(51, 66);
            this.tpos.Name = "tpos";
            this.tpos.Size = new System.Drawing.Size(119, 20);
            this.tpos.TabIndex = 22;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 69);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "light:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(91, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "Аппроксимация:";
            // 
            // nr
            // 
            this.nr.Location = new System.Drawing.Point(14, 25);
            this.nr.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nr.Name = "nr";
            this.nr.Size = new System.Drawing.Size(75, 20);
            this.nr.TabIndex = 27;
            this.nr.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nr.ValueChanged += new System.EventHandler(this.RebuildMesh);
            // 
            // nh
            // 
            this.nh.Location = new System.Drawing.Point(96, 25);
            this.nh.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nh.Name = "nh";
            this.nh.Size = new System.Drawing.Size(74, 20);
            this.nh.TabIndex = 28;
            this.nh.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.nh.ValueChanged += new System.EventHandler(this.RebuildMesh);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 324);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 13);
            this.label10.TabIndex = 29;
            this.label10.Text = "Полуоси:";
            // 
            // numericB
            // 
            this.numericB.DecimalPlaces = 2;
            this.numericB.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericB.Location = new System.Drawing.Point(94, 340);
            this.numericB.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericB.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericB.Name = "numericB";
            this.numericB.Size = new System.Drawing.Size(74, 20);
            this.numericB.TabIndex = 31;
            this.numericB.Value = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            this.numericB.ValueChanged += new System.EventHandler(this.RebuildMesh);
            // 
            // numericA
            // 
            this.numericA.DecimalPlaces = 2;
            this.numericA.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericA.Location = new System.Drawing.Point(12, 340);
            this.numericA.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericA.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericA.Name = "numericA";
            this.numericA.Size = new System.Drawing.Size(75, 20);
            this.numericA.TabIndex = 30;
            this.numericA.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericA.ValueChanged += new System.EventHandler(this.RebuildMesh);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 368);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(48, 13);
            this.label11.TabIndex = 32;
            this.label11.Text = "Высота:";
            // 
            // numericH
            // 
            this.numericH.DecimalPlaces = 2;
            this.numericH.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericH.Location = new System.Drawing.Point(65, 366);
            this.numericH.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericH.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericH.Name = "numericH";
            this.numericH.Size = new System.Drawing.Size(103, 20);
            this.numericH.TabIndex = 33;
            this.numericH.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericH.ValueChanged += new System.EventHandler(this.RebuildMesh);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(783, 450);
            this.Controls.Add(this.numericH);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.numericB);
            this.Controls.Add(this.numericA);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.nh);
            this.Controls.Add(this.nr);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tpos);
            this.Controls.Add(this.tpower);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tis);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tid);
            this.Controls.Add(this.tia);
            this.Controls.Add(this.tks);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tkd);
            this.Controls.Add(this.tka);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cbguro);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Вариант 6";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericH)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox cbguro;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tka;
        private System.Windows.Forms.TextBox tkd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tks;
        private System.Windows.Forms.TextBox tis;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tid;
        private System.Windows.Forms.TextBox tia;
        private System.Windows.Forms.TextBox tpower;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tpos;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown nr;
        private System.Windows.Forms.NumericUpDown nh;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numericB;
        private System.Windows.Forms.NumericUpDown numericA;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numericH;
    }
}

