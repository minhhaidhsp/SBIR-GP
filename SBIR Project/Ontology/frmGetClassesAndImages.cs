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
    public partial class frmGetClassesAndImages : Form
    {
        public frmGetClassesAndImages()
        {
            InitializeComponent();
        }
        public string getFileClassImageNamePath()
        {
            string path = string.Empty;
            if (radioButton1.Checked == true)
                path = @"../../OntologyCOREL/ClassImageCOREL.txt";
            else if (radioButton2.Checked == true)
                path = @"../../OntologyWang/ClassImageWang.txt";
            else if (radioButton3.Checked == true)
                path = @"../../OntologyStanfordDogs/ClassImageStanfordDogs.txt";
            else if (radioButton4.Checked == true)
                path = @"../../OntologyImageCLEF/ClassImageImageCLEF.txt";
            return path;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            if (folderDlg.ShowDialog() == DialogResult.OK)
            {
                DataProcessing dp = new DataProcessing();
                List<string> SubFolder = dp.getSubFloder(folderDlg.SelectedPath);
                if (SubFolder == null) return;
                if (SubFolder.Count == 0) return;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = SubFolder.Count;
                int count = 0;
                progressBar1.Value = count;
                TextfileCluster tfc = new TextfileCluster(getFileClassImageNamePath());
                foreach (string folder in SubFolder)
                {
                    string data = string.Empty;
                    if (radioButton3.Checked == true)
                    {
                        Stanford Sf = new Stanford();
                        string foldername = dp.getFolderName(folder);
                        data = Sf.getFolderClassName(foldername) + " : ";
                    }
                    else
                        data = dp.getFolderName(folder) + " :";

                    string[] filenames = Directory.GetFiles(folder, "*.jpg", SearchOption.TopDirectoryOnly);
                    for (int i = 0; i < filenames.Length; i++)
                        data = data + " " + Path.GetFileNameWithoutExtension(filenames[i]);

                    textBox1.Text = data;
                    tfc.WriteLineTextFile(data);
                    count++;
                    progressBar1.Value = count;
                    Application.DoEvents();
                }
                progressBar1.Value = SubFolder.Count;
            }
            MessageBox.Show("DONE!");
        }
    }
}
