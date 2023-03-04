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
    public partial class frmRenameImageFile : Form
    {
        public frmRenameImageFile()
        {
            InitializeComponent();
        }

        private void frmRenameImageFile_Load(object sender, EventArgs e)
        {

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
                foreach (string name in filenames)
                {
                    if (File.Exists(name))
                    {
                        
                        DataProcessing dp = new DataProcessing();
                        string filename = Path.GetFileNameWithoutExtension(name);
                        filename = "COREL" + filename + ".jpg";
                        string newName = Path.GetDirectoryName(name) + "/" + filename;

                        File.Copy(name, newName, true);
                        File.Delete(name);

                        textBox1.Text = "Old Name: " + name + "\r\n" + "New Name: " + newName;
                    }
                    count++;
                    progressBar1.Value = count;
                    Application.DoEvents();
                }
                progressBar1.Value = filenames.Length;
            }
            MessageBox.Show("DONE!");
        }
    }
}
