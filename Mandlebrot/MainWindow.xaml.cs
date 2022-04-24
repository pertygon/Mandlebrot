using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mandlebrot
{
    
    public partial class MainWindow : Window
    {
        static double X1 = -2.5;
        static double X2 = -1.5;
        static double X3 = 1;
        static double X4 = 1.5;

        public Rect DrawingArea = new Rect(new Point(X1,X2), new Point(X3,X4));
        public MainWindow()
        {
            InitializeComponent();
            Img.Source = DrawSet(DrawingArea);
        }
       
        //Mandlebrot function
        Int32 MandlebrotPattern(Complex GivenZ)
        {
            Int32 count = 0;
            Complex z = Complex.Zero;
            int Accuracy = 1000;
            while(count<Accuracy && z.Magnitude<2)
            {
                z = z * z + GivenZ;
                count++;
            }
            return count;
        }
        //Coloring pixels
        Color Coloring(int count)
        {
            Color RGB = new Color();
            RGB.B = (byte)(count / 100*25);
            count=  count % 100;
            RGB.G = (byte)(count / 10*25);
            RGB.R = (byte)(count % 10*25);
            RGB.A = 255;
            return RGB;
        }
    
        WriteableBitmap DrawSet(Rect area)
        {
            int PxHeight = (int)Img.Height;
            int PxWidth= (int)Img.Width;
            WriteableBitmap map = new WriteableBitmap(
                PxHeight,
                PxWidth,
                96,
                96,
                PixelFormats.Bgra32,
                null);
            //create array that contains information about color
            int BPP = map.Format.BitsPerPixel / 8;
            byte[] Pixels = new byte[PxHeight * PxWidth * BPP];
            int SRow = PxWidth* BPP;
            double xScale = (area.Right - area.Left)/PxWidth;
            double yScale = (area.Top - area.Bottom)/PxHeight;
            for(int i = 0; i < Pixels.Length;i+=BPP)
            {
                //calculate pixels to color
                int PixelXCords = i % SRow/BPP;
                int PixelYCords = i / SRow;
                double x = area.Left + PixelXCords * xScale;
                double y = area.Top - PixelYCords * yScale;
                Complex Cnumber=new Complex(x,y);
                int count = MandlebrotPattern(Cnumber);
                Color RGB = Coloring(count);
                Pixels[i] = RGB.B;
                Pixels[i + 1] = RGB.G;
                Pixels[i + 2] = RGB.R;
                Pixels[i + 3] = RGB.A;
            }
            map.WritePixels(new Int32Rect(0, 0, PxWidth, PxHeight), Pixels, SRow, 0);
            return map;
        }
    }

}
