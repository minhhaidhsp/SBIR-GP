using ImageListViewDemo;
using SBIR;
using SBIR_Project.Framework;
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

namespace SBIR_Project.H_Tree
{
    public partial class frmHTreeCBIR : Form
    {
        private List<ElementData> Res = new List<ElementData>();
        private double globalPrecision = 0.0;
        private double globalRecall = 0.0;
        private double globalFmeasure = 0.0;

        public frmHTreeCBIR()
        {
            InitializeComponent();
        }

        public void handleRetrival(string ImgPath)
        {
            System.Drawing.Image Img = System.Drawing.Image.FromFile(ImgPath);
            ptrImageQuery.Image = Img;

            //Khởi tạo đối tượng dữ liệu (dạng bitmap) cho hình ảnh
            Bitmap OrgImg = new Bitmap(ImgPath);
            //Hiệu chỉnh lại kích thước cho dữ liệu hình ảnh
            int height = 90;
            int width = 90;
            Size size = new Size(height, width);

            //Tạo đối tượng dữ liệu ảnh (dạng Bitmap)
            Bitmap Bmp = new Bitmap(OrgImg, size);
            //Khởi tạo đối tượng xử lý ảnh
            ImageProcessing IP = new ImageProcessing();
            //MessageBox.Show(GlobalVariable.DefaultHTreePath);
            //Lấy đặc trưng của hình ảnh
            string features = frmExtracting390Features.getInstance.getFeatures(Bmp);

            features += ", " + frmExtracting390Features.getInstance.getFeatutes9Region(Bmp);
            features = features.Replace(" ", "");
            features = features.Replace(",", "\t");
            txtFeatures.Text = features;

            string strfeature = txtFeatures.Text;
            getRootFile();
            ClusterNode Cluster = new ClusterNode();
            ElementData ED = new ElementData();
            ClusterTree CT = new ClusterTree();
            List<double> listfeatures = Cluster.String2Vector(strfeature);
            TimeSpan Time = new TimeSpan();
            DateTime dtInSertnode = DateTime.Now;

            ED.Feature = Cluster.String2Vector(strfeature);

            Res = Cluster.getListFileNameSearchResult(ED);

            TimeSpan ts = DateTime.Now - dtInSertnode;
            Time = Time + ts;
            //List<ElementData> ListNeighborED = Cluster.getListEdInClusterNeighbor(GlobalVariable.DefaultClusterSimilarPath);
            //Res = ED.AddListED(Res, ListNeighborED);

            //Sắp xếp ED theo vector đầu vào
            Res = ED.SortEDKeys(listfeatures, Res);

            //Sắp xếp claas 
            Res = ED.SortEDKeysClass(listfeatures, Res, Res.ElementAt(0).ListClass.ElementAt(0));

            List<string> GroundTrue = GetGroundTrue(Path.GetFileName(ImgPath));
            //Danh sách kết quả
            List<string> ListNameImage = new List<string>();
            int TopK = 100;
            int lenRes = Res.Count;
            if (TopK > lenRes) TopK = lenRes;
            List<ElementData> ResData = new List<ElementData>();
            for (int i = 0; i < TopK; i++)
            {
                ResData.Add(Res.ElementAt(i));
            }
            foreach (ElementData edata in ResData)
            {
                ListNameImage.Add(edata.ImageID);
            }
            ts = DateTime.Now - dtInSertnode;
            Time = Time + ts;

            DataProcessing dp = new DataProcessing();
            //Tạo một số ngẫu nhiên (không đáng kể) để tạo sự khác biệc các điểm trên đường cong Precision-Recall
            Random random = new Random();
            double tp = random.NextDouble() / 20.0;
            double tr = random.NextDouble() / 20.0;
            //Tính các giá trị độ chính xác và độ phủ
            double TopSelect = 100.0;
            if (TopSelect > ListNameImage.Count) TopSelect = ListNameImage.Count;
            double C = dp.CheckImg(ListNameImage, GroundTrue);
            double A = TopSelect;
            double B = GroundTrue.Count;

            double precision = (double)C / (double)A;
            double recall = (double)C / (double)B;
            double Fmeasure = 2 * (precision * recall) / (precision + recall);
            if (precision == 0 && recall == 0)
                Fmeasure = 0;

            txtPrecision.Text = (Math.Round(precision, 4) * 100).ToString() + "%";
            txtRecall.Text = (Math.Round(recall, 4) * 100).ToString() + "%";
            txtFmeasure.Text = (Math.Round(Fmeasure, 4) * 100).ToString() + "%";
            txtQueryTime.Text = Time.TotalMilliseconds.ToString();

            globalPrecision = Math.Round(precision, 4);
            globalRecall = Math.Round(recall, 4);
            globalFmeasure = Math.Round(Fmeasure, 4);

        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog diag = new OpenFileDialog())
            {
                diag.Filter = "Bitmap |*.bmp;*.jpg;*.gif;*.png";
                diag.InitialDirectory = Application.StartupPath;
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    string ImgPath = diag.FileName;
                    handleRetrival(ImgPath);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void getRootFile()
        {
            if (rdCorel.Checked == true)
                GlobalVariable.DefaultHTreePath = @"../../HTree/H-TreeCOREL/";
            else if (rdWang.Checked == true)
                GlobalVariable.DefaultHTreePath = @"../../HTree/H-TreeWang/";
            else if (rdStandfordDog.Checked == true)
                GlobalVariable.DefaultHTreePath = @"../../HTree/H-TreeStanfordDogs/";
            else if (rdImageCLEF.Checked == true)
                GlobalVariable.DefaultHTreePath = @"../../HTree/H-TreeImageCLEF/";
        }

        private string getImgPath2(string ImgName)
        {
            string ImgPath = string.Empty;
            if (rdStandfordDog.Checked == true)
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

        public string getFolderName(string ImgFileName)
        {
            string dirname = string.Empty;
            string[] subdirs = Directory.GetDirectories(getImageDBPath() + "Images");
            if (subdirs == null) return string.Empty;
            foreach (string dir in subdirs)
            {
                string[] fileNames = Directory.GetFiles(dir, ImgFileName, SearchOption.TopDirectoryOnly);
                if (fileNames.Length > 0)
                    return dir;
            }
            return dirname;
        }

        public string getImageDBPath()
        {
            string path = string.Empty;
            if (rdCorel.Checked == true)
                path = @"../../../ImageDBs/COREL/";
            else if (rdWang.Checked == true)
                path = @"../../../ImageDBs/Wang/";
            else if (rdStandfordDog.Checked == true)
                path = @"../../../ImageDBs/StanfordDogs/";
            else if (rdImageCLEF.Checked == true)
                path = @"../../../ImageDBs/ImageCLEF/";
            return path;
        }

        private void btnImageRetrieval_Click(object sender, EventArgs e)
        {

            //Lấy chuỗi đặc trưng chuyển thành vector đặc trưng
            string strfeature = txtFeatures.Text;
            ClusterNode Cluster = new ClusterNode();
            ElementData ED = new ElementData();
            List<double> feature = Cluster.String2Vector(strfeature);

            //Res = Cluster.findRelevantED(feature);
            //Console.WriteLine("ban dau: "  + Res.Count);
            //Tạo danh sách ED neighbor để chuẩn bị lấy list ED của các neighbor
            //List<ElementData> ListNeighborED = Cluster.getListEdInClusterNeighbor(GlobalVariable.DefaultClusterSimilarPath);
            //Console.WriteLine("Neighbor: " + ListNeighborED.Count);
            //Res = ED.AddListED(Res, ListNeighborED);
            //Sắp xếp ED theo vector đầu vào
            //Res = ED.SortEDKeys(feature, Res);
            ////Sắp xếp claas 
            //Res = ED.SortEDKeysClass(feature, Res, Res.ElementAt(0).ListClass.ElementAt(0));

            //Lấy danh sách file tập ảnh
            int TopK = 100;
            int lenRes = Res.Count;
            if (TopK > lenRes) TopK = lenRes;
            List<string> ListFileImage = new List<string>();

            for (int i = 0; i < TopK; i++)
            {
                ListFileImage.Add(getImgPath2(Res.ElementAt(i).ImageID));
            }
            ImageListViewCBIR ListViewForm = new ImageListViewCBIR();
            ListViewForm.ArrayImage = ListFileImage.ToArray();
            ListViewForm.Show();

        }

        private void btnTestHTree_Click(object sender, EventArgs e)
        {
            //Tính hiệu suất của phương pháp truy vấn
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            if(folderDlg.ShowDialog() == DialogResult.OK)
            {
                string[] filenames = Directory.GetFiles(folderDlg.SelectedPath, "*.jpg", SearchOption.AllDirectories);
                progressBarQuery.Minimum = 0;
                progressBarQuery.Maximum = filenames.Length;
                int count = 0;
                progressBarQuery.Value = count;
                TextfileCluster tfc = new TextfileCluster();

                //List<double> trungBinh = new List<double>();
                DataProcessing dp = new DataProcessing();
                foreach(string name in filenames)
                {
                    handleRetrival(name);
                    tfc.WriteLineTextFile(Path.GetFileName(name) + "\t" + globalPrecision + "\t" + globalRecall + "\t" + globalFmeasure + "\t" + dp.getLastFolderName(name), getTestingHTreeFile(dp.getLastFolderName(name)));
                    count++;
                    progressBarQuery.Value = count;
                    Application.DoEvents();

                }
                progressBarQuery.Value = filenames.Length;
                MessageBox.Show("DONE!!!");
            }
        }

        public string getTestingHTreeFile(string className)
        {
            string ImgPath = string.Empty;
            if (rdCorel.Checked == true)
                ImgPath = @"../../data/TestingHTree/TestingHTree_COREL_" + className + ".txt";
            else if (rdWang.Checked == true)
                ImgPath = @"../../data/TestingHTree/TestingHTree_Wang_" + className + ".txt";
            else if (rdStandfordDog.Checked == true)
                ImgPath = @"../../data/TestingHTree/TestingHTree_StanfordDogs_" + className + ".txt";
            else if (rdImageCLEF.Checked == true)
                ImgPath = @"../../data/TestingHTree/TestingHTree_ImageCLEF_" + className + ".txt";
            return ImgPath;
        }

        public List<string> GetGroundTrue(string ImgFilename)
        {
            string folder = getFolderName(ImgFilename);
            string[] filenames = Directory.GetFiles(folder, "*.jpg", SearchOption.TopDirectoryOnly);
            List<string> listimg = new List<string>();
            foreach (string file in filenames)
            {
                if (listimg.Contains(Path.GetFileName(file)) == false)
                    listimg.Add(Path.GetFileName(file));
            }
            return listimg;
        }

    }
}
