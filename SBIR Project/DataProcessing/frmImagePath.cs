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
    public partial class frmImagePath : Form
    {
        public frmImagePath()
        {
            InitializeComponent();
        }
        public string getImgPath()
        {
            string file = string.Empty;
            if (radioButton1.Checked == true)
                file = @"../../data/ImagePathCOREL.txt";
            else if (radioButton2.Checked == true)
                file = @"../../data/ImagePathWang.txt";
            else if (radioButton3.Checked == true)
                file = @"../../data/ImagePathStanfordDogs.txt";
            else if (radioButton4.Checked == true)
                file = @"../../data/ImagePathImageCLEF.txt";
            return file;
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

        public string getImageDBPath()
        {
            string path = string.Empty;
            if (radioButton1.Checked == true)
                path = @"../../../ImageDBs/COREL/";
            else if (radioButton2.Checked == true)
                path = @"../../../ImageDBs/Wang/";
            else if (radioButton3.Checked == true)
                path = @"../../../ImageDBs/StanfordDogs/";
            else if (radioButton4.Checked == true)
                path = @"../../../ImageDBs/ImageCLEF/";
            return path;
        }

        private void btnImgPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            string ImagePathFile = getImgPath();
            string file = getFileClass();
            DataProcessing dp = new DataProcessing();
            string DataPath = string.Empty;
            if (folderDlg.ShowDialog() == DialogResult.OK)
            {
                string[] filenames = Directory.GetFiles(folderDlg.SelectedPath, "*.jpg", SearchOption.AllDirectories);
                progressBar1.Minimum = 0;
                progressBar1.Maximum = filenames.Length;
                int count = 0;
                progressBar1.Value = count;
                foreach (string name in filenames)
                {
                    DataPath = Path.GetFileName(name);
                    if (radioButton3.Checked == true)
                    {
                        Stanford Sf = new Stanford();
                        DataPath += "\t" + "(" + getImageDBPath() + "Images" + "/" + Sf.getFolderName(Path.GetFileNameWithoutExtension(name)) + "/" + Path.GetFileName(name) + ")";
                        DataPath += "\t" + "(" + getImageDBPath() + "Anotations" + "/" + Sf.getFolderName(Path.GetFileNameWithoutExtension(name)) + "/" + Path.GetFileNameWithoutExtension(name) + ".xml" + ")";
                        DataPath += "\t" + "(" + dp.getClass(Path.GetFileNameWithoutExtension(name), file) + ")";
                    }
                    else
                    {
                        DataPath += "\t" + "(" + getImageDBPath() + "Images" + "/" + dp.getClass(Path.GetFileNameWithoutExtension(name), file) + "/" + Path.GetFileName(name) + ")";
                        DataPath += "\t" + "(" + getImageDBPath() + "Anotations" + "/" + dp.getClass(Path.GetFileNameWithoutExtension(name), file) + "/" + Path.GetFileNameWithoutExtension(name) + ".xml" + ")";
                        DataPath += "\t" + "(" + dp.getClass(Path.GetFileNameWithoutExtension(name), file) + ")";
                    }
                    TextfileCluster tfc = new TextfileCluster();
                    tfc.WriteLineTextFile(DataPath, ImagePathFile);

                    textBox1.Text = DataPath;
                    count++;
                    progressBar1.Value = count;
                    Application.DoEvents();
                }
                progressBar1.Value = filenames.Length;
                MessageBox.Show("DONE!");
            }
        }
    }
}
