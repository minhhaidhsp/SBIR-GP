using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBIR
{
    class DataProcessing
    {
        public string StrArr2String(string[] StrArr)
        {
            string str = string.Empty;
            int len = StrArr.Length;
            for (int i = 0; i < len; i++)
                str += StrArr[i] + " ";
            return str.Trim();
        }
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
        public string getClass(string nName, string classfile)
        {
            //string file = getFileClass();
            TextfileCluster tfc = new TextfileCluster(classfile);
            string[] Lines = tfc.ReadAllLine();
            string classname = string.Empty;
            ClusterNode cn = new ClusterNode();
            foreach (string line in Lines)
            {
                char[] delimiters = new char[] { '\t', '\r', '\n', ';', '!', ':', ',', ' ' };
                string[] words = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                words = cn.RemoveBlank(words);
                if (words.Contains(nName))
                    classname += words[1].Trim() + " ";
            }
            return classname.Trim();
        }

        public int CheckImg(List<string> list, List<string> Groudtruth)
        {
            int count = 0;
            if (list.Count == 0) return 0;
            foreach (string name in list)
            {
                if (Groudtruth.Contains(Path.GetFileName(name)) == true)
                    count++;
            }
            return count;
        }

        //Lấy tên thư mục cuối cùng của một đường dẫn chứa tên file
        public string getLastFolderName(string fullPath)
        {
            string lastDirectory = Path.GetDirectoryName(fullPath).Split('\\').LastOrDefault();
            return lastDirectory;
        }

        //Lấy tên thư mục cuối cùng của một đường dẫn chứa tên file
        public string getFolderName(string DirPath)
        {
            string[] words = DirPath.Split('\\');
            words = RemoveBlank(words);
            string FolderName = words[words.Length - 1];
            return FolderName;
        }

        //Lấy danh sách đường dẫn thư mục cuối cùng từ một folder
        public List<string> getSubFloder(string folderPath)
        {
            //string[] dirs = Directory.GetDirectories(folderPath);
            string[] folders = System.IO.Directory.GetDirectories(folderPath, "*", System.IO.SearchOption.AllDirectories);
            return folders.ToList();
        }

        //Lấy danh sách đường dẫn thư mục cuối cùng từ một folder
        public List<string> getSubFloderName(string folderPath)
        {
            //string[] dirs = Directory.GetDirectories(folderPath);
            string[] folders = System.IO.Directory.GetDirectories(folderPath, "*", System.IO.SearchOption.AllDirectories);
            List<string> folderName = new List<string>();
            for (int i = 0; i < folders.Length; i++)
                folderName.Add(getFolderName(folders[i]));

            return folderName;
        }

        public string CheckImgGPTree(List<string> list, List<string> Groudtruth)
        {
            int count = 0;
            string listImageResult = String.Empty;
            if (list.Count == 0) return null;
            foreach (string name in list)
            {
                if (Groudtruth.Contains(Path.GetFileName(name)) == true)
                    listImageResult += name + "\t";
            }
            return listImageResult;
        }
    }
}
