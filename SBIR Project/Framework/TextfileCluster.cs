using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SBIR_Project.Framework;
using System.Threading;

namespace SBIR
{
    public class TextfileCluster
    {
        //Các thuộc tính của lóp thao tác trên tập tin văn bản
        private string textFile;
        private int numLines;
        
        //Các phương thức trích xuất thuộc tính
        public string TextFile
        {
            set { textFile = value; }
            get { return textFile; }
        }
        public int NumberLines
        {
            set { numLines = value; }
            get { return numLines; }
        }

#region Các phương thức mới
        //Phương thức khởi tạo với tên một tập tin văn bản
        public TextfileCluster()
        {
            textFile = string.Empty;
            numLines = 0;
        }
        //Phương thức khởi tạo vớn một đường dẫn của file, nếu file chưa tồn tại thì tạo mới
        public TextfileCluster(string textFile)
        {
            FileStream fs = null;
            if (!File.Exists(textFile))
            {
                using (fs = File.Create(textFile)) { }
            }
            if (File.Exists(textFile))
            {
                this.textFile = textFile;
                string[] Lines = File.ReadAllLines(textFile);
                Lines = RemoveBlank(Lines);
                if (Lines == null)
                {
                    numLines = 0;
                    return;
                }
                if (Lines.Length == 0)
                {
                    numLines = 0;
                    return;
                }
                this.numLines = Lines.Length;
            }
        }

        //Thêm một dòng str và một tập tin văn bản có tên filename
        public void WriteLineTextFile(string str, string textFile)
        {
            // Create a Text File
            FileStream fs = null;
            if (!File.Exists(textFile))
            {
                using (fs = File.Create(textFile)) { }
            }

            // Append to a Text File
            if (File.Exists(textFile))
            {
                using (StreamWriter sw = File.AppendText(textFile))
                {
                    sw.WriteLine(str);
                    this.numLines++;
                }
            }
        }

        public void WriteLineTextFile(string str)
        {
            // Create a Text File
            FileStream fs = null;
            if (!File.Exists(textFile))
            {
                using (fs = File.Create(textFile)) { }
            }

            // Append to a Text File
            if (File.Exists(textFile))
            {
                using (StreamWriter sw = File.AppendText(textFile))
                {
                    sw.WriteLine(str);
                    this.numLines++;
                }
            }
        }
        
        public void WriteNewTextFile(string str, string textFile)
        {
            // Create a Text File
            FileStream fs = null;
            if (!File.Exists(textFile))
            {
                using (fs = File.Create(textFile)) { }
            }

            // Write to Text File
            if (File.Exists(textFile))
            {
                using (StreamWriter sw = File.CreateText(textFile))
                {
                    sw.WriteLine(str);
                }
            }
        }
        public void WriteNewTextFile(string str)
        {
            // Create a Text File
            FileStream fs = null;
            if (!File.Exists(textFile))
            {
                using (fs = File.Create(textFile)) { }
            }

            // Write to Text File
            if (File.Exists(textFile))
            {
                using (StreamWriter sw = File.CreateText(textFile))
                {
                    sw.Write(str);
                }
            }
        }
        public void WriteNewTextFile(string[] ArrStr, string textFile)
        {
            // Create a Text File
            FileStream fs = null;
            if (!File.Exists(textFile))
            {
                using (fs = File.Create(textFile)) { }
            }

            //string str = string.Empty;
            int len = ArrStr.Length;
            //for (int i = 0; i < len; i++)
            //{
            //    str += ArrStr[i] + "\r\n";
            //}

            // Write to Text File
            if (File.Exists(textFile))
            {
                using (StreamWriter sw = File.CreateText(textFile))
                {
                    for (int i = 0; i < len; i++)
                    {
                        sw.WriteLine(ArrStr[i].Trim());
                        //str += ArrStr[i] + "\r\n";
                    }
                    
                }
            }
        }
        public void WriteNewTextFile(string[] ArrStr)
        {
            // Create a Text File
            FileStream fs = null;
            if (!File.Exists(textFile))
            {
                using (fs = File.Create(textFile)) { }
            }

            //string str = string.Empty;
            int len = ArrStr.Length;
            //for (int i = 0; i < len; i++)
            //{
            //    str += ArrStr[i] + "\r\n";
            //}

            // Write to Text File
            if (File.Exists(textFile))
            {
                using (StreamWriter sw = File.CreateText(textFile))
                {
                    for (int i = 0; i < len; i++)
                    {
                        sw.WriteLine(ArrStr[i].Trim());
                        //str += ArrStr[i] + "\r\n";
                    }
                }
            }
        }

        public string[] ReadAllLine(string textFile)
        {
            FileStream fs = null;
            if (!File.Exists(textFile))
            {
                using (fs = File.Create(textFile)) { }
                return null;
            }
            string[] AllLine = File.ReadAllLines(textFile);
            return AllLine;
        }

        public string[] ReadAllLine()
        {
            FileStream fs = null;
            if (!File.Exists(textFile))
            {
                using (fs = File.Create(textFile)) { }
                return null;
            }
            string[] AllLine = File.ReadAllLines(textFile);
            return AllLine;
        }

        public string ReadFirstLine(string SubString, string textFile)
        {
            StringBuilder newfile = new StringBuilder();
            string[] file = File.ReadAllLines(textFile);
            file = RemoveBlank(file);
            if (file == null) return string.Empty;
            //bool flag = false;
            foreach (string line in file)
            {
                if (line.Contains(SubString))
                    return line;
            }
            return string.Empty;
        }

        public string ReadFirstLine(string SubString)
        {
            StringBuilder newfile = new StringBuilder();
            string[] file = File.ReadAllLines(textFile);
            file = RemoveBlank(file);
            if (file == null) return string.Empty;
            //bool flag = false;
            foreach (string line in file)
            {
                if (line.Contains(SubString))
                    return line;
            }
            return string.Empty;
        }

        public string ReadFirstWordLine(string word)
        {
            StringBuilder newfile = new StringBuilder();
            string[] file = File.ReadAllLines(textFile);
            file = RemoveBlank(file);
            if (file == null) return string.Empty;
            //bool flag = false;
            foreach (string line in file)
            {
                char[] delimiters = new char[] { '\t', '\r', '\n', ';', '!', ':', ',', ' ' };
                string[] words = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                words = RemoveBlank(words);
                if (words.Contains(word))
                    return line;
            }
            return string.Empty;
        }

        //Update Line in text File with Header Line
        public void UpdateLineTextFile(string OldHeaderString, string NewLineString, string textFile)
        {
            StringBuilder newfile = new StringBuilder();
            string temp = "";
            string[] file = File.ReadAllLines(textFile);
            foreach (string line in file)
            {
                if (line.Contains(OldHeaderString))
                {
                    temp = line.Replace(line, NewLineString);
                    newfile.Append(temp + "\r\n");
                }
                else
                    newfile.Append(line + "\r\n");
            }
            File.WriteAllText(textFile, newfile.ToString());
        }
        //Delete a Line in text file with string
        public void DeleteFirstLine(string SubString)
        {
            StringBuilder newfile = new StringBuilder();
            string[] file = File.ReadAllLines(textFile);
            file = RemoveBlank(file);
            if (file.Length == 0) return;
            bool flag = false;
            foreach (string line in file)
            {
                if (!line.Contains(SubString) || flag)
                    newfile.Append(line + "\r\n");
                else
                    flag = true;
            }
            File.WriteAllText(textFile, newfile.ToString());
        }
        public void DeleteFirstLine(string SubString, string textFile)
        {
            StringBuilder newfile = new StringBuilder();
            string[] file = File.ReadAllLines(textFile);
            file = RemoveBlank(file);
            if (file.Length == 0) return;
            bool flag = false;
            foreach (string line in file)
            {
                if (!line.Contains(SubString) || flag)
                    newfile.Append(line + "\r\n");
                else
                    flag = true;
            }
            File.WriteAllText(textFile, newfile.ToString());
        }

        //Delete a Line in text file with two SubString
        public void DeleteFirst2String(string SubString1, string SubString2)
        {
            StringBuilder newfile = new StringBuilder();
            string[] file = File.ReadAllLines(textFile);
            file = RemoveBlank(file);
            if (file == null) return;
            bool flag = false;
            foreach (string line in file)
            {
                if ((!line.Contains(SubString1)&&!line.Contains(SubString2)) || flag)
                    newfile.Append(line + "\r\n");
                else
                    flag = true;
            }
            File.WriteAllText(textFile, newfile.ToString());
        }

        //Delete a Line in text file with two SubString
        public void DeleteFirst2String(string SubString1, string SubString2, string textFile)
        {
            StringBuilder newfile = new StringBuilder();
            string[] file = File.ReadAllLines(textFile);
            file = RemoveBlank(file);
            if (file.Length == 0) return;
            bool flag = false;
            foreach (string line in file)
            {
                if ((!line.Contains(SubString1) && !line.Contains(SubString2)) || flag)
                    newfile.Append(line + "\r\n");
                else
                    flag = true;
            }
            File.WriteAllText(textFile, newfile.ToString());
        }
        //Delete All Line in text file with string
        public void DeleteLine(string SubString)
        {
            StringBuilder newfile = new StringBuilder();

            string[] file = File.ReadAllLines(textFile);
            file = RemoveBlank(file);
            if (file.Length == 0) return;
            foreach (string line in file)
            {
                if (!line.Contains(SubString))
                    newfile.Append(line + "\r\n");
            }
            File.WriteAllText(textFile, newfile.ToString());
        }
        public void DeleteLine(string SubString, string textFile)
        {
            StringBuilder newfile = new StringBuilder();

            string[] file = File.ReadAllLines(textFile);
            file = RemoveBlank(file);
            if (file.Length == 0) return;
            foreach (string line in file)
            {
                if (!line.Contains(SubString))
                    newfile.Append(line + "\r\n");
            }
            File.WriteAllText(textFile, newfile.ToString());
        }

        //Delete All Line in text file with two SubStrings
        public void DeleteAll2String(string SubString1, string SubString2)
        {
            StringBuilder newfile = new StringBuilder();

            string[] file = File.ReadAllLines(textFile);
            file = RemoveBlank(file);
            if (file.Length == 0) return;
            foreach (string line in file)
            {
                if (!line.Contains(SubString1) && !line.Contains(SubString2))
                    newfile.Append(line + "\r\n");
            }
            File.WriteAllText(textFile, newfile.ToString());
        }
        //Delete All Line in text file with two SubStrings
        public void DeleteAll2String(string SubString1, string SubString2, string textFile)
        {
            StringBuilder newfile = new StringBuilder();

            string[] file = File.ReadAllLines(textFile);
            file = RemoveBlank(file);
            if (file.Length == 0) return;
            foreach (string line in file)
            {
                if (!line.Contains(SubString1) && !line.Contains(SubString2))
                    newfile.Append(line + "\r\n");
            }
            File.WriteAllText(textFile, newfile.ToString());
        }
        //Update index
        public void UpdateIndex(string textFile)
        {
            StringBuilder newfile = new StringBuilder();
            string[] file = File.ReadAllLines(textFile);
            int len = file.Length;
            int count = 0;
            foreach (string line in file)
            {
                count++;
                char[] delimiters = new char[] { '\t', '\r', '\n', ';', '!', ':', ',', ' ' };
                string[] words = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                words[0] = count.ToString();
                string nline = StrArr2String(words);
                newfile.Append(nline + "\r\n");
            }
            File.WriteAllText(textFile, newfile.ToString());
        }
        public void UpdateIndex()
        {
            StringBuilder newfile = new StringBuilder();
            string[] file = File.ReadAllLines(textFile);
            int len = file.Length;
            int count = 0;
            foreach (string line in file)
            {
                count++;
                char[] delimiters = new char[] { '\t', '\r', '\n', ';', '!', ':', ',', ' ' };
                string[] words = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                words[0] = count.ToString();
                string nline = StrArr2String(words);
                newfile.Append(nline + "\r\n");
            }
            File.WriteAllText(textFile, newfile.ToString());
        }
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
#endregion

#region Các phương thức cũ, chưa cập nhật và chưa kiểm tra

        //Read a line in text file
        public string ReadRow(string pathName)
        {
            string row = string.Empty;
            using (FileStream fs = File.Open(pathName, FileMode.Open, FileAccess.Read))
            {
                using (BufferedStream bs = new BufferedStream(fs))
                {
                    using (StreamReader sr = new StreamReader(bs))
                    {
                        row = sr.ReadLine();
                    }
                }
            }
            return row;
        }
        //Read All line in text file
        public string[][] ReadAllRows(string pathName)
        {
            List<string[]> nRows = new List<string[]>();
            using (FileStream fs = File.Open(pathName, FileMode.Open, FileAccess.Read))
            {
                using (BufferedStream bs = new BufferedStream(fs))
                {
                    using (StreamReader sr = new StreamReader(bs))
                    {
                        //bool readOutput = false;
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            line = line.Trim();
                            char[] delimiters = new char[] { ' ', '\t', '\r', '\n', ';', '!', ':', ',' };
                            string[] words = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                            nRows.Add(words);
                        }
                    }
                }
            }
            return nRows.ToArray();
        }
        public string[] ReadRowtoArray(string pathName)
        {
            string row = string.Empty;



            using (FileStream fs = File.Open(pathName, FileMode.Open, FileAccess.Read))
            {
                using (BufferedStream bs = new BufferedStream(fs))
                {
                    using (StreamReader sr = new StreamReader(bs))
                    {
                        //List<double> row = new List<double>();
                        row = sr.ReadLine();

                        //while ((line = sr.ReadLine()) != null)
                        //{

                        //}
                    }
                }
            }
            row = row.Trim();
            char[] delimiters = new char[] { ' ', '\t', '\r', '\n', ';', '!', ':', ',' };
            string[] words = row.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            return words;
        }
        public double[] ReadRowtoNumeric(string pathName)
        {
            string[] line = ReadRowtoArray(pathName);
            List<double> list = new List<double>();
            
            for (int i = 0; i < line.Length; i++)
            {
                double tmp = ToDecimal(line[i]);
                list.Add(tmp);
            }
            return list.ToArray();
        }

        public double[][] ReadAllRowtoNumeric(string pathName)
        {
            List<double[]> nRows = new List<double[]>();
            
            using (FileStream fs = File.Open(pathName, FileMode.Open, FileAccess.Read))
            {
                using (BufferedStream bs = new BufferedStream(fs))
                {
                    using (StreamReader sr = new StreamReader(bs))
                    {
                        //bool readOutput = false;
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            line = line.Trim();
                            char[] delimiters = new char[] { ' ', '\t', '\r', '\n', ';', '!', ':', ',' };
                            string[] words = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                            double[] tmp = new double[words.Length];
                            for (int i = 0; i < words.Length; i++)
                            {
                                tmp[i] = ToDecimal(words[i]);
                            }

                            nRows.Add(tmp);
                        }
                    }
                }
            }
            return nRows.ToArray();
        }

        //Read n line in text file
        public string[] ReadNline(string filename, int n)
        {
            string[] ListLines = new string[n];
            //Load our text file
            TextReader tr = new StreamReader(filename);
            //Read the number of lines and put them in the array
            for (int i = 0; i < n; i++)
            {
                ListLines[i] = tr.ReadLine();
            }
            tr.Close();
            return ListLines;
        }

        //Read n line not blank
        public string[] ReadNlineNotBlank(string filename, int n)
        {
            string[] ListLines = new string[n];
            string line = null;
            //Load our text file
            TextReader tr = new StreamReader(filename);
            //Read the number of lines and put them in the array
            int i = 0;
            while (i < n)
            {
                line = tr.ReadLine();
                if (line.Trim() != "")
                {
                    ListLines[i] = line;
                    i++;
                }
            }
            tr.Close();
            return ListLines;
        }

        //Read the header file
        public string[] ReadHeaderFile(string filename, string endheader)
        {
            List<string> ListLines = new List<string>();
            string line = "";
            TextReader tr = new StreamReader(filename);
           // int i = 0;
            line = tr.ReadLine();
            while (line != endheader)
            {
                if (line.Trim() != "") ListLines.Add(line);
                line = tr.ReadLine();
            }
            tr.Close();
            return ListLines.ToArray();
        }
        //Read the content file
        public string[] ReadContentFile(string filename, string endheader)
        {
            string[] AllLine = File.ReadAllLines(filename);
            int Len = AllLine.Length;
            
            int i = 0;
            
            while (AllLine[i] != endheader) i++;
            
            List<string> content = new List<string>();
            for (int j = i + 1; j < Len; j++)
                if (AllLine[j] != "")
                    content.Add(AllLine[j]);
            return content.ToArray();
        }
        //Read a line d in text file
        public string ReadiLine(string filename, int d)
        {
            string line = string.Empty;
            //string[] AllLine = File.ReadAllLines(filename);
            //if (d < AllLine.Length)
            //    line = AllLine[d];
            StreamReader tr = new StreamReader(filename);
           
            int i = 1;
            while (((line = tr.ReadLine()) != null) && i < d) i++;
            return line;
        }
       
       
        
        

        //Get TextBlock k in text file
        public string GetBlock(string filename, int k, int nLine)
        {
            string BlockText = string.Empty;
            string[] ListBlocktext = TextfileToBlock(filename, nLine);
            BlockText = ListBlocktext[k];
            return BlockText;
        }
        //Count word in TextBlock
        private int CountWords(string TextBlock)
        {
            TextBlock = TextBlock.Trim();
            char[] delimiters = new char[] { ' ', '\r', '\n', '.', ';', '!', ':', ',' };
            string[] words = TextBlock.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            return words.Length;
        }
        //Step 1: text file to List TextBlock
        public string[] TextfileToBlock(string filename, int nLine)
        {
            if (!File.Exists(filename)) return null;
            List<string> ListTextBlock = new List<string>();
            int iLine = 0;
            string TextBlock = string.Empty;
            foreach (string line in File.ReadAllLines(filename))
            {
                numLines++;
                numLines += CountWords(line);
                if (iLine == nLine)
                {
                    ListTextBlock.Add(TextBlock);
                    iLine = 0;
                    TextBlock = string.Empty;
                }
                else
                {
                    TextBlock += line + "\r\n";
                    iLine++;
                }
            }
            if (iLine > 0)
            {
                ListTextBlock.Add(TextBlock);
            }
            return ListTextBlock.ToArray();
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////     Các phương thức để xử lý cây H-Tree                                                         ///////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////


      





    }
    #endregion

}
