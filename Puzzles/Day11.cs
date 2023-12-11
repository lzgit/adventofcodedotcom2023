namespace Puzzles;

public class Day11(string input) : DailyPuzzleBase(input)
{
    public override string GetPuzzleOneSolution()
    {
        return GetGalaxyDistancesSum(GetUniverse(), 2).ToString();
    }

    public override string GetPuzzleTwoSolution()
    {
        return GetGalaxyDistancesSum(GetUniverse(), 1000000).ToString();
    }

    private static long GetGalaxyDistancesSum(Universe universe, int galaxySizeMultiplier)
    {
        var sum = universe.GalaxyPairs
            .Sum(gp =>
            {
                var minRow = Math.Min(gp.Item1.Row, gp.Item2.Row);
                var maxRow = Math.Max(gp.Item1.Row, gp.Item2.Row);
                var emptyRowCount = universe.EmptyRows.Count(r => minRow < r && r < maxRow);
                
                var minCol = Math.Min(gp.Item1.Col, gp.Item2.Col);
                var maxCol = Math.Max(gp.Item1.Col, gp.Item2.Col);
                var emptyColCount = universe.EmptyColumns.Count(c => minCol < c && c < maxCol);

                var rowDistance = Math.Abs(gp.Item1.Row - gp.Item2.Row) - emptyRowCount + emptyRowCount * galaxySizeMultiplier;
                var colDistance = Math.Abs(gp.Item1.Col - gp.Item2.Col) - emptyColCount + emptyColCount * galaxySizeMultiplier;

                return (long)(rowDistance + colDistance);
            });
        return sum;
    }

    private Universe GetUniverse()
    {
        var originalUniverse = Input.Split(Environment.NewLine).Select(l => l.Select(ch => ch).ToList()).ToList();

        var universe = new Universe();
        universe.GalaxyPairs = GetGalaxyPairs(originalUniverse);

        for (int row = 0; row < originalUniverse.Count; row++)
            if(originalUniverse[row].All(c => c == '.'))
                universe.EmptyRows.Add(row);

        for (int col = 0; col < originalUniverse.First().Count; col++)
        {
            bool isAllEmpty = true;
            for (int row = 0; row < originalUniverse.Count; row++)
            {
                if (originalUniverse[row][col] == '#')
                    isAllEmpty = false;
            }

            if(isAllEmpty)
                universe.EmptyColumns.Add(col);
        }

        return universe;
    }

    private static List<Tuple<Galaxy, Galaxy>> GetGalaxyPairs(List<List<char>> universe)
    {
        var galaxies = new List<Galaxy>();
        for (int row = 0; row < universe.Count; row++)
        {
            for (int col = 0; col < universe.First().Count; col++)
            {
                if (universe[row][col] == '#')
                    galaxies.Add(new Galaxy(row, col));
            }
        }

        var galaxyPairs = new List<Tuple<Galaxy, Galaxy>>();

        var unpairedGalaxies = galaxies.ToList();
        foreach (var galaxy in galaxies)
        {
            unpairedGalaxies = unpairedGalaxies.Skip(1).ToList();
            galaxyPairs.AddRange(unpairedGalaxies.Select(g => new Tuple<Galaxy,Galaxy>(galaxy, g)).ToList());
        }

        return galaxyPairs;
    }
    
    public class Universe
    {
        public List<int> EmptyRows { get; set; } = new();
        public List<int> EmptyColumns { get; set; } = new();
        public List<Tuple<Galaxy, Galaxy>> GalaxyPairs { get; set; } = new();
    }

    public class Galaxy(int row, int col)
    {
        public int Row { get; set; } = row;
        public int Col { get; set; } = col;

        public override string ToString()
        {
            return $"({Row}, {Col})";
        }
    }
}