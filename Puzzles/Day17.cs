namespace Puzzles;

public class Day17(string input) : DailyPuzzleBase(input)
{
    public override string GetPuzzleOneSolution() => GetMinHeatLoss(GetHeatLossMap(), 0, 3).ToString();
    public override string GetPuzzleTwoSolution() => GetMinHeatLoss(GetHeatLossMap(), 4, 10).ToString();

    protected int GetMinHeatLoss(int[,] map, int minSteps, int maxSteps)
    {
        var priorityQueue = new PriorityQueue<Step, int>();
        var visitedCache = new HashSet<string>();

        priorityQueue.Enqueue(new Step(new Location(0, 0), Direction.Right, 1), 0);
        priorityQueue.Enqueue(new Step(new Location(0, 0), Direction.BottomDown, 1), 0);
        
        while (priorityQueue.TryDequeue(out var currentStep, out var heatLoss))
        {
            var width = map.GetLength(1);
            var height = map.GetLength(0);

            if (currentStep.Location.Col == width - 1 && currentStep.Location.Row == height - 1 && currentStep.StraightStepCount + 1 >= minSteps)
                return heatLoss;

            var newDirections = currentStep.StraightStepCount < minSteps - 1 
                ? [currentStep.Direction] 
                : Enum.GetValues<Direction>().Where(d => d != GetOpposite(currentStep.Direction)).ToList();

            foreach (var newDirection in newDirections)
            {
                var newStraightStepCount = newDirection == currentStep.Direction ? currentStep.StraightStepCount + 1 : 0;
                if (newStraightStepCount == maxSteps)
                    continue;

                var newLocation = Add(currentStep.Location, GetLocationDelta(newDirection));
                if (newLocation.Col < 0 || newLocation.Col == width || newLocation.Row < 0 || newLocation.Row == height)
                    continue;

                var key = $"{currentStep.Location.Col}-{currentStep.Location.Row}-{newLocation.Row}-{newLocation.Col}-{newStraightStepCount}";

                if (visitedCache.Add(key))
                    priorityQueue.Enqueue(new Step(newLocation, newDirection, newStraightStepCount), heatLoss + map[newLocation.Row, newLocation.Col]);
            }
        }

        return 0;
    }

    private int[,] GetHeatLossMap()
    {
        var lines = Input.Split(Environment.NewLine);
        var map = new int[lines.Length, lines[0].Length];

        for (var row = 0; row < lines.Length; row++)
            for (var col = 0; col < lines[0].Length; col++)
                map[row, col] = int.Parse(lines[row][col].ToString());

        return map;
    }

    public class Step(Location location, Direction direction, int straightStepCount)
    {
        public Location Location { get; set; } = location;
        public Direction Direction { get; set; } = direction;
        public int StraightStepCount { get; set; } = straightStepCount;
    }

    public enum Direction
    {
        TopUp,
        BottomDown,
        Left,
        Right
    }

    private Direction GetOpposite(Direction direction)
    {
        switch (direction)
        {
            case Direction.TopUp: return Direction.BottomDown;
            case Direction.BottomDown: return Direction.TopUp;
            case Direction.Left: return Direction.Right;
            case Direction.Right: return Direction.Left;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    public Location Add(Location a, Location b) => new(a.Row + b.Row, a.Col + b.Col);

    public class Location(int row, int col)
    {
        public int Row { get; set; } = row;
        public int Col { get; set; } = col;

        public override string ToString() => $"[{Row}, {Col}]";
    }

    private Location GetLocationDelta(Direction direction)
    {
        switch (direction)
        {
            case Direction.TopUp:
                return new Location(-1, 0);
            case Direction.BottomDown:
                return new Location(+1, 0);
            case Direction.Left:
                return new Location(0, -1);
            case Direction.Right:
                return new Location(0, +1);
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }
}