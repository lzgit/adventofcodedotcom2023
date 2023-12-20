using Puzzles;

namespace Tests;

[TestFixture]
public class Day20Tests : DayTestBase<Day20>
{
    [TestCase(@"broadcaster -> a, b, c
%a -> b
%b -> c
%c -> inv
&inv -> a", "32000000")]
    public void PuzzleOneTestOne(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleOneSolution(), Is.EqualTo(output));
    
    [TestCase(@"broadcaster -> a
%a -> inv, con
&inv -> b
%b -> con
&con -> output", "11687500")]
    public void PuzzleOneTestTwo(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleOneSolution(), Is.EqualTo(output));
    
    [TestCase(@"%nr -> hq
%xk -> sn
%cl -> xk
%mj -> dq, qr
%gm -> lk, cl
%mv -> lz, rr
&qr -> cz, sp, lb, xt, fx
%xt -> vl
&dd -> rx
%rv -> qr
%ts -> lz, nk
%vl -> zj, qr
%qm -> db, lk
%sn -> lp
%xc -> lz
%jn -> sz, ft
%vg -> lk, ks
%hq -> ft, lh
&lz -> gx, xn, jq, fb, ts, rr
%nk -> mv, lz
&nx -> dd
&sp -> dd
%jj -> qr, mj
%sz -> nr, ft
%rn -> qm
%cz -> xt, qr
%fr -> ft
%vb -> lz, xn
broadcaster -> cz, gm, jn, ts
%fb -> vb
%hd -> lz, xc
%gx -> fb
%db -> mh, lk
&ft -> jx, nx, lh, pc, nr, jn, kr
%qc -> pl, ft
%fx -> bz
%jx -> kr
%pl -> ft, fr
%lh -> jx
%rr -> gx
&cc -> dd
%xn -> xl
%kr -> pc
%xl -> dv, lz
%dq -> qr, rv
%mh -> lk, vg
%sb -> lk, rn
%bz -> lb, qr
%ks -> lk
%qh -> ft, qc
%pc -> qh
%lb -> mb
%dv -> lz, hd
%mb -> qr, jj
%zj -> fx, qr
%lp -> sb
&jq -> dd
&lk -> sn, cc, xk, rn, gm, cl, lp", "241823802412393")]
    public void PuzzleTwoTest(string input, string output) => Assert.That(GetPuzzle(input).GetPuzzleTwoSolution(), Is.EqualTo(output));
}