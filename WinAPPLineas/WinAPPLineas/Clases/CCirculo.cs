using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAPPLineas
{
    internal class CCirculo
    {
        public static void DrawCircleMidpoint(PictureBox picCanvas, int xc, int yc, int r, Color color)
        {
            if (picCanvas == null) return;

            int w = Math.Max(1, picCanvas.Width);
            int h = Math.Max(1, picCanvas.Height);
            Bitmap bmp = new Bitmap(w, h);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
            }

            int x = 0;
            int y = r;
            int p = 1 - r;


            Plot8Points(bmp, xc, yc, x, y, color);


            while (x < y)
            {
                x++;

                if (p < 0)
                {

                    p = p + 2 * x + 1;
                }
                else
                {

                    y--;
                    p = p + 2 * (x - y) + 1;
                }

                Plot8Points(bmp, xc, yc, x, y, color);
            }


            var old = picCanvas.Image;
            picCanvas.Image = bmp;
            old?.Dispose();
        }

        // Algoritmo Paramétrico (Circle Parametric Equation)
        public static void DrawCircleParametric(PictureBox picCanvas, int xc, int yc, int r, Color color)
        {
            if (picCanvas == null) return;
            if (r <= 0) return;

            int w = Math.Max(1, picCanvas.Width);
            int h = Math.Max(1, picCanvas.Height);
            Bitmap bmp = new Bitmap(w, h);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
            }

            // Paso angular basado en el radio para aproximar pixeles contiguos
            double step = Math.Max(0.01, 1.0 / r); // evita pasos demasiado grandes en círculos grandes y división por cero
            for (double theta = 0.0; theta < 2.0 * Math.PI; theta += step)
            {
                int x = (int)Math.Round(xc + r * Math.Cos(theta));
                int y = (int)Math.Round(yc + r * Math.Sin(theta));
                SetPixelSafe(bmp, x, y, color);
            }

            // Asegurar el cierre exacto
            SetPixelSafe(bmp, xc + r, yc, color);

            var old = picCanvas.Image;
            picCanvas.Image = bmp;
            old?.Dispose();
        }

        // Algoritmo de Bresenham para Círculos (Circle Bresenham Algorithm)
        public static void DrawCircleBresenham(PictureBox picCanvas, int xc, int yc, int r, Color color)
        {
            if (picCanvas == null) return;
            if (r <= 0) return;

            int w = Math.Max(1, picCanvas.Width);
            int h = Math.Max(1, picCanvas.Height);
            Bitmap bmp = new Bitmap(w, h);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
            }

            int x = 0;
            int y = r;
            int d = 3 - 2 * r; // parámetro de decisión inicial típico para Bresenham

            Plot8Points(bmp, xc, yc, x, y, color);

            while (x <= y)
            {
                x++;
                if (d <= 0)
                {
                    d = d + 4 * x + 6;
                }
                else
                {
                    y--;
                    d = d + 4 * (x - y) + 10;
                }
                Plot8Points(bmp, xc, yc, x, y, color);
            }

            var old = picCanvas.Image;
            picCanvas.Image = bmp;
            old?.Dispose();
        }

        private static void Plot8Points(Bitmap bmp, int xc, int yc, int x, int y, Color color)
        {
            SetPixelSafe(bmp, xc + x, yc + y, color);
            SetPixelSafe(bmp, xc - x, yc + y, color);
            SetPixelSafe(bmp, xc + x, yc - y, color);
            SetPixelSafe(bmp, xc - x, yc - y, color);

            SetPixelSafe(bmp, xc + y, yc + x, color);
            SetPixelSafe(bmp, xc - y, yc + x, color);
            SetPixelSafe(bmp, xc + y, yc - x, color);
            SetPixelSafe(bmp, xc - y, yc - x, color);
        }

        private static void SetPixelSafe(Bitmap bmp, int x, int y, Color c)
        {
            if (x >= 0 && x < bmp.Width && y >= 0 && y < bmp.Height)
                bmp.SetPixel(x, y, c);
        }
    }
}
