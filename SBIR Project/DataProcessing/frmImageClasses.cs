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
    public partial class frmImageClasses : Form
    {
        public frmImageClasses()
        {
            InitializeComponent();
        }

        public string getFileClass()
        {
            string file = string.Empty;
            if (radioButton1.Checked == true)
                file = @"../../data/COREL-Class.txt";
            else if (radioButton2.Checked == true)
                file = @"../../data/Wang-Class.txt";
            else if (radioButton3.Checked == true)
                file = @"../../data/StanfordDogs-Class.txt";
            else if (radioButton4.Checked == true)
                file = @"../../data/ImageCLEF-Class.txt";
            return file;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            string fileClass = getFileClass();
            
            if (folderDlg.ShowDialog() == DialogResult.OK)
            {
                string[] Files = Directory.GetFiles(folderDlg.SelectedPath, "*.jpg", SearchOption.AllDirectories);
                progressBar1.Minimum = 0;
                progressBar1.Maximum = Files.Length;
                int count = 0;
                progressBar1.Value = count;
                TextfileCluster tfc = new TextfileCluster(fileClass);
                foreach (string file in Files)
                {
                    string Imgfilename = Path.GetFileNameWithoutExtension(file);
                    string ClassName = string.Empty;
                    if(radioButton3.Checked == true)
                    {
                        Stanford Sf = new Stanford();
                        ClassName = Sf.getClassName(Imgfilename);
                    }
                    tfc.WriteLineTextFile(Imgfilename + "\t" + ClassName);
                    count++;
                    textBox1.Text = count.ToString() + "\t" + Imgfilename + "\t" + ClassName;
                    progressBar1.Value = count;
                    Application.DoEvents();
                }
                progressBar1.Value = Files.Length;
            }
        }
    }
}
