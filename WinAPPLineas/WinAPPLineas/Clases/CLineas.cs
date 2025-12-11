using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinAPPLineas
{
    namespace WinAPPLineas
    {
        internal static class CLineas
        {
            // Inicio algoritmo DDA
            public static void DrawLineDDA(PictureBox picCanvas, int x0, int y0, int x1, int y1, Color color)
            {
                if (picCanvas == null) return;
                int w = Math.Max(1, picCanvas.Width);
                int h = Math.Max(1, picCanvas.Height);
                var bmp = new Bitmap(w, h);
                using (var g = System.Drawing.Graphics.FromImage(bmp)) // FIX: Usar el espacio de nombres correcto
                {
                    g.Clear(Color.White);
                }

                int dx = x1 - x0;
                int dy = y1 - y0;
                int steps = Math.Max(Math.Abs(dx), Math.Abs(dy));
                if (steps == 0)
                {
                    if (x0 >= 0 && x0 < w && y0 >= 0 && y0 < h)
                        bmp.SetPixel(x0, y0, color);
                    SwapAndAssignImage(picCanvas, bmp);
                    return;
                }

                double xi = dx / (double)steps;
                double yi = dy / (double)steps;
                double x = x0;
                double y = y0;

                for (int i = 0; i <= steps; i++)
                {
                    int px = (int)Math.Round(x);
                    int py = (int)Math.Round(y);
                    if (px >= 0 && px < w && py >= 0 && py < h)
                        bmp.SetPixel(px, py, color);
                    x += xi;
                    y += yi;
                }

                SwapAndAssignImage(picCanvas, bmp);
            }

            // Inicio algoritmo Midpoint (Bresenham)
            public static void DrawLineMidpoint(PictureBox picCanvas, int x0, int y0, int x1, int y1, Color color)
            {
                if (picCanvas == null) return;
                int w = Math.Max(1, picCanvas.Width);
                int h = Math.Max(1, picCanvas.Height);
                var bmp = new Bitmap(w, h);
                using (var g = System.Drawing.Graphics.FromImage(bmp)) // FIX: Usar el espacio de nombres correcto
                {
                    g.Clear(Color.White);
                }

                bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
                if (steep)
                {
                    Swap(ref x0, ref y0);
                    Swap(ref x1, ref y1);
                }

                if (x0 > x1)
                {
                    Swap(ref x0, ref x1);
                    Swap(ref y0, ref y1);
                }

                int dx = x1 - x0;
                int dy = Math.Abs(y1 - y0);
                int error = dx / 2;
                int ystep = (y0 < y1) ? 1 : -1;
                int y = y0;

                for (int x = x0; x <= x1; x++)
                {
                    int px = steep ? y : x;
                    int py = steep ? x : y;
                    if (px >= 0 && px < w && py >= 0 && py < h)
                        bmp.SetPixel(px, py, color);

                    error -= dy;
                    if (error < 0)
                    {
                        y += ystep;
                        error += dx;
                    }
                }

                SwapAndAssignImage(picCanvas, bmp);
            }

            // Inicio algoritmo Xiaolin Wu (línea antialiasing)
            public static void DrawLineXiaolinWu(PictureBox picCanvas, int x0, int y0, int x1, int y1, Color color)
            {
                if (picCanvas == null) return;
                int w = Math.Max(1, picCanvas.Width);
                int h = Math.Max(1, picCanvas.Height);
                var bmp = new Bitmap(w, h);
                using (var g = System.Drawing.Graphics.FromImage(bmp)) // FIX: Usar el espacio de nombres correcto
                {
                    g.Clear(Color.White);
                }

                // punto único
                if (x0 == x1 && y0 == y1)
                {
                    if (x0 >= 0 && x0 < w && y0 >= 0 && y0 < h)
                        bmp.SetPixel(x0, y0, color);
                    SwapAndAssignImage(picCanvas, bmp);
                    return;
                }

                Func<double, int> ipart = d => (int)Math.Floor(d);
                Func<double, double> fpart = d => d - Math.Floor(d);
                Func<double, double> rfpart = d => 1 - fpart(d);

                Action<int, int, double> plot = (px, py, c) =>
                {
                    if (px < 0 || px >= w || py < 0 || py >= h) return;
                    Color bg = bmp.GetPixel(px, py);
                    double a = Math.Max(0.0, Math.Min(1.0, c));
                    int r = (int)Math.Round(color.R * a + bg.R * (1 - a));
                    int gC = (int)Math.Round(color.G * a + bg.G * (1 - a));
                    int b = (int)Math.Round(color.B * a + bg.B * (1 - a));
                    r = Math.Min(255, Math.Max(0, r));
                    gC = Math.Min(255, Math.Max(0, gC));
                    b = Math.Min(255, Math.Max(0, b));
                    bmp.SetPixel(px, py, Color.FromArgb(255, r, gC, b));
                };

                bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
                if (steep)
                {
                    Swap(ref x0, ref y0);
                    Swap(ref x1, ref y1);
                }

                if (x0 > x1)
                {
                    Swap(ref x0, ref x1);
                    Swap(ref y0, ref y1);
                }

                double dx = x1 - x0;
                double dy = y1 - y0;
                double gradient = (dx == 0) ? 1.0 : dy / dx;

                // primer extremo
                double xend = Math.Round((double)x0);
                double yend = y0 + gradient * (xend - x0);
                double xgap = rfpart(x0 + 0.5);
                int xpxl1 = (int)xend;
                int ypxl1 = ipart(yend);
                if (steep)
                {
                    plot(ypxl1, xpxl1, rfpart(yend) * xgap);
                    plot(ypxl1 + 1, xpxl1, fpart(yend) * xgap);
                }
                else
                {
                    plot(xpxl1, ypxl1, rfpart(yend) * xgap);
                    plot(xpxl1, ypxl1 + 1, fpart(yend) * xgap);
                }
                double intery = yend + gradient;

                // segundo extremo
                xend = Math.Round((double)x1);
                yend = y1 + gradient * (xend - x1);
                xgap = fpart(x1 + 0.5);
                int xpxl2 = (int)xend;
                int ypxl2 = ipart(yend);
                if (steep)
                {
                    plot(ypxl2, xpxl2, rfpart(yend) * xgap);
                    plot(ypxl2 + 1, xpxl2, fpart(yend) * xgap);
                }
                else
                {
                    plot(xpxl2, ypxl2, rfpart(yend) * xgap);
                    plot(xpxl2, ypxl2 + 1, fpart(yend) * xgap);
                }

                // bucle principal
                if (steep)
                {
                    for (int x = xpxl1 + 1; x <= xpxl2 - 1; x++)
                    {
                        int y = ipart(intery);
                        plot(y, x, rfpart(intery));
                        plot(y + 1, x, fpart(intery));
                        intery += gradient;
                    }
                }
                else
                {
                    for (int x = xpxl1 + 1; x <= xpxl2 - 1; x++)
                    {
                        int y = ipart(intery);
                        plot(x, y, rfpart(intery));
                        plot(x, y + 1, fpart(intery));
                        intery += gradient;
                    }
                }

                SwapAndAssignImage(picCanvas, bmp);
            }

            private static void Swap(ref int a, ref int b)
            {
                int t = a; a = b; b = t;
            }

            // Asigna la imagen al PictureBox y libera la anterior para evitar fugas GDI.
            private static void SwapAndAssignImage(PictureBox picCanvas, Bitmap bmp)
            {
                var old = picCanvas.Image;
                picCanvas.Image = bmp;
                if (old != null && !ReferenceEquals(old, bmp))
                {
                    var disp = old as IDisposable;
                    disp?.Dispose();
                }
            }
        }
    }
}

