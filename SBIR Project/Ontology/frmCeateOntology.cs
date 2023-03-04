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

namespace SBIR
{
    public partial class frmCeateOntology : Form
    {
        string fileClass = @"../../fileOntology/SBIR-Classes.txt";
        string fileOnto = @"../../FileOntology/SBIR-Ontology.n3";
        string fileSubClass = @"../../FileOntology/SBIR-SubClasses.txt";
        string fileImage = @"../../FileOntology/SIBR-Images.txt";
        string fileLiterals = @"../../FileOntology/SBIR-Literals.txt";

        public static Ontology O = new Ontology();

        public frmCeateOntology()
        {
            InitializeComponent();
            //O = new Ontology(fileOnto);
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
        private void btnClassesFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog diag = new OpenFileDialog())
            {
                diag.Filter = "Text File |*.txt";
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    string file = diag.FileName;
                    txtFileClass.Text = file;
                    fileClass = file;

                    //Load các file tự động cùng thư mục
                    string dir = Path.GetDirectoryName(file) + "\\";
                    fileOnto = dir + @"Ontology.n3";
                    fileSubClass = dir + @"SubClasses.txt";
                    fileImage = dir + @"Images.txt";
                    fileLiterals = dir + @"Literals.txt";
                    txtFileSubClasses.Text = fileSubClass;
                    txtImgFIle.Text = fileImage;
                    txtFileLiteral.Text = fileLiterals;

                    try
                    {
                        string text = File.ReadAllText(fileClass);
                        char[] delimiters = new char[] { '@', '\t', '\r', '\n', ',' };
                        string[] Classes = text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                        Classes = RemoveBlank(Classes);
                        if (Classes == null) return;
                        string strClasses = string.Empty;
                        string strClassFile = string.Empty;
                        foreach (string Class in Classes)
                        {
                            string cla = Class.Trim();
                            if (strClasses.Contains(" " + cla.ToUpper() + " ") == false)
                            {
                                strClasses += cla.ToUpper() + " ";
                                strClassFile += cla.ToUpper() + "\n";
                            }
                        }
                        txtClasses.Text = strClasses;
                        TextfileCluster tfc = new TextfileCluster();
                        tfc.WriteNewTextFile(strClassFile, fileClass);

                        DisnableAll();
                        btnCreateOntoClasses.Enabled = true;
                    }
                    catch (IOException)
                    {
                    }
                }
            }
        }

        private void btnCreateOntoClasses_Click(object sender, EventArgs e)
        {
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;

            fileClass = txtFileClass.Text;
            O.CreateOntoClass(fileClass);
            progressBar1.Value = 50;
            
            O.Writer.Save(O.Graph, fileOnto);
            progressBar1.Value = 100;
            
            string text = File.ReadAllText(fileOnto);
            txtOnClasses.Text = text;

            DisnableAll();
            btnSubClassesFile.Enabled = true;
        }

        private void btnCreateOntoSubClasses_Click(object sender, EventArgs e)
        {
            try
            {
                fileSubClass = txtFileSubClasses.Text;
                TextfileCluster tfc = new TextfileCluster();
                string[] Lines = tfc.ReadAllLine(fileSubClass);
                Lines = RemoveBlank(Lines);
                if (Lines == null) return;
                int count = 0;
                progressBar2.Minimum = 0;
                progressBar2.Maximum = Lines.Length;
                char[] delimiters = new char[] { '@', '\t', '\r', '\n', ',', ':' };
                foreach (string line in Lines)
                {
                    string[] Words = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    int len = Words.Length;
                    string superClass = Words[0].Trim();
                    superClass = superClass.ToUpper();
                    for (int i = 1; i < len; i++)
                    {
                        string subClass = Words[i].Trim();
                        subClass = subClass.ToUpper();
                        O.CreateOntoSubClass(superClass, subClass);
                    }
                    count++;
                    progressBar2.Value = count;
                }
                progressBar2.Value = Lines.Length;
            }
            catch (IOException)
            {
            }
            O.Writer.Save(O.Graph, fileOnto);
            string text = File.ReadAllText(fileOnto);
            txtOnClasses.Text = text;

            DisnableAll();
            btnCreateIndividuals.Enabled = true;
        }
        private List<string> getClasses()
        {
            string text = File.ReadAllText(fileClass);
            char[] delimiters = new char[] { '@', '\t', '\r', '\n' };
            string[] Classes = text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            Classes = RemoveBlank(Classes);
            return Classes.ToList();
        }
        private List<string> getInClasses(List<string> Classes)
        {
            List<string> LInClasses = new List<string>();
            string name = string.Empty;
            foreach (string cla in Classes)
            {
                name = "in" + cla;
                LInClasses.Add(name);
            }
            return LInClasses;
        }
        private void btnCreateIndividuals_Click(object sender, EventArgs e)
        {
            progressBar2.Minimum = 0;
            progressBar2.Maximum = 100;
            List<string> LClasses = getClasses();
            List<string> LInClasses = getInClasses(LClasses);
            O.CreateInClass(LClasses, LInClasses);
            progressBar2.Value = 50;
            O.Writer.Save(O.Graph, fileOnto);
            string text = File.ReadAllText(fileOnto);
            txtOnClasses.Text = text;
            progressBar2.Value = 100;

            DisnableAll();
            btnCreateOProperties.Enabled = true;
        }
        private List<string> getOPClasses(List<string> Classes)
        {
            List<string> LOPClasses = new List<string>();
            string name = string.Empty;
            foreach (string cla in Classes)
            {
                name = "op" + cla;
                LOPClasses.Add(name);
            }
            return LOPClasses;
        }
        private void btnCreateOProperties_Click(object sender, EventArgs e)
        {
            progressBar2.Minimum = 0;
            progressBar2.Maximum = 100;
            List<string> LClasses = getClasses();
            progressBar2.Value = 25;
            List<string> LOPClasses = getOPClasses(LClasses);
            progressBar2.Value = 40;
            O.CreateOPClass(LClasses, LOPClasses);
            O.Writer.Save(O.Graph, fileOnto);
            string text = File.ReadAllText(fileOnto);
            txtOnClasses.Text = text;
            progressBar2.Value = 100;

            DisnableAll();
            btnImageFile.Enabled = true;
        }
        private void btnSubClassesFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog diag = new OpenFileDialog())
            {
                diag.Filter = "Text File |*.txt";
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    string file = diag.FileName;
                    txtFileSubClasses.Text = file;
                    fileSubClass = file;

                    DisnableAll();
                    btnCreateOntoSubClasses.Enabled = true;
                }
            }
        }
        private void btnImageFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog diag = new OpenFileDialog())
            {
                diag.Filter = "Text File |*.txt";
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    string file = diag.FileName;
                    txtImgFIle.Text = file;
                    fileImage = file;

                    DisnableAll();
                    btnCreateImgInd.Enabled = true;
                }
            }
        }
        private string getIndividual(string Line)
        {
            string ind = string.Empty;
            char[] delimiters = new char[] { '@', '\t', '\r', '\n', ' ', ':' };
            string[] Words = Line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            ind = "in" + Words[0].Trim().ToUpper();
            return ind;
        }
        private string getOProperty(string Line)
        {
            string op = string.Empty;
            char[] delimiters = new char[] { '@', '\t', '\r', '\n', ' ', ':' };
            string[] Words = Line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            op = "op" + Words[0].Trim().ToUpper();
            return op;
        }
        private List<string> getListImgIndi(string Line)
        {
            List<string> LImg = new List<string>();
            char[] delimiters = new char[] { '@', '\t', '\r', '\n', ' ', ':' };
            string[] Words = Line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            int len = Words.Length;
            for (int i = 1; i < len; i++)
            {
                string Ind = "IMG" + Words[i];
                LImg.Add(Ind);
            }
            return LImg;
        }
        private void btnCreateImgInd_Click(object sender, EventArgs e)
        {
            TextfileCluster tfc = new TextfileCluster();
            string[] Lines = tfc.ReadAllLine(fileImage);
            Lines = RemoveBlank(Lines);
            if (Lines == null) return;
            int count = 0;
            progressBar3.Minimum = 0;
            progressBar3.Maximum = Lines.Length;
            //Ontology O = new Ontology(fileOnto);
            foreach (string line in Lines)
            {
                string strObj = getIndividual(line);
                string strPre = "sbir:" + getOProperty(line);
                List<string> LstrImg = getListImgIndi(line);
                O.CreateListImageIndividual(LstrImg, strPre, strObj);

                label2.Text = line;
                count++;
                progressBar3.Value = count;
                Application.DoEvents();
            }
            progressBar3.Value = Lines.Length;
            O.Writer.Save(O.Graph, fileOnto);
            string text = File.ReadAllText(fileOnto);
            txtOnClasses.Text = text;

            DisnableAll();
            btnAddProperties.Enabled = true;
        }

        private void btnAddProperties_Click(object sender, EventArgs e)
        {
            progressBar3.Minimum = 0;
            progressBar3.Maximum = 100;
            progressBar3.Value = 0;
            //Ontology O = new Ontology(fileOnto);
            progressBar3.Value = 20;
            O.AnotationProperties();
            progressBar3.Value = 40;
            O.Dataproperties();
            O.Writer.Save(O.Graph, fileOnto);
            progressBar3.Value = 100;
            string text = File.ReadAllText(fileOnto);
            txtOnClasses.Text = text;

            DisnableAll();
            btnLiteralFile.Enabled = true;
        }

        private string getNameLine(string line)
        {
            char[] delimiters = new char[] { '@', '\t', '\r', '\n', ' ', ':' };
            string[] Words = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            return Words[0];
        }
        private string[] getPropClass(string line)
        {
            List<string> LProp = new List<string>();
            string[] Group = line.Split('(', ')');
            Group = RemoveBlank(Group);
            for (int i = 1; i < Group.Length; i++)
                LProp.Add(Group[i].Trim());
            return LProp.ToArray();
        }
        private int getValueLine(string line)
        {
            int len = line.Length;
            line = line.Trim();
            if (len > 0)
            {
                if (line.Substring(0, 3) == "IMG")
                    return 1;
                else
                    if (line.Substring(0, 2) == "in")
                        return 2;
                    else if (line.Substring(0, 3) == "dpr")
                        return 3;
                    else if (line.Substring(0, 3) == "ano")
                        return 4;
                    else return 5;
            }
            return 0;
        }
        private void btnAddLiterals_Click(object sender, EventArgs e)
        {
            TextfileCluster tf = new TextfileCluster();
            string[] Lines = tf.ReadAllLine(fileLiterals);
            Lines = RemoveBlank(Lines);
            progressBar4.Minimum = 0;
            progressBar4.Maximum = Lines.Length;
            int count = 0;
            progressBar4.Value = count;
            //Ontology O = new Ontology(fileOnto);
            foreach (string line in Lines)
            {
                string name = getNameLine(line);
                int d = getValueLine(name);
                string[] Prop = getPropClass(line);
                if (d == 1)
                    O.AddImageLiteral(name, Prop);
                else if (d == 2)
                    O.AddIndLiteral(name, Prop);
                else if (d == 3)
                    O.AddDataPropLiteral(name, Prop);
                else if (d == 4)
                    O.AddAnoPropLiteral(name, Prop);
                else if (d == 5)
                    O.AddClassLiteral(name, Prop);
                count++;
                progressBar4.Value = count;
                label2.Text = line;
                Application.DoEvents();
            }
            O.Writer.Save(O.Graph, fileOnto);
            string text = File.ReadAllText(fileOnto);
            txtOnClasses.Text = text;
            progressBar4.Value = Lines.Length;

            DisnableAll();
            btnLiteralFile.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog diag = new OpenFileDialog())
            {
                diag.Filter = "Text File |*.txt";
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    string file = diag.FileName;
                    txtFileLiteral.Text = file;
                    fileLiterals = file;

                    DisnableAll();
                    btnAddLiterals.Enabled = true;
                }
            }
        }

        private void DisnableAll()
        {
            btnClassesFile.Enabled = false;
            btnCreateOntoClasses.Enabled = false;
            btnSubClassesFile.Enabled = false;
            btnCreateOntoSubClasses.Enabled = false;
            btnCreateIndividuals.Enabled = false;
            btnCreateOProperties.Enabled = false;
            btnImageFile.Enabled = false;
            btnCreateImgInd.Enabled = false;
            btnAddProperties.Enabled = false;
            btnLiteralFile.Enabled = false;
            btnAddLiterals.Enabled = false;
        }
        private void ResetAll()
        {
            txtFileClass.Text = string.Empty;
            txtClasses.Text = string.Empty;
            txtFileSubClasses.Text = string.Empty;
            txtImgFIle.Text = string.Empty;
            txtFileLiteral.Text = string.Empty;
            txtOnClasses.Text = string.Empty;
            progressBar1.Value = 0;
            progressBar2.Value = 0;
            progressBar3.Value = 0;
            progressBar4.Value = 0;
            progressBar3.Value = 0;
            label2.Text = "data object name";
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            DisnableAll();
            ResetAll();
            btnClassesFile.Enabled = true;
        }

        private void frmCeateOntology_Load(object sender, EventArgs e)
        {
            DisnableAll();
            btnClassesFile.Enabled = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            //Application.Exit();
            Close();
        }
    }
}
