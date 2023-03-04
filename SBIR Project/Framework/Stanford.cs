using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBIR
{
    class Stanford
    {
        public string pathImage = @"../../../ImageDBs/StanfordDogs/Images";
        public string pathAno = @"../../../ImageDBs/StanfordDogs/Annotations";
        public string pathURL = @"../../../ImageDBs/StanfordDogs/URL";
        string filewords = @"../../data/words.txt";
        string filegloss = @"../../data/gloss.txt";

        public string getFolderName(string ImgFileName) //withouth extention
        {
            string dirname = string.Empty;
            string[] words = ImgFileName.Split('_');
            string[] subdirs = Directory.GetDirectories(pathImage);
            if(subdirs == null) return string.Empty;
            foreach (string dir in subdirs)
                if (dir.Contains(words[0]))
                    dirname = Path.GetFileName(Path.GetDirectoryName(dir + "/" + ImgFileName));
            return dirname;
        }
        public string getFileImg(string ImgFileName) //withouth extention
        {
            string dirname = getFolderName(ImgFileName);
            string fileImg = pathImage + "/" + dirname + "/" + ImgFileName + ".jpg";
            return fileImg;
        }
        public string getFileAno(string ImgFileName) //withouth extention
        {
            string dirname = getFolderName(ImgFileName);
            string fileAno = pathAno + "/" + dirname + "/" + ImgFileName;
            return fileAno;
        }
        public string getFileURL(string ImgFileName)
        { 
            string foldername = getFolderName(ImgFileName);
            string[] wordname = Splitfirst('-', foldername);
            string filenameURL = wordname[1].Replace("_", " ") + ".txt";
            string dirURL = pathURL + "/" + filenameURL;
            return dirURL;
        }
        public string getFileNameURL(string ImgFileName)
        {
            string foldername = getFolderName(ImgFileName);
            string[] wordname = Splitfirst('-', foldername);
            string filenameURL = wordname[1].Replace("_", " ") + ".txt";
            return filenameURL;
        }
        public string[] Splitfirst(char c, string str)
        {
            List<string> Lw = new List<string>();
            string strtmp = string.Empty;
            int pos = 0;
            for (int i = pos; i < str.Length; i++)
                if (str[i] != c)
                {
                    strtmp += str[i];
                    pos = i;
                }
                else
                    break;
            Lw.Add(strtmp);
            pos++; pos++;
            strtmp = string.Empty;
            for (int i = pos; i < str.Length; i++)
                strtmp += str[i];
            Lw.Add(strtmp);
            return Lw.ToArray();
        }

        public string getFileNameImage(string fileImg)
        {
            //string dirFileImage = Path.GetDirectoryName(fileImg) + "\\";
            string fileName = Path.GetFileNameWithoutExtension(fileImg);
            return fileName;
        }

        public string getClassID(string ImgFileName)
        {
            string[] wordname = Splitfirst('_', ImgFileName);
            return wordname[0];
        }
        public string getClassFolder(string FolderName)
        {
            string[] wordname = Splitfirst('-', FolderName);
            wordname = RemoveBlank(wordname);
            return wordname[1];
        }
        public string getClassName(string ImgFileName)
        {
            string IDClass = getClassID(ImgFileName);
            TextfileCluster tfc = new TextfileCluster(filewords);
            string line = tfc.ReadFirstLine(IDClass);
            string[] words = line.Split('\t');
            words = RemoveBlank(words);
            string ClassName = words[1];
            return ClassName;
        }
        public string getOneClassName(string ImgFileName)
        {
            string foldername = getFolderName(ImgFileName);
            string[] wordname = Splitfirst('-', foldername);
            string ClassName = wordname[1].Trim();
            return ClassName;
        }
        public string getFolderClassName(string FolderName)
        {
            //string foldername = getFolderName(ImgFileName);
            string[] wordname = Splitfirst('-', FolderName);
            string ClassName = wordname[1].Trim();
            return ClassName;
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
    }
}
