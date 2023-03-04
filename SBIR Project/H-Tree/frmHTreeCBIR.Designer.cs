namespace SBIR_Project.H_Tree
{
    partial class frmHTreeCBIR
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.txtFeatures = new System.Windows.Forms.TextBox();
            this.ptrImageQuery = new System.Windows.Forms.PictureBox();
            this.pnlMidle = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtQueryTime = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtFmeasure = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRecall = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPrecision = new System.Windows.Forms.Label();
            this.btnTestHTree = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnImageRetrieval = new System.Windows.Forms.Button();
            this.btnLoadImage = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdImageCLEF = new System.Windows.Forms.RadioButton();
            this.rdStandfordDog = new System.Windows.Forms.RadioButton();
            this.rdWang = new System.Windows.Forms.RadioButton();
            this.rdCorel = new System.Windows.Forms.RadioButton();
            this.pnlBotton = new System.Windows.Forms.Panel();
            this.progressBarQuery = new System.Windows.Forms.ProgressBar();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptrImageQuery)).BeginInit();
            this.pnlMidle.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnlBotton.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.txtFeatures);
            this.pnlTop.Controls.Add(this.ptrImageQuery);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1063, 211);
            this.pnlTop.TabIndex = 40;
            // 
            // txtFeatures
            // 
            this.txtFeatures.Location = new System.Drawing.Point(226, 9);
            this.txtFeatures.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtFeatures.Multiline = true;
            this.txtFeatures.Name = "txtFeatures";
            this.txtFeatures.ReadOnly = true;
            this.txtFeatures.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtFeatures.Size = new System.Drawing.Size(818, 197);
            this.txtFeatures.TabIndex = 23;
            // 
            // ptrImageQuery
            // 
            this.ptrImageQuery.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ptrImageQuery.Location = new System.Drawing.Point(9, 9);
            this.ptrImageQuery.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ptrImageQuery.Name = "ptrImageQuery";
            this.ptrImageQuery.Size = new System.Drawing.Size(202, 197);
            this.ptrImageQuery.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptrImageQuery.TabIndex = 22;
            this.ptrImageQuery.TabStop = false;
            // 
            // pnlMidle
            // 
            this.pnlMidle.Controls.Add(this.btnClose);
            this.pnlMidle.Controls.Add(this.txtQueryTime);
            this.pnlMidle.Controls.Add(this.label7);
            this.pnlMidle.Controls.Add(this.txtFmeasure);
            this.pnlMidle.Controls.Add(this.label5);
            this.pnlMidle.Controls.Add(this.txtRecall);
            this.pnlMidle.Controls.Add(this.label3);
            this.pnlMidle.Controls.Add(this.txtPrecision);
            this.pnlMidle.Controls.Add(this.btnTestHTree);
            this.pnlMidle.Controls.Add(this.label1);
            this.pnlMidle.Controls.Add(this.btnImageRetrieval);
            this.pnlMidle.Controls.Add(this.btnLoadImage);
            this.pnlMidle.Controls.Add(this.groupBox1);
            this.pnlMidle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMidle.Location = new System.Drawing.Point(0, 211);
            this.pnlMidle.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlMidle.Name = "pnlMidle";
            this.pnlMidle.Size = new System.Drawing.Size(1063, 81);
            this.pnlMidle.TabIndex = 41;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(968, 47);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(63, 23);
            this.btnClose.TabIndex = 51;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtQueryTime
            // 
            this.txtQueryTime.AutoSize = true;
            this.txtQueryTime.Location = new System.Drawing.Point(800, 20);
            this.txtQueryTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.txtQueryTime.Name = "txtQueryTime";
            this.txtQueryTime.Size = new System.Drawing.Size(35, 13);
            this.txtQueryTime.TabIndex = 50;
            this.txtQueryTime.Text = "label8";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(713, 20);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 13);
            this.label7.TabIndex = 49;
            this.label7.Text = "Query time (ms):";
            // 
            // txtFmeasure
            // 
            this.txtFmeasure.AutoSize = true;
            this.txtFmeasure.Location = new System.Drawing.Point(644, 20);
            this.txtFmeasure.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.txtFmeasure.Name = "txtFmeasure";
            this.txtFmeasure.Size = new System.Drawing.Size(35, 13);
            this.txtFmeasure.TabIndex = 48;
            this.txtFmeasure.Text = "label6";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(580, 20);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 47;
            this.label5.Text = "F-measure:";
            // 
            // txtRecall
            // 
            this.txtRecall.AutoSize = true;
            this.txtRecall.Location = new System.Drawing.Point(523, 20);
            this.txtRecall.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.txtRecall.Name = "txtRecall";
            this.txtRecall.Size = new System.Drawing.Size(35, 13);
            this.txtRecall.TabIndex = 46;
            this.txtRecall.Text = "label4";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(481, 20);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 45;
            this.label3.Text = "Recall:";
            // 
            // txtPrecision
            // 
            this.txtPrecision.AutoSize = true;
            this.txtPrecision.Location = new System.Drawing.Point(404, 20);
            this.txtPrecision.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.txtPrecision.Name = "txtPrecision";
            this.txtPrecision.Size = new System.Drawing.Size(35, 13);
            this.txtPrecision.TabIndex = 44;
            this.txtPrecision.Text = "label2";
            // 
            // btnTestHTree
            // 
            this.btnTestHTree.Location = new System.Drawing.Point(368, 46);
            this.btnTestHTree.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnTestHTree.Name = "btnTestHTree";
            this.btnTestHTree.Size = new System.Drawing.Size(122, 23);
            this.btnTestHTree.TabIndex = 43;
            this.btnTestHTree.Text = "Testing H-Tree";
            this.btnTestHTree.UseVisualStyleBackColor = true;
            this.btnTestHTree.Click += new System.EventHandler(this.btnTestHTree_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(349, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 42;
            this.label1.Text = "Precision:";
            // 
            // btnImageRetrieval
            // 
            this.btnImageRetrieval.Location = new System.Drawing.Point(187, 46);
            this.btnImageRetrieval.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnImageRetrieval.Name = "btnImageRetrieval";
            this.btnImageRetrieval.Size = new System.Drawing.Size(127, 23);
            this.btnImageRetrieval.TabIndex = 40;
            this.btnImageRetrieval.Text = "H-Tree Retrieval";
            this.btnImageRetrieval.UseVisualStyleBackColor = true;
            this.btnImageRetrieval.Click += new System.EventHandler(this.btnImageRetrieval_Click);
            // 
            // btnLoadImage
            // 
            this.btnLoadImage.Location = new System.Drawing.Point(11, 46);
            this.btnLoadImage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnLoadImage.Name = "btnLoadImage";
            this.btnLoadImage.Size = new System.Drawing.Size(122, 23);
            this.btnLoadImage.TabIndex = 39;
            this.btnLoadImage.Text = "Load Image";
            this.btnLoadImage.UseVisualStyleBackColor = true;
            this.btnLoadImage.Click += new System.EventHandler(this.btnLoadImage_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdImageCLEF);
            this.groupBox1.Controls.Add(this.rdStandfordDog);
            this.groupBox1.Controls.Add(this.rdWang);
            this.groupBox1.Controls.Add(this.rdCorel);
            this.groupBox1.Location = new System.Drawing.Point(8, 5);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(306, 38);
            this.groupBox1.TabIndex = 41;
            this.groupBox1.TabStop = false;
            // 
            // rdImageCLEF
            // 
            this.rdImageCLEF.AutoSize = true;
            this.rdImageCLEF.Location = new System.Drawing.Point(239, 15);
            this.rdImageCLEF.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rdImageCLEF.Name = "rdImageCLEF";
            this.rdImageCLEF.Size = new System.Drawing.Size(80, 17);
            this.rdImageCLEF.TabIndex = 3;
            this.rdImageCLEF.TabStop = true;
            this.rdImageCLEF.Text = "ImageCLEF";
            this.rdImageCLEF.UseVisualStyleBackColor = true;
            // 
            // rdStandfordDog
            // 
            this.rdStandfordDog.AutoSize = true;
            this.rdStandfordDog.Location = new System.Drawing.Point(146, 15);
            this.rdStandfordDog.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rdStandfordDog.Name = "rdStandfordDog";
            this.rdStandfordDog.Size = new System.Drawing.Size(93, 17);
            this.rdStandfordDog.TabIndex = 2;
            this.rdStandfordDog.Text = "Stanford Dogs";
            this.rdStandfordDog.UseVisualStyleBackColor = true;
            // 
            // rdWang
            // 
            this.rdWang.AutoSize = true;
            this.rdWang.Location = new System.Drawing.Point(75, 15);
            this.rdWang.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rdWang.Name = "rdWang";
            this.rdWang.Size = new System.Drawing.Size(54, 17);
            this.rdWang.TabIndex = 1;
            this.rdWang.Text = "Wang";
            this.rdWang.UseVisualStyleBackColor = true;
            // 
            // rdCorel
            // 
            this.rdCorel.AutoSize = true;
            this.rdCorel.Checked = true;
            this.rdCorel.Location = new System.Drawing.Point(4, 15);
            this.rdCorel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rdCorel.Name = "rdCorel";
            this.rdCorel.Size = new System.Drawing.Size(61, 17);
            this.rdCorel.TabIndex = 0;
            this.rdCorel.TabStop = true;
            this.rdCorel.Text = "COREL";
            this.rdCorel.UseVisualStyleBackColor = true;
            // 
            // pnlBotton
            // 
            this.pnlBotton.Controls.Add(this.progressBarQuery);
            this.pnlBotton.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBotton.Location = new System.Drawing.Point(0, 292);
            this.pnlBotton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlBotton.Name = "pnlBotton";
            this.pnlBotton.Size = new System.Drawing.Size(1063, 38);
            this.pnlBotton.TabIndex = 42;
            // 
            // progressBarQuery
            // 
            this.progressBarQuery.Location = new System.Drawing.Point(9, 7);
            this.progressBarQuery.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.progressBarQuery.Name = "progressBarQuery";
            this.progressBarQuery.Size = new System.Drawing.Size(1036, 23);
            this.progressBarQuery.TabIndex = 40;
            // 
            // frmHTreeCBIR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1063, 340);
            this.Controls.Add(this.pnlBotton);
            this.Controls.Add(this.pnlMidle);
            this.Controls.Add(this.pnlTop);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "frmHTreeCBIR";
            this.Text = "frmHTreeCBIR";
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptrImageQuery)).EndInit();
            this.pnlMidle.ResumeLayout(false);
            this.pnlMidle.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnlBotton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.TextBox txtFeatures;
        private System.Windows.Forms.PictureBox ptrImageQuery;
        private System.Windows.Forms.Panel pnlMidle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label txtQueryTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label txtFmeasure;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label txtRecall;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label txtPrecision;
        private System.Windows.Forms.Button btnTestHTree;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnImageRetrieval;
        private System.Windows.Forms.Button btnLoadImage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdImageCLEF;
        private System.Windows.Forms.RadioButton rdStandfordDog;
        private System.Windows.Forms.RadioButton rdWang;
        private System.Windows.Forms.RadioButton rdCorel;
        private System.Windows.Forms.Panel pnlBotton;
        private System.Windows.Forms.ProgressBar progressBarQuery;
    }
}