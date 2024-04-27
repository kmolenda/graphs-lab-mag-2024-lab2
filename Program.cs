
Test2();

// ========================================================
void Test2()
{
    var g = new GraphAdjList<char>();

    g.AddVertices("ABCDEFGHI");
    g.AddEdgesAndVertices(new List<(char, char)> { 
        ('A', 'B'), ('A', 'C'), ('B', 'C'), ('B', 'E'), ('E', 'F'), ('E', 'C'),
         ('C', 'D'), ('H', 'I')});
    // Console.WriteLine( g.ToDot() );
    Console.WriteLine(g.CountConnectedComponents());
}


void Test1()
{
    var g = new GraphAdjList<char>();
    g.AddEdgesAndVertices(new List<(char, char)> { ('A', 'B'), ('A', 'C'), ('B', 'C'), ('B', 'D') });
    // g.AddVertices("ABCDE");
    // g.AddEdge('A', 'B');
    // g.AddEdge('A', 'C');
    // g.AddEdge('B', 'C');
    // g.AddEdge('B', 'D');

    g.Dump();

    foreach (var v in GraphAlgorithms.DFS(g, 'A'))
        Console.WriteLine($"Vertex: {v}");

    foreach (var v in g.BFS(start: 'B'))
        Console.WriteLine($"Vertex: {v}");


    Console.WriteLine(string.Join(", ", g.DFS('A') ));

    var l = GraphAlgorithms.DFS(g, 'A').ToList();

    Console.WriteLine( g.ToDot() );
}
