using System.Reflection;

namespace Puzzles
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var dayNumber = 15;
            //var dayNumber = DateTime.Now.Day;
            //if (args.Length > 0)
            //    dayNumber = int.Parse(args[0]);

            var currentDay = $"Day{dayNumber.ToString().PadLeft(2, '0')}";
            var dayPuzzleInput = File.ReadAllText($"PuzzleInputs\\{currentDay}.txt");
            var dayPuzzleSolution = Assembly.GetExecutingAssembly().GetType($"Puzzles.{currentDay}")!;
            var currentDaySolution = (DailyPuzzleBase)Activator.CreateInstance(dayPuzzleSolution, dayPuzzleInput)!;

            Console.WriteLine($"PuzzleOneSolution: {currentDaySolution.GetPuzzleOneSolution()}");
            Console.WriteLine($"PuzzleTwoSolution: {currentDaySolution.GetPuzzleTwoSolution()}");

            Console.ReadKey();
        }
    }
}
