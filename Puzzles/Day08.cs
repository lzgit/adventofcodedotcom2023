namespace Puzzles;

public class Day08(string input) : DailyPuzzleBase(input)
{
    public override string GetPuzzleOneSolution()
    {
        var lines = Input.Split(Environment.NewLine).Where(l => !string.IsNullOrEmpty(l) && !string.IsNullOrWhiteSpace(l)).ToList();
        var navigation = lines.First().Trim();
        var network = GetNodes(lines.Skip(1).ToList());

        var startNode = network["AAA"];
        return GetStepCount(network, navigation, startNode, node => string.Equals(node.From, "ZZZ", StringComparison.InvariantCultureIgnoreCase)).ToString();
    }

    public override string GetPuzzleTwoSolution()
    {
        var lines = Input.Split(Environment.NewLine).Where(l => !string.IsNullOrEmpty(l) && !string.IsNullOrWhiteSpace(l)).ToList();
        var navigation = lines.First().Trim();
        var network = GetNodes(lines.Skip(1).ToList());

        var startingNodes = network.Where(kv => kv.Key.Last() == 'A').Select(kv => kv.Value).ToList();

        var minimumSteps = startingNodes
            .Select(n => GetStepCount(network, navigation, n, node => node.From.Last() == 'Z'))
            .ToList();

        return LeastCommonMultiplier(minimumSteps).ToString();
    }

    public static long LeastCommonMultiplier(List<int> elements)
    {
        long lcm = 1;
        int divisor = 2;

        while (true)
        {
            int counter = 0;
            bool divisible = false;
            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i] == 0)
                    return 0;
                
                if (elements[i] == 1)
                    counter++;

                if (elements[i] % divisor == 0)
                {
                    divisible = true;
                    elements[i] /= divisor;
                }
            }

            if (divisible)
                lcm *= divisor;
            else
                divisor++;

            if (counter == elements.Count)
                return lcm;
        }
    }
    
    private static int GetStepCount(Dictionary<string, Node> network, string navigation, Node startNode, Func<Node, bool> isAtTarget)
    {
        var current = startNode;
        int step = 0;
        do
        {
            var nextNodeFrom = navigation[step % navigation.Length] == 'L' ? current.ToLeft : current.ToRight;
            current = network[nextNodeFrom];
            step++;
        } while (!isAtTarget(current));

        return step;
    }

    private Dictionary<string, Node> GetNodes(List<string> lines)
    {
        var nodes = new Dictionary<string, Node>();
        foreach (var l in lines)
        {
            var fromTo = l.Split('=');
            var leftRight = fromTo.Last().Split(',');
            var node = new Node(
                fromTo.First().Trim(),
                leftRight.First().Trim().Trim('(').Trim(),
                leftRight.Last().Trim().Trim(')').Trim()
                );

            nodes[node.From] = node;
        }

        return nodes;
    }

    public class Node(string from, string toLeft, string toRight)
    {
        public string From { get; set; } = from;
        public string ToLeft { get; set; } = toLeft;
        public string ToRight { get; set; } = toRight;

        public override string ToString() => $"{From} = ({ToLeft}, {ToRight})";
    }
}