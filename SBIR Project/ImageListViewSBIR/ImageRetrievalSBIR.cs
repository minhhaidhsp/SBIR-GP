using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Manina.Windows.Forms;
using System.Reflection;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using ImageCLEF;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using VDS.RDF.Update;
using VDS.RDF.Storage.Params;
using VDS.RDF.Writing;
using VDS.RDF.Writing.Formatting;
using SBIR;
using VDS.RDF.Ontology;
using ImageListViewDemo;

namespace SBIR
{
    public partial class ImageRetrievalSBIR : Form
    {
        #region A Custom Renderer for Demonstrating the Control
        /// <summary>
        /// A renderer that displays useful information
        /// when the control is empty.
        /// </summary>
        private class DemoRenderer : ImageListView.ImageListViewRenderer
        {
            string[] infoTexts = new string[] { 
                "Start by adding some image files.",
                "You can switch between Thumbnails, Gallery, Pane and Details view modes.",
                "In Details mode, ImageListView displays image properties in columns.",
                "The appearance of ImageListView can be customized to a great extent.\r\nTry selecting a different renderer from the drop down.",
                "Size of generated thumbnails can be customized.\r\nImageListView will try to extract embedded Exif thumbnails if possible.",
            };

            int[] infoLocations = new int[]{
                23,
                190,
                244,
                360,
                490,
            };

            int current;
            private Timer infoTimer;

            public DemoRenderer()
            {
                current = 0;
                infoTimer = new Timer();
                infoTimer.Interval = 5000;
                infoTimer.Tick += new EventHandler(infoTimer_Tick);
                infoTimer.Enabled = true;
            }

            void infoTimer_Tick(object sender, EventArgs e)
            {
                current++;
                if (current == infoTexts.Length)
                    current = 0;

                ImageListView.Refresh();
            }

            public override void Dispose()
            {
                infoTimer.Dispose();
                base.Dispose();
            }

            /// <summary>
            /// Initializes the System.Drawing.Graphics used to draw
            /// control elements.
            /// </summary>
            /// <param name="g">The System.Drawing.Graphics to draw on.</param>
            public override void InitializeGraphics(Graphics g)
            {
                base.InitializeGraphics(g);
                g.SmoothingMode = SmoothingMode.HighQuality;
            }

            /// <summary>
            /// Draws an overlay image over the client area.
            /// </summary>
            /// <param name="g">The System.Drawing.Graphics to draw on.</param>
            /// <param name="bounds">The bounding rectangle of the client area.</param>
            public override void DrawOverlay(Graphics g, Rectangle bounds)
            {
                if (ImageListView.Items.Count != 0)
                {
                    infoTimer.Enabled = false;
                    return;
                }

                if (!infoTimer.Enabled)
                {
                    current = 0;
                    infoTimer.Enabled = true;
                }

                DrawToolTip(g, infoLocations[current], infoTexts[current]);
            }

            /// <summary>
            /// Draws a tooltip.
            /// </summary>
            private void DrawToolTip(Graphics g, int x, string s)
            {
                bool onLeft = (x < ImageListView.ClientRectangle.Width / 2);
                int width = 200;
                Size sz = Size.Round(g.MeasureString(s, ImageListView.Font, width));
                sz.Width += 20;
                sz.Height += 10;
                int y = 12;

                int arrowOffset = 15;
                if (!onLeft) arrowOffset = sz.Width - 30;
                if (!onLeft) x -= sz.Width - 45;

                int fillet = 10;
                if (fillet > sz.Height / 2) fillet = sz.Height / 2;
                int shadow = 3;

                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddLine(x - 20 + arrowOffset, y, x - 15 + arrowOffset, y - 10);
                    path.AddLine(x - 15 + arrowOffset, y - 10, x - 10 + arrowOffset, y);
                    path.AddLine(x - 10 + arrowOffset, y, x + sz.Width - 20 - fillet, y);
                    path.AddArc(x + sz.Width - 20 - 2 * fillet, y, 2 * fillet, 2 * fillet, 270.0f, 90.0f);
                    path.AddLine(x + sz.Width - 20, y + fillet, x + sz.Width - 20, y + sz.Height - fillet);
                    path.AddArc(x + sz.Width - 20 - 2 * fillet, y + sz.Height - 2 * fillet, 2 * fillet, 2 * fillet, 0.0f, 90.0f);
                    path.AddLine(x + sz.Width - 20 - fillet, y + sz.Height, x - 20 + fillet, y + sz.Height);
                    path.AddArc(x - 20, y + sz.Height - 2 * fillet, 2 * fillet, 2 * fillet, 90.0f, 90.0f);
                    path.AddLine(x - 20, y + sz.Height - fillet, x - 20, y + fillet);
                    path.AddArc(x - 20, y, 2 * fillet, 2 * fillet, 180.0f, 90.0f);
                    path.AddLine(x - 20 + fillet, y, x - 20 + arrowOffset, y);
                    path.CloseFigure();

                    path.Transform(new Matrix(1, 0, 0, 1, shadow, shadow));
                    using (Brush b = new SolidBrush(Color.FromArgb(128, Color.Gray)))
                    {
                        g.FillPath(b, path);
                    }
                    path.Transform(new Matrix(1, 0, 0, 1, -shadow, -shadow));

                    using (Brush b = new LinearGradientBrush(path.GetBounds(), Color.BlanchedAlmond, Color.White, LinearGradientMode.ForwardDiagonal))
                    {
                        g.FillPath(b, path);
                    }
                    using (Pen p = new Pen(SystemColors.InfoText))
                    {
                        g.DrawPath(p, path);
                    }
                    using (Brush b = new SolidBrush(SystemColors.InfoText))
                    {
                        g.DrawString(infoTexts[current], ImageListView.Font, b, new Rectangle(x - 20 + 10, y + 5, sz.Width - 16, sz.Height - 10));
                    }
                }
            }
        }
        #endregion

        #region Constructor
        public ImageRetrievalSBIR()
        {
            InitializeComponent();

            Application.Idle += new EventHandler(Application_Idle);

            // Populate renderer dropdown
            Assembly assembly = Assembly.GetAssembly(typeof(ImageListView));
            int i = 0;
            foreach (Type t in assembly.GetTypes())
            {
                if (t.BaseType == typeof(Manina.Windows.Forms.ImageListView.ImageListViewRenderer))
                {
                    renderertoolStripComboBox.Items.Add(new RendererItem(t));
                    if (t.Name == "DefaultRenderer")
                        renderertoolStripComboBox.SelectedIndex = i;
                    i++;
                }
            }
            imageListView1.SetRenderer(new DemoRenderer());
        }
        #endregion

        #region Refresh UI Cues
        void Application_Idle(object sender, EventArgs e)
        {
            // Refresh UI cues
            removeToolStripButton.Enabled = (imageListView1.SelectedItems.Count > 0);
            removeToolStripButton.Enabled = (imageListView1.SelectedItems.Count > 0);
            removeAllToolStripButton.Enabled = (imageListView1.Items.Count > 0);

            thumbnailsToolStripButton.Checked = (imageListView1.View == Manina.Windows.Forms.View.Thumbnails);
            detailsToolStripButton.Checked = (imageListView1.View == Manina.Windows.Forms.View.Details);
            galleryToolStripButton.Checked = (imageListView1.View == Manina.Windows.Forms.View.Gallery);
            paneToolStripButton.Checked = (imageListView1.View == Manina.Windows.Forms.View.Pane);

            clearCacheToolStripButton.Enabled = (imageListView1.Items.Count > 0);

            deleteToolStripMenuItem.Enabled = (imageListView1.SelectedItems.Count > 0);

            x48ToolStripMenuItem.Checked = (imageListView1.ThumbnailSize == new Size(48, 48));
            x96ToolStripMenuItem.Checked = (imageListView1.ThumbnailSize == new Size(96, 96));
            x120ToolStripMenuItem.Checked = (imageListView1.ThumbnailSize == new Size(120, 120));
            x150ToolStripMenuItem.Checked = (imageListView1.ThumbnailSize == new Size(150, 150));
            x200ToolStripMenuItem.Checked = (imageListView1.ThumbnailSize == new Size(200, 200));

            rotateCCWToolStripButton.Enabled = (imageListView1.SelectedItems.Count > 0);
            rotateCWToolStripButton.Enabled = (imageListView1.SelectedItems.Count > 0);
        }
        #endregion

        #region Add/Remove Items
        private void addToolStripButton_Click(object sender, EventArgs e)
        {
            string folder = ImageListViewSBIR.Properties.Settings.Default.LastFolder;
            if (Directory.Exists(folder))
                openFileDialog.InitialDirectory = folder;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                folder = Path.GetDirectoryName(openFileDialog.FileName);
                ImageListViewSBIR.Properties.Settings.Default.LastFolder = folder;
                ImageListViewSBIR.Properties.Settings.Default.Save();
                imageListView1.Items.AddRange(openFileDialog.FileNames);
            }
        }

        private void removeToolStripButton_Click(object sender, EventArgs e)
        {
            // Suspend the layout logic while we are removing items.
            // Otherwise the control will be refreshed after each item
            // is removed.
            imageListView1.SuspendLayout();

            // Remove selected items
            foreach (var item in imageListView1.SelectedItems)
                imageListView1.Items.Remove(item);

            // Resume layout logic.
            imageListView1.ResumeLayout(true);
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            removeToolStripButton_Click(sender, e);
        }

        private void removeAllToolStripButton_Click(object sender, EventArgs e)
        {
            imageListView1.Items.Clear();
        }
        #endregion

        #region Switch Renderers
        private struct RendererItem
        {
            public Type Type;

            public override string ToString()
            {
                return Type.Name;
            }

            public RendererItem(Type type)
            {
                Type = type;
            }
        }

        private void renderertoolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Change the renderer
            Assembly assembly = Assembly.GetAssembly(typeof(ImageListView));
            RendererItem item = (RendererItem)renderertoolStripComboBox.SelectedItem;
            ImageListView.ImageListViewRenderer renderer = assembly.CreateInstance(item.Type.FullName) as ImageListView.ImageListViewRenderer;
            imageListView1.SetRenderer(renderer);
            imageListView1.Focus();
        }
        #endregion

        #region Switch View Modes
        private void detailsToolStripButton_Click(object sender, EventArgs e)
        {
            imageListView1.View = Manina.Windows.Forms.View.Details;
        }

        private void thumbnailsToolStripButton_Click(object sender, EventArgs e)
        {
            imageListView1.View = Manina.Windows.Forms.View.Thumbnails;
        }

        private void galleryToolStripButton_Click(object sender, EventArgs e)
        {
            imageListView1.View = Manina.Windows.Forms.View.Gallery;
        }

        private void paneToolStripButton_Click(object sender, EventArgs e)
        {
            imageListView1.View = Manina.Windows.Forms.View.Pane;
        }
        #endregion

        #region Update Status Bar
        private void imageListView1_ThumbnailCached(object sender, ThumbnailCachedEventArgs e)
        {
            // This event is fired after a new thumbnail is cached.
            UpdateStatus(string.Format("Cached image: {0}", e.Item.Text));
            timerStatus.Enabled = true;
        }

        private void imageListView1_SelectionChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void timerStatus_Tick(object sender, EventArgs e)
        {
            UpdateStatus();
            timerStatus.Enabled = false;
        }

        private void UpdateStatus(string text)
        {
            toolStripStatusLabel.Text = text;
        }

        private void UpdateStatus()
        {
            if (imageListView1.Items.Count == 0)
                UpdateStatus("Ready");
            else if (imageListView1.SelectedItems.Count == 0)
                UpdateStatus(string.Format("{0} images", imageListView1.Items.Count));
            else
                UpdateStatus(string.Format("{0} images ({1} selected)", imageListView1.Items.Count, imageListView1.SelectedItems.Count));
        }
        #endregion

        #region Modify Column Headers
        private void columnsToolStripButton_Click(object sender, EventArgs e)
        {
            ChooseColumns form = new ChooseColumns();
            form.imageListView = imageListView1;
            form.ShowDialog();
        }
        #endregion

        #region Change Thumbnail Size
        private void x48ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageListView1.ThumbnailSize = new Size(48, 48);
        }

        private void x96ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageListView1.ThumbnailSize = new Size(96, 96);
        }

        private void x120ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageListView1.ThumbnailSize = new Size(120, 120);
        }

        private void x150ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageListView1.ThumbnailSize = new Size(150, 150);
        }

        private void x200ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageListView1.ThumbnailSize = new Size(200, 200);
        }

        private void clearCacheToolStripButton_Click(object sender, EventArgs e)
        {
            imageListView1.ClearThumbnailCache();
        }
        #endregion

        #region Rotate Selected Images
        private void rotateCCWToolStripButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Rotating will overwrite original images. Are you sure you want to continue?",
                "ImageListViewDemo", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                foreach (ImageListViewItem item in imageListView1.SelectedItems)
                {
                    item.BeginEdit();
                    using (Image img = Image.FromFile(item.FileName))
                    {
                        img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        img.Save(item.FileName);
                    }
                    item.Update();
                    item.EndEdit();
                }
            }
        }

        private void rotateCWToolStripButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Rotating will overwrite original images. Are you sure you want to continue?",
                "ImageListViewDemo", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                foreach (ImageListViewItem item in imageListView1.SelectedItems)
                {
                    item.BeginEdit();
                    using (Image img = Image.FromFile(item.FileName))
                    {
                        img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        img.Save(item.FileName);
                    }
                    item.Update();
                    item.EndEdit();
                }
            }
        }
        #endregion


        public string[] ArrayImage { get; set; }
        public string GroundClass { get; set; }
        public List<double[]>[] ClusterCLEF = new List<double[]>[41];
        string[][] Wlist = null;
       
        string fileOnto = @"../../../CreateOntology/FileOntology/SBIR-Ontology.n3";
        static string pathNameWlist = @"../../data/wlist.txt";
        static string pathNameAllFeature = @"../../data/AllFeatures.txt";

        public OntologyListViewSBIR O;

        public OntologyListViewSBIR Onto
        {
            get
            {
                return this.O;
            }
            set
            {
                this.O = value;
            }
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

        public string GetFileName(string path)
        {
            string result = Path.GetFileName(path);
            return result;
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
        public string GetNameImg(double nName)
        {
            int q = ((int)nName) / 1000;
            //int r = Int32.Parse(nName) % 1000;
            string name = string.Empty;
            if (q < 10)
                name = "0" + q.ToString() + "/" + "images" + "/" + nName.ToString() + ".jpg";
            else
                name = q.ToString() + "/" + nName.ToString() + ".jpg";
            return name;
        }

        public string GetPathImgCLEF(double nName)
        {
            return @"../../../ImageDBs/ImageCLEF/SAIAPR TC-12 Benchmark/benchmark/saiapr_tc-12/" + GetNameImg(nName);
        }
        public string GetPathSegImgCLEF(double nName)
        {
            return @"../../../ImageDBs/ImageCLEF/SAIAPR TC-12 Benchmark/benchmark/saiapr_tc-12/" + GetLastFolderName(nName) + "/" + "segmented_images" + "/" + nName.ToString() + ".jpg";
        }
        public string GetPathFeatureImgCLEF(double nName)
        {
            return @"../../../ImageDBs/ImageCLEF/SAIAPR TC-12 Benchmark/benchmark/saiapr_tc-12/" + GetLastFolderName(nName) + "/" + "features.txt";
        }
        public string GetPathAnoImgCLEF(double nName)
        {
            return @"../../../ImageDBs/ImageCLEF/SAIAPR TC-12 Benchmark/benchmark/branch/annotations/" + GetLastFolderName(nName) + "/" + nName.ToString() + ".eng";
        }
        double[] GetGroundClass(double[][] Testing, double nName)
        {
            List<double> res = new List<double>();
            for (int i = 0; i < Testing.Length; i++)
                if (Testing[i][0] == nName)
                    res.Add(Testing[i][Testing[i].Length - 1]);
            return res.ToArray();
        }
        void ShowClass(double[] GroundClass, string[][] Wlist)
        {
            string text = string.Empty;
            int i = 0;
            double[] Cl = GetDistinctClass(GroundClass);
            for (i = 0; i < Cl.Length - 1; i++)
            {
                text += Wlist[(int)Cl[i] - 1][1] + "(" + Count(GroundClass, Cl[i]).ToString() + ")" + ", ";
            }
            text += Wlist[(int)Cl[i] - 1][1];
            txtImgInfo.Text += "\r\n" + text + "(" + Count(GroundClass, Cl[i]).ToString() + ")" + " [No. segments: " + GroundClass.Length.ToString() + "]";
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

        //Tạo câu truy vấn tìm têm thông tin hình ảnh các hình ảnh từ ID ảnh
        private string getImageFileDes(string IMGname)
        {
            string SPARQL = string.Empty;

            SPARQL = @"PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>
PREFIX owl: <http://www.w3.org/2002/07/owl#>
PREFIX xml: <http://www.w3.org/XML/1998/namespace>
PREFIX sbir: <http://sbir-hcm.vn/>
SELECT DISTINCT ?FileDesciption
WHERE{";
            SPARQL += "sbir:" + IMGname + " " + "sbir:imgFilename" + " " + "?FileDesciption" + " ." + "}";
            List<string> LResult;
            string strkq = string.Empty;

            if (SPARQL != string.Empty)
            {
                LResult = O.QuerySPARQL(SPARQL);
                foreach (string res in LResult)
                    strkq += res + " ";
            }
            return strkq.Trim();
        }

        private string getImageListClass(string IMGname)
        {
            string SPARQL = string.Empty;

            SPARQL = @"PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>
PREFIX owl: <http://www.w3.org/2002/07/owl#>
PREFIX xml: <http://www.w3.org/XML/1998/namespace>
PREFIX sbir: <http://sbir-hcm.vn/>
SELECT DISTINCT ?ListClasss
WHERE{";
            SPARQL += "sbir:" + IMGname + " " + "sbir:imgListclass" + " " + "?ListClasss" + " ." + "}";
            List<string> LResult;
            string strkq = string.Empty;

            if (SPARQL != string.Empty)
            {
                LResult = O.QuerySPARQL(SPARQL);
                foreach (string res in LResult)
                    strkq += res + " ";
            }
            return strkq.Trim();
        }

        private string getImagePath(string IMGname)
        {
            string SPARQL = string.Empty;

            SPARQL = @"PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>
PREFIX owl: <http://www.w3.org/2002/07/owl#>
PREFIX xml: <http://www.w3.org/XML/1998/namespace>
PREFIX sbir: <http://sbir-hcm.vn/>
SELECT DISTINCT ?ImgPath
WHERE{";
            SPARQL += "sbir:" + IMGname + " " + "sbir:imgPath" + " " + "?ImgPath" + " ." + "}";
            List<string> LResult;
            string strkq = string.Empty;

            if (SPARQL != string.Empty)
            {
                LResult = O.QuerySPARQL(SPARQL);
                foreach (string res in LResult)
                    strkq += res + " ";
            }
            return strkq.Trim();
        }

        private string getImageURI(string IMGname)
        {
            string SPARQL = string.Empty;

            SPARQL = @"PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>
PREFIX owl: <http://www.w3.org/2002/07/owl#>
PREFIX xml: <http://www.w3.org/XML/1998/namespace>
PREFIX sbir: <http://sbir-hcm.vn/>
SELECT DISTINCT ?ImgURI
WHERE{";
            SPARQL += "sbir:" + IMGname + " " + "sbir:imgURI" + " " + "?ImgURI" + " ." + "}";
            List<string> LResult;
            string strkq = string.Empty;

            if (SPARQL != string.Empty)
            {
                LResult = O.QuerySPARQL(SPARQL);
                foreach (string res in LResult)
                    strkq += res + " ";
            }
            return strkq.Trim();
        }
        private string getImageClassName(string IMGname)
        {
            string SPARQL = string.Empty;

            SPARQL = @"PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>
PREFIX owl: <http://www.w3.org/2002/07/owl#>
PREFIX xml: <http://www.w3.org/XML/1998/namespace>
PREFIX sbir: <http://sbir-hcm.vn/>
SELECT DISTINCT ?Info
WHERE{";
            SPARQL += "sbir:" + IMGname + " " + "?Pre" + " " + "?Info" + " ." + "}";
            List<string> LResult;
            string strkq = string.Empty;

            if (SPARQL != string.Empty)
            {
                LResult = O.QuerySPARQL(SPARQL);
                foreach (string res in LResult)
                    strkq += res + " ";
            }
            return strkq.Trim();
        }
        private string getListClassfromString(string ImgListClasses)
        {
            char[] delimiters = new char[] { '@', '\t', '\r', '\n', ' ', '/', ' ' };
            string[] Classes = ImgListClasses.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            string tmp = string.Empty;
            int len = Classes.Length;
            for (int i = 0; i < len - 1; i++)
            {
                tmp += Classes[i] + ", ";
            }
            tmp += Classes[len - 1];
            return tmp;
        }
        private string QueryImageSemantic(string name)
        {
            string strImgInfo = string.Empty;
            //Query image filename
            string ImgFileDes = getImageFileDes("IMG" + name);
            strImgInfo += "\rThe name of image description file: " + ImgFileDes;
            //Query the list ofimage classes:
            string ImgListClasses = getImageListClass("IMG" + name);
            ImgListClasses = getListClassfromString(ImgListClasses);
            strImgInfo += "\r\nThe list of image classes: " + ImgListClasses;
            //Query Image Path
            string ImgPath = getImagePath("IMG" + name);
            strImgInfo += "\r\nThe path of image name: " + ImgPath;
            //Query Image URI
            string ImgURI = getImageURI("IMG" + name);
            strImgInfo += "\r\nImage URI: " + ImgURI;
            return strImgInfo;
        }
        private void DemoForm_Load(object sender, EventArgs e)
        {
            
            imageListView1.Items.AddRange(ArrayImage);
            //O = new OntologyLisview(fileOnto);
            string name = Path.GetFileNameWithoutExtension(ArrayImage[0]);
            txtImgInfo.Text += QueryImageSemantic(name);
            string ImgFileDes = getImageFileDes("IMG" + name);
            txtImgInfo.Text += "\r\n" + File.ReadAllText(ImgFileDes);
            string ImgPath = getImagePath("IMG" + name);
            pictureBoxImage.Image = Image.FromFile(ImgPath);
        }

        public string[] GetClassName(double[] GroundClass, string[][] Wlist)
        {
            List<string> lstClass = new List<string>();
            double[] Cl = GetDistinctClass(GroundClass);
            for (int i = 0; i < Cl.Length; i++)
            {
                lstClass.Add(Wlist[(int)Cl[i] - 1][1]);
            }
            return lstClass.ToArray();
        }

        private void imageListView1_ItemClick(object sender, ItemClickEventArgs e)
        {
            string name = Path.GetFileNameWithoutExtension(e.Item.FileName);
            txtImgInfo.Text = "";
            txtImgInfo.Text += "\r\n" + QueryImageSemantic(name);
            string ImgFileDes = getImageFileDes("IMG" + name);
            txtImgInfo.Text += "\r\n" + File.ReadAllText(ImgFileDes);
            string ImgPath = getImagePath("IMG" + name);
            pictureBoxImage.Image = Image.FromFile(ImgPath);
        }
        bool ChkClass(string[] ClassNane, string name)
        {
            for (int i = 0; i < ClassNane.Length; i++)
            {
                if (name == ClassNane[i])
                    return true;
            }
            return false;
        }
        static string GetNodeString(INode node)
        {
            string s = node.ToString();
            switch (node.NodeType)
            {
                case NodeType.Uri:
                    int lio = s.LastIndexOf('#');
                    if (lio == -1)
                        return s;
                    else
                        return s.Substring(lio + 1);
                case NodeType.Literal:
                    return string.Format("\"{0}\"", s);
                default:
                    return s;
            }
        }
        public void ShowAnnotation(string[] Ano)
        {
            string text = string.Empty;
            txtImgInfo.Text = "";
            int i = 0;
            for (i = 0; i < Ano.Length - 1; i++)
                text += Ano[i] + "\r\n";
            text += Ano[i];
            txtImgInfo.Text = text;
        }

        private void imageListView1_ImeModeChanged(object sender, EventArgs e)
        {

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
        bool ChkInArr(double[] Arr, double a)
        {
            for (int i = 0; i < Arr.Length; i++)
            {
                if (Arr[i] == a)
                    return true;
            }
            return false;
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

        //void ShowClass(double[] GroundClass, string[][] Wlist)
        //{
        //    string text = string.Empty;
        //    int i = 0;
        //    double[] Cl = GetDistinctClass(GroundClass);
        //    for (i = 0; i < Cl.Length - 1; i++)
        //    {
        //        text += Wlist[(int)Cl[i]][1] + "(" + Count(GroundClass, Cl[i]).ToString() + ")" + ", ";
        //    }
        //    text += Wlist[(int)Cl[i]][1];
        //    txtImgInfo.Text = text + "(" + Count(GroundClass, Cl[i]).ToString() + ")" + " [No. segments: " + GroundClass.Length.ToString() + "]";
        //}
    }
}
