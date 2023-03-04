namespace SBIR
{
    partial class frmSBIR
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSBIR));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.imageProcessingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageFeatureaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractFeaturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extract390FeaturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataProcessingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getImageClassesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getImagePathsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createAnnotationFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameImageFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gPTreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createGPTreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usingGPTreeCBIRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usingGPTreeSBIRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clusteringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kMeansToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.meanShiftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.heirachicalClusteringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sOMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.classificationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kNNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sVMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aNNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deepLearningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convolutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convolutionNetworkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cNNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cNNToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.createOntologyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getClassesInImageDBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getSubClassesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getClassesAndImagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getClassesLiteralsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getDataPropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createImageLiteralsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createOntologyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.imageProcessingToolStripMenuItem,
            this.imageFeatureaToolStripMenuItem,
            this.dataProcessingToolStripMenuItem,
            this.gPTreeToolStripMenuItem,
            this.clusteringToolStripMenuItem,
            this.classificationToolStripMenuItem,
            this.deepLearningToolStripMenuItem,
            this.createOntologyToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(720, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // imageProcessingToolStripMenuItem
            // 
            this.imageProcessingToolStripMenuItem.Name = "imageProcessingToolStripMenuItem";
            this.imageProcessingToolStripMenuItem.Size = new System.Drawing.Size(112, 20);
            this.imageProcessingToolStripMenuItem.Text = "Image Processing";
            // 
            // imageFeatureaToolStripMenuItem
            // 
            this.imageFeatureaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extractFeaturesToolStripMenuItem,
            this.extract390FeaturesToolStripMenuItem});
            this.imageFeatureaToolStripMenuItem.Name = "imageFeatureaToolStripMenuItem";
            this.imageFeatureaToolStripMenuItem.Size = new System.Drawing.Size(99, 20);
            this.imageFeatureaToolStripMenuItem.Text = "Image Features";
            // 
            // extractFeaturesToolStripMenuItem
            // 
            this.extractFeaturesToolStripMenuItem.Name = "extractFeaturesToolStripMenuItem";
            this.extractFeaturesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.extractFeaturesToolStripMenuItem.Text = "Extract features";
            this.extractFeaturesToolStripMenuItem.Click += new System.EventHandler(this.extractFeaturesToolStripMenuItem_Click);
            // 
            // extract390FeaturesToolStripMenuItem
            // 
            this.extract390FeaturesToolStripMenuItem.Name = "extract390FeaturesToolStripMenuItem";
            this.extract390FeaturesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.extract390FeaturesToolStripMenuItem.Text = "Extract 390 features";
            this.extract390FeaturesToolStripMenuItem.Click += new System.EventHandler(this.extract390FeaturesToolStripMenuItem_Click);
            // 
            // dataProcessingToolStripMenuItem
            // 
            this.dataProcessingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.getImageClassesToolStripMenuItem,
            this.getImagePathsToolStripMenuItem,
            this.createAnnotationFilesToolStripMenuItem,
            this.renameImageFilesToolStripMenuItem});
            this.dataProcessingToolStripMenuItem.Name = "dataProcessingToolStripMenuItem";
            this.dataProcessingToolStripMenuItem.Size = new System.Drawing.Size(103, 20);
            this.dataProcessingToolStripMenuItem.Text = "Data Processing";
            // 
            // getImageClassesToolStripMenuItem
            // 
            this.getImageClassesToolStripMenuItem.Name = "getImageClassesToolStripMenuItem";
            this.getImageClassesToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.getImageClassesToolStripMenuItem.Text = "get Image Classes";
            this.getImageClassesToolStripMenuItem.Click += new System.EventHandler(this.getImageClassesToolStripMenuItem_Click);
            // 
            // getImagePathsToolStripMenuItem
            // 
            this.getImagePathsToolStripMenuItem.Name = "getImagePathsToolStripMenuItem";
            this.getImagePathsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.getImagePathsToolStripMenuItem.Text = "get Image Paths";
            this.getImagePathsToolStripMenuItem.Click += new System.EventHandler(this.getImagePathsToolStripMenuItem_Click);
            // 
            // createAnnotationFilesToolStripMenuItem
            // 
            this.createAnnotationFilesToolStripMenuItem.Name = "createAnnotationFilesToolStripMenuItem";
            this.createAnnotationFilesToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.createAnnotationFilesToolStripMenuItem.Text = "create Annotation Files";
            this.createAnnotationFilesToolStripMenuItem.Click += new System.EventHandler(this.createAnnotationFilesToolStripMenuItem_Click);
            // 
            // renameImageFilesToolStripMenuItem
            // 
            this.renameImageFilesToolStripMenuItem.Name = "renameImageFilesToolStripMenuItem";
            this.renameImageFilesToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.renameImageFilesToolStripMenuItem.Text = "rename Image Files";
            this.renameImageFilesToolStripMenuItem.Click += new System.EventHandler(this.renameImageFilesToolStripMenuItem_Click);
            // 
            // gPTreeToolStripMenuItem
            // 
            this.gPTreeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createGPTreeToolStripMenuItem,
            this.usingGPTreeCBIRToolStripMenuItem,
            this.usingGPTreeSBIRToolStripMenuItem});
            this.gPTreeToolStripMenuItem.Name = "gPTreeToolStripMenuItem";
            this.gPTreeToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.gPTreeToolStripMenuItem.Text = "GP-Tree";
            // 
            // createGPTreeToolStripMenuItem
            // 
            this.createGPTreeToolStripMenuItem.Name = "createGPTreeToolStripMenuItem";
            this.createGPTreeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.createGPTreeToolStripMenuItem.Text = "Create GP-Tree";
            this.createGPTreeToolStripMenuItem.Click += new System.EventHandler(this.createGPTreeToolStripMenuItem_Click);
            // 
            // usingGPTreeCBIRToolStripMenuItem
            // 
            this.usingGPTreeCBIRToolStripMenuItem.Name = "usingGPTreeCBIRToolStripMenuItem";
            this.usingGPTreeCBIRToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.usingGPTreeCBIRToolStripMenuItem.Text = "Using GP-Tree CBIR";
            this.usingGPTreeCBIRToolStripMenuItem.Click += new System.EventHandler(this.usingGPTreeCBIRToolStripMenuItem_Click);
            // 
            // usingGPTreeSBIRToolStripMenuItem
            // 
            this.usingGPTreeSBIRToolStripMenuItem.Name = "usingGPTreeSBIRToolStripMenuItem";
            this.usingGPTreeSBIRToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.usingGPTreeSBIRToolStripMenuItem.Text = "Using GP-Tree SBIR";
            this.usingGPTreeSBIRToolStripMenuItem.Click += new System.EventHandler(this.usingGPTreeSBIRToolStripMenuItem_Click);
            // 
            // clusteringToolStripMenuItem
            // 
            this.clusteringToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kMeansToolStripMenuItem,
            this.meanShiftToolStripMenuItem,
            this.heirachicalClusteringToolStripMenuItem,
            this.sOMToolStripMenuItem});
            this.clusteringToolStripMenuItem.Name = "clusteringToolStripMenuItem";
            this.clusteringToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.clusteringToolStripMenuItem.Text = "Clustering";
            // 
            // kMeansToolStripMenuItem
            // 
            this.kMeansToolStripMenuItem.Name = "kMeansToolStripMenuItem";
            this.kMeansToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.kMeansToolStripMenuItem.Text = "K-Means";
            // 
            // meanShiftToolStripMenuItem
            // 
            this.meanShiftToolStripMenuItem.Name = "meanShiftToolStripMenuItem";
            this.meanShiftToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.meanShiftToolStripMenuItem.Text = "MeanShift";
            // 
            // heirachicalClusteringToolStripMenuItem
            // 
            this.heirachicalClusteringToolStripMenuItem.Name = "heirachicalClusteringToolStripMenuItem";
            this.heirachicalClusteringToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.heirachicalClusteringToolStripMenuItem.Text = "Heirachical Clustering";
            // 
            // sOMToolStripMenuItem
            // 
            this.sOMToolStripMenuItem.Name = "sOMToolStripMenuItem";
            this.sOMToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.sOMToolStripMenuItem.Text = "SOM";
            // 
            // classificationToolStripMenuItem
            // 
            this.classificationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kNNToolStripMenuItem,
            this.sVMToolStripMenuItem,
            this.aNNToolStripMenuItem});
            this.classificationToolStripMenuItem.Name = "classificationToolStripMenuItem";
            this.classificationToolStripMenuItem.Size = new System.Drawing.Size(89, 20);
            this.classificationToolStripMenuItem.Text = "Classification";
            // 
            // kNNToolStripMenuItem
            // 
            this.kNNToolStripMenuItem.Name = "kNNToolStripMenuItem";
            this.kNNToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.kNNToolStripMenuItem.Text = "k-NN";
            // 
            // sVMToolStripMenuItem
            // 
            this.sVMToolStripMenuItem.Name = "sVMToolStripMenuItem";
            this.sVMToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.sVMToolStripMenuItem.Text = "SVM";
            // 
            // aNNToolStripMenuItem
            // 
            this.aNNToolStripMenuItem.Name = "aNNToolStripMenuItem";
            this.aNNToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.aNNToolStripMenuItem.Text = "ANN";
            // 
            // deepLearningToolStripMenuItem
            // 
            this.deepLearningToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.convolutionToolStripMenuItem,
            this.convolutionNetworkToolStripMenuItem,
            this.cNNToolStripMenuItem,
            this.cNNToolStripMenuItem1});
            this.deepLearningToolStripMenuItem.Name = "deepLearningToolStripMenuItem";
            this.deepLearningToolStripMenuItem.Size = new System.Drawing.Size(95, 20);
            this.deepLearningToolStripMenuItem.Text = "Deep Learning";
            // 
            // convolutionToolStripMenuItem
            // 
            this.convolutionToolStripMenuItem.Name = "convolutionToolStripMenuItem";
            this.convolutionToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.convolutionToolStripMenuItem.Text = "Convolution";
            // 
            // convolutionNetworkToolStripMenuItem
            // 
            this.convolutionNetworkToolStripMenuItem.Name = "convolutionNetworkToolStripMenuItem";
            this.convolutionNetworkToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.convolutionNetworkToolStripMenuItem.Text = "Convolution Network";
            // 
            // cNNToolStripMenuItem
            // 
            this.cNNToolStripMenuItem.Name = "cNNToolStripMenuItem";
            this.cNNToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.cNNToolStripMenuItem.Text = "DNN";
            // 
            // cNNToolStripMenuItem1
            // 
            this.cNNToolStripMenuItem1.Name = "cNNToolStripMenuItem1";
            this.cNNToolStripMenuItem1.Size = new System.Drawing.Size(188, 22);
            this.cNNToolStripMenuItem1.Text = "CNN";
            // 
            // createOntologyToolStripMenuItem
            // 
            this.createOntologyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.getClassesInImageDBToolStripMenuItem,
            this.getSubClassesToolStripMenuItem,
            this.getClassesAndImagesToolStripMenuItem,
            this.getClassesLiteralsToolStripMenuItem,
            this.getDataPropertiesToolStripMenuItem,
            this.createImageLiteralsToolStripMenuItem,
            this.createOntologyToolStripMenuItem1});
            this.createOntologyToolStripMenuItem.Name = "createOntologyToolStripMenuItem";
            this.createOntologyToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.createOntologyToolStripMenuItem.Text = "Ontology";
            this.createOntologyToolStripMenuItem.Click += new System.EventHandler(this.createOntologyToolStripMenuItem_Click);
            // 
            // getClassesInImageDBToolStripMenuItem
            // 
            this.getClassesInImageDBToolStripMenuItem.Name = "getClassesInImageDBToolStripMenuItem";
            this.getClassesInImageDBToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.getClassesInImageDBToolStripMenuItem.Text = "get Classes in ImageDB";
            this.getClassesInImageDBToolStripMenuItem.Click += new System.EventHandler(this.getClassesInImageDBToolStripMenuItem_Click);
            // 
            // getSubClassesToolStripMenuItem
            // 
            this.getSubClassesToolStripMenuItem.Name = "getSubClassesToolStripMenuItem";
            this.getSubClassesToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.getSubClassesToolStripMenuItem.Text = "get SubClasses";
            // 
            // getClassesAndImagesToolStripMenuItem
            // 
            this.getClassesAndImagesToolStripMenuItem.Name = "getClassesAndImagesToolStripMenuItem";
            this.getClassesAndImagesToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.getClassesAndImagesToolStripMenuItem.Text = "get Classes and Images";
            this.getClassesAndImagesToolStripMenuItem.Click += new System.EventHandler(this.getClassesAndImagesToolStripMenuItem_Click);
            // 
            // getClassesLiteralsToolStripMenuItem
            // 
            this.getClassesLiteralsToolStripMenuItem.Name = "getClassesLiteralsToolStripMenuItem";
            this.getClassesLiteralsToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.getClassesLiteralsToolStripMenuItem.Text = "get Classes Literals";
            // 
            // getDataPropertiesToolStripMenuItem
            // 
            this.getDataPropertiesToolStripMenuItem.Name = "getDataPropertiesToolStripMenuItem";
            this.getDataPropertiesToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.getDataPropertiesToolStripMenuItem.Text = "get Data Properties";
            // 
            // createImageLiteralsToolStripMenuItem
            // 
            this.createImageLiteralsToolStripMenuItem.Name = "createImageLiteralsToolStripMenuItem";
            this.createImageLiteralsToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.createImageLiteralsToolStripMenuItem.Text = "get Image Literals";
            this.createImageLiteralsToolStripMenuItem.Click += new System.EventHandler(this.createImageLiteralsToolStripMenuItem_Click);
            // 
            // createOntologyToolStripMenuItem1
            // 
            this.createOntologyToolStripMenuItem1.Name = "createOntologyToolStripMenuItem1";
            this.createOntologyToolStripMenuItem1.Size = new System.Drawing.Size(196, 22);
            this.createOntologyToolStripMenuItem1.Text = "Create Ontology";
            this.createOntologyToolStripMenuItem1.Click += new System.EventHandler(this.createOntologyToolStripMenuItem1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(80, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(539, 39);
            this.label1.TabIndex = 2;
            this.label1.Text = "SEMANTIC IMAGE RETRIEVAL";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // frmSBIR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 112);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "frmSBIR";
            this.Text = "SBIR";
            this.Load += new System.EventHandler(this.frmSBIR_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem imageFeatureaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataProcessingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gPTreeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extractFeaturesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clusteringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem classificationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deepLearningToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem convolutionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem convolutionNetworkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cNNToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kMeansToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem meanShiftToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem heirachicalClusteringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sOMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kNNToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sVMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aNNToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cNNToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem extract390FeaturesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getImagePathsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getImageClassesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createOntologyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getClassesAndImagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getClassesInImageDBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getClassesLiteralsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getDataPropertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createImageLiteralsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createOntologyToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem getSubClassesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageProcessingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createAnnotationFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameImageFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createGPTreeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem usingGPTreeCBIRToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem usingGPTreeSBIRToolStripMenuItem;
        private System.Windows.Forms.Label label1;
    }
}

