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
            ParseInput("E:\\C# Projects\\SpanningTree\\input.txt"); // initialise all vertexes with themselfs as root nodes
            //at start each vertex thinks it is the root so ToRootId = Id, RootWeight = Weight, RootId = Id, Cost = 0 (already at root)

            var rnd = new Random();
            _graph.Vertices = _graph.Vertices.OrderBy(x => rnd.Next()).ToList(); //randomly sort list --> algorithm starts with first list element

            CalcTree(); //Start Node doesn't matter for success
        
            var res = GetMinSpanningTree();

            OutputSpanningTree(res);
        }
        private static void CalcTree()
        {
            for (var i = 0; i < _depth; i++) //run process multiple times
            {
                foreach (var vtx in _graph.Vertices) //run foreach vertex
                {
                    var edges = _graph.Edges.Where(x => x.Vertex1 == vtx.ID || x.Vertex2 == vtx.ID); //get adjacent edges from vertex

                    var aVtx = new List<Vertex>();
                    foreach (var edge in edges)
                        aVtx.AddRange(_graph.Vertices.Where(x => (x.ID == edge.Vertex1 || x.ID == edge.Vertex2) && x.ID != vtx.ID)); //get adjacent vertecies from current vertex

                    foreach (var v in aVtx) // run through adjacent vertecies
                    {
                        var edge = _graph.Edges.Where(x => (x.Vertex1 == v.ID && x.Vertex2 == vtx.ID) || (x.Vertex2 == v.ID && x.Vertex1 == vtx.ID)).First(); // get edge from vertext and adjacent vertext

                        if(v.RootWeight > vtx.RootWeight || // compare root weights --> weight is lower = new way to root
                        ((v.RootWeight == vtx.RootWeight) && v.Cost > (vtx.Cost + edge.Weight))){  // rootweights are same and costs to root are faster 
                            v.ToRootID = vtx.ID; // vertex is new waypoint to root for adjacent vertex
                            v.RootID = vtx.RootID; // adjacent vertex root is same then vertex
                            v.RootWeight = vtx.RootWeight;  // weight of root vertex are the same
                            v.Cost = vtx.Cost + edge.Weight; // cost is vertex + weight of edge
                        }
                    }
                }
            }
        }
        private static List<Edge> GetMinSpanningTree()
        {
            var edges = new List<Edge>();

            foreach (var vtx in _graph.Vertices){
                if (vtx.ID != vtx.RootID) // check if its the root vertex
                    edges.Add(_graph.Edges.Where(x => (x.Vertex1 == vtx.ToRootID && x.Vertex2 == vtx.ID) || (x.Vertex2 == vtx.ToRootID && x.Vertex1 == vtx.ID)).First());
            }

            return edges;
        }

        //parses graph config
        private static void ParseInput(string path)
        {
            foreach (var line in File.ReadAllLines(path))
            {
                if (string.IsNullOrEmpty(line) || line.StartsWith(@"//")) //skip empty line and command line
                    continue;

                if (line.Contains("=")) //parse vertex config
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
                else if (line.Contains(":")) //parse edge config
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
                else if (line.Contains("Graph")) // parse graph name
                    _graph.GraphName = line.Replace("Graph ", "").Replace(" {", "");
            }
        }

        //outputs graph config in console
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
