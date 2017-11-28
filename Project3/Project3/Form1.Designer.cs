namespace Project3
{
    partial class Form1
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.calculateRadio = new System.Windows.Forms.RadioButton();
            this.predefinedRadio = new System.Windows.Forms.RadioButton();
            this.Gamma = new System.Windows.Forms.NumericUpDown();
            this.WPy = new System.Windows.Forms.NumericUpDown();
            this.BPy = new System.Windows.Forms.NumericUpDown();
            this.GPy = new System.Windows.Forms.NumericUpDown();
            this.RPy = new System.Windows.Forms.NumericUpDown();
            this.WPx = new System.Windows.Forms.NumericUpDown();
            this.BPx = new System.Windows.Forms.NumericUpDown();
            this.GPx = new System.Windows.Forms.NumericUpDown();
            this.RPx = new System.Windows.Forms.NumericUpDown();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Gamma)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WPy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BPy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GPy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RPy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WPx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BPx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GPx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RPx)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(673, 465);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(12, 564);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(370, 247);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(703, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(190, 39);
            this.button2.TabIndex = 5;
            this.button2.Text = "Load Image";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(703, 123);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(190, 40);
            this.button3.TabIndex = 6;
            this.button3.Text = "Calculate";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Location = new System.Drawing.Point(422, 564);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(370, 247);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 8;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Location = new System.Drawing.Point(817, 564);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(370, 247);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox4.TabIndex = 9;
            this.pictureBox4.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "YCbCr",
            "HSV",
            "Lab"});
            this.comboBox1.Location = new System.Drawing.Point(703, 67);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(190, 28);
            this.comboBox1.TabIndex = 10;
            this.comboBox1.Text = "YCbCr";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.calculateRadio);
            this.groupBox1.Controls.Add(this.predefinedRadio);
            this.groupBox1.Controls.Add(this.Gamma);
            this.groupBox1.Controls.Add(this.WPy);
            this.groupBox1.Controls.Add(this.BPy);
            this.groupBox1.Controls.Add(this.GPy);
            this.groupBox1.Controls.Add(this.RPy);
            this.groupBox1.Controls.Add(this.WPx);
            this.groupBox1.Controls.Add(this.BPx);
            this.groupBox1.Controls.Add(this.GPx);
            this.groupBox1.Controls.Add(this.RPx);
            this.groupBox1.Controls.Add(this.comboBox3);
            this.groupBox1.Controls.Add(this.comboBox2);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(926, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(468, 422);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Lab Settings";
            // 
            // calculateRadio
            // 
            this.calculateRadio.AutoSize = true;
            this.calculateRadio.Location = new System.Drawing.Point(26, 366);
            this.calculateRadio.Name = "calculateRadio";
            this.calculateRadio.Size = new System.Drawing.Size(232, 24);
            this.calculateRadio.TabIndex = 21;
            this.calculateRadio.TabStop = true;
            this.calculateRadio.Text = "Calculate from values above";
            this.calculateRadio.UseVisualStyleBackColor = true;
            this.calculateRadio.CheckedChanged += new System.EventHandler(this.calculateRadio_CheckedChanged);
            // 
            // predefinedRadio
            // 
            this.predefinedRadio.AutoSize = true;
            this.predefinedRadio.Checked = true;
            this.predefinedRadio.Location = new System.Drawing.Point(26, 335);
            this.predefinedRadio.Name = "predefinedRadio";
            this.predefinedRadio.Size = new System.Drawing.Size(111, 24);
            this.predefinedRadio.TabIndex = 20;
            this.predefinedRadio.TabStop = true;
            this.predefinedRadio.Text = "Predefined";
            this.predefinedRadio.UseVisualStyleBackColor = true;
            this.predefinedRadio.CheckedChanged += new System.EventHandler(this.predefinedRadio_CheckedChanged);
            // 
            // Gamma
            // 
            this.Gamma.DecimalPlaces = 2;
            this.Gamma.Enabled = false;
            this.Gamma.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.Gamma.Location = new System.Drawing.Point(272, 288);
            this.Gamma.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.Gamma.Name = "Gamma";
            this.Gamma.Size = new System.Drawing.Size(120, 26);
            this.Gamma.TabIndex = 19;
            this.Gamma.Value = new decimal(new int[] {
            22,
            0,
            0,
            65536});
            // 
            // WPy
            // 
            this.WPy.DecimalPlaces = 6;
            this.WPy.Enabled = false;
            this.WPy.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.WPy.Location = new System.Drawing.Point(272, 252);
            this.WPy.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.WPy.Name = "WPy";
            this.WPy.Size = new System.Drawing.Size(120, 26);
            this.WPy.TabIndex = 18;
            this.WPy.Value = new decimal(new int[] {
            329020,
            0,
            0,
            393216});
            // 
            // BPy
            // 
            this.BPy.DecimalPlaces = 6;
            this.BPy.Enabled = false;
            this.BPy.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.BPy.Location = new System.Drawing.Point(272, 218);
            this.BPy.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.BPy.Name = "BPy";
            this.BPy.Size = new System.Drawing.Size(120, 26);
            this.BPy.TabIndex = 17;
            this.BPy.Value = new decimal(new int[] {
            6,
            0,
            0,
            131072});
            // 
            // GPy
            // 
            this.GPy.DecimalPlaces = 6;
            this.GPy.Enabled = false;
            this.GPy.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.GPy.Location = new System.Drawing.Point(272, 185);
            this.GPy.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.GPy.Name = "GPy";
            this.GPy.Size = new System.Drawing.Size(120, 26);
            this.GPy.TabIndex = 16;
            this.GPy.Value = new decimal(new int[] {
            6,
            0,
            0,
            65536});
            // 
            // RPy
            // 
            this.RPy.DecimalPlaces = 6;
            this.RPy.Enabled = false;
            this.RPy.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.RPy.Location = new System.Drawing.Point(272, 149);
            this.RPy.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.RPy.Name = "RPy";
            this.RPy.Size = new System.Drawing.Size(120, 26);
            this.RPy.TabIndex = 15;
            this.RPy.Value = new decimal(new int[] {
            33,
            0,
            0,
            131072});
            // 
            // WPx
            // 
            this.WPx.DecimalPlaces = 6;
            this.WPx.Enabled = false;
            this.WPx.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.WPx.Location = new System.Drawing.Point(146, 252);
            this.WPx.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.WPx.Name = "WPx";
            this.WPx.Size = new System.Drawing.Size(120, 26);
            this.WPx.TabIndex = 14;
            this.WPx.Value = new decimal(new int[] {
            31273,
            0,
            0,
            327680});
            // 
            // BPx
            // 
            this.BPx.DecimalPlaces = 6;
            this.BPx.Enabled = false;
            this.BPx.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.BPx.Location = new System.Drawing.Point(146, 218);
            this.BPx.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.BPx.Name = "BPx";
            this.BPx.Size = new System.Drawing.Size(120, 26);
            this.BPx.TabIndex = 13;
            this.BPx.Value = new decimal(new int[] {
            15,
            0,
            0,
            131072});
            // 
            // GPx
            // 
            this.GPx.DecimalPlaces = 6;
            this.GPx.Enabled = false;
            this.GPx.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.GPx.Location = new System.Drawing.Point(146, 185);
            this.GPx.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.GPx.Name = "GPx";
            this.GPx.Size = new System.Drawing.Size(120, 26);
            this.GPx.TabIndex = 12;
            this.GPx.Value = new decimal(new int[] {
            3,
            0,
            0,
            65536});
            // 
            // RPx
            // 
            this.RPx.DecimalPlaces = 6;
            this.RPx.Enabled = false;
            this.RPx.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.RPx.Location = new System.Drawing.Point(146, 149);
            this.RPx.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.RPx.Name = "RPx";
            this.RPx.Size = new System.Drawing.Size(120, 26);
            this.RPx.TabIndex = 11;
            this.RPx.Value = new decimal(new int[] {
            64,
            0,
            0,
            131072});
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(251, 77);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(121, 28);
            this.comboBox3.TabIndex = 10;
            this.comboBox3.Text = "D65";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(251, 34);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 28);
            this.comboBox2.TabIndex = 9;
            this.comboBox2.Text = "sRGB";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(22, 80);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(156, 20);
            this.label9.TabIndex = 8;
            this.label9.Text = "Predefined illuminant";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(22, 37);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(171, 20);
            this.label8.TabIndex = 7;
            this.label8.Text = "Predefined color profile";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(200, 294);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 20);
            this.label7.TabIndex = 6;
            this.label7.Text = "Gamma";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(311, 121);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 20);
            this.label6.TabIndex = 5;
            this.label6.Text = "y";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(200, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "x";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 254);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "White point";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 220);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Blue primary";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 187);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Green primary";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 151);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Red primary";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1415, 823);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Gamma)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WPy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BPy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GPy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RPy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WPx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BPx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GPx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RPx)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown RPx;
        private System.Windows.Forms.NumericUpDown WPy;
        private System.Windows.Forms.NumericUpDown GPy;
        private System.Windows.Forms.NumericUpDown WPx;
        private System.Windows.Forms.NumericUpDown GPx;
        private System.Windows.Forms.NumericUpDown Gamma;
        private System.Windows.Forms.RadioButton calculateRadio;
        private System.Windows.Forms.RadioButton predefinedRadio;
        private System.Windows.Forms.NumericUpDown RPy;
        private System.Windows.Forms.NumericUpDown BPy;
        private System.Windows.Forms.NumericUpDown BPx;
    }
}

