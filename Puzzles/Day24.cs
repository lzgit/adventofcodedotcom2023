namespace Puzzles;

public class Day24(string input) : DailyPuzzleBase(input)
{
    public override string GetPuzzleOneSolution()
    {
        decimal testAreaMin = 7;
        decimal testAreaMax = 27;

        var hailStones = GetHailstones();
        if (hailStones.Count > 10)
        {
            testAreaMin = 200000000000000;
            testAreaMax = 400000000000000;
        }

        int intersectionCount = 0;
        for (var i = 0; i < hailStones.Count - 1; i++)
        {
            var hailStoneA = hailStones[i];
            for (int j = i + 1; j < hailStones.Count; j++)
            {
                var hailStoneB = hailStones[j];

                if (!IsParallel(hailStoneA, hailStoneB))
                {
                    var intersection = GetIntersection(hailStoneA, hailStoneB);
                    if (IsInTestArea(intersection, testAreaMin, testAreaMax) && IsInFuture(intersection, hailStoneA, hailStoneB))
                        intersectionCount++;
                }
            }
        }

        return intersectionCount.ToString();
    }

    public override string GetPuzzleTwoSolution()
    {
        return "";
    }

    public List<HailStone> GetHailstones()
    {
        return Input.Split(Environment.NewLine)
            .Select(l =>
            {
                var hailStoneParts = l.Split("@");
                var p = hailStoneParts.First().Trim().Split(",").Select(n => long.Parse(n.Trim())).ToList();
                var v = hailStoneParts.Last().Trim().Split(",").Select(n => long.Parse(n.Trim())).ToList();

                return new HailStone(new C(p[0], p[1], p[2]), new C(v[0], v[1], v[2]));
            })
            .ToList();
    }

    public C GetIntersection(HailStone a, HailStone b)
    {
        var y = decimal.Divide(a.C * a.A.Y - a.A.X - b.C * b.A.Y + b.A.X, a.C - b.C);
        var x = a.C * y - a.C * a.A.Y + a.A.X;
        return new C(x, y, 0m);
    }

    public bool IsParallel(HailStone a, HailStone b) => a.C - b.C == 0;

    public bool IsInTestArea(C p, decimal min, decimal max) =>
        min <= p.X && p.X <= max &&
        min <= p.Y && p.Y <= max;

    public bool IsInFuture(C p, HailStone a, HailStone b) =>
        Math.Sign(a.Velocity.X) * a.Position.X <= Math.Sign(a.Velocity.X) * p.X &&
        Math.Sign(b.Velocity.X) * b.Position.X <= Math.Sign(b.Velocity.X) * p.X &&
        Math.Sign(a.Velocity.Y) * a.Position.Y <= Math.Sign(a.Velocity.Y) * p.Y &&
        Math.Sign(b.Velocity.Y) * b.Position.Y <= Math.Sign(b.Velocity.Y) * p.Y;

    public record HailStone(C Position, C Velocity)
    {
        public C A => Position;
        public C B => Position.Add(Velocity);
        public decimal C => decimal.Divide(B.X - A.X, B.Y - A.Y);
    }

    public record C(decimal X, decimal Y, decimal Z)
    {
        public C Add(C c) => new(X + c.X, Y + c.Y, Z + c.Z);
    }
}