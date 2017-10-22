namespace Project1
{
    partial class MainWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.mainPictureBox = new System.Windows.Forms.PictureBox();
            this.pointMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deletePointStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lineMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addRestrictionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lengthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verticalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.horizontalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).BeginInit();
            this.pointMenuStrip.SuspendLayout();
            this.lineMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPictureBox
            // 
            this.mainPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("mainPictureBox.Image")));
            this.mainPictureBox.Location = new System.Drawing.Point(171, 91);
            this.mainPictureBox.Name = "mainPictureBox";
            this.mainPictureBox.Size = new System.Drawing.Size(559, 539);
            this.mainPictureBox.TabIndex = 0;
            this.mainPictureBox.TabStop = false;
            // 
            // pointMenuStrip
            // 
            this.pointMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.pointMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deletePointStripMenuItem});
            this.pointMenuStrip.Name = "pointMenuStrip";
            this.pointMenuStrip.Size = new System.Drawing.Size(182, 34);
            // 
            // deletePointStripMenuItem
            // 
            this.deletePointStripMenuItem.Name = "deletePointStripMenuItem";
            this.deletePointStripMenuItem.Size = new System.Drawing.Size(181, 30);
            this.deletePointStripMenuItem.Text = "Delete point";
            this.deletePointStripMenuItem.Click += new System.EventHandler(this.deletePointStripMenuItem_Click);
            // 
            // lineMenuStrip
            // 
            this.lineMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.lineMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPointToolStripMenuItem,
            this.removeLineToolStripMenuItem,
            this.addRestrictionToolStripMenuItem});
            this.lineMenuStrip.Name = "lineMenuStrip";
            this.lineMenuStrip.Size = new System.Drawing.Size(202, 127);
            // 
            // addPointToolStripMenuItem
            // 
            this.addPointToolStripMenuItem.Name = "addPointToolStripMenuItem";
            this.addPointToolStripMenuItem.Size = new System.Drawing.Size(201, 30);
            this.addPointToolStripMenuItem.Text = "Add point";
            this.addPointToolStripMenuItem.Click += new System.EventHandler(this.addPointToolStripMenuItem_Click);
            // 
            // removeLineToolStripMenuItem
            // 
            this.removeLineToolStripMenuItem.Name = "removeLineToolStripMenuItem";
            this.removeLineToolStripMenuItem.Size = new System.Drawing.Size(201, 30);
            this.removeLineToolStripMenuItem.Text = "Remove line";
            this.removeLineToolStripMenuItem.Click += new System.EventHandler(this.removeLineToolStripMenuItem_Click);
            // 
            // addRestrictionToolStripMenuItem
            // 
            this.addRestrictionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lengthToolStripMenuItem,
            this.verticalToolStripMenuItem,
            this.horizontalToolStripMenuItem});
            this.addRestrictionToolStripMenuItem.Name = "addRestrictionToolStripMenuItem";
            this.addRestrictionToolStripMenuItem.Size = new System.Drawing.Size(201, 30);
            this.addRestrictionToolStripMenuItem.Text = "Add restriction";
            // 
            // lengthToolStripMenuItem
            // 
            this.lengthToolStripMenuItem.Name = "lengthToolStripMenuItem";
            this.lengthToolStripMenuItem.Size = new System.Drawing.Size(210, 30);
            this.lengthToolStripMenuItem.Text = "Length";
            this.lengthToolStripMenuItem.Click += new System.EventHandler(this.lengthToolStripMenuItem_Click);
            // 
            // verticalToolStripMenuItem
            // 
            this.verticalToolStripMenuItem.Name = "verticalToolStripMenuItem";
            this.verticalToolStripMenuItem.Size = new System.Drawing.Size(210, 30);
            this.verticalToolStripMenuItem.Text = "Vertical";
            this.verticalToolStripMenuItem.Click += new System.EventHandler(this.verticalToolStripMenuItem_Click);
            // 
            // horizontalToolStripMenuItem
            // 
            this.horizontalToolStripMenuItem.Name = "horizontalToolStripMenuItem";
            this.horizontalToolStripMenuItem.Size = new System.Drawing.Size(210, 30);
            this.horizontalToolStripMenuItem.Text = "Horizontal";
            this.horizontalToolStripMenuItem.Click += new System.EventHandler(this.horizontalToolStripMenuItem_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 839);
            this.Controls.Add(this.mainPictureBox);
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).EndInit();
            this.pointMenuStrip.ResumeLayout(false);
            this.lineMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox mainPictureBox;
        private System.Windows.Forms.ContextMenuStrip pointMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem deletePointStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip lineMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addPointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addRestrictionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lengthToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verticalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem horizontalToolStripMenuItem;
    }
}

