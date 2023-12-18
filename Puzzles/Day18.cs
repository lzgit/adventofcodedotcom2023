namespace Puzzles;

public class Day18(string input) : DailyPuzzleBase(input)
{
    public override string GetPuzzleOneSolution() => GetAreaSize(false).ToString();
    public override string GetPuzzleTwoSolution() => GetAreaSize(true).ToString();

    private long GetAreaSize(bool useDecodedInstructions)
    {
        var digSteps = GetDigMap(useDecodedInstructions);
        var trenches = GetTrenches(digSteps);

        var minRow = trenches.Min(dtp => dtp.TopLeft.Row);
        var maxRow = trenches.Max(dtp => dtp.BottomRight.Row);
        var minCol = trenches.Min(dtp => dtp.TopLeft.Col);
        var maxCol = trenches.Max(dtp => dtp.BottomRight.Col);

        bool IsTrench(List<Trench> trenchList, int r, int c) => trenchList.Any(t => t.Contains(r, c));

        long capacity = 0;
        for (int row = minRow; row < maxRow + 1; row++)
        {
            bool topHole = false;
            bool bottomHole = false;
            bool dig = false;

            //var line = "";
            var trenchesInCurrentRow = trenches.Where(t => t.TopLeft.Row <= row && row <= t.BottomRight.Row).ToList();
            var trenchesAboveCurrentRow = trenches.Where(t => t.TopLeft.Row <= row - 1 && row - 1 <= t.BottomRight.Row).ToList();
            var trenchesBelowCurrentRow = trenches.Where(t => t.TopLeft.Row <= row + 1 && row + 1 <= t.BottomRight.Row).ToList();

            var subsequentTrenchCount = 0;
            for (int col = minCol; col < maxCol + 1; col++)
            {
                if (IsTrench(trenchesInCurrentRow, row, col))
                {
                    var currentTrenches = trenchesInCurrentRow.Where(t => t.Contains(row, col)).ToList();

                    if (currentTrenches.Count == 2)
                        subsequentTrenchCount = 0;

                    if (subsequentTrenchCount > 4)
                    {
                        var currentTrench = currentTrenches.Single();
                        capacity += currentTrench.BottomRight.Col - col;
                        //line += string.Join("", Enumerable.Repeat('#', currentTrench.BottomRight.Col - col));
                        col = currentTrench.BottomRight.Col - 1;
                        subsequentTrenchCount = 0;
                    }
                    else
                    {
                        subsequentTrenchCount++;

                        var isRowAboveTrench = IsTrench(trenchesAboveCurrentRow, row - 1, col);
                        var isRowBelowTrench = IsTrench(trenchesBelowCurrentRow, row + 1, col);
                        if (isRowAboveTrench || isRowBelowTrench)
                        {
                            if (
                                (isRowAboveTrench && isRowBelowTrench) ||
                                (isRowAboveTrench && bottomHole) ||
                                (topHole && isRowBelowTrench)
                            )
                            {
                                if (!IsTrench(trenchesInCurrentRow, row, col + 1))
                                    dig = !dig;
                            }

                            topHole = isRowAboveTrench;
                            bottomHole = isRowBelowTrench;
                        }

                        capacity++;
                        //line += "#";
                    }
                }
                else
                {
                    var minByTrenchCol = trenchesInCurrentRow.Where(t => col < t.TopLeft.Col).MinBy(t => t.TopLeft.Col);
                    if (minByTrenchCol != null)
                    {
                        capacity += dig ? minByTrenchCol.TopLeft.Col - col : 0;
                        //line += string.Join("", Enumerable.Repeat(dig ? '#' : '.', minByTrenchCol.TopLeft.Col - col));
                        col = minByTrenchCol.TopLeft.Col - 1;
                    }
                    else
                    {
                        //line += string.Join("", Enumerable.Repeat('.', maxCol - col + 1));
                        col = maxCol;
                    }

                    subsequentTrenchCount = 0;
                }
            }

            //Debug.WriteLine(line);
        }

        return capacity;
    }

    private List<Trench> GetTrenches(List<DigStep> digSteps)
    {
        var trenches = new List<Trench>();

        var currentLocation = new Location(0, 0);
        var previous = currentLocation;
        foreach (var digStep in digSteps)
        {
            currentLocation = Add(currentLocation, GetLocationDelta(digStep.Direction, digStep.Length));
            trenches.Add(new Trench(previous, currentLocation));
            previous = currentLocation;
        }

        return trenches;
    }

    private List<DigStep> GetDigMap(bool useDecoded = false)
    {
        return Input.Split(Environment.NewLine).Select(l =>
        {
            var lineParts = l.Split(" ");
            var colorCode = lineParts[2].Trim().Trim('(').Trim(')');
            var directionCharacter = lineParts[0].Trim().Single();
            var length = int.Parse(lineParts[1].Trim());

            if (useDecoded)
            {
                directionCharacter = colorCode.Last();
                length = Convert.ToInt32(colorCode.Substring(1, 5), 16);
            }

            return new DigStep(GetDirection(directionCharacter), length);
        }).ToList();
    }

    private Direction GetDirection(char directionCharacter)
    {
        switch (directionCharacter)
        {
            case 'L': case '2': return Direction.Left;
            case 'R': case '0': return Direction.Right;
            case 'U': case '3': return Direction.TopUp;
            case 'D': case '1': return Direction.BottomDown;
            default:
                throw new ArgumentOutOfRangeException(nameof(directionCharacter), directionCharacter, null);
        }
    }

    public enum Direction
    {
        TopUp,
        BottomDown,
        Left,
        Right
    }

    public Location Add(Location a, Location b) => new(a.Row + b.Row, a.Col + b.Col);

    public class Location(int row, int col)
    {
        public int Row { get; set; } = row;
        public int Col { get; set; } = col;

        public override string ToString() => $"[{Row}, {col}]";
    }

    public class DigStep(Direction direction, int length)
    {
        public Direction Direction { get; set; } = direction;
        public int Length { get; set; } = length;
    }

    private Location GetLocationDelta(Direction direction, int distance = 1)
    {
        switch (direction)
        {
            case Direction.TopUp:
                return new Location(-distance, 0);
            case Direction.BottomDown:
                return new Location(+distance, 0);
            case Direction.Left:
                return new Location(0, -distance);
            case Direction.Right:
                return new Location(0, +distance);
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    public class Trench
    {
        public Trench(Location start, Location end)
        {
            Start = start;
            End = end;
            IsHorizontal = Start.Row == End.Row;
            TopLeft = IsHorizontal ? (Start.Col < End.Col ? Start : End) : (Start.Row < End.Row ? Start : End);
            BottomRight = TopLeft.Row == Start.Row && TopLeft.Col == Start.Col ? End : Start;
        }

        public Location Start { get; }
        public Location End { get; }

        public bool IsHorizontal { get; }

        public Location TopLeft { get; }
        public Location BottomRight { get; }

        public override string ToString() => $"TL:{TopLeft}, BR:{BottomRight}";
        public bool Contains(Location location) => Contains(location.Row, location.Col);
        public bool Contains(int row, int col) => TopLeft.Row <= row && row <= BottomRight.Row && TopLeft.Col <= col && col <= BottomRight.Col;
    }
}