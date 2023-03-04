using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SBIR
{
    public partial class frmCreateAnoFile : Form
    {
        public frmCreateAnoFile()
        {
            InitializeComponent();
        }

        public void CreateAnnotationFile(string ImgPath)
        {
            Image Img = Image.FromFile(ImgPath);
            pictureBox1.Image = Img;

            //Lấy tên lớp là tên thư mục chứa ảnh đó
            DataProcessing dp = new DataProcessing();
            string classname = string.Empty;
            if (radioButton3.Checked == true)
            {
                Stanford Sf = new Stanford();
                classname = Sf.getClassName(Path.GetFileNameWithoutExtension(ImgPath));
            }
            else
            {

                classname = dp.getLastFolderName(ImgPath);
            }
            label2.Text = classname;

            //Lấy tên tập tin ảnh
            string filenameNoExt = Path.GetFileNameWithoutExtension(ImgPath);
            label4.Text = filenameNoExt;
            //Lấy tên tập dữ liệu ảnh
            string[] words = Path.GetDirectoryName(ImgPath).Split('\\');
            words = dp.RemoveBlank(words);
            string ImageDBname = words[words.Length - 3];
            label6.Text = ImageDBname;
            //Lấy chiều dài và chiều rộng của ảnh
            label8.Text = Img.Width.ToString();
            label10.Text = Img.Height.ToString();
            //Lấy độ dài dải màu
            label12.Text = Img.PixelFormat.ToString();
            //Lấy loại file ảnh
            string ext = Path.GetExtension(ImgPath).Replace(".", "");
            label14.Text = ext;
            //Lấy đường dẫn ảnh
            string ImgLocation = @"../../../" + @"ImageDBs" + @"/" + ImageDBname + @"/" + "Images" + @"/" + dp.getLastFolderName(ImgPath) + @"/" + filenameNoExt + "." + ext;
            label16.Text = ImgLocation;
            //Lấy các thông tin ảnh
            FileInfo fi = new FileInfo(ImgPath);
            string created = fi.CreationTime.ToString();
            string lastmodified = fi.LastWriteTime.ToString();
            //label18.Text = created; //không cần thiết
            label20.Text = lastmodified;
            //Lấy tên owner
            string user = System.IO.File.GetAccessControl(ImgPath).GetOwner(typeof(System.Security.Principal.NTAccount)).ToString();
            //label22.Text = user; //không cần thiết
            //Lấy kích thức file
            long length = new System.IO.FileInfo(ImgPath).Length;
            string strsize = length.ToString() + " bytes";
            label24.Text = strsize;
            //Lấy độ phân giải của ảnh
            Bitmap Bmp = new Bitmap(ImgPath);
            string Hres = Bmp.HorizontalResolution + " dpi";
            string Vres = Bmp.VerticalResolution + " dpi";
            label26.Text = Hres;
            label28.Text = Vres;
            //Mô tả hình ảnh
            string description = "This is an image in " + ImageDBname + " dataset." + " The image has a category as " + classname.ToUpper() + ". " + "It had been created at " + lastmodified + ". ";
            textBox1.Text = description;

            //Lưu tên file
            string fileAnno = @"../../../" + @"ImageDBs" + @"/" + ImageDBname + @"/" + "Annotations" + @"/" + dp.getLastFolderName(ImgPath) + @"/" + filenameNoExt + ".xml";
            if (File.Exists(fileAnno)) File.Delete(fileAnno);
            TextfileCluster tfc = new TextfileCluster(fileAnno);
            tfc.WriteLineTextFile("<Dataset>" + " " + ImageDBname + " " + "</Dataset>");
            tfc.WriteLineTextFile("<ClassName>" + " " + classname + " " + "</ClassName>");
            tfc.WriteLineTextFile("<FileName>" + " " + filenameNoExt + " " + "</FileName>");
            tfc.WriteLineTextFile("<FileType>" + " " + ext + " " + "</FileType>");
            tfc.WriteLineTextFile("<Width>" + " " + Img.Width.ToString() + " " + "</Width>");
            tfc.WriteLineTextFile("<Width>" + " " + Img.Height.ToString() + " " + "</Width>");
            tfc.WriteLineTextFile("<Color-Chanel>" + " " + Img.PixelFormat.ToString() + " " + "</Color-Chanel>");
            tfc.WriteLineTextFile("<File-Size>" + " " + strsize + " " + "</File-Size>");
            tfc.WriteLineTextFile("<Horizontal-Resolution>" + " " + Hres + " " + "</Horizontal-Resolution>");
            tfc.WriteLineTextFile("<Vertical-Resolution>" + " " + Vres + " " + "</Vertical-Resolution>");
            tfc.WriteLineTextFile("<Description>" + " " + description + " " + "</Description>");
            tfc.WriteLineTextFile("<Modified-date>" + " " + lastmodified + " " + "</Modified-date>");
            tfc.WriteLineTextFile("<Image-path>" + " " + ImgLocation + " " + "</Image-path>");
            tfc.WriteLineTextFile("<Annotation-path>" + " " + fileAnno + " " + "</Annotation-path>");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog diag = new OpenFileDialog())
            {
                diag.Filter = "Bitmap |*.bmp;*.jpg;*.gif;*.png";
                diag.InitialDirectory = Application.StartupPath;
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    string ImgPath = diag.FileName;
                    CreateAnnotationFile(ImgPath);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
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
                    CreateAnnotationFile(name);
                    count++;
                    progressBar1.Value = count;
                    Application.DoEvents();
                }
                progressBar1.Value = filenames.Length;
            }
            MessageBox.Show("DONE!");
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
