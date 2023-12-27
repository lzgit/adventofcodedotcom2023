namespace Puzzles;

public class Day25(string input) : DailyPuzzleBase(input)
{
    public override string GetPuzzleOneSolution()
    {
        var wireGraph = GetWireGraph();
        var originalReachableNodeCount = CountNodes(wireGraph);

        var split = new List<string>();
        while (split.Count < originalReachableNodeCount)
        {
            var maxByExternalConnections = wireGraph.Keys
                .Where(n => !split.Contains(n))
                .MaxBy(n => wireGraph[n].Count(nb => split.Contains(nb)));

            split.Add(maxByExternalConnections);
            var sum = split.Sum(n => wireGraph[n].Count(nb => !split.Contains(nb)));
            if (sum == 3)
                return (split.Count * (originalReachableNodeCount - split.Count)).ToString();
        }

        throw new Exception("!");
    }

    public override string GetPuzzleTwoSolution()
    {
        return "";
    }

    private static List<(string From, string To)> GetWires(Dictionary<string, List<string>> wireGraph)
    {
        return wireGraph
            .OrderBy(kv => kv.Value.Count)
            .SelectMany(kv => kv.Value.Select(v => (From: kv.Key, To: v)))
            .DistinctBy(w => string.Join("-", new List<string> { w.From, w.To }.OrderBy(n => n)))
            .ToList();
    }

    private int CountNodes(Dictionary<string, List<string>> graph)
    {
        var start = graph.Keys.First();

        var q = new Queue<string>();
        q.Enqueue(start);

        var isVisited = new HashSet<string>();
        int count = 0;
        while (q.TryDequeue(out var nodeName))
        {
            if (!isVisited.Contains(nodeName))
            {
                isVisited.Add(nodeName);
                count++;
                graph[nodeName].Where(n => !isVisited.Contains(n)).ToList().ForEach(q.Enqueue);
            }
        }

        return count;
    }

    private Dictionary<string, List<string>> GetWireGraph()
    {
        var graph = new Dictionary<string, List<string>>();
        foreach (var l in Input.Split(Environment.NewLine))
        {
            var graphNodeParts = l.Split(":");
            var nodeName = graphNodeParts.First().Trim();
            var neighbours = graphNodeParts.Last().Trim().Split(" ").Select(n => n.Trim()).ToList();

            if (!graph.ContainsKey(nodeName))
                graph[nodeName] = new List<string>();

            graph[nodeName].AddRange(neighbours);
            foreach (var neighbour in neighbours)
            {
                if (!graph.ContainsKey(neighbour))
                    graph[neighbour] = new List<string>();

                graph[neighbour].Add(nodeName);
            }
        }

        return graph;
    }
}