using System;
using System.Data;
using System.Drawing;

namespace Puzzles;

public class Day21(string input) : DailyPuzzleBase(input)
{
    public override string GetPuzzleOneSolution()
    {
        var dailyStepLimit = GetDailyStepLimit(64);

        var map = GetMap();
        var start = GetMapStart(map);

        var directions = new[] { Direction.TopUp, Direction.BottomDown, Direction.Right, Direction.Left };

        var previous = new List<Location> { start };
        for (int i = 0; i < dailyStepLimit; i++)
        {
            var current = new List<Location>();
            foreach (var loc in previous)
            {
                foreach (var d in directions)
                {
                    var location = Add(loc, GetLocationDelta(d));
                    if (!IsOutOfBoundary(map, location) && map[location.Row, location.Col] != '#')
                        if (current.All(l => l.Row != location.Row || l.Col != location.Col))
                            current.Add(location);
                }
            }

            previous = current;
        }

        return previous.Count.ToString();
    }

    public override string GetPuzzleTwoSolution()
    {
        if (string.IsNullOrEmpty(Input))
            return string.Empty;

        //Note: From reddit

        var input = Input.Split(Environment.NewLine).ToList();
        var gridSize = input.Count == input[0].Length ? input.Count : throw new ArgumentOutOfRangeException();

        var start = Enumerable.Range(0, gridSize)
            .SelectMany(i => Enumerable.Range(0, gridSize)
                .Where(j => input[i][j] == 'S')
                .Select(j => new Location(i, j)))
            .Single();

        var grids = 26501365 / input.Count;
        var rem = 26501365 % input.Count;

        // By inspection, the grid is square and there are no barriers on the direct horizontal / vertical path from S
        // So, we'd expect the result to be quadratic in (rem + n * gridSize) steps, i.e. (rem), (rem + gridSize), (rem + 2 * gridSize), ...
        // Use the code from Part 1 to calculate the first three values of this sequence, which is enough to solve for ax^2 + bx + c
        var sequence = new List<int>();
        var work = new HashSet<Location> { start };
        var steps = 0;
        for (var n = 0; n < 3; n++)
        {
            for (; steps < n * gridSize + rem; steps++)
            {
                // Funky modulo arithmetic bc modulo of a negative number is negative, which isn't what we want here
                work = new HashSet<Location>(work
                    .SelectMany(it => new[] { Direction.TopUp, Direction.BottomDown, Direction.Right, Direction.Left }.Select(dir => Add(it, GetLocationDelta(dir))))
                    .Where(dest => input[((dest.Row % 131) + 131) % 131][((dest.Col % 131) + 131) % 131] != '#'));
            }

            sequence.Add(work.Count);
        }

        // Solve for the quadratic coefficients
        var c = sequence[0];
        var aPlusB = sequence[1] - c;
        var fourAPlusTwoB = sequence[2] - c;
        var twoA = fourAPlusTwoB - (2 * aPlusB);
        var a = twoA / 2;
        var b = aPlusB - a;

        long F(long n)
        {
            return a * (n * n) + b * n + c;
        }

        return F(grids).ToString();


        //var dailyStepLimit = GetDailyStepLimit(26501365);
        //var iterationCount = GetRepetitionIterationCount(dailyStepLimit);

        //var map = GetMap();
        //var mapSize = map.GetLength(0) == map.GetLength(1) && map.GetLength(0) == iterationCount ? iterationCount : throw new ArgumentOutOfRangeException();

        //var start = GetMapStart(map);

        //var mapCount = dailyStepLimit / iterationCount;
        //var rem = dailyStepLimit % iterationCount;

        //var directions = new[] { Direction.TopUp, Direction.BottomDown, Direction.Right, Direction.Left };
        //var sequence = new List<int>();
        //var previous = new List<Location> { start };
        //var steps = 0;
        //for (var n = 0; n < 3; n++)
        //{
        //    for (; steps < n * mapSize + rem; steps++)
        //    {
        //        var current = new List<Location>();
        //        foreach (var loc in previous)
        //        {
        //            map[loc.Row, loc.Col] = '.';
        //            foreach (var d in directions)
        //            {
        //                var location = Add(loc, GetLocationDelta(d));
        //                if (map[((location.Row % iterationCount) + iterationCount) % iterationCount, ((location.Col % iterationCount) + iterationCount) % iterationCount] != '#')
        //                {
        //                    if (current.All(l => l.Row != location.Row || l.Col != location.Col))
        //                        current.Add(location);

        //                }
        //            }
        //        }
        //    }

        //    sequence.Add(previous.Count);
        //}

        //// Solve for the quadratic coefficients
        //var c = sequence[0];
        //var aPlusB = sequence[1] - c;
        //var fourAPlusTwoB = sequence[2] - c;
        //var twoA = fourAPlusTwoB - (2 * aPlusB);
        //var a = twoA / 2;
        //var b = aPlusB - a;

        //long F(long n)
        //{
        //    return a * (n * n) + b * n + c;
        //}

        //return F(mapCount).ToString();
    }

    private int GetRepetitionIterationCount(int dailyStepLimit)
    {
        var map = GetMap();
        var start = GetMapStart(map);
        map[start.Row, start.Col] = 'O';

        var directions = new[] { Direction.TopUp, Direction.BottomDown, Direction.Right, Direction.Left };

        var cache = new HashSet<string>();

        var previous = new List<Location> { start };
        for (int i = 0; i < dailyStepLimit; i++)
        {
            var key = GetKey(map);
            if (cache.Contains(key))
                return i;

            var current = new List<Location>();
            foreach (var loc in previous)
            {
                map[loc.Row, loc.Col] = '.';
                foreach (var d in directions)
                {
                    var location = Add(loc, GetLocationDelta(d));
                    if (!IsOutOfBoundary(map, location) && map[location.Row, location.Col] != '#')
                    {
                        if (current.All(l => l.Row != location.Row || l.Col != location.Col))
                            current.Add(location);

                        map[location.Row, location.Col] = 'O';
                    }
                }

                cache.Add(key);
            }

            previous = current;
        }

        throw new Exception();
    }

    private int GetDailyStepLimit(int defaultStepLimit)
    {
        var dailyStepLimit = defaultStepLimit;

        var firstLine = Input.Split(Environment.NewLine).First();
        if (firstLine.Length < 5)
            dailyStepLimit = int.Parse(firstLine.Trim());

        return dailyStepLimit;
    }

    private string GetKey(char[,] map)
    {
        string key = string.Empty;
        for (var row = 0; row < map.GetLength(0); row++)
            for (var col = 0; col < map.GetLength(1); col++)
                key += map[row, col];

        return key;
    }

    private Location GetMapStart(char[,] map)
    {
        Location start = null;
        for (var row = 0; row < map.GetLength(0); row++)
            for (var col = 0; col < map.GetLength(1); col++)
                if (map[row, col] == 'S')
                    start = new Location(row, col);

        return start;
    }

    private bool IsOutOfBoundary(char[,] map, Location location) => location.Row < 0 || location.Col < 0 || location.Row >= map.GetLength(0) || location.Col >= map.GetLength(1);

    private char[,] GetMap()
    {
        var lines = Input.Split(Environment.NewLine);
        var firstLine = lines.First();
        if (firstLine.Length < 5)
            lines = lines.Skip(1).ToArray();

        var map = new char[lines.Length, lines[0].Length];

        for (var row = 0; row < lines.Length; row++)
            for (var col = 0; col < lines[0].Length; col++)
                map[row, col] = lines[row][col];

        return map;
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

    public record Location(int Row, int Col)
    {
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