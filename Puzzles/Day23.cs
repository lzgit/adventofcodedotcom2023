using System.Diagnostics;

namespace Puzzles;

public class Day23(string input) : DailyPuzzleBase(input)
{
    public override string GetPuzzleOneSolution()
    {
        var map = GetMap();
        return GetMaxHikeLength(map, CreateMapVisitedMatrix(map), GetStartLocation(map), GetEndLocation(map), -1, false).ToString();
    }

    public override string GetPuzzleTwoSolution()
    {
        var map = GetMap();
        return GetMaxHikeLength(map, CreateMapVisitedMatrix(map), GetStartLocation(map), GetEndLocation(map), -1, true).ToString();
    }

    private static Location GetStartLocation(string[] map)
    {
        Location start = null;
        for (int col = 0; col < map.Length; col++)
            if (map[0][col] == '.')
                start = new Location(0, col);
        return start!;
    }

    private static Location GetEndLocation(string[] map)
    {
        Location end = null;
        for (int col = 0; col < map.Length; col++)
            if (map[map.First().Length - 1][col] == '.')
                end = new Location(map.First().Length - 1, col);
        return end!;
    }

    private int GetMaxHikeLength(string[] map, bool?[,] mapVisitedMatrix, Location currentLocation, Location end, int hikeLength, bool ignoreSlopes)
    {
        var currentHikeLength = hikeLength + 1;
        mapVisitedMatrix[currentLocation.Row, currentLocation.Col] = true;

        if (currentLocation == end)
        {
            //Debug.WriteLine("");
            //Debug.WriteLine(currentHikeLength);
            //for (int r = 0; r < map.Length; r++)
            //{
            //    for (int c = 0; c < map.First().Length; c++)
            //    {
            //        Debug.Write(mapVisitedMatrix[r, c] == true ? 'O' : map[r][c]);
            //    }
            //    Debug.WriteLine("");
            //}

            return currentHikeLength;
        }

        if (!ignoreSlopes && IsSlope(map, currentLocation))
        {
            var nextStepLocation = currentLocation.Move(SlopeDirections[map[currentLocation.Row][currentLocation.Col]]);
            if (map[nextStepLocation.Row][nextStepLocation.Col] == '#')
                return currentHikeLength;

            return GetMaxHikeLength(map, mapVisitedMatrix, nextStepLocation, end, currentHikeLength, ignoreSlopes);
        }

        var nextStepLocations = new List<Location>();
        foreach (var direction in Enum.GetValues<Direction>())
        {
            var nextStepLocation = currentLocation.Move(direction);
            if (!IsOutOfBoundary(map, nextStepLocation))
            {
                var nextStepValue = map[nextStepLocation.Row][nextStepLocation.Col];
                if (
                    nextStepValue != '#' &&
                    mapVisitedMatrix[nextStepLocation.Row, nextStepLocation.Col] != true &&
                    (ignoreSlopes || !IsSlope(nextStepValue) || SlopeDirections[nextStepValue] == direction)
                    )
                {
                    nextStepLocations.Add(nextStepLocation);
                }
            }
        }

        //-1 == DeadEnd
        if (!nextStepLocations.Any())
            return -1;

        if (nextStepLocations.Count == 1)
            return GetMaxHikeLength(map, mapVisitedMatrix, nextStepLocations.Single(), end, currentHikeLength, ignoreSlopes);

        return nextStepLocations.Max(l => GetMaxHikeLength(map, CloneMapVisitedMatrix(mapVisitedMatrix), l, end, currentHikeLength, ignoreSlopes));
    }

    private string[] GetMap() => Input.Split(Environment.NewLine);

    private bool?[,] CreateMapVisitedMatrix(string[] map) => new bool?[map.Length, map.First().Length];
    private bool?[,] CloneMapVisitedMatrix(bool?[,] mapVisitedMatrix)
    {
        var copy = new bool?[mapVisitedMatrix.GetLength(0), mapVisitedMatrix.GetLength(1)];
        for (int row = 0; row < mapVisitedMatrix.GetLength(0); row++)
            for (int col = 0; col < mapVisitedMatrix.GetLength(1); col++)
                if (mapVisitedMatrix[row, col] == true)
                    copy[row, col] = true;

        return copy;
    }

    private Dictionary<char, Direction> SlopeDirections = new()
    {
        {'<',Direction.Left},
        {'>',Direction.Right},
        {'^',Direction.TopUp},
        {'v',Direction.BottomDown}
    };

    private bool IsOutOfBoundary(string[] map, Location location) => location.Row < 0 || location.Col < 0 || location.Row >= map.Length || location.Col >= map.First().Length;
    private bool IsSlope(string[] map, Location location) => IsSlope(map[location.Row][location.Col]);
    private bool IsSlope(char ch) => SlopeDirections.Keys.Contains(ch);

    public enum Direction
    {
        TopUp,
        BottomDown,
        Left,
        Right
    }

    public record Location(int Row, int Col)
    {
        public override string ToString() => $"[{Row}, {Col}]";

        public Location Move(Direction direction, int distance = 1)
        {
            switch (direction)
            {
                case Direction.TopUp:
                    return this with { Row = Row - distance };
                case Direction.BottomDown:
                    return this with { Row = Row + distance };
                case Direction.Left:
                    return this with { Col = Col - distance };
                case Direction.Right:
                    return this with { Col = Col + distance };
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}