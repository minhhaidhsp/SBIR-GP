using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBIR
{
    class ClusterTree:ClusterNode
    {
        //Các thuộc tính trực tiếp
        private ClusterNode root = new ClusterNode(true, true); //Thông tin nút gốc
        //Các thuộc tính liên kết
        private string fileRoot = string.Empty;
        //Các phương thức khởi tạo, ban đầu Root được khỏi tạo vừa là gốc và vừa là Leaf.
        public ClusterTree()
        {
            //Các thuộc tính trực tiếp
            this.root = new ClusterNode(true, true); //Thông tin nút gốc
            //Các thuộc tính liên kết
            this.fileRoot = string.Empty;
        }
        public ClusterTree(ClusterNode root, string fileRoot)
        {
            //Các thuộc tính trực tiếp
            this.root = root; //Thông tin nút gốc
            //Các thuộc tính liên kết
            this.fileRoot = fileRoot;
        }
        public ClusterTree(ClusterTree CT)
        {
            //Các thuộc tính trực tiếp
            this.root = CT.Root; //Thông tin nút gốc
            //Các thuộc tính liên kết
            this.fileRoot = CT.FileRoot;
        }
        public ClusterNode Root
        {
            get { return root; }
            set { root = value; }
        }
        public string FileRoot
        {
            get { return fileRoot; }
            set { fileRoot = value; }
        }
        public void CreateClusterTree(string fileListED)
        { 
            //Nếu nút gốc chưa tồn tại thì tạo ra một nút gốc mới
            string dir = Path.GetDirectoryName(fileListED) + "\\";
            this.fileRoot = dir + "Root.txt"; 
            FileStream fs = null;
            if (!File.Exists(fileRoot))
            {
                using (fs = File.Create(fileRoot)) { }
                ClusterNode Root = new ClusterNode(true, true);
                Root.SaveCluster(fileRoot);
            }
            ElementData ED = new ElementData();
            List<ElementData> listED = ED.getListElementData(fileListED);
            if(listED == null) return;

            foreach(ElementData ed in listED)
            {
                AddEDfromRootByCenter(ed, fileRoot);
            }
        }
        public List<ElementData> SearchCTreetoList(List<double> feature, string Root)
        {
            List<ElementData> listED = new List<ElementData>();
            //Tìm hướng đi tốt nhất theo vector tâm
            string file = Root;
            string fileEC;
            string dir = Path.GetDirectoryName(Root) + "\\";
           
            ElementCenter EC = new ElementCenter();
            ElementData ED = new ElementData();
            Stack<string> STACK = new Stack<string>();
            STACK.Push(Root);
            while (STACK.Count > 0)
            {
                file = STACK.Pop();
                if (getIsLeaf(file) == false)
                {
                    fileEC = dir + getFileEC(file);
                    EC = EC.getMinEC(feature, fileEC);
                    STACK.Push(dir + EC.FileNameChild);
                }
            }
            string fileED = dir + getFileED(file);
            return ED.getListElementData(fileED);
        }
        public string SearchCTreetoFileED(List<double> feature, string Root)
        {
            //List<ElementData> listED = new List<ElementData>();
            //Tìm hướng đi tốt nhất theo vector tâm
            string file = Root;
            string fileEC;
            string dir = Path.GetDirectoryName(Root) + "\\";

            ElementCenter EC = new ElementCenter();
            ElementData ED = new ElementData();
            Stack<string> STACK = new Stack<string>();
            STACK.Push(Root);
            while (STACK.Count > 0)
            {
                file = STACK.Pop();
                if (getIsLeaf(file) == false)
                {
                    fileEC = dir + getFileEC(file);
                    EC = EC.getMinEC(feature, fileEC);
                    STACK.Push(dir + EC.FileNameChild);
                }
            }
            string fileED = dir + getFileED(file);
            return fileED;
        }

        public string SearchCTreetoFileEDSecond(List<double> feature, string Root)
        {
            //List<ElementData> listED = new List<ElementData>();
            //Tìm hướng đi tốt nhất theo vector tâm
            string file = Root;
            string fileEC = string.Empty ;
            string dir = Path.GetDirectoryName(Root) + "\\";

            ElementCenter EC = new ElementCenter();
            ElementData ED = new ElementData();
            Stack<string> STACK = new Stack<string>();
            STACK.Push(Root);
           
            while (STACK.Count > 0)
            {
                file = STACK.Pop();
                if (getIsLeaf(file) == false)
                {
                    fileEC = dir + getFileEC(file);
                    EC = EC.getMinEC(feature, fileEC);
                    STACK.Push(dir + EC.FileNameChild);
                    
                }
            }

            EC = EC.getMinECSecond(feature, fileEC, Path.GetFileName(file));
            string fileED = dir + getFileED(dir + EC.FileNameChild);
            return fileED;
        }

        public string SearchCTreetoLeaf(List<double> feature, string Root)
        {
            //List<ElementData> listED = new List<ElementData>();
            //Tìm hướng đi tốt nhất theo vector tâm
            string file = Root;
            string fileEC;
            string dir = Path.GetDirectoryName(Root) + "\\";

            ElementCenter EC = new ElementCenter();
            ElementData ED = new ElementData();
            Stack<string> STACK = new Stack<string>();
            STACK.Push(Root);
            while (STACK.Count > 0)
            {
                file = STACK.Pop();
                if (getIsLeaf(file) == false)
                {
                    fileEC = dir + getFileEC(file);
                    EC = EC.getMinEC(feature, fileEC);
                    STACK.Push(dir + EC.FileNameChild);
                }
            }
            return file;
        }



        /// ///////////////////////////////////////////////////////////////////////////////////////////
        ///                             GP Tree                     
        ////////////////////////////////////////////////////////////////////////////////////////////////

        public string SearchGPTreeToLeaf(List<double> feature, string Root)
        {
            //List<ElementData> listED = new List<ElementData>();
            //Tìm hướng đi tốt nhất theo vector tâm
            string file = Root;
            string fileEC;
            string dir = Path.GetDirectoryName(Root) + "\\";
            ElementCenter EC = new ElementCenter();
            ElementData ED = new ElementData();
            Stack<string> STACK = new Stack<string>();
            STACK.Push(Root);
            while (STACK.Count > 0)
            {
                file = STACK.Pop();
                Console.WriteLine(file);
                if (getIsLeaf(file) == false)
                {
                    fileEC = dir + getFileEC(file);
                    EC = EC.getMinEC_GPTree(feature, fileEC);
                    STACK.Push(dir + EC.FileNameChild);
                }
            }
            return file;
        }

        public string SearchGPTreeToLeafSecond(List<double> feature, string Root)
        {
            //List<ElementData> listED = new List<ElementData>();
            //Tìm hướng đi tốt nhất theo vector tâm
            string file = Root;
            string fileEC = string.Empty;
            string dir = Path.GetDirectoryName(Root) + "\\";
            ElementCenter EC = new ElementCenter();
            ElementData ED = new ElementData();
            Stack<string> STACK = new Stack<string>();
            STACK.Push(Root);

            while (STACK.Count > 0)
            {
                file = STACK.Pop();
                if (getIsLeaf(file) == false)
                {
                    fileEC = dir + getFileEC(file);
                    EC = EC.getMinEC_GPTree(feature, fileEC);
                    STACK.Push(dir + EC.FileNameChild);

                }
            }
            List<ElementData> Res = ED.getListElementDataGPTree(dir + getFileED(file));
            EC = EC.getMinECSecondGPTree(feature, fileEC, Path.GetFileName(file));
            string fileED = dir + getFileED(dir + EC.FileNameChild);
            //List<ElementData> ResSecond = ED.getListElementDataGPTree(fileED);
            //Res = ED.AddListED(Res, ResSecond);
            return fileED;
        }

        public List<ElementData> SearchGPTreeToLeafThird(List<double> feature, string Root)
        {
            //List<ElementData> listED = new List<ElementData>();
            //Tìm hướng đi tốt nhất theo vector tâm
            string file = Root;
            string fileEC = string.Empty;
            string dir = Path.GetDirectoryName(Root) + "\\";
            ElementCenter EC = new ElementCenter();
            ElementData ED = new ElementData();
            Stack<string> STACK = new Stack<string>();
            STACK.Push(Root);

            while (STACK.Count > 0)
            {
                file = STACK.Pop();
                if (getIsLeaf(file) == false)
                {
                    fileEC = dir + getFileEC(file);
                    EC = EC.getMinEC_GPTree(feature, fileEC);
                    STACK.Push(dir + EC.FileNameChild);

                }
            }
            fileEC = dir + getFileEC(dir + getFileParent(file));
            List<ElementCenter> ListEC = EC.getListElementCenterGPTree(fileEC);
            List<ElementData> Res = new List<ElementData>();
            foreach (ElementCenter ec in ListEC)
            {
                if(ec.IsNextLeaf == true)
                    Res =  ED.AddListED(Res, ED.getListElementDataGPTree(dir + getFileED(dir + ec.FileNameChild)));
            }    
          
            return Res;
        }


        //Hàm sử dụng KNN để tìm ra ED có độ tương tự gần nhất với vec tở đầu vào và gán phân lớp cho vector đầu vào
        //Đồng thời lấy ra danh sách các nút có cùng lớp đại diện
        public Tuple<string, List<string>>  getClassAndNeighborLevelTwo(List<double> feature, string Root, string fileLeaf)
        {
            string file = Root;
            string fileEC = string.Empty;
            string dir = Path.GetDirectoryName(Root) + "\\";
            ElementData ED = new ElementData();
            ElementData EDMin = ED.getMinED_GPTree(feature, dir + getFileED(fileLeaf));
            string label = EDMin.ListClass.ElementAt(0);
            List<string> listLabelNeighbor = new List<string>();
            

            ElementCenter EC = new ElementCenter();
            List<ElementCenter> listEC = new List<ElementCenter>();
            Stack<string> STACK = new Stack<string>();
            STACK.Push(Root);
            while (STACK.Count > 0)
            {
                file = STACK.Pop();
                if (getIsLeaf(file) == false)
                {
                    fileEC = dir + getFileEC(file);
                    listEC = EC.getListElementCenterGPTree(fileEC);
                    foreach(ElementCenter ec in listEC)
                    {
                        if (ec.IsNextLeaf == true)
                            if(ec.MaxClass == label)
                                listLabelNeighbor.Add(ec.FileNameChild);
                        else
                            STACK.Push(dir + ec.FileNameChild);
                    }
                }
            }
            return new Tuple<string, List<string>>(label, listLabelNeighbor);
        }
    }
}
