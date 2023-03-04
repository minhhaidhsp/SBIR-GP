using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBIR
{
    class ElementData:ClusterMethod
    {
        //Các thuộc tính trực tiếp
        private int index = 0;  //vị trí của ElementData trên file
        private List<double> feature = new List<double>(); //vector đặc trưng của một phần tử (thuộc tính khóa)
        private string imageID = string.Empty; //Định danh của hình ảnh (thuộc tính khóa)
        private string uri = string.Empty; //Địa chỉ URI của hình ảnh (thuộc tính khóa)
        private string fileNameDescription = string.Empty; //tên tập tin mô tả về thông tin ảnh
        private string fileName = string.Empty;//Tên tập tin chứa phần tử EkementData
        List<string> listClass = new List<string>(); //Các lớp mà một ElementData thuộc về
        //Các thuộc tính liên kết
        //<NULL>

        //Phương thức khởi tạo cho đối tương ElementData không đối số
        public ElementData()
        {
            this.index = 0;  //vị trí của ElementData trên file
            this.feature = new List<double>(); //vector đặc trưng của một phần tử
            this.imageID = string.Empty; //Định danh của hình ảnh
            this.uri = string.Empty; //Địa chỉ URL của hình ảnh
            this.fileNameDescription = string.Empty; //tên tập tin mô tả về ảnh (là một thuộc tính trực tiếp của ElementData)
            this.fileName = string.Empty;//Tên tập tin chứa phần tử Ekement (là một thuộc tính trực tiếp của ElementData)
            this.listClass = new List<string>(); //Các lớp mà một ElementData thuộc về
        }
        //Phương thức khởi tạo từ một mảng chuỗi chứa thông tin đối tượng Element Data
        public ElementData(string[] strElementData)
        {
            this.index = ToInt32(strElementData[0]);  //vị trí của ElementData trên file
            this.feature = String2Vector(strElementData[1]); //vector đặc trưng của một phần tử (thuộc tính khóa)
            this.imageID = strElementData[2]; //Định danh của hình ảnh (thuộc tính khóa)
            this.uri = strElementData[3]; //Địa chỉ URL của hình ảnh (thuộc tính khóa)
            this.fileNameDescription = strElementData[4]; //tên tập tin mô tả về ảnh
            this.fileName = strElementData[5];//Tên tập tin chứa phần tử Ekement 
            this.listClass = String2Classes(strElementData[6]); //Các lớp mà một ElementData thuộc về
        }
        //Phương thức khởi tạp đôi tượng gồm những thông tin
        public ElementData(int index, List<double> feature, string imageID, string uri, string fileNameDescription, string fileName, List<string> listClass)
        {
            this.index = index;
            this.feature = feature;
            this.imageID = imageID;
            this.uri = uri;
            this.fileNameDescription = fileNameDescription;
            this.fileName = fileName;
            this.listClass = listClass;
        }
        //Phương thức khởi tạo sao chép đối tượng
        public ElementData(ElementData ED)
        {
            this.index = ED.Index;
            this.feature = ED.Feature;
            this.imageID = ED.ImageID;
            this.uri = ED.URI;
            this.fileNameDescription = ED.FileNameDescription;
            this.fileName = ED.FileName;
            this.listClass = ED.ListClass;
        }
        //Các phương thức truy xuất thuộc tính get/set
        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        public List<double> Feature
        {
            get { return feature; }
            set { feature = value; }
        }
        public string ImageID
        {
            get { return imageID; }
            set { imageID = value; }
        }
        public string URI
        {
            get { return uri; }
            set { uri = value; }
        }
        public string FileNameDescription
        {
            get { return fileNameDescription; }
            set { fileNameDescription = value; }
        }
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        public List<string> ListClass
        {
            get { return listClass; }
            set { listClass = value; }
        }
        //Chuyển một vector feature thành một chuỗi (mỗi giá trị cách nhau bằng một khoảng trắng) và ngược lại
        public string Feature2String()
        {
            if (this.feature == null) return string.Empty;
            if (this.feature.Count == 0) return string.Empty;
            string strFeature = string.Empty;
            foreach (double num in this.feature)
                strFeature += num.ToString() + " ";
            return strFeature.Trim();
        }
        //Chuyển một danh sách các lớp thành một chuỗi (mỗi giá trị cách nhau bằng một khoảng trắng) và ngược lại
        public string Classes2String()
        {
            if (this.listClass == null) return string.Empty;
            if (this.listClass.Count == 0) return string.Empty;
            string strClass = string.Empty;
            int len = this.listClass.Count;
            for (int i = 0; i < len - 1; i++)
                strClass += this.listClass.ElementAt(i) + ", ";
            strClass += this.listClass.ElementAt(len - 1);
            return strClass.Trim();
        }
        //Kiếm tra phần tử ElementData chứa trong cụm của một tập tin văn bản
        public bool CheckElementData(string file)
        {
            string lineElementData = string.Empty;
            TextfileCluster tfc = new TextfileCluster(file);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return false;
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return false;
            string f = Vector2String(this.feature);
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                if ((f == words[1]) && (this.imageID == words[2]))
                    return true;
            }
            return false;
        }
        //Kiểm tra một ImageID có trong dang sách các phần tử dữ liệu Element Data
        public bool CheckElementData(string ImgID, string feature, string file)
        {
            string lineElementData = string.Empty;
            TextfileCluster tfc = new TextfileCluster(file);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return false;
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return false;
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                if (words.Length < 2) return false;
                if ((feature == words[1]) && (ImgID == words[2]))
                    return true;
            }
            return false;
        }
        //Kiểm tra một phần tử Element Data trong fileED tương ứng với ImageID và đặc trưng feature
        public bool CheckElementData(string ImageID, List<double> feature, string fileED)
        {
            string lineElementData = string.Empty;
            TextfileCluster tfc = new TextfileCluster(fileED);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return false;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return false;
            string f = Vector2String(feature);
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                if (words.Length < 2) return false;
                if ((f == words[1]) && (ImageID == words[2]))
                    return true;
            }
            return false;
        }
        //Thêm một ElementData vào sau của tập tin văn bản chứa các ElementData
        //Nếu một ElementData giống nhau tất cả thành phần thì không thêm
        public void SaveElementData(string fileED)
        {
            string lineElementData = string.Empty;
            TextfileCluster tfc = new TextfileCluster(fileED);
            if (CheckElementData(this.imageID, this.feature, fileED) == true) return;
            this.index = tfc.NumberLines + 1;
            lineElementData += this.index.ToString() + " ";
            string tmp = this.Feature2String().Trim();
            if (tmp == string.Empty)
                lineElementData += "(" + "#NONE#" + ")" + " ";
            else
                lineElementData += "(" + this.Feature2String().Trim() + ")" + " ";
            if (this.imageID.Trim() == string.Empty)
                lineElementData += "(" + "#NONE#" + ")" + " ";
            else
                lineElementData += "(" + this.imageID.Trim() + ")" + " ";
            if (this.uri.Trim() == string.Empty)
                lineElementData += "(" + "#NONE#" + ")" + " ";
            else
                lineElementData += "(" + this.uri.Trim() + ")" + " ";
            if (this.fileNameDescription.Trim() == string.Empty)
                lineElementData += "(" + "#NONE#" + ")" + " ";
            else
                lineElementData += "(" + this.fileNameDescription.Trim() + ")" + " ";
            if (this.fileName.Trim() == string.Empty)
                lineElementData += "(" + "#NONE#" + ")" + " ";
            else
                lineElementData += "(" + Path.GetFileName(fileED) + ")" + " ";
            tmp = this.Classes2String().Trim();
            if (tmp == string.Empty)
                lineElementData += "(" + "#NONE#" + ")" + " ";
            else
                lineElementData += "(" + this.Classes2String().Trim() + ")" + " ";
            tfc.WriteLineTextFile(lineElementData, fileED);
        }
        //Lấy (tìm kiếm) một ElementData từ file tương ứng với ImageID
        public string[] getStrElementData(string ImageID, string fileED)
        {
            TextfileCluster tfc = new TextfileCluster(fileED);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return null;
            if (Lines.Length == 0) return null;
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                if (words.Length < 3) return null;
                if (words[2] == ImageID)
                    return words;
            }
            return null;
        }
        //Lấy (tìm kiếm) một ElementData từ file tương ứng với ImageID và chuyển thành một chuỗi
        public string getElementData2String(string ImageID, string fileED)
        {
            string[] StrArr = getStrElementData(ImageID, fileED);
            return StrArr2String(StrArr);
        }
        //Lấy (tìm kiếm) một ElementData từ file tương ứng với ImageID và chuyển thành một mảng các chuỗi
        public string[] getStrElementData(List<double> feature, string ImageID, string fileED)
        {
            TextfileCluster tfc = new TextfileCluster(fileED);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return null;
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return null;
            string f = Vector2String(feature).Trim();
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                if (words.Length < 2) return null;
                if (words[1] == f && words[2] == ImageID)
                    return words;
            }
            return null;
        }
        //Lấy (tìm kiếm) một ElementData từ file tương ứng với feature và ImageID, sau đó chuyển thành một chuỗi
        public string getElementData2String(List<double> feature, string ImageID, string fileED)
        {
            string[] StrArr = getStrElementData(feature, ImageID, fileED);
            return StrArr2String(StrArr);
        }
        //Lấy (tìm kiếm) một ElementData từ file tương ứng với feature và chuyển thành một mảng các chuỗi
        public string[] getStrElementData(List<double> feature, string fileED)
        {
            TextfileCluster tfc = new TextfileCluster(fileED);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return null;
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return null;
            string f = Vector2String(feature).Trim();
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                if (words.Length < 1) return null;
                if (words[1] == f)
                    return words;
            }
            return null;
        }
        //chuyển một chuỗi thành một đối tượng ElementData
        public ElementData getElementData(string[] strElementData)
        {
            if (strElementData == null) return null;
            if (strElementData.Length < 7) return null;
            ElementData ED = new ElementData();
            ED.Index = ToInt32(strElementData[0]);  //vị trí của ElementData trên file
            ED.Feature = String2Vector(strElementData[1]); //vector đặc trưng của một phần tử (thuộc tính khóa)
            ED.ImageID = strElementData[2]; //Định danh của hình ảnh (thuộc tính khóa)
            ED.URI = strElementData[3]; //Địa chỉ URL của hình ảnh (thuộc tính khóa)
            ED.FileNameDescription = strElementData[4]; //tên tập tin mô tả về ảnh (là một thuộc tính trực tiếp của ElementData)
            ED.FileName = strElementData[5];//Tên tập tin chứa phần tử Ekement (là một thuộc tính trực tiếp của ElementData)
            ED.ListClass = String2Classes(strElementData[6]); //Các lớp mà một ElementData thuộc về
            return ED;
        }
        //Lấy (tìm kiếm) một ElementData từ file tương ứng với feature, imageID và chuyển thành một mảng các chuỗi
        public string[] getStrElementData(string feature, string ImageID, string fileED)
        {
            TextfileCluster tfc = new TextfileCluster(fileED);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return null;
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return null;
            string f = feature.Trim();
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                if (words.Length < 3) return null;
                if (words[1] == f && words[2] == ImageID)
                    return words;
            }
            return null;
        }
        //Lấy một danh sách các phần tử ElementData từ một tập tin
        public List<ElementData> getListElementData(string fileED)
        {
            List<ElementData> listED = new List<ElementData>();
            TextfileCluster tfc = new TextfileCluster(fileED);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return null;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return null;
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                ElementData ED = getElementData(words);
                listED.Add(ED);
            }
            return listED.Where(_ => _ != null).ToList();
        }
        //Lấy giá trị vetor tâm của các phần tử Element Data
        public List<double> getCenterED(string fileED)
        {
            List<ElementData> listED = getListElementData(fileED);
            if (listED == null) return null;
            int NumOfList = listED.Count;
            if (NumOfList == 0) return null;
            List<double> CenterED = new List<double>();
            int m = NumOfList;
            int n = listED.ElementAt(0).Feature.Count;
            for (int j = 0; j < n; j++)
            {
                double ave = 0.0;
                for (int i = 0; i < m; i++)
                    ave = ave + listED.ElementAt(i).Feature.ElementAt(j);
                ave = ave / (double)m;
                CenterED.Add(ave);
            }
            return CenterED;
        }
        //Xóa một ElementData trong file tương ứng với ImgID
        public void DelElementData(string ImageID, string fileED)
        {
            TextfileCluster tfc = new TextfileCluster(fileED);
            tfc.DeleteFirstLine(ImageID);
            tfc.UpdateIndex();
        }
        //Xóa tất cả ElementData trong file tương ứng với ImgID
        public void DelAllElementData(string ImageID, string fileED)
        {
            TextfileCluster tfc = new TextfileCluster(fileED);
            tfc.DeleteLine(ImageID);
            tfc.UpdateIndex();
        }
        //Xóa một phần tử ElementData tương ứng với feature
        public void DelElementData(List<double> feature, string fileED)
        {
            TextfileCluster tfc = new TextfileCluster(fileED);
            string f = Vector2String(feature);
            tfc.DeleteFirstLine(f);
            tfc.UpdateIndex();
        }
        //Xóa tất cả các phần tử Element Data tương ứng với feature
        public void DelAllElementData(List<double> feature, string fileED)
        {
            TextfileCluster tfc = new TextfileCluster(fileED);
            string f = Vector2String(feature);
            tfc.DeleteLine(f);
            tfc.UpdateIndex();
        }
        //Xóa một ElementData trong file tương ứng với feature và ImgID
        public void DelElementData(string ImageID, List<double> feature, string fileED)
        {
            TextfileCluster tfc = new TextfileCluster(fileED);
            string f = Vector2String(feature);
            tfc.DeleteFirst2String(ImageID, f);
            tfc.UpdateIndex();
        }
        //Xóa tất cả ElementData trong file tương ứng với feature và ImgID
        public void DelAllElementData(string ImageID, List<double> feature, string fileED)
        {
            TextfileCluster tfc = new TextfileCluster(fileED);
            string f = Vector2String(feature);
            tfc.DeleteAll2String(ImageID, f);
            tfc.UpdateIndex();
        }
        //Đếm số lần xuất hiện của một tên lớp (class name) trong danh sách các phần tử dữ liệu Element Data
        public int CountClassED(string nameClass, string fileED)
        {
            int count = 0;
            TextfileCluster tfc = new TextfileCluster(fileED);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return 0;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return 0;
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                char[] delimiters = new char[] { '\t', '\r', '\n', ';', ',', ' ' };
                string[] Class = words[6].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                if (Class.Contains(nameClass))
                    count++;
            }
            return count;
        }
        //Đếm số lần xuất hiện của một tên lớp (class name) trong danh sách các phần tử dữ liệu Element Data
        public int CountClassED(string nameClass, List<ElementData> ListED)
        {
            int count = 0;
            if (ListED == null) return 0;
            foreach (ElementData ed in ListED)
            {
                if (ed.ListClass.Contains(nameClass))
                    count++;
            }
            return count;
        }
        //Lấy tên lớp xuất hiện nhiều nhất trong một danh sách các thành phần dữ liệu Element Data
        public string getClassMax(string fileED)
        {
            string nameClass = "#NONE#";
            TextfileCluster tfc = new TextfileCluster(fileED);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return "#NONE#";
            Lines = RemoveBlank(Lines);
            if (Lines == null) return "#NONE#";
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                char[] delimiters = new char[] { '\t', '\r', '\n', ';', ',', ' ' };
                string[] Class = words[6].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                foreach (string name in Class)
                {
                    if (CountClassED(nameClass, fileED) < CountClassED(name, fileED))
                        nameClass = name;
                }
            }
            return nameClass;
        }
        //Lấy tên lớp xuất hiện nhiều nhất nhưng nhỏ hơn một lớp cho trước
        public string getClassLessthan(string classname, string fileED)
        {
            string nameClass = "#NONE#";
            TextfileCluster tfc = new TextfileCluster(fileED);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return "#NONE#";
            Lines = RemoveBlank(Lines);
            if (Lines == null) return "#NONE#";
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                char[] delimiters = new char[] { '\t', '\r', '\n', ';', ',', ' ' };
                string[] Class = words[6].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                foreach (string name in Class)
                {
                    if ((CountClassED(nameClass, fileED) < CountClassED(name, fileED)) && (name != classname) && (CountClassED(name, fileED) <= CountClassED(classname, fileED)))
                        nameClass = name;
                }
            }
            return nameClass;
        }
        //Lấy tên lớp xuất hiện nhiều nhất nhưng không có trong một tập danh sách lớp cho trước
        public string getClassLessthan(List<string> Listname, string fileED)
        {
            string nameClass = "#NONE#";
            TextfileCluster tfc = new TextfileCluster(fileED);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return "#NONE#";
            Lines = RemoveBlank(Lines);
            if (Lines == null) return "#NONE#";
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                char[] delimiters = new char[] { '\t', '\r', '\n', ';', ',', ' ' };
                string[] Class = words[6].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                foreach (string name in Class)
                {
                    if ((CountClassED(nameClass, fileED) < CountClassED(name, fileED)) && (Listname.Contains(name)==false))
                        nameClass = name;
                }
            }
            return nameClass;
        }
        //Lấy danh sách tên lớp của cụm các dữ liệu
        public List<string> getListClassName(string fileED)
        {
            List<string> lstName = new List<string>();
            TextfileCluster tfc = new TextfileCluster(fileED);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return lstName;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return lstName;
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                char[] delimiters = new char[] { '\t', '\r', '\n', ';', ',', ' ' };
                string[] Class = words[6].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                foreach (string name in Class)
                {
                    if (!lstName.Contains(name))
                        lstName.Add(name);
                }
            }
            return lstName;
        }
        //Rút trích danh sách các phần tử có xuất hiện trong một tên lớp
        public List<ElementData> getListElementData(List<ElementData> ListED, List<string> ClassName)
        {
            List<ElementData> L = new List<ElementData>();
            if (ListED == null) return null;
            foreach (ElementData ed in ListED)
            {
                List<string> list = ed.ListClass;
                foreach (string name in list)
                    if ((ClassName.Contains(name) == true) && (L.Contains(ed) == false))
                        L.Add(ed);
            }
            return L;
        }
        //Lấy danh sách tên lớp
        public List<string> getListClass(string fileED)
        {
            List<string> ListClass = new List<string>();
            TextfileCluster tfc = new TextfileCluster(fileED);
            string[] Lines = tfc.ReadAllLine();
            Lines = RemoveBlank(Lines);
            if (Lines == null) return null;
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                char[] delimiters = new char[] { '\t', '\r', '\n', ';', ',', ' ' };
                string[] Class = words[6].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                foreach (string name in Class)
                {
                    if (ListClass.Contains(name) == false)
                        ListClass.Add(name);
                }
            }
            return ListClass;
        }
        //Lấy danh sách tên lớp
        public List<string> getListClass(List<ElementData> listED)
        {
            List<string> ListClass = new List<string>();
            if (listED == null) return null;
            foreach (ElementData ed in listED)
            {
                foreach (string name in ed.ListClass)
                {
                    if (ListClass.Contains(name) == false)
                        ListClass.Add(name);
                }
            }
            return ListClass;
        }
        //Sắp xếp lại các Elemente Data theo thứ tự độ đo Euclide của một vector đầu vào
        public List<ElementData> SortED(List<double> feature, List<ElementData> listED)
        { 
            ElementData[] ArrED = listED.ToArray();
            int n = ArrED.Length;
            for(int i = 0; i<n-1; i++)
                for(int j = i+1; j < n; j++)
                    if (EuclideDistance(ArrED[i].Feature, feature) > EuclideDistance(ArrED[j].Feature, feature))
                    {
                        ElementData tmp = ArrED[i];
                        ArrED[i] = ArrED[j];
                        ArrED[j] = tmp;
                    }
            return ArrED.ToList();
        }
        //Sắp xếp lại các Element Data theo thứ tự độ đo Euclide của một vector đầu vào
        public List<ElementData> SortEDKeys(List<double> feature, List<ElementData> listED)
        {
            ElementData[] ArrED = listED.ToArray();
            int n = ArrED.Length;
            double[] keys = new double[n];
            for (int i = 0; i < n; i++)
                keys[i] = EuclideDistance(ArrED[i].Feature, feature);
            Array.Sort(keys, ArrED);
            return ArrED.ToList();
        }
        //Sắp xếp lại danh sách ED có class gần với classname đưa vào nhất
        public List<ElementData> SortEDKeysClass(List<double> feature, List<ElementData> listED, string classname)
        {
            ElementData[] ArrED = listED.ToArray();
            int n = ArrED.Length;
            double[] keys = new double[n];
            for (int i = 0; i < n; i++)
            {
                if (ArrED[i].listClass.Contains(classname))
                    keys[i] = 0.0;
                else
                    keys[i] = EuclideDistance(ArrED[i].Feature, feature);
            }
            Array.Sort(keys, ArrED);
            return ArrED.ToList();
        }

        //Sắp xếp các danh sách các lớp theo tần xuất xuất hiện trong tập danh sách
        public List<string> SortClass(List<string> listClass, List<ElementData> listED)
        {
            string [] ArrClass = listClass.ToArray();
            int n = ArrClass.Length;
            for (int i = 0; i < n - 1; i++)
                for (int j = i + 1; j < n; j++)
                    if (CountClassED(ArrClass[i], listED) < CountClassED(ArrClass[j], listED))
                    {
                        string tmp = ArrClass[i];
                        ArrClass[i] = ArrClass[j];
                        ArrClass[j] = tmp;
                    }
            return ArrClass.ToList();
        }
        //Sắp xếp các danh sách các lớp theo tần xuất xuất hiện trong tập danh sách
        public List<string> SortClass(List<string> listClass, string fileED)
        {
            string[] ArrClass = listClass.ToArray();
            int n = ArrClass.Length;
            for (int i = 0; i < n - 1; i++)
                for (int j = i + 1; j < n; j++)
                    if (CountClassED(ArrClass[i], fileED) < CountClassED(ArrClass[j], fileED))
                    {
                        string tmp = ArrClass[i];
                        ArrClass[i] = ArrClass[j];
                        ArrClass[j] = tmp;
                    }
            return ArrClass.ToList();
        }
        //Lấy giá trị tâm của lớp xuất hiện nhiều nhất
        public List<double> getWeight(string fileED)
        {
            string nameclass = getClassMax(fileED);
            List<double> center = new List<double>();
            TextfileCluster tfc = new TextfileCluster(fileED);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return null;
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return null;
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                char[] delimiters = new char[] { '\t', '\r', '\n', ';', ',', ' ' };
                string[] Class = words[6].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                if (Class.Contains(nameclass))
                {
                    List<double> f = String2Vector(words[1]);
                    center = SumList(center, f);
                }
            }
            int n = CountClassED(nameclass, fileED);
            center = DivList(center, n);
            return center;
        }

        //Lấy tên lớp xuất hiện nhiều thứ hai trong một danh sách các thành phần dữ liệu Element Data
        public string getClassMaxSecond(string fileED)
        {
            string MaxClass = getClassMax(fileED);
            string nameClass = "#NONE#";
            TextfileCluster tfc = new TextfileCluster(fileED);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return "#NONE#";
            Lines = RemoveBlank(Lines);
            if (Lines == null) return "#NONE#";
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                char[] delimiters = new char[] { '\t', '\r', '\n', ';', ',', ' ' };
                string[] Class = words[6].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                foreach (string name in Class)
                {
                    if (CountClassED(nameClass, fileED) < CountClassED(name, fileED) && name != MaxClass)
                        nameClass = name;
                }
            }
            return nameClass;
        }

        //Lấy giá trị tâm của lớp xuất hiện nhiều thứ hai
        public List<double> getWeightSecond(string fileED)
        {
            string nameclass = getClassMaxSecond(fileED);
            List<double> center = new List<double>();
            TextfileCluster tfc = new TextfileCluster(fileED);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return null;
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return null;
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                char[] delimiters = new char[] { '\t', '\r', '\n', ';', ',', ' ' };
                string[] Class = words[6].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                if (Class.Contains(nameclass))
                {
                    List<double> f = String2Vector(words[1]);
                    center = SumList(center, f);
                }
            }
            int n = CountClassED(nameclass, fileED);
            center = DivList(center, n);
            return center;
        }

        public ElementData getEDwithClassname(string fileED, string ClassName)
        {
            List<ElementData> listED = getListElementData(fileED);
            if (listED == null) return null;
            foreach (ElementData ed in listED)
            {
                if (ed.listClass.Contains(ClassName))
                    return ed;
            }
            return null;
        }

        //Tìm một phần tử xa nhất với một phần tử ElementData cho trước theo khoảng cách Euclide
        public ElementData getFarestED(ElementData ED, string fileED)
        { 
            ElementData EDmax = ED;
            List<ElementData> listED = getListElementData(fileED);
            if (listED.Count == 0) return EDmax;
            foreach (ElementData ed in listED)
            {
                if (EuclideDistance(EDmax.feature, ED.feature) < EuclideDistance(ed.feature, ED.feature))
                    EDmax = ed;
            }
            return EDmax;
        }
        //Tìm một phần tử xa nhất so với tâm
        public ElementData getFarestEDcenter(string fileED)
        {
            List<double> CenterED = getCenterED(fileED);
            ElementData EDmax = new ElementData();
            List<ElementData> listED = getListElementData(fileED);
            if (listED == null) return EDmax;
            EDmax = listED.ElementAt(0);
            foreach (ElementData ed in listED)
            {
                if (EuclideDistance(EDmax.feature, CenterED) < EuclideDistance(ed.feature, CenterED))
                    EDmax = ed;
            }
            return EDmax;
        }
        //tìm phần tử gần nhất ứng với một vector
        public ElementData getMinED(List<double> vector, string fileED)
        {
            ElementData EDmin;
            List<ElementData> listED = getListElementData(fileED);
            if (listED == null) return null;
            EDmin = listED.ElementAt(0);
            foreach (ElementData ed in listED)
            {
                if (EuclideDistance(vector, EDmin.feature) > EuclideDistance(vector, ed.feature))
                    EDmin = ed;
            }
            return EDmin;
        }
        public bool CheckImgInList(string ImgFile, List<ElementData> listED)
        {
            if (listED == null) return false;
            if (listED.Count == 0) return false;
            foreach (ElementData ed in listED)
                if (ed.ImageID == ImgFile)
                    return true;
            return false;
        }
        //Ghép 2 danh sách ED thành một danh sách ED
        public List<ElementData> AddListED(List<ElementData> ListED1, List<ElementData> ListED2)
        {
            if (ListED1 == null) return ListED2;
            if (ListED2 == null) return ListED1;
            if (ListED1.Count == 0) return ListED2;
            if (ListED2.Count == 0) return ListED1;
            foreach (ElementData ed in ListED2)
            {
                if (CheckImgInList(ed.ImageID, ListED1) == false)
                    ListED1.Add(ed);
            }
            return ListED1;
        }






        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////     Các phương thức để xử lý cây H-Tree                                                         ///////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        //Thêm một ElementData vào sau của tập tin văn bản chứa các ElementData
        //Nếu một ElementData giống nhau tất cả thành phần thì không thêm
        public void SaveElementDataHTree(string fileED)
        {
            string lineElementData = string.Empty;
            TextfileCluster tfc = new TextfileCluster(fileED);
            //if (CheckElementData(this.imageID, this.feature, filename) == true) return;
            this.index = tfc.NumberLines + 1;
            lineElementData += this.index.ToString() + " ";
            string tmp = this.Feature2String().Trim();
            if (tmp == string.Empty)
                lineElementData += "(" + "#NONE#" + ")" + " ";
            else
                lineElementData += "(" + this.Feature2String().Trim() + ")" + " ";
            if (this.imageID.Trim() == string.Empty)
                lineElementData += "(" + "#NONE#" + ")" + " ";
            else
                lineElementData += "(" + this.imageID.Trim() + ")" + " ";
            if (this.uri.Trim() == string.Empty)
                lineElementData += "(" + "#NONE#" + ")" + " ";
            else
                lineElementData += "(" + this.uri.Trim() + ")" + " ";
            if (this.fileNameDescription.Trim() == string.Empty)
                lineElementData += "(" + "#NONE#" + ")" + " ";
            else
                lineElementData += "(" + this.fileNameDescription.Trim() + ")" + " ";
            if (this.fileName.Trim() == string.Empty)
                lineElementData += "(" + "#NONE#" + ")" + " ";
            else
                lineElementData += "(" + this.fileName.Trim() + ")" + " ";
            tmp = this.Classes2String().Trim();
            if (tmp == string.Empty)
                lineElementData += "(" + "#NONE#" + ")" + " ";
            else
                lineElementData += "(" + this.Classes2String().Trim() + ")" + " ";
            tfc.WriteLineTextFile(lineElementData, fileED);
        }

        public List<ElementData> getListElementDataHTree(string fileED)
        {
            List<ElementData> listED = new List<ElementData>();
            TextfileCluster tfc = new TextfileCluster(fileED);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return null;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return null;
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                ElementData ED = getElementDataHTree(words);
                listED.Add(ED);
            }
            return listED.Where(_ => _ != null).ToList();
        }

        //Lấy giá trị tâm của lớp xuất hiện nhiều nhất
        public List<double> getWeightHTree(string fileED)
        {
            string nameclass = getClassMax(fileED);
            List<double> center = new List<double>();
            TextfileCluster tfc = new TextfileCluster(fileED);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return null;
            Lines = RemoveBlank(Lines);
            if (Lines.Length == 0) return null;
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                char[] delimiters = new char[] { '\t', '\r', '\n', ';', ',', ' ' };
                string[] Class = words[6].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                if (Class.Contains(nameclass))
                {
                    List<double> f = String2Vector(words[1]);
                    center = SumList(center, f);
                }
            }
            int n = CountClassED(nameclass, fileED);
            center = DivList(center, n);
            return center;
        }

        //chuyển một chuỗi thành một đối tượng ElementData
        public ElementData getElementDataHTree(string[] strElementData)
        {
            //if (strElementData == null) return null;
            //Console.WriteLine(strElementData.Length);
            //ElementData ED = new ElementData();
            //ED.Index = ToInt32(strElementData[0]);  //vị trí của ElementData trên file
            //ED.Feature = String2Vector(strElementData[1]); //vector đặc trưng của một phần tử (thuộc tính khóa)
            //ED.ImageID = strElementData[2]; //Định danh của hình ảnh (thuộc tính khóa)
            //ED.URI = strElementData[3]; //Địa chỉ URL của hình ảnh (thuộc tính khóa)
            //ED.FileNameDescription = strElementData[4]; //tên tập tin mô tả về ảnh (là một thuộc tính trực tiếp của ElementData)
            //ED.FileName = strElementData[5];//Tên tập tin chứa phần tử Ekement (là một thuộc tính trực tiếp của ElementData)
            //ED.ListClass = strElementData.Length == 7 ?  String2Classes(strElementData[6]) : new List<string>(); //Các lớp mà một ElementData thuộc về
            //return ED;

            if (strElementData == null) return null;
            if (strElementData.Length < 7) return null;
            ElementData ED = new ElementData();
            ED.Index = ToInt32(strElementData[0]);  //vị trí của ElementData trên file
            ED.Feature = String2Vector(strElementData[1]); //vector đặc trưng của một phần tử (thuộc tính khóa)
            ED.ImageID = strElementData[2]; //Định danh của hình ảnh (thuộc tính khóa)
            ED.URI = strElementData[3]; //Địa chỉ URL của hình ảnh (thuộc tính khóa)
            ED.FileNameDescription = strElementData[4]; //tên tập tin mô tả về ảnh (là một thuộc tính trực tiếp của ElementData)
            ED.FileName = strElementData[5];//Tên tập tin chứa phần tử Ekement (là một thuộc tính trực tiếp của ElementData)
            ED.ListClass = String2Classes(strElementData[6]); //Các lớp mà một ElementData thuộc về
            return ED;
        }

        //Lấy giá trị vetor tâm của các phần tử Element Data
        public List<double> getCenterEDHTree(string fileED)
        {
            List<ElementData> listED = getListElementData(fileED);
            if (listED == null) return null;
            int NumOfList = listED.Count;
            if (NumOfList == 0) return null;
            List<double> CenterED = new List<double>();
            int m = NumOfList;
            int n = listED.ElementAt(0).Feature.Count;
            for (int j = 0; j < n; j++)
            {
                double ave = 0.0;
                for (int i = 0; i < m; i++)
                    ave = ave + listED.ElementAt(i).Feature.ElementAt(j);
                ave = ave / (double)m;
                CenterED.Add(ave);
            }
            return CenterED;
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////     Các phương thức để xử lý cây GP-Tree                                                         ///////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        //chuyển một chuỗi thành một đối tượng ElementData
        public ElementData getElementDataGPTree(string[] strElementData)
        {
            if (strElementData == null) return null;
            if (strElementData.Length < 7) return null;
            ElementData ED = new ElementData();
            ED.Index = ToInt32(strElementData[0]);  //vị trí của ElementData trên file
            ED.Feature = String2Vector(strElementData[1]); //vector đặc trưng của một phần tử (thuộc tính khóa)
            ED.ImageID = strElementData[2]; //Định danh của hình ảnh (thuộc tính khóa)
            ED.URI = strElementData[3]; //Địa chỉ URL của hình ảnh (thuộc tính khóa)
            ED.FileNameDescription = strElementData[4]; //tên tập tin mô tả về ảnh (là một thuộc tính trực tiếp của ElementData)
            ED.FileName = strElementData[5];//Tên tập tin chứa phần tử Ekement (là một thuộc tính trực tiếp của ElementData)
            ED.ListClass = String2Classes(strElementData[6]); //Các lớp mà một ElementData thuộc về
            return ED;
        }

        //Lấy danh sấch ED để xây dựng GP-Tree
        public List<ElementData> getListElementDataGPTree(string fileED)
        {
            List<ElementData> listED = new List<ElementData>();
            TextfileCluster tfc = new TextfileCluster(fileED);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return null;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return null;
            foreach (string line in Lines)
            {
                string[] words = line.Split('(', ')');
                words = RemoveBlank(words);
                ElementData ED = getElementDataGPTree(words);
                listED.Add(ED);
            }
            return listED.Where(_ => _ != null).ToList();
        }

        //Lấy giá trị vetor tâm của các phần tử Element Data
        public List<double> getCenterED_GPTree(string fileED)
        {
            List<ElementData> listED = getListElementDataGPTree(fileED);
            if (listED == null) return null;
            int NumOfList = listED.Count;
            if (NumOfList == 0) return null;
            List<double> CenterED = new List<double>();
            int m = NumOfList;
            int n = listED.ElementAt(0).Feature.Count;
            for (int j = 0; j < n; j++)
            {
                double ave = 0.0;
                for (int i = 0; i < m; i++)
                    ave = ave + listED.ElementAt(i).Feature.ElementAt(j);
                ave = ave / (double)m;
                CenterED.Add(ave);
            }
            return CenterED;
        }

        public ElementData getMinED_GPTree(List<double> vector, string fileED)
        {
            ElementData EDmin;
            List<ElementData> listED = getListElementDataGPTree(fileED);
            if (listED == null) return null;
            EDmin = listED.ElementAt(0);
            foreach (ElementData ed in listED)
            {
                if (EuclideDistance(vector, EDmin.feature) > EuclideDistance(vector, ed.feature))
                    EDmin = ed;
            }
            return EDmin;
        }
    }
}
