using SBIR_Project.GP_Tree;
using SBIR_Project.H_Tree;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SBIR
{
    public partial class frmSBIR : Form
    {
        public frmSBIR()
        {
            InitializeComponent();
        }

        private void createCTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void extractFeaturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmExtractingFeatures frm = new frmExtractingFeatures();
            frm.Show();
        }

        private void extract390FeaturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmExtracting390Features frm = new frmExtracting390Features();
            frm.Show();
        }

      
        private void getImagePathsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmImagePath frm = new frmImagePath();
            frm.Show();
        }

        private void getImageClassesToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmImageClasses frm = new frmImageClasses();
            frm.Show();
        }

     

        private void createOntologyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frmCeateOntology frm = new frmCeateOntology();
            //frm.Show();
        }

        private void createOntologyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmCeateOntology frm = new frmCeateOntology();
            frm.Show();
        }

        private void getClassesInImageDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGetImageDBClasses frm = new frmGetImageDBClasses();
            frm.Show();
        }

        private void getClassesAndImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGetClassesAndImages frm = new frmGetClassesAndImages();
            frm.Show();
        }

        private void createImageLiteralsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmImageLiterals frm = new frmImageLiterals();
            frm.Show();
        }

        private void createAnnotationFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCreateAnoFile frm = new frmCreateAnoFile();
            frm.Show();
        }

        private void renameImageFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRenameImageFile frm = new frmRenameImageFile();
            frm.Show();
        }

        private void createHTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCreateHTree frm = new frmCreateHTree();
            frm.Show();
        }

        private void frmSBIR_Load(object sender, EventArgs e)
        {

        }

        private void usingHTreeCBIRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmHTreeCBIR frm = new frmHTreeCBIR();
            frm.Show();
        }

        private void createGPTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCreateGPTree frm = new frmCreateGPTree();
            frm.Show();
        }

        private void usingGPTreeCBIRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGPTreeCBIR frm = new frmGPTreeCBIR();
            frm.Show();
        }

        private void usingGPTreeSBIRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGP_TreeSBIR frm = new frmGP_TreeSBIR();
            frm.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
