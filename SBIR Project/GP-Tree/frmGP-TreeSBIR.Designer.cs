namespace SBIR_Project.GP_Tree
{
    partial class frmGP_TreeSBIR
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
            this.button1 = new System.Windows.Forms.Button();
            this.btnLoadDBsOntology = new System.Windows.Forms.Button();
            this.btnLoadOntology = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.txtSPARQL = new System.Windows.Forms.TextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lsvClasses = new System.Windows.Forms.ListView();
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.btnImageRetrieval = new System.Windows.Forms.Button();
            this.btnUNIONQueryImg = new System.Windows.Forms.Button();
            this.btnQueryImg = new System.Windows.Forms.Button();
            this.btnUNIONQueryText = new System.Windows.Forms.Button();
            this.btnQueryText = new System.Windows.Forms.Button();
            this.btnSgGPTreeClassification = new System.Windows.Forms.Button();
            this.btnGPTreeClassification = new System.Windows.Forms.Button();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.btnGPTreeGraphClassification = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnLoadImage = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(684, 243);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(138, 23);
            this.button1.TabIndex = 110;
            this.button1.Text = "Refresh";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnLoadDBsOntology
            // 
            this.btnLoadDBsOntology.Location = new System.Drawing.Point(538, 271);
            this.btnLoadDBsOntology.Margin = new System.Windows.Forms.Padding(2);
            this.btnLoadDBsOntology.Name = "btnLoadDBsOntology";
            this.btnLoadDBsOntology.Size = new System.Drawing.Size(140, 23);
            this.btnLoadDBsOntology.TabIndex = 109;
            this.btnLoadDBsOntology.Text = "Load ImageDBs Ontology";
            this.btnLoadDBsOntology.UseVisualStyleBackColor = true;
            this.btnLoadDBsOntology.Click += new System.EventHandler(this.btnLoadDBsOntology_Click);
            // 
            // btnLoadOntology
            // 
            this.btnLoadOntology.Location = new System.Drawing.Point(396, 271);
            this.btnLoadOntology.Margin = new System.Windows.Forms.Padding(2);
            this.btnLoadOntology.Name = "btnLoadOntology";
            this.btnLoadOntology.Size = new System.Drawing.Size(138, 23);
            this.btnLoadOntology.TabIndex = 108;
            this.btnLoadOntology.Text = "Load Ontology";
            this.btnLoadOntology.UseVisualStyleBackColor = true;
            this.btnLoadOntology.Click += new System.EventHandler(this.btnLoadOntology_Click);
            // 
            // txtResult
            // 
            this.txtResult.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtResult.Location = new System.Drawing.Point(396, 347);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(426, 199);
            this.txtResult.TabIndex = 107;
            // 
            // txtSPARQL
            // 
            this.txtSPARQL.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtSPARQL.Location = new System.Drawing.Point(4, 348);
            this.txtSPARQL.Multiline = true;
            this.txtSPARQL.Name = "txtSPARQL";
            this.txtSPARQL.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSPARQL.Size = new System.Drawing.Size(378, 198);
            this.txtSPARQL.TabIndex = 106;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox2.Location = new System.Drawing.Point(212, 9);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(201, 197);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 105;
            this.pictureBox2.TabStop = false;
            // 
            // lsvClasses
            // 
            this.lsvClasses.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsvClasses.HideSelection = false;
            this.lsvClasses.Location = new System.Drawing.Point(421, 123);
            this.lsvClasses.Name = "lsvClasses";
            this.lsvClasses.Size = new System.Drawing.Size(401, 39);
            this.lsvClasses.TabIndex = 104;
            this.lsvClasses.UseCompatibleStateImageBehavior = false;
            this.lsvClasses.View = System.Windows.Forms.View.List;
            this.lsvClasses.SelectedIndexChanged += new System.EventHandler(this.lsvClasses_SelectedIndexChanged);
            // 
            // txtQuery
            // 
            this.txtQuery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQuery.Location = new System.Drawing.Point(418, 186);
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Size = new System.Drawing.Size(404, 20);
            this.txtQuery.TabIndex = 103;
            // 
            // btnImageRetrieval
            // 
            this.btnImageRetrieval.Location = new System.Drawing.Point(684, 216);
            this.btnImageRetrieval.Name = "btnImageRetrieval";
            this.btnImageRetrieval.Size = new System.Drawing.Size(138, 23);
            this.btnImageRetrieval.TabIndex = 102;
            this.btnImageRetrieval.Text = "Ontology Retrieval";
            this.btnImageRetrieval.UseVisualStyleBackColor = true;
            this.btnImageRetrieval.Click += new System.EventHandler(this.btnImageRetrieval_Click);
            // 
            // btnUNIONQueryImg
            // 
            this.btnUNIONQueryImg.Location = new System.Drawing.Point(396, 243);
            this.btnUNIONQueryImg.Name = "btnUNIONQueryImg";
            this.btnUNIONQueryImg.Size = new System.Drawing.Size(138, 23);
            this.btnUNIONQueryImg.TabIndex = 101;
            this.btnUNIONQueryImg.Text = "UNION Query images";
            this.btnUNIONQueryImg.UseVisualStyleBackColor = true;
            this.btnUNIONQueryImg.Click += new System.EventHandler(this.btnUNIONQueryImg_Click);
            // 
            // btnQueryImg
            // 
            this.btnQueryImg.Location = new System.Drawing.Point(396, 216);
            this.btnQueryImg.Name = "btnQueryImg";
            this.btnQueryImg.Size = new System.Drawing.Size(138, 23);
            this.btnQueryImg.TabIndex = 100;
            this.btnQueryImg.Text = "AND Query images";
            this.btnQueryImg.UseVisualStyleBackColor = true;
            this.btnQueryImg.Click += new System.EventHandler(this.btnQueryImg_Click);
            // 
            // btnUNIONQueryText
            // 
            this.btnUNIONQueryText.Location = new System.Drawing.Point(540, 243);
            this.btnUNIONQueryText.Name = "btnUNIONQueryText";
            this.btnUNIONQueryText.Size = new System.Drawing.Size(138, 23);
            this.btnUNIONQueryText.TabIndex = 99;
            this.btnUNIONQueryText.Text = "UNION Query objects";
            this.btnUNIONQueryText.UseVisualStyleBackColor = true;
            this.btnUNIONQueryText.Click += new System.EventHandler(this.btnUNIONQueryText_Click);
            // 
            // btnQueryText
            // 
            this.btnQueryText.Location = new System.Drawing.Point(540, 216);
            this.btnQueryText.Name = "btnQueryText";
            this.btnQueryText.Size = new System.Drawing.Size(138, 23);
            this.btnQueryText.TabIndex = 98;
            this.btnQueryText.Text = "AND Query objects";
            this.btnQueryText.UseVisualStyleBackColor = true;
            this.btnQueryText.Click += new System.EventHandler(this.btnQueryText_Click);
            // 
            // btnSgGPTreeClassification
            // 
            this.btnSgGPTreeClassification.Location = new System.Drawing.Point(205, 270);
            this.btnSgGPTreeClassification.Margin = new System.Windows.Forms.Padding(2);
            this.btnSgGPTreeClassification.Name = "btnSgGPTreeClassification";
            this.btnSgGPTreeClassification.Size = new System.Drawing.Size(177, 23);
            this.btnSgGPTreeClassification.TabIndex = 97;
            this.btnSgGPTreeClassification.Text = "Classification on SgGP-Tree";
            this.btnSgGPTreeClassification.UseVisualStyleBackColor = true;
            this.btnSgGPTreeClassification.Click += new System.EventHandler(this.btnSgGPTreeClassification_Click);
            // 
            // btnGPTreeClassification
            // 
            this.btnGPTreeClassification.Location = new System.Drawing.Point(205, 216);
            this.btnGPTreeClassification.Margin = new System.Windows.Forms.Padding(2);
            this.btnGPTreeClassification.Name = "btnGPTreeClassification";
            this.btnGPTreeClassification.Size = new System.Drawing.Size(177, 23);
            this.btnGPTreeClassification.TabIndex = 95;
            this.btnGPTreeClassification.Text = "Classification on GP-Tree";
            this.btnGPTreeClassification.UseVisualStyleBackColor = true;
            this.btnGPTreeClassification.Click += new System.EventHandler(this.btnGPTreeClassification_Click);
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(101, 11);
            this.radioButton4.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(80, 17);
            this.radioButton4.TabIndex = 3;
            this.radioButton4.Text = "ImageCLEF";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(7, 36);
            this.radioButton3.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(93, 17);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.Text = "Stanford Dogs";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Location = new System.Drawing.Point(7, 17);
            this.radioButton2.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(54, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Wang";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(101, 34);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(61, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.Text = "COREL";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // btnGPTreeGraphClassification
            // 
            this.btnGPTreeGraphClassification.Location = new System.Drawing.Point(205, 243);
            this.btnGPTreeGraphClassification.Margin = new System.Windows.Forms.Padding(2);
            this.btnGPTreeGraphClassification.Name = "btnGPTreeGraphClassification";
            this.btnGPTreeGraphClassification.Size = new System.Drawing.Size(177, 23);
            this.btnGPTreeGraphClassification.TabIndex = 96;
            this.btnGPTreeGraphClassification.Text = "Classification on Graph GP-Tree";
            this.btnGPTreeGraphClassification.UseVisualStyleBackColor = true;
            this.btnGPTreeGraphClassification.Click += new System.EventHandler(this.btnGPTreeGraphClassification_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(6, 297);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(816, 23);
            this.progressBar1.TabIndex = 94;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(684, 271);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(138, 23);
            this.btnClose.TabIndex = 93;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnLoadImage
            // 
            this.btnLoadImage.Location = new System.Drawing.Point(6, 271);
            this.btnLoadImage.Margin = new System.Windows.Forms.Padding(2);
            this.btnLoadImage.Name = "btnLoadImage";
            this.btnLoadImage.Size = new System.Drawing.Size(195, 23);
            this.btnLoadImage.TabIndex = 91;
            this.btnLoadImage.Text = "Load Image";
            this.btnLoadImage.UseVisualStyleBackColor = true;
            this.btnLoadImage.Click += new System.EventHandler(this.btnLoadImage_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(421, 24);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(401, 81);
            this.textBox1.TabIndex = 90;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Location = new System.Drawing.Point(10, 9);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(201, 197);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 89;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton4);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new System.Drawing.Point(6, 211);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(195, 55);
            this.groupBox1.TabIndex = 92;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Image Datasets";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(418, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 115;
            this.label1.Text = "Features:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(418, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 115;
            this.label2.Text = "Classes:";
            this.label2.Click += new System.EventHandler(this.label1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 331);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 115;
            this.label3.Text = "SPARQL Query:";
            this.label3.Click += new System.EventHandler(this.label1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(393, 331);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 115;
            this.label4.Text = "Similar images:";
            this.label4.Click += new System.EventHandler(this.label1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(418, 170);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 13);
            this.label5.TabIndex = 116;
            this.label5.Text = "Query Image Class:";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // frmGP_TreeSBIR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(828, 552);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnLoadDBsOntology);
            this.Controls.Add(this.btnLoadOntology);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btnGPTreeGraphClassification);
            this.Controls.Add(this.txtSPARQL);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.lsvClasses);
            this.Controls.Add(this.txtQuery);
            this.Controls.Add(this.btnImageRetrieval);
            this.Controls.Add(this.btnUNIONQueryImg);
            this.Controls.Add(this.btnQueryImg);
            this.Controls.Add(this.btnUNIONQueryText);
            this.Controls.Add(this.btnQueryText);
            this.Controls.Add(this.btnSgGPTreeClassification);
            this.Controls.Add(this.btnGPTreeClassification);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnLoadImage);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmGP_TreeSBIR";
            this.Text = "SBIR-GP";
            this.Load += new System.EventHandler(this.frmGP_TreeSBIR_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnLoadDBsOntology;
        private System.Windows.Forms.Button btnLoadOntology;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.TextBox txtSPARQL;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ListView lsvClasses;
        private System.Windows.Forms.TextBox txtQuery;
        private System.Windows.Forms.Button btnImageRetrieval;
        private System.Windows.Forms.Button btnUNIONQueryImg;
        private System.Windows.Forms.Button btnQueryImg;
        private System.Windows.Forms.Button btnUNIONQueryText;
        private System.Windows.Forms.Button btnQueryText;
        private System.Windows.Forms.Button btnSgGPTreeClassification;
        private System.Windows.Forms.Button btnGPTreeClassification;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Button btnGPTreeGraphClassification;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnLoadImage;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}