using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace TextFile
{
    public class TextFileClass
    {
        private string filename;
        public long NumberOfWords { get; set; }
        public long NumberOfLines { get; set; }
        public long NumberOfSignatures { get; set; }
        public string Filename
        {
            set { filename = value; }
            get { return filename; }
        }

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
                double tmp = Double.Parse(line[i]);
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
                                tmp[i] = Double.Parse(words[i]);
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
        public string[] ReadAllLine(string filename)
        {
            string[] AllLine = File.ReadAllLines(filename);
            return AllLine;
        }
        public void WriteLineTextFile(string str, string filename)
        {
            // Create a Text File
            FileStream fs = null;
            if (!File.Exists(filename))
            {
                using (fs = File.Create(filename)) { }
            }

            // Append to a Text File
            if (File.Exists(filename))
            {
                using (StreamWriter sw = File.AppendText(filename))
                {
                    sw.WriteLine(str);
                }
            }
        }

        //Update Line in text File with Header Line
        public void UpdateLineTextFile(string OldHeaderString, string NewLineString, string filename)
        {
            StringBuilder newfile = new StringBuilder();
            string temp = "";
            string[] file = File.ReadAllLines(filename);
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
            File.WriteAllText(filename, newfile.ToString());
        }

        //Delete Line in text file with string
        public void DeleteLineTextFile(string SubString, string filename)
        {
            StringBuilder newfile = new StringBuilder();
           
            string[] file = File.ReadAllLines(filename);
            foreach (string line in file)
            {
                if (!line.Contains(SubString))
                    newfile.Append(line + "\r\n");
            }
            File.WriteAllText(filename, newfile.ToString());
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
                NumberOfLines++;
                NumberOfWords += CountWords(line);
                if (iLine == nLine)
                {
                    ListTextBlock.Add(TextBlock);
                    iLine = 0;
                    TextBlock = string.Empty;
                    NumberOfSignatures++;
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
                NumberOfSignatures++;
            }
            return ListTextBlock.ToArray();
        }     
    }
}
