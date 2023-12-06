namespace Puzzles;

public class Day02(string input) : DailyPuzzleBase(input)
{
    public override string GetPuzzleOneSolution()
    {
        //only 12 red cubes, 13 green cubes, and 14 blue cubes
        Dictionary<string, int> cubeCounts = new() { { "red", 12 }, { "green", 13 }, { "blue", 14 } };

        return GetGames()
            .Where(g => !g.Sets.Any(s => s.Any(cc => cubeCounts[cc.Key] < cc.Value)))
            .Sum(g => g.Id).ToString();
    }

    public override string GetPuzzleTwoSolution()
    {
        return GetGames()
            .Sum(g => g.Sets
                .SelectMany(s => s)
                .GroupBy(cc => cc.Key, cc => cc.Value)
                .Select(gg => gg.Max())
                .Aggregate(1, (c1, c2) => c1 * c2)).ToString();
    }

    private List<Game> GetGames()
    {
        var games = new List<Game>();

        foreach (var line in Input.Split(Environment.NewLine))
        {
            var game = new Game();
            game.Id = int.Parse(line.Split(':').First().Split(' ').Last().Trim());
            game.Sets = line.Split(':').Last()
                .Split(';')
                .Select(set => set
                    .Split(',')
                    .ToDictionary(
                        cc => cc.Trim().Split(' ').Last().Trim(),
                        cc => int.Parse(cc.Trim().Split(' ').First().Trim())))
                .ToList();

            games.Add(game);
        }

        return games;
    }

    private class Game
    {
        public int Id { get; set; }

        public List<Dictionary<string, int>> Sets { get; set; }
    }
}
