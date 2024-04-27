
using System.Text;

public static class GraphAlgorithms
{
    // Przeglądanie grafu w głąb g, począwszy od wierzchołka `start`
    public static IEnumerable<T> DFS<T>(this IGraphNonWeighted<T> g, T start) where T : IEquatable<T>
    {
        var visited = new HashSet<T>();
        var stack = new Stack<T>();
        stack.Push(start);
        while (stack.Count > 0)
        {
            var current = stack.Pop();
            if (visited.Contains(current))
                continue;
            visited.Add(current);
            yield return current;
            foreach (var neighbour in g.Neighbours(current))
                stack.Push(neighbour);
        }
    }

    // Przeglądanie grafu wszerz g, począwszy od wierzchołka `start`
    public static IEnumerable<T> BFS<T>(this IGraphNonWeighted<T> g, T start) where T : IEquatable<T>
    {
        var visited = new HashSet<T>();
        var queue = new Queue<T>();
        queue.Enqueue(start);
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (visited.Contains(current))
                continue;
            visited.Add(current);
            yield return current;
            foreach (var neighbour in g.Neighbours(current))
                queue.Enqueue(neighbour);
        }
    }

    // Konwerter grafu na notację DOT
    public static string ToDot<T>(this IGraphNonWeighted<T> g) where T : IEquatable<T>
    {
        var sb = new StringBuilder();
        sb.AppendLine("graph {");
        foreach (var vertex in g.Vertices)
            sb.AppendLine($"\t{vertex};");
        foreach (var (v1, v2) in g.Edges)
            sb.AppendLine($"\t{v1} -- {v2};");
        sb.AppendLine("}");
        return sb.ToString();
    } 

    // liczba spójnych składowych grafu g, metoda brute-force
    public static int CountConnectedComponents<T>(this IGraphNonWeighted<T> g) where T : IEquatable<T>
    {
        var visited = new HashSet<T>();
        var count = 0;
        foreach (var vertex in g.Vertices)
        {
            if (visited.Contains(vertex))
                continue;
            count++;
            foreach (var v in g.DFS(vertex))
                visited.Add(v);
        }
        return count;
    }

    // lista składowych spójnych grafu g
    public static IEnumerable<IEnumerable<T>> GetConnectedComponents<T>(this IGraphNonWeighted<T> g) where T : IEquatable<T>
    {
        var visited = new HashSet<T>();
        foreach (var vertex in g.Vertices)
        {
            if (visited.Contains(vertex))
                continue;
            var component = new List<T>();
            foreach (var v in g.DFS(vertex))
            {
                component.Add(v);
                visited.Add(v);
            }
            yield return component;
        }
    }

    // articulation points definicja:
    // https://en.wikipedia.org/wiki/Biconnected_component
    public static IEnumerable<T> GetArticulationPoints<T>(this IGraphNonWeighted<T> g) where T : IEquatable<T>
    {
        var visited = new HashSet<T>();
        var parent = new Dictionary<T, T>();
        var low = new Dictionary<T, int>();
        var disc = new Dictionary<T, int>();
        var ap = new HashSet<T>();
        var time = 0;

        void DFS(T u)
        {
            visited.Add(u);
            disc[u] = low[u] = ++time;
            var children = 0;
            foreach (var v in g.Neighbours(u))
            {
                if (!visited.Contains(v))
                {
                    children++;
                    parent[v] = u;
                    DFS(v);
                    low[u] = Math.Min(low[u], low[v]);
                    if (parent[u] == null && children > 1)
                        ap.Add(u);
                    if (parent[u] != null && low[v] >= disc[u])
                        ap.Add(u);
                }
                else if (!v.Equals(parent[u]))
                    low[u] = Math.Min(low[u], disc[v]);
            }
        }

        foreach (var vertex in g.Vertices)
            if (!visited.Contains(vertex))
                DFS(vertex);

        return ap;
    }
} 