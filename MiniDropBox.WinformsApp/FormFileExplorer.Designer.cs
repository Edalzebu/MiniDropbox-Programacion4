namespace MiniDropBox.WinformsApp
{
    partial class FormFileExplorer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFileExplorer));
            this.btnBack = new System.Windows.Forms.Button();
            this.pnlFileInfo = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.btnRename = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ModifiedDate = new System.Windows.Forms.Label();
            this.Type = new System.Windows.Forms.Label();
            this.NameFile = new System.Windows.Forms.Label();
            this.btnDownload = new System.Windows.Forms.Button();
            this.lblModifiedDate = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.pnlNewFolder = new System.Windows.Forms.Panel();
            this.BtnCreateNewFolder = new System.Windows.Forms.Button();
            this.txtNewFolder = new System.Windows.Forms.TextBox();
            this.btnUpload = new System.Windows.Forms.Button();
            this.BtnNewFolder = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.BtnDownload2 = new System.Windows.Forms.Button();
            this.ListExplorer = new System.Windows.Forms.ListView();
            this.pnlRename = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.textRename = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlFileInfo.SuspendLayout();
            this.pnlNewFolder.SuspendLayout();
            this.pnlRename.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBack
            // 
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.Location = new System.Drawing.Point(-6, -2);
            this.btnBack.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(130, 75);
            this.btnBack.TabIndex = 1;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.BtnBackClick);
            // 
            // pnlFileInfo
            // 
            this.pnlFileInfo.BackColor = System.Drawing.Color.White;
            this.pnlFileInfo.Controls.Add(this.label2);
            this.pnlFileInfo.Controls.Add(this.lblName);
            this.pnlFileInfo.Controls.Add(this.btnRename);
            this.pnlFileInfo.Controls.Add(this.label1);
            this.pnlFileInfo.Controls.Add(this.ModifiedDate);
            this.pnlFileInfo.Controls.Add(this.Type);
            this.pnlFileInfo.Controls.Add(this.NameFile);
            this.pnlFileInfo.Controls.Add(this.btnDownload);
            this.pnlFileInfo.Controls.Add(this.lblModifiedDate);
            this.pnlFileInfo.Controls.Add(this.lblType);
            this.pnlFileInfo.Location = new System.Drawing.Point(-1, 69);
            this.pnlFileInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlFileInfo.Name = "pnlFileInfo";
            this.pnlFileInfo.Size = new System.Drawing.Size(612, 523);
            this.pnlFileInfo.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label2.Location = new System.Drawing.Point(476, 149);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 19);
            this.label2.TabIndex = 12;
            this.label2.Text = "X";
            this.label2.Visible = false;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(156, 180);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(62, 19);
            this.lblName.TabIndex = 10;
            this.lblName.Text = "Nombre:";
            // 
            // btnRename
            // 
            this.btnRename.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRename.Location = new System.Drawing.Point(-5, 0);
            this.btnRename.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(130, 28);
            this.btnRename.TabIndex = 9;
            this.btnRename.Text = "Rename";
            this.btnRename.UseVisualStyleBackColor = true;
            this.btnRename.Click += new System.EventHandler(this.BtnRenameClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(434, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 19);
            this.label1.TabIndex = 8;
            this.label1.Text = "label1";
            // 
            // ModifiedDate
            // 
            this.ModifiedDate.AutoSize = true;
            this.ModifiedDate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ModifiedDate.Location = new System.Drawing.Point(274, 238);
            this.ModifiedDate.Name = "ModifiedDate";
            this.ModifiedDate.Size = new System.Drawing.Size(0, 19);
            this.ModifiedDate.TabIndex = 7;
            // 
            // Type
            // 
            this.Type.AutoSize = true;
            this.Type.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Type.Location = new System.Drawing.Point(274, 209);
            this.Type.Name = "Type";
            this.Type.Size = new System.Drawing.Size(0, 19);
            this.Type.TabIndex = 6;
            // 
            // NameFile
            // 
            this.NameFile.AutoSize = true;
            this.NameFile.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameFile.Location = new System.Drawing.Point(274, 180);
            this.NameFile.Name = "NameFile";
            this.NameFile.Size = new System.Drawing.Size(0, 19);
            this.NameFile.TabIndex = 5;
            // 
            // btnDownload
            // 
            this.btnDownload.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownload.Location = new System.Drawing.Point(182, 313);
            this.btnDownload.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(161, 67);
            this.btnDownload.TabIndex = 4;
            this.btnDownload.Text = "Download!";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.BtnDownloadClick);
            // 
            // lblModifiedDate
            // 
            this.lblModifiedDate.AutoSize = true;
            this.lblModifiedDate.Location = new System.Drawing.Point(156, 237);
            this.lblModifiedDate.Name = "lblModifiedDate";
            this.lblModifiedDate.Size = new System.Drawing.Size(95, 19);
            this.lblModifiedDate.TabIndex = 2;
            this.lblModifiedDate.Text = "ModifiedDate:";
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(156, 208);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(41, 19);
            this.lblType.TabIndex = 1;
            this.lblType.Text = "Type:";
            // 
            // pnlNewFolder
            // 
            this.pnlNewFolder.BackColor = System.Drawing.Color.White;
            this.pnlNewFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlNewFolder.Controls.Add(this.BtnCreateNewFolder);
            this.pnlNewFolder.Controls.Add(this.txtNewFolder);
            this.pnlNewFolder.Location = new System.Drawing.Point(80, 212);
            this.pnlNewFolder.Name = "pnlNewFolder";
            this.pnlNewFolder.Size = new System.Drawing.Size(411, 122);
            this.pnlNewFolder.TabIndex = 8;
            this.pnlNewFolder.Visible = false;
            // 
            // BtnCreateNewFolder
            // 
            this.BtnCreateNewFolder.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCreateNewFolder.Location = new System.Drawing.Point(295, 31);
            this.BtnCreateNewFolder.Name = "BtnCreateNewFolder";
            this.BtnCreateNewFolder.Size = new System.Drawing.Size(75, 48);
            this.BtnCreateNewFolder.TabIndex = 1;
            this.BtnCreateNewFolder.Text = "Create";
            this.BtnCreateNewFolder.UseVisualStyleBackColor = true;
            this.BtnCreateNewFolder.Click += new System.EventHandler(this.BtnCreateNewFolderClick);
            // 
            // txtNewFolder
            // 
            this.txtNewFolder.Location = new System.Drawing.Point(23, 44);
            this.txtNewFolder.Name = "txtNewFolder";
            this.txtNewFolder.Size = new System.Drawing.Size(266, 25);
            this.txtNewFolder.TabIndex = 0;
            // 
            // btnUpload
            // 
            this.btnUpload.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpload.Location = new System.Drawing.Point(122, -2);
            this.btnUpload.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(125, 75);
            this.btnUpload.TabIndex = 4;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.BtnUploadClick);
            // 
            // BtnNewFolder
            // 
            this.BtnNewFolder.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnNewFolder.Location = new System.Drawing.Point(242, -2);
            this.BtnNewFolder.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnNewFolder.Name = "BtnNewFolder";
            this.BtnNewFolder.Size = new System.Drawing.Size(125, 75);
            this.BtnNewFolder.TabIndex = 5;
            this.BtnNewFolder.Text = "New Folder";
            this.BtnNewFolder.UseVisualStyleBackColor = true;
            this.BtnNewFolder.Click += new System.EventHandler(this.BtnNewFolderClick);
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(363, -2);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(125, 75);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.BtnDeleteClick);
            // 
            // BtnDownload2
            // 
            this.BtnDownload2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnDownload2.Location = new System.Drawing.Point(483, -2);
            this.BtnDownload2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnDownload2.Name = "BtnDownload2";
            this.BtnDownload2.Size = new System.Drawing.Size(125, 75);
            this.BtnDownload2.TabIndex = 7;
            this.BtnDownload2.Text = "Download!";
            this.BtnDownload2.UseVisualStyleBackColor = true;
            this.BtnDownload2.Click += new System.EventHandler(this.BtnDownloadClick);
            // 
            // ListExplorer
            // 
            this.ListExplorer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ListExplorer.Location = new System.Drawing.Point(-6, 69);
            this.ListExplorer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ListExplorer.Name = "ListExplorer";
            this.ListExplorer.Size = new System.Drawing.Size(617, 523);
            this.ListExplorer.TabIndex = 0;
            this.ListExplorer.UseCompatibleStateImageBehavior = false;
            this.ListExplorer.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListExplorerMouseDoubleClick);
            // 
            // pnlRename
            // 
            this.pnlRename.BackColor = System.Drawing.Color.White;
            this.pnlRename.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRename.Controls.Add(this.button2);
            this.pnlRename.Controls.Add(this.textRename);
            this.pnlRename.Location = new System.Drawing.Point(80, 212);
            this.pnlRename.Name = "pnlRename";
            this.pnlRename.Size = new System.Drawing.Size(411, 122);
            this.pnlRename.TabIndex = 9;
            this.pnlRename.Visible = false;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(295, 31);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 48);
            this.button2.TabIndex = 1;
            this.button2.Text = "Ok";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2Click);
            // 
            // textRename
            // 
            this.textRename.Location = new System.Drawing.Point(23, 44);
            this.textRename.Name = "textRename";
            this.textRename.Size = new System.Drawing.Size(266, 25);
            this.textRename.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(266, 586);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(336, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // FormFileExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(604, 645);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.BtnDownload2);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.BtnNewFolder);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.ListExplorer);
            this.Controls.Add(this.pnlRename);
            this.Controls.Add(this.pnlNewFolder);
            this.Controls.Add(this.pnlFileInfo);
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFileExplorer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MiniDropbox";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormFileExplorerFormClosed);
            this.Load += new System.EventHandler(this.FormFileExplorerLoad);
            this.pnlFileInfo.ResumeLayout(false);
            this.pnlFileInfo.PerformLayout();
            this.pnlNewFolder.ResumeLayout(false);
            this.pnlNewFolder.PerformLayout();
            this.pnlRename.ResumeLayout(false);
            this.pnlRename.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Panel pnlFileInfo;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label ModifiedDate;
        private System.Windows.Forms.Label Type;
        private System.Windows.Forms.Label NameFile;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Label lblModifiedDate;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button BtnNewFolder;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button BtnDownload2;
        private System.Windows.Forms.Panel pnlNewFolder;
        private System.Windows.Forms.ListView ListExplorer;
        private System.Windows.Forms.Button BtnCreateNewFolder;
        private System.Windows.Forms.TextBox txtNewFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRename;
        private System.Windows.Forms.Panel pnlRename;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textRename;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}