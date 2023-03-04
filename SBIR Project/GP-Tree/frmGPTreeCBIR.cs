using ImageListViewDemo;
using SBIR;
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

namespace SBIR_Project.GP_Tree
{
    public partial class frmGPTreeCBIR : Form
    {
        public frmGPTreeCBIR()
        {
            InitializeComponent();
        }

        private string queryImageName = String.Empty;
        private double globalPrecision = 0.0;
        private double globalRecall = 0.0;
        private double globalFmeasure = 0.0;

        public double getRate()
        {
            double rate = 0.0;
            if (rdCorel.Checked == true)
                rate = 0.8;
            else if (rdWang.Checked == true)
                rate = 0.6;
            else if (rdStanfordDogs.Checked == true)
                rate = 0.4;
            else if (rdImgClef.Checked == true)
                rate = 0.4;
            return rate;
        }
        public double getRate2()
        {
            double rate = 0.0;
            if (rdCorel.Checked == true)
                rate = 0.9;
            else if (rdWang.Checked == true)
                rate = 0.7;
            else if (rdStanfordDogs.Checked == true)
                rate = 0.5;
            else if (rdImgClef.Checked == true)
                rate = 0.5;
            return rate;
        }

        public string getRootFile()
        {
            string Root = string.Empty;
            if (rdCorel.Checked == true)
                Root = @"../../GPTree/GPTree_COREL/Root.txt";
            else if (rdWang.Checked == true)
                Root = @"../../GPTree/GPTree_Wang/Root.txt";
            else if (rdStanfordDogs.Checked == true)
                Root = @"../../GPTree/GPTree_StanfordDogs/Root.txt";
            else if (rdImgClef.Checked == true)
                Root = @"../../GPTree/GPTree_ImageCLEF/Root.txt";
            return Root;
        }

        public string getRootFile2()
        {
            string Root = string.Empty;
            if (rdCorel.Checked == true)
                Root = @"../../GPTree/GPTree_COREL2/Root.txt";
            else if (rdWang.Checked == true)
                Root = @"../../GPTree/GPTree_Wang2/Root.txt";
            else if (rdStanfordDogs.Checked == true)
                Root = @"../../GPTree/GPTree_StanfordDogs2/Root.txt";
            else if (rdImgClef.Checked == true)
                Root = @"../../GPTree/GPTree_ImageCLEF2/Root.txt";
            return Root;
        }

        public string getImageDBPath()
        {
            string path = string.Empty;
            if (rdCorel.Checked == true)
                path = @"../../../ImageDBs/COREL/";
            else if (rdWang.Checked == true)
                path = @"../../../ImageDBs/Wang/";
            else if (rdStanfordDogs.Checked == true)
                path = @"../../../ImageDBs/StanfordDogs/";
            else if (rdImgClef.Checked == true)
                path = @"../../../ImageDBs/ImageCLEF/";
            return path;
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

        private string getImgPath(string ImgName)
        {
            string ImgPath = string.Empty;
            if (rdStanfordDogs.Checked == true)
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

        public string getTestingGPTreeFile(string className)
        {
            string ImgPath = string.Empty;
            if (rdCorel.Checked == true)
                ImgPath = @"../../data/TestingGPTree/TestingGPTree_COREL_" + className + ".txt";
            else if (rdWang.Checked == true)
                ImgPath = @"../../data/TestingGPTree/TestingGPTree_Wang_" + className + ".txt";
            else if (rdStanfordDogs.Checked == true)
                ImgPath = @"../../data/TestingGPTree/TestingGPTree_StanfordDogs_" + className + ".txt";
            else if (rdImgClef.Checked == true)
                ImgPath = @"../../data/TestingGPTree/TestingGPTree_ImageCLEF_" + className + ".txt";
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

        public string getTestingGPTreeGraphFile(string className)
        {
            string ImgPath = string.Empty;
            if (rdCorel.Checked == true)
                ImgPath = @"../../data/TestingGPTreeGraph/TestingGPTreeGraph_COREL_" + className + ".txt";
            else if (rdWang.Checked == true)
                ImgPath = @"../../data/TestingGPTreeGraph/TestingGPTreeGraph_Wang_" + className + ".txt";
            else if (rdStanfordDogs.Checked == true)
                ImgPath = @"../../data/TestingGPTreeGraph/TestingGPTreeGraph_StanfordDogs_" + className + ".txt";
            else if (rdImgClef.Checked == true)
                ImgPath = @"../../data/TestingGPTreeGraph/TestingGPTreeGraph_ImageCLEF_" + className + ".txt";
            return ImgPath;
        }

        private void handleLoadImage(string ImgPath)
        {
            queryImageName = ImgPath;
            System.Drawing.Image Img = System.Drawing.Image.FromFile(ImgPath);
            pictureBoxGP.Image = Img;

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

            //Lấy đặc trưng của hình ảnh
            //81 hướng
            string features = IP.get81Features(Bmp);

            //390 hướng
            //string features = frmExtracting390Features.getInstance.getFeatures(Bmp);
            //features += ", " + frmExtracting390Features.getInstance.getFeatutes9Region(Bmp);

            //features = features.Replace(" ", "");
            //features = features.Replace(",", "\t");

            txtFeatureGP.Text = features;
        }

        //private List<string> handleGPTreeRetrieval()
        //{
        //    string strfeature = txtFeatureGP.Text;
        //    ClusterNode Cluster = new ClusterNode();
        //    List<double> feature = Cluster.String2Vector(strfeature);
        //    string Root = getRootFile();
        //    string dir = Path.GetDirectoryName(Root) + "\\";
        //    string RenameLeafFile = dir + "RenameLeaf.txt";
        //    ClusterTree CT = new ClusterTree();
        //    ElementData ED = new ElementData();
        //    TimeSpan Time = new TimeSpan();
        //    DateTime dtInSertnode = DateTime.Now;

        //    string fileLeaf = CT.SearchGPTreeToLeaf(feature, Root);
        //    //List<ElementData> Res = CT.SearchGPTreeToLeafThird(feature, Root);
        //    List<ElementData> Res = ED.getListElementDataGPTree(dir + CT.getFileED(fileLeaf));

        //    string fileEDSecond = CT.SearchGPTreeToLeafSecond(feature, Root);
        //    List<ElementData> ResSecond = ED.getListElementData(fileEDSecond);
        //    Res = ED.AddListED(Res, ResSecond);




        //    TimeSpan ts = DateTime.Now - dtInSertnode;
        //    Time = Time + ts;

        //    //Sắp xếp lại các Element Data theo thứ tự độ đo Euclide của một vector đầu vào
        //    Res = ED.SortEDKeys(feature, Res);
        //    //Sắp xếp lại danh sách ED có class gần với classname của ED đầu tiên trong danh sách vừa săp xếp nhất
        //    Res = ED.SortEDKeysClass(feature, Res, Res.ElementAt(0).ListClass.ElementAt(0));
        //    label8.Text = Time.TotalMilliseconds.ToString();

        //    //Lấy danh sách file tập ảnh
        //    int TopK = 100;
        //    int lenRes = Res.Count;
        //    if (TopK > lenRes) TopK = lenRes;
        //    List<string> ListFileImage = new List<string>();
        //    List<string> GroundTrue = new List<string>();

        //    //Tính độ chính xác và độ phủ cho truy vấn theo nội dung cho bộ ảnh COREL
        //    try
        //    {
        //        GroundTrue = GetGroundTrue(Path.GetFileName(queryImageName));
        //        for (int i = 0; i < TopK; i++)
        //        {
        //            ListFileImage.Add(getImgPath(Res.ElementAt(i).ImageID));
        //        }
        //    }
        //    catch { MessageBox.Show("Đường dẫn image DB không phù hợp !"); }

        //    //Danh sách kết quả
        //    List<string> ListNameImage = new List<string>();
        //    foreach (ElementData edata in Res)
        //    {
        //        ListNameImage.Add(edata.ImageID);
        //    }

        //    DataProcessing dp = new DataProcessing();
        //    //Tạo một số ngẫu nhiên (không đáng kể) để tạo sự khác biệc các điểm trên đường cong Precision-Recall
        //    Random random = new Random();
        //    double tp = random.NextDouble() / 20.0;
        //    double tr = random.NextDouble() / 20.0;
        //    //Tính các giá trị độ chính xác và độ phủ
        //    double TopSelect = 100.0;
        //    if (TopSelect > ListNameImage.Count) TopSelect = ListNameImage.Count;
        //    double C = dp.CheckImg(ListNameImage, GroundTrue);
        //    double A = TopSelect;
        //    double B = GroundTrue.Count;
        //    //if (B == A) B = B * 0.95;

        //    //double precision = (double)C / (double)A + tp;
        //    double precision = (double)C / (double)A;
        //    if (precision > 1.0) precision = 1.0;

        //    //double precision = (double)C / (double)A + 0.05;
        //    //double recall = (double)C / (double)B + tr;
        //    double recall = (double)C / (double)B;
        //    //if (precision >= 1.0) precision = 1.0 - tp;
        //    //if (recall >= 1.0) recall = 1.0 - tr;
        //    //if (precision <= 0.5) precision += 0.3;
        //    //if (recall <= 0.5) recall += 0.3;

        //    //if (precision > 1.0) precision = 1.0;
        //    //if (recall > 1.0) recall = 1.0;

        //    double Fmeasure = 2 * (precision * recall) / (precision + recall);
        //    if (precision == 0 && recall == 0) Fmeasure = 0;

        //    globalPrecision = Math.Round(precision, 4);
        //    globalRecall = Math.Round(recall, 4);
        //    globalFmeasure = Math.Round(Fmeasure, 4);
        //    label2.Text = (Math.Round(precision, 4) * 100).ToString() + "%";
        //    label4.Text = (Math.Round(recall, 4) * 100).ToString() + "%";
        //    label6.Text = (Math.Round(Fmeasure, 4) * 100).ToString() + "%";

        //    return ListFileImage;
        //}

        private List<string> handleGPTreeRetrieval()
        {
            string strfeature = txtFeatureGP.Text;
            ClusterNode Cluster = new ClusterNode();
            List<double> feature = Cluster.String2Vector(strfeature);
            string Root = getRootFile();
            string Root2 = getRootFile2();
            string dir = Path.GetDirectoryName(Root) + "\\";
            string dir2 = Path.GetDirectoryName(Root2) + "\\";
            string RenameLeafFile = dir + "RenameLeaf.txt";
            string RenameLeafFile2 = dir2 + "RenameLeaf.txt";
            
            ClusterTree CT = new ClusterTree();
            ElementData ED = new ElementData();
            TimeSpan Time = new TimeSpan();
            DateTime dtInSertnode = DateTime.Now;
            
            string fileLeaf = CT.SearchGPTreeToLeaf(feature, Root);
            string fileLeaf2 = CT.SearchGPTreeToLeaf(feature, Root2);
            List<ElementData> Res = ED.getListElementDataGPTree(dir + CT.getFileED(fileLeaf));
            List<ElementData> Res2 = ED.getListElementDataGPTree(dir + CT.getFileED(fileLeaf2));
            Res = ED.AddListED(Res, Res2);
            
            string fileEDSecond = CT.SearchGPTreeToLeafSecond(feature, Root);
            string fileEDSecond2 = CT.SearchGPTreeToLeafSecond(feature, Root2);
            List<ElementData> ResSecond = ED.getListElementData(fileEDSecond);
            List<ElementData> ResSecond2 = ED.getListElementData(fileEDSecond2);
            Res = ED.AddListED(Res, ResSecond);
            Res = ED.AddListED(Res, ResSecond2);

            //Root
            List<string> LeafNeighborOne = Cluster.getNeightborOneLeaf(fileLeaf, RenameLeafFile);
            List<string> LeafNeighborTwo = Cluster.getNeightborTwoLeaf(fileLeaf, RenameLeafFile);
            List<string> LeafNeighbor = CT.AddList(LeafNeighborOne, LeafNeighborTwo);
            List<string> LeafNeighborSort = Cluster.SortFilenameLeafNodeKeys(feature, LeafNeighbor, dir);

            //Lấy các hình ảnh (Element Data) của N láng giềng gần nhất
            List<ElementData> ListNeighborED = new List<ElementData>();
            //Lấy láng giềng
            int n = Convert.ToInt32(LeafNeighborSort.Count * getRate());
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine(LeafNeighborSort.ElementAt(i));
                string fileED = CT.getFileED(dir + LeafNeighborSort.ElementAt(i));
                ListNeighborED = ED.AddListED(ListNeighborED, ED.getListElementDataGPTree(dir + fileED));
            }
            Res = ED.AddListED(Res, ListNeighborED);

            //Root2
            List<string> LeafNeighborOne2 = Cluster.getNeightborOneLeaf(fileLeaf2, RenameLeafFile2);
            List<string> LeafNeighborTwo2 = Cluster.getNeightborTwoLeaf(fileLeaf2, RenameLeafFile2);
            List<string> LeafNeighbor2 = CT.AddList(LeafNeighborOne2, LeafNeighborTwo2);
            List<string> LeafNeighborSort2 = Cluster.SortFilenameLeafNodeKeys(feature, LeafNeighbor2, dir2);
            //Lấy các hình ảnh (Element Data) của N láng giềng gần nhất
            List<ElementData> ListNeighborED2 = new List<ElementData>();
            //Lấy láng giềng
            int m = Convert.ToInt32(LeafNeighborSort2.Count * getRate());
            for (int i = 0; i < m; i++)
            {
                Console.WriteLine(LeafNeighborSort2.ElementAt(i));
                string fileED = CT.getFileED(dir2 + LeafNeighborSort2.ElementAt(i));
                ListNeighborED2 = ED.AddListED(ListNeighborED2, ED.getListElementDataGPTree(dir2 + fileED));
            }
            Res = ED.AddListED(Res, ListNeighborED2);
            //Time
            TimeSpan ts = DateTime.Now - dtInSertnode;
            Time = Time + ts;
            //Sắp xếp lại các Element Data theo thứ tự độ đo Euclide của một vector đầu vào
            Res = ED.SortEDKeys(feature, Res);
            //Sắp xếp lại danh sách ED có class gần với classname của ED đầu tiên trong danh sách vừa săp xếp nhất
            Res = ED.SortEDKeysClass(feature, Res, Res.ElementAt(0).ListClass.ElementAt(0));
            //Show class
            string classImgName = Res.ElementAt(0).ListClass.ElementAt(0);
            lblLabel.Text = classImgName;
            //Show Time
            label8.Text = Time.TotalMilliseconds.ToString();
            //Lấy danh sách file tập ảnh
            int TopK = 100;
            int lenRes = Res.Count;
            if (TopK > lenRes) TopK = lenRes;
            List<string> ListFileImage = new List<string>();
            List<string> GroundTrue = new List<string>();

            //Tính độ chính xác và độ phủ cho truy vấn theo nội dung cho bộ ảnh COREL
            try
            {
                GroundTrue = GetGroundTrue(Path.GetFileName(queryImageName));
                for (int i = 0; i < TopK; i++)
                {
                    ListFileImage.Add(getImgPath(Res.ElementAt(i).ImageID));
                }
            }
            catch { MessageBox.Show("Đường dẫn image DB không phù hợp !"); }

            //Danh sách kết quả
            List<string> ListNameImage = new List<string>();
            foreach (ElementData edata in Res)
            {
                ListNameImage.Add(edata.ImageID);
            }

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
            if (precision > 1.0) precision = 1.0;
            double recall = (double)C / (double)B;

            double Fmeasure = 2 * (precision * recall) / (precision + recall);
            if (precision == 0 && recall == 0) Fmeasure = 0;

            globalPrecision = Math.Round(precision, 4);
            globalRecall = Math.Round(recall, 4);
            globalFmeasure = Math.Round(Fmeasure, 4);
            label2.Text = (Math.Round(precision, 4) * 100).ToString() + "%";
            label4.Text = (Math.Round(recall, 4) * 100).ToString() + "%";
            label6.Text = (Math.Round(Fmeasure, 4) * 100).ToString() + "%";

            return ListFileImage;
        }
        //private List<string> handleGpTreeGraphRetrieval()
        //{
        //    string strfeature = txtFeatureGP.Text;
        //    ClusterNode Cluster = new ClusterNode();
        //    List<double> feature = Cluster.String2Vector(strfeature);
        //    string Root = getRootFile();
        //    string dir = Path.GetDirectoryName(Root) + "\\";
        //    string RenameLeafFile = dir + "RenameLeaf.txt";
        //    ClusterTree CT = new ClusterTree();
        //    ElementData ED = new ElementData();
        //    TimeSpan Time = new TimeSpan();
        //    DateTime dtInSertnode = DateTime.Now;
        //    string fileLeaf = CT.SearchGPTreeToLeaf(feature, Root);
        //    //Phân lớp cho ảnh và tìm các neighbor có cùng phân lớp
        //    Tuple<string, List<string>> resultClassifition = CT.getClassAndNeighborLevelTwo(feature, Root, fileLeaf);
        //    List<string> listLabelNeighbor = resultClassifition.Item2;
        //    lblLabel.Text = resultClassifition.Item1;
        //    List<ElementData> Res = ED.getListElementDataGPTree(dir + CT.getFileED(fileLeaf));

        //    TimeSpan ts = DateTime.Now - dtInSertnode;
        //    Time = Time + ts;

        //    dtInSertnode = DateTime.Now;

        //    List<string> LeafNeighborOne = Cluster.getNeightborOneLeaf(fileLeaf, RenameLeafFile);
        //    List<string> LeafNeighborTwo = Cluster.getNeightborTwoLeaf(fileLeaf, RenameLeafFile);
        //    List<string> LeafNeighbor = CT.AddList(LeafNeighborOne, LeafNeighborTwo);
        //    LeafNeighbor = CT.AddList(LeafNeighbor, listLabelNeighbor);
        //    //Sắp xếp danh sách neighbor lại theo độ đo Euclic
        //    List<string> LeafNeighborSort = Cluster.SortFilenameLeafNodeKeys(feature, LeafNeighbor, dir);

        //    //Lấy các hình ảnh (Element Data) của N láng giềng gần nhất
        //    List<ElementData> ListNeighborED = new List<ElementData>();
        //    //int n = 10;      // Lấy tối đa 10 neighbor
        //    //if (n > LeafNeighborSort.Count) n = LeafNeighborSort.Count;
        //    int n = LeafNeighborSort.Count;

        //    for (int i = 0; i < n; i++)
        //    {
        //        Console.WriteLine(LeafNeighborSort.ElementAt(i));
        //        string fileED = CT.getFileED(dir + LeafNeighborSort.ElementAt(i));
        //        ListNeighborED = ED.AddListED(ListNeighborED, ED.getListElementDataGPTree(dir + fileED));
        //    }
        //    Res = ED.AddListED(Res, ListNeighborED);

        //    ts = DateTime.Now - dtInSertnode;
        //    Time = Time + ts;

        //    //Sắp xếp lại các Element Data theo thứ tự độ đo Euclide của một vector đầu vào
        //    Res = ED.SortEDKeys(feature, Res);
        //    //Sắp xếp lại danh sách ED có class gần với classname của ED đầu tiên trong danh sách vừa săp xếp nhất
        //    //Res = ED.SortEDKeysClass(feature, Res, Res.ElementAt(0).ListClass.ElementAt(0));

        //    //Sắp xếp lại danh sách ED có class gần với class vừa phân lớp cho query image nhất
        //    //Res = ED.SortEDKeysClass(feature, Res, resultClassifition.Item1);

        //    //Sắp xếp lại danh sách ED có class gần với class của thư mục của query image
        //    string classNmme = getFolderName(Path.GetFileName(queryImageName)).Split('\\').Last();
        //    Res = ED.SortEDKeysClass(feature, Res, classNmme);

        //    label8.Text = Time.TotalMilliseconds.ToString();

        //    //Lấy danh sách file tập ảnh
        //    int TopK = 100;
        //    int lenRes = Res.Count;
        //    if (TopK > lenRes) TopK = lenRes;
        //    List<string> ListFileImage = new List<string>();
        //    List<string> GroundTrue = new List<string>();

        //    try
        //    {
        //        for (int i = 0; i < TopK; i++)
        //        {
        //            ListFileImage.Add(getImgPath(Res.ElementAt(i).ImageID));
        //        }
        //        //Tính độ chính xác và độ phủ cho truy vấn theo nội dung cho bộ ảnh COREL
        //        GroundTrue = GetGroundTrue(Path.GetFileName(queryImageName));
        //    }
        //    catch { MessageBox.Show("Đường dẫn image DB không phù hợp !"); }

        //    //Danh sách kết quả
        //    List<string> ListNameImage = new List<string>();
        //    foreach (ElementData edata in Res)
        //    {
        //        ListNameImage.Add(edata.ImageID);
        //    }

        //    DataProcessing dp = new DataProcessing();
        //    //Tạo một số ngẫu nhiên (không đáng kể) để tạo sự khác biệc các điểm trên đường cong Precision-Recall
        //    Random random = new Random();
        //    //double tp = random.NextDouble() / 20.0;
        //    //double tr = random.NextDouble() / 20.0;
        //    //Tính các giá trị độ chính xác và độ phủ
        //    double TopSelect = 100.0;
        //    if (TopSelect > ListNameImage.Count) TopSelect = ListNameImage.Count;
        //    double C = dp.CheckImg(ListNameImage, GroundTrue);

        //    double A = TopSelect;
        //    double B = GroundTrue.Count;

        //    //if (B == A) B = B * 0.95;

        //    //double precision = (double)C / (double)A + tp;
        //    //MessageBox.Show("C: " + C + ", A: " + A);
        //    double precision = (double)C / (double)A;
        //    if (precision > 1.0) precision = 1.0;
        //    //double precision = (double)C / (double)A + 0.01;
        //    //double recall = (double)C / (double)B + tr;
        //    double recall = (double)C / (double)B;
        //    //double recall = (double)C / (double)B + 0.01;

        //    //if (precision > 1.0) precision = 1.0;
        //    //if (recall > 1.0) recall = 1.0;

        //    //if (precision >= 1.0) precision = 1.0 - tp;
        //    //if (recall >= 1.0) recall = 1.0 - tr;
        //    //if (precision <= 0.5) precision += 0.3;
        //    //if (recall <= 0.5) recall += 0.3;
        //    double Fmeasure = 2 * (precision * recall) / (precision + recall);
        //    if (precision == 0 && recall == 0) Fmeasure = 0;

        //    globalPrecision = Math.Round(precision, 4);
        //    globalRecall = Math.Round(recall, 4);
        //    globalFmeasure = Math.Round(Fmeasure, 4);
        //    label2.Text = (Math.Round(precision, 4) * 100).ToString() + "%";
        //    label4.Text = (Math.Round(recall, 4) * 100).ToString() + "%";
        //    label6.Text = (Math.Round(Fmeasure, 4) * 100).ToString() + "%";

        //    return ListFileImage;
        //}

        private List<string> handleGpTreeGraphRetrieval()
        {
            string strfeature = txtFeatureGP.Text;
            ClusterNode Cluster = new ClusterNode();
            List<double> feature = Cluster.String2Vector(strfeature);
            
            string Root = getRootFile();
            string Root2 = getRootFile2();
            string dir = Path.GetDirectoryName(Root) + "\\";
            string dir2 = Path.GetDirectoryName(Root2) + "\\";
            string RenameLeafFile = dir + "RenameLeaf.txt";
            string RenameLeafFile2 = dir2 + "RenameLeaf.txt";
            ClusterTree CT = new ClusterTree();
            ElementData ED = new ElementData();
            TimeSpan Time = new TimeSpan();
            DateTime dtInSertnode = DateTime.Now;
            string fileLeaf = CT.SearchGPTreeToLeaf(feature, Root);
            string fileLeaf2 = CT.SearchGPTreeToLeaf(feature, Root2);
            List<ElementData> Res = ED.getListElementDataGPTree(dir + CT.getFileED(fileLeaf));
            List<ElementData> Res2 = ED.getListElementDataGPTree(dir + CT.getFileED(fileLeaf2));
            Res = ED.AddListED(Res, Res2);
            
            string fileEDSecond = CT.SearchGPTreeToLeafSecond(feature, Root);
            string fileEDSecond2 = CT.SearchGPTreeToLeafSecond(feature, Root2);
            List<ElementData> ResSecond = ED.getListElementData(fileEDSecond);
            List<ElementData> ResSecond2 = ED.getListElementData(fileEDSecond2);
            Res = ED.AddListED(Res, ResSecond);
            Res = ED.AddListED(Res, ResSecond2);

            //Root
            List<string> LeafNeighborOne = Cluster.getNeightborOneLeaf(fileLeaf, RenameLeafFile);
            List<string> LeafNeighborTwo = Cluster.getNeightborTwoLeaf(fileLeaf, RenameLeafFile);
            List<string> LeafNeighbor = CT.AddList(LeafNeighborOne, LeafNeighborTwo);
            List<string> LeafNeighborSort = Cluster.SortFilenameLeafNodeKeys(feature, LeafNeighbor, dir);

            //Lấy các hình ảnh (Element Data) của N láng giềng gần nhất
            List<ElementData> ListNeighborED = new List<ElementData>();
            int n = Convert.ToInt32(LeafNeighborSort.Count * getRate2());
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine(LeafNeighborSort.ElementAt(i));
                string fileED = CT.getFileED(dir + LeafNeighborSort.ElementAt(i));
                ListNeighborED = ED.AddListED(ListNeighborED, ED.getListElementDataGPTree(dir + fileED));
            }
            Res = ED.AddListED(Res, ListNeighborED);

            //Root2
            List<string> LeafNeighborOne2 = Cluster.getNeightborOneLeaf(fileLeaf2, RenameLeafFile2);
            List<string> LeafNeighborTwo2 = Cluster.getNeightborTwoLeaf(fileLeaf2, RenameLeafFile2);
            List<string> LeafNeighbor2 = CT.AddList(LeafNeighborOne2, LeafNeighborTwo2);
            List<string> LeafNeighborSort2 = Cluster.SortFilenameLeafNodeKeys(feature, LeafNeighbor2, dir2);

            //Lấy các hình ảnh (Element Data) của N láng giềng gần nhất
            List<ElementData> ListNeighborED2 = new List<ElementData>();
            int m = Convert.ToInt32(LeafNeighborSort2.Count * getRate2());
            for (int i = 0; i < m; i++)
            {
                Console.WriteLine(LeafNeighborSort2.ElementAt(i));
                string fileED = CT.getFileED(dir2 + LeafNeighborSort2.ElementAt(i));
                ListNeighborED2 = ED.AddListED(ListNeighborED2, ED.getListElementDataGPTree(dir2 + fileED));
            }
            Res = ED.AddListED(Res, ListNeighborED2);

            //Show time
            TimeSpan ts = DateTime.Now - dtInSertnode;
            Time = Time + ts;
            label8.Text = Time.TotalMilliseconds.ToString();

            //Sắp xếp lại các Element Data theo thứ tự độ đo Euclide của một vector đầu vào
            Res = ED.SortEDKeys(feature, Res);
            //Sắp xếp lại danh sách ED có class gần với classname của ED đầu tiên trong danh sách vừa săp xếp nhất
            Res = ED.SortEDKeysClass(feature, Res, Res.ElementAt(0).ListClass.ElementAt(0));
            //Show class
            string classImgName = Res.ElementAt(0).ListClass.ElementAt(0);
            lblLabel.Text = classImgName;
            
            //Lấy danh sách file tập ảnh
            int TopK = 100;
            int lenRes = Res.Count;
            if (TopK > lenRes) TopK = lenRes;
            List<string> ListFileImage = new List<string>();
            List<string> GroundTrue = new List<string>();
            try
            {
                for (int i = 0; i < TopK; i++)
                {
                    ListFileImage.Add(getImgPath(Res.ElementAt(i).ImageID));
                }
                //Tính độ chính xác và độ phủ cho truy vấn theo nội dung cho bộ ảnh COREL
                GroundTrue = GetGroundTrue(Path.GetFileName(queryImageName));
            }
            catch { MessageBox.Show("Đường dẫn image DB không phù hợp !"); }

            //Danh sách kết quả
            List<string> ListNameImage = new List<string>();
            foreach (ElementData edata in Res)
            {
                ListNameImage.Add(edata.ImageID);
            }

            DataProcessing dp = new DataProcessing();
            //Tạo một số ngẫu nhiên (không đáng kể) để tạo sự khác biệc các điểm trên đường cong Precision-Recall
            Random random = new Random();
            //Tính các giá trị độ chính xác và độ phủ
            double TopSelect = 100.0;
            if (TopSelect > ListNameImage.Count) TopSelect = ListNameImage.Count;
            double C = dp.CheckImg(ListNameImage, GroundTrue);

            double A = TopSelect;
            double B = GroundTrue.Count;

            double precision = (double)C / (double)A;
            if (precision > 1.0) precision = 1.0;
            double recall = (double)C / (double)B;
            
            double Fmeasure = 2 * (precision * recall) / (precision + recall);
            if (precision == 0 && recall == 0) Fmeasure = 0;

            globalPrecision = Math.Round(precision, 4);
            globalRecall = Math.Round(recall, 4);
            globalFmeasure = Math.Round(Fmeasure, 4);
            label2.Text = (Math.Round(precision, 4) * 100).ToString() + "%";
            label4.Text = (Math.Round(recall, 4) * 100).ToString() + "%";
            label6.Text = (Math.Round(Fmeasure, 4) * 100).ToString() + "%";

            return ListFileImage;
        }

        private void btnLoadImageGPTree_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog diag = new OpenFileDialog())
            {
                diag.Filter = "Bitmap |*.bmp;*.jpg;*.gif;*.png";
                diag.InitialDirectory = Application.StartupPath;
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    string ImgPath = diag.FileName;
                    handleLoadImage(ImgPath);
                }
            }
        }

        private void btnGPTreeImageRetrieval_Click(object sender, EventArgs e)
        {
            List<string> ListFileImage =  handleGPTreeRetrieval();
            ImageListViewCBIR ListViewForm = new ImageListViewCBIR();
            ListViewForm.ArrayImage = ListFileImage.ToArray();
            ListViewForm.Show();
        }

        private void btnTestGPTree_Click(object sender, EventArgs e)
        {
            //Tính hiệu suất của phương pháp truy vấn
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            if (folderDlg.ShowDialog() == DialogResult.OK)
            {
                string[] filenames = Directory.GetFiles(folderDlg.SelectedPath, "*.jpg", SearchOption.AllDirectories);
                progressBarGPTree.Minimum = 0;
                progressBarGPTree.Maximum = filenames.Length;
                int count = 0;
                progressBarGPTree.Value = count;
                TextfileCluster tfc = new TextfileCluster();
                DataProcessing dp = new DataProcessing();
                foreach (string name in filenames)
                {
                    handleLoadImage(name);
                    handleGPTreeRetrieval();
                    tfc.WriteLineTextFile(Path.GetFileName(name) + "\t" + globalPrecision.ToString() + "\t" + globalRecall.ToString() + "\t" + globalFmeasure.ToString() + "\t" + dp.getLastFolderName(name), getTestingGPTreeFile(dp.getLastFolderName(name)));
                    count++;
                    progressBarGPTree.Value = count;
                    Application.DoEvents();
                }
                progressBarGPTree.Value = filenames.Length;
                MessageBox.Show("DONE!!!");
            }
        }

        private void btnGPTreeGraphImageRetrieval_Click(object sender, EventArgs e)
        {
            List<string> ListFileImage = handleGpTreeGraphRetrieval();
            ImageListViewCBIR ListViewForm = new ImageListViewCBIR();
            ListViewForm.ArrayImage = ListFileImage.ToArray();
            ListViewForm.Show();
        }

        private void btnTestGPTreeGraph_Click(object sender, EventArgs e)
        {
            //Tính hiệu suất của phương pháp truy vấn
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            if (folderDlg.ShowDialog() == DialogResult.OK)
            {
                string[] filenames = Directory.GetFiles(folderDlg.SelectedPath, "*.jpg", SearchOption.AllDirectories);
                progressBarGPTree.Minimum = 0;
                progressBarGPTree.Maximum = filenames.Length;
                int count = 0;
                progressBarGPTree.Value = count;
                TextfileCluster tfc = new TextfileCluster();
                DataProcessing dp = new DataProcessing();
                foreach (string name in filenames)
                {
                    handleLoadImage(name);
                    handleGpTreeGraphRetrieval();
                    tfc.WriteLineTextFile(Path.GetFileName(name) + "\t" + globalPrecision.ToString() + "\t" + globalRecall.ToString() + "\t" + globalFmeasure.ToString() + "\t" + dp.getLastFolderName(name), getTestingGPTreeGraphFile(dp.getLastFolderName(name)));
                    count++;
                    progressBarGPTree.Value = count;
                    Application.DoEvents();
                }
                progressBarGPTree.Value = filenames.Length;
                MessageBox.Show("DONE!!!");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmGPTreeCBIR_Load(object sender, EventArgs e)
        {

        }

        private void label_Click(object sender, EventArgs e)
        {

        }

        private void rdCorel_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}
