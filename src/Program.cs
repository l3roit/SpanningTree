using System;
using System.Collections.Generic;

namespace SpanningTree
{
    public class SpanningTree
    {
        private static readonly Graph _graph = new();
        private static readonly int _depth = 20;
        private static void Main(string[] args) 
        {
            ParseInput("E:\\C# Projects\\SpanningTree\\input.txt");

            Random rnd = new Random();
            var startNode = _graph.Vertices[rnd.Next(0, _graph.Vertices.Count - 1)].ID;

            CalcTree(); //Start Node doesn't matter for success --> startNode is first element in Vertex List

            var res = GetMinSpanningTree();

            OutputSpanningTree(res);
        }

        private static void CalcTree()
        {
            for (var i = 0; i < _depth; i++)
            {
                foreach (var vtx in _graph.Vertices)
                {
                    var edges = _graph.Edges.Where(x => x.Vertex1 == vtx.ID || x.Vertex2 == vtx.ID);

                    var aVtx = new List<Vertex>();
                    foreach (var edge in edges)
                        aVtx.AddRange(_graph.Vertices.Where(x => (x.ID == edge.Vertex1 || x.ID == edge.Vertex2) && x.ID != vtx.ID));

                    foreach (var v in aVtx)
                    {
                        var edge = _graph.Edges.Where(x => (x.Vertex1 == v.ID && x.Vertex2 == vtx.ID) || (x.Vertex2 == v.ID && x.Vertex1 == vtx.ID)).First();

                        if(v.RootWeight > vtx.RootWeight || ((v.RootWeight == vtx.RootWeight) && v.Cost > (vtx.Cost + edge.Weight))){ 
                            v.ToRootID = vtx.ID;
                            v.RootID = vtx.RootID;
                            v.RootWeight = vtx.RootWeight;
                            v.Cost = vtx.Cost + edge.Weight;
                        }
                    }          
                }
            }
        }
        private static List<Edge> GetMinSpanningTree()
        {
            var edges = new List<Edge>();

            foreach (var vtx in _graph.Vertices){
                if (vtx.ID != vtx.RootID)
                    edges.Add(_graph.Edges.Where(x => (x.Vertex1 == vtx.ToRootID && x.Vertex2 == vtx.ID) || (x.Vertex2 == vtx.ToRootID && x.Vertex1 == vtx.ID)).First());
            }

            return edges;
        }
        private static void ParseInput(string path)
        {
            foreach (var line in File.ReadAllLines(path))
            {
                if (string.IsNullOrEmpty(line) || line.StartsWith(@"\\"))
                    continue;

                if (line.Contains("="))
                {
                    var l = line.Replace(";", "").Replace(" ", "").Split("=");

                    _graph.Vertices.Add(new Vertex()
                    {
                        ID = char.Parse(l[0]) - 65,
                        Weight = int.Parse(l[1]),
                        RootWeight = int.Parse(l[1]),
                        ToRootID = char.Parse(l[0]) - 65,
                        RootID = char.Parse(l[0]) - 65,
                        Cost = 0
                    });
                }
                else if (line.Contains(":"))
                {
                    var l = line.Replace(";", "").Replace(" ", "").Split('-', ':');

                    if(int.Parse(l[2]) == 0)
                        continue;

                    _graph.Edges.Add(new Edge()
                    {
                        Vertex1 = char.Parse(l[0]) - 65,
                        Vertex2 = char.Parse(l[1]) - 65,
                        Weight = int.Parse(l[2])
                    });
                }
                else if (line.Contains("Graph"))
                    _graph.GraphName = line.Replace("Graph ", "").Replace(" {", "");
            }
        }

        private static void OutputSpanningTree( List<Edge> res)
        {
            Console.WriteLine("Graph " + _graph.GraphName + " {");

            _graph.Vertices.Sort((p, q) => p.Weight.CompareTo(q.Weight));

            Console.WriteLine($"Root: {(char)(_graph.Vertices[0].ID + 65)};");

            if(res == null)
                return;

            foreach (var edge in res)
                Console.WriteLine($"{(char)(edge.Vertex1 + 65)} - {(char)(edge.Vertex2 + 65)};");
            
            Console.WriteLine("}");
        }
    }
    public class Edge
    {
        public int Vertex1 {get; set;}
        public int Vertex2 {get; set;}
        public int Weight {get; set;}
    }

    public class Vertex
    {
        public int ID {get; set;}
        public int Weight { get; set; }
        public int Cost {get; set;}
        public int RootID {get; set; }
        public int RootWeight {get; set; }
        public int ToRootID {get; set; }
    }

    public class Graph
    {
        public List<Edge> Edges = new();
        public List<Vertex> Vertices = new();
        public string? GraphName { get; set; }
    }
}