using SBIR_Project.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBIR
{
    class ClusterNode:ClusterMethod
    {
        //Các thuộc tính trực tiếp
        private int numED = 0;//Số lượng phần tử trong một nút Cluster
        private int numEC = 0; //Số lượng phần tử là tâm của một cụm
        private List<ElementData> listED = new List<ElementData>(); //Danh sách các phần tử tại cụm
        private List<ElementCenter> listEC = new List<ElementCenter>();//Phần tử tâm của cụm (đại diệm cụm), nếu là lá thì danh sách này là rỗng
        private bool isLeaf = true; //Kiểm tra nút lá
        private bool isRoot = true; //Kiểm tra nút gốc
        private List<double> weightED = new List<double>(); //Vector trọng số huấn luyện, tri thức bổ sung của Element Data cho việc chọn cụm trong SOM, SVM, ANN, DNN nhằm gia tăng độ chính xác
        private List<double> weightEC = new List<double>(); //Vector trọng số huấn luyện, tri thức bổ sung của Element Center cho việc chọn cụm trong SOM, SVM, ANN, DNN nhằm gia tăng độ chính xác
        private string fileName = string.Empty; //tên tập tin lưu trữ thông tin của cụm
        private List<double> centerED = new List<double>(); // Vector tâm cụm của các phần tử dữ liệu Element Data
        private List<double> centerEC = new List<double>(); // Vector tâm cụm của các phần tử dữ tâm Element Center
        private int clusterID = 0; //ID của node
        private int parentID = 0; //ID của node cha
        private int level = 0; //Level của nút
        private double radius = 0.0; //bán kính của cụm
      
        private List<List<string>> listEDClass = new List<List<string>>();
        //Các thuộc tính liên kết
        private string fileNameED = string.Empty; //tập tin lưu trữ các phần tử dữ liệu Element Data
        private string fileNameEC = string.Empty; //tập tin lưu trữ các phần tử tâm Element Center
        private List<string> listFileNameCluster = new List<string>(); //Danh sách các tên file của các cụm liên kết để tạo đồ thị cụm
        private List<string> listFileNameChild = new List<string>();//Danh sách tên tập tin của các cụm con
        private string fileNameLevel = string.Empty; //tên tập tin lưu trữ thông tin về mức của cụm
        private string fileNameParent = string.Empty;//Tên tập tin lưu trữ thông tin của cụm cha

        //Tập tin lưu vết của việc thay đổi tên file
        public string RenameLeafFile = @"../../data/RenameLeafFile.txt";
        public string RenameNodeFile = @"../../data/RenameNodeFile.txt";
        public string FileClass = @"../../data/IsAClasses.txt";
        //public int M = 150;
        //public int N = 20;
        //public int m = 2;
        public int M = 10;
        public int N = 4;
        public int m = 2;
        public double theta = 0.11;
        public double epcilon = 0.01;
   
        public int MaxED
        {
            get { return M; }
            set { M = value; }
        }
        public int MaxEC
        {
            get { return N; }
            set { N = value; }
        }
        public int MinEDEC
        {
            get { return m; }
            set { m = value; }
        }
        public double Theta
        {
            get { return theta; }
            set { theta = value; }
        }

        public double Epcilon { get => epcilon; set => epcilon = value; }
        public int ClusterId { get => clusterID; set => clusterID = value; }
        public int ParentID { get => parentID; set => parentID = value; }
        public int Level { get => level; set => level = value; }
        public double Radius { get => radius; set => radius = value; }

        //Phương thức khởi tạo đối tượng không tham số
        public ClusterNode()
        {
            //Các thuộc tính trực tiếp
            this.numED = 0;//Số lượng phần tử trong một nút Cluster
            this.numEC = 0; //Số lượng phần tử là tâm của một cụm
            this.listED = new List<ElementData>(); //Danh sách các phần tử tại cụm
            this.listEC = new List<ElementCenter>();//Phần tử tâm của cụm (đại diệm cụm), nếu là lá thì danh sách này là rỗng
            this.isLeaf = true; //Kiểm tra nút lá
            this.isRoot = true; //Kiểm tra nút gốc
            this.weightED = new List<double>(); //Trọng số huấn luyện cho việc chọn cụm trong SOM, SVM, ANN, DNN
            this.weightEC = new List<double>(); //Trọng số huấn luyện cho việc chọn cụm trong SOM, SVM, ANN, DNN
            this.fileName = string.Empty; //tên tập tin lưu trữ thông tin của cụm
            this.centerED = new List<double>(); // Vector tâm cụm của các phần tử dữ liệu Element Data
            this.centerEC = new List<double>(); // Vector tâm cụm của các phần tử dữ tâm Element Center
            //Các thuộc tính liên kết
            this.fileNameED = string.Empty; //tập tin lưu trữ các phần tử dữ liệu Element Data
            this.fileNameEC = string.Empty; //tập tin lưu trữ các phần tử tâm Element Center
            this.listFileNameCluster = new List<string>(); //Danh sách các tên file của các cụm liên kết để tạo đồ thị cụm
            this.listFileNameChild = new List<string>();//Danh sách tên tập tin của các cụm con
            this.fileNameLevel = string.Empty; //tên tập tin lưu trữ thông tin về mức của cụm
            this.fileNameParent = string.Empty;//Tên tập tin lưu trữ thông tin của cụm cha
        }
        //Phương thức khởi tạo theo nút lá và nút gốc
        public ClusterNode(bool isLeaf, bool isRoot)
        {
            //Các thuộc tính trực tiếp
            this.numED = 0;//Số lượng phần tử trong một nút Cluster
            this.numEC = 0; //Số lượng phần tử là tâm của một cụm
            this.listED = new List<ElementData>(); //Danh sách các phần tử tại cụm
            this.listEC = new List<ElementCenter>();//Phần tử tâm của cụm (đại diệm cụm), nếu là lá thì danh sách này là rỗng
            this.isLeaf = isLeaf; //Kiểm tra nút lá
            this.isRoot = isRoot; //Kiểm tra nút gốc
            this.weightED = new List<double>(); //Trọng số huấn luyện cho việc chọn cụm trong SOM, SVM, ANN, DNN
            this.weightEC = new List<double>(); //Trọng số huấn luyện cho việc chọn cụm trong SOM, SVM, ANN, DNN
            this.fileName = string.Empty; //tên tập tin lưu trữ thông tin của cụm
            this.centerED = new List<double>(); // Vector tâm cụm của các phần tử dữ liệu Element Data
            this.centerEC = new List<double>(); // Vector tâm cụm của các phần tử dữ tâm Element Center
            //Các thuộc tính liên kết
            this.fileNameED = string.Empty; //tập tin lưu trữ các phần tử dữ liệu Element Data
            this.fileNameEC = string.Empty; //tập tin lưu trữ các phần tử tâm Element Center
            this.listFileNameCluster = new List<string>(); //Danh sách các tên file của các cụm liên kết để tạo đồ thị cụm
            this.listFileNameChild = new List<string>();//Danh sách tên tập tin của các cụm con
            this.fileNameLevel = string.Empty; //tên tập tin lưu trữ thông tin về mức của cụm
            this.fileNameParent = string.Empty;//Tên tập tin lưu trữ thông tin của cụm cha
        }
        //Phương thức khởi tạo theo nút lá và nút gốc có level
        public ClusterNode(bool isLeaf, bool isRoot, int level)
        {
            //Các thuộc tính trực tiếp
            this.numED = 0;//Số lượng phần tử trong một nút Cluster
            this.numEC = 0; //Số lượng phần tử là tâm của một cụm
            this.listED = new List<ElementData>(); //Danh sách các phần tử tại cụm
            this.listEC = new List<ElementCenter>();//Phần tử tâm của cụm (đại diệm cụm), nếu là lá thì danh sách này là rỗng
            this.isLeaf = isLeaf; //Kiểm tra nút lá
            this.isRoot = isRoot; //Kiểm tra nút gốc
            this.level = level; //Level của nút
            this.weightED = new List<double>(); //Trọng số huấn luyện cho việc chọn cụm trong SOM, SVM, ANN, DNN
            this.weightEC = new List<double>(); //Trọng số huấn luyện cho việc chọn cụm trong SOM, SVM, ANN, DNN
            this.fileName = string.Empty; //tên tập tin lưu trữ thông tin của cụm
            this.centerED = new List<double>(); // Vector tâm cụm của các phần tử dữ liệu Element Data
            this.centerEC = new List<double>(); // Vector tâm cụm của các phần tử dữ tâm Element Center
            //Các thuộc tính liên kết
            this.fileNameED = string.Empty; //tập tin lưu trữ các phần tử dữ liệu Element Data
            this.fileNameEC = string.Empty; //tập tin lưu trữ các phần tử tâm Element Center
            this.listFileNameCluster = new List<string>(); //Danh sách các tên file của các cụm liên kết để tạo đồ thị cụm
            this.listFileNameChild = new List<string>();//Danh sách tên tập tin của các cụm con
            this.fileNameLevel = string.Empty; //tên tập tin lưu trữ thông tin về mức của cụm
            this.fileNameParent = string.Empty;//Tên tập tin lưu trữ thông tin của cụm cha
        }
        //Phương thức khởi tạo Cluster node HTree
        public ClusterNode(int clusterID, int parentID, int level, bool isLeaf, bool isRoot, ElementData ED)
        {
            //Các thuộc tính trực tiếp
            this.numED = 0;//Số lượng phần tử trong một nút Cluster
            this.numEC = 0; //Số lượng phần tử là tâm của một cụm
            this.clusterID = clusterID; //Id của nút
            this.parentID = parentID; //Id của  cụm cha
            this.level = level; //Level của nút
            this.listED = new List<ElementData>() { ED }; //Danh sách các phần tử tại cụm
            this.listEC = null;//Phần tử tâm của cụm (đại diệm cụm), nếu là lá thì danh sách này là rỗng
            this.radius = GlobalVariable.Epsilon; //Bán kính của cụm
            this.isLeaf = isLeaf; //Kiểm tra nút lá
            this.isRoot = isRoot; //Kiểm tra nút gốc
            this.weightED = new List<double>(); //Trọng số huấn luyện cho việc chọn cụm trong SOM, SVM, ANN, DNN
            this.listEDClass = new List<List<string>>();
            this.weightEC = new List<double>(); //Trọng số huấn luyện cho việc chọn cụm trong SOM, SVM, ANN, DNN
            this.fileName = string.Empty; //tên tập tin lưu trữ thông tin của cụm
            this.centerED = ED.Feature; // Vector tâm cụm của các phần tử dữ liệu Element Data
            this.centerEC = new List<double>(); // Vector tâm cụm của các phần tử dữ tâm Element Center
            //Các thuộc tính liên kết
            this.fileNameED = string.Empty; //tập tin lưu trữ các phần tử dữ liệu Element Data
            this.fileNameEC = string.Empty; //tập tin lưu trữ các phần tử tâm Element Center
            this.listFileNameCluster = new List<string>(); //Danh sách các tên file của các cụm liên kết để tạo đồ thị cụm
            this.listFileNameChild = new List<string>();//Danh sách tên tập tin của các cụm con
            this.fileNameLevel = string.Empty; //tên tập tin lưu trữ thông tin về mức của cụm
            this.fileNameParent = string.Empty;//Tên tập tin lưu trữ thông tin của cụm cha
        }

        //Phương thức khởi tạo đối tượng có tham số đầu vào tương ứng với các thuộc tính
        public ClusterNode(int numED, int numEC, List<ElementData> listED, List<ElementCenter> listEC, bool isLeaf, bool isRoot, List<double> weightED, List<double> weightEC, string fileName, List<double> centerED, List<double> centerEC, string fileListED, string fileListEC, List<string> listFileCluster, List<string> listFileChild, string fileLevel, string fileParent)
        {
            //Các thuộc tính trực tiếp
            this.numED = numED;//Số lượng phần tử trong một nút Cluster
            this.numEC = numEC; //Số lượng phần tử là tâm của một cụm
            this.listED = listED; //Danh sách các phần tử tại cụm
            this.listEC = listEC;//Phần tử tâm của cụm (đại diệm cụm), nếu là lá thì danh sách này là rỗng
            this.isLeaf = isLeaf; //Kiểm tra nút lá
            this.isRoot = isRoot; //Kiểm tra nút gốc
            this.weightED = weightED; //Trọng số huấn luyện cho việc chọn cụm trong SOM, SVM, ANN, DNN
            this.weightEC = weightEC; //Trọng số huấn luyện cho việc chọn cụm trong SOM, SVM, ANN, DNN
            this.fileName = fileName; //tên tập tin lưu trữ thông tin của cụm
            this.centerED = centerED; // Vector tâm cụm của các phần tử dữ liệu Element Data
            this.centerEC = centerEC; // Vector tâm cụm của các phần tử dữ tâm Element Center
            //Các thuộc tính liên kết
            this.fileNameED = fileListED; //tập tin lưu trữ các phần tử dữ liệu Element Data
            this.fileNameEC = fileListEC; //tập tin lưu trữ các phần tử tâm Element Center
            this.listFileNameCluster = listFileCluster; //Danh sách các tên file của các cụm liên kết để tạo đồ thị cụm
            this.listFileNameChild = listFileChild;//Danh sách tên tập tin của các cụm con
            this.fileNameLevel = fileLevel; //tên tập tin lưu trữ thông tin về mức của cụm
            this.fileNameParent = fileParent;//Tên tập tin lưu trữ thông tin của cụm cha
        }
        //Phương thức khởi tạo đối tượng sao chép
        public ClusterNode(ClusterNode Cluster)
        {
            //Các thuộc tính trực tiếp
            this.numED = Cluster.NumED;//Số lượng phần tử trong một nút Cluster
            this.numEC = Cluster.NumEC; //Số lượng phần tử là tâm của một cụm
            this.listED = Cluster.ListED; //Danh sách các phần tử tại cụm
            this.listEC = Cluster.ListEC;//Phần tử tâm của cụm (đại diệm cụm), nếu là lá thì danh sách này là rỗng
            this.isLeaf = Cluster.IsLeaf; //Kiểm tra nút lá
            this.isRoot = Cluster.IsRoot; //Kiểm tra nút gốc
            this.weightED = Cluster.WeightED; //Trọng số huấn luyện cho việc chọn cụm trong SOM, SVM, ANN, DNN
            this.weightEC = Cluster.WeightEC; //Trọng số huấn luyện cho việc chọn cụm trong SOM, SVM, ANN, DNN
            this.fileName = Cluster.FileName; //tên tập tin lưu trữ thông tin của cụm
            this.centerED = Cluster.CenterED; // Vector tâm cụm của các phần tử dữ liệu Element Data
            this.centerEC = Cluster.CenterEC; // Vector tâm cụm của các phần tử dữ tâm Element Center
            //Các thuộc tính liên kết
            this.fileNameED = Cluster.FileListED; //tập tin lưu trữ các phần tử dữ liệu Element Data
            this.fileNameEC = Cluster.FileListEC; //tập tin lưu trữ các phần tử tâm Element Center
            this.listFileNameCluster = Cluster.ListFileCluster; //Danh sách các tên file của các cụm liên kết để tạo đồ thị cụm
            this.listFileNameChild = Cluster.ListFileChild;//Danh sách tên tập tin của các cụm con
            this.fileNameLevel = Cluster.FileLevel; //tên tập tin lưu trữ thông tin về mức của cụm
            this.fileNameParent = Cluster.FileParent;//Tên tập tin lưu trữ thông tin của cụm cha
        }
        //Các phương thức truy cập (dang Properties)
        public int NumED
        {
            get { return numED; }
            set { numED = value; }
        }
        public int NumEC
        {
            get { return numEC; }
            set { numEC = value; }
        }
        public List<ElementData> ListED
        {
            get { return listED; }
            set { listED = value; }
        }
        public List<ElementCenter> ListEC
        {
            get { return listEC; }
            set { listEC = value; }
        }
        public bool IsLeaf
        {
            get { return isLeaf; }
            set { isLeaf = value; }
        }
        public bool IsRoot
        {
            get { return isRoot; }
            set { isRoot = value; }
        }
        public List<double> WeightED
        {
            get { return weightED; }
            set { weightED = value; }
        }
        public List<double> WeightEC
        {
            get { return weightEC; }
            set { weightEC = value; }
        }
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        public List<double> CenterED
        {
            get { return centerED; }
            set { centerED = value; }
        }
        public List<double> CenterEC
        {
            get { return centerEC; }
            set { centerEC = value; }
        }
        //Các thuộc tính liên kết
        public string FileListED
        {
            get { return fileNameED; }
            set { fileNameED = value; }
        }
        public string FileListEC
        {
            get { return fileNameEC; }
            set { fileNameEC = value; }
        }
        public List<string> ListFileCluster
        {
            get { return listFileNameCluster; }
            set { listFileNameCluster = value; }
        }
        public List<string> ListFileChild
        {
            get { return listFileNameChild; }
            set { listFileNameChild = value; }
        }
        public string FileLevel
        {
            get { return fileNameLevel; }
            set { fileNameLevel = value; }
        }
        public string FileParent
        {
            get { return fileNameParent; }
            set { fileNameParent = value; }
        }

        //Lưu thông tin một cụm vào file văn bản, tạo một cụm và lưu trữ trên tập tin
        public void SaveCluster(string fileCluster)
        {
            string lineCluster = string.Empty;
            TextfileCluster tfc = new TextfileCluster(fileCluster);

            //Lưu trữ số lượng phần tử dữ liệu và phần tử tâm, tuy nhiên phải trùng số lượng trong dang sách tập tin lưu trữ ED và EC
            //lineCluster += numED.ToString() + "\r\n";
            //lineCluster += numEC.ToString() + "\r\n";
            
            //Lưu trữ giá trị kiểm tra (bool) nút lá và nút gốc (2 nút có vai trò trực tiếp để trích xuất dữ liệu)
            lineCluster += isLeaf.ToString() + " " + isRoot.ToString() + "\r\n";
            //Lưu trữ vector trọng số cho các phần tử dữ liệu ED, dùng để làm tri thức bổ sung trong truy vấn và huấn luyện
            lineCluster += Vector2String(weightED) + "\r\n";
            //Lưu trữ vectow trọng số cho các phần tử tâm EC, dùng để làm tri thức bổ sung trong truy vấn và huấn luyện
            lineCluster += Vector2String(weightEC) + "\r\n";
            //Lưu trữ tập tin lưu trữ cluster, là tập tin hiện hành (đầu vào)
            lineCluster += "(" + Path.GetFileName(fileCluster) + ")" + " ";
            //Tạo tập tin để lưu trữ các phần tử dữ liệu ED, nếu chưa tồn tại thì tạo ra một tập tin mới
            //Nếu tập tin này đã tồn tại thì lấy số lượng phần tử để cập nhật, nếu chưa tồn tại thì tạo tập tin mới
            if (IsLeaf == false)
                this.fileNameED = "#NONE#";
            else
            {
                if (this.fileNameED == string.Empty)
                {
                    this.fileNameED = "list" + Path.GetFileNameWithoutExtension(fileCluster) + "ED" + ".txt";
                    FileStream fs = null;
                    //if (!File.Exists(Path.GetDirectoryName(fileCluster) + "\\" + this.fileNameED))
                    //{
                        using (fs = File.Create(Path.GetDirectoryName(fileCluster) + "\\" + this.fileNameED)) { }
                    //}
                }
                else
                {
                    ElementData ED = new ElementData();
                    if (ED.getListElementData(Path.GetDirectoryName(fileCluster) + "\\" + this.fileNameED) != null)
                        this.listED = ED.getListElementData(Path.GetDirectoryName(fileCluster) + "\\" + this.fileNameED);
                    this.numED = this.listED.Count;
                }
            }
            //Lưu tên file lưu trữ các phần tử dữ liệu ED
            lineCluster += "(" + this.fileNameED + ")" + " ";
            //Tạo tập tin để lưu trữ các phần tử dữ liệu ED, nếu chưa tồn tại thì tạo ra một tập tin mới
            //Nếu tập tin này đã tồn tại thì lấy số lượng phần tử để cập nhật, nếu chưa tồn tại thì tạo tập tin mới
            //Nếu là nút lá thì không cần phải tạo ra tập tin này. (tức là bỏ qua)
            if (IsLeaf == true)
                this.fileNameEC = "#NONE#";
            else
            {
                if (this.fileNameEC == string.Empty)
                {
                    this.fileNameEC = "list" + Path.GetFileNameWithoutExtension(fileCluster) + "EC" + ".txt";
                    FileStream fs = null;
                    //if (!File.Exists(Path.GetDirectoryName(fileCluster) + "\\" + this.fileNameEC))
                    //{
                        using (fs = File.Create(Path.GetDirectoryName(fileCluster) + "\\" + this.fileNameEC)) { }
                    //}
                }
                else
                {
                    ElementCenter EC = new ElementCenter();
                    if (EC.getListElementCenter(Path.GetDirectoryName(fileCluster) + "\\" + this.fileNameEC) != null)
                        this.ListEC = EC.getListElementCenter(Path.GetDirectoryName(fileCluster) + "\\" + this.fileNameEC);
                    this.numEC = this.ListEC.Count;
                }
            }
            //Cập nhật só lượng phần tử ED và EC cho thông tin của Cluster
            lineCluster = numED.ToString() + "\r\n" + numEC.ToString() + "\r\n" + lineCluster;
            //Lưu tên file lưu trữ các phần tử dữ liệu EC
            lineCluster += "(" + this.fileNameEC + ")" + "\r\n";
            //Danh sách tên các tập tin của các cụm dữ liệu láng giềng theo đồ thị
            string tmp = string.Empty;
            if (listFileNameCluster.Count > 0)
                foreach (string file in listFileNameCluster)
                    tmp += "(" + file + ")" + " ";
            else
                tmp = "(#NONE#)";
            //Lưu danh sách tên các Cluster láng giềng
            lineCluster += tmp.Trim() + "\r\n";
            //Danh sách tên các tập tin của các cụm con (Child) theo phân cấp
            tmp = string.Empty;
            if (ListFileChild.Count > 0)
                foreach (string file in listFileNameChild)
                    tmp += "(" + file + ")" + " ";
            else
                tmp = "(#NONE#)";
            //Lưu danh sách tên các cụm con (Child) phân cấp
            lineCluster += tmp.Trim() + "\r\n";
            //Lưu tên file chứa các cụm đồng cấp để quản lý đồ thị theo cấp
            //Nếu một file Le
            if (fileNameLevel != string.Empty)
                lineCluster += "(" + fileNameLevel + ")" + " ";
            else
                lineCluster += "(#NONE#)" + " ";
            //Lưu tên file quản lý các cụm nút cha của nút hiện hành
            if (fileNameParent != string.Empty)
                lineCluster += "(" + fileNameParent + ")" + "\r\n";
            else
                lineCluster += "(#NONE#)" + "\r\n";
            //Lưu trữ vector tâm các phần tử dữ liệu ED
            lineCluster += Vector2String(centerED) + "\r\n";
            //Lưu trữ vector tâm các phần tử tâm EC
            lineCluster += Vector2String(centerEC);

            tfc.WriteNewTextFile(lineCluster);
        }

        /////////////////////Các phương thức thao tác trực tiếp trên tập tin chứa thông tin của Cluster//////////////////
       
        //Lấy số lượng phần tử Element Data
        public int getNumED(string fileCluster)
        {
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return 0;
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return 0;
            Lines = RemoveBlank(Lines);
            string line = Lines[0].Trim();
            return ToInt32(line);
        }
        //Cập nhật số lượng phần tử Element Data
        public void updateNumED(int numEDnew, string fileCluster)
        {
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return;
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return;
            Lines = RemoveBlank(Lines);
            Lines[0] = numEDnew.ToString();
            tfc.WriteNewTextFile(Lines);
        }
        //Lấy số lượng phần tử Element Center
        public int getNumEC(string fileCluster)
        {
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return 0;
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return 0;
            Lines = RemoveBlank(Lines);
            string line = Lines[1].Trim();
            return ToInt32(line);
        }
        //Câp nhật số lượng phần tử Element Center
        public void updateNumEC(int numECnew, string fileCluster)
        {
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return;
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return;
            Lines = RemoveBlank(Lines);
            Lines[1] = numECnew.ToString();
            tfc.WriteNewTextFile(Lines);
        }
        //Lấy giá trị isLeaf từ tập tin Cluster
        public bool getIsLeaf(string fileCluster)
        {
            bool isLeaf = true;
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return true;
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return true;
            string line = Lines[2];
            char[] delimiters = new char[] { '\t', '\r', '\n', ';', '!', ':', ',', ' ' };
            string[] words = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            words = RemoveBlank(words);
            if (words.Length < 1) return true;
            isLeaf = bool.Parse(words[0]);
            return isLeaf;
        }
        //Cập nhật giá trị isLeaf từ tập tin Cluster
        public void updateIsLeaf(bool isLeafnew, string fileCluster)
        {
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return;
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return;
            string line = Lines[2];
            char[] delimiters = new char[] { '\t', '\r', '\n', ';', '!', ':', ',', ' ' };
            string[] words = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            words = RemoveBlank(words);
            if (words.Length < 1) return;
            words[0] = isLeafnew.ToString();
            Lines[2] = StrArr2String(words);
            tfc.WriteNewTextFile(Lines);
        }
        //Lấy giá trị isRoot từ tập tin Cluster
        public bool getIsRoot(string fileCluster)
        {
            bool isRoot = true;
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return true;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return true;
            string line = Lines[2];
            char[] delimiters = new char[] { '\t', '\r', '\n', ';', '!', ':', ',', ' ' };
            string[] words = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            words = RemoveBlank(words);
            if (words.Length < 2) return true;
            isRoot = bool.Parse(words[1]);
            return isRoot;
        }
        //Cập nhật giá trị isRoot từ tập tin Cluster
        public void updateIsRoot(bool isRootnew, string fileCluster)
        {
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return;
            string line = Lines[2];
            char[] delimiters = new char[] { '\t', '\r', '\n', ';', '!', ':', ',', ' ' };
            string[] words = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            words = RemoveBlank(words);
            if (words.Length < 2) return;
            words[1] = isRootnew.ToString();
            Lines[2] = StrArr2String(words);
            tfc.WriteNewTextFile(Lines);
        }
        //Lấy giá trị trọng số cho tập Element Data từ tập tin Cluster
        public List<double> getWeightED(string fileCluster)
        {
            List<double> wED = new List<double>();
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return null;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return null;
            string line = Lines[3].Trim();
            wED = String2Vector(line);
            return wED;
        }
        //Cập nhật giá trị trọng số cho tập Element Data từ tập tin Cluster
        public void updateWeightED(List<double> weightEDnew, string fileCluster)
        {
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return;
            Lines[3] = Vector2String(weightEDnew);
            tfc.WriteNewTextFile(Lines);
        }
        //Lấy giá trị trọng số cho tập Element Center từ tập tin Cluster
        public List<double> getWeightEC(string fileCluster)
        {
            List<double> wEC = new List<double>();
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return null;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return null;
            string line = Lines[4].Trim();
            wEC = String2Vector(line);
            return wEC;
        }
        //Cập nhật giá trị trọng số cho tập Element Center từ tập tin Cluster
        public void updateWeightEC(List<double> weightECnew, string fileCluster)
        {
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return;
            Lines[4] = Vector2String(weightECnew);
            tfc.WriteNewTextFile(Lines);
        }
        //Lấy tên tập tin của Cluster 
        public string getFileCluster(string fileCluster)
        {
            string fileNameCluster = string.Empty;
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return string.Empty;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return string.Empty;
            string line = Lines[5];
            string[] words = line.Split('(', ')');
            words = RemoveBlank(words);
            if (words.Length < 1) return string.Empty;
            fileNameCluster = words[0];
            return fileNameCluster;
        }
        //Cập nhật tên tập tin của Cluster 
        public void updateFileCluster(string fileNameClusternew, string fileCluster)
        {
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return;
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return;
            string line = Lines[5];
            string[] words = line.Split('(', ')');
            words = RemoveBlank(words);
            if (words.Length < 1) return;
            words[0] = fileNameClusternew;
            Lines[5] = StrArr2Element(words);
            tfc.WriteNewTextFile(Lines);
        }
        //Lấy tên tập tin chứa các Element Data
        public string getFileED(string fileCluster)
        {
            string fileNameED = string.Empty;
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return string.Empty;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return string.Empty;
            string line = Lines[5];
            string[] words = line.Split('(', ')');
            words = RemoveBlank(words);
            if (words.Length < 2) return string.Empty;
            fileNameED = words[1];
            return fileNameED;
        }
        //Cập nhật tên tập tin chứa các Element Data
        public void updateFileED(string fileNameEDnew, string fileCluster)
        {
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return;
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return;
            string line = Lines[5];
            string[] words = line.Split('(', ')');
            words = RemoveBlank(words);
            if (words.Length < 2) return;
            words[1] = fileNameEDnew;
            Lines[5] = StrArr2Element(words);
            tfc.WriteNewTextFile(Lines);
        }
        //Lấy tên tập tin chứa các Element Center
        public string getFileEC(string fileCluster)
        {
            string fileNameEC = string.Empty;
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return string.Empty;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return string.Empty;
            string line = Lines[5];
            string[] words = line.Split('(', ')');
            words = RemoveBlank(words);
            if (words.Length < 3) return string.Empty;
            fileNameEC = words[2];
            return fileNameEC;
        }
        //Cập nhật tên tập tin chứa các Element Center
        public void updateFileEC(string fileNameECnew, string filename)
        {
            TextfileCluster tfc = new TextfileCluster(filename);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return;
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return;
            string line = Lines[5];
            string[] words = line.Split('(', ')');
            words = RemoveBlank(words);
            if (words.Length < 3) return;
            words[2] = fileNameECnew;
            Lines[5] = StrArr2Element(words);
            tfc.WriteNewTextFile(Lines);
        }
        //Lấy danh sách tên tập tin các Cluster láng giềng từ tập tin Cluster
        public List<string> getListFileCluster(string fileCluster)
        {
            List<string> listFileNameClusters = new List<string>();
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return null;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return null;
            string line = Lines[6];
            listFileNameClusters = String2WordsList(line);
            return listFileNameClusters;
        }
        //Cập nhật dang sách tên tập tin các Cluster láng giềng từ tập tin Cluster
        public void updateListFileCluster(List<string> listNameFilenew, string fileCluster)
        {
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return;
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return;
            Lines[6] = StrList2Element(listNameFilenew);
            tfc.WriteNewTextFile(Lines);
        }
        //Lấy dang sách tên tập tin các ClusterClild láng giềng từ tập tin Cluster
        public List<string> getListFileChild(string fileCluster)
        {
            List<string> ListFileChilds = new List<string>();
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return null;
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return null;
            string line = Lines[7];
            ListFileChilds = String2WordsList(line);
            return ListFileChilds;
        }
        //Cập nhật dang sách tên tập tin các ClusterClild láng giềng từ tập tin Cluster
        public void updateListFileChild(List<string> listFileNameChildnew, string fileCluster)
        {
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return;
            Lines[7] = StrList2Element(listFileNameChildnew);
            tfc.WriteNewTextFile(Lines);
        }
        //Lấy tên tập tin Level của Cluster 
        public string getFileLevel(string fileCluster)
        {
            string fileNameLevel = string.Empty;
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return string.Empty;
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return string.Empty;
            string line = Lines[8];
            string[] words = line.Split('(', ')');
            words = RemoveBlank(words);
            if (words.Length < 1) return string.Empty;
            fileNameLevel = words[0];
            return fileNameLevel;
        }
        //Cập nhật tên tập tin Level của Cluster 
        public void updateFileLevel(string fileNameLevelnew, string fileCluster)
        {
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return;
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return;
            string line = Lines[8];
            string[] words = line.Split('(', ')');
            words = RemoveBlank(words);
            if (words.Length < 1) return;
            words[0] = fileNameLevelnew;
            Lines[8] = StrArr2Element(words);
            tfc.WriteNewTextFile(Lines);
        }
        //Lấy tên tập tin Parent của Cluster 
        public string getFileParent(string fileCluster)
        {
            string fileNameParent = string.Empty;
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return string.Empty;
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return string.Empty;
            string line = Lines[8];
            string[] words = line.Split('(', ')');
            words = RemoveBlank(words);
            fileNameParent = words[1];
            return fileNameParent;
        }
        //Cập nhật tên tập tin Parent của Cluster 
        public void updateFileParent(string fileNameParentnew, string fileCluster)
        {
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return;
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return;
            string line = Lines[8];
            string[] words = line.Split('(', ')');
            words = RemoveBlank(words);
            if (words.Length < 2) return;
            words[1] = fileNameParentnew;
            Lines[8] = StrArr2Element(words);
            tfc.WriteNewTextFile(Lines);
        }
        //Lấy vector tâm của các thành phần dữ liệu Element Data
        public List<double> getCenterED(string fileCluster)
        {
            List<double> CenterED = new List<double>();
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return null;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return null;
            string line = Lines[9];
            CenterED = String2Vector(line);
            return CenterED;
        }
        //Cập nhật vector tâm các phần tử dữ liệu Element Data
        public void updateCenterED(List<double> CenterEDnew, string fileCluster)
        {
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return;
            Lines[9] = Vector2String(CenterEDnew);
            tfc.WriteNewTextFile(Lines);
        }
        //Lấy vector tâm của các thành phần tâm Element Center
        public List<double> getCenterEC(string fileCluster)
        {
            List<double> CenterEC = new List<double>();
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return null;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return null;
            string line = Lines[10];
            CenterEC = String2Vector(line);
            return CenterEC;
        }
        //Cập nhật vector tâm các phần tử tâm Element Center
        public void updateCenterEC(List<double> CenterECnew, string fileCluster)
        {
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return;
            Lines[10] = Vector2String(CenterECnew);
            tfc.WriteNewTextFile(Lines);
        }
        //Load một cụm từ một tập tin văn bản
        public void LoadCluster(string fileCluster)
        {
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            Lines = RemoveBlank(Lines);
            if (Lines == null) return;
            //Load số lượng phần tử ED và EC của cụm trên tập tin filename
            this.numED = ToInt32(Lines[0]);
            this.numEC = ToInt32(Lines[1]);
            //Load giá trị nút lá hoặc nút gốc
            char[] delimiters = new char[] { '\t', '\r', '\n', ';', '!', ':', ',', ' ' };
            string[] words = Lines[2].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            words = RemoveBlank(words);
            this.isLeaf = bool.Parse(words[0]);
            this.isRoot = bool.Parse(words[1]);
            //Load trọng số của các phần tử ED để thực hiện huấn luyện và làm tri thức bổ sung trong việc chọn cụm
            if (Lines[3] != "#NONE#")
                this.weightED = String2Vector(Lines[3]);
            //Load trọng số của các phần từ EC để thực hiện huấn luyện và làm tri thức bổ sung trong việc chọn cụm
            if (Lines[4] != "#NONE#")
                this.weightEC = String2Vector(Lines[4]);
            //Load tên file cả cụm, nếu chưa có tên thì lấy tên file đầu vào hiện tại
            words = Lines[5].Split('(', ')');
            words = RemoveBlank(words);
            if (words[0].Trim() == "#NONE#")
                this.fileName = Path.GetFileName(fileCluster);
            else
                this.fileName = words[0].Trim();
            //Lấy tên file chứa các phần tử ED, nếu chưa có thì tạo ra một file mới
            //Nếu đã có thì đọc các phần tử và cập nhật lại số lượng phần tử ED
            if (words[1].Trim() != "#NONE#")
            //{
            //    this.fileNameED = Path.GetFileNameWithoutExtension(fileCluster) + "ED" + ".txt";
            //    FileStream fs = null;
            //    if (!File.Exists(Path.GetDirectoryName(fileCluster) + "\\" + this.fileNameED))
            //    {
            //        using (fs = File.Create(Path.GetDirectoryName(fileCluster) + "\\" + this.fileNameED)) { }
            //    }
            //}
            //else
            {
                this.fileNameED = words[1].Trim();
                ElementData ED = new ElementData();
                if (ED.getListElementData(Path.GetDirectoryName(fileCluster) + "\\" + this.fileNameED) != null)
                    this.listED = ED.getListElementData(Path.GetDirectoryName(fileCluster) + "\\" + this.fileNameED);
                this.numED = this.listED.Count;
            }
            //Lấy tên file chứa các phần tử EC, nếu chưa có thì tạo ra một file mới
            //Nếu đã có file thì đọc các phần tử và cập nhật lại số lượng phần tử EC
            if (words[2].Trim() != "#NONE#")
            //{
            //    this.fileNameEC = Path.GetFileNameWithoutExtension(fileCluster) + "EC" + ".txt";
            //    FileStream fs = null;
            //    if (!File.Exists(Path.GetDirectoryName(fileCluster) + "\\" + this.fileNameEC))
            //    {
            //        using (fs = File.Create(Path.GetDirectoryName(fileCluster) + "\\" + this.fileNameED)) { }
            //    }
            //}
            //else
            {
                this.fileNameEC = words[2].Trim();
                ElementCenter EC = new ElementCenter();
                if (EC.getListElementCenter(Path.GetDirectoryName(fileCluster) + "\\" + this.fileNameEC) != null)
                    this.ListEC = EC.getListElementCenter(Path.GetDirectoryName(fileCluster) + "\\" + this.fileNameEC);
                this.numEC = this.ListEC.Count;

            }
            //Cập nhật lại số lượng phần tử ED và EC cho đúng với số lượng trong tập tin chứa các phần tử ED, EC
            updateNumED(this.numED, fileCluster);
            updateNumEC(this.numEC, fileCluster);
            //Lấy danh sách các tập tin Cluster láng giềng (đồng cấp), lưu ý mỗi file phân biệt bằng cặp dấu ngoặc "( )"
            if (Lines[6] != "(#NONE#)")
                this.listFileNameCluster = String2WordsList(Lines[6]);
            //Lấy danh sách các tên file chứa thông tin các cụm con
            //Danh sách tên file này phải trùng với danh sách tên file các phần tử EC
            if (Lines[7] != "(#NONE#)")
                this.listFileNameChild = String2WordsList(Lines[7]);
            words = Lines[8].Split('(', ')');
            words = RemoveBlank(words);
            //Lấy tên tập tin lưu trữ danh sách các Cụm đồng cấp, trong trường hợp là Root thì có giá trị là #NONE#
            if (words[0] != "#NONE#")
                this.fileNameLevel = words[0];
            //Lấy tên tập tin lưu trữ Cụm nút cha, trong trường hợp là Root thì có giá trị là #NONE#
            if (words[1] != "#NONE#")
                this.fileNameParent = words[1];
            //Lấy vector tâm của các phần tử dữ liệu ED
            if (Lines[9] != "#NONE#")
                this.centerED = String2Vector(Lines[9]);
            //Lấy vector tâm của các phần tử tâm EC
            if (Lines[10] != "#NONE#")
                this.centerEC = String2Vector(Lines[10]);
        }
        //Lấy (tìm kiếm) một phần tử dữ liệu Element Data từ một cụm dựa vào một ImageID
        public string[] getED(string ImageID, string fileCluster)
        {
            string fileED = Path.GetDirectoryName(fileCluster) + "\\" + getFileED(fileCluster);
            ElementData ED = new ElementData();
            return ED.getStrElementData(ImageID, fileED);
        }
        public string[] getED(string ImageID, List<double> feature, string fileCluster)
        {
            string fileED = Path.GetDirectoryName(fileCluster) + "\\" + getFileED(fileCluster);
            ElementData ED = new ElementData();
            return ED.getStrElementData(feature, ImageID, fileED);
        }
        public string getED2String(string ImageID, string fileCluster)
        {
            string fileED = Path.GetDirectoryName(fileCluster) + "\\" + getFileED(fileCluster);
            ElementData ED = new ElementData();
            return ED.getElementData2String(ImageID, fileED);
        }
        public string getED2String(string ImageID, List<double> feature, string fileCluster)
        {
            string fileED = Path.GetDirectoryName(fileCluster) + "\\" + getFileED(fileCluster);
            ElementData ED = new ElementData();
            return ED.getElementData2String(feature, ImageID, fileED);
        }
        //Lấy (tìm kiếm) một phần tử Element Center từ một cụm dựa vào một ImageID và values
        public string[] getEC(string fileChild, string fileCluster)
        {
            string fileEC = Path.GetDirectoryName(fileCluster) + "\\" + getFileEC(fileCluster);
            ElementCenter EC = new ElementCenter();
            return EC.getStrElementCenter(fileChild, fileEC);
        }
        public string[] getEC(string fileChild, List<double> values, string fileCluster)
        {
            string fileEC = Path.GetDirectoryName(fileCluster) + "\\" + getFileEC(fileCluster);
            ElementCenter EC = new ElementCenter();
            return EC.getStrElementCenter(values, fileChild, fileEC);
        }
        public string getEC2String(string fileChild, string fileCluster)
        {
            string fileEC = Path.GetDirectoryName(fileCluster) + "\\" + getFileEC(fileCluster);
            ElementCenter EC = new ElementCenter();
            return EC.getElementCenter2String(fileChild, fileEC);
        }
        public string getEC2String(string fileChild, List<double> values, string fileCluster)
        {
            string fileEC = Path.GetDirectoryName(fileCluster) + "\\" + getFileEC(fileCluster);
            ElementCenter EC = new ElementCenter();
            return EC.getElementCenter2String(values, fileChild, fileEC);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////Các phương thức thao tác trên các Cluster và làm thay đổi đến dữ liệu của các đối tượng khác////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Cập nhật tâm cụm từ cụm hiện hành đên cụm nút gốc
        public void UpdateClusterInCTree(string fileChild, string fileCluster)
        {
            //1. Cập nhật lại giá trị values cho phần tử tâm cụm EC trong dang sách listEC với nút con với tên tập tin là ClusterChild
            //2. Tính lại và cập nhật vector trung bình CenterEC của cụm hiện hành
            //3. Nếu cụm hiện hành không phải là cụm nút gốc thì cập nhật nút cha (ParentNode): (gọi lại 2 bước 1 và bước 2 ứng với nút cha), nghĩa là:
            // 3.1. Cập nhật giá trị values cho phần tử tâm cụm EC trong dang sách listEC (của nút cha) với nút con là CLuster 
            // 3.2. Tính lại và cập nhật vector trung bình CenterEC của cụm hiện hành

            //Sử dụng cơ chế STACK để khử đệ quy
            string dir = Path.GetDirectoryName(fileCluster) + "\\";
            string FILEChild = string.Empty;
            string FILECluster = string.Empty;
            Stack<string> STACKchild = new Stack<string>();
            Stack<string> STACKcluster = new Stack<string>();
            //Thêm tên cụm hiện hành vào STACK
            STACKchild.Push(fileChild);
            STACKcluster.Push(fileCluster);
            List<double> values = new List<double>();
            while (STACKchild.Count > 0 && STACKcluster.Count > 0)
            {
                FILEChild = STACKchild.Pop();
                FILECluster = STACKcluster.Pop();
                //Xử lý cập nhật lại values của phần tử EC
                string fileEC = dir + getFileEC(FILECluster);
                //Nếu là phần tử liên kết đến nút lá thì lấy trung bình của các phần tử dữ liệu ElementData
                if (getIsLeaf(FILEChild) == true)
                    values = getCenterED(FILEChild);
                //Nếu là phần tử không liên kết đến nút lá thì tính trung bình tâm của các phần tử tâm ElementCenter
                else
                    values = getCenterEC(FILEChild);

                ElementCenter EC = new ElementCenter();
                string fileNameChild = Path.GetFileName(FILEChild);
                EC.updateValuesEC(values, fileNameChild, fileEC);

                //Cập nhật CenterEC tại Cluster
                List<double> CenterECnew = EC.getCenterEC(fileEC);
                updateCenterEC(CenterECnew, FILECluster);

                //Cập nhật WeightEC tại Cluster
                List<double> Weightnew = EC.getWeightEC(fileEC);
                updateWeightEC(Weightnew, FILECluster);

                if (getIsRoot(FILECluster) == false)
                {
                    FILEChild = FILECluster;
                    FILECluster = dir + getFileParent(FILECluster);
                    STACKchild.Push(FILEChild);
                    STACKcluster.Push(FILECluster);
                }
            }
        }

        //Thêm một phân tử dữ liệu vào cụm nút lá
        public void AddEDtoLeafCLuster(ElementData ED, string fileCluster)
        {
            //Số lượng phần tử ED sẽ thay đổi, Vector trọng số bị thay đổi, Vetor tâm bị thay đổi
            //Cập nhật lại tâm (values) của phần tử tâm EC ở cụm nút cha tương ứng, cập nhật tâm của phần tử EC từ cụm hiện hành đến gốc
            
            string dir = Path.GetDirectoryName(fileCluster) + "\\";
            string filenameED = getFileED(fileCluster);
            string fileED = dir + filenameED;
            if (filenameED == "#NONE#") return; //Lúc này là nút lá của cây C-Tree, R-Tree, Kd-Tree

            //1.Thêm một phần tử vào danh sách tại nút lá của cây
            ED.SaveElementData(fileED);
            //2. Cập nhật số lượng phần tử dữ liệu Element Data của cụm nút lá
            TextfileCluster tfc = new TextfileCluster(fileED);
            string[] Lines = tfc.ReadAllLine();
            Lines = RemoveBlank(Lines);
            int numEDnew = Lines.Length;
            updateNumED(numEDnew, fileCluster);
            //3. Cập nhật vector trọng số, tức là vector trung bình của các lớp xuất hiện nhiều nhất
            List<double> WeightEDnew = ED.getWeight(fileED);
            updateWeightED(WeightEDnew, fileCluster);
            //4. Cập nhật vector tâm tại cụm hiện hành
            List<double> CenterEDnew = ED.getCenterED(fileED);
            updateCenterED(CenterEDnew, fileCluster);
            //Cập nhật fileLevel bên node trái (chưa sử dụng vì chưa có đồ thị)
            //int level = getLevel(fileCluster);
            //string levelfilename = "Level" + level.ToString() + ".txt";
            //if (!File.Exists(dir + levelfilename))
            //    File.Create(dir + levelfilename);
            //updateFileLevel(levelfilename, fileCluster);


            ////5. Nếu cụm hiện hành KHÔNG là nút gốc thì: Cập nhật vector tâm EC tại cụm nút cha của nút hiện hành cho tới nút gốc
            //if (getIsRoot(fileCluster) == false)
            //{
            //    string fileParentCluster = Path.GetDirectoryName(fileCluster) + "\\" + getFileParent(fileCluster);
            //    UpdateClusterInCTree(fileCluster, fileParentCluster);
            //}
        }

        //Xóa một phân tử dữ liệu ED của một cụm nút lá
        public void DelEDofLeafCLuster(ElementData ED, string fileCluster)
        {
            //Số lượng phần tử ED sẽ thay đổi, Vector trọng số bị thay đổi, Vetor tâm bị thay đổi
            //Cập nhật lại tâm (values) của phần tử tâm EC ở cụm nút cha tương ứng, cập nhật tâm của phần tử EC từ cụm hiện hành đến gốc

            string dir = Path.GetDirectoryName(fileCluster) + "\\";
            string filenameED = getFileED(fileCluster);
            string fileED = dir + filenameED;
            if (filenameED == "#NONE#") return; 

            //1.Xóa một phần tử vào danh sách tại nút lá của cây
            ED.DelElementData(ED.ImageID, ED.Feature, fileED);
            //2. Cập nhật số lượng phần tử dữ liệu Element Data của cụm nút lá
            TextfileCluster tfc = new TextfileCluster(fileED);
            string[] Lines = tfc.ReadAllLine();
            Lines = RemoveBlank(Lines);
            int numEDnew = Lines.Length;
            updateNumED(numEDnew, fileCluster);
            //3. Cập nhật vector trọng số, tức là vector trung bình của các lớp xuất hiện nhiều nhất
            List<double> WeightEDnew = ED.getWeight(fileED);
            updateWeightED(WeightEDnew, fileCluster);
            //4. Cập nhật vector tâm tại cụm hiện hành
            List<double> CenterEDnew = ED.getCenterED(fileED);
            updateCenterED(CenterEDnew, fileCluster);
            //5. Nếu cụm hiện hành KHÔNG là nút gốc thì: Cập nhật vector tâm EC tại cụm nút cha của nút hiện hành cho tới nút gốc
            //if (getIsRoot(fileCluster) == false)
            //{
            //    string fileParentCluster = Path.GetDirectoryName(fileCluster) + "\\" + getFileParent(fileCluster);
            //    UpdateClusterInCTree(fileCluster, fileParentCluster);
            //}
        }
        //Xóa một phân tử dữ liệu ED của một cụm nút lá
        public void DelEDofLeafCLuster(string ImageID, List<double> Feature, string fileCluster)
        {
            //Số lượng phần tử ED sẽ thay đổi, Vector trọng số bị thay đổi, Vetor tâm bị thay đổi
            //Cập nhật lại tâm (values) của phần tử tâm EC ở cụm nút cha tương ứng, cập nhật tâm của phần tử EC từ cụm hiện hành đến gốc

            string dir = Path.GetDirectoryName(fileCluster) + "\\";
            string filenameED = getFileED(fileCluster);
            string fileED = dir + filenameED;
            if (filenameED == "#NONE#") return; 

            //1.Xóa một phần tử vào danh sách tại nút lá của cây
            ElementData ED = new ElementData();
            ED.DelElementData(ImageID, Feature, fileED);
            //2. Cập nhật số lượng phần tử dữ liệu Element Data của cụm nút lá
            TextfileCluster tfc = new TextfileCluster(fileED);
            string[] Lines = tfc.ReadAllLine();
            Lines = RemoveBlank(Lines);
            int numEDnew = Lines.Length;
            updateNumED(numEDnew, fileCluster);
            //3. Cập nhật vector trọng số, tức là vector trung bình của các lớp xuất hiện nhiều nhất
            List<double> WeightEDnew = ED.getWeight(fileED);
            updateWeightED(WeightEDnew, fileCluster);
            //4. Cập nhật vector tâm tại cụm hiện hành
            List<double> CenterEDnew = ED.getCenterED(fileED);
            updateCenterED(CenterEDnew, fileCluster);
            //5. Nếu cụm hiện hành KHÔNG là nút gốc thì: Cập nhật vector tâm EC tại cụm nút cha của nút hiện hành cho tới nút gốc
            //if (getIsRoot(fileCluster) == false)
            //{
            //    string fileParentCluster = Path.GetDirectoryName(fileCluster) + "\\" + getFileParent(fileCluster);
            //    UpdateClusterInCTree(fileCluster, fileParentCluster);
            //}
        }

        //Xóa một phân tử dữ liệu ED của một cụm nút lá
        public void DelEDofLeafCLuster(string ImageID, string fileCluster)
        {
            //Số lượng phần tử ED sẽ thay đổi, Vector trọng số bị thay đổi, Vetor tâm bị thay đổi
            //Cập nhật lại tâm (values) của phần tử tâm EC ở cụm nút cha tương ứng, cập nhật tâm của phần tử EC từ cụm hiện hành đến gốc

            string dir = Path.GetDirectoryName(fileCluster) + "\\";
            string filenameED = getFileED(fileCluster);
            string fileED = dir + filenameED;
            if (filenameED == "#NONE#") return; 

            //1.Xóa một phần tử vào danh sách tại nút lá của cây
            ElementData ED = new ElementData();
            ED.DelElementData(ImageID, fileED);
            //2. Cập nhật số lượng phần tử dữ liệu Element Data của cụm nút lá
            TextfileCluster tfc = new TextfileCluster(fileED);
            string[] Lines = tfc.ReadAllLine();
            Lines = RemoveBlank(Lines);
            int numEDnew = Lines.Length;
            updateNumED(numEDnew, fileCluster);
            //3. Cập nhật vector trọng số, tức là vector trung bình của các lớp xuất hiện nhiều nhất
            List<double> WeightEDnew = ED.getWeight(fileED);
            updateWeightED(WeightEDnew, fileCluster);
            //4. Cập nhật vector tâm tại cụm hiện hành
            List<double> CenterEDnew = ED.getCenterED(fileED);
            updateCenterED(CenterEDnew, fileCluster);
            //5. Nếu cụm hiện hành KHÔNG là nút gốc thì: Cập nhật vector tâm EC tại cụm nút cha của nút hiện hành cho tới nút gốc
            //if (getIsRoot(fileCluster) == false)
            //{
            //    string fileParentCluster = Path.GetDirectoryName(fileCluster) + "\\" + getFileParent(fileCluster);
            //    UpdateClusterInCTree(fileCluster, fileParentCluster);
            //}
        }

        //Xóa một phân tử dữ liệu ED của một cụm nút lá
        public void DelEDofLeafCLuster(List<double> Feature, string fileCluster)
        {
            //Số lượng phần tử ED sẽ thay đổi, Vector trọng số bị thay đổi, Vetor tâm bị thay đổi
            //Cập nhật lại tâm (values) của phần tử tâm EC ở cụm nút cha tương ứng, cập nhật tâm của phần tử EC từ cụm hiện hành đến gốc

            string dir = Path.GetDirectoryName(fileCluster) + "\\";
            string filenameED = getFileED(fileCluster);
            string fileED = dir + filenameED;
            if (filenameED == "#NONE#") return; 

            //1.Xóa một phần tử vào danh sách tại nút lá của cây
            ElementData ED = new ElementData();
            ED.DelElementData(Feature, fileED);
            //2. Cập nhật số lượng phần tử dữ liệu Element Data của cụm nút lá
            TextfileCluster tfc = new TextfileCluster(fileED);
            string[] Lines = tfc.ReadAllLine();
            Lines = RemoveBlank(Lines);
            int numEDnew = Lines.Length;
            updateNumED(numEDnew, fileCluster);
            //3. Cập nhật vector trọng số, tức là vector trung bình của các lớp xuất hiện nhiều nhất
            List<double> WeightEDnew = ED.getWeight(fileED);
            updateWeightED(WeightEDnew, fileCluster);
            //4. Cập nhật vector tâm tại cụm hiện hành
            List<double> CenterEDnew = ED.getCenterED(fileED);
            updateCenterED(CenterEDnew, fileCluster);
            //5. Nếu cụm hiện hành KHÔNG là nút gốc thì: Cập nhật vector tâm EC tại cụm nút cha của nút hiện hành cho tới nút gốc
            //if (getIsRoot(fileCluster) == false)
            //{
            //    string fileParentCluster = Path.GetDirectoryName(fileCluster) + "\\" + getFileParent(fileCluster);
            //    UpdateClusterInCTree(fileCluster, fileParentCluster);
            //}
        }

        //Thêm một phân tử tâm Element Center vào cụm nút trong iNode
        public void AddECtoNodeCLuster(ElementCenter EC, string fileCluster)
        { 
            //Số lượng phần tử EC bị thay đổi, câp nhật lại trọng số WeightEC, cập nhật lại tâm CenterEC
           
            string dir = Path.GetDirectoryName(fileCluster) + "\\";
            string filenameEC = getFileEC(fileCluster);
            string fileEC = dir + filenameEC;
            if (filenameEC == "#NONE#") return; //Lúc này là nút lá của cây C-Tree, R-Tree, Kd-Tree

            EC.SaveElementCenter(fileEC);
            //Cập nhật số lượng phần tử Element Center
            TextfileCluster tfc = new TextfileCluster(fileEC);
            string[] Lines = tfc.ReadAllLine();
            Lines = RemoveBlank(Lines);
            int numECnew = Lines.Length;
            updateNumEC(numECnew, fileCluster);
            //Câp nhật phần tử tâm CenterEC tại cụm hiện hành
            List<double> CenterECnew = EC.getCenterEC(fileEC);
            updateCenterEC(CenterECnew, fileCluster);
            //Cập nhật trọng số WeightEC tại Cluster hiện hành
            List<double> Weightnew = EC.getWeightEC(fileEC);
            updateWeightEC(Weightnew, fileCluster);

            //Cập nhật fileLevel (chưa sử dụng vì chưa có đồ thị)
            //int level = getLevel(fileCluster);
            //string levelfilename = "Level" + level.ToString() + ".txt";
            //if (!File.Exists(dir + levelfilename))
            //    File.Create(dir + levelfilename);
            //updateFileLevel(levelfilename, fileCluster);

            ////Nếu cụm hiện hành KHÔNG là nút gốc thì: Cập nhật vector tâm EC tại cụm nút cha của nút hiện hành cho tới nút gốc
            //if (getIsRoot(fileCluster) == false)
            //{
            //    string fileParentCluster = Path.GetDirectoryName(fileCluster) + "\\" + getFileParent(fileCluster);
            //    UpdateClusterInCTree(fileCluster, fileParentCluster);
            //}
        }
        //Xóa một phân tử tâm Element Center tại một cụm nút iNode
        public void DelECofNodeCLuster(ElementCenter EC, string fileCluster)
        {
            //Nếu số phần tử EC của cụm = 0 thì không thực hiện
            int numEC = getNumEC(fileCluster);
            if (numEC == 0) return;
            //Lấy đường dẫn tập tin chứa danh sách các EC 
            string dir = Path.GetDirectoryName(fileCluster) + "\\";
            string filenameEC = getFileEC(fileCluster);
            string fileEC = dir + filenameEC;
            if (filenameEC == "#NONE#") return; //Lúc này là nút lá của cây C-Tree, R-Tree, Kd-Tree
            EC.DelElementCenter(EC.FileNameChild, EC.Values,fileEC);
            //Cập nhật số lượng phần tử Element Center
            TextfileCluster tfc = new TextfileCluster(fileEC);
            string[] Lines = tfc.ReadAllLine();
            Lines = RemoveBlank(Lines);
            int numECnew = Lines.Length;
            updateNumEC(numECnew, fileCluster);
            //Câp nhật phần tử tâm CenterEC tại cụm hiện hành
            List<double> CenterECnew = EC.getCenterEC(fileEC);
            updateCenterEC(CenterECnew, fileCluster);
            //Cập nhật trọng số WeightEC tại Cluster hiện hành
            List<double> Weightnew = EC.getWeightEC(fileEC);
            updateWeightEC(Weightnew, fileCluster);
            //Nếu cụm hiện hành KHÔNG là nút gốc thì: Cập nhật vector tâm EC tại cụm nút cha của nút hiện hành cho tới nút gốc
            //if (getIsRoot(fileCluster) == false)
            //{
            //    string fileParentCluster = Path.GetDirectoryName(fileCluster) + "\\" + getFileParent(fileCluster);
            //    UpdateClusterInCTree(fileCluster, fileParentCluster);
            //}
        }

        //Xóa một phân tử tâm Element Center tại một cụm nút iNode
        public void DelECofNodeCLuster(string FileNameChild, List<double> Values, string fileCluster)
        {
            //Nếu số phần tử EC của cụm = 0 thì không thực hiện
            int numEC = getNumEC(fileCluster);
            if (numEC == 0) return;
            //Lấy đường dẫn tập tin chứa danh sách các EC 
            string dir = Path.GetDirectoryName(fileCluster) + "\\";
            string filenameEC = getFileEC(fileCluster);
            string fileEC = dir + filenameEC;
            if (filenameEC == "#NONE#") return; //Lúc này là nút lá của cây C-Tree, R-Tree, Kd-Tree
            ElementCenter EC = new ElementCenter();
            EC.DelElementCenter(FileNameChild, Values, fileEC);
            //Cập nhật số lượng phần tử Element Center
            TextfileCluster tfc = new TextfileCluster(fileEC);
            string[] Lines = tfc.ReadAllLine();
            Lines = RemoveBlank(Lines);
            int numECnew = Lines.Length;
            updateNumEC(numECnew, fileCluster);
            //Câp nhật phần tử tâm CenterEC tại cụm hiện hành
            List<double> CenterECnew = EC.getCenterEC(fileEC);
            updateCenterEC(CenterECnew, fileCluster);
            //Cập nhật trọng số WeightEC tại Cluster hiện hành
            List<double> Weightnew = EC.getWeightEC(fileEC);
            updateWeightEC(Weightnew, fileCluster);
            //Nếu cụm hiện hành KHÔNG là nút gốc thì: Cập nhật vector tâm EC tại cụm nút cha của nút hiện hành cho tới nút gốc
            //if (getIsRoot(fileCluster) == false)
            //{
            //    string fileParentCluster = Path.GetDirectoryName(fileCluster) + "\\" + getFileParent(fileCluster);
            //    UpdateClusterInCTree(fileCluster, fileParentCluster);
            //}
        }

        //Xóa một phân tử tâm Element Center tại một cụm nút iNode
        public void DelECofNodeCLuster(string FileNameChild, string fileCluster)
        {
            //Nếu số phần tử EC của cụm = 0 thì không thực hiện
            int numEC = getNumEC(fileCluster);
            if (numEC == 0) return;
            //Lấy đường dẫn tập tin chứa danh sách các EC 
            string dir = Path.GetDirectoryName(fileCluster) + "\\";
            string filenameEC = getFileEC(fileCluster);
            string fileEC = dir + filenameEC;
            if (filenameEC == "#NONE#") return; //Lúc này là nút lá của cây C-Tree, R-Tree, Kd-Tree
            ElementCenter EC = new ElementCenter();
            EC.DelElementCenter(FileNameChild, fileEC);
            //Cập nhật số lượng phần tử Element Center
            TextfileCluster tfc = new TextfileCluster(fileEC);
            string[] Lines = tfc.ReadAllLine();
            Lines = RemoveBlank(Lines);
            int numECnew = Lines.Length;
            updateNumEC(numECnew, fileCluster);
            //Câp nhật phần tử tâm CenterEC tại cụm hiện hành
            List<double> CenterECnew = EC.getCenterEC(fileEC);
            updateCenterEC(CenterECnew, fileCluster);
            //Cập nhật trọng số WeightEC tại Cluster hiện hành
            List<double> Weightnew = EC.getWeightEC(fileEC);
            updateWeightEC(Weightnew, fileCluster);
            //Nếu cụm hiện hành KHÔNG là nút gốc thì: Cập nhật vector tâm EC tại cụm nút cha của nút hiện hành cho tới nút gốc
            //if (getIsRoot(fileCluster) == false)
            //{
            //    string fileParentCluster = Path.GetDirectoryName(fileCluster) + "\\" + getFileParent(fileCluster);
            //    UpdateClusterInCTree(fileCluster, fileParentCluster);
            //}
        }
        //Xóa một phân tử tâm Element Center tại một cụm nút iNode
        public void DelECofNodeCLuster(List<double> Values, string fileCluster)
        {
            //Nếu số phần tử EC của cụm = 0 thì không thực hiện
            int numEC = getNumEC(fileCluster);
            if (numEC == 0) return;
            //Lấy đường dẫn tập tin chứa danh sách các EC 
            string dir = Path.GetDirectoryName(fileCluster) + "\\";
            string filenameEC = getFileEC(fileCluster);
            string fileEC = dir + filenameEC;
            if (filenameEC == "#NONE#") return; //Lúc này là nút lá của cây C-Tree, R-Tree, Kd-Tree
            ElementCenter EC = new ElementCenter();
            EC.DelElementCenter(Values, fileEC);
            //Cập nhật số lượng phần tử Element Center
            TextfileCluster tfc = new TextfileCluster(fileEC);
            string[] Lines = tfc.ReadAllLine();
            Lines = RemoveBlank(Lines);
            int numECnew = Lines.Length;
            updateNumEC(numECnew, fileCluster);
            //Câp nhật phần tử tâm CenterEC tại cụm hiện hành
            List<double> CenterECnew = EC.getCenterEC(fileEC);
            updateCenterEC(CenterECnew, fileCluster);
            //Cập nhật trọng số WeightEC tại Cluster hiện hành
            List<double> Weightnew = EC.getWeightEC(fileEC);
            updateWeightEC(Weightnew, fileCluster);
            //Nếu cụm hiện hành KHÔNG là nút gốc thì: Cập nhật vector tâm EC tại cụm nút cha của nút hiện hành cho tới nút gốc
            //if (getIsRoot(fileCluster) == false)
            //{
            //    string fileParentCluster = Path.GetDirectoryName(fileCluster) + "\\" + getFileParent(fileCluster);
            //    UpdateClusterInCTree(fileCluster, fileParentCluster);
            //}
        }
        public void AddEDfromRootByCenter(ElementData ED, string Root)
        { 
            //Tìm hướng đi tốt nhất theo vector tâm
            List<double> vector = ED.Feature;
            string file = Root;
            string fileEC;
            string dir = Path.GetDirectoryName(Root) + "\\";
            ElementCenter EC = new ElementCenter();
            Stack<string> STACK = new Stack<string>();
            STACK.Push(Root);
            while (STACK.Count > 0)
            {
                file = STACK.Pop();
                if (getIsLeaf(file)==false)
                {
                    fileEC = dir + getFileEC(file);
                    EC = EC.getMinEC(vector, fileEC);
                    //vector = EC.Values;
                    STACK.Push(dir + EC.FileNameChild);
                }
            }
            AddEDtoLeafCLuster(ED, file);
            if (getIsRoot(file) == false)
                UpdateClusterInCTree(file, dir + getFileParent(file));
            //Nếu nút lá có 2 lớp trở lên thì gọi hàm SplitLeafCluster2, ngược lại nếu chỉ có một lớp thì gọi hàm SplitLeaftCluster
            if (getNumED(file) > M)
            {
                SplitLeafCLuster(theta, file);
            }
        }

        public void AddEDfromRootByCenter2(ElementData ED, string Root)
        {
            //Tìm hướng đi tốt nhất theo vector tâm
            List<double> vector = ED.Feature;
            string file = Root;
            string fileEC;
            string dir = Path.GetDirectoryName(Root) + "\\";
            ElementCenter EC = new ElementCenter();
            Stack<string> STACK = new Stack<string>();
            STACK.Push(Root);
            while (STACK.Count > 0)
            {
                file = STACK.Pop();
                if (getIsLeaf(file) == false)
                {
                    fileEC = dir + getFileEC(file);
                    EC = EC.getMinEC(vector, fileEC);
                    //vector = EC.Values;
                    STACK.Push(dir + EC.FileNameChild);
                }
            }
            AddEDtoLeafCLuster(ED, file);
            if (getIsRoot(file) == false)
                UpdateClusterInCTree(file, dir + getFileParent(file));
            //Nếu nút lá có 2 lớp trở lên thì gọi hàm SplitLeafCluster2, ngược lại nếu chỉ có một lớp thì gọi hàm SplitLeaftCluster
            if (getNumED(file) > M)
            {
                ClusterNode cluster = new ClusterNode();
                string fileED = dir + cluster.getFileED(file);
                if (ED.getListClassName(fileED).Count >= 2)
                    SplitLeafCLuster2(theta, file);
                else
                    SplitLeafCLuster(theta, file);
            }
        }

        //Khi thêm/xóa/cập nhật một Element Center thì phải thêm/xóa/cập nhật danh sách các file cụm con (Mỗi cụm con được nằm trong một Element Center)
        //Điều này sẽ giảm tải (tăng tốc độ) cho quá trình tìm kiếm vì không cần phải trích lục các cụm con trong từng Element Center

        //Thuật toán tách cụm lớn thành hai cụm nhỏ theo tiêu chí độ đo Euclide
        //1. Chọn 2 phần tử Element Data xa nhất
        //2. Nếu là một cụm nút gốc thì tạo một nút gốc mới chứa hai phần tử
        //3. Nếu là một iNode hoăc leafNode thì bổ sung vào nút cha hai phần tử đại diện
        //3. Lần lượt phân bố các phần tử Element Center và Element Data vào hai cụm đã tách
        //4. Cập nhật phần tử đại diện cho cụm nút cha và cập nhật các trong số cho cụm nút cha (thực hiện đệ quy đến nút gốc)
        public int getInt(string strNum)
        {
            string sNum = string.Empty;
            int val = 0;
            int len = strNum.Length;
            for (int i = 0; i < len; i++)
            {
                if (Char.IsDigit(strNum[i]))
                    sNum += strNum[i];
            }

            if (sNum.Length > 0)
                val = ToInt32(sNum);
            return val;
        }
        public string getNameLeafNode(string dir, string LeafNode)
        {
            string[] Names = Directory.GetFiles(dir, LeafNode + "*" + ".txt");
             string nameMax = string.Empty;
            if (Names.Length > 0)
            {
                nameMax = Path.GetFileName(Names[0]);
                int len = Names.Length;
                for (int i = 0; i < len; i++)
                {
                    string filename = Path.GetFileName(Names[i]);
                    if (getInt(nameMax) < getInt(filename))
                        nameMax = filename;
                }
            }
            string nameLeafNode = LeafNode + (getInt(nameMax) + 1).ToString() + ".txt";
            return nameLeafNode;
        }
        //lấy mức của một nút
        public int getLevel(string NodeLeaf)
        {
            int h = 0;
            Stack<string> STACK = new Stack<string>();
            if (getIsRoot(NodeLeaf) == true && getIsLeaf(NodeLeaf) == true) return h;
            string dir = Path.GetDirectoryName(NodeLeaf) + "\\";
            STACK.Push(NodeLeaf);
            while (STACK.Count > 0)
            { 
                string Node = STACK.Pop();
                if (getIsRoot(Node) == false)
                {
                    h++;
                    Node = dir + getFileParent(Node);
                    STACK.Push(Node);
                }
            }
            return h;
        }
        public bool isLeafAlias(string filename, string[] Lines)
        {
            Lines = RemoveBlank(Lines);
            if (Lines == null) return true;
            int len = Lines.Length;
            for (int i = 0; i < len; i++)
            {
                string[] words = Lines[i].Split(' ');
                if (filename.Split() == words[0].Split())
                    return false;
            }
            return true;
        }

        public int FindAlias(string filename, string[] Lines)
        {
            Lines = RemoveBlank(Lines);
            if (Lines == null) return -1;
            int len = Lines.Length;
            for (int i = 0; i < len; i++)
            {
                 string[] words = Lines[i].Split(' ');
                 words = RemoveBlank(words);
                 if (filename.Trim() == words[0].Trim())
                     return i;
            }
            return -1;
        }

        public List<string> getAliasLeftLeaf(string Leaf, string RenameLeafFile)
        {
            TextfileCluster tfc = new TextfileCluster(RenameLeafFile);
            string[] FileNames = tfc.ReadAllLine();
            int len = FileNames.Length;
            if (len == 0) return null;
            List<string> ListFileName = new List<string>();
            Stack<string> StackFile = new Stack<string>();
            StackFile.Push(Leaf);
            int idx = 0;
            
            string filename = string.Empty;
            while (StackFile.Count > 0)
            {
                filename = StackFile.Pop();
                idx = FindAlias(filename, FileNames);
                if (idx == -1) //không 
                    ListFileName.Add(filename);
                else
                {
                    string[] words = FileNames[idx].Split(' ');
                    words = RemoveBlank(words);
                    StackFile.Push(words[1]);
                    StackFile.Push(words[2]);
                }
            }
            return ListFileName;
        }

        public List<string> AddList(List<string> List1, List<string> List2)
        {
            if (List1 == null) return List2;
            if (List2 == null) return List1;
            if (List1.Count == 0) return List2;
            if (List2.Count == 0) return List1;
            foreach (string name in List2)
                if (List1.Contains(name) == false)
                    List1.Add(name);
            return List1;
        }

        public List<string> getNeightborOneLeaf(string Leaf, string RenameLeafFile)
        {
            List<string> filenameList = new List<string>();
            List<string> filenameNeighbor = new List<string>();
            string dir = Path.GetDirectoryName(Leaf) + "\\";
            filenameNeighbor = getListFileCluster(Leaf);
            TextfileCluster tfc = new TextfileCluster(dir + filenameNeighbor.ElementAt(0));
            string[] LeafNeighbor = tfc.ReadAllLine();
            if (LeafNeighbor == null) return null;
            foreach (string filename in LeafNeighbor)
            {
                List<string> ListAlias = getAliasLeftLeaf(filename, RenameLeafFile);
                filenameList = AddList(filenameList, ListAlias);
            }

            return filenameList;
        }

        public List<string> getNeightborTwoLeaf(string Leaf, string RenameLeafFile)
        {
            List<string> filenameList = new List<string>();
            List<string> filenameNeighbor = new List<string>();
            string dir = Path.GetDirectoryName(Leaf) + "\\";
            filenameNeighbor = getListFileCluster(Leaf);
            TextfileCluster tfc = new TextfileCluster(dir + filenameNeighbor.ElementAt(1));
            string[] LeafNeighbor = tfc.ReadAllLine();
            if (LeafNeighbor == null) return null;
            foreach (string filename in LeafNeighbor)
            {
                List<string> ListAlias = getAliasLeftLeaf(filename, RenameLeafFile);
                filenameList = AddList(filenameList, ListAlias);
            }

            return filenameList;
        }

        //Sắp xếp các nút tăng dần theo khoảng cách từ tâm đến phần tử feature
        public List<string> SortFilenameLeafNode(List<double> feature, List<string> FilenameLeafNode, string dir)
        {
            ClusterNode Cluster = new ClusterNode();
            string[] Arr = FilenameLeafNode.ToArray();
            for (int i = 0; i < FilenameLeafNode.Count - 1; i++)
            {
                List<double> center1 = Cluster.getCenterED(dir + FilenameLeafNode.ElementAt(i));
                for (int j = i + 1; j < FilenameLeafNode.Count; j++)
                {
                    List<double> center2 = Cluster.getCenterED(dir + FilenameLeafNode.ElementAt(j));
                    if (EuclideDistance(feature, center1) > EuclideDistance(feature, center2))
                    {
                        string tmp = Arr[i];
                        Arr[i] = Arr[j];
                        Arr[j] = tmp;
                    }
                }
            }
            return Arr.ToList();
        }

        //Sắp xếp các nút tăng dần theo khoảng cách từ tâm đến phần tử feature
        public List<string> SortFilenameLeafNodeKeys(List<double> feature, List<string> FilenameLeafNode, string dir)
        {
            ClusterNode Cluster = new ClusterNode();
            string[] Arr = FilenameLeafNode.ToArray();
            int n = Arr.Length;
            double[] keys = new double[n];
            for (int i = 0; i < n; i++)
            {
                List<double> center = Cluster.getCenterED(dir + FilenameLeafNode.ElementAt(i));
                keys[i] = EuclideDistance(feature, center);
            }
            Array.Sort(keys, Arr);
            return Arr.ToList();
        }

        //Áp dụng cho trường hợp nút lá có từ 2 phân lớp trở lên
        public void SplitLeafCLuster2(double theta, string fileLeaf)
        {
            //Chọn hai tâm của hai nhóm phần tử xuất hiện nhiều nhất
            string dir = Path.GetDirectoryName(fileLeaf) + "\\";
            string fileED = dir + getFileED(fileLeaf);
            ElementData ED = new ElementData();
            List<double> CenterFirst = ED.getWeight(fileED);
            List<double> CenterSecond = ED.getWeightSecond(fileED);
            //if (CenterFirst == CenterSecond) Console.Write("Erorr Center");
            //Lấy hai tên lớp xuất hiện nhiều nhất
            string ClassNameFirst = ED.getClassMax(fileED);
            string ClassNameSecond = ED.getClassMaxSecond(fileED);
            //if (ClassNameFirst == ClassNameSecond) Console.Write("Erorr Classname");

            ElementData EDleft = new ElementData();
            ElementData EDright = new ElementData();
            EDleft = EDleft.getEDwithClassname(fileED, ClassNameFirst);
            EDright = EDright.getEDwithClassname(fileED, ClassNameSecond);

            //Xóa hai phần tử dữ liệu EDleft, EDright trong tập tin fileED
            EDleft.DelElementData(EDleft.ImageID, EDleft.Feature, fileED);
            EDright.DelElementData(EDright.ImageID, EDright.Feature, fileED);
            //Lấy danh sách các phần tử Element Data còn lại
            List<ElementData> ListElementData = EDleft.getListElementData(fileED);

            //Tạo hai nút lá mới để phân bố các phần tử, nếu nút lá hiện hành là nút gốc thì tạo nút gốc mới
            ClusterNode LeafLeft = new ClusterNode(true, false);
            ClusterNode LeafRight = new ClusterNode(true, false);

            //Tạo ra tên file cho nút lá mới
            string fileLeafLeft = dir + getNameLeafNode(dir, "Leaf");
            LeafLeft.SaveCluster(fileLeafLeft);
            string fileLeafRight = dir + getNameLeafNode(dir, "Leaf");
            LeafRight.SaveCluster(fileLeafRight);

            //Lưu vết sự thay đổi tên tập tin
            TextfileCluster tfc = new TextfileCluster();
            string strRenameLeaf = Path.GetFileName(fileLeaf) + "  " + Path.GetFileName(fileLeafLeft) + "  " + Path.GetFileName(fileLeafRight);
            string RenameLeafFile = dir + "RenameLeaf.txt";
            tfc.WriteLineTextFile(strRenameLeaf, RenameLeafFile);

            //Cập nhật tên tập tin filenameParent
            if (getIsRoot(fileLeaf) == true)
            {
                updateFileParent("Root.txt", fileLeafLeft);
                updateFileParent("Root.txt", fileLeafRight);
            }
            else
            {
                updateFileParent(getFileParent(fileLeaf), fileLeafLeft);
                updateFileParent(getFileParent(fileLeaf), fileLeafRight);
            }

            //Phân bố các phần tử vào hai cụm;
            LeafLeft.AddEDtoLeafCLuster(EDleft, fileLeafLeft);
            LeafRight.AddEDtoLeafCLuster(EDright, fileLeafRight);
            if (ListElementData.Count > 0)
            {
                foreach (ElementData ed in ListElementData)
                {
                    double disLeft = EuclideDistance(ed.Feature, CenterFirst);
                    double disRight = EuclideDistance(ed.Feature, CenterSecond);
                    if (ed.ListClass.Contains(ClassNameFirst))
                        disLeft = 0.0;
                    if (ed.ListClass.Contains(ClassNameSecond))
                        disRight = 0.0;
                    if (disLeft < disRight)
                        LeafLeft.AddEDtoLeafCLuster(ed, fileLeafLeft);
                    else
                        LeafRight.AddEDtoLeafCLuster(ed, fileLeafRight);
                }
            }

            //Cập nhật danh sách tên tập tin chứa các Cluster con.
            updateListFileChild(null, fileLeafLeft);
            updateListFileChild(null, fileLeafRight);

            //Cập nhật fileLevel bên node trái (chưa sử dụng vì chưa có đồ thị)
            //int level = getLevel(fileLeafLeft);
            //string levelfilename = "Level" + level.ToString() + ".txt";
            //if (!File.Exists(dir + levelfilename))
            //    File.Create(dir + levelfilename);
            //updateFileLevel(levelfilename, fileLeafLeft);
            ////Cập nhật fileLevel bên node trái (chưa sử dụng vì chưa có đồ thị)
            //updateFileLevel(levelfilename, fileLeafRight);

            //Xóa hai phần tử dữ liệu EDleft, EDright trong tập tin fileED
            //EDleft.DelElementData(EDleft.ImageID, EDleft.Feature, fileED);
            //EDright.DelElementData(EDright.ImageID, EDright.Feature, fileED);

            //XÓA TẬP TIN fileED VÌ ĐÃ TÁCH NÚT (string fileED = dir + getFileED(fileLeaf);)

            //Tạo hai thành phần ECLeft và Right để kết nối vào nút cha
            ElementCenter ECLeft = new ElementCenter(true);
            ElementCenter ECRight = new ElementCenter(true);
            ECLeft.Index = 0;
            ECLeft.Values = getCenterED(fileLeafLeft);
            ECLeft.IsNextLeaf = true;
            ECLeft.Filename = Path.GetFileName(getFileParent(fileLeafLeft));
            ECLeft.FileNameChild = Path.GetFileName(fileLeafLeft);

            ECRight.Index = 0;
            ECRight.Values = getCenterED(fileLeafRight);
            ECRight.IsNextLeaf = true;
            ECRight.Filename = Path.GetFileName(getFileParent(fileLeafRight));
            ECRight.FileNameChild = Path.GetFileName(fileLeafRight);

            //Nếu nút hiện tại là nút gốc thì tạo ra một nút gốc mới
            string fileParent = dir + getFileParent(fileLeaf);
            if (getIsRoot(fileLeaf) == true)
            {
                ClusterNode Root = new ClusterNode(false, true);
                string fileRoot = dir + "Root.txt";
                Root.SaveCluster(fileRoot);
                AddECtoNodeCLuster(ECLeft, fileRoot);
                AddECtoNodeCLuster(ECRight, fileRoot);
                //XÓA TẬP TIN fileLeaf VÌ ĐÃ TÁCH NÚT
            }
            //Ngược lại thêm vào nút cha ParentNode
            else
            {
                //string fileParent = dir + getFileParent(fileLeaf);
                AddECtoNodeCLuster(ECLeft, fileParent);
                AddECtoNodeCLuster(ECRight, fileParent);
                //Xóa bỏ thành phần EC của nút đã bị tách tại nút cha
                DelECofNodeCLuster(Path.GetFileName(fileLeaf), fileParent);
            }

            //Cập nhật thông tin cho hai nút lá mới
            updateIsRoot(false, fileLeafLeft);
            updateIsRoot(false, fileLeafRight);
            //Tạo tên tập tin chứa các fileCluster láng giềng
            List<string> listNeighborLeft = new List<string>();
            listNeighborLeft.Add("Neighbor" + "1" + Path.GetFileName(fileLeafLeft));
            listNeighborLeft.Add("Neighbor" + "2" + Path.GetFileName(fileLeafLeft));
            listNeighborLeft.Add("Neighbor" + "3" + Path.GetFileName(fileLeafLeft));
            listNeighborLeft.Add("Neighbor" + "4" + Path.GetFileName(fileLeafLeft));
            updateListFileCluster(listNeighborLeft, fileLeafLeft);
            List<string> listNeighborRight = new List<string>();
            listNeighborRight.Add("Neighbor" + "1" + Path.GetFileName(fileLeafRight));
            listNeighborRight.Add("Neighbor" + "2" + Path.GetFileName(fileLeafRight));
            listNeighborRight.Add("Neighbor" + "3" + Path.GetFileName(fileLeafRight));
            listNeighborRight.Add("Neighbor" + "4" + Path.GetFileName(fileLeafRight));
            updateListFileCluster(listNeighborRight, fileLeafRight);
            //Cập nhật danh sách file láng giềng
            if (getIsRoot(fileLeaf) == false)
            {
                List<string> listNeighbor = getListFileCluster(fileLeaf);
                string[] Lines = tfc.ReadAllLine(dir + listNeighbor.ElementAt(0));
                Lines = RemoveBlank(Lines);
                if (Lines != null)
                { tfc.WriteNewTextFile(Lines, dir + listNeighborLeft.ElementAt(0)); tfc.WriteNewTextFile(Lines, dir + listNeighborRight.ElementAt(0)); }
                Lines = tfc.ReadAllLine(dir + listNeighbor.ElementAt(1));
                Lines = RemoveBlank(Lines);
                if (Lines != null)
                { tfc.WriteNewTextFile(Lines, dir + listNeighborLeft.ElementAt(1)); tfc.WriteNewTextFile(Lines, dir + listNeighborRight.ElementAt(1)); }
                Lines = tfc.ReadAllLine(dir + listNeighbor.ElementAt(2));
                Lines = RemoveBlank(Lines);
                if (Lines != null)
                { tfc.WriteNewTextFile(Lines, dir + listNeighborLeft.ElementAt(2)); tfc.WriteNewTextFile(Lines, dir + listNeighborRight.ElementAt(2)); }
                Lines = tfc.ReadAllLine(dir + listNeighbor.ElementAt(3));
                Lines = RemoveBlank(Lines);
                if (Lines != null)
                { tfc.WriteNewTextFile(Lines, dir + listNeighborLeft.ElementAt(3)); tfc.WriteNewTextFile(Lines, dir + listNeighborRight.ElementAt(3)); }
            }
            //Láng giềng cấp 1: hai tâm cụm gần nhau (nhỏ hơn giá trị ngưỡng theta)
            List<double> centerLeft = getCenterED(fileLeafLeft);
            List<double> centerRight = getCenterED(fileLeafRight);
            if (EuclideDistance(centerLeft, centerRight) < theta)
            {
                tfc.WriteLineTextFile(Path.GetFileName(fileLeafLeft), dir + listNeighborRight.ElementAt(0));
                tfc.WriteLineTextFile(Path.GetFileName(fileLeafRight), dir + listNeighborLeft.ElementAt(0));
            }
            //Láng giềng cấp 2: tên lớp đại diện của cụm giống nhau
            string nameclassLeft = ED.getClassMax(dir + getFileED(fileLeafLeft));
            string nameclassRight = ED.getClassMax(dir + getFileED(fileLeafRight));
            if (nameclassLeft == nameclassRight)
            {
                tfc.WriteLineTextFile(Path.GetFileName(fileLeafLeft), dir + listNeighborRight.ElementAt(1));
                tfc.WriteLineTextFile(Path.GetFileName(fileLeafRight), dir + listNeighborLeft.ElementAt(1));
            }
            //Láng giềng cấp 3: có quan hệ cha con IS-A
            if (IsAClass(nameclassLeft, nameclassRight, FileClass) || IsAClass(nameclassRight, nameclassLeft, FileClass))
            {
                tfc.WriteLineTextFile(Path.GetFileName(fileLeafLeft), dir + listNeighborRight.ElementAt(2));
                tfc.WriteLineTextFile(Path.GetFileName(fileLeafRight), dir + listNeighborLeft.ElementAt(2));
            }
            //Láng giềng cấp 4: khoảng cách trọng số đại diện nhỏ hơn ngưỡng theta
            List<double> WeightLeft = getWeightED(fileLeafLeft);
            List<double> WeightRight = getWeightED(fileLeafRight);
            if (EuclideDistance(WeightLeft, WeightRight) < theta)
            {
                tfc.WriteLineTextFile(Path.GetFileName(fileLeafLeft), dir + listNeighborRight.ElementAt(3));
                tfc.WriteLineTextFile(Path.GetFileName(fileLeafRight), dir + listNeighborLeft.ElementAt(3));
            }

            //Cập nhật từ nút lá tới gốc
            UpdateClusterInCTree(fileLeafLeft, dir + getFileParent(fileLeafLeft));
            UpdateClusterInCTree(fileLeafRight, dir + getFileParent(fileLeafRight));

            //Nếu như nút cha của nút lá bị đầy thì tiếp tục tách nút trong
            if (getIsRoot(fileLeaf) == false)
                if (getNumEC(fileParent) > N)
                    SplitNodeCLuster(theta, fileParent);

            //XÓA TẬP TIN fileLeaf VÌ ĐÃ TÁCH NÚT
        }

        public void SplitLeafCLuster(double theta, string fileLeaf)
        {
            //1. Chọn hai phần tử xa tâm nhất theo độ đo Euclide (lưu ý điều kiện số phần tử ED phải lớn hơn 2, tức là M > 2)
            ElementData EDleft = new ElementData();
            ElementData EDright = new ElementData();
            string dir = Path.GetDirectoryName(fileLeaf) + "\\";
            string fileED = dir + getFileED(fileLeaf);
            EDleft = EDleft.getFarestEDcenter(fileED);
            EDright = EDright.getFarestED(EDleft, fileED);
            //Xóa hai phần tử dữ liệu EDleft, EDright trong tập tin fileED
            EDleft.DelElementData(EDleft.ImageID, EDleft.Feature, fileED);
            EDright.DelElementData(EDright.ImageID, EDright.Feature, fileED);
            //Lấy danh sách các phần tử Element Data còn lại
            List<ElementData> ListElementData = EDleft.getListElementData(fileED);

            //Tạo hai nút lá mới để phân bố các phần tử, nếu nút lá hiện hành là nút gốc thì tạo nút gốc mới
            ClusterNode LeafLeft = new ClusterNode(true, false);
            ClusterNode LeafRight = new ClusterNode(true, false);
            //Tạo ra tên file cho nút lá mới
            string fileLeafLeft = dir + getNameLeafNode(dir, "Leaf");
            LeafLeft.SaveCluster(fileLeafLeft);
            string fileLeafRight = dir + getNameLeafNode(dir, "Leaf");
            LeafRight.SaveCluster(fileLeafRight);

            //Lưu vết sư thay đổi tên tập tin
            TextfileCluster tfc = new TextfileCluster();
            string strRenameLeaf = Path.GetFileName(fileLeaf) + "  " + Path.GetFileName(fileLeafLeft) + "  " + Path.GetFileName(fileLeafRight);
            string RenameLeafFile = dir + "RenameLeaf.txt";
            tfc.WriteLineTextFile(strRenameLeaf, RenameLeafFile);

            //Cập nhật tên tập tin filenameParent
            if (getIsRoot(fileLeaf) == true)
            {
                updateFileParent("Root.txt", fileLeafLeft);
                updateFileParent("Root.txt", fileLeafRight);
            }
            else
            {
                updateFileParent(getFileParent(fileLeaf), fileLeafLeft);
                updateFileParent(getFileParent(fileLeaf), fileLeafRight);
            }

            //Phân bố các phần tử còn lại vào hai cụm;
            LeafLeft.AddEDtoLeafCLuster(EDleft, fileLeafLeft);
            LeafRight.AddEDtoLeafCLuster(EDright, fileLeafRight);
            if (ListElementData.Count > 0)
            {
                foreach (ElementData ed in ListElementData)
                    if (EuclideDistance(ed.Feature, EDleft.Feature) < EuclideDistance(ed.Feature, EDright.Feature))
                        LeafLeft.AddEDtoLeafCLuster(ed, fileLeafLeft);
                    else
                        LeafRight.AddEDtoLeafCLuster(ed, fileLeafRight);
            }

            //Cập nhật danh sách tên tập tin chứa các Cluster con.
            updateListFileChild(null, fileLeafLeft);
            updateListFileChild(null, fileLeafRight);
            //Cập nhật fileLevel bên node trái (chưa sử dụng vì chưa có đồ thị)
            //int level = getLevel(fileLeafLeft);
            //string levelfilename = "Level" + level.ToString() + ".txt";
            //if (!File.Exists(dir + levelfilename))
            //    File.Create(dir + levelfilename);
            //updateFileLevel(levelfilename, fileLeafLeft);
            ////Cập nhật fileLevel bên node trái (chưa sử dụng vì chưa có đồ thị)
            //updateFileLevel(levelfilename, fileLeafRight);

            //Xóa hai phần tử dữ liệu EDleft, EDright trong tập tin fileED
            //EDleft.DelElementData(EDleft.ImageID, EDleft.Feature, fileED);
            //EDright.DelElementData(EDright.ImageID, EDright.Feature, fileED);

            //XÓA TẬP TIN fileED VÌ ĐÃ TÁCH NÚT (string fileED = dir + getFileED(fileLeaf);)

            //Tạo hai thành phần ECLeft và Right để kết nối vào nút cha
            ElementCenter ECLeft = new ElementCenter(true);
            ElementCenter ECRight = new ElementCenter(true);
            ECLeft.Index = 0;
            ECLeft.Values = getCenterED(fileLeafLeft);
            ECLeft.IsNextLeaf = true;
            ECLeft.Filename = Path.GetFileName(getFileParent(fileLeafLeft));
            ECLeft.FileNameChild = Path.GetFileName(fileLeafLeft);

            ECRight.Index = 0;
            ECRight.Values = getCenterED(fileLeafRight);
            ECRight.IsNextLeaf = true;
            ECRight.Filename = Path.GetFileName(getFileParent(fileLeafRight));
            ECRight.FileNameChild = Path.GetFileName(fileLeafRight);

            //Nếu nút hiện tại là nút gốc thì tạo ra một nút gốc mới
            string fileParent = dir + getFileParent(fileLeaf);
            if (getIsRoot(fileLeaf) == true)
            {
                ClusterNode Root = new ClusterNode(false, true);
                string fileRoot = dir + "Root.txt";
                Root.SaveCluster(fileRoot);
                AddECtoNodeCLuster(ECLeft, fileRoot);
                AddECtoNodeCLuster(ECRight, fileRoot);
                //XÓA TẬP TIN fileLeaf VÌ ĐÃ TÁCH NÚT
            }
            //Ngược lại thêm vào nút cha ParentNode
            else
            {
                //string fileParent = dir + getFileParent(fileLeaf);
                AddECtoNodeCLuster(ECLeft, fileParent);
                AddECtoNodeCLuster(ECRight, fileParent);
                //Xóa bỏ thành phần EC của nút đã bị tách tại nút cha
                DelECofNodeCLuster(Path.GetFileName(fileLeaf), fileParent);
            }

            //Cập nhật thông tin cho hai nút lá mới
            updateIsRoot(false, fileLeafLeft);
            updateIsRoot(false, fileLeafRight);
            //Tạo tên tập tin chứa các fileCluster láng giềng
            List<string> listNeighborLeft = new List<string>();
            listNeighborLeft.Add("Neighbor" + "1" + Path.GetFileName(fileLeafLeft));
            listNeighborLeft.Add("Neighbor" + "2" + Path.GetFileName(fileLeafLeft));
            listNeighborLeft.Add("Neighbor" + "3" + Path.GetFileName(fileLeafLeft));
            listNeighborLeft.Add("Neighbor" + "4" + Path.GetFileName(fileLeafLeft));
            updateListFileCluster(listNeighborLeft, fileLeafLeft);
            List<string> listNeighborRight = new List<string>();
            listNeighborRight.Add("Neighbor" + "1" + Path.GetFileName(fileLeafRight));
            listNeighborRight.Add("Neighbor" + "2" + Path.GetFileName(fileLeafRight));
            listNeighborRight.Add("Neighbor" + "3" + Path.GetFileName(fileLeafRight));
            listNeighborRight.Add("Neighbor" + "4" + Path.GetFileName(fileLeafRight));
            updateListFileCluster(listNeighborRight, fileLeafRight);
            //Cập nhật danh sách file láng giềng
            if (getIsRoot(fileLeaf) == false)
            {
                List<string> listNeighbor = getListFileCluster(fileLeaf);
                string[] Lines = tfc.ReadAllLine(dir + listNeighbor.ElementAt(0));
                Lines = RemoveBlank(Lines);
                //cập nhật các tên file trong Lines (truy vết tên cũ đã xóa để thay thế bằng tên mới)
                if (Lines != null)
                { tfc.WriteNewTextFile(Lines, dir + listNeighborLeft.ElementAt(0)); tfc.WriteNewTextFile(Lines, dir + listNeighborRight.ElementAt(0)); }
                Lines = tfc.ReadAllLine(dir + listNeighbor.ElementAt(1));
                Lines = RemoveBlank(Lines);
                if (Lines != null)
                { tfc.WriteNewTextFile(Lines, dir + listNeighborLeft.ElementAt(1)); tfc.WriteNewTextFile(Lines, dir + listNeighborRight.ElementAt(1)); }
                Lines = tfc.ReadAllLine(dir + listNeighbor.ElementAt(2));
                Lines = RemoveBlank(Lines);
                if (Lines != null)
                { tfc.WriteNewTextFile(Lines, dir + listNeighborLeft.ElementAt(2)); tfc.WriteNewTextFile(Lines, dir + listNeighborRight.ElementAt(2)); }
                Lines = tfc.ReadAllLine(dir + listNeighbor.ElementAt(3));
                Lines = RemoveBlank(Lines);
                if (Lines != null)
                { tfc.WriteNewTextFile(Lines, dir + listNeighborLeft.ElementAt(3)); tfc.WriteNewTextFile(Lines, dir + listNeighborRight.ElementAt(3)); }
            }
            //Láng giềng cấp 1: hai tâm cụm gần nhau (nhỏ hơn giá trị ngưỡng theta)
            List<double> centerLeft = getCenterED(fileLeafLeft);
            List<double> centerRight = getCenterED(fileLeafRight);
            if (EuclideDistance(centerLeft, centerRight) < 2*theta)
            {
                tfc.WriteLineTextFile(Path.GetFileName(fileLeafLeft), dir + listNeighborRight.ElementAt(0));
                tfc.WriteLineTextFile(Path.GetFileName(fileLeafRight), dir + listNeighborLeft.ElementAt(0));
            }
            //Láng giềng cấp 2: tên lớp đại diện của cụm giống nhau
            string nameclassLeft = EDleft.getClassMax(dir + getFileED(fileLeafLeft));
            string nameclassRight = EDright.getClassMax(dir + getFileED(fileLeafRight));
            if (nameclassLeft == nameclassRight)
            {
                tfc.WriteLineTextFile(Path.GetFileName(fileLeafLeft), dir + listNeighborRight.ElementAt(1));
                tfc.WriteLineTextFile(Path.GetFileName(fileLeafRight), dir + listNeighborLeft.ElementAt(1));
            }
            //Láng giềng cấp 3: có quan hệ cha con IS-A
            if (IsAClass(nameclassLeft, nameclassRight, FileClass) || IsAClass(nameclassRight, nameclassLeft, FileClass))
            {
                tfc.WriteLineTextFile(Path.GetFileName(fileLeafLeft), dir + listNeighborRight.ElementAt(2));
                tfc.WriteLineTextFile(Path.GetFileName(fileLeafRight), dir + listNeighborLeft.ElementAt(2));
            }
            //Láng giềng cấp 4: khoảng cách trọng số đại diện nhỏ hơn ngưỡng theta
            List<double> WeightLeft = getWeightED(fileLeafLeft);
            List<double> WeightRight = getWeightED(fileLeafRight);
            if (EuclideDistance(WeightLeft, WeightRight) < theta)
            {
                tfc.WriteLineTextFile(Path.GetFileName(fileLeafLeft), dir + listNeighborRight.ElementAt(3));
                tfc.WriteLineTextFile(Path.GetFileName(fileLeafRight), dir + listNeighborLeft.ElementAt(3));
            }

            //Cập nhật từ nút lá tới gốc
            UpdateClusterInCTree(fileLeafLeft, dir + getFileParent(fileLeafLeft));
            UpdateClusterInCTree(fileLeafRight, dir + getFileParent(fileLeafRight));

            //Nếu như nút cha của nút lá bị đầy thì tiếp tục tách nút trong
            if (getIsRoot(fileLeaf) == false)
                if (getNumEC(fileParent) > N)
                    SplitNodeCLuster(theta, fileParent);

            //XÓA TẬP TIN fileLeaf VÌ ĐÃ TÁCH NÚT
        }

        public void SplitNodeCLuster(double theta, string fileNode)
        {
            //1. Chọn hai phần tử xa tâm nhất theo độ đo Euclide (lưu ý điều kiện số phần tử ED phải lớn hơn 2, tức là M > 2)
            ElementCenter ECleft = new ElementCenter();
            ElementCenter ECright = new ElementCenter();
            string dir = Path.GetDirectoryName(fileNode) + "\\";
            string fileEC = dir + getFileEC(fileNode);
            ECleft = ECleft.getFarestECcenter(fileEC);
            ECright = ECright.getFarestEC(ECleft, fileEC);
            //Xóa hai phần tử ECleft, ECright trong tập tin fileEC
            ECleft.DelElementCenter(ECleft.FileNameChild, ECleft.Values, fileEC);
            ECright.DelElementCenter(ECright.FileNameChild, ECright.Values, fileEC);

            //Lấy danh sách các phần tử Element Center còn lại
            List<ElementCenter> ListElementCenter = ECleft.getListElementCenter(fileEC);

            ClusterNode NodeLeft = new ClusterNode(false, false);
            ClusterNode NodeRight = new ClusterNode(false, false);
            //Tạo ra tên file cho nút mới
            string fileNodeLeft = dir + getNameLeafNode(dir, "Node");
            NodeLeft.SaveCluster(fileNodeLeft);
            string fileNodeRight = dir + getNameLeafNode(dir, "Node");
            NodeRight.SaveCluster(fileNodeRight);

            //Lưu vết sự thay đổi tên tập tin
            TextfileCluster tfc = new TextfileCluster();
            string strRenameNode = Path.GetFileName(fileNode) + "  " + Path.GetFileName(fileNodeLeft) + "  " + Path.GetFileName(fileNodeRight);
            tfc.WriteLineTextFile(strRenameNode, RenameNodeFile);

            
            
            //Cập nhật fileParent
            string fileRoot = dir + "Root.txt";
            if (getIsRoot(fileNode) == true)
            {
                ClusterNode Root = new ClusterNode(false, true);
                
                Root.SaveCluster(fileRoot);
                updateFileParent("Root.txt", fileNodeLeft);
                updateFileParent("Root.txt", fileNodeRight);
            }
            else
            {
                updateFileParent(getFileParent(fileNode), fileNodeLeft);
                updateFileParent(getFileParent(fileNode), fileNodeRight);
            }

            //Cập nhật danh sách tên tập tin chứa các Cluster con (chưa sử dụng)
            updateListFileChild(null, fileNodeLeft);
            updateListFileChild(null, fileNodeRight);
            //Cập nhật fileLevel (chưa sử dụng vì chưa có đồ thị)
            //int level = getLevel(fileNodeLeft);
            //string levelfilename = "Level" + level.ToString() + ".txt";
            //if (!File.Exists(dir + levelfilename))
            //    File.Create(dir + levelfilename);
            //updateFileLevel(levelfilename, fileNodeLeft);
            //updateFileLevel(levelfilename, fileNodeRight);

            //Phân bố các phần tử vào hai cụm
            ECleft.Filename = Path.GetFileName(fileNodeLeft);
            NodeLeft.AddECtoNodeCLuster(ECleft, fileNodeLeft);
            updateFileParent(Path.GetFileName(fileNodeLeft), dir + ECleft.FileNameChild);
            ECright.Filename = Path.GetFileName(fileNodeRight);
            NodeRight.AddECtoNodeCLuster(ECright, fileNodeRight);
            updateFileParent(Path.GetFileName(fileNodeRight), dir + ECright.FileNameChild);
            if (ListElementCenter.Count > 0)
            {
                foreach (ElementCenter ec in ListElementCenter)
                    if (EuclideDistance(ec.Values, ECleft.Values) < EuclideDistance(ec.Values, ECright.Values))
                    {
                        ec.Filename = Path.GetFileName(fileNodeLeft);
                        NodeLeft.AddECtoNodeCLuster(ec, fileNodeLeft);
                        updateFileParent(Path.GetFileName(fileNodeLeft), dir + ec.FileNameChild);
                    }
                    else
                    {
                        ec.Filename = Path.GetFileName(fileNodeRight);
                        NodeRight.AddECtoNodeCLuster(ec, fileNodeRight);
                        updateFileParent(Path.GetFileName(fileNodeRight), dir + ec.FileNameChild);
                    }
            }

            //XÓA TẬP TIN fileED VÌ ĐÃ TÁCH NÚT

            //Tạo hai thành phần ECLeft và Right để kết nối vào nút cha
            ElementCenter ECParentLeft = new ElementCenter(false);
            ElementCenter ECParentRight = new ElementCenter(false);
            ECParentLeft.Index = 0;
            ECParentLeft.Values = getCenterEC(fileNodeLeft);
            ECParentLeft.IsNextLeaf = false;
            ECParentLeft.Filename = Path.GetFileName(getFileParent(fileNodeLeft));
            ECParentLeft.FileNameChild = Path.GetFileName(fileNodeLeft);

            ECParentRight.Index = 0;
            ECParentRight.Values = getCenterEC(fileNodeRight);
            ECParentRight.IsNextLeaf = false;
            ECParentRight.Filename = Path.GetFileName(getFileParent(fileNodeRight));
            ECParentRight.FileNameChild = Path.GetFileName(fileNodeRight);
            //Nếu nút hiện tại là nút gốc thì tạo ra một nút gốc mới
            string fileParent = dir + getFileParent(fileNode);
            if (getIsRoot(fileNode) == true)
            {

                AddECtoNodeCLuster(ECParentLeft, fileRoot);
                AddECtoNodeCLuster(ECParentRight, fileRoot);

                //XÓA TẬP TIN fileNode VÌ ĐÃ TÁCH NÚT
            }
            //Ngược lại thêm vào nút cha ParentNode, nếu số phần tử lớn hơn M thì tiếp tục tách nút cha
            else
            {
                //string fileParent = dir + getFileParent(fileNode);
                AddECtoNodeCLuster(ECParentLeft, fileParent);
                AddECtoNodeCLuster(ECParentRight, fileParent);
                //Xóa bỏ thành phần EC của nút đã bị tách tại nút cha
                DelECofNodeCLuster(Path.GetFileName(fileNode), fileParent);
            }

            //Cập nhật thông tin cho hai nút mới
            updateIsRoot(false, fileNodeLeft); updateIsLeaf(false, fileNodeLeft);
            updateIsRoot(false, fileNodeRight); updateIsLeaf(false, fileNodeRight);
            //Tạo tên tập tin chứa các fileCluster láng giềng
            List<string> listNeighborLeft = new List<string>();
            listNeighborLeft.Add("Neighbor" + "1" + Path.GetFileName(fileNodeLeft));
            listNeighborLeft.Add("Neighbor" + "2" + Path.GetFileName(fileNodeLeft));
            updateListFileCluster(listNeighborLeft, fileNodeLeft);
            List<string> listNeighborRight = new List<string>();
            listNeighborRight.Add("Neighbor" + "1" + Path.GetFileName(fileNodeRight));
            listNeighborRight.Add("Neighbor" + "2" + Path.GetFileName(fileNodeRight));
            updateListFileCluster(listNeighborRight, fileNodeRight);
            //Cập nhật danh sách file láng giềng
            if (getIsRoot(fileNode) == false)
            {
                List<string> listNeighbor = getListFileCluster(fileNode);
                string[] Lines = tfc.ReadAllLine(dir + listNeighbor.ElementAt(0));
                Lines = RemoveBlank(Lines);
                if (Lines != null)
                { tfc.WriteNewTextFile(Lines, dir + listNeighborLeft.ElementAt(0)); tfc.WriteNewTextFile(Lines, dir + listNeighborRight.ElementAt(0)); }
                Lines = tfc.ReadAllLine(dir + listNeighbor.ElementAt(1));
                Lines = RemoveBlank(Lines);
                if (Lines != null)
                { tfc.WriteNewTextFile(Lines, dir + listNeighborLeft.ElementAt(1)); tfc.WriteNewTextFile(Lines, dir + listNeighborRight.ElementAt(1)); }
                Console.WriteLine("Xong cap nhat parent neighbor !");

            }
            //Láng giềng cấp 1: hai tâm cụm gần nhau (nhỏ hơn giá trị ngưỡng theta)
            List<double> centerLeft = getCenterEC(fileNodeLeft);
            List<double> centerRight = getCenterEC(fileNodeRight);
            if (EuclideDistance(centerLeft, centerRight) < theta)
            {
                tfc.WriteLineTextFile(Path.GetFileName(fileNodeLeft), dir + listNeighborRight.ElementAt(0));
                tfc.WriteLineTextFile(Path.GetFileName(fileNodeRight), dir + listNeighborLeft.ElementAt(0));
            }
            //Láng giềng cấp 2: khoảng cách trọng số đại diện nhỏ hơn ngưỡng theta
            List<double> WeightLeft = getWeightEC(fileNodeLeft);
            List<double> WeightRight = getWeightEC(fileNodeRight);
            if (EuclideDistance(WeightLeft, WeightRight) < theta)
            {
                tfc.WriteLineTextFile(Path.GetFileName(fileNodeLeft), dir + listNeighborRight.ElementAt(1));
                tfc.WriteLineTextFile(Path.GetFileName(fileNodeRight), dir + listNeighborLeft.ElementAt(1));
            }

            //Cập nhật từ nút hiện tại tới gốc
            UpdateClusterInCTree(fileNodeLeft, dir + getFileParent(fileNodeLeft));
            UpdateClusterInCTree(fileNodeRight, dir + getFileParent(fileNodeRight));


            //Nếu như nút iNode bị đầy thì tiếp tục tách nút trong
            if (getIsRoot(fileNode) == false)
                if (getNumEC(fileParent) > N)
                    SplitNodeCLuster(theta, fileParent);

            //XÓA TẬP TIN fileNode VÌ ĐÃ TÁCH NÚT
        }

        //Spliting Cluster
        //Merge Cluster
        //Training cluster (k-NN, SOM, ANN)
        //Cập nhật trọng số
        //Tính khảng cách đến các nút đồng cấp


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////     Các phương thức để xử lý cây H-Tree                                                         ///////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void SaveClusterHTree()
        {
            //string fileCluster = "Cluster_" + this.ClusterId + ".txt";
            string fileCluster = clusterID == 0 ? "Root.txt" : "Cluster_" + this.ClusterId + ".txt";
            string ClusterFilePath =  Path.Combine(GlobalVariable.DefaultHTreePath, fileCluster);
            
            string lineCluster = string.Empty;
            TextfileCluster tfc = new TextfileCluster(ClusterFilePath);

            //Lưu trữ số lượng phần tử dữ liệu và phần tử tâm, tuy nhiên phải trùng số lượng trong dang sách tập tin lưu trữ ED và EC
            //lineCluster += numED.ToString() + "\r\n";
            //lineCluster += numEC.ToString() + "\r\n";
            //Lưu trữ id của node
            lineCluster += ClusterId.ToString() + "\r\n";
            //Lưu trữ id của node cha
            lineCluster += parentID.ToString() + "\r\n";
            //Lưu trữ level 
            lineCluster += Level.ToString() + "\r\n";

            //Lưu trữ giá trị kiểm tra (bool) nút lá và nút gốc (2 nút có vai trò trực tiếp để trích xuất dữ liệu)
            lineCluster += isLeaf.ToString() + " " + isRoot.ToString() + "\r\n";
            //Lưu trữ vector trọng số cho các phần tử dữ liệu ED, dùng để làm tri thức bổ sung trong truy vấn và huấn luyện
            lineCluster += Vector2String(weightED) + "\r\n";
            //Lưu trữ vectow trọng số cho các phần tử tâm EC, dùng để làm tri thức bổ sung trong truy vấn và huấn luyện
            lineCluster += Vector2String(weightEC) + "\r\n";
            //Lưu trữ tập tin lưu trữ cluster, là tập tin hiện hành (đầu vào)
            lineCluster += "(" + Path.GetFileName(fileCluster) + ")" + " ";
            //Tạo tập tin để lưu trữ các phần tử dữ liệu ED, nếu chưa tồn tại thì tạo ra một tập tin mới
            //Nếu tập tin này đã tồn tại thì lấy số lượng phần tử để cập nhật, nếu chưa tồn tại thì tạo tập tin mới
            if (this.fileNameED == string.Empty)
            {
                this.fileNameED = "list" + Path.GetFileNameWithoutExtension(ClusterFilePath) + "ED" + ".txt";
                FileStream fs = null;
                using (fs = File.Create(Path.Combine(GlobalVariable.DefaultHTreePath, this.fileNameED))) { }
            }
            else
            {
                ElementData ED = new ElementData();
                if (ED.getListElementData(Path.Combine(GlobalVariable.DefaultHTreePath, this.fileNameED)) != null)
                    this.listED = ED.getListElementData(Path.Combine(GlobalVariable.DefaultHTreePath, this.fileNameED));
                this.numED = this.listED.Count;
            }
            //Lưu tên file lưu trữ các phần tử dữ liệu ED
            lineCluster += "(" + this.fileNameED + ")" + " ";
            //Tạo tập tin để lưu trữ các phần tử dữ liệu ED, nếu chưa tồn tại thì tạo ra một tập tin mới
            //Nếu tập tin này đã tồn tại thì lấy số lượng phần tử để cập nhật, nếu chưa tồn tại thì tạo tập tin mới
            //Nếu là nút lá thì không cần phải tạo ra tập tin này. (tức là bỏ qua)
            if (this.fileNameEC == string.Empty)
            {
                this.fileNameEC = "list" + Path.GetFileNameWithoutExtension(ClusterFilePath) + "EC" + ".txt";
                FileStream fs = null;
                using (fs = File.Create(Path.Combine(GlobalVariable.DefaultHTreePath, this.fileNameEC))) { }
            }
            else
            {
                ElementCenter EC = new ElementCenter();
                if (EC.getListElementCenter(Path.Combine(GlobalVariable.DefaultHTreePath, this.fileNameEC)) != null)
                    this.ListEC = EC.getListElementCenter(Path.Combine(GlobalVariable.DefaultHTreePath, this.fileNameEC));
                this.numEC = this.ListEC.Count;
            }
            //Lưu tên file lưu trữ các phần tử dữ liệu EC
            lineCluster += "(" + this.fileNameEC + ")" + "\r\n";
            //Cập nhật só lượng phần tử ED và EC cho thông tin của Cluster
            lineCluster += numED.ToString() + "\r\n" + numEC.ToString() + "\r\n";
            //Danh sách tên các tập tin của các cụm dữ liệu láng giềng theo đồ thị
            string tmp = string.Empty;
            if (listFileNameCluster.Count > 0)
                foreach (string file in listFileNameCluster)
                    tmp += "(" + file + ")" + " ";
            else
                tmp = "(#NONE#)";
            //Lưu danh sách tên các Cluster láng giềng
            lineCluster += tmp.Trim() + "\r\n";
            //Danh sách tên các tập tin của các cụm con (Child) theo phân cấp
            tmp = string.Empty;
            if (ListFileChild.Count > 0)
                foreach (string file in listFileNameChild)
                    tmp += "(" + file + ")" + " ";
            else
                tmp = "(#NONE#)";
            //Lưu danh sách tên các cụm con (Child) phân cấp
            lineCluster += tmp.Trim() + "\r\n";
            //Lưu tên file chứa các cụm đồng cấp để quản lý đồ thị theo cấp
            //Nếu một file Level
            if (fileNameLevel != string.Empty)
                lineCluster += "(" + fileNameLevel + ")" + " ";
            else
                lineCluster += "(#NONE#)" + " ";
            //Lưu tên file quản lý các cụm nút cha của nút hiện hành
            if (fileNameParent != string.Empty)
                lineCluster += "(" + fileNameParent + ")" + "\r\n";
            else
                lineCluster += "(#NONE#)" + "\r\n";
            //Lưu trữ vector tâm các phần tử dữ liệu ED
            lineCluster += Vector2String(centerED) + "\r\n";
            //Lưu trữ vector tâm các phần tử tâm EC
            lineCluster += Vector2String(centerEC);

            tfc.WriteNewTextFile(lineCluster);
        }

        // Lấy đường dẫn file cluster
        public string getClusterFilePathHTree(string filename)
        {
            return filename.Contains("Root") ? Path.Combine(GlobalVariable.DefaultHTreePath, "Root.txt") : Path.Combine(GlobalVariable.DefaultHTreePath, filename.Replace(".txt", "") + ".txt"); 
        }

        //Lấy level của cluster
        public int getClusterLevelHTree(string filename)
        {
            string clusterFilePath = getClusterFilePathHTree(filename);
            TextfileCluster tfc = new TextfileCluster(clusterFilePath);
            string[] Lines = tfc.ReadAllLine();
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return 0;
            Lines = RemoveBlank(Lines);
            string line = Lines[2].Trim();
            return ToInt32(line);
        }

        //Lấy Center ED từ file cluster
        public List<double> getCenterEDClusterHTree(string cluster)
        {
            string clusterFilePath = getClusterFilePathHTree(cluster);
            TextfileCluster tfc = new TextfileCluster(clusterFilePath);
            string[] Lines = tfc.ReadAllLine();
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return null;
            Lines = RemoveBlank(Lines);
            List<double> line = String2Vector(Lines[12]);
            return line;
        }

        //Lấy Center EC từ file cluster
        public List<double> getCenterECClusterHTree(string cluster)
        {
            string clusterFilePath = getClusterFilePathHTree(cluster);
            TextfileCluster tfc = new TextfileCluster(clusterFilePath);
            string[] Lines = tfc.ReadAllLine();
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return null;
            Lines = RemoveBlank(Lines);
            List<double> line = String2Vector(Lines[13]);
            return line;
        }

        //Lấy cluster ID
        public int getClusterIDHTree(string filename)
        {
            string clusterFilePath = getClusterFilePathHTree(filename);
            TextfileCluster tfc = new TextfileCluster(clusterFilePath);
            string[] Lines = tfc.ReadAllLine();
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return 0;
            Lines = RemoveBlank(Lines);
            string line = Lines[0].Trim();
            return ToInt32(line);
        }

        //public List<string> getListFileChildHTree(string fileCluster)
        //{
        //    List<string> ListFileChilds = new List<string>();
        //    string clusterPath = getClusterFilePathHTree(fileCluster);
        //    TextfileCluster tfc = new TextfileCluster(clusterPath);
        //    string[] Lines = tfc.ReadAllLine();
        //    if (Lines == null) return null;
        //    Lines = RemoveBlank(Lines);
        //    if (Lines.Length == 0) return null;
        //    string line = Lines[10];
        //    ListFileChilds = String2WordsList(line);
        //    return ListFileChilds;
        //}

        public List<string> GetListFileChildHTree(string filename) // tham số là 1 file cluster node
        {
            string clusterPath = getClusterFilePathHTree(filename);
            string[] Lines = File.ReadAllLines(clusterPath); // Đọc tất cả các giá trị trong file cluster node
            if (Lines.Length == 0) return null;
            if (Lines[10] != "(#NONE#)")
            {
                string listFileChild = Lines[10];
                string[] words = listFileChild.Split('(', ')');
                words = RemoveBlank(words);
                List<string> ListFileChilds = words.ToList();
                return ListFileChilds;
            }
            return null;
        }

        /* add thêm element vào ListED.txt của một file Cluster node */
        public void AddClusterEDHTree(ElementData ED)
        {
            //ClusterNode clusterNode = new ClusterNode();
            //string clusterName = "Cluster_" + this.ClusterId.ToString();
            string ClusterEDPath = this.getClusterEDFilePathByIdHTree(this.ClusterId);
            ED.SaveElementData(ClusterEDPath); //Tuấn: Tạm thời dùng hàm này

            //Số lượng phần tử ED sẽ thay đổi, Vector trọng số bị thay đổi, Vetor tâm bị thay đổi
            //Cập nhật lại tâm (values) của phần tử tâm EC ở cụm nút cha tương ứng, cập nhật tâm của phần tử EC từ cụm hiện hành đến gốc

            //2. Cập nhật số lượng phần tử dữ liệu Element Data của cụm nút lá
            TextfileCluster tfc = new TextfileCluster(ClusterEDPath);
            string[] Lines = tfc.ReadAllLine();
            Lines = RemoveBlank(Lines);
            int numEDnew = Lines.Length;
            string pathFileCluster = getClusterFilePathByIdHTee(this.clusterID);
            updateNumEDHTree(numEDnew, getClusterNameHTree(this.clusterID));
            //3. Cập nhật vector trọng số, tức là vector trung bình của các lớp xuất hiện nhiều nhất
            List<double> WeightEDnew = ED.getWeightHTree(ClusterEDPath);
            updateWeightEDHTree(WeightEDnew, pathFileCluster);
            //4. Cập nhật vector tâm tại cụm hiện hành
            List<double> CenterEDnew = ED.getCenterEDHTree(ClusterEDPath);
            updateCenterEDHTree(CenterEDnew, pathFileCluster);


            //FileStream FSED = new FileStream(ClusterEDPath, FileMode.Append);
            //using (StreamWriter writer = new StreamWriter(FSED))
            //{
            //    Console.WriteLine(ED.Feature);
            //    writer.Write("(");
            //    foreach (var feature in ED.Feature)                         // ghi vector feature
            //    {
            //        writer.Write(feature.ToString() + " ");
            //    }
            //    writer.Write(")\t");

            //    writer.Write("(" + ED.ImageID.ToString() + ".jpg)\t");      // tên ảnh
            //    writer.Write("(#NONE#) \t");                                                 // đường dẫn của ảnh
            //    writer.Write("(" + clusterNode.Classes2String(ED.ListClass) + ")\n");                   // id phân lớp của ảnh
            //}
            //FSED.Close();
        }

        // Lấy đường dẫn folder cluster bằng ID
        public string getClusterNameHTree(int clusterID)
        {
            return clusterID == 0 ? "Root" : "Cluster_" + clusterID;
        }

        // Lấy đường dẫn file clusterED
        public string getClusterEDFilePathHTree(string clusterName)
        {
            return clusterName.Contains("Root") ? Path.Combine(GlobalVariable.DefaultHTreePath, "listRootED.txt") : Path.Combine(GlobalVariable.DefaultHTreePath, "list" + clusterName.Replace(".txt", "") + "ED.txt");
        }

        // Lấy đường dẫn file clusterED bằng clusterId
        public string getClusterEDFilePathByIdHTree(int clusterId)
        {
            return clusterId == 0 ? Path.Combine(GlobalVariable.DefaultHTreePath, "listRootED.txt") : Path.Combine(GlobalVariable.DefaultHTreePath, "listCluster_" + clusterId.ToString() + "ED.txt");
        }

        // Lấy đường dẫn file clusterEC
        public string getClusterECFilePathHTree(string clusterName)
        {
            return clusterName.Contains("Root") ? Path.Combine(GlobalVariable.DefaultHTreePath, "listRootEC.txt") : Path.Combine(GlobalVariable.DefaultHTreePath, "list" + clusterName.Replace(".txt", "") + "EC.txt");
        }

        // Lấy đường dẫn file clusterEC bằng clusterId
        public string getClusterECFilePathByIdHTree(int clusterId)
        {
            return clusterId == 0 ? Path.Combine(GlobalVariable.DefaultHTreePath, "listRootEC.txt") : Path.Combine(GlobalVariable.DefaultHTreePath, "listCluster_" + clusterId.ToString() + "EC.txt");
        }

        // Lấy đường dẫn file cluster bằng ID
        public string getClusterFilePathByIdHTee(int ClusterID)
        {
            return ClusterID == 0 ? Path.Combine(GlobalVariable.DefaultHTreePath, "Root.txt") : Path.Combine(GlobalVariable.DefaultHTreePath, "Cluster_" + ClusterID + ".txt");
        }

        public void LoadClusterHTree(string filename)
        {
            string clusterFilePath = getClusterFilePathHTree(filename);
            TextfileCluster tfc = new TextfileCluster(clusterFilePath);
            string[] Lines = tfc.ReadAllLine();
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return;
            char[] delimiters = new char[] { '\t', '\r', '\n', ';', '!', ':', ',', ' ' };
            //string[] words = Lines[0].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            // Cluster ID
            this.clusterID = ToInt32(Lines[0]);         ///[0]
            // Parent ID
            this.parentID = ToInt32(Lines[1]);          ///[1]
            // Radius
            //this.radius = ToDecimal(Lines[2]);          ///[2]
            //Level
            this.level = ToInt32(Lines[2]);             ///[2]

            //Load isLeaf isRoot
            string[] words = Lines[3].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            words = RemoveBlank(words);
            this.isLeaf = bool.Parse(words[0]);
            this.isRoot = bool.Parse(words[1]);
            //Load trọng số của các phần tử ED để thực hiện huấn luyện và làm tri thức bổ sung trong việc chọn cụm
            if (Lines[4] != "#NONE#")
                this.weightED = String2Vector(Lines[4]);
            //Load trọng số của các phần từ EC để thực hiện huấn luyện và làm tri thức bổ sung trong việc chọn cụm
            if (Lines[5] != "#NONE#")
                this.weightEC = String2Vector(Lines[5]);
            //Load tên file cả cụm, nếu chưa có tên thì lấy tên file đầu vào hiện tại
            words = Lines[6].Split('(', ')');
            words = RemoveBlank(words);
            if (words[0].Trim() == "#NONE#")
                this.fileName = filename;
            else
                this.fileName = words[0].Trim();
            //Load trọng số của các phần tử ED để thực hiện huấn luyện và làm tri thức bổ sung trong việc chọn cụm
            //string[] words = Lines[5].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            ////Load trọng số của các phần từ EC để thực hiện huấn luyện và làm tri thức bổ sung trong việc chọn cụm
            //if (Lines[5] != "#NONE#")                   ///[5]
            //    this.listEDClass = null;// String2ListEDClass(Lines[5]);

            //Lấy tên file chứa các phần tử ED, nếu chưa có thì tạo ra một file mới
            //Nếu đã có thì đọc các phần tử và cập nhật lại số lượng phần tử ED
            FileStream fs = null;
            if (!File.Exists(getClusterEDFilePathHTree(filename)))
            {
                using (fs = File.Create(getClusterEDFilePathHTree(filename))) { }
            }
            if (words[1].Trim() != "#NONE#")
            {
                this.fileNameED = words[1].Trim();
                ElementData ED = new ElementData();
                if (ED.getListElementData(Path.Combine(GlobalVariable.DefaultHTreePath, this.fileNameED)) != null)
                    this.listED = ED.getListElementData(Path.Combine(GlobalVariable.DefaultHTreePath, this.fileNameED));
                this.numED = this.listED.Count;
            }
            else
            {
                this.fileNameED = Path.GetFileName(getClusterEDFilePathHTree(filename));
            }

            //File EC
            if (!File.Exists(getClusterECFilePathHTree(filename)))
            {
                using (fs = File.Create(getClusterECFilePathHTree(filename))) { }
            }
            //Lấy tên file chứa các phần tử EC, nếu chưa có thì tạo ra một file mới
            //Nếu đã có file thì đọc các phần tử và cập nhật lại số lượng phần tử EC
            if (words[2].Trim() != "#NONE#")
            {
                this.fileNameEC = words[2].Trim();
                ElementCenter EC = new ElementCenter();
                if (EC.getListElementCenterHTree(Path.Combine(GlobalVariable.DefaultHTreePath, this.fileNameEC)) != null)
                { 
                    this.ListEC = EC.getListElementCenterHTree(Path.Combine(GlobalVariable.DefaultHTreePath, this.fileNameEC));
                }    
                this.numEC = this.ListEC.Count;
            }
            else
            {
                this.fileNameEC = Path.GetFileName(getClusterECFilePathHTree(filename));
            }

            //Cập nhật lại số lượng phần tử ED và EC cho đúng với số lượng trong tập tin chứa các phần tử ED, EC
            updateNumEDHTree(this.numED, filename);     //[7]
            updateNumECHTree(this.numEC, filename);       // [8]
            //Lấy danh sách các tập tin Cluster láng giềng (đồng cấp), lưu ý mỗi file phân biệt bằng cặp dấu ngoặc "( )"
            if (Lines[9] != "(#NONE#)")
                this.listFileNameCluster = String2WordsList(Lines[9]);

            //List File Child
            if (Lines[10] != "(#NONE#)")                 ///[10]
                this.listFileNameChild = String2WordsList(Lines[10]);
            //File Level
            //words = Lines[7].Split('(', ')');
            //words = RemoveBlank(words);
            //Lấy tên tập tin lưu trữ danh sách các Cụm đồng cấp, trong trường hợp là Root thì có giá trị là #NONE#
            //if (words[0] != "(#NONE#)")
            //this.fileLevel = words[0];

            ////Lấy danh sách các tập tin Cluster láng giềng (đồng cấp), lưu ý mỗi file phân biệt bằng cặp dấu ngoặc "( )"
            //if (Lines[9] != "(#NONE#)")
            //    this.listFileNameCluster = String2WordsList(Lines[9]);
            // File Parent
            //Lấy tên tập tin lưu trữ Cụm nút cha, trong trường hợp là Root thì có giá trị là #NONE#
            words = Lines[11].Split('(', ')');                   //[11]
            words = RemoveBlank(words);
            if (words[1] != "(#NONE#)")
                this.fileNameParent = words[1];

            //Lấy vector tâm của các phần tử dữ liệu ED
            if (Lines[12] != "#NONE#")                           //[12]
                this.centerED = String2Vector(Lines[12]);
            //Lấy vector tâm của các phần tử tâm EC
            if (Lines[13] != "#NONE#")
                this.centerEC = String2Vector(Lines[13]);
        }

        //Cập nhật số lượng phần tử Element Data
        public void updateNumEDHTree(int numEDnew, string fileCluster)
        {
            string clusterFilePath = getClusterFilePathHTree(fileCluster);
            TextfileCluster tfc = new TextfileCluster(clusterFilePath);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return;
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return;
            Lines = RemoveBlank(Lines);
            Lines[7] = numEDnew.ToString();
            tfc.WriteNewTextFile(Lines);
        }

        //Câp nhật số lượng phần tử Element Center
        public void updateNumECHTree(int numECnew, string fileCluster)
        {
            string clusterFilePath = getClusterFilePathHTree(fileCluster);
            TextfileCluster tfc = new TextfileCluster(clusterFilePath);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return;
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return;
            Lines = RemoveBlank(Lines);
            Lines[8] = numECnew.ToString();
            tfc.WriteNewTextFile(Lines);
        }

        /* Update lại EC của 1 file cluster node */
        public void AddClusterEC() // Tham số truyền vào là 1 tên file cluster node , center và tên file của node con
        {
            ClusterNode cluster = new ClusterNode();
            cluster.LoadClusterHTree("Cluster_" + clusterID + ".txt");
            //string parentPath = cluster.getClusterPathByIdHTee(cluster.ParentID);
            //string childPath = cluster.getClusterPathByIdHTee(cluster.ClusterId);
            string childClusterName = "Cluster_" + this.ClusterId.ToString();

            //Thêm center của cluster con vô file ClusterEC của cluster cha
            string parentECPath = cluster.getClusterECFilePathByIdHTree(cluster.ParentID);
            FileStream FSPEC = new FileStream(parentECPath, FileMode.Append);
            using (StreamWriter writer = new StreamWriter(FSPEC))
            {
                writer.Write("(");
                foreach (var v in this.CenterED)                       // Ghi tưng phần tử center vào file
                {
                    writer.Write(v.ToString() + " ");
                }
                writer.Write(")\t");
                writer.Write("(" + childClusterName + ")\n");  // Tên file của node con trong ListEC
            }
            FSPEC.Close(); // Đóng file ClusterEC.txt của cluster node 

            //Cập nhật số lượng phần tử Element Center
            TextfileCluster tfc = new TextfileCluster(parentECPath);
            string[] Lines = tfc.ReadAllLine();
            Lines = RemoveBlank(Lines);
            int numECnew = Lines.Length;
            string parentFileName = getClusterNameHTree(cluster.parentID);
            updateNumECHTree(numECnew, parentFileName);
            //Câp nhật phần tử tâm CenterEC tại cụm hiện hành
            ElementCenter EC = new ElementCenter();
            List<double> CenterECnew = EC.getCenterECHTree(parentECPath);
            updateCenterECHTree(CenterECnew, getClusterFilePathByIdHTee(cluster.parentID));
            //Cập nhật trọng số WeightEC tại Cluster hiện hành
            //List<double> Weightnew = EC.getWeightEC(parentECPath);
            //updateWeightEC(Weightnew, fileCluster);


            ////Thêm center của cluster con vô file ClusterEC của cluster con
            //string childECPath = childPath + "\\ClusterEC.txt";
            //FileStream FSCEC = new FileStream(childECPath, FileMode.Append);
            //using (StreamWriter writer = new StreamWriter(FSCEC))
            //{
            //    writer.Write("(");
            //    foreach (var v in this.CenterED)                       // Ghi tưng phần tử center vào file
            //    {
            //        writer.Write(v.ToString() + " ");
            //    }
            //    writer.Write(")\t");
            //    writer.Write("(" + childClusterName + ")\n");  // Tên file của node con trong ListEC
            //}
            //FSCEC.Close(); // Đóng file ClusterEC.txt của cluster node
        }

        //Cập nhật vector tâm các phần tử tâm Element Center
        public void updateCenterECHTree(List<double> CenterECnew, string fileCluster)
        {
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return;
            Lines[13] = Vector2String(CenterECnew);
            tfc.WriteNewTextFile(Lines);
        }

        //Cập nhật vector tâm các phần tử dữ liệu Element Data
        public void updateCenterEDHTree(List<double> CenterEDnew, string fileCluster)
        {
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return;
            Lines[12] = Vector2String(CenterEDnew);
            tfc.WriteNewTextFile(Lines);
        }

        public void UpdateListFileChildHTree() // Tham số truyền vào là 1 name cluster node và tên file node con cần update vào
        {
            ClusterNode cluster = new ClusterNode();
            //string childPath = cluster.getClusterPathByID(this.ClusterID) + "\\Cluster.txt";
            //string parentPath = cluster.getClusterPathByID(this.ParentID) + "\\Cluster.txt";
            string childPath = cluster.getClusterFilePathByIdHTee(this.ClusterId);
            string parentPath = cluster.getClusterFilePathByIdHTee(this.ParentID);

            //string clusterPath = GlobalVariables.DefaultHTreePath + parentClusterName + "\\Cluster.txt";
            string[] Lines = File.ReadAllLines(parentPath); // Đọc tất cả các giá trị trong file cluster node
            string newChild = "(Cluster_" + this.ClusterId + ")";
            if (Utils.Instance.getString(Lines[10]) == "#NONE#")                                    // Kiểm tra nếu links null thì chỉ cần add vào 
                Lines[10] = newChild;
            else
                Lines[10] = Lines[10].Trim('\n') + "\t" + newChild;   // Nếu cluster node đã có link thì cần cộng chuỗi vào
            File.WriteAllLines(parentPath, Lines, Encoding.UTF8); // Sau khi update xong ghi tất cả vào lại file cluster node
        }

        //Cập nhật giá trị trọng số cho tập Element Data từ tập tin Cluster
        public void updateWeightEDHTree(List<double> weightEDnew, string fileCluster)
        {
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return;
            Lines[4] = Vector2String(weightEDnew);
            tfc.WriteNewTextFile(Lines);
        }

        //Create new cluster node, it not for the root node
        public void createNewClusterHTree(ElementData ED)
        {
            string fileNameCluster = this.getClusterNameHTree(this.clusterID);
            this.SaveClusterHTree();
            //this.AddED(ED);

            //this.createFileWeightED();
            //this.updateWeightED(ED, "Cluster_" + this.ClusterID);
            //this.updateWeightED(ED); //cập nhật weightED nhỏ trong file cluster
            this.AddClusterEDHTree(ED);
            this.AddClusterEC();
            this.UpdateListFileChildHTree();
            //this.defineNeighbor(fileNameCluster);
            this.updateListClusterNeighbor(fileNameCluster);
        }

        /* Lấy cluster có khoảng cách ngắn nhất so với vecto đầu vào */
        public Tuple<double, string> getClusterMinHTree(string clusterName, ElementData ED)
        {
            ClusterNode cluster = new ClusterNode();
            string clusterFileECPath = cluster.getClusterECFilePathHTree(clusterName);
            Console.WriteLine(clusterFileECPath);
            string[] Lines = File.ReadAllLines(clusterFileECPath); // Đọc tất cả các giá trị trong file cluster EC 
           
            if (Lines.Length == 0) return null;
            double EuclideDistance;
            List<Tuple<double, string>> tempList = new List<Tuple<double, string>>(); // Tạo 1 list để giữ list các giá trị <độ đo euclide , file name>
            foreach (var clusterCenter in Lines)
            {
                string[] words = clusterCenter.Split('(',')');
                words = RemoveBlank(words);
                string center = words[0].Trim();
                string childClusterName = words[1].Trim();
                Console.WriteLine(center);
                EuclideDistance = Utils.Instance.EuclideDistance(String2Vector(center), ED.Feature);        // Tính độ đo euclide của center vừa đọc lên với vector đưa vào
                tempList.Add(new Tuple<double, string>(EuclideDistance, childClusterName));                 // Tính xong sẽ add vào list cùng với tên file

            }
            //tempList.OrderBy(x => x.Item1).First(); // Sắp xếp tăng dần theo độ đo euclide và lấy phần tử thấp nhất là first                    
            Tuple<double, string> clusterMin = tempList.OrderBy(x => x.Item1).First();
            double dMin = clusterMin.Item1;
            string CMName = clusterMin.Item2;
            //ClusterNode CNMin = new ClusterNode();
            //CNMin.LoadCluster(CMName);

            //if (dMin < GlobalVariables.Epsilon || CNMin.isLeaf == true)
            //{
            //    return clusterMin; // nội tại
            //}
            //else if (dMin < (CNMin.Radius + CNMin.Radius / 2))
            //{
            //    getClusterMin(CMName, ED);
            //}
            return clusterMin;
        }

        //Cập nhật danh sách tên tập tin các Cluster láng giềng từ tập tin Cluster
        public void updateListFileNeighbor(List<string> listNameFilenew, string fileCluster)
        {
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return;
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return;
            Lines[9] = StrList2Element(listNameFilenew);
            tfc.WriteNewTextFile(Lines);
        }

        //Lấy danh sách các file name neighbor
        public List<string> getListFileNeighbor(string filename)
        {
            string clusterPath = getClusterFilePathHTree(filename);
            string[] Lines = File.ReadAllLines(clusterPath); // Đọc tất cả các giá trị trong file cluster node
            if (Lines.Length == 0) return null;
            if (Lines[9] != "(#NONE#)")
            {
                string listFIleNeighbor = Lines[9];
                string[] words = listFIleNeighbor.Split('(', ')');
                words = RemoveBlank(words);
                List<string> fileNeighborList = words.ToList();
                return fileNeighborList;
            }
            return null;
        }

        //Định nghĩa láng giềng 
        public void defineNeighborHTree(string fileNameCluster)
        {
            // Tạo danh sách cluster neighbor và cập nhật vô file cluster
            //Ở đây chỉ tạo một file neighbor đầu tiên định nghĩa cluster anh em
            List<string> listNeighbor = getFileNameStoreClusterNeighbor(fileNameCluster, 1);

            //listNeighbor.Add("Neighbor" + "1" + fileNameCluster.Replace(".txt", ""));
            //listNeighbor.Add("Neighbor" + "2" + Path.GetFileName(fileNameCluster));
            //listNeighbor.Add("Neighbor" + "3" + Path.GetFileName(fileNameCluster));
            //listNeighbor.Add("Neighbor" + "4" + Path.GetFileName(fileNameCluster));
            string fileCluster = getClusterFilePathHTree(fileNameCluster);
            updateListFileNeighbor(listNeighbor, fileCluster);
            FileStream fs = null;
            foreach (string neighbor in listNeighbor)
            {
                if (!File.Exists(Path.Combine(GlobalVariable.DefaultHTreePath, neighbor)))
                {
                    using (fs = File.Create(Path.Combine(GlobalVariable.DefaultHTreePath, neighbor))) { }
                }
            }

        }

        //Hàm tạo, lấy ra tên file lưu trữ cluster neighbor
        public List<string> getFileNameStoreClusterNeighbor(string filenameCluster, int numOfNeighborFile)
        {
            List<string> listFileName = new List<string>();
            for(int i=0; i < numOfNeighborFile; i++)
            {
                listFileName.Add("Neighbor" + (i + 1) + filenameCluster.Replace(".txt", "") + ".txt");
            }
            return listFileName;
        }

        //Hàm lấy ra path các file lưu trữ cluster neighbor
        public List<string> getFilePathStoreClusterNeighbor(string filenameCluster, int numOfNeighborFile)
        {
            List<string> listFilePath = new List<string>();
            for (int i = 0; i < numOfNeighborFile; i++)
            {
                listFilePath.Add(Path.Combine(GlobalVariable.DefaultHTreePath, "Neighbor" + (i + 1) + filenameCluster.Replace(".txt", "") + ".txt"));
            }
            return listFilePath;
        }

        //Tạo, cập nhật list cluster neighbor trong các file neighbor
        public void updateListClusterNeighbor(string fileNameCluster)
        {
            //Tạo danh sách file láng giềng
            if (getClusterIDHTree(fileNameCluster) != 0)
            {
                TextfileCluster tfc = new TextfileCluster();
                // Lấy danh sách các tên các cluster đồng cấp
                List<string> listClusterNeighbor = GetListFileChildHTree(getClusterNameHTree(this.parentID));
                //Nếu không có cluster con nào thì không làm gì
                if (listClusterNeighbor == null) return;
                //duyệt qua từng cluster neighbor và tìm ra file lưu trữ neighbor của nó, cập nhật và thay đổi file đó
                foreach(string clusterneighbor in listClusterNeighbor)
                {
                    //Danh sách path các file lưu danh sách neighbor của cluster
                    List<string> listFilePathStoreClusterNeighbor = getFilePathStoreClusterNeighbor(clusterneighbor, 1);
                    foreach (string filePathStoreClusterNeighbor in listFilePathStoreClusterNeighbor)
                    {
                        //Lấy danh sách tên cluster neighbor
                        //string[] listNeighbor = tfc.ReadAllLine(filePathStoreClusterNeighbor);

                        //Danh sách neighbor đã cập nhật
                        string[] listNeighborNew = listClusterNeighbor.Where(x => x != clusterneighbor).ToArray();
                        if (File.Exists(Path.Combine(GlobalVariable.DefaultHTreePath, filePathStoreClusterNeighbor)))
                        {
                            File.Delete(Path.Combine(GlobalVariable.DefaultHTreePath, filePathStoreClusterNeighbor));
                        }
                        //Lưu xuống file
                        //tfc.WriteNewTextFile(listClusterNeighbor.ToArray(), filePathStoreClusterNeighbor);
                        tfc.WriteNewTextFile(listNeighborNew, filePathStoreClusterNeighbor);
                    }
                }

                ////Lấy danh sách tên file neighbor trong file cluster
                //List<string> listFileNeighbor = getListFileNeighbor(fileNameCluster);

                //string dir = GlobalVariable.DefaultHTreePath;
                //string[] Lines = tfc.ReadAllLine(dir + listFileNeighbor.ElementAt(0));
                //Lines = RemoveBlank(Lines);
                ////cập nhật các tên file trong Lines (truy vết tên cũ đã xóa để thay thế bằng tên mới)
                //if (Lines != null)
                //    tfc.WriteNewTextFile(Lines, dir + listFileNeighbor.ElementAt(0));

                //Lines = tfc.ReadAllLine(dir + listFileNeighbor.ElementAt(1));
                //Lines = RemoveBlank(Lines);
                //if (Lines != null)
                //    tfc.WriteNewTextFile(Lines, dir + listFileNeighbor.ElementAt(1));
                //Lines = tfc.ReadAllLine(dir + listFileNeighbor.ElementAt(2));
                //Lines = RemoveBlank(Lines);
                //if (Lines != null)
                //    tfc.WriteNewTextFile(Lines, dir + listFileNeighbor.ElementAt(2));  
                //Lines = tfc.ReadAllLine(dir + listFileNeighbor.ElementAt(3));
                //Lines = RemoveBlank(Lines);
                //if (Lines != null)
                //    tfc.WriteNewTextFile(Lines, dir + listFileNeighbor.ElementAt(3));
            }
            //Láng giềng cấp 1: hai tâm cụm gần nhau (nhỏ hơn giá trị ngưỡng theta)
            //List<double> centerLeft = getCenterED(fileLeafLeft);
            //List<double> centerRight = getCenterED(fileLeafRight);
            //if (EuclideDistance(centerLeft, centerRight) < theta)
            //{
            //    tfc.WriteLineTextFile(Path.GetFileName(fileLeafLeft), dir + listNeighborRight.ElementAt(0));
            //    tfc.WriteLineTextFile(Path.GetFileName(fileLeafRight), dir + listNeighborLeft.ElementAt(0));
            //}
        }

        //hàm tìm kiếm
        public List<ElementData> findRelevantED(List<double> feature)
        {
            ElementData elementData = new ElementData();
            elementData.Feature = feature;
            List<ElementData> elementDatas = getListFileNameSearchResult(elementData);
            Console.WriteLine("Find Image relevant vector success!");
            return elementDatas;
        }

        //lấy danh sách cluster dựa vào KNN => từ cluster level 01 đến lá
        public List<ElementData> getListFileNameSearchResult(ElementData ED)
        {
            string root = "Root.txt";
            Stack<string> STACKstart = new Stack<string>();
            List<ElementData> elementDatas = new List<ElementData>();
            List<string> _listClusterKNN = new List<string>();
            string clusterParent = String.Empty;
            string clusterMain = String.Empty;
            //Duyệt qua từng cluster và tìm cluster gần nhất
            Tuple<double, string> clusterMin = getClusterMinHTree(root, ED);
            STACKstart.Push(clusterMin.Item2);
            _listClusterKNN.Add(clusterMin.Item2);

            while (STACKstart.Count > 0)
            {
                string nameFile = STACKstart.Pop();
                List<string> listChild = GetListFileChildHTree(nameFile);
                if (listChild != null)
                {
                    _listClusterKNN.AddRange(listChild);
                    listChild.ForEach(i => STACKstart.Push(i));
                }
            }
            Console.WriteLine(_listClusterKNN.Count+ "khiem");
            _listClusterKNN = calculateNN(ED, _listClusterKNN);
            foreach (var item in _listClusterKNN)
            {
                string pathFileED = getClusterEDFilePathHTree(item);
                //từ các cluster tương tự ta lấy ra list ED
                elementDatas.AddRange(ED.getListElementDataHTree(pathFileED));
            }

            // lấy phần % số ảnh lấy ra
            //int percent = (elementDatas.Count * 40) / 100;//----------------------------------------------------------------------------------------
            //elementDatas = elementDatas.Take(percent).ToList();

            // ghi file các cluster tương tự để dùng lại
            string pathSave = GlobalVariable.DefaultClusterSimilarPath;
            File.WriteAllLines(pathSave, _listClusterKNN, Encoding.UTF8);

            return elementDatas.ToList();
        }

        //tính khoảng cách giữa vector search vào với các cluster lân cận
        //ghi file classes
        private List<string> calculateNN(ElementData ED, List<string> _listClusterKNN)
        {
            string root = "Root";
            Stack<string> STACKstart = new Stack<string>();
            List<ElementData> elementDatas = new List<ElementData>();
            string clusterMain = string.Empty;
            STACKstart.Push(root);
            while (STACKstart.Count > 0)
            {
                string name = STACKstart.Pop();
                List<string> listFileChild = GetListFileChildHTree(name);
                if (listFileChild == null && name != root)
                {
                    clusterMain = name;
                }
                else
                {
                    Tuple<double, string> clusterMin = getClusterMinHTree(name, ED);
                    if (clusterMin.Item1 <= GlobalVariable.Epsilon)
                    {
                        clusterMain = clusterMin.Item2;
                    }
                    else
                        STACKstart.Push(clusterMin.Item2);
                }
            }

            _listClusterKNN.Remove(clusterMain);

            List<double> centerMain = getCenterEDClusterHTree(clusterMain);

            List<Tuple<double, string>> _listResult = new List<Tuple<double, string>>();
            foreach (var item in _listClusterKNN)
            {
                double d = Utils.Instance.EuclideDistance(centerMain, getCenterEDClusterHTree(item));
                _listResult.Add(new Tuple<double, string>(d, item));
            }

            _listResult = _listResult.OrderBy(x => x.Item1).ToList();

            _listClusterKNN.Clear();
            foreach (var item in _listResult)
            {
                _listClusterKNN.Add(item.Item2);
            }
            return _listClusterKNN;
        }

        //Lấy ra cluster tương tự đầu tiên trong file clusterSimilar
        public string getFirstClusterSimilar(string fileClusterSimilar)
        {
            TextfileCluster tfc = new TextfileCluster();
            //Láy danh sách cluster tương tự (cluster neighbor)
            string[] clusterSimilar = tfc.ReadAllLine(fileClusterSimilar);
            clusterSimilar = RemoveBlank(clusterSimilar);
            if (clusterSimilar.Length == 0) return null;
            clusterSimilar = RemoveBlank(clusterSimilar);
            string firstCluster = clusterSimilar[0].Trim();
            return firstCluster;
        }

        //Lấy ra cluster tương tự thứ 2 trong file clusterSimilar
        public string getSecondClusterSimilar(string fileClusterSimilar)
        {
            TextfileCluster tfc = new TextfileCluster();
            //Láy danh sách cluster tương tự (cluster neighbor)
            string[] clusterSimilar = tfc.ReadAllLine(fileClusterSimilar);
            clusterSimilar = RemoveBlank(clusterSimilar);
            if (clusterSimilar.Length == 0) return null;
            clusterSimilar = RemoveBlank(clusterSimilar);
            string firstCluster = clusterSimilar[1].Trim();
            return firstCluster;
        }

        //Tìm tất cả ED của tất cả các cluster neighbor và trả về list ED
        public List<ElementData> getListEDOfAllClusterNeighbor(List<string> listClusterNeighbor)
        {
            List<ElementData> Res = new List<ElementData>();
            ElementData ED = new ElementData();
            //Duyệt qua từng file neighbor, tìm file ED của nó và add vào list ED
            foreach(string cluster in listClusterNeighbor)
            {
                string fileEDCluster = getClusterEDFilePathHTree(cluster);
                List<ElementData> listEDClusterNeighbor = ED.getListElementDataHTree(fileEDCluster);
                //Tim cac cluster con va neighbor cua cluster con neu co
                if(GetListFileChildHTree(cluster) != null)
                {
                    getListEDOfAllClusterNeighbor(GetListFileChildHTree(cluster));
                }
                Res = ED.AddListED(Res, listEDClusterNeighbor);
            }
            return Res;
        }

        //Hàm để gọi ở form, dùng để lấy ra list ED của cluster tương tự nhất và các cluster neighbor của nó
        public List<ElementData> getListEdInClusterNeighbor(string fileClusterSimilar)
        {
            List<ElementData> Res = new List<ElementData>();
            ElementData ED = new ElementData();
            //Tìm cluster tương tự nhất và lấy ra list ED của nó
            string firstClusterSimilar = getFirstClusterSimilar(fileClusterSimilar);
            Res = ED.AddListED(Res, getListEDNeighbor(firstClusterSimilar));
            //Tìm cluster tương tự thứ 2 và lấy ra list ED của nó
            string secondClusterSimilar = getFirstClusterSimilar(fileClusterSimilar);
            Res = ED.AddListED(Res, getListEDNeighbor(secondClusterSimilar));
            return Res;
        }

        public List<ElementData> getListEDNeighbor(string clusterName)
        {
            List<ElementData> Res = new List<ElementData>();
            ClusterNode clusterSimilar = new ClusterNode();
            ElementData ED = new ElementData();
            clusterSimilar.LoadClusterHTree(clusterName);
            string fileEDfirstClusterSimilar = getClusterEDFilePathHTree(clusterName);
            Res = ED.getListElementDataHTree(fileEDfirstClusterSimilar);
            //Tìm cluster neighbor của cluster tương tự nhất
            List<string> listFileClusterNeighbor = GetListFileChildHTree(getClusterNameHTree(clusterSimilar.parentID));
            List<ElementData> listEDClusterNeighbor = getListEDOfAllClusterNeighbor(listFileClusterNeighbor);
            //Laay ra list ED cua cac cluster con cua cluster tuong tu nhat
            List<ElementData> listEdInClusterChildOfFirstClusterSimilar = getListEDOfAllClusterNeighbor(GetListFileChildHTree(clusterName));

            //Kết hợp 2 list ED lại
            Res = ED.AddListED(Res, listEDClusterNeighbor);
            Res = ED.AddListED(Res, listEdInClusterChildOfFirstClusterSimilar);
            return Res;
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////     Các phương thức để xử lý cây GP-Tree                                                         ///////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        //Lưu một cluster
        public void SaveClusterGPTree(string fileCluster)
        {
            Console.WriteLine("begin save cluster gp");
            string lineCluster = string.Empty;
            TextfileCluster tfc = new TextfileCluster(fileCluster);

            //Lưu trữ số lượng phần tử dữ liệu và phần tử tâm, tuy nhiên phải trùng số lượng trong dang sách tập tin lưu trữ ED và EC
            //lineCluster += numED.ToString() + "\r\n";
            //lineCluster += numEC.ToString() + "\r\n";
            //Lưu trữ giá trị kiểm tra (bool) nút lá và nút gốc (2 nút có vai trò trực tiếp để trích xuất dữ liệu)
            lineCluster += isLeaf.ToString() + " " + isRoot.ToString() + "\r\n";
            //Lưu trữ vector trọng số cho các phần tử dữ liệu ED, dùng để làm tri thức bổ sung trong truy vấn và huấn luyện
            lineCluster += Vector2String(weightED) + "\r\n";
            //Lưu trữ vectow trọng số cho các phần tử tâm EC, dùng để làm tri thức bổ sung trong truy vấn và huấn luyện
            lineCluster += Vector2String(weightEC) + "\r\n";
            //Lưu trữ tập tin lưu trữ cluster, là tập tin hiện hành (đầu vào)
            lineCluster += "(" + Path.GetFileName(fileCluster) + ")" + " ";
            //Tạo tập tin để lưu trữ các phần tử dữ liệu ED, nếu chưa tồn tại thì tạo ra một tập tin mới
            //Nếu tập tin này đã tồn tại thì lấy số lượng phần tử để cập nhật, nếu chưa tồn tại thì tạo tập tin mới
            if (IsLeaf == false)
                this.fileNameED = "#NONE#";
            else
            {
                if (this.fileNameED == string.Empty)
                {
                    this.fileNameED = "list" + Path.GetFileNameWithoutExtension(fileCluster) + "ED" + ".txt";
                    FileStream fs = null;
                    //if (!File.Exists(Path.GetDirectoryName(fileCluster) + "\\" + this.fileNameED))
                    //{
                    using (fs = File.Create(Path.GetDirectoryName(fileCluster) + "\\" + this.fileNameED)) { }
                    //}
                }
                else
                {
                    ElementData ED = new ElementData();
                    if (ED.getListElementData(Path.GetDirectoryName(fileCluster) + "\\" + this.fileNameED) != null)
                        this.listED = ED.getListElementData(Path.GetDirectoryName(fileCluster) + "\\" + this.fileNameED);
                    this.numED = this.listED.Count;
                }
            }
            //Lưu tên file lưu trữ các phần tử dữ liệu ED
            lineCluster += "(" + this.fileNameED + ")" + " ";
            //Tạo tập tin để lưu trữ các phần tử dữ liệu ED, nếu chưa tồn tại thì tạo ra một tập tin mới
            //Nếu tập tin này đã tồn tại thì lấy số lượng phần tử để cập nhật, nếu chưa tồn tại thì tạo tập tin mới
            //Nếu là nút lá thì không cần phải tạo ra tập tin này. (tức là bỏ qua)
            if (IsLeaf == true)
                this.fileNameEC = "#NONE#";
            else
            {
                if (this.fileNameEC == string.Empty)
                {
                    this.fileNameEC = "list" + Path.GetFileNameWithoutExtension(fileCluster) + "EC" + ".txt";
                    FileStream fs = null;
                    //if (!File.Exists(Path.GetDirectoryName(fileCluster) + "\\" + this.fileNameEC))
                    //{
                    using (fs = File.Create(Path.GetDirectoryName(fileCluster) + "\\" + this.fileNameEC)) { }
                    //}
                }
                else
                {
                    ElementCenter EC = new ElementCenter();
                    if (EC.getListElementCenterGPTree(Path.GetDirectoryName(fileCluster) + "\\" + this.fileNameEC) != null)
                        this.ListEC = EC.getListElementCenterGPTree(Path.GetDirectoryName(fileCluster) + "\\" + this.fileNameEC);
                    this.numEC = this.ListEC.Count;
                }
            }
            //Cập nhật só lượng phần tử ED và EC cho thông tin của Cluster
            lineCluster = numED.ToString() + "\r\n" + numEC.ToString() + "\r\n" + lineCluster;
            //Lưu tên file lưu trữ các phần tử dữ liệu EC
            lineCluster += "(" + this.fileNameEC + ")" + "\r\n";
            //Danh sách tên các tập tin của các cụm dữ liệu láng giềng theo đồ thị
            string tmp = string.Empty;
            if (listFileNameCluster.Count > 0)
                foreach (string file in listFileNameCluster)
                    tmp += "(" + file + ")" + " ";
            else
                tmp = "(#NONE#)";
            //Lưu danh sách tên các Cluster láng giềng
            lineCluster += tmp.Trim() + "\r\n";
            //Danh sách tên các tập tin của các cụm con (Child) theo phân cấp
            tmp = string.Empty;
            if (ListFileChild.Count > 0)
                foreach (string file in listFileNameChild)
                    tmp += "(" + file + ")" + " ";
            else
                tmp = "(#NONE#)";
            //Lưu danh sách tên các cụm con (Child) phân cấp
            lineCluster += tmp.Trim() + "\r\n";
            //Lưu tên file chứa các cụm đồng cấp để quản lý đồ thị theo cấp
            //Nếu một file Le
            if (fileNameLevel != string.Empty)
                lineCluster += "(" + fileNameLevel + ")" + " ";
            else
                lineCluster += "(#NONE#)" + " ";
            //Lưu tên file quản lý các cụm nút cha của nút hiện hành
            if (fileNameParent != string.Empty)
                lineCluster += "(" + fileNameParent + ")" + "\r\n";
            else
                lineCluster += "(#NONE#)" + "\r\n";
            //Lưu trữ vector tâm các phần tử dữ liệu ED
            lineCluster += Vector2String(centerED) + "\r\n";
            //Lưu trữ vector tâm các phần tử tâm EC
            lineCluster += Vector2String(centerEC) + "\r\n";
            lineCluster += level.ToString() + "\r\n";

            tfc.WriteNewTextFile(lineCluster);

            Console.WriteLine("End save cluster gp");
        }

        //Lấy level của cluster
        public int getClusterLevelGPTree(string fileCluster)
        {
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return 0;
            Lines = RemoveBlank(Lines);
            string line = Lines[11].Trim();
            return ToInt32(line);
        }
        
        //Update level file cluster
        public void updateLevelClusterGPTree(int level, string fileCluster)
        {
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return;
            Lines[11] = level.ToString();
            tfc.WriteNewTextFile(Lines);
        }

        //Tạo một nút mới là nút con của fileParent và thêm ED vào nút mới tạo đó
        public string createNewLeafOnNode(ElementData ED, string fileParent)
        {
            Console.WriteLine("begin create new leaf on node");
            string dir = Path.GetDirectoryName(fileParent) + "\\";
            int level = getClusterLevelGPTree(fileParent);
            ClusterNode NewLeaf = new ClusterNode(true, false, level+1);
            NewLeaf.fileNameParent = Path.GetFileName(fileParent);
            //Tạo ra tên file cho nút lá mới
            string fileNewLeaf = dir + getNameLeafNode(dir, "Leaf");
            NewLeaf.SaveClusterGPTree(fileNewLeaf);

            //Cập nhật fileNameParent cho file new leaf và thêm ED vào
            //updateFileParent(Path.GetFileName(fileParent), fileNewLeaf);
            NewLeaf.AddEDtoLeafCLusterGPTree(ED, fileNewLeaf);
            //Cập nhật danh sách tên tập tin chứa các Cluster con.
            updateListFileChild(null, fileNewLeaf);

            //Tạo thành phần ECNewLeaf để kết nối vào nút cha
            ElementCenter ECNewLeaf = new ElementCenter(true);

            ECNewLeaf.Index = 0;
            ECNewLeaf.Values = getCenterED(fileNewLeaf);
            ECNewLeaf.IsNextLeaf = true;
            ECNewLeaf.Filename = Path.GetFileName(fileParent);
            ECNewLeaf.FileNameChild = Path.GetFileName(fileNewLeaf);
            ECNewLeaf.MaxClass = ED.getClassMax(dir + getFileED(fileNewLeaf));

            //Thêm EC mới vào nút cha
            AddECtoNodeCLusterGPTree(ECNewLeaf, fileParent);

            ////Cập nhật thông tin cho nút lá mới
            //updateIsRoot(false, fileNewLeaf);

            //return file leaf vừa tạo để ra ngoài cập nhật lại center từ file này tới nút gốc
            Console.WriteLine("end create new leaf on node");
            return fileNewLeaf;
        }

        //Cập nhật vector tâm các phần tử dữ liệu Element Data
        public void updateCenterED_GPTree(List<double> CenterEDnew, string fileCluster)
        {
            TextfileCluster tfc = new TextfileCluster(fileCluster);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return;
            Lines[9] = Vector2String(CenterEDnew);
            tfc.WriteNewTextFile(Lines);
        }

        //Thêm một phân tử dữ liệu vào cụm nút lá
        public void AddEDtoLeafCLusterGPTree(ElementData ED, string fileCluster)
        {
            Console.WriteLine("begin add ed to leaf");
            //Số lượng phần tử ED sẽ thay đổi, Vector trọng số bị thay đổi, Vetor tâm bị thay đổi
            //Cập nhật lại tâm (values) của phần tử tâm EC ở cụm nút cha tương ứng, cập nhật tâm của phần tử EC từ cụm hiện hành đến gốc
            string dir = Path.GetDirectoryName(fileCluster) + "\\";
            string filenameED = getFileED(fileCluster);
            string fileED = dir + filenameED;
            if (filenameED == "#NONE#") return; //Lúc này là nút trong của cây C-Tree, R-Tree, Kd-Tree, GP Tree

            //1.Thêm một phần tử vào danh sách tại nút lá của cây
            ED.SaveElementData(fileED);
            //2. Cập nhật số lượng phần tử dữ liệu Element Data của cụm nút lá
            TextfileCluster tfc = new TextfileCluster(fileED);
            string[] Lines = tfc.ReadAllLine();
            Lines = RemoveBlank(Lines);
            int numEDnew = Lines.Length;
            updateNumED(numEDnew, fileCluster);
            //3. Cập nhật vector trọng số, tức là vector trung bình của các lớp xuất hiện nhiều nhất
            List<double> WeightEDnew = ED.getWeight(fileED);
            updateWeightED(WeightEDnew, fileCluster);

            //4. Cập nhật vector tâm tại cụm hiện hành

            //Cách duyệt lại toàn bộ ED để tính tâm (1)
            //List<double> CenterEDnew = ED.getCenterED_GPTree(fileED);

            //Cách mới (2)
            // Nếu là ED đầu tiên thì lấy nó làm tâm, ngược lại lấy tâm cũ cộng lại chia trung bình với vector ED đưa vào
            if (numEDnew == 1)
            {
                updateCenterED_GPTree(ED.Feature, fileCluster);
            }
            else
            {
                List<double> CenterED = getCenterED(fileCluster);
                List<double> CenterEDNew = new List<double>();
                int n = CenterED.Count;
                for (int i = 0; i < n; i++)
                {
                    double value = (CenterED.ElementAt(i) + ED.Feature.ElementAt(i)) / 2.0;
                    CenterEDNew.Add(value);
                }
                updateCenterED_GPTree(CenterEDNew, fileCluster);
            }

            Console.WriteLine("end add ed to leaf");
        }

        //Định nghĩa neighbor cấp 2 giữa 2 nút truyền vào
        public void defineNeighborTwo(string firstLeaf, string secondLeaf)
        {
            Console.WriteLine("begin define neighbor 2");
            //Tạo tên tập tin chứa các fileCluster láng giềng
            List<string> listNeighborLeft = new List<string>();
            listNeighborLeft.Add("Neighbor" + "2" + Path.GetFileName(firstLeaf));
            updateListFileCluster(listNeighborLeft, firstLeaf);
            List<string> listNeighborRight = new List<string>();
            listNeighborRight.Add("Neighbor" + "2" + Path.GetFileName(secondLeaf));
            updateListFileCluster(listNeighborRight, secondLeaf);

            //Bắt đầu định nghĩa láng giềng cấp 2 cho 2 nút
            TextfileCluster tfc = new TextfileCluster();
            ElementData ED = new ElementData();
            string dir = Path.GetDirectoryName(secondLeaf) + "\\";
            //Láng giềng cấp 2: tên lớp đại diện của cụm giống nhau
            string nameclassLeft = ED.getClassMax(dir + getFileED(firstLeaf));
            string nameclassRight = ED.getClassMax(dir + getFileED(secondLeaf));
            if (nameclassLeft == nameclassRight)
            {
                tfc.WriteLineTextFile(Path.GetFileName(firstLeaf), dir + listNeighborRight.ElementAt(0));
                tfc.WriteLineTextFile(Path.GetFileName(secondLeaf), dir + listNeighborLeft.ElementAt(0));
            }
            Console.WriteLine("Xong Defined Neighbor level 2");
            Console.WriteLine("end define neighbor");
        }

        //Định nghĩa neighbor giữa 2 nút truyền vào
        public void defineAllNeighbor(string firstLeaf, string secondLeaf)
        {
            Console.WriteLine("begin define neighbor");
            //Tạo tên tập tin chứa các fileCluster láng giềng
            List<string> listNeighborLeft = new List<string>();
            listNeighborLeft.Add("Neighbor" + "1" + Path.GetFileName(firstLeaf));
            listNeighborLeft.Add("Neighbor" + "2" + Path.GetFileName(firstLeaf));
            updateListFileCluster(listNeighborLeft, firstLeaf);
            List<string> listNeighborRight = new List<string>();
            listNeighborRight.Add("Neighbor" + "1" + Path.GetFileName(secondLeaf));
            listNeighborRight.Add("Neighbor" + "2" + Path.GetFileName(secondLeaf));
            updateListFileCluster(listNeighborRight, secondLeaf);

            //Bắt đầu định nghĩa láng giềng cấp 1 và cấp 2 cho 2 nút
            TextfileCluster tfc = new TextfileCluster();
            ElementData ED = new ElementData();
            string dir = Path.GetDirectoryName(secondLeaf) + "\\";

            //Láng giềng cấp 1: hai tâm cụm gần nhau (nhỏ hơn giá trị ngưỡng theta)
            List<double> centerLeft = getCenterED(firstLeaf);
            List<double> centerRight = getCenterED(secondLeaf);
            Console.WriteLine("Băt dau Defined Neighbor cap 1");
            if (EuclideDistance(centerLeft, centerRight) < 2*theta)
            {
                tfc.WriteLineTextFile(Path.GetFileName(firstLeaf), dir + listNeighborRight.ElementAt(0));
                tfc.WriteLineTextFile(Path.GetFileName(secondLeaf), dir + listNeighborLeft.ElementAt(0));
            }
            Console.WriteLine("Xong Defined Neighbor level 1");
            //Láng giềng cấp 2: tên lớp đại diện của cụm giống nhau
            string nameclassLeft = ED.getClassMax(dir + getFileED(firstLeaf));
            string nameclassRight = ED.getClassMax(dir + getFileED(secondLeaf));
            if (nameclassLeft == nameclassRight)
            {
                tfc.WriteLineTextFile(Path.GetFileName(firstLeaf), dir + listNeighborRight.ElementAt(1));
                tfc.WriteLineTextFile(Path.GetFileName(secondLeaf), dir + listNeighborLeft.ElementAt(1));
            }
            Console.WriteLine("Xong Defined Neighbor level 2");
            Console.WriteLine("end define neighbor");
        }

        //Định nghĩa neighbor cấp 2 giữa 2 nút truyền vào kết thừa neighbor cha
        public void defineNeighborTwo(string leafLeft, string leafRight,string fileParent)
        {
            Console.WriteLine("begin define neighbor 2");
            Console.WriteLine(leafLeft);
            Console.WriteLine(leafRight);
            Console.WriteLine(fileParent);
            
            //Tạo tên tập tin chứa các fileCluster láng giềng
            List<string> listNeighborLeft = new List<string>();
            listNeighborLeft.Add("Neighbor" + "2" + Path.GetFileName(leafLeft));
            updateListFileCluster(listNeighborLeft, leafLeft);
            List<string> listNeighborRight = new List<string>();
            listNeighborRight.Add("Neighbor" + "2" + Path.GetFileName(leafRight));
            updateListFileCluster(listNeighborRight, leafRight);

            //Bắt đầu định nghĩa láng giềng cấp 2 cho 2 nút
            TextfileCluster tfc = new TextfileCluster();
            ElementData ED = new ElementData();
            string dir = Path.GetDirectoryName(fileParent) + "\\";

            //Cập nhật danh sách file láng giềng
            //string fileParent = dir + getFileParent(leafParent);
            if (getIsRoot(fileParent) == false)
            {
                //Kế thừa láng giềng của nút cha
                List<string> listNeighbor = getListFileCluster(fileParent);
                string[] Lines = tfc.ReadAllLine(dir + listNeighbor.ElementAt(0));
                Lines = RemoveBlank(Lines);
                //cập nhật các tên file trong Lines (truy vết tên cũ đã xóa để thay thế bằng tên mới)
                if (Lines != null)
                { 
                    tfc.WriteNewTextFile(Lines, dir + listNeighborLeft.ElementAt(0)); 
                    tfc.WriteNewTextFile(Lines, dir + listNeighborRight.ElementAt(0)); 
                }
            }
            //Láng giềng cấp 2: tên lớp đại diện của cụm giống nhau
            string nameclassLeft = ED.getClassMax(dir + getFileED(leafLeft));
            string nameclassRight = ED.getClassMax(dir + getFileED(leafRight));
            if (nameclassLeft == nameclassRight)
            {
                tfc.WriteLineTextFile(Path.GetFileName(leafLeft), dir + listNeighborRight.ElementAt(0));
                tfc.WriteLineTextFile(Path.GetFileName(leafRight), dir + listNeighborLeft.ElementAt(0));
            }
            Console.WriteLine("Xong Defined Neighbor level 2");
            Console.WriteLine("end define neighbor 2");
        }

        //Hàm cập nhật tất cả neighbor, truyền vào một nút và duyệt hết các nút trên cây để cập nhật neighbor
        public void updateAllNeighbor(string fileLeaf, string Root)
        {
            Console.WriteLine("begin update neighbor");
            Stack<string> STACKneighbor = new Stack<string>();
            string fileEC = string.Empty;
            string fileNeighbor = string.Empty;
            string oldLeaf = string.Empty;
            string dir = Path.GetDirectoryName(Root) + "\\";
            ElementCenter EC = new ElementCenter();
            List<ElementCenter> listEC = new List<ElementCenter>();
            STACKneighbor.Push(Root);

            while (STACKneighbor.Count > 0)
            {
                fileNeighbor = STACKneighbor.Pop();
                if (getIsLeaf(fileNeighbor) == false)
                {
                    fileEC = dir + getFileEC(fileNeighbor);
                    listEC = EC.getListElementCenterGPTree(fileEC);
                    foreach (ElementCenter ec in listEC)
                    {
                        if (ec.IsNextLeaf == true)
                        {
                            oldLeaf = dir + ec.FileNameChild;
                            defineAllNeighbor(fileLeaf, oldLeaf);
                        }
                        else
                        {
                            STACKneighbor.Push(dir + ec.FileNameChild);
                        }
                    }
                }
            }
            Console.WriteLine("end update neighbor");
        }

        //Hàm cập nhật neighbor cấp 2, truyền vào một nút và duyệt hết các nút trên cây để cập nhật neighbor
        public void updateNeighborTwo(string fileLeaf, string Root)
        {
            Console.WriteLine("begin update neighbor");
            Stack<string> STACKneighbor = new Stack<string>();
            string fileEC = string.Empty;
            string fileNeighbor = string.Empty;
            string oldLeaf = string.Empty;
            string dir = Path.GetDirectoryName(Root) + "\\";
            ElementCenter EC = new ElementCenter();
            List<ElementCenter> listEC = new List<ElementCenter>();
            STACKneighbor.Push(Root);

            while (STACKneighbor.Count > 0)
            {
                fileNeighbor = STACKneighbor.Pop();
                if (getIsLeaf(fileNeighbor) == false)
                {
                    fileEC = dir + getFileEC(fileNeighbor);
                    listEC = EC.getListElementCenterGPTree(fileEC);
                    foreach (ElementCenter ec in listEC)
                    {
                        if (ec.IsNextLeaf == true)
                        {
                            oldLeaf = dir + ec.FileNameChild;
                            defineNeighborTwo(fileLeaf, oldLeaf);
                        }
                        else
                        {
                            STACKneighbor.Push(dir + ec.FileNameChild);
                        }
                    }
                }
            }
            Console.WriteLine("end update neighbor");
        }

        //Hàm xoá một neighbor
        public void removeNeighbor(string Root, string neighborName)
        {
            Console.WriteLine("begin remove neighbor");
            Stack<string> STACKneighbor = new Stack<string>();
            string fileEC = string.Empty;
            string fileNeighbor = string.Empty;
            string dir = Path.GetDirectoryName(Root) + "\\";
            ElementCenter EC = new ElementCenter();
            TextfileCluster tfc = new TextfileCluster();
            List<ElementCenter> listEC = new List<ElementCenter>();

            STACKneighbor.Push(Root);
            while (STACKneighbor.Count > 0)
            {
                fileNeighbor = STACKneighbor.Pop();
                if (getIsLeaf(fileNeighbor) == false)
                {
                    fileEC = dir + getFileEC(fileNeighbor);
                    listEC = EC.getListElementCenterGPTree(fileEC);
                    foreach (ElementCenter ec in listEC)
                    {
                        if (ec.IsNextLeaf == true)
                        {
                            //Tìm file neighbor và xoá neighbor phù hợp
                            string fileNeighbor1 = dir + "Neighbor" + "1" + ec.FileNameChild;
                            string fileNeighbor2 = dir + "Neighbor" + "2" + ec.FileNameChild;
                            tfc.DeleteFirstLine(neighborName, fileNeighbor1);
                            tfc.DeleteFirstLine(neighborName, fileNeighbor2);
                        }
                        else
                        {
                            STACKneighbor.Push(dir + ec.FileNameChild);
                        }
                    }
                }
            }
            Console.WriteLine("end remove neighbor");
        }

        //Cập nhật tâm cụm từ cụm hiện hành đên cụm nút gốc
        public void UpdateClusterInGPTree(string fileChild, string fileCluster)
        {
            Console.WriteLine("begin update GP tree");

            //1. Cập nhật lại giá trị values cho phần tử tâm cụm EC trong danh sách listEC với nút con với tên tập tin là ClusterChild
            //2. Tính lại và cập nhật vector trung bình CenterEC của cụm hiện hành
            //3. Nếu cụm hiện hành không phải là cụm nút gốc thì cập nhật nút cha (ParentNode): (gọi lại 2 bước 1 và bước 2 ứng với nút cha), nghĩa là:
            // 3.1. Cập nhật giá trị values cho phần tử tâm cụm EC trong dang sách listEC (của nút cha) với nút con là CLuster 
            // 3.2. Tính lại và cập nhật vector trung bình CenterEC của cụm hiện hành

            //Sử dụng cơ chế STACK để khử đệ quy
            string dir = Path.GetDirectoryName(fileCluster) + "\\";
            string FILEChild = string.Empty;
            string FILECluster = string.Empty;
            Stack<string> STACKchild = new Stack<string>();
            Stack<string> STACKcluster = new Stack<string>();
            //Thêm tên cụm hiện hành vào STACK
            STACKchild.Push(fileChild);
            STACKcluster.Push(fileCluster);
            List<double> values = new List<double>();
            while (STACKchild.Count > 0 && STACKcluster.Count > 0)
            {
                FILEChild = STACKchild.Pop();
                Console.WriteLine(FILEChild);
                FILECluster = STACKcluster.Pop();
                Console.WriteLine(FILECluster);
                //Xử lý cập nhật lại values của phần tử EC 
                string fileEC = dir + getFileEC(FILECluster);
                ElementCenter EC = new ElementCenter();
                string fileNameChild = Path.GetFileName(FILEChild);
                //Nếu là phần tử liên kết đến nút lá thì lấy trung bình của các phần tử dữ liệu ElementData
                if (getIsLeaf(FILEChild) == true)
                {
                    values = getCenterED(FILEChild);
                    //lấy ra file ED để tìm max Class để cập nhật vô EC
                    ElementData ED = new ElementData();
                    string fileED = dir + getFileED(FILEChild);
                    string maxClass = ED.getClassMax(fileED);
                    EC.updateValuesEC_GPTree(values, fileNameChild, fileEC, maxClass);
                }
                //Nếu là phần tử không liên kết đến nút lá thì tính trung bình tâm của các phần tử tâm ElementCenter
                else
                {
                    values = getCenterEC(FILEChild);
                    EC.updateValuesEC_GPTree(values, fileNameChild, fileEC);
                }
                //Cập nhật CenterEC tại Cluster
                List<double> CenterECnew = EC.getCenterEC_GPTree(fileEC);
                updateCenterEC(CenterECnew, FILECluster);

                //Cập nhật WeightEC tại Cluster
                //List<double> Weightnew = EC.getWeightEC_GPTree(fileEC);
                //updateWeightEC(Weightnew, FILECluster);

                if (getIsRoot(FILECluster) == false)
                {
                    FILEChild = FILECluster;
                    FILECluster = dir + getFileParent(FILECluster);
                    STACKchild.Push(FILEChild);
                    STACKcluster.Push(FILECluster);
                }
                Console.WriteLine("Cập nhật cây GP Tree xong");
            }
            Console.WriteLine("end update GP tree");
        }

        //Xóa một phân tử tâm Element Center tại một cụm nút iNode
        public void DelECofNodeCLusterGPTree(string FileNameChild, string fileCluster)
        {
            Console.WriteLine("begin delete ec on node");
            //Nếu số phần tử EC của cụm = 0 thì không thực hiện
            int numEC = getNumEC(fileCluster);
            if (numEC == 0) return;
            //Lấy đường dẫn tập tin chứa danh sách các EC 
            string dir = Path.GetDirectoryName(fileCluster) + "\\";
            string filenameEC = getFileEC(fileCluster);
            string fileEC = dir + filenameEC;
            if (filenameEC == "#NONE#") return; //Lúc này là nút lá của cây C-Tree, R-Tree, Kd-Tree, GP Tree
            ElementCenter EC = new ElementCenter();
            EC.DelElementCenterGPTree(FileNameChild, fileEC);
            //Cập nhật số lượng phần tử Element Center
            TextfileCluster tfc = new TextfileCluster(fileEC);
            string[] Lines = tfc.ReadAllLine();
            Lines = RemoveBlank(Lines);
            int numECnew = Lines.Length;
            updateNumEC(numECnew, fileCluster);
            //Câp nhật phần tử tâm CenterEC tại cụm hiện hành
            List<double> CenterECnew = EC.getCenterEC_GPTree(fileEC);
            updateCenterEC(CenterECnew, fileCluster);
            //Cập nhật trọng số WeightEC tại Cluster hiện hành
            //List<double> Weightnew = EC.getWeightEC_GPTree(fileEC);
            //updateWeightEC(Weightnew, fileCluster);
            Console.WriteLine("end delete ec on node");
        }

        //Thêm một phần tử tâm Element Center vào cụm nút trong iNode
        public void AddECtoNodeCLusterGPTree(ElementCenter EC, string fileCluster)
        {
            Console.WriteLine("begin add ec on node");
            //Số lượng phần tử EC bị thay đổi, câp nhật lại trọng số WeightEC, cập nhật lại tâm CenterEC

            string dir = Path.GetDirectoryName(fileCluster) + "\\";
            string filenameEC = getFileEC(fileCluster);
            string fileEC = dir + filenameEC;
            if (filenameEC == "#NONE#") return; //Lúc này là nút lá của cây C-Tree, R-Tree, Kd-Tree, GP Tree

            EC.SaveElementCenterGPTree(fileEC);
            //Cập nhật số lượng phần tử Element Center
            TextfileCluster tfc = new TextfileCluster(fileEC);
            string[] Lines = tfc.ReadAllLine();
            Lines = RemoveBlank(Lines);
            int numECnew = Lines.Length;
            updateNumEC(numECnew, fileCluster);
            //Câp nhật phần tử tâm CenterEC tại cụm hiện hành
            List<double> CenterECnew = EC.getCenterEC_GPTree(fileEC);
            updateCenterEC(CenterECnew, fileCluster);
            //Cập nhật trọng số WeightEC tại Cluster hiện hành
            //List<double> Weightnew = EC.getWeightEC_GPTree(fileEC);
            //updateWeightEC(Weightnew, fileCluster);
            Console.WriteLine("end add ec on node");
        }

        //Hàm tách nút trong khi vượt quá ngưỡng N (số Element Center tối đa của một nút)
        public void SplitNodeCLusterGPTree(double theta, string fileNode)
        {
            Console.WriteLine("begin split node");
            //1. Chọn hai phần tử xa tâm nhất theo độ đo Euclide (lưu ý điều kiện số phần tử EC phải lớn hơn 2, tức là N > 2)
            ElementCenter ECleft = new ElementCenter();
            ElementCenter ECright = new ElementCenter();
            string dir = Path.GetDirectoryName(fileNode) + "\\";
            string fileEC = dir + getFileEC(fileNode);
            ECleft = ECleft.getFarestECcenterGPTree(fileEC);
            ECright = ECright.getFarestEC_GPTree(ECleft, fileEC);
            //Xóa hai phần tử ECleft, ECright trong tập tin fileEC
            ECleft.DelElementCenter(ECleft.FileNameChild, ECleft.Values, fileEC);
            ECright.DelElementCenter(ECright.FileNameChild, ECright.Values, fileEC);

            //Lấy danh sách các phần tử Element Center còn lại
            List<ElementCenter> ListElementCenter = ECleft.getListElementCenterGPTree(fileEC);

            ClusterNode NodeLeft = new ClusterNode(false, false);
            ClusterNode NodeRight = new ClusterNode(false, false);
            //Tạo ra tên file cho nút mới
            string fileNodeLeft = dir + getNameLeafNode(dir, "Node");
            NodeLeft.SaveClusterGPTree(fileNodeLeft);
            string fileNodeRight = dir + getNameLeafNode(dir, "Node");
            NodeRight.SaveClusterGPTree(fileNodeRight);

            //Lưu vết sự thay đổi tên tập tin
            TextfileCluster tfc = new TextfileCluster();
            string strRenameNode = Path.GetFileName(fileNode) + "  " + Path.GetFileName(fileNodeLeft) + "  " + Path.GetFileName(fileNodeRight);
            tfc.WriteLineTextFile(strRenameNode, RenameNodeFile);



            //Cập nhật fileParent
            if (getIsRoot(fileNode) == true)
            {
                updateFileParent("Root.txt", fileNodeLeft);
                updateFileParent("Root.txt", fileNodeRight);
            }
            else
            {
                updateFileParent(getFileParent(fileNode), fileNodeLeft);
                updateFileParent(getFileParent(fileNode), fileNodeRight);
            }

            //Cập nhật danh sách tên tập tin chứa các Cluster con (chưa sử dụng)
            updateListFileChild(null, fileNodeLeft);
            updateListFileChild(null, fileNodeRight);
            //Cập nhật fileLevel (chưa sử dụng vì chưa có đồ thị)
            //int level = getLevel(fileNodeLeft);
            //string levelfilename = "Level" + level.ToString() + ".txt";
            //if (!File.Exists(dir + levelfilename))
            //    File.Create(dir + levelfilename);
            //updateFileLevel(levelfilename, fileNodeLeft);
            //updateFileLevel(levelfilename, fileNodeRight);

            //Phân bố các phần tử vào hai cụm
            ECleft.Filename = Path.GetFileName(fileNodeLeft);
            NodeLeft.AddECtoNodeCLusterGPTree(ECleft, fileNodeLeft);
            updateFileParent(Path.GetFileName(fileNodeLeft), dir + ECleft.FileNameChild);
            ECright.Filename = Path.GetFileName(fileNodeRight);
            NodeRight.AddECtoNodeCLusterGPTree(ECright, fileNodeRight);
            updateFileParent(Path.GetFileName(fileNodeRight), dir + ECright.FileNameChild);
            if (ListElementCenter.Count > 0)
            {
                foreach (ElementCenter ec in ListElementCenter)
                    if (EuclideDistance(ec.Values, ECleft.Values) < EuclideDistance(ec.Values, ECright.Values))
                    {
                        ec.Filename = Path.GetFileName(fileNodeLeft);
                        NodeLeft.AddECtoNodeCLusterGPTree(ec, fileNodeLeft);
                        updateFileParent(Path.GetFileName(fileNodeLeft), dir + ec.FileNameChild);
                    }
                    else
                    {
                        ec.Filename = Path.GetFileName(fileNodeRight);
                        NodeRight.AddECtoNodeCLusterGPTree(ec, fileNodeRight);
                        updateFileParent(Path.GetFileName(fileNodeRight), dir + ec.FileNameChild);
                    }
            }

            //XÓA TẬP TIN fileEC VÌ ĐÃ TÁCH NÚT

            //Tạo hai thành phần ECLeft và Right để kết nối vào nút cha
            ElementCenter ECParentLeft = new ElementCenter(false);
            ElementCenter ECParentRight = new ElementCenter(false);
            ECParentLeft.Index = 0;
            ECParentLeft.Values = getCenterEC(fileNodeLeft);
            ECParentLeft.IsNextLeaf = false;
            ECParentLeft.Filename = Path.GetFileName(getFileParent(fileNodeLeft));
            ECParentLeft.FileNameChild = Path.GetFileName(fileNodeLeft);

            ECParentRight.Index = 0;
            ECParentRight.Values = getCenterEC(fileNodeRight);
            ECParentRight.IsNextLeaf = false;
            ECParentRight.Filename = Path.GetFileName(getFileParent(fileNodeRight));
            ECParentRight.FileNameChild = Path.GetFileName(fileNodeRight);
            //Nếu nút hiện tại là nút gốc thì tạo ra một nút gốc mới
            string fileParent = dir + getFileParent(fileNode);
            if (getIsRoot(fileNode) == true)
            {
                //Change
                ClusterNode Root = new ClusterNode(false, true);
                string fileRoot = dir + "Root.txt";
                Root.SaveClusterGPTree(fileRoot);
                //Change
                AddECtoNodeCLusterGPTree(ECParentLeft, fileRoot);
                AddECtoNodeCLusterGPTree(ECParentRight, fileRoot);
            }
            //Ngược lại thêm vào nút cha ParentNode, nếu số phần tử lớn hơn M thì tiếp tục tách nút cha
            else
            {
                //string fileParent = dir + getFileParent(fileNode);
                AddECtoNodeCLusterGPTree(ECParentLeft, fileParent);
                AddECtoNodeCLusterGPTree(ECParentRight, fileParent);
                //Xóa bỏ thành phần EC của nút đã bị tách tại nút cha
                DelECofNodeCLusterGPTree(Path.GetFileName(fileNode), fileParent);
            }

            //Cập nhật thông tin cho hai nút mới
            updateIsRoot(false, fileNodeLeft); updateIsLeaf(false, fileNodeLeft);
            updateIsRoot(false, fileNodeRight); updateIsLeaf(false, fileNodeRight);
            //Tạo tên tập tin chứa các fileCluster láng giềng
            List<string> listNeighborLeft = new List<string>();
            listNeighborLeft.Add("Neighbor" + "1" + Path.GetFileName(fileNodeLeft));
            listNeighborLeft.Add("Neighbor" + "2" + Path.GetFileName(fileNodeLeft));
            updateListFileCluster(listNeighborLeft, fileNodeLeft);
            List<string> listNeighborRight = new List<string>();
            listNeighborRight.Add("Neighbor" + "1" + Path.GetFileName(fileNodeRight));
            listNeighborRight.Add("Neighbor" + "2" + Path.GetFileName(fileNodeRight));
            updateListFileCluster(listNeighborRight, fileNodeRight);
            //Cập nhật danh sách file láng giềng
            if (getIsRoot(fileNode) == false)
            {
                List<string> listNeighbor = getListFileCluster(fileNode);
                string[] Lines = tfc.ReadAllLine(dir + listNeighbor.ElementAt(0));
                Lines = RemoveBlank(Lines);
                if (Lines != null)
                { tfc.WriteNewTextFile(Lines, dir + listNeighborLeft.ElementAt(0)); tfc.WriteNewTextFile(Lines, dir + listNeighborRight.ElementAt(0)); }
                Lines = tfc.ReadAllLine(dir + listNeighbor.ElementAt(1));
                Lines = RemoveBlank(Lines);
                if (Lines != null)
                { tfc.WriteNewTextFile(Lines, dir + listNeighborLeft.ElementAt(1)); tfc.WriteNewTextFile(Lines, dir + listNeighborRight.ElementAt(1)); }
            }
            //Láng giềng cấp 1: hai tâm cụm gần nhau (nhỏ hơn giá trị ngưỡng theta)
            List<double> centerLeft = getCenterEC(fileNodeLeft);
            List<double> centerRight = getCenterEC(fileNodeRight);
            if (EuclideDistance(centerLeft, centerRight) < theta)
            {
                tfc.WriteLineTextFile(Path.GetFileName(fileNodeLeft), dir + listNeighborRight.ElementAt(0));
                tfc.WriteLineTextFile(Path.GetFileName(fileNodeRight), dir + listNeighborLeft.ElementAt(0));
            }
            //Láng giềng cấp 2: khoảng cách trọng số đại diện nhỏ hơn ngưỡng theta
            List<double> WeightLeft = getWeightEC(fileNodeLeft);
            List<double> WeightRight = getWeightEC(fileNodeRight);
            if (EuclideDistance(WeightLeft, WeightRight) < theta)
            {
                tfc.WriteLineTextFile(Path.GetFileName(fileNodeLeft), dir + listNeighborRight.ElementAt(1));
                tfc.WriteLineTextFile(Path.GetFileName(fileNodeRight), dir + listNeighborLeft.ElementAt(1));
            }

            //Cập nhật từ nút hiện tại tới gốc
            UpdateClusterInGPTree(fileNodeLeft, dir + getFileParent(fileNodeLeft));
            UpdateClusterInGPTree(fileNodeRight, dir + getFileParent(fileNodeRight));

            //Nếu như nút iNode bị đầy thì tiếp tục tách nút trong
            if (getIsRoot(fileNode) == false)
                if (getNumEC(fileParent) > N)
                    SplitNodeCLusterGPTree(theta, fileParent);

            Console.WriteLine("end split node");
        }

        public void SplitLeafCLusterGPTree(double theta, string fileLeaf)
        {
            Console.WriteLine("begin split leaf");
            //1. Chọn hai phần tử xa tâm nhất theo độ đo Euclide (lưu ý điều kiện số phần tử ED phải lớn hơn 2, tức là M > 2)
            ElementData EDleft = new ElementData();
            ElementData EDright = new ElementData();
            string dir = Path.GetDirectoryName(fileLeaf) + "\\";
            string fileED = dir + getFileED(fileLeaf);
            EDleft = EDleft.getFarestEDcenter(fileED);
            EDright = EDright.getFarestED(EDleft, fileED);
            //Xóa hai phần tử dữ liệu EDleft, EDright trong tập tin fileED
            EDleft.DelElementData(EDleft.ImageID, EDleft.Feature, fileED);
            EDright.DelElementData(EDright.ImageID, EDright.Feature, fileED);
            //Lấy danh sách các phần tử Element Data còn lại
            List<ElementData> ListElementData = EDleft.getListElementData(fileED);

            //Tạo hai nút lá mới để phân bố các phần tử, nếu nút lá hiện hành là nút gốc thì tạo nút gốc mới để lưu tâm của 2 nút lá mới tạo
            //Nếu nút bị tách không phải nút gốc thì tạo một nút trong mới để thay thế cho nút lá cũ (nút lá trước khi tách) và lưu 2 tâm của 2 nút lá mới
            ClusterNode LeafLeft = new ClusterNode(true, false);
            ClusterNode LeafRight = new ClusterNode(true, false);
            ClusterNode NewNode = new ClusterNode(false, false);
            
            //Tạo ra tên file cho nút lá mới
            string fileLeafLeft = dir + getNameLeafNode(dir, "Leaf");
            LeafLeft.SaveClusterGPTree(fileLeafLeft);
            string fileLeafRight = dir + getNameLeafNode(dir, "Leaf");
            LeafRight.SaveClusterGPTree(fileLeafRight);

            //Lưu vết sư thay đổi tên tập tin
            TextfileCluster tfc = new TextfileCluster();
            string strRenameLeaf = Path.GetFileName(fileLeaf) + "  " + Path.GetFileName(fileLeafLeft) + "  " + Path.GetFileName(fileLeafRight);
            string RenameLeafFile = dir + "RenameLeaf.txt";
            tfc.WriteLineTextFile(strRenameLeaf, RenameLeafFile);

            //Tạo ra 2 biến để lưu tên nút trong mới và tên file nút trong mới được tạo ra nếu có
            string nameNewNode = string.Empty;
            string fileNewNode = string.Empty;
            //Cập nhật tên tập tin filenameParent
            if (getIsRoot(fileLeaf) == true) {
                updateFileParent("Root.txt", fileLeafLeft);
                updateLevelClusterGPTree(1, fileLeafLeft);
                updateFileParent("Root.txt", fileLeafRight);
                updateLevelClusterGPTree(1, fileLeafRight);
            }
            else {
                //Tạo ra tên file cho nút trong mới và lưu nó xuống file cluster
                int level = getClusterLevelGPTree(fileLeaf);
                NewNode.level = level;
                nameNewNode = getNameLeafNode(dir, "Node");
                fileNewNode = dir + nameNewNode;
                NewNode.SaveClusterGPTree(fileNewNode);
                //Cập nhật tên file parent cho các node vừa tạo ra
                updateFileParent(getFileParent(fileLeaf), fileNewNode);
                updateFileParent(nameNewNode, fileLeafLeft);
                updateFileParent(nameNewNode, fileLeafRight);
                updateLevelClusterGPTree(level+1, fileLeafLeft);
                updateLevelClusterGPTree(level+1, fileLeafRight);
            }

            if (getIsRoot(fileLeaf) == false)
            {
                List<string> listNeighbor = getListFileCluster(fileLeaf);
                string[] Lines = tfc.ReadAllLine(dir + listNeighbor.ElementAt(0));
                Lines = RemoveBlank(Lines);
                //Tiến hành cập nhật neighbor nút cha với các nút đã tồn tại trong cây
                //if (Lines.ElementAt(0) == "#NONE#")
                if (Lines == null)
                {
                    updateNeighborTwo(fileLeaf, dir + "Root.txt");
                }
                Console.WriteLine("Xong định nghĩa neighbor cho nút cha trước khi tách");
            }

            //Thêm 2 phần tử xa nhau nhất tìm được lúc đầu vào 2 nút lá
            LeafLeft.AddEDtoLeafCLusterGPTree(EDleft, fileLeafLeft);
            LeafRight.AddEDtoLeafCLusterGPTree(EDright, fileLeafRight);
            //Phân bố các phần tử còn lại vào hai cụm;
            if (ListElementData.Count > 0)
            {
                foreach (ElementData ed in ListElementData)
                    if (EuclideDistance(ed.Feature, EDleft.Feature) < EuclideDistance(ed.Feature, EDright.Feature))
                        LeafLeft.AddEDtoLeafCLusterGPTree(ed, fileLeafLeft);
                    else
                        LeafRight.AddEDtoLeafCLusterGPTree(ed, fileLeafRight);
            }

            //Cập nhật danh sách tên tập tin chứa các Cluster con.
            updateListFileChild(null, fileLeafLeft);
            updateListFileChild(null, fileLeafRight);
         
            //Tạo hai thành phần ECLeft và Right để kết nối vào nút cha
            ElementCenter ECLeft = new ElementCenter(true);
            ElementCenter ECRight = new ElementCenter(true);
  
            ECLeft.Index = 0;
            ECLeft.Values = getCenterED(fileLeafLeft);
            ECLeft.IsNextLeaf = true;
            ECLeft.Filename = Path.GetFileName(getFileParent(fileLeafLeft));
            ECLeft.FileNameChild = Path.GetFileName(fileLeafLeft);
            ECLeft.MaxClass = EDleft.getClassMax(dir + getFileED(fileLeafLeft));

            ECRight.Index = 0;
            ECRight.Values = getCenterED(fileLeafRight);
            ECRight.IsNextLeaf = true;
            ECRight.Filename = Path.GetFileName(getFileParent(fileLeafRight));
            ECRight.FileNameChild = Path.GetFileName(fileLeafRight);
            ECRight.MaxClass = EDright.getClassMax(dir + getFileED(fileLeafRight));

            //Nếu nút hiện tại là nút gốc thì tạo ra một nút gốc mới
            string fileParent = dir + getFileParent(fileLeaf);
            string fileRoot = dir + "Root.txt";
            if (getIsRoot(fileLeaf) == true)
            {
                ClusterNode Root = new ClusterNode(false, true, 0);
                Root.SaveClusterGPTree(fileRoot);
                AddECtoNodeCLusterGPTree(ECLeft, fileRoot);
                AddECtoNodeCLusterGPTree(ECRight, fileRoot);
            }
            //Ngược lại thêm vào nút trong mới tạo
            else
            {
                AddECtoNodeCLusterGPTree(ECLeft, fileNewNode);
                AddECtoNodeCLusterGPTree(ECRight, fileNewNode);

                ElementCenter ECNode = new ElementCenter(false);
                ECNode.Index = 0;
                ECNode.Values = getCenterEC(fileNewNode);
                ECNode.IsNextLeaf = false;
                ECNode.Filename = Path.GetFileName(getFileParent(fileNewNode));
                //ECNode.FileNameChild = Path.GetFileName(fileNewNode);
                ECNode.FileNameChild = nameNewNode;

                AddECtoNodeCLusterGPTree(ECNode, fileParent);

                //Xóa bỏ thành phần EC của nút đã bị tách tại nút cha
                DelECofNodeCLusterGPTree(Path.GetFileName(fileLeaf), fileParent);
            }

            //Cập nhật thông tin cho hai nút lá mới
            updateIsRoot(false, fileLeafLeft);
            updateIsRoot(false, fileLeafRight);

            //Định nghĩa neighbor cho 2 nút mới tách
            defineNeighborTwo(fileLeafLeft, fileLeafRight, fileLeaf);
            //Cập nhật từ nút lá tới gốc
            UpdateClusterInGPTree(fileLeafLeft, dir + getFileParent(fileLeafLeft));
            UpdateClusterInGPTree(fileLeafRight, dir + getFileParent(fileLeafRight));

            //Xoá neighbor của nút lá cũ
            //removeNeighbor(fileRoot, Path.GetFileName(fileLeaf));

            //Nếu như nút cha của nút lá bị đầy thì tiếp tục tách nút trong
            //if (getIsRoot(fileLeaf) == false)
            //    if (getNumEC(fileParent) > N)
            //        SplitNodeCLusterGPTree(theta, fileParent);

            Console.WriteLine("end split leaf");
        }
        
        //Áp dụng cho trường hợp nút lá có từ 2 phân lớp trở lên
        public void SplitLeafCLusterGPTree2(double theta, string fileLeaf)
        {
            Console.WriteLine("begin split leaf");
            //Chọn hai tâm của hai nhóm phần tử xuất hiện nhiều nhất
            string dir = Path.GetDirectoryName(fileLeaf) + "\\";
            string fileED = dir + getFileED(fileLeaf);
            ElementData ED = new ElementData();
            List<double> CenterFirst = ED.getWeight(fileED);
            List<double> CenterSecond = ED.getWeightSecond(fileED);
            //Lấy hai tên lớp xuất hiện nhiều nhất
            string ClassNameFirst = ED.getClassMax(fileED);
            string ClassNameSecond = ED.getClassMaxSecond(fileED);


            ElementData EDleft = new ElementData();
            ElementData EDright = new ElementData();
            EDleft = EDleft.getEDwithClassname(fileED, ClassNameFirst);
            EDright = EDright.getEDwithClassname(fileED, ClassNameSecond);
            //Xóa hai phần tử dữ liệu EDleft, EDright trong tập tin fileED
            EDleft.DelElementData(EDleft.ImageID, EDleft.Feature, fileED);
            EDright.DelElementData(EDright.ImageID, EDright.Feature, fileED);
            //Lấy danh sách các phần tử Element Data còn lại
            List<ElementData> ListElementData = EDleft.getListElementData(fileED);

            //Tạo hai nút lá mới để phân bố các phần tử, nếu nút lá hiện hành là nút gốc thì tạo nút gốc mới để lưu tâm của 2 nút lá mới tạo
            //Nếu nút bị tách không phải nút gốc thì tạo một nút trong mới để thay thế cho nút lá cũ (nút lá trước khi tách) và lưu 2 tâm của 2 nút lá mới
            ClusterNode LeafLeft = new ClusterNode(true, false);
            ClusterNode LeafRight = new ClusterNode(true, false);
            ClusterNode NewNode = new ClusterNode(false, false);

            //Tạo ra tên file cho nút lá mới
            string fileLeafLeft = dir + getNameLeafNode(dir, "Leaf");
            LeafLeft.SaveClusterGPTree(fileLeafLeft);
            string fileLeafRight = dir + getNameLeafNode(dir, "Leaf");
            LeafRight.SaveClusterGPTree(fileLeafRight);

            //Lưu vết sư thay đổi tên tập tin
            TextfileCluster tfc = new TextfileCluster();
            string strRenameLeaf = Path.GetFileName(fileLeaf) + "  " + Path.GetFileName(fileLeafLeft) + "  " + Path.GetFileName(fileLeafRight);
            string RenameLeafFile = dir + "RenameLeaf.txt";
            tfc.WriteLineTextFile(strRenameLeaf, RenameLeafFile);

            //Tạo ra 2 biến để lưu tên nút trong mới và tên file nút trong mới được tạo ra nếu có
            string nameNewNode = string.Empty;
            string fileNewNode = string.Empty;
            //Cập nhật tên tập tin filenameParent
            if (getIsRoot(fileLeaf) == true)
            {
                updateFileParent("Root.txt", fileLeafLeft);
                updateLevelClusterGPTree(1, fileLeafLeft);
                updateFileParent("Root.txt", fileLeafRight);
                updateLevelClusterGPTree(1, fileLeafRight);
            }
            else
            {
                //Tạo ra tên file cho nút trong mới và lưu nó xuống file cluster
                int level = getClusterLevelGPTree(fileLeaf);
                NewNode.level = level;
                nameNewNode = getNameLeafNode(dir, "Node");
                fileNewNode = dir + nameNewNode;
                NewNode.SaveClusterGPTree(fileNewNode);
                //Cập nhật tên file parent cho các node vừa tạo ra
                updateFileParent(getFileParent(fileLeaf), fileNewNode);
                updateFileParent(nameNewNode, fileLeafLeft);
                updateFileParent(nameNewNode, fileLeafRight);
                updateLevelClusterGPTree(level + 1, fileLeafLeft);
                updateLevelClusterGPTree(level + 1, fileLeafRight);
            }

            if (getIsRoot(fileLeaf) == false)
            {
                List<string> listNeighbor = getListFileCluster(fileLeaf);
                string[] Lines = tfc.ReadAllLine(dir + listNeighbor.ElementAt(0));
                Lines = RemoveBlank(Lines);
                //Tiến hành cập nhật neighbor nút cha với các nút đã tồn tại trong cây
                //if (Lines.ElementAt(0) == "#NONE#")
                if (Lines == null)
                {
                    updateNeighborTwo(fileLeaf, dir + "Root.txt");
                }
                Console.WriteLine("Xong định nghĩa neighbor cho nút cha trước khi tách");
            }

            //Thêm 2 phần tử xa nhau nhất tìm được lúc đầu vào 2 nút lá
            LeafLeft.AddEDtoLeafCLusterGPTree(EDleft, fileLeafLeft);
            LeafRight.AddEDtoLeafCLusterGPTree(EDright, fileLeafRight);
            //Phân bố các phần tử còn lại vào hai cụm;
            if (ListElementData.Count > 0)
            {
                foreach (ElementData ed in ListElementData)
                {
                    double disLeft = EuclideDistance(ed.Feature, CenterFirst);
                    double disRight = EuclideDistance(ed.Feature, CenterSecond);
                    if (ed.ListClass.Contains(ClassNameFirst))
                        disLeft = 0.0;
                    if (ed.ListClass.Contains(ClassNameSecond))
                        disRight = 0.0;
                    if (disLeft < disRight)
                        LeafLeft.AddEDtoLeafCLusterGPTree(ed, fileLeafLeft);
                    else
                        LeafRight.AddEDtoLeafCLusterGPTree(ed, fileLeafRight);
                }
            }

            //Cập nhật danh sách tên tập tin chứa các Cluster con.
            updateListFileChild(null, fileLeafLeft);
            updateListFileChild(null, fileLeafRight);

            //Tạo hai thành phần ECLeft và Right để kết nối vào nút cha
            ElementCenter ECLeft = new ElementCenter(true);
            ElementCenter ECRight = new ElementCenter(true);

            ECLeft.Index = 0;
            ECLeft.Values = getCenterED(fileLeafLeft);
            ECLeft.IsNextLeaf = true;
            ECLeft.Filename = Path.GetFileName(getFileParent(fileLeafLeft));
            ECLeft.FileNameChild = Path.GetFileName(fileLeafLeft);
            ECLeft.MaxClass = EDleft.getClassMax(dir + getFileED(fileLeafLeft));

            ECRight.Index = 0;
            ECRight.Values = getCenterED(fileLeafRight);
            ECRight.IsNextLeaf = true;
            ECRight.Filename = Path.GetFileName(getFileParent(fileLeafRight));
            ECRight.FileNameChild = Path.GetFileName(fileLeafRight);
            ECRight.MaxClass = EDright.getClassMax(dir + getFileED(fileLeafRight));

            //Nếu nút hiện tại là nút gốc thì tạo ra một nút gốc mới
            string fileParent = dir + getFileParent(fileLeaf);
            string fileRoot = dir + "Root.txt";
            if (getIsRoot(fileLeaf) == true)
            {
                ClusterNode Root = new ClusterNode(false, true, 0);
                Root.SaveClusterGPTree(fileRoot);
                AddECtoNodeCLusterGPTree(ECLeft, fileRoot);
                AddECtoNodeCLusterGPTree(ECRight, fileRoot);
            }
            //Ngược lại thêm vào nút trong mới tạo
            else
            {
                AddECtoNodeCLusterGPTree(ECLeft, fileNewNode);
                AddECtoNodeCLusterGPTree(ECRight, fileNewNode);

                ElementCenter ECNode = new ElementCenter(false);
                ECNode.Index = 0;
                ECNode.Values = getCenterEC(fileNewNode);
                ECNode.IsNextLeaf = false;
                ECNode.Filename = Path.GetFileName(getFileParent(fileNewNode));
                //ECNode.FileNameChild = Path.GetFileName(fileNewNode);
                ECNode.FileNameChild = nameNewNode;

                AddECtoNodeCLusterGPTree(ECNode, fileParent);

                //Xóa bỏ thành phần EC của nút đã bị tách tại nút cha
                DelECofNodeCLusterGPTree(Path.GetFileName(fileLeaf), fileParent);
            }

            //Cập nhật thông tin cho hai nút lá mới
            updateIsRoot(false, fileLeafLeft);
            updateIsRoot(false, fileLeafRight);

            //Định nghĩa neighbor cho 2 nút mới tách
            defineNeighborTwo(fileLeafLeft, fileLeafRight, fileLeaf);
            //Cập nhật từ nút lá tới gốc
            UpdateClusterInGPTree(fileLeafLeft, dir + getFileParent(fileLeafLeft));
            UpdateClusterInGPTree(fileLeafRight, dir + getFileParent(fileLeafRight));

            //Xoá neighbor của nút lá cũ
            //removeNeighbor(fileRoot, Path.GetFileName(fileLeaf));

            //Nếu như nút cha của nút lá bị đầy thì tiếp tục tách nút trong
            //if (getIsRoot(fileLeaf) == false)
            //    if (getNumEC(fileParent) > N)
            //        SplitNodeCLusterGPTree(theta, fileParent);

            Console.WriteLine("end split leaf");
        }

        public void AddEDfromRootByCenterGPTree(ElementData ED, string Root, double theta)
        {
            //Tìm hướng đi tốt nhất theo vector tâm
            List<double> vector = ED.Feature;
            string file = Root;
            string fileEC;
            string dir = Path.GetDirectoryName(Root) + "\\";
            ElementCenter EC = new ElementCenter();
            Stack<string> STACK = new Stack<string>();
            Stack<string> STACKneighbor = new Stack<string>();
            STACK.Push(Root);
            while (STACK.Count > 0)
            {
                file = STACK.Pop();
                if (getIsLeaf(file) == false)
                {
                    fileEC = dir + getFileEC(file);
                    EC = EC.getMinEC_GPTree(vector, fileEC);
                    if (EuclideDistance(vector, EC.Values) <= theta)
                    {
                        STACK.Push(dir + EC.FileNameChild);
                    }
                }
            }
            Console.WriteLine("Theta: " + theta);
            //Sau Stack thì sẽ tìm được file cluster tương tự nhất : có thể là Root, nút lá hoặc là nút trong
            //Nếu là Root(trường hợp cây một nút) hoặc nút lá thì thêm ED vào và cập nhật lại, ngược lại nếu là nút trong thì tạo một nút lá là con của nút trong đó
            string fileLeaf = file;
            string fileChild = dir + EC.FileNameChild;
            bool isCreateNewLeaf = false;
            if (getIsLeaf(fileLeaf) == true)
                AddEDtoLeafCLusterGPTree(ED, fileLeaf);
            else
            {
                //Tạo nút mới đồng cấp với ECMin tìm được hay nói cách khác là tạo leaf mới là con của file Node vừa tìm được
                fileLeaf = createNewLeafOnNode(ED, file);
                isCreateNewLeaf = true;
            }
            //File Leaf lúc này thì có thể là nút lá mới vừa tạo hoặc Root hay nút lá vừa tìm được
            //bây giờ thì file leaf chắc chắn là nút lá
            
            //Nếu là nút gốc thì không cần cập nhật, ngược lại thì cập nhật
            if (getIsRoot(fileLeaf) == false)
            {
                UpdateClusterInGPTree(fileLeaf, dir + getFileParent(fileLeaf));
            }
            int level = getIsRoot(fileLeaf) == true ? 0 : getClusterLevelGPTree(dir + getFileParent(fileLeaf));
            Console.WriteLine("Level: " + level);
            //Nếu số ED vượt ngưỡng thì tách nút
            if (getNumED(fileLeaf) > M)
            {
                SplitLeafCLusterGPTree(theta, fileLeaf);
            }
            //Không phải nút gốc thì mới cập nhật neighbor và tách nút trong
            if (getIsRoot(fileLeaf) == false)
            {
                //Tiến hành cập nhật neighbor nút mới tạo với các nút đã tồn tại trong cây
                if (isCreateNewLeaf)
                {
                    updateNeighborTwo(fileLeaf, Root);
                }
                Console.WriteLine("Xong cap nhat neighbor");
            }
            Console.WriteLine("Xong thêm ED vào cây");
        }

        public void AddEDfromRootByCenterGPTree2(ElementData ED, string Root, double theta)
        {
            //Tìm hướng đi tốt nhất theo vector tâm
            List<double> vector = ED.Feature;
            string file = Root;
            string fileEC;
            string dir = Path.GetDirectoryName(Root) + "\\";
            ElementCenter EC = new ElementCenter();
            Stack<string> STACK = new Stack<string>();
            Stack<string> STACKneighbor = new Stack<string>();
            STACK.Push(Root);
            while (STACK.Count > 0)
            {
                file = STACK.Pop();
                if (getIsLeaf(file) == false)
                {
                    fileEC = dir + getFileEC(file);
                    EC = EC.getMinEC_GPTree(vector, fileEC);
                    if (EuclideDistance(vector, EC.Values) <= theta)
                    {
                        STACK.Push(dir + EC.FileNameChild);
                    }
                }
            }
            Console.WriteLine("Theta: " + theta);
            //Sau Stack thì sẽ tìm được file cluster tương tự nhất : có thể là Root, nút lá hoặc là nút trong
            //Nếu là Root(trường hợp cây một nút) hoặc nút lá thì thêm ED vào và cập nhật lại, ngược lại nếu là nút trong thì tạo một nút lá là con của nút trong đó
            string fileLeaf = file;
            string fileChild = dir + EC.FileNameChild;
            bool isCreateNewLeaf = false;
            if (getIsLeaf(fileLeaf) == true)
                AddEDtoLeafCLusterGPTree(ED, fileLeaf);
            else
            {
                //Tạo nút mới đồng cấp với ECMin tìm được hay nói cách khác là tạo leaf mới là con của file Node vừa tìm được
                fileLeaf = createNewLeafOnNode(ED, file);
                isCreateNewLeaf = true;
            }
            //File Leaf lúc này thì có thể là nút lá mới vừa tạo hoặc Root hay nút lá vừa tìm được
            //bây giờ thì file leaf chắc chắn là nút lá

            //Nếu là nút gốc thì không cần cập nhật, ngược lại thì cập nhật
            if (getIsRoot(fileLeaf) == false)
            {
                UpdateClusterInGPTree(fileLeaf, dir + getFileParent(fileLeaf));
            }
            int level = getIsRoot(fileLeaf) == true ? 0 : getClusterLevelGPTree(dir + getFileParent(fileLeaf));
            Console.WriteLine("Level: " + level);
            //Nếu số ED vượt ngưỡng thì tách nút
            if (getNumED(fileLeaf) > M)
            {
                //SplitLeafCLusterGPTree2(theta, fileLeaf);

                ClusterNode cluster = new ClusterNode();
                string fileED = dir + cluster.getFileED(fileLeaf);
                if (ED.getListClassName(fileED).Count >= 2)
                    SplitLeafCLusterGPTree2(theta, fileLeaf);
                else
                    SplitLeafCLusterGPTree(theta, fileLeaf);
            }

            if (getIsRoot(fileLeaf) == false)
            {
                //Tiến hành cập nhật neighbor nút mới tạo với các nút đã tồn tại trong cây
                if (isCreateNewLeaf)
                {
                    updateNeighborTwo(fileLeaf, Root);
                }
                Console.WriteLine("Xong cap nhat neighbor");
            }
            Console.WriteLine("Xong thêm ED vào cây");
        }

        public void AddEDfromRootByCenterGPTree(ElementData ED, string Root, double theta, int maxLevel)
        {
            //Tìm hướng đi tốt nhất theo vector tâm
            List<double> vector = ED.Feature;
            string file = Root;
            string fileEC;
            string dir = Path.GetDirectoryName(Root) + "\\";
            ElementCenter EC = new ElementCenter();
            Stack<string> STACK = new Stack<string>();
            Stack<string> STACKneighbor = new Stack<string>();
            STACK.Push(Root);
            while (STACK.Count > 0)
            {
                file = STACK.Pop();
                if (getIsLeaf(file) == false)
                {
                    fileEC = dir + getFileEC(file);
                    EC = EC.getMinEC_GPTree(vector, fileEC);
                    if (EuclideDistance(vector, EC.Values) <= theta)
                    {
                        STACK.Push(dir + EC.FileNameChild);
                    }
                }
            }
            Console.WriteLine("Theta: " + theta);
            //Sau Stack thì sẽ tìm được file cluster tương tự nhất : có thể là Root, nút lá hoặc là nút trong
            //Nếu là Root(trường hợp cây một nút) hoặc nút lá thì thêm ED vào và cập nhật lại, ngược lại nếu là nút trong thì tạo một nút lá là con của nút trong đó
            string fileLeaf = file;
            string fileChild = dir + EC.FileNameChild;
            bool isCreateNewLeaf = false;
            if (getIsLeaf(fileLeaf) == true)
                AddEDtoLeafCLusterGPTree(ED, fileLeaf);
            else
            {
                //Tạo nút mới đồng cấp với ECMin tìm được hay nói cách khác là tạo leaf mới là con của file Node vừa tìm được
                fileLeaf = createNewLeafOnNode(ED, file);
                isCreateNewLeaf = true;
            }
            //File Leaf lúc này thì có thể là nút lá mới vừa tạo hoặc Root hay nút lá vừa tìm được
            //bây giờ thì file leaf chắc chắn là nút lá

            //Nếu là nút gốc thì không cần cập nhật, ngược lại thì cập nhật
            if (getIsRoot(fileLeaf) == false)
            {
                UpdateClusterInGPTree(fileLeaf, dir + getFileParent(fileLeaf));
            }
            int level = getIsRoot(fileLeaf) == true ? 0 : getClusterLevelGPTree(dir + getFileParent(fileLeaf));
            Console.WriteLine("Level: " + level);
            //Nếu số ED vượt ngưỡng thì tách nút
            if (level < maxLevel && getNumED(fileLeaf) > M)
            {
                SplitLeafCLusterGPTree(theta, fileLeaf);
            }
            
            Console.WriteLine("Xong thêm ED vào cây");
        }


        //Hàm dùng để tạo nút đồng cấp, xoá nút Root cũ và tạo nút Root mới khi Eu(vector,tâm Root) > theta
        //Trường hợp cây chỉ có nút Root nhưng khi thêm vào thì độ đo Euclide giữa vector với tâm Root lớn hơn Theta
        // => Tạo một nút mới thứ nhất để thêm vector, xoá nút Root cũ, chuyển tất cả ED trong nút Root cũ sang nút mới thứ hai
        //Tạo nút Root mới lưu tâm của 2 nút vừa tạo
        public void splitRootGPTree(string fileRoot, ElementData ED)
        {
            Console.WriteLine("bắt đầu handle tạo nút đồng cấp Root");
            string dir = Path.GetDirectoryName(fileRoot) + "\\";
            //Tạo nút lá mới để chứa tập ED cũ của nút Root
            ClusterNode firstNewLeaf = new ClusterNode(true, false, 1);
            //Tạo nút lá mới thứ 2 để lưu thành phần ed lớn hơn theta
            ClusterNode secondNewLeaf = new ClusterNode(true, false, 1);

            firstNewLeaf.fileNameParent = "Root.txt";
            secondNewLeaf.fileNameParent = "Root.txt";
            //Tạo ra file cho nút lá mới thứ nhất, thêm tập ED của nút Root cũ vào
            string fileFirstNewLeaf = dir + getNameLeafNode(dir, "Leaf");
            firstNewLeaf.SaveClusterGPTree(fileFirstNewLeaf);

            string fileRootED = dir + getFileED(fileRoot);
            List<ElementData> listED = ED.getListElementDataGPTree(fileRootED);
            foreach (ElementData ed in listED)
            {
                firstNewLeaf.AddEDtoLeafCLusterGPTree(ed, fileFirstNewLeaf);
            }

            //Tạo ra file cho nút lá thứ 2, lưu vector có độ đo lớn hơn theta vào trong
            string fileSecondNewLeaf = dir + getNameLeafNode(dir, "Leaf");
            secondNewLeaf.SaveClusterGPTree(fileSecondNewLeaf);
            secondNewLeaf.AddEDtoLeafCLusterGPTree(ED, fileSecondNewLeaf);

            //Cập nhật danh sách tên tập tin chứa các Cluster con.
            updateListFileChild(null, fileFirstNewLeaf);
            updateListFileChild(null, fileSecondNewLeaf);

            //Tạo thành phần ECFirstNewLeaf để kết nối vào nút cha
            ElementCenter ECFirstNewLeaf = new ElementCenter(true);

            ECFirstNewLeaf.Index = 0;
            ECFirstNewLeaf.Values = getCenterED(fileFirstNewLeaf);
            ECFirstNewLeaf.IsNextLeaf = true;
            ECFirstNewLeaf.Filename = "Root.txt";
            ECFirstNewLeaf.FileNameChild = Path.GetFileName(fileFirstNewLeaf);
            ECFirstNewLeaf.MaxClass = ED.getClassMax(dir + getFileED(fileFirstNewLeaf));

            //Tạo thành phần ECSecondNewLeaf để kết nối vào nút cha
            ElementCenter ECSecondNewLeaf = new ElementCenter(true);

            ECSecondNewLeaf.Index = 0;
            ECSecondNewLeaf.Values = getCenterED(fileSecondNewLeaf);
            ECSecondNewLeaf.IsNextLeaf = true;
            ECSecondNewLeaf.Filename = "Root.txt";
            ECSecondNewLeaf.FileNameChild = Path.GetFileName(fileSecondNewLeaf);
            ECSecondNewLeaf.MaxClass = ED.getClassMax(dir + getFileED(fileSecondNewLeaf));

            //Tạo nút Root mới, xoá nút Root cũ
            //Thêm 2 EC mới vào nút cha
            ClusterNode Root = new ClusterNode(false, true, 0);
            Root.SaveClusterGPTree(fileRoot);
            AddECtoNodeCLusterGPTree(ECFirstNewLeaf, fileRoot);
            AddECtoNodeCLusterGPTree(ECSecondNewLeaf, fileRoot);

            Console.WriteLine("Xong xử lý tạo nút đồng cấp cho root");
        }


        //Tạo cây, có kiểm tra tâm Root, nhỏ hơn thêm vào, lớn hơn tạo nút đồng cấp
        public void AddEDfromRootByCenterGPTreeSecond(ElementData ED, string Root, double theta)
        {
            //Tìm hướng đi tốt nhất theo vector tâm
            List<double> vector = ED.Feature;
            string file = Root;
            string fileEC;
            string dir = Path.GetDirectoryName(Root) + "\\";
            ElementCenter EC = new ElementCenter();
            Stack<string> STACK = new Stack<string>();
            Stack<string> STACKneighbor = new Stack<string>();
            STACK.Push(Root);
            while (STACK.Count > 0)
            {
                file = STACK.Pop();
                //Trường hợp file là nút gốc và nút gốc này cũng là lá ( cây 1 nút)
                if (getIsLeaf(file) == true && getIsRoot(file) == true)
                {
                    //Nếu nút gốc chưa có ed nào thì thêm vô không cần kiểm tra điều kiện
                    if (getNumED(Root) == 0)
                    {
                        AddEDtoLeafCLusterGPTree(ED, file);
                    }
                    string fileED = dir + getFileED(file);
                    List<double> centerED = ED.getCenterED_GPTree(fileED);
                    //Kiểm tra vector đầu vào với tâm Root
                    // Nếu lớn hơn theta thì tạo nút đồng cấp
                    if (EuclideDistance(vector, centerED) <= theta)
                    {
                        AddEDtoLeafCLusterGPTree(ED, file);
                        //Nếu số ED vượt ngưỡng thì tách nút
                        if (getNumED(file) > M)
                        {
                            SplitLeafCLusterGPTree(theta, file);
                        }
                        return;
                    }
                    else
                    {
                        //Trường hợp cây chỉ có nút Root nhưng khi thêm vào thì độ đo Euclide giữa vector với tâm Root lớn hơn Theta
                        // => Tạo một nút mới thứ nhất để thêm vector, xoá nút Root cũ, chuyển tất cả ED trong nút Root cũ sang nút mới thứ hai
                        //Tạo nút Root mới lưu tâm của 2 nút vừa tạo
                        splitRootGPTree(file, ED);
                        return;
                    }
                }
                else
                {
                    //Trường hợp sau khi nút gốc không còn là nút lá nữa, tiến hành duyệt ừu trên xuống
                    if (getIsLeaf(file) == false)
                    {
                        fileEC = dir + getFileEC(file);
                        EC = EC.getMinEC_GPTree(vector, fileEC);
                        if (EuclideDistance(vector, EC.Values) <= theta)
                        {
                            STACK.Push(dir + EC.FileNameChild);
                        }
                    }
                }
            }
            Console.WriteLine("Theta: " + theta);
            //Sau Stack thì sẽ tìm được file cluster tương tự nhất : có thể là nút lá hoặc là nút trong
            //Nếu là nút lá thì thêm ED vào và cập nhật lại, ngược lại nếu là nút trong thì tạo một nút lá là con của nút trong đó
            string fileLeaf = file;
            if (getIsLeaf(fileLeaf) == true)
                AddEDtoLeafCLusterGPTree(ED, fileLeaf);
            else
            {
                //Nếu không phải nút lá
                //Tạo nút mới đồng cấp với ECMin tìm được hay nói cách khác là tạo leaf mới là con của file Node vừa tìm được
                fileLeaf = createNewLeafOnNode(ED, file);
            }
            //File Leaf lúc này thì có thể là nút lá mới vừa tạo hoặc nút lá vừa tìm được
            //bây giờ thì file leaf chắc chắn là nút lá
            UpdateClusterInGPTree(fileLeaf, dir + getFileParent(fileLeaf));
            //Nếu số ED vượt ngưỡng thì tách nút
            if (getNumED(fileLeaf) > M)
            {
                SplitLeafCLusterGPTree(theta, fileLeaf);
            }

            Console.WriteLine("Xong thêm ED vào cây");
        }

    }
}
