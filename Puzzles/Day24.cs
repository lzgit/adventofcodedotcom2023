using Microsoft.Z3;

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
        var hails = GetHailstones();

        var ctx = new Context();
        var solver = ctx.MkSolver();

        var x = ctx.MkIntConst("x");
        var y = ctx.MkIntConst("y");
        var z = ctx.MkIntConst("z");

        var vx = ctx.MkIntConst("vx");
        var vy = ctx.MkIntConst("vy");
        var vz = ctx.MkIntConst("vz");

        for (var i = 0; i < 3; i++)
        {
            var t = ctx.MkIntConst($"t{i}");
            var hail = hails[i];

            var px = ctx.MkInt(Convert.ToInt64(hail.Position.X));
            var py = ctx.MkInt(Convert.ToInt64(hail.Position.Y));
            var pz = ctx.MkInt(Convert.ToInt64(hail.Position.Z));

            var pvx = ctx.MkInt(Convert.ToInt64(hail.Velocity.X));
            var pvy = ctx.MkInt(Convert.ToInt64(hail.Velocity.Y));
            var pvz = ctx.MkInt(Convert.ToInt64(hail.Velocity.Z));

            solver.Add(t >= 0);
            solver.Add(ctx.MkEq(ctx.MkAdd(x, ctx.MkMul(t, vx)), ctx.MkAdd(px, ctx.MkMul(t, pvx)))); // x + t * vx = px + t * pvx
            solver.Add(ctx.MkEq(ctx.MkAdd(y, ctx.MkMul(t, vy)), ctx.MkAdd(py, ctx.MkMul(t, pvy)))); // y + t * vy = py + t * pvy
            solver.Add(ctx.MkEq(ctx.MkAdd(z, ctx.MkMul(t, vz)), ctx.MkAdd(pz, ctx.MkMul(t, pvz)))); // z + t * vz = pz + t * pvz
        }

        solver.Check();
        var model = solver.Model;

        var rx = model.Eval(x);
        var ry = model.Eval(y);
        var rz = model.Eval(z);

        return (Convert.ToInt64(rx.ToString()) + Convert.ToInt64(ry.ToString()) + Convert.ToInt64(rz.ToString())).ToString();
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