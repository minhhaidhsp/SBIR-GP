using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBIR
{
    public abstract class ClusterMethod
    {
        //Chuyển một chuỗi thành một số nguyên
        public int ToInt32(string myStr)
        {
            bool res;
            int a;
            res = int.TryParse(myStr, out a);
            return a;
        }
        //chuyển một chuỗi thành một số thực
        public double ToDecimal(string myStr)
        {
            bool res;
            double a;
            res = double.TryParse(myStr, out a);
            return a;
        }
        //Loại bỏ các phần tử trống trong một mảng string
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
        //Loại bỏ các phần tử trống trong một List string
        public List<string> RemoveBlank(List<string> Names)
        {
            if (Names == null) return null;
            if (Names.Count == 0) return null;
            string[] ArrStr = Names.ToArray();
            ArrStr = RemoveBlank(ArrStr);
            return ArrStr.ToList();
        }
        //Chuyển một Vector (List) số thực (double) thành một chuỗi số, mỗi số cách nhau một khoảng trắng
        public string Vector2String(List<double> Vector)
        {
            string strFeature = string.Empty;
            if (Vector == null) return "#NONE#";
            if (Vector.Count == 0) return "#NONE#";
            foreach (double num in Vector)
                strFeature += num.ToString() + " ";
            return strFeature.Trim();
        }
        //Chuyển một mảng các chuỗi số thành một Vector (List) chứa các số double
        public List<double> String2Vector(string[] DigitString)
        {
            if (DigitString == null) return null;
            if (DigitString.Length == 0) return null;
            DigitString = RemoveBlank(DigitString);
            int len = DigitString.Length;
            List<double> Vector = new List<double>();
            for (int i = 0; i < len; i++)
                Vector.Add(ToDecimal(DigitString[i]));
            return Vector;
        }
        //Chuyển một chuỗi số thành một Vector (List) chứa các số double
        public List<double> String2Vector(string DigitString)
        {
            if (DigitString.Trim() == string.Empty) return null;
            List<double> Vector = new List<double>();
            if (DigitString == string.Empty) return null;
            char[] delimiters = new char[] { '\t', '\r', '\n', ';', '!', ':', ',', ' ' };
            string[] wordsDigit = DigitString.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            wordsDigit = RemoveBlank(wordsDigit);
            return String2Vector(wordsDigit);
        }
        //Chuyển một danh sách tên (Names) thành một chuỗi
        public string Classes2String(List<string> listNames)
        {
            if (listNames == null) return string.Empty;
            if (listNames.Count == 0) return string.Empty;
            string strNames = string.Empty;
            foreach (string cla in listNames)
                strNames += cla + " ";
            return strNames.Trim();
        }
        //Chuyển một chuỗi thành danh sách các tên (Names)
        public List<string> String2Classes(string strNames)
        {
            if (strNames.Trim() == string.Empty) return null;
            List<string> listNames = new List<string>();
            char[] delimiters = new char[] { '\t', '\r', '\n', ';', ',', ' ' };
            string[] wordsNames = strNames.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            wordsNames = RemoveBlank(wordsNames);
            foreach (string cla in wordsNames)
                if (listNames.Contains(cla) == false)
                    listNames.Add(cla);
            return listNames;
        }
        //tách một dòng có dấu ngoặc "()" thành một danh sách các từ
        public List<string> String2WordsList(string strWords)
        {
            if (strWords.Trim() == string.Empty) return null;
            List<string> listWords = new List<string>();
            string[] Words = strWords.Split('(', ')');
            Words = RemoveBlank(Words);
            if (Words == null) return null;
            if (Words.Length == 0) return null;
            foreach (string word in Words)
                if (listWords.Contains(word) == false)
                    listWords.Add(word);
            return listWords;
        }
        //Chuyển một mảng các chuỗi thành một chuỗi
        public string StrArr2String(string[] StrArr)
        {
            StrArr = RemoveBlank(StrArr);
            if (StrArr == null) return string.Empty;
            if (StrArr.Length == 0) return string.Empty;
            int len = StrArr.Length;
            string str = string.Empty;
            for (int i = 0; i < len; i++)
                str += StrArr[i].Trim() + " ";
            return str.Trim();
        }
        //Chuyển một mảng các chuổi thành một chuỗi các thành phần Element
        public string StrArr2Element(string[] StrArr)
        {
            StrArr = RemoveBlank(StrArr);
            if (StrArr == null) return "(#NONE#)";
            if (StrArr.Length == 0) return "(#NONE#)";
            int len = StrArr.Length;
            string str = string.Empty;
            for (int i = 0; i < len; i++)
                str += "(" + StrArr[i].Trim() + ")" + " ";
            return str.Trim();
        }
        //Chuyển một danh sách các chuổi thành một chuỗi các thành phần Element
        public string StrList2Element(List<string> StrList)
        {
            StrList = RemoveBlank(StrList);
            if (StrList == null) return "(#NONE#)";
            int len = StrList.Count;
            if (len == 0) return "(#NONE#)";
            string str = string.Empty;
            for (int i = 0; i < len; i++)
                str += "(" + StrList.ElementAt(i).Trim() + ")" + " ";
            return str.Trim();
        }
        //Tính giá trị trung bình của hai danh sách các feature
        public List<double> AverageList(List<double> list1, List<double> list2)
        {
            
            if (list1.Count == 0) return list2;
            if (list2.Count == 0) return list1;
            int len = list1.Count;
            List<double> ave = new List<double>();
            for (int i = 0; i < len; i++)
            {
                double a = (list1.ElementAt(i) + list2.ElementAt(i)) / (double)2.0;
                ave.Add(a);
            }
            return ave;
        }
        //Tính giá trị tổng của hai danh sách các feature
        public List<double> SumList(List<double> list1, List<double> list2)
        {
            if (list1 == null) return list2;
            if (list2 == null) return list1;
            if (list1.Count == 0) return list2;
            if (list2.Count == 0) return list1;
            int len = list1.Count;
            List<double> sum = new List<double>();
            for (int i = 0; i < len; i++)
            {
                double a = (list1.ElementAt(i) + list2.ElementAt(i));
                sum.Add(a);
            }
            return sum;
        }
        //Chia giá trị của một cho một số n 
        public List<double> DivList(List<double> list, int n)
        {
            if (list == null) return null;
            if (list.Count == 0) return null;
            List<double> div = new List<double>();
            int len = list.Count;
            for (int i = 0; i < len; i++)
            {
                double a = list.ElementAt(i) / n;
                div.Add(a);
            }
            return div;
        }
        //Tính khoảng cách Euclide trung bình của một vector đến gốc tọa độ
        public double EuclideDistance(List<double> V)
        {
            if (V == null) return 0.0;
            if (V.Count == 0) return 0.0;
            double dis = 0.0;
            foreach (double a in V)
            {
                dis += a*a;
            }
            dis = dis / (double)(V.Count);
            return Math.Sqrt(dis);
        }
        //Tính khoảng cách Euclide của hai vector
        public double EuclideDistance(List<double> V1, List<double> V2)
        {
            double dis = 0.0;
            if (V1 == null) return EuclideDistance(V2);
            if (V2 == null) return EuclideDistance(V1);
            int m = V1.Count;
            int n = V2.Count;
            if (m == 0) return EuclideDistance(V2);
            if (n == 0) return EuclideDistance(V1);
            if (m != n) return 1.0;
            for (int i = 0; i < n; i++)
            {
                dis += (V1.ElementAt(i) - V2.ElementAt(i)) * (V1.ElementAt(i) - V2.ElementAt(i));
            }
            dis = dis / (double)n;
            return Math.Sqrt(dis);
        }
        //Kiểm tra hai Class có mối quan hệ cha con (is-a) hay không
        public bool IsAClass(string superClass, string subClass, string fileClass)
        {
            TextfileCluster tfc = new TextfileCluster(fileClass);
            string[] Lines = tfc.ReadAllLine();
            if (Lines == null) return false;
            Lines = RemoveBlank(Lines);
            if (Lines == null) return false;
            foreach (string line in Lines)
            {
                char[] delimiters = new char[] { '\t', '\r', '\n', ';', ',', ':', ' ' };
                string[] words = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                if (superClass == words[0])
                    if (words.Contains(subClass))
                        return true;
            }
            return false;
        }
        //Tính tích vô hướng của hai vector
        public double productVector(List<double> V1, List<double> V2)
        {
            double prod = 0.0;
            if (V1 == null) return 0.0;
            if (V2 == null) return 0.0;
            int m = V1.Count;
            int n = V2.Count;
            if (m == 0) return 0.0;
            if (n == 0) return 0.0;
            if (m != n) return 0.0;
            for (int i = 0; i < n; i++)
            {
                prod += (V1.ElementAt(i) *V2.ElementAt(i));
            }
            return prod;
        }

        //Tính khoảng cách Euclide trung bình của một vector đến gốc tọa độ
        public double disVector(List<double> V)
        {
            if (V == null) return 0.0;
            if (V.Count == 0) return 0.0;
            double dis = 0.0;
            foreach (double a in V)
            {
                dis += a;
            }
            dis = dis / (double)(V.Count);
            return dis;
        }

        //Tính trung bình khoảng cách L1 của hai vector
        public double disVectorL1(List<double> V1, List<double> V2)
        {
            double dis = 0.0;
            if (V1 == null) return disVector(V2);
            if (V2 == null) return disVector(V1);
            int m = V1.Count;
            int n = V2.Count;
            if (m == 0) return disVector(V2);
            if (n == 0) return disVector(V1);
            if (m != n) return 0.0;
            for (int i = 0; i < n; i++)
            {
                dis += Math.Abs(V1.ElementAt(i) - V2.ElementAt(i));
            }
            return dis/(double)n;
        }

        //Tính giá trị sigmoid của một giá trị
        public double sigmoid(double a)
        {
            double ex = Math.Exp(a);
            double sig = ex / (ex + 1.0);
            return sig;
        }
    }
}
