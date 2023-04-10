using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Elements;
using System.Numerics;
using System.Text.RegularExpressions;


namespace Model
{
    public static class ObjParser
    {
        // Possible cultures
        static readonly System.Globalization.CultureInfo USculture = new System.Globalization.CultureInfo("en-US");
        static readonly System.Globalization.CultureInfo PLculture = new System.Globalization.CultureInfo("pl-PL");

        // For dot numbers
        static readonly Regex dotRegV = new Regex(@"^v -?[0-9]+\.[0-9]+ -?[0-9]+\.[0-9]+ -?[0-9]+\.[0-9]+");
        static readonly Regex dotRegNV = new Regex(@"^vn -?[0-9]+\.[0-9]+ -?[0-9]+\.[0-9]+ -?[0-9]+\.[0-9]+");
        static readonly Regex dotRegF = new Regex("^f [0-9]+//[0-9]+ [0-9]+//[0-9]+ [0-9]+//[0-9]+");

        // For comma numbers
        static readonly Regex commaRegV = new Regex("^v -?[0-9]+,[0-9]+ -?[0-9]+,[0-9]+ -?[0-9]+,[0-9]+");
        static readonly Regex commaRegNV = new Regex("^vn -?[0-9]+,[0-9]+ -?[0-9]+,[0-9]+ -?[0-9]+,[0-9]+");
        static readonly Regex commaRegF = new Regex("^f [0-9]+//[0-9]+ [0-9]+//[0-9]+ [0-9]+//[0-9]+");

        public static Shape PareOjeFile(string path, float size)
        {
            Shape tmp = new Shape();
            List<MPoint> points = new List<MPoint>();
            List<Vector3> norms = new List<Vector3>();
            Dictionary<(int, int), Edge> edges = new Dictionary<(int, int), Edge>();

            // Starting culture
            System.Globalization.CultureInfo culture = USculture;
            Regex regV = dotRegV;
            Regex regNV = dotRegNV;
            Regex regF = dotRegF;

            int counter = 0;
            while (tmp.Edges.Count <= 0 && counter < 2)
            {
                tmp = new Shape();
                points = new List<MPoint>();
                norms = new List<Vector3>();
                edges = new Dictionary<(int, int), Edge>();
                counter++;
                foreach (string line in System.IO.File.ReadLines(@path))
                {
                    //System.Console.WriteLine(line);
                    if (regV.IsMatch(line))
                    {
                        string[] words = line.Split(' ');
                        float X = float.Parse(words[1], culture);
                        float Y = float.Parse(words[2], culture);
                        float Z = float.Parse(words[3], culture);

                        points.Add(new MPoint( X * size, 
                                               Y * size, 
                                               Z * size));
                    }
                    else if (regNV.IsMatch(line))
                    {
                        string[] words = line.Split(' ');
                        float X = float.Parse(words[1], culture);
                        float Y = float.Parse(words[2], culture);
                        float Z = float.Parse(words[3], culture);


                        Vector3 norm_s = Vector3.Normalize(new Vector3(X, Y, Z));
                        norms.Add(norm_s);
                    }
                    else if (regF.IsMatch(line))
                    {
                        if (points.Count == 0 || norms.Count == 0) continue;

                        string[] words = line.Split(' ');

                        string[] pair1 = words[1].Split("//");
                        int idxP1 = int.Parse(pair1[0], culture) - 1;
                        int idxN1 = int.Parse(pair1[1], culture) - 1;
                        points[idxP1].Norm = norms[idxN1];

                        string[] pair2 = words[2].Split("//");
                        int idxP2 = int.Parse(pair2[0], culture) - 1;
                        int idxN2 = int.Parse(pair2[1], culture) - 1;
                        points[idxP2].Norm = norms[idxN2];

                        string[] pair3 = words[3].Split("//");
                        int idxP3 = int.Parse(pair3[0], culture) - 1;
                        int idxN3 = int.Parse(pair3[1], culture) - 1;
                        points[idxP3].Norm = norms[idxN3];

                        bool NoWhare1 = true, NoWhare2 = true, NoWhare3 = true;
                        Edge e1 = new Edge(points[idxP1], points[idxP2]),
                             e2 = new Edge(points[idxP1], points[idxP3]),
                             e3 = new Edge(points[idxP2], points[idxP3]);
                        if (edges.ContainsKey((idxP1, idxP2)))
                        {
                            e1 = edges[(idxP1, idxP2)];
                            NoWhare1 = false;
                        }
                        else if (edges.ContainsKey((idxP2, idxP1)))
                        {
                            e1 = edges[(idxP2, idxP1)];
                            NoWhare1 = false;
                        }

                        if (edges.ContainsKey((idxP1, idxP3)))
                        {
                            e2 = edges[(idxP1, idxP3)];
                            NoWhare2 = false;
                        }
                        else if (edges.ContainsKey((idxP3, idxP1)))
                        {
                            e2 = edges[(idxP3, idxP1)];
                            NoWhare2 = false;
                        }

                        if (edges.ContainsKey((idxP3, idxP2)))
                        {
                            e3 = edges[(idxP3, idxP2)];
                            NoWhare3 = false;
                        }
                        else if (edges.ContainsKey((idxP2, idxP3)))
                        {
                            e3 = edges[(idxP2, idxP3)];
                            NoWhare3 = false;
                        }

                        if (NoWhare1)
                        {
                            edges.Add((idxP1, idxP2), e1);
                        }

                        if (NoWhare2)
                        {
                            edges.Add((idxP1, idxP3), e2);
                        }

                        if (NoWhare3)
                        {
                            edges.Add((idxP3, idxP2), e3);
                        }

                        Triangle tmp_t = new Triangle(e1, e2, e3, points[idxP1], points[idxP2], points[idxP3]);
                        tmp.Add(tmp_t);
                    }
                }

                culture = PLculture;
                regV = commaRegV;
                regNV = commaRegNV;
                regF = commaRegF;
            }

            return tmp;
        }
    }
}
