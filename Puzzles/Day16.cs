namespace Puzzles;

public class Day16(string input) : DailyPuzzleBase(input)
{
    public override string GetPuzzleOneSolution()
    {
        return Traverse(GetMap(), new Location(0, -1), Direction.Right).Sum(row => row.Count(c => c.IsEnergized)).ToString();
    }

    public override string GetPuzzleTwoSolution()
    {
        var map = GetMap();

        var entranceList = new List<Tuple<Location, Direction>>();
        entranceList.AddRange(Enumerable.Range(0, map.Length).Select(i => new Tuple<Location, Direction>(new Location(i, -1), Direction.Right)));
        entranceList.AddRange(Enumerable.Range(0, map.Length).Select(i => new Tuple<Location, Direction>(new Location(i, map[0].Length), Direction.Left)));
        entranceList.AddRange(Enumerable.Range(0, map[0].Length).Select(i => new Tuple<Location, Direction>(new Location(-1, i), Direction.BottomDown)));
        entranceList.AddRange(Enumerable.Range(0, map[0].Length).Select(i => new Tuple<Location, Direction>(new Location(map.Length, i), Direction.TopUp)));

        return entranceList.Max(e => Traverse(Reset(map), e.Item1, e.Item2).Sum(row => row.Count(c => c.IsEnergized))).ToString();
    }

    private Cell[][] GetMap()
    {
        var rawMap = Input.Split(Environment.NewLine);

        var map = new Cell[rawMap.Length][];
        var width = rawMap.First().Length;
        for (int row = 0; row < rawMap.Length; row++)
        {
            map[row] = new Cell[width];
            for (int col = 0; col < width; col++)
            {
                map[row][col] = new Cell
                {
                    Location = new Location(row, col),
                    Type = rawMap[row][col],
                    IsEnergized = false,
                    ExitDirections = new List<Direction>()
                };
            }
        }

        return map;
    }

    private Cell[][] Reset(Cell[][] map)
    {
        for (int row = 0; row < map.Length; row++)
        {
            for (int col = 0; col < map[0].Length; col++)
            {
                map[row][col].IsEnergized = false;
                map[row][col].ExitDirections = new List<Direction>();
            }
        }

        return map;
    }

    private Cell[][] Traverse(Cell[][] map, Location location, Direction direction)
    {
        var currentLocation = Add(location, GetLocationDelta(direction));
        if(currentLocation.Row >= map.Length || currentLocation.Row < 0 || currentLocation.Col >= map[0].Length ||  currentLocation.Col < 0)
            return map;

        var currentCell = map[currentLocation.Row][currentLocation.Col];
        currentCell.IsEnergized = true;

        var directions = CellConfigurations[currentCell.Type][DirectionToOppositeDirection[direction]];
        if (directions.All(currentCell.ExitDirections.Contains))
            return map;

        var notTraversedDirections = directions.Where(d => !currentCell.ExitDirections.Contains(d)).ToList();
        currentCell.ExitDirections.AddRange(notTraversedDirections);
        foreach (var notTraversedDirection in notTraversedDirections)
            Traverse(map, currentLocation, notTraversedDirection);

        return map;
    }

    public enum Direction
    {
        TopUp,
        BottomDown,
        Left,
        Right
    }

    private Dictionary<Direction, Direction> DirectionToOppositeDirection = new()
    {
        { Direction.TopUp ,Direction.BottomDown},
        { Direction.BottomDown , Direction.TopUp},
        { Direction.Left ,Direction.Right},
        { Direction.Right ,Direction.Left}
    };

    public Location Add(Location a, Location b) => new(a.Row + b.Row, a.Col + b.Col);

    public class Location(int row, int col)
    {
        public int Row { get; set; } = row;
        public int Col { get; set; } = col;

        public override string ToString() => $"({Row}, {Col})";
    }

    public class Cell
    {
        public Location Location { get; set; }
        public char Type { get; set; }

        public bool IsEnergized { get; set; }

        public List<Direction> ExitDirections { get; set; } = new();

        public override string ToString() => $"{Type}-{Location} [E:{IsEnergized}]";
    }

    private Dictionary<char, Dictionary<Direction, List<Direction>>> CellConfigurations = new()
    {
        //Type - From - To
        { '.', new Dictionary<Direction, List<Direction>> { { Direction.Left, [Direction.Right] }, { Direction.Right, [Direction.Left]}, { Direction.BottomDown, [Direction.TopUp]}, { Direction.TopUp, [Direction.BottomDown]} }},
        { '-', new Dictionary<Direction, List<Direction>> { { Direction.Left, [Direction.Right]}, { Direction.Right, [Direction.Left]}, {Direction.TopUp, [Direction.Left, Direction.Right]}, {Direction.BottomDown, [Direction.Left, Direction.Right]} }},
        { '|', new Dictionary<Direction, List<Direction>> { { Direction.BottomDown, [Direction.TopUp]}, { Direction.TopUp, [Direction.BottomDown]}, { Direction.Left, [Direction.BottomDown, Direction.TopUp]}, { Direction.Right, [Direction.BottomDown, Direction.TopUp] } }},
        { '\\', new Dictionary<Direction, List<Direction>> { { Direction.BottomDown, [Direction.Left]}, { Direction.Left, [Direction.BottomDown]}, { Direction.Right, [Direction.TopUp]}, { Direction.TopUp, [Direction.Right]} }},
        { '/', new Dictionary<Direction, List<Direction>> { { Direction.BottomDown, [Direction.Right]}, { Direction.Left, [Direction.TopUp]}, { Direction.Right, [Direction.BottomDown]}, { Direction.TopUp, [Direction.Left]} }},
    };

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