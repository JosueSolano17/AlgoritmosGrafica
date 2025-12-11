using System;
using System.Collections.Generic;

namespace WinAPPLineas.Clases
{
    public class CohenSutherlandStep
    {
        public enum StepAction
        {
            Start,
            CodesComputed,
            TriviallyAccepted,
            TriviallyRejected,
            IntersectionComputed,
            EndpointUpdated,
            FinalAccepted,
            FinalRejected
        }

        public StepAction Action { get; set; }
        public double X0 { get; set; }
        public double Y0 { get; set; }
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public int Outcode0 { get; set; }
        public int Outcode1 { get; set; }
        public int OutcodeOut { get; set; }
        public double IntersectX { get; set; }
        public double IntersectY { get; set; }
        public string Message { get; set; }
    }

    public class LiangBarskyStep
    {
        public enum StepAction
        {
            Start,
            ParamTest,
            Reject,
            UpdateU,
            FinalAccepted,
            FinalRejected
        }

        public StepAction Action { get; set; }
        public double X0 { get; set; }
        public double Y0 { get; set; }
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double P { get; set; }
        public double Q { get; set; }
        public double U0 { get; set; }
        public double U1 { get; set; }
        public string Message { get; set; }
    }
    public class CyrusBeckStep
    {
        public enum StepAction
        {
            Start,
            ComputeNormals,
            ParamTest,
            UpdateT,
            FinalAccepted,
            FinalRejected
        }

        public StepAction Action { get; set; }
        public double X0 { get; set; }
        public double Y0 { get; set; }
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double T0 { get; set; }
        public double T1 { get; set; }
        public double Numer { get; set; }
        public double Denom { get; set; }
        public string Message { get; set; }
    }

    internal class CRecortarlineas
    {
        private const int INSIDE = 0;
        private const int LEFT = 1;
        private const int RIGHT = 2;
        private const int BOTTOM = 4;
        private const int TOP = 8;

        private static int ComputeOutCode(double x, double y, double xmin, double ymin, double xmax, double ymax)
        {
            int code = INSIDE;
            if (x < xmin) code |= LEFT;
            else if (x > xmax) code |= RIGHT;
            if (y < ymin) code |= BOTTOM;
            else if (y > ymax) code |= TOP;
            return code;
        }

        public static bool CohenSutherlandClip(ref double x0, ref double y0, ref double x1, ref double y1,
                                               double xmin, double ymin, double xmax, double ymax)
        {
            int outcode0 = ComputeOutCode(x0, y0, xmin, ymin, xmax, ymax);
            int outcode1 = ComputeOutCode(x1, y1, xmin, ymin, xmax, ymax);

            bool accept = false;

            while (true)
            {
                if ((outcode0 | outcode1) == 0)
                {
                    accept = true;
                    break;
                }
                else if ((outcode0 & outcode1) != 0)
                {
                    accept = false;
                    break;
                }
                else
                {
                    int outcodeOut = (outcode0 != 0) ? outcode0 : outcode1;
                    double x = 0, y = 0;
                    double dx = x1 - x0;
                    double dy = y1 - y0;

                    if ((outcodeOut & TOP) != 0)
                    {
                        y = ymax;
                        x = Math.Abs(dy) > double.Epsilon ? x0 + dx * (ymax - y0) / dy : x0;
                    }
                    else if ((outcodeOut & BOTTOM) != 0)
                    {
                        y = ymin;
                        x = Math.Abs(dy) > double.Epsilon ? x0 + dx * (ymin - y0) / dy : x0;
                    }
                    else if ((outcodeOut & RIGHT) != 0)
                    {
                        x = xmax;
                        y = Math.Abs(dx) > double.Epsilon ? y0 + dy * (xmax - x0) / dx : y0;
                    }
                    else if ((outcodeOut & LEFT) != 0)
                    {
                        x = xmin;
                        y = Math.Abs(dx) > double.Epsilon ? y0 + dy * (xmin - x0) / dx : y0;
                    }

                    if (outcodeOut == outcode0)
                    {
                        x0 = x; y0 = y;
                        outcode0 = ComputeOutCode(x0, y0, xmin, ymin, xmax, ymax);
                    }
                    else
                    {
                        x1 = x; y1 = y;
                        outcode1 = ComputeOutCode(x1, y1, xmin, ymin, xmax, ymax);
                    }
                }
            }

            return accept;
        }

        private static string OutcodeToText(int oc)
        {
            return $"{Convert.ToString(oc, 2).PadLeft(4, '0')}";
        }

        public static IEnumerable<CohenSutherlandStep> TraceCohenSutherland(double x0, double y0, double x1, double y1,
                                                                            double xmin, double ymin, double xmax, double ymax)
        {
            yield return new CohenSutherlandStep
            {
                Action = CohenSutherlandStep.StepAction.Start,
                X0 = x0,
                Y0 = y0,
                X1 = x1,
                Y1 = y1
            };

            int outcode0 = ComputeOutCode(x0, y0, xmin, ymin, xmax, ymax);
            int outcode1 = ComputeOutCode(x1, y1, xmin, ymin, xmax, ymax);

            yield return new CohenSutherlandStep
            {
                Action = CohenSutherlandStep.StepAction.CodesComputed,
                X0 = x0,
                Y0 = y0,
                X1 = x1,
                Y1 = y1,
                Outcode0 = outcode0,
                Outcode1 = outcode1
            };

            bool done = false;
            bool accept = false;

            while (!done)
            {
                if ((outcode0 | outcode1) == 0)
                {
                    accept = true; done = true;
                    yield return new CohenSutherlandStep
                    {
                        Action = CohenSutherlandStep.StepAction.TriviallyAccepted,
                        X0 = x0,
                        Y0 = y0,
                        X1 = x1,
                        Y1 = y1
                    };
                    break;
                }
                else if ((outcode0 & outcode1) != 0)
                {
                    accept = false; done = true;
                    yield return new CohenSutherlandStep
                    {
                        Action = CohenSutherlandStep.StepAction.TriviallyRejected,
                        X0 = x0,
                        Y0 = y0,
                        X1 = x1,
                        Y1 = y1
                    };
                    break;
                }
                else
                {
                    int outcodeOut = (outcode0 != 0) ? outcode0 : outcode1;
                    double dx = x1 - x0;
                    double dy = y1 - y0;
                    double x = 0, y = 0;

                    if ((outcodeOut & TOP) != 0)
                    {
                        y = ymax;
                        x = x0 + dx * (ymax - y0) / dy;
                    }
                    else if ((outcodeOut & BOTTOM) != 0)
                    {
                        y = ymin;
                        x = x0 + dx * (ymin - y0) / dy;
                    }
                    else if ((outcodeOut & RIGHT) != 0)
                    {
                        x = xmax;
                        y = y0 + dy * (xmax - x0) / dx;
                    }
                    else if ((outcodeOut & LEFT) != 0)
                    {
                        x = xmin;
                        y = y0 + dy * (xmin - x0) / dx;
                    }

                    yield return new CohenSutherlandStep
                    {
                        Action = CohenSutherlandStep.StepAction.IntersectionComputed,
                        X0 = x0,
                        Y0 = y0,
                        X1 = x1,
                        Y1 = y1,
                        OutcodeOut = outcodeOut,
                        IntersectX = x,
                        IntersectY = y
                    };

                    if (outcodeOut == outcode0)
                    {
                        x0 = x; y0 = y;
                        outcode0 = ComputeOutCode(x0, y0, xmin, ymin, xmax, ymax);
                    }
                    else
                    {
                        x1 = x; y1 = y;
                        outcode1 = ComputeOutCode(x1, y1, xmin, ymin, xmax, ymax);
                    }

                    yield return new CohenSutherlandStep
                    {
                        Action = CohenSutherlandStep.StepAction.CodesComputed,
                        X0 = x0,
                        Y0 = y0,
                        X1 = x1,
                        Y1 = y1,
                        Outcode0 = outcode0,
                        Outcode1 = outcode1
                    };
                }
            }

            if (accept)
            {
                yield return new CohenSutherlandStep
                {
                    Action = CohenSutherlandStep.StepAction.FinalAccepted,
                    X0 = x0,
                    Y0 = y0,
                    X1 = x1,
                    Y1 = y1
                };
            }
            else
            {
                yield return new CohenSutherlandStep
                {
                    Action = CohenSutherlandStep.StepAction.FinalRejected,
                    X0 = x0,
                    Y0 = y0,
                    X1 = x1,
                    Y1 = y1
                };
            }
        }

        public static bool LiangBarskyClip(ref double x0, ref double y0, ref double x1, ref double y1,
                                           double xmin, double ymin, double xmax, double ymax)
        {
            double dx = x1 - x0;
            double dy = y1 - y0;

            double u0 = 0.0;
            double u1 = 1.0;

            bool ClipTest(double p, double q, ref double u0r, ref double u1r)
            {
                if (Math.Abs(p) < 1e-12)
                {
                    if (q < 0) return false;
                    return true;
                }

                double r = q / p;

                if (p < 0)
                {
                    if (r > u1r) return false;
                    if (r > u0r) u0r = r;
                }
                else if (p > 0)
                {
                    if (r < u0r) return false;
                    if (r < u1r) u1r = r;
                }

                return true;
            }

            if (!ClipTest(-dx, x0 - xmin, ref u0, ref u1)) return false;
            if (!ClipTest(dx, xmax - x0, ref u0, ref u1)) return false;
            if (!ClipTest(-dy, y0 - ymin, ref u0, ref u1)) return false;
            if (!ClipTest(dy, ymax - y0, ref u0, ref u1)) return false;

            if (u1 < u0) return false;

            double nx0 = x0 + u0 * dx;
            double ny0 = y0 + u0 * dy;
            double nx1 = x0 + u1 * dx;
            double ny1 = y0 + u1 * dy;

            x0 = nx0; y0 = ny0;
            x1 = nx1; y1 = ny1;

            return true;
        }

        public static IEnumerable<LiangBarskyStep> TraceLiangBarsky(double x0, double y0, double x1, double y1,
                                                                    double xmin, double ymin, double xmax, double ymax)
        {
            yield return new LiangBarskyStep
            {
                Action = LiangBarskyStep.StepAction.Start,
                X0 = x0,
                Y0 = y0,
                X1 = x1,
                Y1 = y1
            };

            double dx = x1 - x0;
            double dy = y1 - y0;
            double u0 = 0.0;
            double u1 = 1.0;



            List<(double p, double q)> tests = new List<(double p, double q)>
            {
                (-dx, x0 - xmin),
                ( dx, xmax - x0),
                (-dy, y0 - ymin),
                ( dy, ymax - y0)
            };

            foreach (var t in tests)
            {
                double p = t.p;
                double q = t.q;

                double r = p != 0 ? q / p : double.PositiveInfinity;

                yield return new LiangBarskyStep
                {
                    Action = LiangBarskyStep.StepAction.ParamTest,
                    X0 = x0,
                    Y0 = y0,
                    X1 = x1,
                    Y1 = y1,
                    P = p,
                    Q = q,
                    U0 = u0,
                    U1 = u1,
                    Message = $"p={p:F3} q={q:F3} r={(double.IsInfinity(r) ? 999 : r):F3}"
                };

                if (Math.Abs(p) < 1e-12)
                {
                    if (q < 0)
                    {
                        yield return new LiangBarskyStep
                        {
                            Action = LiangBarskyStep.StepAction.Reject,
                            Message = "Rechazo: línea paralela afuera"
                        };
                        yield break;
                    }
                }
                else
                {
                    if (p < 0)
                    {
                        if (r > u1)
                        {
                            yield return new LiangBarskyStep
                            {
                                Action = LiangBarskyStep.StepAction.Reject,
                                Message = "Rechazo: r > u1"
                            };
                            yield break;
                        }
                        if (r > u0)
                        {
                            u0 = r;
                            yield return new LiangBarskyStep
                            {
                                Action = LiangBarskyStep.StepAction.UpdateU,
                                U0 = u0,
                                U1 = u1,
                                Message = $"Actualiza u0={u0:F3}"
                            };
                        }
                    }
                    else
                    {
                        if (r < u0)
                        {
                            yield return new LiangBarskyStep
                            {
                                Action = LiangBarskyStep.StepAction.Reject,
                                Message = "Rechazo: r < u0"
                            };
                            yield break;
                        }
                        if (r < u1)
                        {
                            u1 = r;
                            yield return new LiangBarskyStep
                            {
                                Action = LiangBarskyStep.StepAction.UpdateU,
                                U0 = u0,
                                U1 = u1,
                                Message = $"Actualiza u1={u1:F3}"
                            };
                        }
                    }
                }
            }

            if (u1 < u0)
            {
                yield return new LiangBarskyStep
                {
                    Action = LiangBarskyStep.StepAction.FinalRejected,
                    Message = "Final: u1 < u0"
                };
                yield break;
            }

            double nx0 = x0 + u0 * dx;
            double ny0 = y0 + u0 * dy;
            double nx1 = x0 + u1 * dx;
            double ny1 = y0 + u1 * dy;

            yield return new LiangBarskyStep
            {
                Action = LiangBarskyStep.StepAction.FinalAccepted,
                X0 = nx0,
                Y0 = ny0,
                X1 = nx1,
                Y1 = ny1,
                Message = "Línea recortada"
            };
        }

        private static bool Clip(double p, double q, ref double u1, ref double u2)
        {
            if (p == 0)
            {
                if (q < 0) return false;
                return true;
            }

            double r = q / p;

            if (p < 0)
            {
                if (r > u2) return false;
                if (r > u1) u1 = r;
            }
            else if (p > 0)
            {
                if (r < u1) return false;
                if (r < u2) u2 = r;
            }

            return true;
        }


        public static IEnumerable<CyrusBeckStep> TraceCyrusBeck(double x0, double y0,
                                                            double x1, double y1,
                                                            double xmin, double ymin,
                                                            double xmax, double ymax)
        {
            yield return new CyrusBeckStep
            {
                Action = CyrusBeckStep.StepAction.Start,
                X0 = x0,
                Y0 = y0,
                X1 = x1,
                Y1 = y1,
                Message = "Inicio Cyrus–Beck"
            };

            double dx = x1 - x0;
            double dy = y1 - y0;

            double tE = 0.0;
            double tL = 1.0;

            var edges = new (double nx, double ny, double fx, double fy, string name)[]
            {
        (-1, 0, xmin, 0, "Izquierda"),
        ( 1, 0, xmax, 0, "Derecha"),
        ( 0,-1, 0, ymin, "Abajo"),
        ( 0, 1, 0, ymax, "Arriba")
            };

            yield return new CyrusBeckStep
            {
                Action = CyrusBeckStep.StepAction.ComputeNormals,
                Message = "Normales del rectángulo generadas"
            };

            foreach (var e in edges)
            {
                double numer = e.nx * (e.fx - x0) + e.ny * (e.fy - y0);
                double denom = e.nx * dx + e.ny * dy;

                yield return new CyrusBeckStep
                {
                    Action = CyrusBeckStep.StepAction.ParamTest,
                    X0 = x0,
                    Y0 = y0,
                    X1 = x1,
                    Y1 = y1,
                    Numer = numer,
                    Denom = denom,
                    T0 = tE,
                    T1 = tL,
                    Message = $"Probando borde: {e.name}"
                };

                if (Math.Abs(denom) < 1e-12)
                {
                    if (numer < 0)
                    {
                        yield return new CyrusBeckStep
                        {
                            Action = CyrusBeckStep.StepAction.FinalRejected,
                            Message = "Rechazo: línea paralela fuera"
                        };
                        yield break;
                    }
                }
                else
                {
                    double t = numer / denom;

                    if (denom < 0)
                    {
                        if (t > tL)
                        {
                            yield return new CyrusBeckStep { Action = CyrusBeckStep.StepAction.FinalRejected };
                            yield break;
                        }
                        if (t > tE)
                        {
                            tE = t;
                            yield return new CyrusBeckStep
                            {
                                Action = CyrusBeckStep.StepAction.UpdateT,
                                T0 = tE,
                                T1 = tL,
                                Message = "Actualiza tE"
                            };
                        }
                    }
                    else
                    {
                        if (t < tE)
                        {
                            yield return new CyrusBeckStep { Action = CyrusBeckStep.StepAction.FinalRejected };
                            yield break;
                        }
                        if (t < tL)
                        {
                            tL = t;
                            yield return new CyrusBeckStep
                            {
                                Action = CyrusBeckStep.StepAction.UpdateT,
                                T0 = tE,
                                T1 = tL,
                                Message = "Actualiza tL"
                            };
                        }
                    }
                }
            }

            if (tL < tE)
            {
                yield return new CyrusBeckStep
                {
                    Action = CyrusBeckStep.StepAction.FinalRejected,
                    Message = "tL < tE"
                };
                yield break;
            }

            double nx0 = x0 + tE * dx;
            double ny0 = y0 + tE * dy;
            double nx1 = x0 + tL * dx;
            double ny1 = y0 + tL * dy;

            yield return new CyrusBeckStep
            {
                Action = CyrusBeckStep.StepAction.FinalAccepted,
                X0 = nx0,
                Y0 = ny0,
                X1 = nx1,
                Y1 = ny1,
                Message = "Línea recortada (Cyrus–Beck)"
            };
        }

        public static bool CyrusBeckClip(ref double x0, ref double y0,
                                  ref double x1, ref double y1,
                                  double xmin, double ymin,
                                  double xmax, double ymax)
        {
            double dx = x1 - x0;
            double dy = y1 - y0;

            double tE = 0.0;
            double tL = 1.0;

           
            double localX0 = x0;
            double localY0 = y0;

            bool ClipEdge(double nx, double ny, double px, double py)
            {
                double numer = nx * (px - localX0) + ny * (py - localY0);
                double denom = nx * dx + ny * dy;

                if (denom == 0)
                {
                    return numer <= 0;
                }

                double t = numer / denom;

                if (denom < 0)
                {
                    if (t > tE) tE = t;
                    if (t > tL) return false;
                }
                else
                {
                    if (t < tL) tL = t;
                    if (t < tE) return false;
                }

                return true;
            }

            if (!ClipEdge(-1, 0, xmin, 0)) return false;
            if (!ClipEdge(1, 0, xmax, 0)) return false;
            if (!ClipEdge(0, -1, 0, ymin)) return false;
            if (!ClipEdge(0, 1, 0, ymax)) return false;

            double nx0 = x0 + tE * dx;
            double ny0 = y0 + tE * dy;
            double nx1 = x0 + tL * dx;
            double ny1 = y0 + tL * dy;

            x0 = nx0;
            y0 = ny0;
            x1 = nx1;
            y1 = ny1;

            return true;
        }


    }
}
