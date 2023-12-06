namespace Puzzles;

public class Day06(string input) : DailyPuzzleBase(input)
{
    public override string GetPuzzleOneSolution()
    {
        return GetRaces().Select(GetRaceWinningCombinations).Aggregate(1, (r1, r2) => r1 * r2).ToString();
    }

    private int GetRaceWinningCombinations(Race r)
    {
        var winnerCombinations = 0;
        for (int speed = 0; speed < r.Time; speed++)
        {
            var distance = (r.Time - speed) * speed;
            if (distance > r.Distance)
                winnerCombinations++;
        }

        return winnerCombinations;
    }

    public override string GetPuzzleTwoSolution()
    {
        return GetRaceWinningCombinations(GetRace()).ToString();
    }

    private List<Race> GetRaces()
    {
        var lines = Input.Split(Environment.NewLine);
        var times = lines.First()
            .Split(':').Last().Trim()
            .Split(' ').Where(t => !string.IsNullOrEmpty(t))
            .Select(int.Parse).ToList();

        var distances = lines.Last()
            .Split(':').Last().Trim()
            .Split(' ').Where(t => !string.IsNullOrEmpty(t))
            .Select(int.Parse).ToList();

        var races = new List<Race>();
        for (int i = 0; i < times.Count; i++)
            races.Add(new Race { Time = times[i], Distance = distances[i] });

        return races;
    }

    private Race GetRace()
    {
        var lines = Input.Split(Environment.NewLine);
        var time = long.Parse(string.Join(string.Empty, lines.First()
            .Split(':').Last().Trim()
            .Split(' ').Where(t => !string.IsNullOrEmpty(t))));


        var distance = long.Parse(string.Join(string.Empty, lines.Last()
            .Split(':').Last().Trim()
            .Split(' ').Where(t => !string.IsNullOrEmpty(t))));

        return new Race { Distance = distance, Time = time };
    }

    private class Race
    {
        public long Time { get; set; }
        public long Distance { get; set; }
    }
}