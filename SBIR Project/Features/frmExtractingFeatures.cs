using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SBIR
{
    public partial class frmExtractingFeatures : Form
    {
        public frmExtractingFeatures()
        {
            InitializeComponent();
        }

        public void showListImage()
        {

        }
        List<Bitmap> getListBitmap(Bitmap Bmp)
        {
            List<Bitmap> lstBmp = new List<Bitmap>();

            return lstBmp;
        }

        public string getFeatures(Bitmap Bmp)
        {
            int height = Bmp.Height;
            int width = Bmp.Width;
            Size size = new Size(height, width);

            //Khởi tạo đối tượng xử lý ảnh
            ImageProcessing IP = new ImageProcessing();
            //Khởi tạo dữ liệu đặc trưng của hình ảnh
            string features = string.Empty;

            //Ảnh giảm mẫu
            Bitmap bmpResize;

            //Lấy đặc trưng màu (histogram) theo MPEG7 (đặc trưng màu sắc)
            int[] hisNewton = IP.getHistogramMPEG7Color(Bmp);
            double[] hisNorm = IP.getHisNormal(hisNewton);
            string strHis = Arr2string(hisNorm);
            features += strHis;

            //Lấy cường độ đặc trưng của ảnh theo đối các điểm ảnh theo láng giềng (đặc trưng hình dạng)
            //bmpResize = IP.getMaxPoolingNtimes(Bmp, 3, 3);
            bmpResize = IP.Resize(Bmp, 3, 3);
            features += ", " + IP.getStrIntensy(bmpResize);

            //Tạo đối tượng ảnh theo độ tương phản (độ sáng tối) dựa trên phép biến đổi gamma
            Bitmap BmpC = new Bitmap(Bmp);
            BmpC = IP.GammaCorrection(BmpC, 1.5);
            BmpC = IP.ImageFilterColor(BmpC, IP.GF1());//sử dụng phép lọc Gauss đê loại bỏ nhiểu tăng vùng cho điểm sáng
            BmpC = IP.ContrastStretching(BmpC);
            BmpC = IP.ImageMedianFilterGS(BmpC, 5);//sử dụng phép lọc trung bình để lọc nhiễu

            //Lấy mặt nạ ảnh đối tượng (foreground) là ảnh tương ứng có độ sáng cao
            Bitmap Mask = new Bitmap(BmpC);
            Mask = IP.ToBlackwhite(Mask, 64); //Màu đen là màu nền, màu trắng là màu đối tượng 
            pictureBox2.Image = Mask;

            //Lấy đối ảnh tượng và hình nền của ảnh ban đầu
            Bitmap BmpObject = IP.getRegionWhite(Bmp, Mask);
            Bitmap BmpBackground = IP.getRegionBlack(Bmp, Mask);
            pictureBox3.Image = BmpObject;
            pictureBox4.Image = BmpBackground;

            //Lấy đặc trưng diện tích của đối tượng (đặc trưng đối tượng)
            double areaObject = IP.getRegionArea(BmpObject);
            features += ", " + areaObject.ToString();

            //Lấy đặc trưng vị trí tương đối của đối tượng theo trục X và trục Y
            double meanObjX = IP.getMeanX(BmpObject);
            double meanObjY = IP.getMeanY(BmpObject);
            features += ", " + meanObjX.ToString() + ", " + meanObjY.ToString();

            //Lấy đặc trưng vị trí tương đối của hình nền theo trục X và trục Y
            double meanBgX = IP.getMeanX(BmpBackground);
            double meanBgY = IP.getMeanY(BmpBackground);
            features += ", " + meanBgX.ToString() + ", " + meanBgY.ToString();

            //Lấy cường độ đặc trưng đối tượng (đặc trưng hình dạng của đối tượng)
            bmpResize = IP.Resize(BmpObject, 3, 3);
            //bmpResize = IP.getMaxPoolingNtimes(BmpObject, 3, 3);
            features += ", " + IP.getStrIntensy(bmpResize);

            //Lấy cường độ đặc trưng hình nền (đặc trưng hình dạng hình nền)
            bmpResize = IP.Resize(BmpBackground, 3, 3);
            //bmpResize = IP.getMaxPoolingNtimes(BmpObject, 3, 3);
            features += ", " + IP.getStrIntensy(bmpResize);

            //Phép lọc Laplace cho ảnh mặt nạ để lấy đường biên đối tượng đặc trưng
            Bitmap BmpBoundary = new Bitmap(Mask, size);
            BmpBoundary = IP.ImageFilterColor(BmpBoundary, IP.LaplaceF1());
            pictureBox5.Image = BmpBoundary;

            //Lấy đặc trưng hình dạng của đường biên ảnh (mỗi hình dạng có một số đại diện)
            bmpResize = IP.Resize2(BmpBoundary, 4, 4);
            features += ", " + IP.getRegionArea(bmpResize);

            //Lấy đặc trưng chu vi của đối tượng
            double cirObject = IP.getRegionArea(BmpBoundary);
            features += ", " + cirObject.ToString();

            //Phép lọc Sobel để lấy đường biên đối tượng và vân ảnh của bề mặt đối tượng
            Bitmap BmpSobel = new Bitmap(Bmp, size);
            BmpSobel = IP.ImageSobelFilterColor(BmpSobel);
            pictureBox6.Image = BmpSobel;

            //Lấy đặc trưng chu vi của đối tượng theo phép lọc Sobel
            cirObject = IP.getRegionArea(BmpSobel);
            features += ", " + cirObject.ToString();

            //Lấy đặc trưng cường độ của ảnh theo đối các điểm ảnh theo láng giềng theo phép lọc Sobel
            bmpResize = IP.Resize2(BmpSobel, 4, 4);
            features += ", " + IP.getRegionArea(bmpResize);

            //Thực hiện phép lọc Laplace để lấy các đường nét của ảnh
            Bitmap BmpLaplace = new Bitmap(Bmp, size);
            BmpLaplace = IP.ImageFilterColor(BmpLaplace, IP.LaplaceF1());
            pictureBox7.Image = BmpLaplace;

            //Lấy đặc trưng chu vi của đối tượng theo phép lọc Laplace
            cirObject = IP.getRegionArea(BmpLaplace);
            features += ", " + cirObject.ToString();

            //Lấy đặc trưng đường nét của ảnh theo phép lọc Laplace (một giá trị đại diện cho đường nét)
            bmpResize = IP.Resize2(BmpLaplace, 4, 4);
            features += ", " + IP.getRegionArea(bmpResize);

            //Thực hiện phép lọc thông cao cho ảnh ban đầu (chỉ cho những tần số cao đi qua, loại bỏ những tần số thấp) phép lọc thông cao dùng để lấy ảnh đường nét
            Bitmap BmpHpf = new Bitmap(Bmp, size);
            BmpHpf = IP.ImageFilterColor(BmpHpf, IP.HPF1());
            //bmpResize = IP.getMaxPoolingNtimes(Bmp6, 3, 3);
            bmpResize = IP.Resize(BmpHpf, 3, 3);
            features += ", " + IP.getStrIntensy(bmpResize);
            pictureBox8.Image = BmpHpf;

            //Thực hiện phép lọc Gaussian để nâng cường độ cho điểm ảnh dựa trên láng giềng láng giềng càng gần thì trọng số càng cao
            Bitmap BmpGauss = new Bitmap(Bmp, size);
            BmpGauss = IP.ImageFilterColor(BmpGauss, IP.GF1());
            //bmpResize = IP.getMaxPoolingNtimes(BmpGauss, 3, 3);
            bmpResize = IP.Resize(BmpGauss, 3, 3);
            features += ", " + IP.getStrIntensy(bmpResize);
            pictureBox7.Image = BmpGauss;
            return features;
        }

        public string getFeatures2(Bitmap Bmp)
        {
            int height = Bmp.Height;
            int width = Bmp.Width;
            Size size = new Size(height, width);

            //Khởi tạo đối tượng xử lý ảnh
            ImageProcessing IP = new ImageProcessing();
            //Khởi tạo dữ liệu đặc trưng của hình ảnh
            string features = string.Empty;

            //Ảnh giảm mẫu
            Bitmap bmpResize;

            //Lấy đặc trưng màu (histogram) theo MPEG7 (đặc trưng màu sắc)
            int[] hisNewton = IP.getHistogramMPEG7Color(Bmp);
            double[] hisNorm = IP.getHisNormal(hisNewton);
            string strHis = Arr2string(hisNorm);
            features += strHis;

            //Lấy cường độ đặc trưng của ảnh theo đối các điểm ảnh theo láng giềng (đặc trưng hình dạng)
            //bmpResize = IP.getMaxPoolingNtimes(Bmp, 3, 3);
            bmpResize = IP.Resize(Bmp, 3, 3);
            features += ", " + IP.getStrIntensy(bmpResize);

            //Tạo đối tượng ảnh theo độ tương phản (độ sáng tối) dựa trên phép biến đổi gamma
            Bitmap BmpC = new Bitmap(Bmp);
            BmpC = IP.GammaCorrection(BmpC, 1.5);
            BmpC = IP.ImageFilterColor(BmpC, IP.GF1());//sử dụng phép lọc Gauss đê loại bỏ nhiểu tăng vùng cho điểm sáng
            BmpC = IP.ContrastStretching(BmpC);
            BmpC = IP.ImageMedianFilterGS(BmpC, 5);//sử dụng phép lọc trung bình để lọc nhiễu

            //Lấy mặt nạ ảnh đối tượng (foreground) là ảnh tương ứng có độ sáng cao
            Bitmap Mask = new Bitmap(BmpC);
            Mask = IP.ToBlackwhite(Mask, 64); //Màu đen là màu nền, màu trắng là màu đối tượng 

            //Lấy đối ảnh tượng và hình nền của ảnh ban đầu
            Bitmap BmpObject = IP.getRegionWhite(Bmp, Mask);
            Bitmap BmpBackground = IP.getRegionBlack(Bmp, Mask);

            //Lấy đặc trưng diện tích của đối tượng (đặc trưng đối tượng)
            double areaObject = IP.getRegionArea(BmpObject);
            features += ", " + areaObject.ToString();

            //Lấy đặc trưng vị trí tương đối của đối tượng theo trục X và trục Y
            double meanObjX = IP.getMeanX(BmpObject);
            double meanObjY = IP.getMeanY(BmpObject);
            features += ", " + meanObjX.ToString() + ", " + meanObjY.ToString();

            //Lấy đặc trưng vị trí tương đối của hình nền theo trục X và trục Y
            double meanBgX = IP.getMeanX(BmpBackground);
            double meanBgY = IP.getMeanY(BmpBackground);
            features += ", " + meanBgX.ToString() + ", " + meanBgY.ToString();

            //Lấy cường độ đặc trưng đối tượng (đặc trưng hình dạng của đối tượng)
            bmpResize = IP.Resize(BmpObject, 3, 3);
            //bmpResize = IP.getMaxPoolingNtimes(BmpObject, 3, 3);
            features += ", " + IP.getStrIntensy(bmpResize);

            //Lấy cường độ đặc trưng hình nền (đặc trưng hình dạng hình nền)
            bmpResize = IP.Resize(BmpBackground, 3, 3);
            //bmpResize = IP.getMaxPoolingNtimes(BmpObject, 3, 3);
            features += ", " + IP.getStrIntensy(bmpResize);

            //Phép lọc Laplace cho ảnh mặt nạ để lấy đường biên đối tượng đặc trưng
            Bitmap BmpBoundary = new Bitmap(Mask, size);
            BmpBoundary = IP.ImageFilterColor(BmpBoundary, IP.LaplaceF1());

            //Lấy đặc trưng hình dạng của đường biên ảnh (mỗi hình dạng có một số đại diện)
            bmpResize = IP.Resize2(BmpBoundary, 4, 4);
            features += ", " + IP.getRegionArea(bmpResize);

            //Lấy đặc trưng chu vi của đối tượng
            double cirObject = IP.getRegionArea(BmpBoundary);
            features += ", " + cirObject.ToString();

            //Phép lọc Sobel để lấy đường biên đối tượng và vân ảnh của bề mặt đối tượng
            Bitmap BmpSobel = new Bitmap(Bmp, size);
            BmpSobel = IP.ImageSobelFilterColor(BmpSobel);

            //Lấy đặc trưng chu vi của đối tượng theo phép lọc Sobel
            cirObject = IP.getRegionArea(BmpSobel);
            features += ", " + cirObject.ToString();

            //Lấy đặc trưng cường độ của ảnh theo đối các điểm ảnh theo láng giềng theo phép lọc Sobel
            bmpResize = IP.Resize2(BmpSobel, 4, 4);
            features += ", " + IP.getRegionArea(bmpResize);

            //Thực hiện phép lọc Laplace để lấy các đường nét của ảnh
            Bitmap BmpLaplace = new Bitmap(Bmp, size);
            BmpLaplace = IP.ImageFilterColor(BmpLaplace, IP.LaplaceF1());

            //Lấy đặc trưng chu vi của đối tượng theo phép lọc Laplace
            cirObject = IP.getRegionArea(BmpLaplace);
            features += ", " + cirObject.ToString();

            //Lấy đặc trưng đường nét của ảnh theo phép lọc Laplace (một giá trị đại diện cho đường nét)
            bmpResize = IP.Resize2(BmpLaplace, 4, 4);
            features += ", " + IP.getRegionArea(bmpResize);

            //Thực hiện phép lọc thông cao cho ảnh ban đầu (chỉ cho những tần số cao đi qua, loại bỏ những tần số thấp) phép lọc thông cao dùng để lấy ảnh đường nét
            Bitmap BmpHpf = new Bitmap(Bmp, size);
            BmpHpf = IP.ImageFilterColor(BmpHpf, IP.HPF1());
            //bmpResize = IP.getMaxPoolingNtimes(Bmp6, 3, 3);
            bmpResize = IP.Resize(BmpHpf, 3, 3);
            features += ", " + IP.getStrIntensy(bmpResize);

            //Thực hiện phép lọc Gaussian để nâng cường độ cho điểm ảnh dựa trên láng giềng láng giềng càng gần thì trọng số càng cao
            Bitmap BmpGauss = new Bitmap(Bmp, size);
            BmpGauss = IP.ImageFilterColor(BmpGauss, IP.GF1());
            //bmpResize = IP.getMaxPoolingNtimes(BmpGauss, 3, 3);
            bmpResize = IP.Resize(BmpGauss, 3, 3);
            features += ", " + IP.getStrIntensy(bmpResize);

            return features;
        }

        public List<Bitmap> getImageRegions(Bitmap Bmp)
        {
            List<Bitmap> lstBmp = new List<Bitmap>();
            //Khởi tạo đối tượng xử lý ảnh
            ImageProcessing IP = new ImageProcessing();
            //Chia ảnh thành 9 vùng (3x3)
            int height = Bmp.Height;
            int width = Bmp.Width;
            Bitmap Bmp10 = new Bitmap(Bmp);
            Bitmap Bmp10a = IP.ImageCut(Bmp10, height / 3, width / 3, ImageProcessing.Anchor.NorthWest);
            lstBmp.Add(Bmp10a);
            
            Bitmap Bmp10b = IP.ImageCut(Bmp10, height / 3, width / 3, ImageProcessing.Anchor.North);
            lstBmp.Add(Bmp10b);
           
            Bitmap Bmp10c = IP.ImageCut(Bmp10, height / 3, width / 3, ImageProcessing.Anchor.NorthEast);
            lstBmp.Add(Bmp10c);
            
            Bitmap Bmp10d = IP.ImageCut(Bmp10, height / 3, width / 3, ImageProcessing.Anchor.West);
            lstBmp.Add(Bmp10d);
           
            Bitmap Bmp10e = IP.ImageCut(Bmp10, height / 3, width / 3, ImageProcessing.Anchor.Middle);
            lstBmp.Add(Bmp10e);
           
            Bitmap Bmp10f = IP.ImageCut(Bmp10, height / 3, width / 3, ImageProcessing.Anchor.East);
            lstBmp.Add(Bmp10f);
            
            Bitmap Bmp10g = IP.ImageCut(Bmp10, height / 3, width / 3, ImageProcessing.Anchor.SouthWest);
            lstBmp.Add(Bmp10g);
           
            Bitmap Bmp10h = IP.ImageCut(Bmp10, height / 3, width / 3, ImageProcessing.Anchor.South);
            lstBmp.Add(Bmp10h);
            
            Bitmap Bmp10i = IP.ImageCut(Bmp10, height / 3, width / 3, ImageProcessing.Anchor.SouthEast);
            lstBmp.Add(Bmp10i);
           
            return lstBmp;

        }

        public string getFeatutes9Region(Bitmap Bmp)
        {
            string features = string.Empty;
            List<Bitmap> lstBmpRegions = getImageRegions(Bmp);
            for (int i = 0; i < lstBmpRegions.Count - 1; i++)
            {
                string f = getFeatures2(lstBmpRegions.ElementAt(i));
                features += f + ", ";
            }
            features += getFeatures2(lstBmpRegions.ElementAt(lstBmpRegions.Count - 1));
            return features;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog diag = new OpenFileDialog())
            {
                diag.Filter = "Bitmap |*.bmp;*.jpg;*.gif;*.png";
                diag.InitialDirectory = Application.StartupPath;
                if (diag.ShowDialog() == DialogResult.OK)
                {
                    string ImgPath = diag.FileName;
                    System.Drawing.Image Img = System.Drawing.Image.FromFile(ImgPath);
                    pictureBox1.Image = Img;

                    //Khởi tạo đối tượng dữ liệu (dạng bitmap) cho hình ảnh
                    Bitmap OrgImg = new Bitmap(ImgPath);
                    //Hiệu chỉnh lại kích thước cho dữ liệu hình ảnh
                    int height = 90;
                    int width = 90;
                    Size size = new Size(height, width);

                    //Tạo đối tượng dữ liệu ảnh (dạng Bitmap)
                    Bitmap Bmp = new Bitmap(OrgImg, size);
                    //Khởi tạo đối tượng xử lý ảnh
                    ImageProcessing IP = new ImageProcessing();

                    //Lấy đặc trưng của hình ảnh
                    string features = getFeatures(Bmp);

                    textBox1.Text = features;
                }
            }
        }

        private string Arr2string(double[]arr)
        {
            string str = string.Empty;
            if (arr == null) return str;
            if (arr.Length == 0) return str;
            for (int i = 0; i < arr.Length - 1; i++)
                str += arr[i].ToString() + ", ";
            str += arr[arr.Length - 1].ToString();
            return str;
        }
        private void frmExtractingFeatures_Load(object sender, EventArgs e)
        {
        //    //int[] Black = { 0, 0, 0 };
        //    panel1.BackColor = Color.FromArgb(0, 0, 0);
        //    //int[] SeaGreen = { 0, 182, 0};
        //    panel2.BackColor = Color.FromArgb(0, 182, 0);
        //    //int[] LightGreen = { 0, 255, 170 };
        //    panel3.BackColor = Color.FromArgb(0, 255, 170);
        //    //int[] OliveGreen = { 36, 73, 0 };
        //    panel4.BackColor = Color.FromArgb(36, 73, 0);
        //    //int[] Aqua = { 36, 16, 170 };
        //    panel5.BackColor = Color.FromArgb(36, 16, 170);
        //    //int[] BrightGreen = { 36, 255, 0 };
        //    panel6.BackColor = Color.FromArgb(36, 255, 0);
        //    //int[] Blue = { 73, 36, 170 };
        //    panel7.BackColor = Color.FromArgb(73, 36, 170);
        //    //int[] Green = { 73, 146, 0};
        //    panel8.BackColor = Color.FromArgb(73, 146, 0);
        //    //int[] Turquoise = { 73, 219, 170 };
        //    panel9.BackColor = Color.FromArgb(73, 219, 170);
        //    //int[] Brown = { 109, 36, 0 };
        //    panel10.BackColor = Color.FromArgb(109, 36, 0);
        //    //int[] BlueGray = { 109, 109, 170 };
        //    panel11.BackColor = Color.FromArgb(109, 109, 170);
        //    //int[] Lime = { 109, 219, 0 };
        //    panel12.BackColor = Color.FromArgb(109, 219, 0);
        //    //int[] Lavenda = { 146, 0, 170 };
        //    panel13.BackColor = Color.FromArgb(146, 0, 170);
        //    //int[] Plum = { 146, 109, 0 };
        //    panel14.BackColor = Color.FromArgb(146, 109, 0);
        //    //int[] Teal = { 146, 182, 170 };
        //    panel15.BackColor = Color.FromArgb(146, 182, 170);
        //    //int[] DarkRed = { 182, 0, 0 };
        //    panel16.BackColor = Color.FromArgb(182, 0, 0);
        //    //int[] Magenta = { 182, 73, 170 };
        //    panel17.BackColor = Color.FromArgb(182, 73, 170);
        //    //int[] YellowGreen = { 182, 182, 0 };
        //    panel18.BackColor = Color.FromArgb(182, 182, 0);
        //    //int[] FlouroGreen = { 182, 255, 170 };
        //    panel19.BackColor = Color.FromArgb(182, 255, 170);
        //    //int[] Red = { 219, 73, 0 };
        //    panel20.BackColor = Color.FromArgb(219, 73, 0);
        //    //int[] Rose = { 219, 146, 170 };
        //    panel21.BackColor = Color.FromArgb(219, 146, 170);
        //    //int[] Yellow = { 219, 255, 0 };
        //    panel22.BackColor = Color.FromArgb(219, 255, 0);
        //    //int[] Pink = { 255, 36, 170 };
        //    panel23.BackColor = Color.FromArgb(255, 36, 170);
        //    //int[] Orange = { 255, 146, 0 };
        //    panel24.BackColor = Color.FromArgb(255, 146, 0);
        //    //int[] White = { 255, 255, 255 };
        //    panel25.BackColor = Color.FromArgb(255, 255, 255);
        }
        public string getFileImageDB()
        {
            string file = string.Empty;
            if(radioButton1.Checked == true)
                file = @"../../C-TreeCOREL/listCorelED.txt";
            else if (radioButton2.Checked == true)
                file = @"../../C-TreeWang/listWangED.txt";
            else if (radioButton3.Checked == true)
                file = @"../../C-TreeStanfordDogs/listStanfordDogsED.txt"; 
            else if (radioButton4.Checked == true)
                file = @"../../C-TreeImageCLEF/listImageClefED.txt";
            return file;
        }
        public string getFileImageDB2()
        {
            string file = string.Empty;
            if (radioButton1.Checked == true)
                file = @"../../C-TreeCOREL2/listCorelED.txt";
            else if (radioButton2.Checked == true)
                file = @"../../C-TreeWang2/listWangED.txt";
            else if (radioButton3.Checked == true)
                file = @"../../C-TreeStanfordDogs2/listStanfordDogsED.txt";
            else if (radioButton4.Checked == true)
                file = @"../../C-TreeImageCLEF2/listImageClefED.txt";
            return file;
        }
        public string getFileClass()
        {
            string file = string.Empty;
            if (radioButton1.Checked == true)
                file = @"../../data/COREL-Class.txt";
            else if (radioButton2.Checked == true)
                file = @"../../data/Wang-Class.txt";
            else if (radioButton3.Checked == true)
                file = @"../../data/StanfordDogs-Class.txt"; 
            else if (radioButton4.Checked == true)
                file = @"../../data/ImageClef-Class.txt";
            return file;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            string featureFile = getFileImageDB();
            string featureFile2 = getFileImageDB2();
            if (folderDlg.ShowDialog() == DialogResult.OK)
            {
                string[] filenames = Directory.GetFiles(folderDlg.SelectedPath, "*.jpg", SearchOption.AllDirectories);
                progressBar1.Minimum = 0;
                progressBar1.Maximum = filenames.Length;
                int count = 0;
                progressBar1.Value = count;
                foreach (string name in filenames)
                {
                    //string features = string.Empty;
                    Bitmap OrgImg = new Bitmap(name);
                    pictureBox1.Image = OrgImg;
                    //Hiệu chỉnh lại kích thước cho dữ liệu hình ảnh
                    int height = 90;
                    int width = 90;
                    Size size = new Size(height, width);

                    //Tạo đối tượng dữ liệu ảnh (dạng Bitmap)
                    Bitmap Bmp = new Bitmap(OrgImg, size);
                    //Khởi tạo đối tượng xử lý ảnh
                    //ImageProcessing IP = new ImageProcessing();
                    //Khởi tạo dữ liệu đặc trưng của hình ảnh
                    string features = getFeatures(Bmp);

                    textBox1.Text = features;
                    DataProcessing dp = new DataProcessing();
                    string file = getFileClass();
                    features = features.Replace(" ", "");
                    features = features.Replace(",", "\t");
                    if (radioButton3.Checked == true)
                    {
                        features = (count + 1).ToString() + " " + "(" + features + ")" + " " + "(" + Path.GetFileName(name) + ")" +
                            " " + "(" + "http //sbir-hcm.vn/" + ")" + " " + "(" + Path.GetFileNameWithoutExtension(name) + ".xml" + ")" + " " +
                            "(" + Path.GetFileName(featureFile) + ")" + " "; 

                        Stanford Sf = new Stanford();
                        string folder = dp.getLastFolderName(name);
                        string ClassName = Sf.getClassFolder(Path.GetFileNameWithoutExtension(folder));
                        ClassName = ClassName.Trim();
                        ClassName = ClassName.Replace(" ", "_");
                        features += "(" + ClassName + ")";
                    }
                    else
                    {
                        features = (count + 1).ToString() + " " + "(" + features + ")" + " " + "(" + Path.GetFileName(name) + ")" +
                            " " + "(" + "http //sbir-hcm.vn/" + ")" + " " + "(" + Path.GetFileNameWithoutExtension(name) + ".xml" + ")" + " " +
                            "(" + Path.GetFileName(featureFile) + ")" + " " + "(" + dp.getClass(Path.GetFileNameWithoutExtension(name), file) + ")";
                    }

                    TextfileCluster tfc = new TextfileCluster();
                    tfc.WriteLineTextFile(features, featureFile);
                    tfc.WriteLineTextFile(features, featureFile2);
                    count++;
                    progressBar1.Value = count;
                    Application.DoEvents();
                }
                progressBar1.Value = filenames.Length;
            }
            MessageBox.Show("DONE!");
        }
        

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    FolderBrowserDialog folderDlg = new FolderBrowserDialog();
        //    string ImagePathFile = @"../../data/ImgPathCorel.txt";
        //    string DataPath = string.Empty;
        //    if (folderDlg.ShowDialog() == DialogResult.OK)
        //    {
        //        string[] filenames = Directory.GetFiles(folderDlg.SelectedPath, "*.jpg", SearchOption.AllDirectories);
        //        progressBar1.Minimum = 0;
        //        progressBar1.Maximum = filenames.Length;
        //        int count = 0;
        //        progressBar1.Value = count;
        //        foreach (string name in filenames)
        //        {
        //            DataPath = Path.GetFileName(name);
        //            DataPath += "\t" + "(" + "../../DBCOREL/" + getClass(Path.GetFileNameWithoutExtension(name)) + "/" + Path.GetFileName(name) + ")";
        //            DataPath += "\t" + "(" + "../../DBCOREL/" + getClass(Path.GetFileNameWithoutExtension(name)) + "/" + Path.GetFileNameWithoutExtension(name) + ".xml" + ")";
        //            DataPath += "\t" + "(" + getClass(Path.GetFileNameWithoutExtension(name)) + ")";
        //            TextfileCluster tfc = new TextfileCluster();
        //            tfc.WriteLineTextFile(DataPath, ImagePathFile);
        //            Application.DoEvents();
        //            count++;
        //            progressBar1.Value = count;
        //        }
        //        progressBar1.Value = filenames.Length;
        //        MessageBox.Show("DONE!");
        //    }
        //}
    }
}
