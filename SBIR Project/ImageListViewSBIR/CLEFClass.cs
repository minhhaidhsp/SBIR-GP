using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextFile;

namespace ImageCLEF
{
    class CLEFClass
    {
        public List<double[]>[] ClusterCLEF = new List<double[]>[41];
        public List<double[]>[] Clustering(string pathNameAllFeature)
        {
            TextFile.TextFileClass tf = new TextFile.TextFileClass();
            List<double[]>[] Clusters = new List<double[]>[41];
            for (int i = 0; i < Clusters.Length; i++)
            {
                Clusters[i] = new List<double[]>();
            }
            double[][] AllFeature = tf.ReadAllRowtoNumeric(pathNameAllFeature);
            int cl = 0;

            for (int i = 0; i < AllFeature.Length; i++)
            {
                cl = ((int)AllFeature[i][0]) / 1000;
                //if (cl < 0 || cl > 40)
                //    MessageBox.Show(cl.ToString());
                Clusters[cl].Add(AllFeature[i]);
            }
            return Clusters;
        }

        public List<double[]>[] ClusteringSubjecs()
        {
            TextFile.TextFileClass tf = new TextFile.TextFileClass();
            string pathFoldes = @"../../../ImageDBs/ImageCLEF/SAIAPR TC-12 Benchmark/benchmark/saiapr_tc-12";

            List<double[]>[] Clusters = new List<double[]>[41];
            for (int i = 0; i < Clusters.Length; i++)
            {
                Clusters[i] = new List<double[]>();
            }

            for (int i = 0; i <= 40; i++)
            {
                string textFloder = "";
                if (i < 10) textFloder = "0" + i.ToString();
                else textFloder = i.ToString();
                string filename = pathFoldes + "/" + textFloder + "/" + "features.txt";
                if (File.Exists(filename))
                {
                    double[][] Features = tf.ReadAllRowtoNumeric(filename);
                    for (int j = 0; j < Features.Length; j++)
                    {
                        Clusters[i].Add(Features[j]);
                    }
                }
            }
            return Clusters;
        }
        public string[] GetAnnotation(string PathAnoImgCLEF)
        {
            TextFile.TextFileClass tf = new TextFile.TextFileClass();
            string[] Ano = tf.ReadAllLine(PathAnoImgCLEF);
            List<string> res = new List<string>();
            string str1 = Ano[1].Replace("<DOCNO>", ""); str1 = str1.Replace("</DOCNO>", ""); str1 = "Image annotation name: " + str1; res.Add(str1);
            string str2 = Ano[2].Replace("<TITLE>", ""); str2 = str2.Replace("</TITLE>", ""); str2 = "Image subject: " + str2; res.Add(str2);
            string str3 = Ano[3].Replace("<DESCRIPTION>", ""); str3 = str3.Replace("</DESCRIPTION>", ""); str3 = "Image descriptions: " + str3; res.Add(str3);
            string str4 = Ano[4].Replace("<NOTES>", ""); str4 = str4.Replace("</NOTES>", ""); str4 = "Image notes: " + str4; res.Add(str4);
            string str5 = Ano[5].Replace("<LOCATION>", ""); str5 = str5.Replace("</LOCATION>", ""); str5 = "Image locations: " + str5; res.Add(str5);
            string str6 = Ano[6].Replace("<DATE>", ""); str6 = str6.Replace("</DATE>", ""); str6 = "Image date: " + str6; res.Add(str6);
            string str7 = Ano[7].Replace("<IMAGE>", ""); str7 = str7.Replace("</IMAGE>", ""); str7 = "Image name: " + str7; res.Add(str7);

            return res.ToArray();
        }
        double[][] GetFeatureName(double[][] AllImg, double nName)
        {
            List<double[]> Features = new List<double[]>();

            for (int i = 0; i < AllImg.Length; i++)
            {
                if (AllImg[i][0] == nName)
                {
                    List<double> row = new List<double>();
                    for (int j = 2; j < AllImg[i].Length - 1; j++)
                        row.Add(AllImg[i][j]);
                    Features.Add(row.ToArray());
                }
            }
            return Features.ToArray();
        }

        //public double[] GetGroundClass(double[][] Testing, double nName)
        //{
        //    List<double> res = new List<double>();
        //    for (int i = 0; i < Testing.Length; i++)
        //        if (Testing[i][0] == nName)
        //            res.Add(Testing[i][Testing[i].Length - 1]);
        //    return res.ToArray();
        //}
        double Euclide(double[] a, double[] b)
        {
            double sum = 0.0;
            for (int i = 0; i < a.Length; i++)
                sum += (a[i] - b[i]) * (a[i] - b[i]);
            return Math.Sqrt(sum);
        }

        double Dmin(double[] a, double[][] B)
        {
            double d = Euclide(a, B[0]);
            for (int i = 0; i < B.Length; i++)
            {
                double tmp = Euclide(a, B[i]);
                if (d > tmp)
                    d = tmp;
            }
            return d;
        }

        double SimilarityMeasure(double[][] A, double[][] B)
        {
            double dis = 0.0;
            for (int i = 0; i < A.Length; i++)
                dis += Dmin(A[i], B);
            return dis / A.Length;
        }

        double[][] GetNsimilarImg(double[][] AllFeature, int n, int nName)
        {
            double[] GroundClass = GetGroundClass(AllFeature, nName);
            double[] ImgSet = GetImageNameInGroudTruth(AllFeature, GroundClass);
            double[][] nNameFeature = GetFeatureName(AllFeature, nName);
            List<double[]> DisImag = new List<double[]>();
            for (int i = 0; i < ImgSet.Length; i++)
            {
                List<double> row = new List<double>();
                double[][] A = GetFeatureName(AllFeature, ImgSet[i]);
                row.Add(ImgSet[i]); row.Add(SimilarityMeasure(A, nNameFeature));
                DisImag.Add(row.ToArray());
            }
            var orderResult = DisImag.OrderBy(x => x[1]).ToList();
            List<double[]> res = new List<double[]>();
            for (int i = 0; i < n; i++)
                res.Add(orderResult[i]);
            return res.ToArray();
        }
        public double[] GetGroundClass(double[][] Testing, double nName)
        {
            List<double> res = new List<double>();
            for (int i = 0; i < Testing.Length; i++)
                if (Testing[i][0] == nName)
                    res.Add(Testing[i][Testing[i].Length - 1]);
            return res.ToArray();
        }

        bool ChkInArr(double[] Arr, double a)
        {
            for (int i = 0; i < Arr.Length; i++)
            {
                if (Arr[i] == a)
                    return true;
            }
            return false;
        }

        int Count(double[] Arr, double a)
        {
            int dem = 0;
            for (int i = 0; i < Arr.Length; i++)
            {
                if (Arr[i] == a)
                    dem++;
            }
            return dem;
        }
        double[] GetDistinctClass(double[] GroundClass)
        {
            List<double> res = new List<double>();
            for (int i = 0; i < GroundClass.Length; i++)
            {
                if (ChkInArr(res.ToArray(), GroundClass[i]) == false)
                    res.Add(GroundClass[i]);
            }
            return res.ToArray();
        }
        double[] GetImageNameInGroudTruth(double[][] AllFeature, double[] GroundClass)
        {
            List<double> ImgSet = new List<double>();
            for (int i = 0; i < AllFeature.Length; i++)
                if (ChkInArr(GroundClass, AllFeature[i][29]))
                    ImgSet.Add(AllFeature[i][0]);
            return GetDistinctClass(ImgSet.ToArray());
        }

        public string GetPathImgCLEF(double nName)
        {
            return @"../../../ImageDBs/ImageCLEF/SAIAPR TC-12 Benchmark/benchmark/saiapr_tc-12/" + GetNameImg(nName);
        }

        public string GetPathSegImgCLEF(double nName)
        {
            return @"../../../ImageDBs/ImageCLEF/SAIAPR TC-12 Benchmark/benchmark/saiapr_tc-12/" + GetLastFolderName(nName) + "/" + "segmented_images" + "/" + nName.ToString() + ".jpg";
        }

        public string GetPathAnoImgCLEF(double nName)
        {
            return @"../../../ImageDBs/ImageCLEF/SAIAPR TC-12 Benchmark/benchmark/branch/annotations/" + GetLastFolderName(nName) + "/" + nName.ToString() + ".eng";
        }
        public string GetNameImg(double nName)
        {
            int q = ((int)nName) / 1000;
            //int r = Int32.Parse(nName) % 1000;
            string name = string.Empty;
            if (q < 10)
                name = "0" + q.ToString() + "/" + nName.ToString() + ".jpg";
            else
                name = q.ToString() + "/" + nName.ToString() + ".jpg";
            return name;
        }

        public string GetLastFolderName(double nName)
        {
            int q = ((int)nName) / 1000;
            //int r = Int32.Parse(nName) % 1000;
            string name = string.Empty;
            if (q < 10)
                name = "0" + q.ToString();
            else
                name = q.ToString();
            return name;
        }

        public string GetLastFolderName(string path)
        {

            string result = new DirectoryInfo(path).Parent.Name;
            //string result = path.split(Path.DirectorySeparatorChar).Last() 
            return result;
        }

        public string GetFileName(string path)
        {
            string result = Path.GetFileName(path);
            return result;
        }

    }
}
