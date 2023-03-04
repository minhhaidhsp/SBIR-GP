using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBIR_Project.Framework
{
    public class GlobalVariable
    {
        //Path folđer cluster node
        private static string defaultHTreePath = @"../../HTree/H-TreeCOREL";
        private static string defaultClusterSimilarPath = @"../../data/ClusterSimilars.txt";
        private static int numOfLeafNode = 1;

        // Hai ngưỡng của cây H-Tree với epsilon < theta
        private static double epsilon = 0.1;
        private static double theta = 0.3;
        private static int clusterID = 0;

        private static List<Tuple<string, string>> wlist = new List<Tuple<string, string>>();
        private static List<Tuple<string, string, string>> _listAllLabels = new List<Tuple<string, string, string>>();
        public static double Epsilon { get => epsilon; set => epsilon = value; }
        public static double Theta { get => theta; set => theta = value; }
        public static int ClusterID { get => clusterID; set => clusterID = value; }
        public static List<Tuple<string, string>> Wlist { get => wlist; set => wlist = value; }
        public static List<Tuple<string, string, string>> ListAllLabels { get => _listAllLabels; set => _listAllLabels = value; }
        public static string DefaultHTreePath { get => defaultHTreePath; set => defaultHTreePath = value; }
        public static string DefaultClusterSimilarPath { get => defaultClusterSimilarPath; set => defaultClusterSimilarPath = value; }
        public static int NumOfLeafNode { get => numOfLeafNode; set => numOfLeafNode = value; }
    }
}
