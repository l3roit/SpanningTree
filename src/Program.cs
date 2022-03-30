using System.Collections.Generic;

namespace SpanningTree
{
    public class SpanningTree
    {
        private static readonly Graph _graph = new();
        private static void Main(string[] args) 
        {
            ParseInput("E:\\C# Projects\\SpanningTree\\input.txt");
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
                        Index = char.Parse(l[0]) - 65,
                        Name = char.Parse(l[0]),
                        Weight = int.Parse(l[1])
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
    }
    public class Edge
    {
        public int Vertex1 {get; set;}
        public int Vertex2 {get; set;}
        public int Weight {get; set;}
    }

    public class Vertex
    {
        public int Index {get; set;}
        public char Name {get; set;}
        public int Weight { get; set; }
    }

    public class Graph
    {
        public List<Edge> Edges = new();
        public List<Vertex> Vertices = new();
        public string? GraphName { get; set; }
    }
}