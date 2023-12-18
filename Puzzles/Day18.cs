using System.Diagnostics;

namespace Puzzles;

public class Day18(string input) : DailyPuzzleBase(input)
{
    public override string GetPuzzleOneSolution()
    {
        var digSteps = GetDigMap();
        
        var rightSum = digSteps.Where(s => s.Direction == Direction.Right).Sum(d => d.Length);
        var downSum = digSteps.Where(s => s.Direction == Direction.BottomDown).Sum(d => d.Length);
        var leftSum = digSteps.Where(s => s.Direction == Direction.Left).Sum(d => d.Length);
        var upSum = digSteps.Where(s => s.Direction == Direction.TopUp).Sum(d => d.Length);

        var width = rightSum + leftSum + 2;
        var height = upSum + downSum + 2;

        var holeMap = new List<List<bool>>();
        for (int row = 0; row < height; row++)
        {
            holeMap.Add(new List<bool>());
            for (int col = 0; col < width; col++)
                holeMap[row].Add(false);
        }

        var current = new Location(downSum, rightSum);
        holeMap[current.Row][current.Col] = true;
        for (var index = 0; index < digSteps.Count; index++)
        {
            var digStep = digSteps[index];
            for (int i = 0; i < digStep.Length; i++)
            {
                current = Add(current, GetLocationDelta(digStep.Direction));
                holeMap[current.Row][current.Col] = true;
            }
        }

        for (int row = 0; row < height; row++)
        {
            Debug.WriteLine(string.Join("", holeMap[row].Select(h => h ? '#' : '.')));
        }

        var copy = holeMap.Select(t => t.ToList()).ToList();

        for (int row = 0; row < holeMap.Count; row++)
        {
            var firstHoleIndex = holeMap[row].FindIndex(h => h);
            var lastHoleIndex = holeMap[row].FindLastIndex(h => h);
            
            bool topHole = false;
            bool bottomHole = false;
            bool dig = false;

            if (firstHoleIndex > 0)
            {
                for (int col = firstHoleIndex; col < lastHoleIndex + 1; col++)
                {
                    if (holeMap[row][col])
                    {
                        if (holeMap[row - 1][col] || holeMap[row + 1][col])
                        {
                            if (
                                    (holeMap[row - 1][col] && holeMap[row + 1][col]) || 
                                    (holeMap[row - 1][col] && bottomHole) || 
                                    (topHole && holeMap[row + 1][col])
                                )
                            {
                                if(!holeMap[row][col + 1])
                                    dig = !dig;
                            }

                            topHole = holeMap[row - 1][col];
                            bottomHole = holeMap[row + 1][col];
                        }
                    }

                    if (dig)
                        copy[row][col] = true;
                }
            }
        }

        return copy.Sum(r => r.Count(h => h)).ToString();
    }

    public override string GetPuzzleTwoSolution()
    {
        var digSteps = GetDigMap(true);
        return "";
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
    }

    public class DigStep(Direction direction, int length)
    {
        public Direction Direction { get; set; } = direction;
        public int Length { get; set; } = length;
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