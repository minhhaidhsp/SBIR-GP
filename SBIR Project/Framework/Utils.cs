using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBIR_Project.Framework
{
    public class Utils
    {
        private static Utils instance;

        public static Utils Instance
        {
            get
            {
                if (instance == null)
                    instance = new Utils();
                return Utils.instance;
            }
            private set { Utils.instance = value; }
        }

        public double EuclideDistance(List<double> vector1, List<double> vector2)
        {
            double euclide = 0.000;
            try
            {
                for (int i = 0; i < vector1.Count; i++)
                {
                    euclide = euclide + (Math.Pow(vector2[i] - vector1[i], 2) / vector1.Count);
                }
               
            }
            catch (Exception e) { }
            return Math.Round(Math.Sqrt(euclide), 3);
        }

        /* Loại bỏ cặp dấu ngoặc trong chuỗi  vd: (abc) => abc */
        public string getString(string str)
        {
            str = str.Trim('\n').Trim('(').Trim(')').Trim(' ');

            return str;
        }
    }
}
