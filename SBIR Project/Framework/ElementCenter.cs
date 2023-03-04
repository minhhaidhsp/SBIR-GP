using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBIR
{
    class ElementCenter:ClusterMethod
    {
        //Các thuộc tính trực tiếp
        private int index = 0; //Vị trí của Element Center trên file
        private List<double> values = new List<double>();//Vector đại diện cho một cụm (thuộc tính khóa)
        private bool isNextLeaf = true; //Kiểm tra cụm kế tiếp có phải là nút lá hay không
        private string fileName = string.Empty;//tên tập tin chứa phần tử Element Center 

        //Các thuộc tính liên kết
        private string fileNameChild = string.Empty;//Tên của nập tin kế tiếp chứa cụm con (thuộc tính khóa)
        private string maxClass = string.Empty; //Tên class(label) xuất hiện nhiều nhất- class đại diện của cluster con

        //Phương thức khởi tạo không đối số
        public ElementCenter()
        {
            this.index = 0;
            this.values = new List<double>();//Vector đại diện cho một cụm
            this.isNextLeaf = true; //Kiểm tra cụm kế tiếp có phải là nút lá hay không
            this.fileName = string.Empty;//tên tập tin chứa phần tử ElementData Center
            this.fileNameChild = string.Empty;//Tên của nập tin kế tiếp chứa cụm con
            this.maxClass = string.Empty;
        }
        //Phương thưc khởi tạo theo IsNextLeaf
        public ElementCenter(bool isNextLeaf)
        {
            this.index = 0;
            this.values = new List<double>();//Vector đại diện cho một cụm
            this.isNextLeaf = isNextLeaf; //Kiểm tra cụm kế tiếp có phải là nút lá hay không
            this.fileName = string.Empty;//tên tập tin chứa phần tử ElementData Center
            this.fileNameChild = string.Empty;//Tên của nập tin kế tiếp chứa cụm con
            this.maxClass = string.Empty;
        }
        //Phương thức khởi tạo có đối sô tương ứng với từng thuộc tính
        public ElementCenter(int index, List<double> values, bool isNextLeaf, string filename, string fileChild, string maxClass)
        {
            this.index = index;
            this.values = values;//Vector đại diện cho một cụm
            this.isNextLeaf = isNextLeaf; //Kiểm tra cụm kế tiếp có phải là nút lá hay không
            this.fileName = filename;//tên tập tin chứa phần tử ElementData Center
            this.fileNameChild = fileChild;//Tên của nập tin kế tiếp chứa cụm con
            this.maxClass = maxClass;
        }
        //Phương thức khởi tạo đối tượng ElementCenter từ một mảng chuỗi chứa thông tin đối tượng
        public ElementCenter(string[] strElementCenter)
        {
            this.index = ToInt32(strElementCenter[0]);
            this.values = String2Vector(strElementCenter[1]);//Vector đại diện cho một cụm
            this.isNextLeaf = bool.Parse(strElementCenter[2]); //Kiểm tra cụm kế tiếp có phải là nút lá hay không
            this.fileName = strElementCenter[3];//tên tập tin chứa phần tử ElementData Center
            this.fileNameChild = strElementCenter[4];//Tên của nập tin kế tiếp chứa cụm con
            this.maxClass = strElementCenter[5];
        }
        //Phuong thức khởi tạo sao chép từ một đối tượng EC
        public ElementCenter(ElementCenter EC)
        {
            this.index = EC.Index;
            this.values = EC.Values;//Vector đại diện cho một cụm
            this.isNextLeaf = EC.IsNextLeaf; //Kiểm tra cụm kế tiếp có phải là nút lá hay không
            this.fileName = EC.Filename;//tên tập tin chứa phần tử ElementData Center
            this.fileNameChild = EC.FileNameChild;//Tên của nập tin kế tiếp chứa cụm con
            this.maxClass = EC.MaxClass;
        }
        //Các phương thức dang thuộc tính (Property) truy cập get/set method 
        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        public List<double> Values
        {
            get { return values; }
            set { values = value; }
        }
        public bool IsNextLeaf
        {
            get { return isNextLeaf; }
            set { isNextLeaf = value; }
        }
        public string Filename
        {
            get { return fileName; }
            set { fileName = value; }
        }
        public string FileNameChild
        {
            get { return fileNameChild; }
            set { fileNameChild = value; }
        }
        public string MaxClass
        {
            get { return maxClass; }
            set { maxClass = value; }
        }
        //Chuyển một vector values của đối tượng ElementCenter thành một chuỗi (mỗi giá trị cách nhau bằng một khoảng trắng)
        public string Values2String()
        {
            string strFeature = string.Empty;
            if (this.values == null) return string.Empty;
            if (this.values.Count == 0) return string.Empty;
            foreach (double num in this.values)
                strFeature += num.ToString() + " ";
            return strFeature.Trim();
        }
        //Kiếm tra phần tử ElementData chứa trong cụm của một tập tin chứa các đối tượng EC
        public bool CheckElementCenter(string fileEC)
        {
            string lineElementCenter = string.Empty;
            TextfileCluster tfc = new TextfileCluster(fileEC);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return false;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return false;
            string f = Vector2String(this.values);
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                if (words.Length < 5) return false;
                if ((f == words[1]) && (this.fileNameChild == words[4]))
                    return true;
            }
            return false;
        }
        //Kiếm tra phần tử ElementData chứa trong cụm của một tập tin chứa các đối tượng EC tương ứng với values và fileChild
        public bool CheckElementCenter(string values, string fileNameChild, string fileEC)
        {
            string lineElementData = string.Empty;
            TextfileCluster tfc = new TextfileCluster(fileEC);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return false;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return false;
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                if (words.Length < 5) return false;
                if ((values == words[1]) && (fileNameChild == words[4]))
                    return true;
            }
            return false;
        }
        //Kiếm tra phần tử ElementData chứa trong cụm của một tập tin chứa các đối tượng EC tương ứng với values và fileChild
        public bool CheckElementCenter(List<double> values, string fileNameChild, string filename)
        {
            string lineElementData = string.Empty;
            TextfileCluster tfc = new TextfileCluster(filename);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return false;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return false;
            string f = Vector2String(values);
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                if (words.Length < 5) return false;
                if ((f == words[1]) && (fileNameChild == words[4]))
                    return true;
            }
            return false;
        }
        //Thêm một ElementCenter vào sau của tập tin văn bản chứa các ElementCenter
        //Nếu một ElementCenter đã tồn tại trong fileEC thì không thêm
        public void SaveElementCenter(string fileEC)
        {
            string lineElementCenter = string.Empty;
            TextfileCluster tfc = new TextfileCluster(fileEC);
            if (CheckElementCenter(this.values, this.fileNameChild, fileEC) == true) return;
            this.index = tfc.NumberLines + 1;
            lineElementCenter += this.index.ToString() + " ";
            string tmp = this.Values2String().Trim();
            if (tmp == string.Empty)
                lineElementCenter += "(" + "#NONE#" + ")" + " ";
            else
                lineElementCenter += "(" + this.Values2String().Trim() + ")" + " ";
            lineElementCenter += "(" + this.isNextLeaf.ToString().Trim() + ")" + " ";
            if (this.fileName.Trim() == string.Empty)
                lineElementCenter += "(" + "#NONE#" + ")" + " ";
            else
                lineElementCenter += "(" + this.fileName.Trim() + ")" + " ";
            if (this.fileNameChild.Trim() == string.Empty)
                lineElementCenter += "(" + "#NONE#" + ")" + " ";
            else
                lineElementCenter += "(" + this.fileNameChild.Trim() + ")" + " ";
            tfc.WriteLineTextFile(lineElementCenter, fileEC);
        }
        //Lấy (tìm kiếm) một ElementCenter từ file tương ứng với fileNameChild
        public string[] getStrElementCenter(string fileNameChild, string fileEC)
        {
            TextfileCluster tfc = new TextfileCluster(fileEC);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return null;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return null;
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                if (words.Length < 5) return null;
                if (words[4] == fileNameChild.Trim())
                    return words;
            }
            return null;
        }
        //Lấy (tìm kiếm) một ElementCenter từ file tương ứng với fileNameChild
        public string getElementCenter2String(string fileNameChild, string fileEC)
        {
            string[] StrArr = getStrElementCenter(fileNameChild, fileEC);
            return StrArr2String(StrArr);
        }
        //Lấy (tìm kiếm) một ElementCenter từ file tương ứng với values và fileNameChild
        public string[] getStrElementCenter(List<double> values, string fileNameChild, string fileEC)
        {
            TextfileCluster tfc = new TextfileCluster(fileEC);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return null;
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return null;
            string f = Vector2String(values).Trim();
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                if (words.Length < 5) return null;
                if (words[1] == f && words[4] == fileNameChild)
                    return words;
            }
            return null;
        }
        //Lấy (tìm kiếm) một ElementCenter từ file tương ứng với values và fileNameChild
        public string getElementCenter2String(List<double> values, string fileNameChild, string fileEC)
        {
            string[] StrArr = getStrElementCenter(values, fileNameChild, fileEC);
            return StrArr2String(StrArr);
        }
        //Lấy (tìm kiếm) một ElementCenter từ file tương ứng với values
        public string[] getStrElementCenter(List<double> values, string fileEC)
        {
            TextfileCluster tfc = new TextfileCluster(fileEC);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return null;
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return null;
            string f = Vector2String(values).Trim();
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                if (words.Length < 2) return null;
                if (words[1] == f)
                    return words;
            }
            return null;
        }
        //Lấy (tìm kiếm) một ElementCenter từ file tương ứng với mảng chuỗi chứa thông tin các phần tử
        public ElementCenter getElementCenter(string[] strElementCenter)
        {
            ElementCenter EC = new ElementCenter();
            EC.Index = ToInt32(strElementCenter[0]);
            EC.Values = String2Vector(strElementCenter[1]);
            EC.IsNextLeaf = bool.Parse(strElementCenter[2]);
            EC.Filename = strElementCenter[3];
            EC.fileNameChild = strElementCenter[4];
            return EC;
        }
        //Lấy một danh sách các phần tử Element Center từ một file
        public List<ElementCenter> getListElementCenter(string fileEC)
        {
            List<ElementCenter> listEC = new List<ElementCenter>();
            TextfileCluster tfc = new TextfileCluster(fileEC);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return null;
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                if (words.Length > 0)
                {
                    ElementCenter EC = getElementCenter(words);
                    listEC.Add(EC);
                }
            }
            return listEC;
        }
        //Lấy một phần tử ElementCenter từ một file tương ứng với values và fileNameChild
        public string[] getStrElementCenter(string values, string fileNameChild, string fileED)
        {
            TextfileCluster tfc = new TextfileCluster(fileED);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return null;
            string f = values.Trim();
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return null;
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                if (words.Length < 5) return null;
                if (words[1] == f && words[4] == fileNameChild)
                    return words;
            }
            return null;
        }
        //Xóa một Element Center trong file tương ứng với fileNameChild
        public void DelElementCenter(string fileNameChild, string fileEC)
        {
            TextfileCluster tfc = new TextfileCluster(fileEC);
            tfc.DeleteFirstLine(fileNameChild);
            tfc.UpdateIndex();
        }
        //Xóa tất cả ElementCenter trong file tương ứng với fileNameChild
        public void DelAllElementCenter(string fileNameChild, string fileEC)
        {
            TextfileCluster tfc = new TextfileCluster(fileEC);
            tfc.DeleteLine(fileNameChild);
            tfc.UpdateIndex();
        }
        //Xóa một ElementCenter trong file tương ứng với values
        public void DelElementCenter(List<double> values, string fileEC)
        {
            TextfileCluster tfc = new TextfileCluster(fileEC);
            string f = Vector2String(values);
            tfc.DeleteFirstLine(f);
            tfc.UpdateIndex();
        }
        //Xóa tất cả ElementCenter trong file tương ứng với values
        public void DelAllElementCenter(List<double> values, string fileEC)
        {
            TextfileCluster tfc = new TextfileCluster(fileEC);
            string f = Vector2String(values);
            tfc.DeleteLine(f);
            tfc.UpdateIndex();
        }
        //Xóa một Element Center trong file tương ứng với fileNameChild và values
        public void DelElementCenter(string fileNameChild, List<double> values, string fileEC)
        {
            TextfileCluster tfc = new TextfileCluster(fileEC);
            string f = Vector2String(values);
            tfc.DeleteFirst2String(fileNameChild, f);
            tfc.UpdateIndex();
        }
        //Xóa tất cả Element Center trong file tương ứng với fileNameChild và values
        public void DelAllElementCenter(string fileNameChild, List<double> values, string fileED)
        {
            TextfileCluster tfc = new TextfileCluster(fileED);
            string f = Vector2String(values);
            tfc.DeleteAll2String(fileNameChild, f);
            tfc.UpdateIndex();
        }
        //Cập nhật giá trị values của một phần tử EC trong một file
        public void updateValuesEC(List<double> values, string fileNameChild, string fileEC)
        {
            string[] strEC = getStrElementCenter(fileNameChild, fileEC);
            ElementCenter E = getElementCenter(strEC);
            E.Values = values;
            DelElementCenter(fileNameChild, fileEC);
            E.SaveElementCenter(fileEC);
        }
        //Lấy giá trị vetor tâm của các phần tử Element Center từ một tập tin chứa danh sách các EC
        public List<double> getCenterEC(string fileEC)
        {
            List<ElementCenter> listEC = getListElementCenter(fileEC);
            if (listEC == null) return null;
            int NumOfList = listEC.Count;
            if (NumOfList == 0) return null;
            List<double> CenterEC = new List<double>();
            int m = NumOfList;
            int n = listEC.ElementAt(0).Values.Count;
            for (int j = 0; j < n; j++)
            {
                double ave = 0.0;
                for (int i = 0; i < m; i++)
                    ave = ave + listEC.ElementAt(i).Values.ElementAt(j);
                ave = ave / (double)m;
                CenterEC.Add(ave);
            }
            return CenterEC;
        }
        //Lấy giá trị vetor tâm của các phần tử Weight Center từ một tập tin chứa danh sách các EC
        public List<double> getWeightEC(string fileEC)
        {
            List<ElementCenter> listEC = getListElementCenter(fileEC);
            if (listEC == null) return null;
            int NumOfList = listEC.Count;
            if (NumOfList == 0) return null;
            List<double> WeightEC = new List<double>();
            int m = NumOfList;
            ClusterNode Cluster = new ClusterNode();
            string dir = Path.GetDirectoryName(fileEC);

            int n;
            if(Cluster.getIsLeaf(dir + "\\" + listEC.ElementAt(0).fileNameChild)==true)
                n= Cluster.getWeightED(dir + "\\" + listEC.ElementAt(0).fileNameChild).Count;
            else
                n = Cluster.getWeightEC(dir + "\\" + listEC.ElementAt(0).fileNameChild).Count;
            
            for (int j = 0; j < n; j++)
            {
                double ave = 0.0;
                for (int i = 0; i < m; i++)
                {
                    if (Cluster.getIsLeaf(dir + "\\" + listEC.ElementAt(i).fileNameChild) == true)
                        ave = ave + Cluster.getWeightED(dir + "\\" + listEC.ElementAt(i).fileNameChild).ElementAt(j);
                    else
                        ave = ave + Cluster.getWeightEC(dir + "\\" + listEC.ElementAt(i).fileNameChild).ElementAt(j);
                }
                ave = ave / (double)m;
                WeightEC.Add(ave);
            }
            return WeightEC;
        }

        //Tìm một phần tử xa nhất với một phần tử ElementCenter cho trước theo khoảng cách Euclide
        public ElementCenter getFarestEC(ElementCenter EC, string fileEC)
        {
            ElementCenter ECmax = EC;
            List<ElementCenter> listEC = getListElementCenter(fileEC);
            if (listEC == null) return ECmax;
            foreach (ElementCenter ec in listEC)
            {
                if (EuclideDistance(ECmax.values, EC.values) < EuclideDistance(ec.values, EC.values))
                    ECmax = ec;
            }
            return ECmax;
        }
        //Tìm một phần tử xa nhất so với tâm
        public ElementCenter getFarestECcenter(string fileEC)
        {
            List<double> CenterEC = getCenterEC(fileEC);
            ElementCenter ECmax = new ElementCenter();
            List<ElementCenter> listEC = getListElementCenter(fileEC);
            if (listEC.Count == 0) return ECmax;
            ECmax = listEC.ElementAt(0);
            foreach (ElementCenter ec in listEC)
            {
                if (EuclideDistance(ECmax.values, CenterEC) < EuclideDistance(ec.values, CenterEC))
                    ECmax = ec;
            }
            return ECmax;
        }

        //tìm phần tử gần nhất ứng với một vector
        public ElementCenter getMinEC(List<double> vector, string fileEC)
        {
            ElementCenter ECmin;
            List<ElementCenter> listEC = getListElementCenter(fileEC);
            if (listEC == null) return null;
            ECmin = listEC.ElementAt(0);
            foreach (ElementCenter ec in listEC)
            {
                if (EuclideDistance(vector, ECmin.values) > EuclideDistance(vector, ec.values))
                    ECmin = ec;
            }
            return ECmin;
        }

        public ElementCenter getMinECSecond(List<double> vector, string fileEC, string fileChild)
        {
            ElementCenter ECmin;
            List<ElementCenter> listEC = getListElementCenter(fileEC);
            if (listEC == null) return null;
            if (listEC.ElementAt(0).FileNameChild != fileChild)
                ECmin = listEC.ElementAt(0);
            else
                ECmin = listEC.ElementAt(1);
            foreach (ElementCenter ec in listEC)
            {
                if ((EuclideDistance(vector, ECmin.values) > EuclideDistance(vector, ec.values))&&(ec.FileNameChild !=fileChild))
                    ECmin = ec;
            }
            return ECmin;
        }









        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////     Các phương thức để xử lý cây H-Tree                                                         ///////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Lấy giá trị vetor tâm của các phần tử Element Center từ một tập tin chứa danh sách các EC
        public List<double> getCenterECHTree(string fileEC)
        {
            List<ElementCenter> listEC = getListElementCenterHTree(fileEC);
            if (listEC == null) return null;
            int NumOfList = listEC.Count;
            if (NumOfList == 0) return null;
            List<double> CenterEC = new List<double>();
            int m = NumOfList;
            int n = listEC.ElementAt(0).Values.Count;
            for (int j = 0; j < n; j++)
            {
                double ave = 0.0;
                for (int i = 0; i < m; i++)
                    ave = ave + listEC.ElementAt(i).Values.ElementAt(j);
                ave = ave / (double)m;
                CenterEC.Add(ave);
            }
            return CenterEC;
        }

        //Lấy một danh sách các phần tử Element Center từ một file
        public List<ElementCenter> getListElementCenterHTree(string fileEC)
        {
            List<ElementCenter> listEC = new List<ElementCenter>();
            TextfileCluster tfc = new TextfileCluster(fileEC);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return null;
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                if (words.Length > 0)
                {
                    ElementCenter EC = getElementCenterHTree(words);
                    listEC.Add(EC);
                }
            }
            return listEC;
        }

        //Lấy (tìm kiếm) một ElementCenter từ file tương ứng với mảng chuỗi chứa thông tin các phần tử
        public ElementCenter getElementCenterHTree(string[] strElementCenter)
        {
            ElementCenter EC = new ElementCenter();
            EC.Values = String2Vector(strElementCenter[0]);
            EC.fileNameChild = strElementCenter[1];
            return EC;
        }

        ////Lấy giá trị vetor tâm của các phần tử Weight Center từ một tập tin chứa danh sách các EC
        //public List<double> getWeightECHTree(string fileEC)
        //{
        //    List<ElementCenter> listEC = getListElementCenter(fileEC);
        //    if (listEC == null) return null;
        //    int NumOfList = listEC.Count;
        //    if (NumOfList == 0) return null;
        //    List<double> WeightEC = new List<double>();
        //    int m = NumOfList;
        //    ClusterNode Cluster = new ClusterNode();
        //    string dir = Path.GetDirectoryName(fileEC);

        //    int n;
        //    if (Cluster.getIsLeaf(dir + "\\" + listEC.ElementAt(0).fileNameChild) == true)
        //        n = Cluster.getWeightED(dir + "\\" + listEC.ElementAt(0).fileNameChild).Count;
        //    else
        //        n = Cluster.getWeightEC(dir + "\\" + listEC.ElementAt(0).fileNameChild).Count;

        //    for (int j = 0; j < n; j++)
        //    {
        //        double ave = 0.0;
        //        for (int i = 0; i < m; i++)
        //        {
        //            if (Cluster.getIsLeaf(dir + "\\" + listEC.ElementAt(i).fileNameChild) == true)
        //                ave = ave + Cluster.getWeightED(dir + "\\" + listEC.ElementAt(i).fileNameChild).ElementAt(j);
        //            else
        //                ave = ave + Cluster.getWeightEC(dir + "\\" + listEC.ElementAt(i).fileNameChild).ElementAt(j);
        //        }
        //        ave = ave / (double)m;
        //        WeightEC.Add(ave);
        //    }
        //    return WeightEC;
        //}


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////     Các phương thức để xử lý cây GP-Tree                                                         ///////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        
        //Lấy một danh sách các phần tử Element Center từ một file
        public List<ElementCenter> getListElementCenterGPTree(string fileEC)
        {
            List<ElementCenter> listEC = new List<ElementCenter>();
            TextfileCluster tfc = new TextfileCluster(fileEC);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return null;
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                if (words.Length > 0)
                {
                    ElementCenter EC = getElementCenterGPTree(words);
                    listEC.Add(EC);
                }
            }
            return listEC;
        }

        //Lấy (tìm kiếm) một ElementCenter từ file tương ứng với mảng chuỗi chứa thông tin các phần tử
        public ElementCenter getElementCenterGPTree(string[] strElementCenter)
        {
            ElementCenter EC = new ElementCenter();
            EC.Index = ToInt32(strElementCenter[0]);
            EC.Values = String2Vector(strElementCenter[1]);
            EC.IsNextLeaf = bool.Parse(strElementCenter[2]);
            EC.Filename = strElementCenter[3];
            EC.fileNameChild = strElementCenter[4];
            //EC.MaxClass = strElementCenter[5] == "#NONE#" ? "" : strElementCenter[5];
            EC.MaxClass = strElementCenter[5];
            return EC;
        }

        //tìm phần tử gần nhất ứng với một vector
        public ElementCenter getMinEC_GPTree(List<double> vector, string fileEC)
        {
            ElementCenter ECmin;
            List<ElementCenter> listEC = getListElementCenterGPTree(fileEC);
            if (listEC == null) return null;
            ECmin = listEC.ElementAt(0);
            foreach (ElementCenter ec in listEC)
            {
                if (EuclideDistance(vector, ECmin.values) > EuclideDistance(vector, ec.values))
                    ECmin = ec;
            }
            return ECmin;
        }

        //Cập nhật giá trị values của một phần tử EC trong một file
        public void updateValuesEC_GPTree(List<double> values, string fileNameChild, string fileEC, string maxClass)
        {
            string[] strEC = getStrElementCenterGPTree(fileNameChild, fileEC);
            ElementCenter E = getElementCenterGPTree(strEC);
            E.Values = values;
            E.MaxClass = maxClass;
            DelElementCenterGPTree(fileNameChild, fileEC);
            E.SaveElementCenterGPTree(fileEC);
        }

        //Cập nhật giá trị values của một phần tử EC trong một file
        public void updateValuesEC_GPTree(List<double> values, string fileNameChild, string fileEC)
        {
            string[] strEC = getStrElementCenterGPTree(fileNameChild, fileEC);
            ElementCenter E = getElementCenterGPTree(strEC);
            E.Values = values;
            DelElementCenterGPTree(fileNameChild, fileEC);
            E.SaveElementCenterGPTree(fileEC);
        }

        //Lấy (tìm kiếm) một ElementCenter từ file tương ứng với fileNameChild
        public string[] getStrElementCenterGPTree(string fileNameChild, string fileEC)
        {
            TextfileCluster tfc = new TextfileCluster(fileEC);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return null;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return null;
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                if (words.Length < 5) return null;
                if (words[4] == fileNameChild.Trim())
                    return words;
            }
            return null;
        }

        //Xóa một Element Center trong file tương ứng với fileNameChild
        public void DelElementCenterGPTree(string fileNameChild, string fileEC)
        {
            TextfileCluster tfc = new TextfileCluster(fileEC);
            tfc.DeleteFirstLine(fileNameChild);
            tfc.UpdateIndex();
        }

        //Thêm một ElementCenter vào sau của tập tin văn bản chứa các ElementCenter
        //Nếu một ElementCenter đã tồn tại trong fileEC thì không thêm
        public void SaveElementCenterGPTree(string fileEC)
        {
            Console.WriteLine(this.Filename);
            Console.WriteLine(this.maxClass);
            Console.WriteLine(this.fileNameChild);
            string lineElementCenter = string.Empty;
            TextfileCluster tfc = new TextfileCluster(fileEC);
            if (CheckElementCenter(this.values, this.fileNameChild, fileEC) == true) return;
            this.index = tfc.NumberLines + 1;
            lineElementCenter += this.index.ToString() + " ";
            string tmp = this.Values2String().Trim();
            if (tmp == string.Empty)
                lineElementCenter += "(" + "#NONE#" + ")" + " ";
            else
                lineElementCenter += "(" + this.Values2String().Trim() + ")" + " ";
            lineElementCenter += "(" + this.isNextLeaf.ToString().Trim() + ")" + " ";
            if (this.fileName.Trim() == string.Empty)
                lineElementCenter += "(" + "#NONE#" + ")" + " ";
            else
                lineElementCenter += "(" + this.fileName.Trim() + ")" + " ";
            if (this.fileNameChild.Trim() == string.Empty)
                lineElementCenter += "(" + "#NONE#" + ")" + " ";
            else
                lineElementCenter += "(" + this.fileNameChild.Trim() + ")" + " ";
            if(this.maxClass.Trim() == string.Empty)
                lineElementCenter += "(" + "#NONE#" + ")" + " ";
            else
                lineElementCenter += "(" + this.maxClass.Trim() + ")" + " ";
            tfc.WriteLineTextFile(lineElementCenter, fileEC);
        }

        //Lấy giá trị vetor tâm của các phần tử Weight Center từ một tập tin chứa danh sách các EC
        public List<double> getWeightEC_GPTree(string fileEC)
        {
            List<ElementCenter> listEC = getListElementCenterGPTree(fileEC);
            if (listEC == null) return null;
            int NumOfList = listEC.Count;
            if (NumOfList == 0) return null;
            List<double> WeightEC = new List<double>();
            int m = NumOfList;
            ClusterNode Cluster = new ClusterNode();
            string dir = Path.GetDirectoryName(fileEC);

            int n;
            if (Cluster.getIsLeaf(dir + "\\" + listEC.ElementAt(0).fileNameChild) == true)
                n = Cluster.getWeightED(dir + "\\" + listEC.ElementAt(0).fileNameChild).Count;
            else
                n = Cluster.getWeightEC(dir + "\\" + listEC.ElementAt(0).fileNameChild).Count;

            for (int j = 0; j < n; j++)
            {
                double ave = 0.0;
                for (int i = 0; i < m; i++)
                {
                    if (Cluster.getIsLeaf(dir + "\\" + listEC.ElementAt(i).fileNameChild) == true)
                        ave = ave + Cluster.getWeightED(dir + "\\" + listEC.ElementAt(i).fileNameChild).ElementAt(j);
                    else
                        ave = ave + Cluster.getWeightEC(dir + "\\" + listEC.ElementAt(i).fileNameChild).ElementAt(j);
                }
                ave = ave / (double)m;
                WeightEC.Add(ave);
            }
            return WeightEC;
        }

        //Lấy giá trị vetor tâm của các phần tử Element Center từ một tập tin chứa danh sách các EC
        public List<double> getCenterEC_GPTree(string fileEC)
        {
            List<ElementCenter> listEC = getListElementCenterGPTree(fileEC);
            if (listEC == null) return null;
            int NumOfList = listEC.Count;
            if (NumOfList == 0) return null;
            List<double> CenterEC = new List<double>();
            int m = NumOfList;
            int n = listEC.ElementAt(0).Values.Count;
            for (int j = 0; j < n; j++)
            {
                double ave = 0.0;
                for (int i = 0; i < m; i++)
                    ave = ave + listEC.ElementAt(i).Values.ElementAt(j);
                ave = ave / (double)m;
                CenterEC.Add(ave);
            }
            return CenterEC;
        }

        public ElementCenter getMinECSecondGPTree(List<double> vector, string fileEC, string fileChild)
        {
            ElementCenter ECmin;
            List<ElementCenter> listEC = getListElementCenterGPTree(fileEC);
            if (listEC == null) return null;
            if (listEC.ElementAt(0).FileNameChild != fileChild)
                ECmin = listEC.ElementAt(0);
            else
                ECmin = listEC.ElementAt(1);
            foreach (ElementCenter ec in listEC)
            {
                if ((EuclideDistance(vector, ECmin.values) > EuclideDistance(vector, ec.values)) && (ec.FileNameChild != fileChild))
                    ECmin = ec;
            }
            return ECmin;
        }

        //Tìm một phần tử xa nhất so với tâm
        public ElementCenter getFarestECcenterGPTree(string fileEC)
        {
            List<double> CenterEC = getCenterEC_GPTree(fileEC);
            ElementCenter ECmax = new ElementCenter();
            List<ElementCenter> listEC = getListElementCenterGPTree(fileEC);
            if (listEC.Count == 0) return ECmax;
            ECmax = listEC.ElementAt(0);
            foreach (ElementCenter ec in listEC)
            {
                if (EuclideDistance(ECmax.values, CenterEC) < EuclideDistance(ec.values, CenterEC))
                    ECmax = ec;
            }
            return ECmax;
        }

        //Tìm một phần tử xa nhất với một phần tử ElementCenter cho trước theo khoảng cách Euclide
        public ElementCenter getFarestEC_GPTree(ElementCenter EC, string fileEC)
        {
            ElementCenter ECmax = EC;
            List<ElementCenter> listEC = getListElementCenterGPTree(fileEC);
            if (listEC == null) return ECmax;
            foreach (ElementCenter ec in listEC)
            {
                if (EuclideDistance(ECmax.values, EC.values) < EuclideDistance(ec.values, EC.values))
                    ECmax = ec;
            }
            return ECmax;
        }
    }
}
