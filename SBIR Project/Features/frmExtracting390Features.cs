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
    public partial class frmExtracting390Features : Form
    {
        private static frmExtracting390Features _instance;

        public static frmExtracting390Features getInstance
        {
            get
            {
                if (_instance == null)
                    _instance = new frmExtracting390Features();
                return frmExtracting390Features._instance;
            }
            private set { frmExtracting390Features._instance = value; }
        }

        public frmExtracting390Features()
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

            //Lấy đặc trưng màu (histogram) theo Newton (đặc trưng màu sắc)
            int[] hisNewton = IP.getHistogramNewtonColor(Bmp);
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
            //Mask = IP.ToBlackwhite(Mask, IP.MaxBrightness(Mask)/3);
            pictureBox2.Image = Mask;

            //Lấy đối ảnh tượng và hình nền của ảnh ban đầu
            Bitmap BmpObject = IP.getRegionWhite(Bmp, Mask);
            Bitmap BmpBackground = IP.getRegionBlack(Bmp, Mask);
            pictureBox3.Image = BmpObject;
            pictureBox4.Image = BmpBackground;

            //Lấy đặc trưng diện tích của đối tượng (đặc trưng đối tượng)
            double areaObject = IP.getRegionArea(BmpObject);
            //double areaBground = 1.0 - areaObject; //IP.getRegionArea(BmpBackground);
            features += ", " + areaObject.ToString();// + ", " + areaBground.ToString();

            //Lấy vị trí tương đối của đối tượng theo trục X và trục Y
            double meanObjX = IP.getMeanX(BmpObject);
            double meanObjY = IP.getMeanY(BmpObject);
            features += ", " + meanObjX.ToString() + ", " + meanObjY.ToString();

            //Lấy vị trí tương đối của hình nền theo trục X và trục Y
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

            ////Lấy đặc trưng hình dạng của đường biên ảnh
            //bmpResize = IP.Resize2(BmpBoundary, 4, 4);
            //features += ", " + IP.getRegionArea(bmpResize);

            //Lấy đặc trưng chu vi của đối tượng
            double cirObject = IP.getRegionArea(BmpBoundary);
            //double scalObject = cirObject / areaObject;
            //double scalBground = cirObject / areaBground;
            //if (scalObject > 1.0) scalObject = 1.0;
            //if (scalBground > 1.0) scalBground = 1.0;
            //features += ", " + scalObject.ToString() + ", " + scalBground.ToString();
            features += ", " + cirObject.ToString();

            //Phép lọc Sobel để lấy đường biên đối tượng và vân ảnh của bề mặt đối tượng
            Bitmap BmpSobel = new Bitmap(Bmp, size);
            BmpSobel = IP.ImageSobelFilterColor(BmpSobel);
            pictureBox6.Image = BmpSobel;

            ////Lấy đặc trưng chu vi của đối tượng theo phép lọc Sobel
            //cirObject = IP.getRegionArea(BmpSobel);
            //features += ", " + cirObject.ToString();

            ////Lấy cường độ đặc trưng của ảnh theo đối các điểm ảnh theo láng giềng theo phép lọc Sobel
            //bmpResize = IP.Resize(BmpSobel, 3, 3);
            //features += ", " + IP.getStrIntensy(bmpResize);

            //Thực hiện phép lọc Laplace để lấy các đường nét của ảnh
            Bitmap BmpLaplace = new Bitmap(Bmp, size);
            BmpLaplace = IP.ImageFilterColor(BmpLaplace, IP.LaplaceF1());
            pictureBox7.Image = BmpLaplace;

            ////Lấy đặc trưng chu vi của đối tượng theo phép lọc Laplace
            //cirObject = IP.getRegionArea(BmpLaplace);
            //features += ", " + cirObject.ToString();

            //Lấy cường độ đường nét của ảnh theo phép lọc Laplace
            bmpResize = IP.Resize(BmpLaplace, 3, 3);
            //features += ", " + IP.getStrIntensy(bmpResize);

            //Thực hiện phép lọc thông cao cho ảnh ban đầu (chỉ cho những tần số cao đi qua, loại bỏ những tần số thấp)
            //Phép lọc thông cao dùng để lấy ảnh đường nét
            Bitmap BmpHpf = new Bitmap(Bmp, size);
            BmpHpf = IP.ImageFilterColor(BmpHpf, IP.HPF1());
            //bmpResize = IP.getMaxPoolingNtimes(Bmp6, 3, 3);
            //bmpResize = IP.Resize(Bmp6, 3, 3);
            //features += ", " + IP.getStrIntensy(bmpResize);
            pictureBox8.Image = BmpHpf;

            ////Sử dụng thư viện Accord để tính giá trị moment
            //HuMoments hu = new HuMoments(Bmp);
            ////hu.Order = 7;
            //double M1 = Math.Round(hu.I1, 6);
            //features += M1.ToString();

            ////Thực hiện phép lọc Gaussian để nâng cường độ cho điểm ảnh dựa trên láng giềng
            ////láng giềng càng gần thì trọng số càng cao
            //Bitmap Bmp7 = new Bitmap(Bmp, size);
            //Bmp7 = IP.ImageFilterColor(Bmp6, IP.GF1());
            //bmpResize = IP.getMaxPoolingNtimes(Bmp7, 3, 3);
            ////bmpResize = IP.Resize(Bmp7, 3, 3);
            //features += ", " + IP.getStrIntensy(bmpResize);
            //pictureBox7.Image = Bmp7;

            //Thực hiện phép tăng mẫu và giảm mẫu để lấy đối tượng trên vùng
            Bitmap BmpPooling = new Bitmap(Bmp, size);
            BmpPooling = IP.MaxPooling(BmpPooling);
            BmpPooling = IP.MaxPooling(BmpPooling);
            //Bmp9 = IP.MaxPooling(Bmp9);
            //Bmp9 = IP.MaxPooling(Bmp9);
            //Bmp9 = IP.MaxPooling(Bmp9);
            //Bmp9 = IP.UnMaxPooling(Bmp9);
            //Bmp9 = IP.UnMaxPooling(Bmp9);
            BmpPooling = IP.UnMaxPooling(BmpPooling);
            BmpPooling = IP.UnMaxPooling(BmpPooling);
            pictureBox9.Image = BmpPooling;

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

            //Lấy đặc trưng màu (histogram) theo Newton (đặc trưng màu sắc)
            int[] hisNewton = IP.getHistogramNewtonColor(Bmp);
            double[] hisNorm = IP.getHisNormal(hisNewton);
            string strHis = Arr2string(hisNorm);
            features += strHis;

            //Lấy cường độ đặc trưng của ảnh theo đối các điểm ảnh theo láng giềng (đặc trưng hình dạng)
            bmpResize = IP.Resize(Bmp, 3, 3);
            features += ", " + IP.getStrIntensy(bmpResize);

            //Tạo đối tượng ảnh theo độ tương phản (độ sáng tối) dựa trên phép biến đổi gama
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
            //double areaBground = 1.0 - areaObject; //IP.getRegionArea(BmpBackground);
            features += ", " + areaObject.ToString();// + ", " + areaBground.ToString();

            //Lấy vị trí tương đối của đối tượng theo trục X và trục Y
            double meanObjX = IP.getMeanX(BmpObject);
            double meanObjY = IP.getMeanY(BmpObject);
            features += ", " + meanObjX.ToString() + ", " + meanObjY.ToString();

            //Lấy vị trí tương đối của hình nền theo trục X và trục Y
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

            ////Lấy đặc trưng hình dạng của đường biên ảnh
            //bmpResize = IP.Resize2(BmpBoundary, 4, 4);
            //features += ", " + IP.getRegionArea(bmpResize);

            //Lấy đặc trưng chu vi của đối tượng (hình nền) so với diện tích đối tượng (hình nền)
            double cirObject = IP.getRegionArea(BmpBoundary);
            //double scalObject = cirObject / areaObject;
            //double scalBground = cirObject / areaBground;
            //if (scalObject > 1.0) scalObject = 1.0;
            //if (scalBground > 1.0) scalBground = 1.0;
            //features += ", " + scalObject.ToString() + ", " + scalBground.ToString();
            features += ", " + cirObject.ToString();
            
            ////Phép lọc Sobel để lấy đường biên đối tượng và vân ảnh của bề mặt đối tượng
            //Bitmap BmpSobel = new Bitmap(Bmp, size);
            //BmpSobel = IP.ImageSobelFilterColor(BmpSobel);

            ////Lấy đặc trưng chu vi của đối tượng theo phép lọc Sobel
            //cirObject = IP.getRegionArea(BmpSobel);
            //features += ", " + cirObject.ToString();

            ////Lấy cường độ đặc trưng của ảnh theo đối các điểm ảnh theo láng giềng theo phép lọc Sobel
            //bmpResize = IP.Resize(BmpSobel, 3, 3);
            //features += ", " + IP.getStrIntensy(bmpResize);

            ////Thực hiện phép lọc Laplace để lấy các đường nét của ảnh
            //Bitmap BmpLaplace = new Bitmap(Bmp, size);
            //BmpLaplace = IP.ImageFilterColor(BmpLaplace, IP.LaplaceF1());

            ////Lấy đặc trưng chu vi của đối tượng theo phép lọc Laplace
            //cirObject = IP.getRegionArea(BmpLaplace);
            //features += ", " + cirObject.ToString();

            ////Lấy cường độ đường nét của ảnh theo phép lọc Laplace
            //bmpResize = IP.Resize(BmpLaplace, 3, 3);
            //features += ", " + IP.getStrIntensy(bmpResize);

            return features;
        }

        public void show9Regions(List<Bitmap> lstBmp)
        {
            pictureBox10.Image = lstBmp.ElementAt(0);
            pictureBox11.Image = lstBmp.ElementAt(1);
            pictureBox12.Image = lstBmp.ElementAt(2);
            pictureBox13.Image = lstBmp.ElementAt(3);
            pictureBox14.Image = lstBmp.ElementAt(4);
            pictureBox15.Image = lstBmp.ElementAt(5);
            pictureBox16.Image = lstBmp.ElementAt(6);
            pictureBox17.Image = lstBmp.ElementAt(7);
            pictureBox18.Image = lstBmp.ElementAt(8);
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

                    List<Bitmap> lstBmpRegions = getImageRegions(Bmp);
                    show9Regions(lstBmpRegions);

                    //features += ", " + getFeatutes9Region(Bmp);

                    //foreach (Bitmap b in lstBmpRegions)
                    //{
                    //    string f = getFeatutes9Region(b);
                    //    features += ", " + f;
                    //}
                    textBox1.Text = features;


                    ////Tính các Hu's moment từ moment trung tâm                  //CentralMoments moments = new CentralMoments(Bmp);
                    //double inv = 1.0 / moments.Mu00;
                    //double inv2 = 1.0 / (moments.Mu00 * moments.Mu00);
                    //double inv5d2 = System.Math.Sqrt(inv2 * inv2 * inv);
                    //float n20 = (float)(moments.Mu20 * inv2);
                    //float n02 = (float)(moments.Mu02 * inv2);
                    //float n11 = (float)(moments.Mu11 * inv2);

                    //float n21 = (float)(moments.Mu21 * inv5d2);
                    //float n12 = (float)(moments.Mu12 * inv5d2);
                    //float n30 = (float)(moments.Mu30 * inv5d2);
                    //float n03 = (float)(moments.Mu03 * inv5d2);
                    ////   (η20 + η02)
                    //float I1 = (n20 + n02);
                    ////   (η20 − η02)²              + 4    η11²
                    //float I2 = (n20 - n02) * (n20 - n02) + 4 * (n11 * n11);
                    ////   (η30 − 3   η12)²
                    //float I3 = (n30 - 3 * n12) * (n30 - 3 * n12)
                    //   // + (3   η21 − η03)²
                    //   + (3 * n21 - n03) * (3 * n21 - n03);
                    ////   (η30 + η12)²              + (η21 + η03)²
                    //float I4 = (n30 + n12) * (n30 + n12) + (n21 + n03) * (n21 + n03);
                    ////   (η30 − 3   η12)   (η30 + η12)   [(η30 + η12)²               −3   (η21 + η03)²             ]
                    //float I5 = (n30 - 3 * n12) * (n30 + n12) * ((n30 + n12) * (n30 + n12) - 3 * (n21 + n03) * (n21 + n03))
                    //   //   (3   η21 − η03)   (η21 + η03)   [3   (η30 + η12)²              − (η21 + η03)²             ]
                    //   + (3 * n21 - n03) * (n21 + n03) * (3 * (n30 + n12) * (n30 + n12) - (n21 + n03) * (n21 + n03));
                    ////   (η20 − η02)   [(η30 + η12)²              − (η21 + η03)²             ]
                    //float I6 = (n20 - n02) * ((n30 + n12) * (n30 + n12) - (n21 + n03) * (n21 + n03))
                    //     //  + 4   η11   (η30 + η12)   (η21 + η03)
                    //     + 4 * n11 * (n30 + n12) * (n21 + n03);
                    ////   (3   η21 − η03)   (η30 + η12)                 [(η30 + η12)²              − 3   (η21 + η03)²             ]
                    //float I7 = (3 * n21 - n03) * (n30 + n12) * (n30 + n12) * ((n30 + n12) * (n30 + n12) - 3 * (n21 + n03) * (n21 + n03))
                    //   // - (η30 − 3   η12)   (η21 + η03)   [3   (η30 + η12)²              − (η21 + η03)²             ]
                    //   - (n30 - 3 * n12) * (n21 + n03) * (3 * (n30 + n12) * (n30 + n12) - (n21 + n03) * (n21 + n03));
                    //features += I1.ToString() + ", " + I2.ToString() + ", " + I3.ToString() + ", " + I4.ToString() + ", " +
                    //        I5.ToString() + ", " + I6.ToString() + ", " + I7.ToString();
                    //features += n20.ToString() + ", " + n02.ToString() + ", " + n11.ToString() + ", " + n21.ToString() + ", " +
                    //        n12.ToString() + ", " + n30.ToString() + ", " + n03.ToString();

                    //float M2 = hu.I2;
                    //float M3 = hu.I3;
                    //float M4 = hu.I4;
                    //float M5 = hu.I5;
                    //float M6 = hu.I6;
                    //float M7 = hu.I7;

                    //features += ", " + M2.ToString() + ", " + M3.ToString() + ", " + M4.ToString() + ", " +
                    //        M5.ToString() + ", " + M6.ToString() + ", " + M7.ToString();

                    //pictureBox7.Image = IP.Resize2(Bmp,64, 64);

                    //pictureBox8.Image = IP.Resize2(Bmp5, 64, 64);
                    //pictureBox7.Image = IP.Resize2(Bmp4, 4,4);

                    ////Phép lọc Laplace để lấy đường biên hình nền
                    //Bitmap Bmp5 = new Bitmap(Bmp3,size);
                    //Bmp5 = IP.ImageFilterColor(Bmp5, IP.LaplaceF1());
                    //pictureBox5.Image = Bmp5;

                    //Lấy đặc trưng hình dạng của đối tượng trên ảnh
                    //Bitmap Bmp6 = new Bitmap(Bmp2,size);
                    //Bmp6 = IP.Skeletonization(Bmp6);
                    //pictureBox5.Image = Bmp6;

                    ////Lấy đặc trưng hình dạng của hình nền
                    //Bitmap Bmp7 = new Bitmap(Bmp3,size);
                    //Bmp7 = IP.Skeletonization(Bmp7);
                    //pictureBox7.Image = Bmp7;

                    //pictureBox8.Image = IP.Resize2(Bmp,4,4);

                    //// Create a new Gabor filter
                    //GaborFilter filter = new GaborFilter();
                    //Bitmap Bmp9 = (Bitmap)System.Drawing.Bitmap.FromFile(ImgPath);
                    ////Bitmap Bmp9 = (Bitmap)Image.FromFile(ImgPath);
                    //Bmp9 = filter.Apply(Bmp9);
                    //pictureBox9.Image = Bmp9;



                    //Bitmap Bmp4 = IP.LoG12x12(Bmp2);
                    //Bitmap Bmp4b = IP.LoG12x12(Bmp3);
                    //Bitmap Bmp4 = IP.AddImages(Bmp4a, Bmp4b);
                    //pictureBox4.Image = IP.getRegionImage(OrgImg, Bmp4);

                    //Lấy Histogram MPEG7 của ảnh ban đầu
                    //int[] hisMPEG7 = IP.getHistogramMPEG7Color(OrgImg);
                    //double[] hisNorm = IP.getHisNormal(hisMPEG7);
                    //string strHis = Arr2string(hisNorm);
                    //textBox1.Text = "(" + strHis + ")";

                    ////Lấy histogram MPEG7 của ảnh đối tượng
                    //hisMPEG7 = IP.getHistogramMPEG7Color(BmpObject);
                    //hisNorm = IP.getHisNormal(hisMPEG7);
                    //strHis = "(" + Arr2string(hisNorm) + ")";
                    //textBox1.Text += strHis;
                    ////Lấy histogram MPEG7 của hình nền
                    //hisMPEG7 = IP.getHistogramMPEG7Color(BmpBackground);
                    //hisNorm = IP.getHisNormal(hisMPEG7);
                    //strHis = "(" + Arr2string(hisNorm) + ")";
                    //textBox1.Text += strHis;


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

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            //string featureFile = @"../../data/listCorelED.txt";
            string featureFile = @"../../data/listStandfordDogED.txt";
            //string featureFile = @"../../data/listWangED.txt";
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
                    ImageProcessing IP = new ImageProcessing();
                    //Khởi tạo dữ liệu đặc trưng của hình ảnh
                    string features = getFeatures(Bmp);

                    List<Bitmap> lstBmpRegions = getImageRegions(Bmp);
                    show9Regions(lstBmpRegions);

                    features += ", " + getFeatutes9Region(Bmp);


                    //foreach (Bitmap b in lstBmpRegions)
                    //{
                    //    string f = getFeatures2(b);
                    //    features += ", " + f;
                    //}
                    //Tuấn: Show vector lên form
                    textBox1.Text = "(" + features + ")";
                    //Tuấn: Tạo cấu trúc dữ liệu để lưu xuống file
                    features = features.Replace(" ", "");
                    features = features.Replace(",", "\t");
                    features = (count + 1).ToString() + " " + "(" + features + ")" + " " + "(" + Path.GetFileName(name) + ")" +
                        " " + "(" + "http //sbir-hcm.vn/" + ")" + " " + "(" + Path.GetFileNameWithoutExtension(name) + ".xml" + ")" + " " +
                        "(" + Path.GetFileName(featureFile) + ")" + " " + "(" + getClass(Path.GetFileNameWithoutExtension(name)) + ")";

                    TextfileCluster tfc = new TextfileCluster();
                    //Tuấn: Ghi xuống file listCorelED.txt
                    tfc.WriteLineTextFile(features, featureFile);
                    Application.DoEvents();
                    count++;
                    progressBar1.Value = count;
                }
                progressBar1.Value = filenames.Length;
            }
            MessageBox.Show("DONE!");
        }
        private string getClass(string nName)
        {
            //string file = @"../../data/COREL-Class.txt";
            string file = @"../../data/StanfordDogs-Class.txt";
            //string file = @"../../data/Wang-Class.txt";

            TextfileCluster tfc = new TextfileCluster(file);
            string[] Lines = tfc.ReadAllLine();
            string classname = string.Empty;
            ClusterNode cn = new ClusterNode();
            foreach (string line in Lines)
            {
                char[] delimiters = new char[] { '\t', '\r', '\n', ';', '!', ':', ',', ' ' };
                string[] words = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                words = cn.RemoveBlank(words);
                if (words.Contains(nName))
                    classname += words[1].Trim() + " ";
            }
            return classname.Trim();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            string ImagePathFile = @"../../data/ImgPathCorel.txt";
            string DataPath = string.Empty;
            if (folderDlg.ShowDialog() == DialogResult.OK)
            {
                string[] filenames = Directory.GetFiles(folderDlg.SelectedPath, "*.jpg", SearchOption.AllDirectories);
                progressBar1.Minimum = 0;
                progressBar1.Maximum = filenames.Length;
                int count = 0;
                progressBar1.Value = count;
                foreach (string name in filenames)
                {
                    //Tuấn: "../../DBCOREL/" ??? 
                    DataPath = Path.GetFileName(name);
                    DataPath += "\t" + "(" + "../../DBCOREL/" + getClass(Path.GetFileNameWithoutExtension(name)) + "/" + Path.GetFileName(name) + ")";
                    DataPath += "\t" + "(" + "../../DBCOREL/" + getClass(Path.GetFileNameWithoutExtension(name)) + "/" + Path.GetFileNameWithoutExtension(name) + ".xml" + ")";
                    DataPath += "\t" + "(" + getClass(Path.GetFileNameWithoutExtension(name)) + ")";
                    TextfileCluster tfc = new TextfileCluster();
                    tfc.WriteLineTextFile(DataPath, ImagePathFile);
                    Application.DoEvents();
                    count++;
                    progressBar1.Value = count;
                }
                progressBar1.Value = filenames.Length;
                MessageBox.Show("DONE!");
            }
        }
    }
}
