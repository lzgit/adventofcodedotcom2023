namespace Puzzles;

public class Day10(string input) : DailyPuzzleBase(input)
{
    public override string GetPuzzleOneSolution()
    {
        var map = Input.Split(Environment.NewLine);

        var start = GetStartCell(map);
        var startDirection = GetStartDirection(start, map);


        var currentCell = start;
        var nextStepDirection = startDirection;
        char currentCellType;

        int step = 0;
        do
        {
            currentCell = Add(currentCell, DirectionToCellDelta(nextStepDirection));
            currentCellType = map[currentCell.Row][currentCell.Col];

            if(currentCellType != 'S')
                nextStepDirection = CellConfigurations[currentCellType][DirectionConnections[nextStepDirection]];

            step++;
        } while (currentCellType != 'S');

        return (step / 2).ToString();
    }

    private Direction GetStartDirection(Cell start, string[] map)
    {
        var possibleStartDirections = new List<Direction> { Direction.Left, Direction.Right, Direction.BottomDown, Direction.TopUp };
        var startDirection = possibleStartDirections.First();

        foreach (var d in possibleStartDirections)
        {
            var startCell = Add(start, DirectionToCellDelta(d));
            if (!IsOutOfBoundary(map, startCell))
            {
                var cellType = map[startCell.Row][startCell.Col];
                if (cellType != '.')
                {
                    startDirection = d;
                    if (CellConfigurations[cellType].ContainsKey(DirectionConnections[startDirection]))
                        break;
                }
            }
        }

        return startDirection;
    }

    private bool IsOutOfBoundary(string[] map, Cell cell)
    {
        return cell.Row < 0 || cell.Col < 0 || cell.Row >= map.Length || cell.Col >= map[0].Length;
    }

    private static Cell GetStartCell(string[] map)
    {
        for (int row = 0; row < map.Length; row++)
        {
            for (int col = 0; col < map[0].Length; col++)
            {
                if (map[row][col] == 'S')
                {
                    return new Cell(row, col);
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

    private Dictionary<Direction, Direction> DirectionConnections = new()
    {
        { Direction.TopUp ,Direction.BottomDown},
        { Direction.BottomDown , Direction.TopUp},
        { Direction.Left ,Direction.Right},
        { Direction.Right ,Direction.Left}
    };

    public Cell Add(Cell a, Cell b) => new(a.Row + b.Row, a.Col + b.Col);

    public class Cell(int row, int col)
    {
        public int Row { get; set; } = row;
        public int Col { get; set; } = col;

        public override string ToString() => $"({Row}, {Col})";
    }

    private Dictionary<char, Dictionary<Direction, Direction>> CellConfigurations = new()
    {
        //Cell - From - To
        { '-', new Dictionary<Direction, Direction> { { Direction.Left, Direction.Right}, { Direction.Right, Direction.Left} }},
        { '|', new Dictionary<Direction, Direction> { { Direction.BottomDown, Direction.TopUp}, { Direction.TopUp, Direction.BottomDown} }},
        { 'F', new Dictionary<Direction, Direction> { { Direction.BottomDown, Direction.Right}, { Direction.Right, Direction.BottomDown } }},
        { 'L', new Dictionary<Direction, Direction> { { Direction.TopUp, Direction.Right}, { Direction.Right, Direction.TopUp } }},
        { '7', new Dictionary<Direction, Direction> { { Direction.Left, Direction.BottomDown}, { Direction.BottomDown, Direction.Left} }},
        { 'J', new Dictionary<Direction, Direction> { { Direction.Left, Direction.TopUp}, { Direction.TopUp, Direction.Left} }},
    };

    private Cell DirectionToCellDelta(Direction direction)
    {
        switch (direction)
        {
            case Direction.TopUp:
                return new Cell(-1, 0);
            case Direction.BottomDown:
                return new Cell(+1, 0);
            case Direction.Left:
                return new Cell(0, -1);
            case Direction.Right:
                return new Cell(0, +1);
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }
    
    public override string GetPuzzleTwoSolution()
    {
        return "";
    }
}