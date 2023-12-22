namespace Puzzles;

public class Day22(string input) : DailyPuzzleBase(input)
{
    public override string GetPuzzleOneSolution()
    {
        var settledBricks = GetSettledBricks(GetBricks());

        var count = 0;
        for (var index = 0; index < settledBricks.Count; index++)
        {
            var settledBrick = settledBricks[index];
            var bricksMinusOne = settledBricks.Except(new List<Brick> { settledBrick }).ToList();

            //if (IsStable(bricksMinusOne))
            //    count++;

            if (GetFallingBrickCount(bricksMinusOne) == 0)
                count++;
        }
        
        return count.ToString();
    }

    public override string GetPuzzleTwoSolution()
    {
        var settledBricks = GetSettledBricks(GetBricks());

        var count = 0L;
        for (var index = 0; index < settledBricks.Count; index++)
        {
            var settledBrick = settledBricks[index];
            var bricksMinusOne = settledBricks.Except(new List<Brick> { settledBrick }).ToList();

            count += GetFallingBrickCount(bricksMinusOne);
        }

        return count.ToString();
    }

    private bool IsStable(List<Brick> bricks)
    {
        var settledBricks = new List<Brick>();
        foreach (var brick in bricks)
        {
            var fallHeight = GetFallHeight(settledBricks, brick);
            if (fallHeight > 0)
                return false;

            settledBricks.Add(brick.Fall(fallHeight));
        }

        return true;
    }

    private long GetFallingBrickCount(List<Brick> bricks)
    {
        long fallingBrickCount = 0;
        var settledBricks = new List<Brick>();
        foreach (var brick in bricks)
        {
            var fallHeight = GetFallHeight(settledBricks, brick);
            if (fallHeight > 0)
                fallingBrickCount++;

            settledBricks.Add(brick.Fall(fallHeight));
        }

        return fallingBrickCount;
    }
    
    private static int GetFallHeight(List<Brick> settledBricks, Brick brick)
    {
        var maxPeakUnderBrick = 0;
        for (int x = brick.BottomStart.X; x <= brick.TopEnd.X; x++)
        {
            for (int y = brick.BottomStart.Y; y <= brick.TopEnd.Y; y++)
            {
                var bricksUnderPoint = settledBricks.Where(b => b.ContainsPointVertically(x, y)).ToList();
                if (bricksUnderPoint.Any()) 
                    maxPeakUnderBrick = Math.Max(maxPeakUnderBrick, bricksUnderPoint.Max(b => b.TopEnd.Z));
            }
        }

        return brick.BottomStart.Z - maxPeakUnderBrick - 1;
    }

    private static List<Brick> GetSettledBricks(List<Brick> bricks)
    {
        var settledBricks = new List<Brick>();
        foreach (var brick in bricks) 
            settledBricks.Add(brick.Fall(GetFallHeight(settledBricks, brick)));

        return settledBricks;
    }

    private List<Brick> GetBricks()
    {
        return Input
            .Split(Environment.NewLine)
            .Select(l =>
            {
                var coordinates = l.Split('~');
                var start = coordinates.First().Split(',');
                var end = coordinates.Last().Split(',');
                return new Brick(
                    new C(int.Parse(start[0].Trim()), int.Parse(start[1].Trim()), int.Parse(start[2].Trim())),
                    new C(int.Parse(end[0].Trim()), int.Parse(end[1].Trim()), int.Parse(end[2].Trim())));
            })
            .OrderBy(b => b.BottomStart.Z)
            .ToList();
    }

    public record Brick(C BottomStart, C TopEnd)
    {
        public bool ContainsPointVertically(int x, int y) => BottomStart.X <= x && x <= TopEnd.X && BottomStart.Y <= y && y <= TopEnd.Y;
        public Brick Fall(int height) => new(BottomStart with { Z = BottomStart.Z - height }, TopEnd with { Z = TopEnd.Z - height });
    }

    public record C(int X, int Y, int Z);
}