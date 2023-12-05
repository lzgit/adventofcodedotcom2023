using Puzzles;

namespace Tests;

public class DayTestBase<TDay> where TDay : DailyPuzzleBase
{
    public TDay GetPuzzle(string input)
    {
        return (TDay)Activator.CreateInstance(typeof(TDay), input)!;
    }
}