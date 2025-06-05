namespace DSA
{
    partial class FormMain
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
            lblUsername = new Label();
            btnLoadFile = new Button();
            btnVerifySignature = new Button();
            btnGenerateKeys = new Button();
            btnLoadPrivateKey = new Button();
            btnLoadPublicKey = new Button();
            btnExportZip = new Button();
            txtPublicKey = new TextBox();
            txtPrivateKey = new TextBox();
            txtFileDetails = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            txtOutput = new RichTextBox();
            btnSignAndExport = new Button();
            SuspendLayout();
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(10, 563);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(38, 15);
            lblUsername.TabIndex = 0;
            lblUsername.Text = "label1";
            // 
            // btnLoadFile
            // 
            btnLoadFile.Location = new Point(12, 12);
            btnLoadFile.Name = "btnLoadFile";
            btnLoadFile.Size = new Size(101, 23);
            btnLoadFile.TabIndex = 1;
            btnLoadFile.Text = "Načíst soubor";
            btnLoadFile.UseVisualStyleBackColor = true;
            btnLoadFile.Click += btnLoadFile_Click;
            // 
            // btnVerifySignature
            // 
            btnVerifySignature.Location = new Point(119, 12);
            btnVerifySignature.Name = "btnVerifySignature";
            btnVerifySignature.Size = new Size(111, 23);
            btnVerifySignature.TabIndex = 4;
            btnVerifySignature.Text = "Ověření Podpisu";
            btnVerifySignature.UseVisualStyleBackColor = true;
            btnVerifySignature.Click += btnVerifySignature_Click;
            // 
            // btnGenerateKeys
            // 
            btnGenerateKeys.Location = new Point(8, 433);
            btnGenerateKeys.Name = "btnGenerateKeys";
            btnGenerateKeys.Size = new Size(109, 23);
            btnGenerateKeys.TabIndex = 5;
            btnGenerateKeys.Text = "Generate Keys";
            btnGenerateKeys.UseVisualStyleBackColor = true;
            btnGenerateKeys.Click += btnGenerateKeys_Click;
            // 
            // btnLoadPrivateKey
            // 
            btnLoadPrivateKey.Location = new Point(655, 521);
            btnLoadPrivateKey.Name = "btnLoadPrivateKey";
            btnLoadPrivateKey.Size = new Size(109, 23);
            btnLoadPrivateKey.TabIndex = 6;
            btnLoadPrivateKey.Text = "Load Private";
            btnLoadPrivateKey.UseVisualStyleBackColor = true;
            btnLoadPrivateKey.Click += btnLoadPrivateKey_Click;
            // 
            // btnLoadPublicKey
            // 
            btnLoadPublicKey.Location = new Point(655, 477);
            btnLoadPublicKey.Name = "btnLoadPublicKey";
            btnLoadPublicKey.Size = new Size(109, 23);
            btnLoadPublicKey.TabIndex = 7;
            btnLoadPublicKey.Text = "Load Public";
            btnLoadPublicKey.UseVisualStyleBackColor = true;
            btnLoadPublicKey.Click += btnLoadPublicKey_Click;
            // 
            // btnExportZip
            // 
            btnExportZip.Location = new Point(689, 183);
            btnExportZip.Name = "btnExportZip";
            btnExportZip.Size = new Size(75, 23);
            btnExportZip.TabIndex = 8;
            btnExportZip.Text = "Export";
            btnExportZip.UseVisualStyleBackColor = true;
            btnExportZip.Click += btnExportZip_Click;
            // 
            // txtPublicKey
            // 
            txtPublicKey.Location = new Point(10, 477);
            txtPublicKey.Name = "txtPublicKey";
            txtPublicKey.Size = new Size(639, 23);
            txtPublicKey.TabIndex = 10;
            // 
            // txtPrivateKey
            // 
            txtPrivateKey.Location = new Point(10, 521);
            txtPrivateKey.Name = "txtPrivateKey";
            txtPrivateKey.Size = new Size(639, 23);
            txtPrivateKey.TabIndex = 11;
            // 
            // txtFileDetails
            // 
            txtFileDetails.AutoSize = true;
            txtFileDetails.Location = new Point(12, 70);
            txtFileDetails.Name = "txtFileDetails";
            txtFileDetails.Size = new Size(16, 15);
            txtFileDetails.TabIndex = 15;
            txtFileDetails.Text = "...";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 45);
            label1.Name = "label1";
            label1.Size = new Size(94, 15);
            label1.TabIndex = 16;
            label1.Text = "Detaily Souboru:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(10, 459);
            label2.Name = "label2";
            label2.Size = new Size(70, 15);
            label2.TabIndex = 17;
            label2.Text = "Veřejný Klíč:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(10, 503);
            label3.Name = "label3";
            label3.Size = new Size(86, 15);
            label3.TabIndex = 18;
            label3.Text = "Soukromý Klíč:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(536, 12);
            label4.Name = "label4";
            label4.Size = new Size(31, 15);
            label4.TabIndex = 19;
            label4.Text = "Text:";
            // 
            // txtOutput
            // 
            txtOutput.Location = new Point(536, 30);
            txtOutput.Name = "txtOutput";
            txtOutput.Size = new Size(228, 147);
            txtOutput.TabIndex = 20;
            txtOutput.Text = "";
            // 
            // btnSignAndExport
            // 
            btnSignAndExport.Location = new Point(689, 212);
            btnSignAndExport.Name = "btnSignAndExport";
            btnSignAndExport.Size = new Size(75, 23);
            btnSignAndExport.TabIndex = 21;
            btnSignAndExport.Text = "Podepsat";
            btnSignAndExport.UseVisualStyleBackColor = true;
            btnSignAndExport.Click += btnSignAndExport_Click;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(776, 589);
            Controls.Add(btnSignAndExport);
            Controls.Add(txtOutput);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtFileDetails);
            Controls.Add(txtPrivateKey);
            Controls.Add(txtPublicKey);
            Controls.Add(btnExportZip);
            Controls.Add(btnLoadPublicKey);
            Controls.Add(btnLoadPrivateKey);
            Controls.Add(btnGenerateKeys);
            Controls.Add(btnVerifySignature);
            Controls.Add(btnLoadFile);
            Controls.Add(lblUsername);
            Name = "FormMain";
            Text = "FormMain";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblUsername;
        private Button btnLoadFile;
        private Button btnVerifySignature;
        private Button btnGenerateKeys;
        private Button btnLoadPrivateKey;
        private Button btnLoadPublicKey;
        private Button btnExportZip;
        private TextBox txtPublicKey;
        private TextBox txtPrivateKey;
        private Label txtFileDetails;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private RichTextBox txtOutput;
        private Button btnSignAndExport;
    }
}