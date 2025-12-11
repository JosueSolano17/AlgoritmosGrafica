using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace WinAPPLineas.Clases
{
    internal static class CRelleno
    {
        private static int Clamp(int v, int min, int max)
        {
            if (v < min) return min;
            if (v > max) return max;
            return v;
        }

        public enum NeighborStatus { Occupied, Free, Enqueued, Backtrack }
        public delegate Task NeighborCallback(int fromX, int fromY, int toX, int toY, NeighborStatus status, CancellationToken token);
        public delegate Task PixelFilledCallback(int x, int y, CancellationToken token);

        public static async Task FloodFillAnimated(Bitmap bmp, int x, int y, Color fillColor, int delayMs, NeighborCallback onNeighbor, PixelFilledCallback onPixelFilled, CancellationToken token)
        {
            if (bmp == null) return;
            if (!IsInside(bmp, x, y)) return;

            int target = bmp.GetPixel(x, y).ToArgb();
            int replacement = fillColor.ToArgb();
            if (target == replacement) return;

            var stack = new Stack<Point>();
            var queued = new HashSet<Point>();
            var parent = new Dictionary<Point, Point>();

            var start = new Point(x, y);
            stack.Push(start);
            queued.Add(start);
            parent[start] = new Point(-1, -1);

            var directions = new (int dx, int dy)[] { (1, 0), (-1, 0), (0, 1), (0, -1) };

            while (stack.Count > 0)
            {
                token.ThrowIfCancellationRequested();

                var p = stack.Pop();
                queued.Remove(p);

                int px = p.X, py = p.Y;
                if (!IsInside(bmp, px, py)) continue;
                if (bmp.GetPixel(px, py).ToArgb() != target) continue;

                var newNeighbors = new List<Point>();

                foreach (var d in directions)
                {
                    int nx = px + d.dx;
                    int ny = py + d.dy;
                    var np = new Point(nx, ny);

                    if (!IsInside(bmp, nx, ny))
                    {
                        if (onNeighbor != null) await onNeighbor(px, py, nx, ny, NeighborStatus.Occupied, token);
                        continue;
                    }

                    int current = bmp.GetPixel(nx, ny).ToArgb();
                    if (current == target || current == replacement)
                    {
                        if (onNeighbor != null) await onNeighbor(px, py, nx, ny, NeighborStatus.Occupied, token);
                        continue;
                    }

                    if (queued.Contains(np))
                    {
                        if (onNeighbor != null) await onNeighbor(px, py, nx, ny, NeighborStatus.Enqueued, token);
                        continue;
                    }

                    if (onNeighbor != null) await onNeighbor(px, py, nx, ny, NeighborStatus.Free, token);
                    newNeighbors.Add(np);
                }

                foreach (var np in newNeighbors)
                {
                    if (!queued.Contains(np) && bmp.GetPixel(np.X, np.Y).ToArgb() == target)
                    {
                        queued.Add(np);
                        parent[np] = p;
                        stack.Push(np);
                    }
                }

                bmp.SetPixel(px, py, fillColor);
                if (onPixelFilled != null) await onPixelFilled(px, py, token);

                if (delayMs > 0) await Task.Delay(delayMs, token);

                if (newNeighbors.Count == 0)
                {
                    if (parent.ContainsKey(p) && parent[p].X >= 0)
                    {
                        var par = parent[p];
                        if (onNeighbor != null) await onNeighbor(px, py, par.X, par.Y, NeighborStatus.Backtrack, token);
                        if (delayMs > 0) await Task.Delay(delayMs, token);
                    }
                }
            }
        }

        public static void FloodFill(Bitmap bmp, int x, int y, Color fillColor)
        {
            if (bmp == null) return;
            if (!IsInside(bmp, x, y)) return;

            int target = bmp.GetPixel(x, y).ToArgb();
            int replacement = fillColor.ToArgb();
            if (target == replacement) return;

            var stack = new Stack<Point>();
            stack.Push(new Point(x, y));

            while (stack.Count > 0)
            {
                Point p = stack.Pop();
                int px = p.X, py = p.Y;

                if (!IsInside(bmp, px, py)) continue;
                if (bmp.GetPixel(px, py).ToArgb() != target) continue;

                bmp.SetPixel(px, py, fillColor);

                stack.Push(new Point(px + 1, py));
                stack.Push(new Point(px - 1, py));
                stack.Push(new Point(px, py + 1));
                stack.Push(new Point(px, py - 1));
            }
        }

        public static void BoundaryFill(Bitmap bmp, int x, int y, Color fillColor, Color boundaryColor)
        {
            if (bmp == null) return;
            if (!IsInside(bmp, x, y)) return;

            int boundary = boundaryColor.ToArgb();
            int replacement = fillColor.ToArgb();

            var stack = new Stack<Point>();
            stack.Push(new Point(x, y));

            while (stack.Count > 0)
            {
                Point p = stack.Pop();
                int px = p.X, py = p.Y;

                if (!IsInside(bmp, px, py)) continue;

                int current = bmp.GetPixel(px, py).ToArgb();
                if (current == boundary || current == replacement) continue;

                bmp.SetPixel(px, py, fillColor);

                stack.Push(new Point(px + 1, py));
                stack.Push(new Point(px - 1, py));
                stack.Push(new Point(px, py + 1));
                stack.Push(new Point(px, py - 1));
            }
        }
        public static async Task BoundaryFillAnimated(
       Bitmap bmp,
       int x,
       int y,
       Color fillColor,
       Color boundaryColor,
       int delayMs,
       NeighborCallback onNeighbor,
       PixelFilledCallback onPixelFilled,
       CancellationToken token)
        {
            if (bmp == null) return;
            if (!IsInside(bmp, x, y)) return;

            int boundary = boundaryColor.ToArgb();
            int replacement = fillColor.ToArgb();

            var stack = new Stack<Point>();
            var queued = new HashSet<Point>();
            var parent = new Dictionary<Point, Point>();

            var start = new Point(x, y);
            stack.Push(start);
            queued.Add(start);
            parent[start] = new Point(-1, -1);

            var directions = new (int dx, int dy)[] { (1, 0), (-1, 0), (0, 1), (0, -1) };

            while (stack.Count > 0)
            {
                token.ThrowIfCancellationRequested();

                var p = stack.Pop();
                queued.Remove(p);

                int px = p.X, py = p.Y;
                if (!IsInside(bmp, px, py)) continue;

                int current = bmp.GetPixel(px, py).ToArgb();
                if (current == boundary || current == replacement) continue;

                var newNeighbors = new List<Point>();

                foreach (var d in directions)
                {
                    int nx = px + d.dx;
                    int ny = py + d.dy;
                    var np = new Point(nx, ny);

                    if (!IsInside(bmp, nx, ny))
                    {
                        if (onNeighbor != null) await onNeighbor(px, py, nx, ny, NeighborStatus.Occupied, token);
                        continue;
                    }

                    int nc = bmp.GetPixel(nx, ny).ToArgb();

                    if (nc == boundary || nc == replacement)
                    {
                        if (onNeighbor != null) await onNeighbor(px, py, nx, ny, NeighborStatus.Occupied, token);
                        continue;
                    }

                    if (queued.Contains(np))
                    {
                        if (onNeighbor != null) await onNeighbor(px, py, nx, ny, NeighborStatus.Enqueued, token);
                        continue;
                    }

                    if (onNeighbor != null) await onNeighbor(px, py, nx, ny, NeighborStatus.Free, token);
                    newNeighbors.Add(np);
                }

                foreach (var np in newNeighbors)
                {
                    if (!queued.Contains(np))
                    {
                        queued.Add(np);
                        parent[np] = p;
                        stack.Push(np);
                    }
                }

                bmp.SetPixel(px, py, fillColor);
                if (onPixelFilled != null) await onPixelFilled(px, py, token);

                if (delayMs > 0)
                    await Task.Delay(delayMs, token);

                if (newNeighbors.Count == 0)
                {
                    if (parent.ContainsKey(p) && parent[p].X >= 0)
                    {
                        var par = parent[p];
                        if (onNeighbor != null)
                            await onNeighbor(px, py, par.X, par.Y, NeighborStatus.Backtrack, token);

                        if (delayMs > 0)
                            await Task.Delay(delayMs, token);
                    }
                }
            }
        }



        public static void ScanlineFill(Bitmap bmp, int x, int y, Color fillColor)
        {
            if (bmp == null) return;
            if (!IsInside(bmp, x, y)) return;

            int target = bmp.GetPixel(x, y).ToArgb();
            int replacement = fillColor.ToArgb();
            if (target == replacement) return;

            var stack = new Stack<Point>();
            stack.Push(new Point(x, y));

            while (stack.Count > 0)
            {
                Point p = stack.Pop();
                int px = p.X;
                int py = p.Y;

                if (!IsInside(bmp, px, py)) continue;
                if (bmp.GetPixel(px, py).ToArgb() != target) continue;

                int left = px;
                while (left >= 0 && bmp.GetPixel(left, py).ToArgb() == target) left--;
                left++;

                int right = px;
                while (right < bmp.Width && bmp.GetPixel(right, py).ToArgb() == target) right++;
                right--;

                for (int i = left; i <= right; i++)
                {
                    bmp.SetPixel(i, py, fillColor);

                    if (py - 1 >= 0 && bmp.GetPixel(i, py - 1).ToArgb() == target) stack.Push(new Point(i, py - 1));
                    if (py + 1 < bmp.Height && bmp.GetPixel(i, py + 1).ToArgb() == target) stack.Push(new Point(i, py + 1));
                }
            }
        }
        public static async Task ScanlineFillAnimated(
    Bitmap bmp,
    int x,
    int y,
    Color fillColor,
    int delayMs,
    NeighborCallback onNeighbor,
    PixelFilledCallback onPixelFilled,
    CancellationToken token)
        {
            if (bmp == null) return;
            if (!IsInside(bmp, x, y)) return;

            int target = bmp.GetPixel(x, y).ToArgb();
            int replacement = fillColor.ToArgb();
            if (target == replacement) return;

            var stack = new Stack<Point>();
            stack.Push(new Point(x, y));

            while (stack.Count > 0)
            {
                token.ThrowIfCancellationRequested();
                Point p = stack.Pop();
                int px = p.X;
                int py = p.Y;

                if (!IsInside(bmp, px, py)) continue;
                if (bmp.GetPixel(px, py).ToArgb() != target) continue;

                int left = px;
                while (left >= 0 && bmp.GetPixel(left, py).ToArgb() == target)
                {
                    await onNeighbor?.Invoke(px, py, left, py, NeighborStatus.Free, token);
                    left--;
                }
                left++;

               
                int right = px;
                while (right < bmp.Width && bmp.GetPixel(right, py).ToArgb() == target)
                {
                    await onNeighbor?.Invoke(px, py, right, py, NeighborStatus.Free, token);
                    right++;
                }
                right--;

                
                for (int i = left; i <= right; i++)
                {
                    bmp.SetPixel(i, py, fillColor);
                    await onPixelFilled?.Invoke(i, py, token);
                    if (delayMs > 0) await Task.Delay(delayMs, token);
                }

          
                for (int i = left; i <= right; i++)
                {
                    int up = py - 1;
                    int down = py + 1;

                    if (IsInside(bmp, i, up))
                    {
                        int pix = bmp.GetPixel(i, up).ToArgb();
                        if (pix == target)
                        {
                            await onNeighbor?.Invoke(i, py, i, up, NeighborStatus.Free, token);
                            stack.Push(new Point(i, up));
                        }
                        else
                        {
                            await onNeighbor?.Invoke(i, py, i, up, NeighborStatus.Occupied, token);
                        }
                    }

                    if (IsInside(bmp, i, down))
                    {
                        int pix2 = bmp.GetPixel(i, down).ToArgb();
                        if (pix2 == target)
                        {
                            await onNeighbor?.Invoke(i, py, i, down, NeighborStatus.Free, token);
                            stack.Push(new Point(i, down));
                        }
                        else
                        {
                            await onNeighbor?.Invoke(i, py, i, down, NeighborStatus.Occupied, token);
                        }
                    }
                }
            }
        }


        private static bool IsInside(Bitmap bmp, int x, int y)
        {
            return bmp != null && x >= 0 && x < bmp.Width && y >= 0 && y < bmp.Height;
        }
    }
}