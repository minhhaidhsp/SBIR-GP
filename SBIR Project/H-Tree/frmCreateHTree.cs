using SBIR;
using SBIR_Project.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SBIR_Project.H_Tree
{
    public partial class frmCreateHTree : Form
    {
        public frmCreateHTree()
        {
            InitializeComponent();
        }
   
        private void btnCreateHTree_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {

                dialog.Filter = "TextFile |*.txt";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    bool isRoot = true; 
                    string fileListEd = dialog.FileName;
                    ////Nếu nút gốc chưa tồn tại thì tạo ra một nút gốc mới
                    ///Tạo file root cùng thư mục với file list ED 
                    //string dir = Path.GetDirectoryName(fileListED) + "\\";
                    //string fileRoot = dir + "Root.txt";

                    string dir = Path.GetDirectoryName(fileListEd) + "\\";
                    GlobalVariable.DefaultHTreePath = dir;
                    Console.WriteLine(GlobalVariable.DefaultHTreePath);
                    //Console.WriteLine(GlobalVariable.DefaultHTreePath);
                    string fileRoot = dir + "Root.txt";
                    FileStream fs = null;
                    if (!File.Exists(fileRoot))
                    {
                        using (fs = File.Create(fileRoot)) { }
                        ClusterNode Root = new ClusterNode(false, isRoot);
                        Root.ClusterId = GlobalVariable.ClusterID;
                        Root.Level = 0;
                        Root.SaveClusterHTree();
                    }

                    ElementData ED = new ElementData();
                    List<ElementData> listED = ED.getListElementDataHTree(fileListEd);
                    Console.WriteLine(listED.Count);
                    if (listED == null) return;
                    //ClusterTree CT = new ClusterTree();
                    //CT.Theta = CT.ToDecimal(txtTheta.Text);
                    //CT.Epcilon = CT.ToDecimal(txtEpcilon.Text);
                    //GlobalVariable.Epsilon = CT.Epcilon;
                    //GlobalVariable.Theta = CT.Theta;

                    GlobalVariable.Epsilon = float.Parse(txtEpcilon.Text.Trim());
                    GlobalVariable.Theta = float.Parse(txtTheta.Text.Trim());

                    progressBarCreateTree.Minimum = 0;
                    progressBarCreateTree.Maximum = listED.Count;
                    int count = 0;
                    progressBarCreateTree.Value = count;
                    foreach (ElementData ed in listED)
                    {
                        //CT.AddEDfromRootByCenterIntoHTree(ed, fileRoot);
                        //INED(ed, isRoot);
                        AddNodeToHTree(ed, isRoot);

                        txtFeature.Text = ed.Feature2String();
                        label2.Text = count.ToString();
                        label4.Text = ed.ImageID;
                        label6.Text = ed.URI;
                        label8.Text = ed.FileNameDescription;
                        label10.Text = ed.FileName;
                        label12.Text = ed.Classes2String(ed.ListClass);
                        Application.DoEvents();
                        progressBarCreateTree.Value = count;
                        count++;
                        isRoot = false;
                    }
                    progressBarCreateTree.Value = listED.Count;
                }
            }
            MessageBox.Show("DONE");
        }

        private void frmCreateHTree_Load(object sender, EventArgs e)
        {

        }
        private void AddNodeToHTree(ElementData ED, bool isRoot, string clusterName = "Root.txt")
        {
            if (isRoot == true)
            {
                //MessageBox.Show("1");
                ClusterNode firstCluster = new ClusterNode(++GlobalVariable.ClusterID, 0, 1, true, false, ED);
                //GlobalVariables.Wlist.ForEach(i => firstCluster.WeightED.Add(Tuple.Create(Tuple.Create(i.Item1, i.Item2), 0)));

                //firstCluster.ListEDClass.Add(ED.ListClass);
                firstCluster.createNewClusterHTree(ED);
            }
            else
            {
                //MessageBox.Show("2");
                //MessageBox.Show(clusterName);
                ClusterNode clusterNode = new ClusterNode();
                //MessageBox.Show(clusterName);
                List<string> listFileChild = clusterNode.GetListFileChildHTree(clusterName);
                int numLevel = clusterNode.getClusterLevelHTree(clusterName);
                int numId = clusterNode.getClusterIDHTree(clusterName);
                //MessageBox.Show("check, numId: " + numId + " " + numLevel);
                if (listFileChild == null)
                {
                    //MessageBox.Show("2.1");
                    ClusterNode cluster = new ClusterNode(++GlobalVariable.ClusterID, numId, numLevel + 1, true, false, ED);
                    cluster.createNewClusterHTree(ED);
                }
                else
                {
                    //MessageBox.Show("2.2");
                    Tuple<double, string> clusterMin = clusterNode.getClusterMinHTree(clusterName, ED);
                    ClusterNode CMMin = new ClusterNode();
                    string CMName = clusterMin.Item2 + ".txt";
                    //MessageBox.Show("hihi," + CMName);
                    CMMin.LoadClusterHTree(CMName);
                    double dMin = clusterMin.Item1;
                    //MessageBox.Show("d: " + dMin + ", name: " + CMName);
                    if (dMin <= GlobalVariable.Epsilon)
                    {
                        //MessageBox.Show("2.2.1");
                        CMMin.AddClusterEDHTree(ED);
                    }
                    else if (dMin > GlobalVariable.Epsilon && dMin <= GlobalVariable.Theta)
                    {
                        //MessageBox.Show("2.2.2");
                        AddNodeToHTree(ED, false, CMName);
                    }
                    else
                    {
                        //MessageBox.Show("2.2.3");
                        //Tạo nút đồng cấp
                        ClusterNode peerCluster = new ClusterNode(++GlobalVariable.ClusterID, CMMin.ParentID, CMMin.Level, true, false, ED); // Tạo nút first
                                                                                                                                             //GlobalVariable.Wlist.ForEach(i => peerCluster.WeightED.Add(Tuple.Create(Tuple.Create(i.Item1, i.Item2), 0)));
                                                                                                                                             //peerCluster.ListEDClass.Add(ED.ListClass);
                        peerCluster.createNewClusterHTree(ED);
                    }
                }
            }
        }

    }
}
