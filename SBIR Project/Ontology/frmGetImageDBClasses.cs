using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SBIR
{
    public partial class frmGetImageDBClasses : Form
    {
        public frmGetImageDBClasses()
        {
            InitializeComponent();
        }

        private void frmGetImageDBClasses_Load(object sender, EventArgs e)
        {

        }

        public string getOntologyPath()
        {
            string path = string.Empty;
            if (radioButton1.Checked == true)
                path = @"../../OntologyCOREL/";
            else if (radioButton2.Checked == true)
                path = @"../../OntologyWang/";
            else if (radioButton3.Checked == true)
                path = @"../../OntologyStanfordDogs";
            else if (radioButton4.Checked == true)
                path = @"../../OntologyImageCLEF/";
            return path;
        }
        public string getFileClassNamePath()
        {
            string path = string.Empty;
            if (radioButton1.Checked == true)
                path = @"../../OntologyCOREL/ClassNameCOREL.txt";
            else if (radioButton2.Checked == true)
                path = @"../../OntologyWang/ClassNameWang.txt";
            else if (radioButton3.Checked == true)
                path = @"../../OntologyStanfordDogs/ClassNameStanfordDogs.txt";
            else if (radioButton4.Checked == true)
                path = @"../../OntologyImageCLEF/ClassNameImageCLEF.txt";
            return path;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            if (folderDlg.ShowDialog() == DialogResult.OK)
            {
                DataProcessing dp = new DataProcessing();
                List<string> SubFolderName = dp.getSubFloderName(folderDlg.SelectedPath);
                if (SubFolderName == null) return;
                if (SubFolderName.Count == 0) return;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = SubFolderName.Count;
                int count = 0;
                progressBar1.Value = count;
                TextfileCluster tfc = new TextfileCluster(getFileClassNamePath());
                foreach (string name in SubFolderName)
                {
                    if (radioButton3.Checked == true)
                    {
                        Stanford Sf = new Stanford();
                        tfc.WriteLineTextFile(Sf.getFolderClassName(name).ToUpper());
                    }
                    else
                        tfc.WriteLineTextFile(name.ToUpper());
                    count++;
                    progressBar1.Value = count;
                    Application.DoEvents();
                }
                progressBar1.Value = SubFolderName.Count;
            }
            MessageBox.Show("DONE!");
        }
    }
}
