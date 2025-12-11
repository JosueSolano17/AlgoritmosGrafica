using System;
using System.Collections.Generic;

namespace WinAPPLineas.Clases
{
    internal class CRecortarPoligono
    {
        // Simple double-precision 2D point used throughout
        public struct PointD
        {
            public double X;
            public double Y;
            public PointD(double x, double y) { X = x; Y = y; }
            public override string ToString() => $"({X:F3},{Y:F3})";
        }

        #region Sutherland–Hodgman

        public class SutherlandHodgmanStep
        {
            public enum StepAction
            {
                Start,
                EdgeStart,
                TestPoint,
                IntersectionComputed,
                OutputVertex,
                EdgeFinished,
                FinalResult
            }

            public StepAction Action { get; set; }
            public PointD ClipA { get; set; }
            public PointD ClipB { get; set; }
            public PointD SubjectPoint { get; set; }
            public PointD PrevSubjectPoint { get; set; }
            public PointD Intersection { get; set; }
            public List<PointD> ResultSoFar { get; set; }
            public string Message { get; set; }
        }

        // Trace Sutherland–Hodgman: assumes clip polygon is convex.
        public static IEnumerable<SutherlandHodgmanStep> TraceSutherlandHodgman(
            List<PointD> subjectPolygon,
            List<PointD> clipPolygon)
        {
            if (subjectPolygon == null) subjectPolygon = new List<PointD>();
            if (clipPolygon == null || clipPolygon.Count < 3)
            {
                yield return new SutherlandHodgmanStep
                {
                    Action = SutherlandHodgmanStep.StepAction.FinalResult,
                    ResultSoFar = new List<PointD>(subjectPolygon),
                    Message = "Polígono de recorte inválido (menos de 3 vértices)."
                };
                yield break;
            }

            yield return new SutherlandHodgmanStep
            {
                Action = SutherlandHodgmanStep.StepAction.Start,
                ResultSoFar = new List<PointD>(subjectPolygon),
                Message = $"Inicio Sutherland–Hodgman: sujeto={subjectPolygon.Count} clip={clipPolygon.Count}"
            };

            
            bool clipIsCCW = SignedArea(clipPolygon) > 0;

            List<PointD> inputList = new List<PointD>(subjectPolygon);

            for (int ci = 0; ci < clipPolygon.Count; ci++)
            {
                PointD A = clipPolygon[ci];
                PointD B = clipPolygon[(ci + 1) % clipPolygon.Count];

                yield return new SutherlandHodgmanStep
                {
                    Action = SutherlandHodgmanStep.StepAction.EdgeStart,
                    ClipA = A,
                    ClipB = B,
                    ResultSoFar = new List<PointD>(inputList),
                    Message = $"Procesando arista de recorte {ci}: {A} -> {B}"
                };

                List<PointD> outputList = new List<PointD>();

                if (inputList.Count == 0)
                {
                    yield return new SutherlandHodgmanStep
                    {
                        Action = SutherlandHodgmanStep.StepAction.EdgeFinished,
                        ClipA = A,
                        ClipB = B,
                        ResultSoFar = new List<PointD>(outputList),
                        Message = "Lista de entrada vacía — no queda nada."
                    };
                    inputList = outputList;
                    continue;
                }

                PointD S = inputList[inputList.Count - 1];

                for (int i = 0; i < inputList.Count; i++)
                {
                    PointD E = inputList[i];

                    bool Ein = IsInside(E, A, B, clipIsCCW);
                    bool Sin = IsInside(S, A, B, clipIsCCW);

                    yield return new SutherlandHodgmanStep
                    {
                        Action = SutherlandHodgmanStep.StepAction.TestPoint,
                        ClipA = A,
                        ClipB = B,
                        PrevSubjectPoint = S,
                        SubjectPoint = E,
                        ResultSoFar = new List<PointD>(outputList),
                        Message = $"Evaluando vértice {i}: {E} (inside={Ein}) frente a {S} (inside={Sin})"
                    };

                    if (Sin && Ein)
                    {
                        
                        outputList.Add(E);
                        yield return new SutherlandHodgmanStep
                        {
                            Action = SutherlandHodgmanStep.StepAction.OutputVertex,
                            SubjectPoint = E,
                            ResultSoFar = new List<PointD>(outputList),
                            Message = $"Ambos dentro -> salida {E}"
                        };
                    }
                    else if (Sin && !Ein)
                    {
                        
                        PointD ip;
                        if (SegmentIntersection(S, E, A, B, out ip))
                        {
                            outputList.Add(ip);
                            yield return new SutherlandHodgmanStep
                            {
                                Action = SutherlandHodgmanStep.StepAction.IntersectionComputed,
                                PrevSubjectPoint = S,
                                SubjectPoint = E,
                                Intersection = ip,
                                ResultSoFar = new List<PointD>(outputList),
                                Message = $"Cruce S->E: intersección calculada {ip}"
                            };
                        }
                        else
                        {
                            yield return new SutherlandHodgmanStep
                            {
                                Action = SutherlandHodgmanStep.StepAction.IntersectionComputed,
                                PrevSubjectPoint = S,
                                SubjectPoint = E,
                                ResultSoFar = new List<PointD>(outputList),
                                Message = $"Cruce S->E: no hay intersección numérica"
                            };
                        }
                    }
                    else if (!Sin && Ein)
                    {
                        
                        PointD ip;
                        if (SegmentIntersection(S, E, A, B, out ip))
                        {
                            outputList.Add(ip);
                            yield return new SutherlandHodgmanStep
                            {
                                Action = SutherlandHodgmanStep.StepAction.IntersectionComputed,
                                PrevSubjectPoint = S,
                                SubjectPoint = E,
                                Intersection = ip,
                                ResultSoFar = new List<PointD>(outputList),
                                Message = $"Cruce fuera->dentro: intersección {ip}"
                            };
                        }
                        outputList.Add(E);
                        yield return new SutherlandHodgmanStep
                        {
                            Action = SutherlandHodgmanStep.StepAction.OutputVertex,
                            SubjectPoint = E,
                            ResultSoFar = new List<PointD>(outputList),
                            Message = $"Añadido vértice interior {E}"
                        };
                    }
                    else
                    {
                    
                        yield return new SutherlandHodgmanStep
                        {
                            Action = SutherlandHodgmanStep.StepAction.TestPoint,
                            PrevSubjectPoint = S,
                            SubjectPoint = E,
                            ResultSoFar = new List<PointD>(outputList),
                            Message = $"Ambos fuera -> nada"
                        };
                    }

                    S = E;
                }

                yield return new SutherlandHodgmanStep
                {
                    Action = SutherlandHodgmanStep.StepAction.EdgeFinished,
                    ClipA = A,
                    ClipB = B,
                    ResultSoFar = new List<PointD>(outputList),
                    Message = $"Arista procesada -> vertices acumulados: {outputList.Count}"
                };

                inputList = outputList;
            }

            yield return new SutherlandHodgmanStep
            {
                Action = SutherlandHodgmanStep.StepAction.FinalResult,
                ResultSoFar = new List<PointD>(inputList),
                Message = $"Resultado final: {inputList.Count} vértices"
            };
        }

        public static List<PointD> SutherlandHodgmanClip(List<PointD> subjectPolygon, List<PointD> clipPolygon)
        {
            var result = new List<PointD>();
            foreach (var step in TraceSutherlandHodgman(subjectPolygon, clipPolygon))
            {
                if (step.Action == SutherlandHodgmanStep.StepAction.FinalResult && step.ResultSoFar != null)
                {
                    result = step.ResultSoFar;
                    break;
                }
            }
            return result;
        }

        #endregion

        #region Weiler–Atherton (simplified tracing)

        public class WeilerAthertonStep
        {
            public enum StepAction
            {
                Start,
                FindIntersections,
                InsertIntersections,
                MarkEntryExit,
                Traverse,
                FinalResult
            }

            public StepAction Action { get; set; }
            public string Message { get; set; }
            public List<PointD> ResultSoFar { get; set; }
        }

       
        public static IEnumerable<WeilerAthertonStep> TraceWeilerAtherton(
            List<PointD> subjectPolygon,
            List<PointD> clipPolygon)
        {
            if (subjectPolygon == null) subjectPolygon = new List<PointD>();
            if (clipPolygon == null || clipPolygon.Count < 3)
            {
                yield return new WeilerAthertonStep
                {
                    Action = WeilerAthertonStep.StepAction.FinalResult,
                    ResultSoFar = new List<PointD>(subjectPolygon),
                    Message = "Polígono de recorte inválido."
                };
                yield break;
            }

            yield return new WeilerAthertonStep
            {
                Action = WeilerAthertonStep.StepAction.Start,
                Message = $"Inicio Weiler–Atherton: sujeto={subjectPolygon.Count} clip={clipPolygon.Count}"
            };

          
            var intersections = new List<PointD>();
            for (int i = 0; i < subjectPolygon.Count; i++)
            {
                var a1 = subjectPolygon[i];
                var a2 = subjectPolygon[(i + 1) % subjectPolygon.Count];

                for (int j = 0; j < clipPolygon.Count; j++)
                {
                    var b1 = clipPolygon[j];
                    var b2 = clipPolygon[(j + 1) % clipPolygon.Count];

                    PointD ip;
                    if (SegmentIntersection(a1, a2, b1, b2, out ip))
                    {
                        intersections.Add(ip);
                        yield return new WeilerAthertonStep
                        {
                            Action = WeilerAthertonStep.StepAction.FindIntersections,
                            Message = $"Intersección encontrada: {ip}"
                        };
                    }
                }
            }

            if (intersections.Count == 0)
            {
               
                bool inside = PointInPolygon(subjectPolygon.Count > 0 ? subjectPolygon[0] : new PointD(0, 0), clipPolygon);
                if (inside)
                {
                    yield return new WeilerAthertonStep
                    {
                        Action = WeilerAthertonStep.StepAction.FinalResult,
                        ResultSoFar = new List<PointD>(subjectPolygon),
                        Message = "No hay intersecciones y sujeto totalmente dentro -> resultado es sujeto"
                    };
                    yield break;
                }
                else
                {
                    yield return new WeilerAthertonStep
                    {
                        Action = WeilerAthertonStep.StepAction.FinalResult,
                        ResultSoFar = new List<PointD>(),
                        Message = "No hay intersecciones y sujeto fuera -> resultado vacío"
                    };
                    yield break;
                }
            }

             
            yield return new WeilerAthertonStep
            {
                Action = WeilerAthertonStep.StepAction.InsertIntersections,
                Message = "Inserción de intersecciones (trazado). Como fallback se aplicará Sutherland–Hodgman para obtener resultado práctico."
            };

            var fallback = SutherlandHodgmanClip(subjectPolygon, clipPolygon);

            yield return new WeilerAthertonStep
            {
                Action = WeilerAthertonStep.StepAction.FinalResult,
                ResultSoFar = new List<PointD>(fallback),
                Message = $"Resultado aproximado usando Sutherland–Hodgman: {fallback.Count} vértices"
            };
        }

        #endregion

        #region Greiner–Hormann (simplified tracing)

        public class GreinerHormannStep
        {
            public enum StepAction
            {
                Start,
                FindIntersections,
                BuildGraph,
                Traverse,
                FinalResult
            }

            public StepAction Action { get; set; }
            public string Message { get; set; }
            public List<PointD> ResultSoFar { get; set; }
        }

        // 
        public static IEnumerable<GreinerHormannStep> TraceGreinerHormann(
            List<PointD> subjectPolygon,
            List<PointD> clipPolygon)
        {
            yield return new GreinerHormannStep
            {
                Action = GreinerHormannStep.StepAction.Start,
                Message = $"Inicio Greiner–Hormann: sujeto={subjectPolygon?.Count ?? 0} clip={clipPolygon?.Count ?? 0}"
            };

            if (subjectPolygon == null) subjectPolygon = new List<PointD>();
            if (clipPolygon == null || clipPolygon.Count < 3)
            {
                yield return new GreinerHormannStep
                {
                    Action = GreinerHormannStep.StepAction.FinalResult,
                    ResultSoFar = new List<PointD>(subjectPolygon),
                    Message = "Polígono de recorte inválido."
                };
                yield break;
            }

           
            int found = 0;
            for (int i = 0; i < subjectPolygon.Count; i++)
            {
                var a1 = subjectPolygon[i];
                var a2 = subjectPolygon[(i + 1) % subjectPolygon.Count];
                for (int j = 0; j < clipPolygon.Count; j++)
                {
                    var b1 = clipPolygon[j];
                    var b2 = clipPolygon[(j + 1) % clipPolygon.Count];

                    PointD ip;
                    if (SegmentIntersection(a1, a2, b1, b2, out ip))
                    {
                        found++;
                        yield return new GreinerHormannStep
                        {
                            Action = GreinerHormannStep.StepAction.FindIntersections,
                            Message = $"Intersección detectada: {ip}"
                        };
                    }
                }
            }

            if (found == 0)
            {
                bool inside = PointInPolygon(subjectPolygon.Count > 0 ? subjectPolygon[0] : new PointD(0, 0), clipPolygon);
                if (inside)
                {
                    yield return new GreinerHormannStep
                    {
                        Action = GreinerHormannStep.StepAction.FinalResult,
                        ResultSoFar = new List<PointD>(subjectPolygon),
                        Message = "Sin intersecciones y sujeto dentro -> resultado sujeto"
                    };
                }
                else
                {
                    yield return new GreinerHormannStep
                    {
                        Action = GreinerHormannStep.StepAction.FinalResult,
                        ResultSoFar = new List<PointD>(),
                        Message = "Sin intersecciones y sujeto fuera -> resultado vacío"
                    };
                }
                yield break;
            }

            yield return new GreinerHormannStep
            {
                Action = GreinerHormannStep.StepAction.BuildGraph,
                Message = "Construcción de grafo de intersecciones (simplificado)."
            };

            
            var fallback = SutherlandHodgmanClip(subjectPolygon, clipPolygon);

            yield return new GreinerHormannStep
            {
                Action = GreinerHormannStep.StepAction.FinalResult,
                ResultSoFar = new List<PointD>(fallback),
                Message = $"Resultado aproximado (fallback Sutherland): {fallback.Count} vértices"
            };
        }

        #endregion

        #region Helpers

        
        private static double SignedArea(List<PointD> poly)
        {
            double a = 0.0;
            int n = poly.Count;
            for (int i = 0; i < n; i++)
            {
                var p = poly[i];
                var q = poly[(i + 1) % n];
                a += (p.X * q.Y) - (q.X * p.Y);
            }
            return a * 0.5;
        }

        
        private static bool PointInPolygon(PointD p, List<PointD> poly)
        {
            bool inside = false;
            int n = poly.Count;
            for (int i = 0, j = n - 1; i < n; j = i++)
            {
                var pi = poly[i];
                var pj = poly[j];
                if (((pi.Y > p.Y) != (pj.Y > p.Y)) &&
                    (p.X < (pj.X - pi.X) * (p.Y - pi.Y) / (pj.Y - pi.Y + 0.0) + pi.X))
                    inside = !inside;
            }
            return inside;
        }

       
        private static bool IsInside(PointD p, PointD a, PointD b, bool clipIsCCW)
        {
            double cross = (b.X - a.X) * (p.Y - a.Y) - (b.Y - a.Y) * (p.X - a.X);
            return clipIsCCW ? (cross >= -1e-12) : (cross <= 1e-12);
        }

        
        private static bool SegmentIntersection(PointD p1, PointD p2, PointD p3, PointD p4, out PointD intersection)
        {
            intersection = new PointD(0, 0);

            double x1 = p1.X, y1 = p1.Y;
            double x2 = p2.X, y2 = p2.Y;
            double x3 = p3.X, y3 = p3.Y;
            double x4 = p4.X, y4 = p4.Y;

            double denom = (y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1);

            if (Math.Abs(denom) < 1e-12)
            {
               
                if (!IsCollinear(p1, p2, p3)) return false;

               
                if (OnSegment(p1, p2, p3)) { intersection = p3; return true; }
                if (OnSegment(p1, p2, p4)) { intersection = p4; return true; }
                if (OnSegment(p3, p4, p1)) { intersection = p1; return true; }
                if (OnSegment(p3, p4, p2)) { intersection = p2; return true; }
                return false;
            }

            double ua = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / denom;
            double ub = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / denom;

            if (ua >= -1e-12 && ua <= 1 + 1e-12 && ub >= -1e-12 && ub <= 1 + 1e-12)
            {
                intersection = new PointD(x1 + ua * (x2 - x1), y1 + ua * (y2 - y1));
                return true;
            }

            return false;
        }

        private static bool IsCollinear(PointD a, PointD b, PointD c)
        {
            return Math.Abs((b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X)) < 1e-12;
        }

        private static bool OnSegment(PointD a, PointD b, PointD p)
        {
            if (!IsCollinear(a, b, p)) return false;
            double minx = Math.Min(a.X, b.X) - 1e-12, maxx = Math.Max(a.X, b.X) + 1e-12;
            double miny = Math.Min(a.Y, b.Y) - 1e-12, maxy = Math.Max(a.Y, b.Y) + 1e-12;
            return p.X >= minx && p.X <= maxx && p.Y >= miny && p.Y <= maxy;
        }

        #endregion
    }
}