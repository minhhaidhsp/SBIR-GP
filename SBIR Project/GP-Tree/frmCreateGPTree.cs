using SBIR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SBIR_Project.GP_Tree
{
    public partial class frmCreateGPTree : Form
    {
        public frmCreateGPTree()
        {
            InitializeComponent();
        }

        //Hàm dùng để định nghiawx neighbor sau khi tạo cây
        public void defineNeighbor(string fileRoot)
        {
            string dir = Path.GetDirectoryName(fileRoot) + "\\";
            ClusterNode CT = new ClusterNode();
            ElementCenter EC = new ElementCenter();
            List<string> listLeafNode = new List<string>();
            Stack<string> stackNode = new Stack<string>();
            string fileEC = string.Empty;
            string fileNode = string.Empty;
            List<ElementCenter> listEC = new List<ElementCenter>();

            stackNode.Push(fileRoot);
            while (stackNode.Count > 0)
            {
                fileNode = stackNode.Pop();
                if (CT.getIsLeaf(fileNode) == true)
                {
                    listLeafNode.Add(fileNode);
                }
                else
                {
                    fileEC = dir + CT.getFileEC(fileNode);
                    listEC = EC.getListElementCenterGPTree(fileEC);
                    foreach (ElementCenter ec in listEC)
                    {
                        if (ec.IsNextLeaf == true)
                            listLeafNode.Add(dir + ec.FileNameChild);
                        else
                            stackNode.Push(dir + ec.FileNameChild);
                    }
                }
            }

            int numOfLeafNode = listLeafNode.Count;
            progressBarNeighborGPTree.Minimum = 0;
            progressBarNeighborGPTree.Maximum = numOfLeafNode;
            int count = 0;
            progressBarNeighborGPTree.Value = count;
            for (int i = 0; i < numOfLeafNode - 1; i++)
            {
                for (int j = i + 1; j < numOfLeafNode; j++)
                {
                    CT.defineAllNeighbor(listLeafNode.ElementAt(i), listLeafNode.ElementAt(j));
                }
                count++;
                progressBarNeighborGPTree.Value = count;
            }
            progressBarNeighborGPTree.Value = numOfLeafNode;

        }

        private void bntCreateGPTree2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog diag = new OpenFileDialog())
            {
                diag.Filter = "TextFile |*.txt";
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    string fileListED = diag.FileName;
                    //Nếu nút gốc chưa tồn tại thì tạo ra một nút gốc mới
                    string dir = Path.GetDirectoryName(fileListED) + "\\";
                    string fileRoot = dir + "Root.txt";
                    FileStream fs = null;
                    if (!File.Exists(fileRoot))
                    {
                        using (fs = File.Create(fileRoot)) { }
                        ClusterNode Root = new ClusterNode(true, true, 0);
                        Root.SaveClusterGPTree(fileRoot);
                    }
                    ElementData ED = new ElementData();
                    List<ElementData> listED = ED.getListElementDataGPTree(fileListED);
                    if (listED == null) return;

                    ClusterTree CT = new ClusterTree();
                    //Lấy các giá trị về số phần tử nút lá, nút con và ngưỡng láng giềng từ fo
                    CT.MaxED = CT.ToInt32(textBox2.Text);
                    CT.MaxEC = CT.ToInt32(textBox3.Text);
                    CT.MinEDEC = CT.ToInt32(textBox4.Text);
                    CT.Theta = CT.ToDecimal(textBox5.Text);

                    progressBarGPTree.Minimum = 0;
                    progressBarGPTree.Maximum = listED.Count;
                    int count = 0;
                    progressBarGPTree.Value = count;

                    foreach (ElementData ed in listED)
                    {
                        CT.AddEDfromRootByCenterGPTree(ed, fileRoot, CT.Theta);
                        count++;
                        textBox1.Text = ed.Feature2String();
                        label2.Text = count.ToString();
                        label4.Text = ed.ImageID;
                        label6.Text = ed.URI;
                        label8.Text = ed.FileNameDescription;
                        label10.Text = ed.FileName;
                        label12.Text = ed.Classes2String(ed.ListClass);
                        progressBarGPTree.Value = count;
                        Application.DoEvents();
                    }
                    progressBarGPTree.Value = listED.Count;

                    //Xóa các tập tin nút lá dư thừa
                    string fileLeafRename = dir + "RenameLeaf.txt";
                    string fileNodeRename = dir + "RenameNode.txt";
                    TextfileCluster tfc = new TextfileCluster();
                    string[] Lines = tfc.ReadAllLine(fileLeafRename);
                    Lines = tfc.RemoveBlank(Lines);
                    if (Lines != null)
                    {
                        foreach (string line in Lines)
                        {
                            char[] delimiters = new char[] { '\t', '\r', '\n', ';', '!', ':', ',', ' ' };
                            string[] words = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                            if (words[0] != "Root.txt")
                            {
                                string file = dir + words[0];
                                if (File.Exists(file))
                                    File.Delete(file);
                                string fileED = dir + "list" + words[0].Replace(".txt", "") + "ED" + ".txt";
                                if (File.Exists(fileED))
                                    File.Delete(fileED);

                                string fileneighbor1 = dir + "Neighbor1" + words[0].Replace(".txt", "") + ".txt";
                                if (File.Exists(fileneighbor1))
                                    File.Delete(fileneighbor1);
                                string fileneighbor2 = dir + "Neighbor2" + words[0].Replace(".txt", "") + ".txt";
                                if (File.Exists(fileneighbor2))
                                    File.Delete(fileneighbor2);
                                string fileneighbor3 = dir + "Neighbor3" + words[0].Replace(".txt", "") + ".txt";
                                if (File.Exists(fileneighbor3))
                                    File.Delete(fileneighbor3);
                                string fileneighbor4 = dir + "Neighbor4" + words[0].Replace(".txt", "") + ".txt";
                                if (File.Exists(fileneighbor4))
                                    File.Delete(fileneighbor4);
                            }
                        }
                    }
                    //Xóa các nút trong dư thừa
                    string[] LinesNode = tfc.ReadAllLine(fileNodeRename);
                    LinesNode = tfc.RemoveBlank(LinesNode);
                    if (LinesNode != null)
                    {
                        foreach (string lnode in LinesNode)
                        {
                            char[] deli = new char[] { '\t', '\r', '\n', ';', '!', ':', ',', ' ' };
                            string[] wordsNode = lnode.Split(deli, StringSplitOptions.RemoveEmptyEntries);
                            if (wordsNode[0] != "Root.txt")
                            {
                                string fileNode = dir + wordsNode[0];
                                if (File.Exists(fileNode))
                                    File.Delete(fileNode);
                                string fileEC = dir + "list" + wordsNode[0].Replace(".txt", "") + "EC" + ".txt";
                                if (File.Exists(fileEC))
                                    File.Delete(fileEC);
                                string fileNodeneighbor1 = dir + "Neighbor1" + wordsNode[0].Replace(".txt", "") + ".txt";
                                if (File.Exists(fileNodeneighbor1))
                                    File.Delete(fileNodeneighbor1);
                                string fileNodeneighbor2 = dir + "Neighbor2" + wordsNode[0].Replace(".txt", "") + ".txt";
                                if (File.Exists(fileNodeneighbor2))
                                    File.Delete(fileNodeneighbor2);
                            }
                        }
                    }

                    //Định nghĩa neighbor
                    defineNeighbor(fileRoot);
                }
            }
            MessageBox.Show("DONE!");
        }

        private void bntCreateGPTree3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog diag = new OpenFileDialog())
            {
                diag.Filter = "TextFile |*.txt";
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    string fileListED = diag.FileName;
                    //Nếu nút gốc chưa tồn tại thì tạo ra một nút gốc mới
                    string dir = Path.GetDirectoryName(fileListED) + "\\";
                    string fileRoot = dir + "Root.txt";
                    FileStream fs = null;
                    if (!File.Exists(fileRoot))
                    {
                        using (fs = File.Create(fileRoot)) { }
                        ClusterNode Root = new ClusterNode(true, true, 0);
                        Root.SaveClusterGPTree(fileRoot);
                    }
                    ElementData ED = new ElementData();
                    List<ElementData> listED = ED.getListElementDataGPTree(fileListED);
                    if (listED == null) return;

                    ClusterTree CT = new ClusterTree();
                    //Lấy các giá trị về số phần tử nút lá, nút con và ngưỡng láng giềng từ fo
                    CT.MaxED = CT.ToInt32(textBox2.Text);
                    CT.MaxEC = CT.ToInt32(textBox3.Text);
                    CT.MinEDEC = CT.ToInt32(textBox4.Text);
                    CT.Theta = CT.ToDecimal(textBox5.Text);

                    progressBarGPTree.Minimum = 0;
                    progressBarGPTree.Maximum = listED.Count;
                    int count = 0;
                    progressBarGPTree.Value = count;

                    foreach (ElementData ed in listED)
                    {
                        CT.AddEDfromRootByCenterGPTree2(ed, fileRoot, CT.Theta);
                        count++;
                        textBox1.Text = ed.Feature2String();
                        label2.Text = count.ToString();
                        label4.Text = ed.ImageID;
                        label6.Text = ed.URI;
                        label8.Text = ed.FileNameDescription;
                        label10.Text = ed.FileName;
                        label12.Text = ed.Classes2String(ed.ListClass);
                        progressBarGPTree.Value = count;
                        Application.DoEvents();
                    }
                    progressBarGPTree.Value = listED.Count;

                    //Xóa các tập tin nút lá dư thừa
                    string fileLeafRename = dir + "RenameLeaf.txt";
                    string fileNodeRename = dir + "RenameNode.txt";
                    TextfileCluster tfc = new TextfileCluster();
                    string[] Lines = tfc.ReadAllLine(fileLeafRename);
                    Lines = tfc.RemoveBlank(Lines);
                    if (Lines != null)
                    {
                        foreach (string line in Lines)
                        {
                            char[] delimiters = new char[] { '\t', '\r', '\n', ';', '!', ':', ',', ' ' };
                            string[] words = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                            if (words[0] != "Root.txt")
                            {
                                string file = dir + words[0];
                                if (File.Exists(file))
                                    File.Delete(file);
                                string fileED = dir + "list" + words[0].Replace(".txt", "") + "ED" + ".txt";
                                if (File.Exists(fileED))
                                    File.Delete(fileED);

                                string fileneighbor1 = dir + "Neighbor1" + words[0].Replace(".txt", "") + ".txt";
                                if (File.Exists(fileneighbor1))
                                    File.Delete(fileneighbor1);
                                string fileneighbor2 = dir + "Neighbor2" + words[0].Replace(".txt", "") + ".txt";
                                if (File.Exists(fileneighbor2))
                                    File.Delete(fileneighbor2);
                                string fileneighbor3 = dir + "Neighbor3" + words[0].Replace(".txt", "") + ".txt";
                                if (File.Exists(fileneighbor3))
                                    File.Delete(fileneighbor3);
                                string fileneighbor4 = dir + "Neighbor4" + words[0].Replace(".txt", "") + ".txt";
                                if (File.Exists(fileneighbor4))
                                    File.Delete(fileneighbor4);
                            }
                        }
                    }
                    //Xóa các nút trong dư thừa
                    string[] LinesNode = tfc.ReadAllLine(fileNodeRename);
                    LinesNode = tfc.RemoveBlank(LinesNode);
                    if (LinesNode != null)
                    {
                        foreach (string lnode in LinesNode)
                        {
                            char[] deli = new char[] { '\t', '\r', '\n', ';', '!', ':', ',', ' ' };
                            string[] wordsNode = lnode.Split(deli, StringSplitOptions.RemoveEmptyEntries);
                            if (wordsNode[0] != "Root.txt")
                            {
                                string fileNode = dir + wordsNode[0];
                                if (File.Exists(fileNode))
                                    File.Delete(fileNode);
                                string fileEC = dir + "list" + wordsNode[0].Replace(".txt", "") + "EC" + ".txt";
                                if (File.Exists(fileEC))
                                    File.Delete(fileEC);
                                string fileNodeneighbor1 = dir + "Neighbor1" + wordsNode[0].Replace(".txt", "") + ".txt";
                                if (File.Exists(fileNodeneighbor1))
                                    File.Delete(fileNodeneighbor1);
                                string fileNodeneighbor2 = dir + "Neighbor2" + wordsNode[0].Replace(".txt", "") + ".txt";
                                if (File.Exists(fileNodeneighbor2))
                                    File.Delete(fileNodeneighbor2);
                            }
                        }
                    }

                    //Định nghĩa neighbor
                    defineNeighbor(fileRoot);
                }
            }
            MessageBox.Show("DONE!");
        }
    }
}
