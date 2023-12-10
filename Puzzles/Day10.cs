namespace Puzzles;

public class Day10(string input) : DailyPuzzleBase(input)
{
    public override string GetPuzzleOneSolution()
    {
        var map = Input.Split(Environment.NewLine);

        var start = GetStartLocation(map);
        var startDirection = GetStartDirection(start, map);

        var current = start;
        var currentDirection = startDirection;
        char currentCellType;

        int step = 0;
        do
        {
            current = Add(current, GetLocationDelta(currentDirection));
            currentCellType = map[current.Row][current.Col];

            if (currentCellType != 'S')
                currentDirection = CellConfigurations[currentCellType][DirectionToOppositeDirection[currentDirection]];

            step++;
        } while (currentCellType != 'S');

        return (step / 2).ToString();
    }

    public override string GetPuzzleTwoSolution()
    {
        var map = Input.Split(Environment.NewLine);

        var maze = new Cell[map.Length][];
        var width = map.First().Length;
        for (int row = 0; row < map.Length; row++)
        {
            maze[row] = new Cell[width];
            for (int col = 0; col < width; col++)
            {
                maze[row][col] = new Cell
                {
                    Location = new Location(row, col),
                    Type = map[row][col],
                    IsPath = false,
                    IsInside = false
                };
            }
        }

        var start = GetStartLocation(map);
        var startDirection = GetStartDirection(start, map);

        var startCell = maze[start.Row][start.Col];

        var startPointConnections = new List<Direction>();

        if (IsConnectingToDirection(map, maze, startCell, Direction.Right)) startPointConnections.Add(Direction.Right);
        if (IsConnectingToDirection(map, maze, startCell, Direction.Left)) startPointConnections.Add(Direction.Left);
        if (IsConnectingToDirection(map, maze, startCell, Direction.TopUp)) startPointConnections.Add(Direction.TopUp);
        if (IsConnectingToDirection(map, maze, startCell, Direction.BottomDown)) startPointConnections.Add(Direction.BottomDown);

        var startCellType = CellConfigurations.Single(kv => kv.Value.Keys.All(k => startPointConnections.Contains(k))).Key;
        startCell.Type = startCellType;

        var currentLocation = start;
        var currentDirection = startDirection;

        int leftTurnCount = 0;
        int rightTurnCount = 0;
        do
        {
            var current = maze[currentLocation.Row][currentLocation.Col];
            current.IsPath = true;
            current.IsInside = false;

            currentLocation = Add(currentLocation, GetLocationDelta(currentDirection));

            if (CellRotationDirection.ContainsKey(maze[currentLocation.Row][currentLocation.Col].Type))
            {
                var turnDirection = CellRotationDirection[maze[currentLocation.Row][currentLocation.Col].Type][DirectionToOppositeDirection[currentDirection]];
                if (turnDirection == Direction.Left)
                    leftTurnCount++;
                else
                    rightTurnCount++;
            }

            if (map[currentLocation.Row][currentLocation.Col] != 'S')
                currentDirection = CellConfigurations[map[currentLocation.Row][currentLocation.Col]][DirectionToOppositeDirection[currentDirection]];
        } while (map[currentLocation.Row][currentLocation.Col] != 'S');

        //Start direction
        var startInsideDirection = RotateRight(startDirection);
        if (leftTurnCount > rightTurnCount)
            startInsideDirection = RotateLeft(startDirection);

        MarkInside(map, maze, start, startDirection, startInsideDirection);

        return maze.Sum(mazeRow => mazeRow.Count(c => c.IsInside)).ToString();
    }

    private void MarkInside(string[] map, Cell[][] maze, Location startLocation, Direction startDirection, Direction startInsideDirection)
    {
        var currentLocation = startLocation;
        var currentDirection = startDirection;
        var currentInsideDirection = startInsideDirection;

        var currentCell = maze[currentLocation.Row][currentLocation.Col];
        do
        {
            SetIsInsides(maze, currentCell, currentInsideDirection);

            currentLocation = Add(currentLocation, GetLocationDelta(currentDirection));
            currentCell = maze[currentLocation.Row][currentLocation.Col];

            SetIsInsides(maze, currentCell, currentInsideDirection);

            if (CellRotationDirection.ContainsKey(currentCell.Type))
            {
                if (CellRotationDirection[currentCell.Type][DirectionToOppositeDirection[currentDirection]] == Direction.Left)
                    currentInsideDirection = RotateLeft(currentInsideDirection);
                else
                    currentInsideDirection = RotateRight(currentInsideDirection);
            }

            currentDirection = CellConfigurations[currentCell.Type][DirectionToOppositeDirection[currentDirection]];

        } while (map[currentLocation.Row][currentLocation.Col] != 'S');
    }

    private Direction RotateRight(Direction d)
    {
        switch (d)
        {
            case Direction.TopUp:
                return Direction.Right;
            case Direction.BottomDown:
                return Direction.Left;
            case Direction.Left:
                return Direction.TopUp;
            case Direction.Right:
                return Direction.BottomDown;
            default:
                throw new ArgumentOutOfRangeException(nameof(d), d, null);
        }
    }
    
    private Direction RotateLeft(Direction d)
    {
        switch (d)
        {
            case Direction.TopUp:
                return Direction.Left;
            case Direction.BottomDown:
                return Direction.Right;
            case Direction.Left:
                return Direction.BottomDown;
            case Direction.Right:
                return Direction.TopUp;
            default:
                throw new ArgumentOutOfRangeException(nameof(d), d, null);
        }
    }

    private bool IsConnectingToDirection(string[] map, Cell[][] maze, Cell startCell, Direction direction)
    {
        bool isConnecting = false;

        var connectingLocation = Add(startCell.Location, GetLocationDelta(direction));
        if (!IsOutOfBoundary(map, connectingLocation))
        {
            isConnecting = CellConfigurations
                .Where(kv => kv.Value.Keys.Contains(DirectionToOppositeDirection[direction]))
                .Select(kv => kv.Key)
                .Contains(maze[connectingLocation.Row][connectingLocation.Col].Type);
        }

        return isConnecting;
    }

    private void SetIsInsides(Cell[][] maze, Cell start, Direction direction)
    {
        var currentLocation = start.Location;
        do
        {
            currentLocation = Add(currentLocation, GetLocationDelta(direction));

            if (!maze[currentLocation.Row][currentLocation.Col].IsPath)
                maze[currentLocation.Row][currentLocation.Col].IsInside = true;

        } while (!maze[currentLocation.Row][currentLocation.Col].IsPath);
    }

    private Direction GetStartDirection(Location start, string[] map)
    {
        var possibleStartDirections = new List<Direction> { Direction.Left, Direction.Right, Direction.BottomDown, Direction.TopUp };
        var startDirection = possibleStartDirections.First();

        foreach (var d in possibleStartDirections)
        {
            var startCell = Add(start, GetLocationDelta(d));
            if (!IsOutOfBoundary(map, startCell))
            {
                var cellType = map[startCell.Row][startCell.Col];
                if (cellType != '.')
                {
                    startDirection = d;
                    if (CellConfigurations[cellType].ContainsKey(DirectionToOppositeDirection[startDirection]))
                        break;
                }
            }
        }

        return startDirection;
    }

    private bool IsOutOfBoundary(string[] map, Location location) => location.Row < 0 || location.Col < 0 || location.Row >= map.Length || location.Col >= map[0].Length;

    private static Location GetStartLocation(string[] map)
    {
        for (int row = 0; row < map.Length; row++)
        {
            for (int col = 0; col < map[0].Length; col++)
            {
                if (map[row][col] == 'S')
                {
                    return new Location(row, col);
                }
            }
        }

        throw new Exception("!");
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
        public bool IsInside { get; set; }
        public bool IsPath { get; set; }

        public override string ToString() => $"{Type}-{Location} [I:{IsInside}, L:{IsPath}]";
    }

    private Dictionary<char, Dictionary<Direction, Direction>> CellConfigurations = new()
    {
        //Type - From - To
        { '-', new Dictionary<Direction, Direction> { { Direction.Left, Direction.Right}, { Direction.Right, Direction.Left} }},
        { '|', new Dictionary<Direction, Direction> { { Direction.BottomDown, Direction.TopUp}, { Direction.TopUp, Direction.BottomDown} }},
        { 'F', new Dictionary<Direction, Direction> { { Direction.BottomDown, Direction.Right}, { Direction.Right, Direction.BottomDown } }},
        { 'L', new Dictionary<Direction, Direction> { { Direction.TopUp, Direction.Right}, { Direction.Right, Direction.TopUp } }},
        { '7', new Dictionary<Direction, Direction> { { Direction.Left, Direction.BottomDown}, { Direction.BottomDown, Direction.Left} }},
        { 'J', new Dictionary<Direction, Direction> { { Direction.Left, Direction.TopUp}, { Direction.TopUp, Direction.Left} }},
    };
    
    private Dictionary<char, Dictionary<Direction, Direction>> CellRotationDirection = new()
    {
        //Type - FromDirection - TurnDirection
        { 'F', new Dictionary<Direction, Direction> { { Direction.BottomDown, Direction.Right}, { Direction.Right, Direction.Left} }},
        { 'L', new Dictionary<Direction, Direction> { { Direction.TopUp, Direction.Left}, { Direction.Right, Direction.Right } }},
        { '7', new Dictionary<Direction, Direction> { { Direction.Left, Direction.Right}, { Direction.BottomDown, Direction.Left} }},
        { 'J', new Dictionary<Direction, Direction> { { Direction.Left, Direction.Left}, { Direction.TopUp, Direction.Right} }},
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