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
    public partial class frmGP_TreeSBIR : Form
    {
        string fileClass = @"../../data/SBIR-Classes.txt";
        public static Ontology O;
        public frmGP_TreeSBIR()
        {
            InitializeComponent();
        }

        public void DisnableOntologyQuery()
        {
            btnQueryText.Enabled = false;
            btnUNIONQueryText.Enabled = false;
            btnQueryImg.Enabled = false;
            btnUNIONQueryImg.Enabled = false;
            btnImageRetrieval.Enabled = false;
            lsvClasses.Enabled = false;
        }
        public void EnableOntologyQuery()
        {
            btnQueryText.Enabled = true;
            btnUNIONQueryText.Enabled = true;
            btnQueryImg.Enabled = true;
            btnUNIONQueryImg.Enabled = true;
            btnImageRetrieval.Enabled = true;
            lsvClasses.Enabled = true;
        }
        public void LoadOntology()
        {
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;

            string fileOnto = getOntologyFile();
            progressBar1.Value = 10;
            Application.DoEvents();

            O = new Ontology(fileOnto);
            progressBar1.Value = 80;
            Application.DoEvents();

            EnableOntologyQuery();
            progressBar1.Value = 100;
            Application.DoEvents();
        }

        public double getRate()
        {
            double rate = 0.0;
            if (radioButton1.Checked == true)
                rate = 0.8;
            else if (radioButton2.Checked == true)
                rate = 0.6;
            else if (radioButton3.Checked == true)
                rate = 0.4;
            else if (radioButton4.Checked == true)
                rate = 0.4;
            return rate;
        }
        public double getRate2()
        {
            double rate = 0.0;
            if (radioButton1.Checked == true)
                rate = 0.9;
            else if (radioButton2.Checked == true)
                rate = 0.7;
            else if (radioButton3.Checked == true)
                rate = 0.5;
            else if (radioButton4.Checked == true)
                rate = 0.5;
            return rate;
        }
        public string getRootFileGPTree()
        {
            string Root = string.Empty;
            if (radioButton1.Checked == true)
                Root = @"../../GPTree/GPTree_COREL/Root.txt";
            else if (radioButton2.Checked == true)
                Root = @"../../GPTree/GPTree_Wang/Root.txt";
            else if (radioButton3.Checked == true)
                Root = @"../../GPTree/GPTree_StanfordDogs/Root.txt";
            else if (radioButton4.Checked == true)
                Root = @"../../GPTree/GPTree_ImageCLEF/Root.txt";
            return Root;
        }

        public string getRootFile()
        {
            string Root = string.Empty;
            if (radioButton1.Checked == true)
                Root = @"../../GPTree/GPTree_COREL/Root.txt";
            else if (radioButton2.Checked == true)
                Root = @"../../GPTree/GPTree_Wang/Root.txt";
            else if (radioButton3.Checked == true)
                Root = @"../../GPTree/GPTree_StanfordDogs/Root.txt";
            else if (radioButton4.Checked == true)
                Root = @"../../GPTree/GPTree_ImageCLEF/Root.txt";
            return Root;
        }

        public string getRootFile2()
        {
            string Root = string.Empty;
            if (radioButton1.Checked == true)
                Root = @"../../GPTree/GPTree_COREL2/Root.txt";
            else if (radioButton2.Checked == true)
                Root = @"../../GPTree/GPTree_Wang2/Root.txt";
            else if (radioButton3.Checked == true)
                Root = @"../../GPTree/GPTree_StanfordDogs2/Root.txt";
            else if (radioButton4.Checked == true)
                Root = @"../../GPTree/GPTree_ImageCLEF2/Root.txt";
            return Root;
        }

        public string getRootFileCTree()
        {
            string Root = string.Empty;
            if (radioButton1.Checked == true)
                Root = @"../../C-TreeCOREL/Root.txt";
            else if (radioButton2.Checked == true)
                Root = @"../../C-TreeWang/Root.txt";
            else if (radioButton3.Checked == true)
                Root = @"../../C-TreeStanfordDogs/Root.txt";
            else if (radioButton4.Checked == true)
                Root = @"../../C-TreeImageCLEF/Root.txt";
            return Root;
        }
        public string getRootFileCTree2()
        {
            string Root = string.Empty;
            if (radioButton1.Checked == true)
                Root = @"../../C-TreeCOREL2/Root.txt";
            else if (radioButton2.Checked == true)
                Root = @"../../C-TreeWang2/Root.txt";
            else if (radioButton3.Checked == true)
                Root = @"../../C-TreeStanfordDogs2/Root.txt";
            else if (radioButton4.Checked == true)
                Root = @"../../C-TreeImageCLEF2/Root.txt";
            return Root;
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
        public string getOntologyFile()
        {
            string Onfile = string.Empty;
            if (radioButton1.Checked == true)
                Onfile = @"../../OntologyCOREL/Ontology.n3";
            else if (radioButton2.Checked == true)
                Onfile = @"../../OntologyWang/Ontology.n3";
            else if (radioButton3.Checked == true)
                Onfile = @"../../OntologyStanfordDogs/Ontology.n3";
            else if (radioButton4.Checked == true)
                Onfile = @"../../OntologyImageCLEF/Ontology.n3";
            //else if (radioButton5.Checked == true)
            //    Onfile = @"../../OntologyImageDBs/Ontology.n3";
            return Onfile;
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

        public string[] RemoveBlank(string[] Names)
        {
            List<string> L = new List<string>();
            if (Names == null) return null;
            if (Names.Length == 0) return null;
            foreach (string str in Names)
            {
                string s = str.Trim();
                if (s.ToUpper() != "")
                    L.Add(s);
            }
            return L.ToArray();
        }
        private List<string> getListName()
        {
            string text = txtQuery.Text;
            text = text.ToUpper();
            char[] delimiters = new char[] { '@', '\t', '\r', '\n', ' ', ',', '.' };
            string[] Classes = text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            Classes = RemoveBlank(Classes);
            string strListClasses = File.ReadAllText(fileClass);
            char[] del = new char[] { '@', '\t', '\r', '\n', ' ' };
            string[] strClasses = strListClasses.Split(del, StringSplitOptions.RemoveEmptyEntries);

            List<string> LClass = new List<string>();
            foreach (string cla in Classes)
            {
                if (strClasses.Contains(cla))
                {
                    LClass.Add(cla);
                    txtSPARQL.Text += cla + " ";
                }
            }
            return LClass;
        }
        private string CreateSPPARQLimg(List<string> LClass)
        {
            string SPARQL = string.Empty;

            SPARQL = @"PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>
PREFIX owl: <http://www.w3.org/2002/07/owl#>
PREFIX xml: <http://www.w3.org/XML/1998/namespace>
PREFIX sbir: <http://sbir-hcm.vn/>
SELECT DISTINCT ?Subject 
WHERE{";
            int len = LClass.Count;
            if (len > 0)
            {
                for (int i = 0; i < len; i++)
                {
                    SPARQL += "{?Subject" + " sbir:op" + LClass.ElementAt(i) + " sbir:in" + LClass.ElementAt(i) + " .}" + "\n";
                }
            }
            SPARQL += "}";
            return SPARQL;
        }

        private string CreateUnionSPPARQLimg(List<string> LClass)
        {
            string SPARQL = string.Empty;

            SPARQL = @"PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>
PREFIX owl: <http://www.w3.org/2002/07/owl#>
PREFIX xml: <http://www.w3.org/XML/1998/namespace>
PREFIX sbir: <http://sbir-hcm.vn/>
SELECT DISTINCT ?Subject 
WHERE{";
            int len = LClass.Count;
            if (len > 0)
            {
                for (int i = 0; i < len - 1; i++)
                {
                    SPARQL += "{?Subject" + " sbir:op" + LClass.ElementAt(i) + " sbir:in" + LClass.ElementAt(i) + " .}" + " UNION " + "\n";
                }
                SPARQL += "{?Subject" + " sbir:op" + LClass.ElementAt(len - 1) + " sbir:in" + LClass.ElementAt(len - 1) + " .}";
            }
            SPARQL += "}";
            return SPARQL;
        }

        private string CreateSPPARQfindImage(List<string> LClass)
        {
            string SPARQL = string.Empty;

            SPARQL = @"PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>
PREFIX owl: <http://www.w3.org/2002/07/owl#>
PREFIX xml: <http://www.w3.org/XML/1998/namespace>
PREFIX sbir: <http://sbir-hcm.vn/>
SELECT DISTINCT ?ImgName
WHERE{";
            int len = LClass.Count;
            if (len > 0)
            {
                for (int i = 0; i < len; i++)
                {
                    SPARQL += "\n {?IMG" + " sbir:imgName ?ImgName. " + "\n" +
                    "?IMG" + " sbir:op" + LClass.ElementAt(i) + " sbir:in" + LClass.ElementAt(i) + " . " + "\n" +
                    " sbir:in" + LClass.ElementAt(i) + " rdf:type owl:NamedIndividual . " + "\n" +
                    " sbir:in" + LClass.ElementAt(i) + " rdf:type " + "sbir:" + LClass.ElementAt(i) + " . }";
                }
            }
            SPARQL += "}";
            return SPARQL;
        }

        private string CreateUnionSPPARQfindImage(List<string> LClass)
        {
            string SPARQL = string.Empty;

            SPARQL = @"PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>
PREFIX owl: <http://www.w3.org/2002/07/owl#>
PREFIX xml: <http://www.w3.org/XML/1998/namespace>
PREFIX sbir: <http://sbir-hcm.vn/>
SELECT DISTINCT ?ImgName
WHERE{";
            int len = LClass.Count;
            if (len > 0)
            {
                for (int i = 0; i < len - 1; i++)
                {
                    SPARQL += "\n {?IMG" + " sbir:imgName ?ImgName. " + "\n" +
                    "?IMG" + " sbir:op" + LClass.ElementAt(i) + " sbir:in" + LClass.ElementAt(i) + " . " + "\n" +
                    " sbir:in" + LClass.ElementAt(i) + " rdf:type owl:NamedIndividual . " + "\n" +
                    " sbir:in" + LClass.ElementAt(i) + " rdf:type " + "sbir:" + LClass.ElementAt(i) + " . }" + " UNION ";
                }
                SPARQL += "\n {?IMG" + " sbir:imgName ?ImgName. " + "\n" +
                    "?IMG" + " sbir:op" + LClass.ElementAt(len - 1) + " sbir:in" + LClass.ElementAt(len - 1) + " . " + "\n" +
                    " sbir:in" + LClass.ElementAt(len - 1) + " rdf:type owl:NamedIndividual . " + "\n" +
                    " sbir:in" + LClass.ElementAt(len - 1) + " rdf:type " + "sbir:" + LClass.ElementAt(len - 1) + " . }";
            }
            SPARQL += "}";
            return SPARQL;
        }

        private string CreateSPARQLImgClass(string ClassName)
        {
            string SPARQL = string.Empty;

            SPARQL = @"PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>
PREFIX owl: <http://www.w3.org/2002/07/owl#>
PREFIX xml: <http://www.w3.org/XML/1998/namespace>
PREFIX sbir: <http://sbir-hcm.vn/>
SELECT DISTINCT ?IMG
WHERE";
            SPARQL += "{" + "sbir:" + "in" + ClassName + " sbir:inImage" + " ?IMG" + " .}";
            return SPARQL;
        }

        private string CreateSPPARQLKeywordsClass(string ClassName)
        {
            string SPARQL = string.Empty;

            SPARQL = @"PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>
PREFIX owl: <http://www.w3.org/2002/07/owl#>
PREFIX xml: <http://www.w3.org/XML/1998/namespace>
PREFIX sbir: <http://sbir-hcm.vn/>
SELECT DISTINCT ?WORDS 
WHERE{";
            SPARQL += "sbir:" + "in" + ClassName + " sbir:inKeywords" + " ?WORDS" + " .}";
            return SPARQL;
        }

        private string CreateSPPARQLDesClass(string ClassName)
        {
            string SPARQL = string.Empty;

            SPARQL = @"PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>
PREFIX owl: <http://www.w3.org/2002/07/owl#>
PREFIX xml: <http://www.w3.org/XML/1998/namespace>
PREFIX sbir: <http://sbir-hcm.vn/>
SELECT DISTINCT ?DES
WHERE{";
            SPARQL += "sbir:" + ClassName + " sbir:anoDescription" + " ?DES" + " .}";
            return SPARQL;
        }

        private string CreateSPPARQLFileClass(string ClassName)
        {
            string SPARQL = string.Empty;

            SPARQL = @"PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>
PREFIX owl: <http://www.w3.org/2002/07/owl#>
PREFIX xml: <http://www.w3.org/XML/1998/namespace>
PREFIX sbir: <http://sbir-hcm.vn/>
SELECT DISTINCT ?FILE
WHERE{";
            SPARQL += "sbir:" + ClassName + " sbir:anoFilename" + " ?FILE" + " .}";
            return SPARQL;
        }

        private string CreateSPPARQLURIClass(string ClassName)
        {
            string SPARQL = string.Empty;

            SPARQL = @"PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>
PREFIX owl: <http://www.w3.org/2002/07/owl#>
PREFIX xml: <http://www.w3.org/XML/1998/namespace>
PREFIX sbir: <http://sbir-hcm.vn/>
SELECT DISTINCT ?URI
WHERE{";
            SPARQL += "sbir:" + ClassName + " sbir:anoURI" + " ?URI" + " .}";
            return SPARQL;
        }

        private string ListStr2String(List<string> ListStr)
        {
            if (ListStr == null) return string.Empty;
            if (ListStr.Count == 0) return string.Empty;
            string str = string.Empty;
            int len = ListStr.Count;
            for (int i = 0; i < len - 1; i++)
                str += ListStr.ElementAt(i) + ", ";
            str += ListStr.ElementAt(len - 1);
            return str;
        }

        private string getImagePath(string IMGname)
        {
            string SPARQL = string.Empty;

            SPARQL = @"PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>
PREFIX owl: <http://www.w3.org/2002/07/owl#>
PREFIX xml: <http://www.w3.org/XML/1998/namespace>
PREFIX sbir: <http://sbir-hcm.vn/>
SELECT DISTINCT ?ImgPath
WHERE{";
            SPARQL += "sbir:" + IMGname + " " + "sbir:imgPath" + " " + "?ImgPath" + " ." + "}";
            List<string> LResult;
            string strkq = string.Empty;

            if (SPARQL != string.Empty)
            {
                LResult = O.QuerySPARQL(SPARQL);
                foreach (string res in LResult)
                    strkq += res + " ";
            }
            return strkq.Trim();
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
                    System.Drawing.Image Img = System.Drawing.Image.FromFile(ImgPath);
                    pictureBox1.Image = Img;

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

                    textBox1.Text = features;
                    lsvClasses.Clear();
                    txtQuery.Text = string.Empty;
                }
            }
        }

        private void btnGPTreeClassification_Click(object sender, EventArgs e)
        {
            string strfeature = textBox1.Text;
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
            int m = Convert.ToInt32(LeafNeighborSort2.Count * getRate());
            for (int i = 0; i < m; i++)
            {
                Console.WriteLine(LeafNeighborSort2.ElementAt(i));
                string fileED = CT.getFileED(dir2 + LeafNeighborSort2.ElementAt(i));
                ListNeighborED2 = ED.AddListED(ListNeighborED2, ED.getListElementDataGPTree(dir2 + fileED));
            }
            Res = ED.AddListED(Res, ListNeighborED2);
            //Sort
            Res = ED.SortEDKeys(feature, Res);
            Res = ED.SortEDKeysClass(feature, Res, Res.ElementAt(0).ListClass.ElementAt(0));
            //Time
            TimeSpan ts = DateTime.Now - dtInSertnode;
            Time = Time + ts;

            lsvClasses.Clear();
            txtQuery.Text = string.Empty;

            //Lấy tên ảnh đại diện
            string imageName = Res.ElementAt(0).ImageID;
            //Lấy danh sách lớp đại diện, chọn lớp của các ảnh gần nhất
            List<string> ListClassRes = ED.getListClass(Res);
            //Xuât C lớp đại diện
            int C = 5;
            if (C > ListClassRes.Count) C = ListClassRes.Count;
            for (int nC = 0; nC < C; nC++)
                lsvClasses.Items.Add(ListClassRes.ElementAt(nC));
            //Lấy danh sách lớp đại diện, chọn lớp của các ảnh gần nhất
            int k = 2;
            if (Res.Count < k) k = Res.Count;
            string classImgName = string.Empty;
            for (int i = 0; i < k; i++)
            {
                foreach (string cla in Res.ElementAt(i).ListClass)
                    if (classImgName.Contains(cla) == false)
                        classImgName += cla + " ";
            }
            txtQuery.Text = classImgName.Trim();
            pictureBox2.Image = Image.FromFile(getImgPath2(imageName));
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSgGPTreeClassification_Click(object sender, EventArgs e)
        {
            string strfeature = textBox1.Text;
            ClusterNode Cluster = new ClusterNode();
            List<double> feature = Cluster.String2Vector(strfeature);
            string Root = getRootFileCTree();
            string Root2 = getRootFileCTree2();
            string dir = Path.GetDirectoryName(Root) + "\\";
            string dir2 = Path.GetDirectoryName(Root2) + "\\";
            string RenameLeafFile = dir + "RenameLeaf.txt";
            string RenameLeafFile2 = dir2 + "RenameLeaf.txt";

            ClusterTree CT = new ClusterTree();
            ElementData ED = new ElementData();
            //Truy vấn trên cây
            string fileLeaf = CT.SearchCTreetoLeaf(feature, Root);
            string fileLeaf2 = CT.SearchCTreetoLeaf(feature, Root2);
            List<ElementData> Res = ED.getListElementData(dir + CT.getFileED(fileLeaf));
            List<ElementData> Res2 = ED.getListElementData(dir2 + CT.getFileED(fileLeaf2));
            Res = ED.AddListED(Res, Res2);

            string fileEDSecond = CT.SearchCTreetoFileEDSecond(feature, Root);
            string fileEDSecond2 = CT.SearchCTreetoFileEDSecond(feature, Root2);
            List<ElementData> ResSecond = ED.getListElementData(fileEDSecond);
            List<ElementData> ResSecond2 = ED.getListElementData(fileEDSecond2);
            Res = ED.AddListED(Res, ResSecond);
            Res = ED.AddListED(Res, ResSecond2);

            //Lấy láng giềng lần 1
            List<string> LeafNeighborOneCT = Cluster.getNeightborOneLeaf(fileLeaf, RenameLeafFile);
            List<string> LeafNeighborTwoCT = Cluster.getNeightborTwoLeaf(fileLeaf, RenameLeafFile);
            List<string> LeafNeighborCT = CT.AddList(LeafNeighborOneCT, LeafNeighborTwoCT);
            List<string> LeafNeighborCTSort = Cluster.SortFilenameLeafNodeKeys(feature, LeafNeighborCT, dir);

            //Lấy các hình ảnh (Element Data) của N láng giềng gần nhất
            List<ElementData> ListNeighborEDCT = new List<ElementData>();
            int m = 5;
            if (m > LeafNeighborCTSort.Count)
                m = LeafNeighborCTSort.Count;
            for (int i = 0; i < m; i++)
            {
                string fileED = CT.getFileED(dir + LeafNeighborCTSort.ElementAt(i));
                ListNeighborEDCT = ED.AddListED(ListNeighborEDCT, ED.getListElementData(dir + fileED));
            }
            Res = ED.AddListED(Res, ListNeighborEDCT);

            //Lấy láng giềng lần 2
            List<string> LeafNeighborOneCT2 = Cluster.getNeightborOneLeaf(fileLeaf2, RenameLeafFile2);
            List<string> LeafNeighborTwoCT2 = Cluster.getNeightborTwoLeaf(fileLeaf2, RenameLeafFile2);
            List<string> LeafNeighborCT2 = CT.AddList(LeafNeighborOneCT2, LeafNeighborTwoCT2);
            List<string> LeafNeighborCTSort2 = Cluster.SortFilenameLeafNodeKeys(feature, LeafNeighborCT2, dir2);

            //Lấy các hình ảnh (Element Data) của N láng giềng gần nhất
            List<ElementData> ListNeighborEDCT2 = new List<ElementData>();
            m = 5;
            if (m > LeafNeighborCTSort2.Count)
                m = LeafNeighborCTSort2.Count;

            for (int i = 0; i < m; i++)
            {
                string fileED = CT.getFileED(dir2 + LeafNeighborCTSort2.ElementAt(i));
                ListNeighborEDCT2 = ED.AddListED(ListNeighborEDCT2, ED.getListElementData(dir2 + fileED));
            }
            Res = ED.AddListED(Res, ListNeighborEDCT2);

            //Tìm kiếm ảnh dựa trên SOM: tìm cụm chiến thắng lần 1
            string[] SOMLeaf = Directory.GetFiles(dir, "Leaf" + "*" + ".txt");
            int len = SOMLeaf.Length;
            //Chọn cụm chiến thắng
            string WinnerLeaf = SOMLeaf[0];
            List<double> WeightWinner = Cluster.getWeightED(WinnerLeaf);
            for (int i = 0; i < len; i++)
            {
                List<double> weightLeaf = Cluster.getWeightED(SOMLeaf[i]);
                double a1 = Cluster.disVectorL1(feature, weightLeaf);
                double a2 = Cluster.disVectorL1(feature, WeightWinner);
                if (Cluster.sigmoid(a1) < Cluster.sigmoid(a2))
                {
                    WinnerLeaf = SOMLeaf[i];
                    WeightWinner = weightLeaf;
                }
            }
            //Lấy danh sách các phần tử cụm chiến thắng
            List<ElementData> ResSOM = ED.getListElementData(dir + CT.getFileED(WinnerLeaf));
            Res = ED.AddListED(Res, ResSOM);

            //Lấy láng giềng cụm chiến thắng
            List<string> LeafNeighborOne = Cluster.getNeightborOneLeaf(WinnerLeaf, RenameLeafFile);
            List<string> LeafNeighborTwo = Cluster.getNeightborTwoLeaf(WinnerLeaf, RenameLeafFile);
            List<string> LeafNeighbor = CT.AddList(LeafNeighborOne, LeafNeighborTwo);

            List<string> LeafNeighborSort = Cluster.SortFilenameLeafNodeKeys(feature, LeafNeighbor, dir);

            //Lấy các hình ảnh (Element Data) của N láng giềng gần nhất
            List<ElementData> ListNeighborED = new List<ElementData>();
            int n = 5;
            if (n > LeafNeighborSort.Count)
                n = LeafNeighborSort.Count;

            for (int i = 0; i < n; i++)
            {
                string fileED = CT.getFileED(dir + LeafNeighborSort.ElementAt(i));
                ListNeighborED = ED.AddListED(ListNeighborED, ED.getListElementData(dir + fileED));
            }
            Res = ED.AddListED(Res, ListNeighborED);

            //Tìm kiếm ảnh dựa trên SOM: tìm cụm chiến thắng lần 2
            string[] SOMLeaf2 = Directory.GetFiles(dir2, "Leaf" + "*" + ".txt");
            int len2 = SOMLeaf2.Length;
            //Chọn cụm chiến thắng
            string WinnerLeaf2 = SOMLeaf2[0];
            List<double> WeightWinner2 = Cluster.getWeightED(WinnerLeaf2);
            for (int i = 0; i < len2; i++)
            {
                List<double> weightLeaf = Cluster.getWeightED(SOMLeaf2[i]);
                double a1 = Cluster.disVectorL1(feature, weightLeaf);
                double a2 = Cluster.disVectorL1(feature, WeightWinner2);
                if (Cluster.sigmoid(a1) < Cluster.sigmoid(a2))
                {
                    WinnerLeaf2 = SOMLeaf2[i];
                    WeightWinner2 = weightLeaf;
                }
            }
            //Lấy danh sách các phần tử cụm chiến thắng
            List<ElementData> ResSOM2 = ED.getListElementData(dir2 + CT.getFileED(WinnerLeaf2));
            Res = ED.AddListED(Res, ResSOM2);

            //Lấy láng giềng cụm chiến thắng
            List<string> LeafNeighborOne2 = Cluster.getNeightborOneLeaf(WinnerLeaf2, RenameLeafFile2);
            List<string> LeafNeighborTwo2 = Cluster.getNeightborTwoLeaf(WinnerLeaf2, RenameLeafFile2);
            List<string> LeafNeighbor2 = CT.AddList(LeafNeighborOne2, LeafNeighborTwo2);

            List<string> LeafNeighborSort2 = Cluster.SortFilenameLeafNodeKeys(feature, LeafNeighbor2, dir2);

            //Lấy các hình ảnh (Element Data) của N láng giềng gần nhất
            List<ElementData> ListNeighborED2 = new List<ElementData>();
            n = 5;
            if (n > LeafNeighborSort2.Count)
                n = LeafNeighborSort2.Count;

            for (int i = 0; i < n; i++)
            {
                string fileED = CT.getFileED(dir2 + LeafNeighborSort2.ElementAt(i));
                ListNeighborED2 = ED.AddListED(ListNeighborED2, ED.getListElementData(dir2 + fileED));
            }
            Res = ED.AddListED(Res, ListNeighborED2);

            Res = ED.SortEDKeys(feature, Res);
            Res = ED.SortEDKeysClass(feature, Res, Res.ElementAt(0).ListClass.ElementAt(0));

            lsvClasses.Clear();
            txtQuery.Text = string.Empty;

            //Lấy tên ảnh đại diện
            string imageName = Res.ElementAt(0).ImageID;
            //Lấy danh sách lớp đại diện, chọn lớp của các ảnh gần nhất
            List<string> ListClassRes = ED.getListClass(Res);
            //Xuât C lớp đại diện
            int C = 5;
            if (C > ListClassRes.Count) C = ListClassRes.Count;

            for (int nC = 0; nC < C; nC++)
                lsvClasses.Items.Add(ListClassRes.ElementAt(nC));
            //Lấy danh sách lớp đại diện, chọn lớp của các ảnh gần nhất
            int k = 2;
            if (Res.Count < k) k = Res.Count;
            string classImgName = string.Empty;
            for (int i = 0; i < k; i++)
            {
                foreach (string cla in Res.ElementAt(i).ListClass)
                    if (classImgName.Contains(cla) == false)
                        classImgName += cla + " ";
            }
            txtQuery.Text = classImgName.Trim();
            pictureBox2.Image = Image.FromFile(getImgPath2(imageName));
        }

        private void btnGPTreeGraphClassification_Click(object sender, EventArgs e)
        {
            string strfeature = textBox1.Text;
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
            int m = Convert.ToInt32(LeafNeighborSort2.Count * getRate());
            for (int i = 0; i < m; i++)
            {
                Console.WriteLine(LeafNeighborSort2.ElementAt(i));
                string fileED = CT.getFileED(dir2 + LeafNeighborSort2.ElementAt(i));
                ListNeighborED2 = ED.AddListED(ListNeighborED2, ED.getListElementDataGPTree(dir2 + fileED));
            }
            Res = ED.AddListED(Res, ListNeighborED2);
            //Sort
            Res = ED.SortEDKeys(feature, Res);
            Res = ED.SortEDKeysClass(feature, Res, Res.ElementAt(0).ListClass.ElementAt(0));
            //Time
            TimeSpan ts = DateTime.Now - dtInSertnode;
            Time = Time + ts;

            lsvClasses.Clear();
            txtQuery.Text = string.Empty;
            //Lấy tên ảnh đại diện
            string imageName = Res.ElementAt(0).ImageID;
            //Lấy danh sách lớp đại diện, chọn lớp của các ảnh gần nhất
            List<string> ListClassRes = ED.getListClass(Res);
            //Xuât C lớp đại diện
            int C = 5;
            if (C > ListClassRes.Count) C = ListClassRes.Count;
            for (int nC = 0; nC < C; nC++)
                lsvClasses.Items.Add(ListClassRes.ElementAt(nC));
            //Lấy danh sách lớp đại diện, chọn lớp của các ảnh gần nhất
            int k = 2;
            if (Res.Count < k) k = Res.Count;
            string classImgName = string.Empty;
            for (int i = 0; i < k; i++)
            {
                foreach (string cla in Res.ElementAt(i).ListClass)
                    if (classImgName.Contains(cla) == false)
                        classImgName += cla + " ";
            }
            txtQuery.Text = classImgName.Trim();
            pictureBox2.Image = Image.FromFile(getImgPath2(imageName));
        }

        private void btnQueryText_Click(object sender, EventArgs e)
        {
            List<string> LClass = getListName();
            string SPARQL = string.Empty;
            if (LClass.Count > 0)
                SPARQL = CreateSPPARQLimg(LClass);
            txtSPARQL.Text = SPARQL;

            List<string> LResult;
            if (SPARQL != string.Empty)
            {
                LResult = O.QuerySPARQL(SPARQL);
                int count = 0;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = LResult.Count;
                string strkq = string.Empty;
                foreach (string res in LResult)
                {
                    strkq += res + "\r\n";
                    count++;
                    progressBar1.Value = count;
                }
                progressBar1.Value = LResult.Count;
                txtResult.Text = strkq;
            }
        }

        private void btnUNIONQueryText_Click(object sender, EventArgs e)
        {
            List<string> LClass = getListName();
            string SPARQL = string.Empty;
            if (LClass.Count > 0)
                SPARQL = CreateUnionSPPARQLimg(LClass);
            txtSPARQL.Text = SPARQL;

            //Ontology O = new Ontology(fileOnto);
            List<string> LResult;
            if (SPARQL != string.Empty)
            {
                LResult = O.QuerySPARQL(SPARQL);
                int count = 0;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = LResult.Count;
                string strkq = string.Empty;
                foreach (string res in LResult)
                {
                    strkq += res + "\r\n";
                    count++;
                    progressBar1.Value = count;
                }
                progressBar1.Value = LResult.Count;
                txtResult.Text = strkq;
            }
        }

        private void btnQueryImg_Click(object sender, EventArgs e)
        {
            List<string> LClass = getListName();

            string SPARQL = string.Empty;
            if (LClass.Count > 0)
                SPARQL = CreateSPPARQfindImage(LClass);
            txtSPARQL.Text = SPARQL;

            List<string> LResult;
            if (SPARQL != string.Empty)
            {
                LResult = O.QuerySPARQL(SPARQL);

                int count = 0;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = LResult.Count;
                string strkq = string.Empty;
                foreach (string res in LResult)
                {
                    strkq += res + " " + "\n";
                    count++;
                    progressBar1.Value = count;
                }
                progressBar1.Value = LResult.Count;
                txtResult.Text = strkq;
            }
        }

        private void btnUNIONQueryImg_Click(object sender, EventArgs e)
        {
            List<string> LClass = getListName();

            string SPARQL = string.Empty;
            if (LClass.Count > 0)
                SPARQL = CreateUnionSPPARQfindImage(LClass);
            txtSPARQL.Text = SPARQL;

            List<string> LResult;
            if (SPARQL != string.Empty)
            {
                LResult = O.QuerySPARQL(SPARQL);

                int count = 0;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = LResult.Count;
                string strkq = string.Empty;
                foreach (string res in LResult)
                {
                    strkq += res + " " + "\n";
                    count++;
                    progressBar1.Value = count;
                }
                progressBar1.Value = LResult.Count;
                txtResult.Text = strkq;
            }
        }

        private void btnImageRetrieval_Click(object sender, EventArgs e)
        {
            List<string> FullNameFile = new List<string>();
            string LImg = txtResult.Text;
            LImg = LImg.Replace(".jpg", "");
            char[] delimiters = new char[] { '@', '\t', '\r', '\n', ' ', '/' };
            string[] ImgNames = LImg.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            ImgNames = RemoveBlank(ImgNames);
            foreach (string name in ImgNames)
                FullNameFile.Add(getImagePath("IMG" + name));
            ImageRetrievalSBIR ListViewForm = new ImageRetrievalSBIR();
            ListViewForm.ArrayImage = FullNameFile.ToArray();
            ListViewForm.O = new OntologyListViewSBIR(O.Parse, O.Graph, O.Writer);
            ListViewForm.Show();
        }

        private void btnLoadOntology_Click(object sender, EventArgs e)
        {
            LoadOntology();
            EnableOntologyQuery();
            btnLoadOntology.Enabled = false;
        }

        private void btnLoadDBsOntology_Click(object sender, EventArgs e)
        {
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;

            string fileOnto = @"../../OntologyImageDBs/Ontology.n3";
            progressBar1.Value = 10;
            Application.DoEvents();

            O = new Ontology(fileOnto);
            progressBar1.Value = 80;
            Application.DoEvents();

            EnableOntologyQuery();
            progressBar1.Value = 100;
            Application.DoEvents();

            EnableOntologyQuery();
            btnLoadOntology.Enabled = false;
            btnLoadDBsOntology.Enabled = false;
        }

        private void frmGP_TreeSBIR_Load(object sender, EventArgs e)
        {
            DisnableOntologyQuery();
        }

        private void lsvClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsvClasses.SelectedItems.Count == 0) return;
            ListViewItem item = lsvClasses.SelectedItems[0];
            string ClassName = item.SubItems[0].Text;
            string SPARQLImgClass = CreateSPARQLImgClass(ClassName.ToUpper());
            string SPARQLKeywords = CreateSPPARQLKeywordsClass(ClassName.ToUpper());
            string SPARQLDes = CreateSPPARQLDesClass(ClassName.ToUpper());
            string SPARQLFile = CreateSPPARQLFileClass(ClassName.ToUpper());
            string SPARQLURI = CreateSPPARQLURIClass(ClassName.ToUpper());

            //string SPARQL

            List<string> LResultImgClass = O.QuerySPARQL(SPARQLImgClass);
            string ImgFile = LResultImgClass.ElementAt(0);
            List<string> LResultKeywords = O.QuerySPARQL(SPARQLKeywords);
            string keywords = ListStr2String(LResultKeywords);
            List<string> LResultDes = O.QuerySPARQL(SPARQLDes);
            string DesClass = LResultDes.ElementAt(0);
            List<string> LResultFile = O.QuerySPARQL(SPARQLFile);
            string FileClass = LResultFile.ElementAt(0);
            List<string> LResultURI = O.QuerySPARQL(SPARQLURI);
            string URIClass = LResultURI.ElementAt(0);

            ////pictureBox1.Image = Image.FromFile(ImgFile);
            //foreach (Form aForm in Application.OpenForms)
            //{
            //    if (aForm.Name.Equals("frmInfoClass"))
            //    {
            //        aForm.Close();
            //    }
            //}
     
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
