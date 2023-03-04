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
    public partial class frmImageLiterals : Form
    {
        public frmImageLiterals()
        {
            InitializeComponent();
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

        public string getFolderName(string ImgFileName)
        {
            string dirname = string.Empty;
            DataProcessing dp = new DataProcessing();
            List<string> subdirs = dp.getSubFloder(getImageDBPath() + "Images");
            if (subdirs == null) return string.Empty;
            foreach (string dir in subdirs)
            {
                string[] fileNames = Directory.GetFiles(dir, ImgFileName, SearchOption.TopDirectoryOnly);
                if (fileNames.Length > 0)
                    return dir;
            }
            return dirname;
        }
        public string getFolderAnoName(string AnoFileName)
        {
            string dirname = string.Empty;
            DataProcessing dp = new DataProcessing();
            List<string> subdirs = dp.getSubFloder(getImageDBPath() + "Annotations");
            if (subdirs == null) return string.Empty;
            foreach (string dir in subdirs)
            {
                string[] fileNames = Directory.GetFiles(dir, AnoFileName, SearchOption.TopDirectoryOnly);
                if (fileNames.Length > 0)
                    return dir;
            }
            return dirname;
        }
        private string getImgPath2(string ImgName)
        {
            string ImgPath = string.Empty;
            if (radioButton3.Checked == true)
            {
                Stanford Sf = new Stanford();
                ImgPath = Sf.getFileImg(Path.GetFileNameWithoutExtension(ImgName));
            }
            else
            {
                ImgPath = getFolderName(ImgName) + "/" + Path.GetFileName(ImgName);
            }
            return ImgPath;
        }

        private string getAnoPath(string ImgName)
        {
            string AnoPath = string.Empty;
            if (radioButton3.Checked == true)
            {
                Stanford Sf = new Stanford();
                AnoPath = Sf.getFileAno(Path.GetFileNameWithoutExtension(ImgName)) + ".xml";
            }
            else
            {
                DataProcessing dp = new DataProcessing();
               
                AnoPath = getFolderAnoName(ImgName) + "/" + Path.GetFileNameWithoutExtension(ImgName) + ".xml";
            }
            return AnoPath;
        }

        public string getFileLiteralImageNamePath()
        {
            string path = string.Empty;
            if (radioButton1.Checked == true)
                path = @"../../OntologyCOREL/ImageLiteralCOREL.txt";
            else if (radioButton2.Checked == true)
                path = @"../../OntologyWang/ImageLiteralWang.txt";
            else if (radioButton3.Checked == true)
                path = @"../../OntologyStanfordDogs/ImageLiteralStanfordDogs.txt";
            else if (radioButton4.Checked == true)
                path = @"../../OntologyImageCLEF/ImageLiteralImageCLEF.txt";
            return path;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            if (folderDlg.ShowDialog() == DialogResult.OK)
            {
                string[] filenames = Directory.GetFiles(folderDlg.SelectedPath, "*.jpg", SearchOption.AllDirectories);
                progressBar1.Minimum = 0;
                progressBar1.Maximum = filenames.Length;
                int count = 0;
                progressBar1.Value = count;
                TextfileCluster tfc = new TextfileCluster(getFileLiteralImageNamePath());
                for (int i = 0; i < filenames.Length; i++)
                {
                    string IMG = "IMG" + Path.GetFileNameWithoutExtension(filenames[i]);
                    
                    string URL = @"http://sbir-hcm.vn/" + IMG;
                    string ImageFileName = Path.GetFileName(filenames[i]);
                    string ImgPath = getImgPath2(ImageFileName);
                    ImgPath = ImgPath.Replace(@"\", @"/");
                    string AnoFileName = Path.GetFileNameWithoutExtension(filenames[i]) + ".xml";
                    string AnoPath = getAnoPath(AnoFileName);
                    AnoPath = AnoPath.Replace(@"\", @"/");
                    string Lclass = string.Empty;
                    if (radioButton3.Checked == true)
                    {
                        Stanford Sf = new Stanford();
                        Lclass = Sf.getClassName(Path.GetFileNameWithoutExtension(filenames[i]));
                    }
                    else
                    {
                        DataProcessing dp = new DataProcessing();
                        Lclass = dp.getLastFolderName(filenames[i]);
                    }

                    tfc.WriteLineTextFile(IMG + " " + "(" + URL + ")" + " " + "(" + ImgPath + ")" + " " + "(" + ImageFileName + ")" + " " + "(" + AnoPath + ")" + " " + "(" + Lclass + ")");
                    count++;
                    progressBar1.Value = count;
                    Application.DoEvents();
                }
                progressBar1.Value = filenames.Length;
                MessageBox.Show("DONE");
            }
        }
    }
}
