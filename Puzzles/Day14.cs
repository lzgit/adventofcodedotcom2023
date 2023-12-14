namespace Puzzles;

public class Day14(string input) : DailyPuzzleBase(input)
{
    private readonly char roundRock = 'O';
    private readonly char emptySlot = '.';

    public override string GetPuzzleOneSolution()
    {
        return GetDishColumns().Sum(c => GetDishColumnWeight(TiltList(c))).ToString();
    }

    public override string GetPuzzleTwoSolution()
    {
        var current = GetDishColumns();
        long currentWeight = 0;
        var resultCache = new List<KeyValuePair<string, long>>();

        var totalCycles = 1000000000;
        for (int i = 0; i < totalCycles; i++)
        {
            var key = ToKey(current);

            var resultIndex = resultCache.FindIndex(c => c.Key == key);
            if (resultIndex >= 0)
            {
                var finalResultIndex = (totalCycles - resultIndex) % (resultCache.Count - resultIndex) + resultIndex - 1;
                return resultCache[finalResultIndex].Value.ToString();
            }

            //Tilt Up
            for (int col = 0; col < current.Count; col++)
            {
                var rockList = GetColumnRockList(current, col);
                var tiltList = TiltList(rockList);
                UpdateColumnRockList(current, col, tiltList);
            }

            //Tilt Left
            for (int row = 0; row < current[0].Count; row++)
            {
                var rockList = GetRowRockList(current, row);
                var tiltList = TiltList(rockList);
                UpdateRowRockList(current, row, tiltList);
            }

            //Tilt Down
            for (int col = 0; col < current.Count; col++)
            {
                var rockList = GetColumnRockList(current, col);
                rockList.Reverse();

                var tiltList = TiltList(rockList);
                tiltList.Reverse();

                UpdateColumnRockList(current, col, tiltList);
            }

            //Tilt Right
            for (int row = 0; row < current[0].Count; row++)
            {
                var rockList = GetRowRockList(current, row);
                rockList.Reverse();

                var tiltList = TiltList(rockList);
                tiltList.Reverse();

                UpdateRowRockList(current, row, tiltList);
            }

            currentWeight = current.Sum(GetDishColumnWeight);
            resultCache.Add(new KeyValuePair<string, long>(key, currentWeight));
        }

        return currentWeight.ToString();
    }

    private static void UpdateColumnRockList(List<List<char>> current, int col, List<char> tiltList)
    {
        for (int row = 0; row < current[0].Count; row++)
            current[col][row] = tiltList[row];
    }
    
    private static void UpdateRowRockList(List<List<char>> current, int row, List<char> tiltList)
    {
        for (int col = 0; col < current[0].Count; col++)
            current[col][row] = tiltList[col];
    }

    private static List<char> GetColumnRockList(List<List<char>> current, int col)
    {
        var rockList = new List<char>();
        for (int row = 0; row < current.Count; row++)
            rockList.Add(current[col][row]);
        return rockList;
    }
    
    private static List<char> GetRowRockList(List<List<char>> current, int row)
    {
        var rockList = new List<char>();
        for (int col = 0; col < current.Count; col++)
            rockList.Add(current[col][row]);
        return rockList;
    }

    private string ToKey(List<List<char>> dishColumns) => string.Join("", dishColumns.SelectMany(c => c));

    private List<char> TiltList(List<char> rockList)
    {
        var rockSearchStartIndex = 0;

        var indexOfNextRockToTilt = rockList.IndexOf(roundRock, rockSearchStartIndex);
        while (indexOfNextRockToTilt > -1)
        {
            for (int i = indexOfNextRockToTilt; i >= 0; i--)
            {
                if (i - 1 < 0 || rockList[i - 1] != emptySlot)
                {
                    rockList[indexOfNextRockToTilt] = emptySlot;
                    rockList[i] = roundRock;

                    rockSearchStartIndex = i + 1;
                    break;
                }
            }

            if (rockSearchStartIndex >= rockList.Count)
                indexOfNextRockToTilt = -1;
            else
                indexOfNextRockToTilt = rockList.IndexOf(roundRock, rockSearchStartIndex);
        }

        return rockList;
    }

    private long GetDishColumnWeight(List<char> dishColumn)
    {
        long weight = 0;
        for (int i = 0; i < dishColumn.Count; i++)
            if (dishColumn[i] == roundRock)
                weight+= dishColumn.Count - i;

        return weight;
    }

    private List<List<char>> GetDishColumns()
    {
        var dishColumns = new List<List<char>>();
        var lines = Input.Split(Environment.NewLine);

        for (int col = 0; col < lines[0].Length; col++)
        {
            dishColumns.Add(new List<char>());
            for (var row = 0; row < lines.Length; row++)
                dishColumns[col].Add(lines[row][col]);
        }

        return dishColumns;
    }
}