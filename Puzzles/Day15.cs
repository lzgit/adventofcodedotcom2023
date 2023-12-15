namespace Puzzles;

public class Day15(string input) : DailyPuzzleBase(input)
{
    public override string GetPuzzleOneSolution()
    {
        return GetSteps().Sum(GetHash).ToString();
    }

    public override string GetPuzzleTwoSolution()
    {
        var boxes = Enumerable.Repeat(0, 256).Select(_ => new List<Lens>()).ToList();

        var steps = GetSteps();
        foreach (var step in steps)
        {
            string label;
            Lens lens = null;

            if (step.Contains("="))
            {
                var splitParts = step.Split('=');
                label = splitParts.First().Trim();
                lens = new Lens(label, int.Parse(splitParts.Last().Trim()));
            }
            else
                label = step.Split('-').First().Trim();

            var box = boxes[GetHash(label)];
            var indexOfLabel = box.FindIndex(l => string.Equals(l.Label, label, StringComparison.InvariantCultureIgnoreCase));

            if (lens != null)
            {
                if (indexOfLabel < 0)
                    box.Add(lens);
                else
                    box[indexOfLabel] = lens;
            }
            else
            {
                if (indexOfLabel >= 0)
                    box.RemoveAt(indexOfLabel);
            }
        }

        long sum = 0;
        for (int boxIndex = 0; boxIndex < boxes.Count; boxIndex++)
            for (int lensIndex = 0; lensIndex < boxes[boxIndex].Count; lensIndex++)
                sum += (boxIndex + 1) * (lensIndex + 1) * boxes[boxIndex][lensIndex].FocalLength;

        return sum.ToString();
    }

    public class Lens(string label, int focalLength)
    {
        public string Label { get; set; } = label;
        public int FocalLength { get; set; } = focalLength;
    }

    private int GetHash(string str)
    {
        int currentValue = 0;
        foreach (var ch in str)
        {
            currentValue += ch;
            currentValue *= 17;
            currentValue %= 256;
        }

        return currentValue;
    }

    private List<string> GetSteps() => input.Split(",").Select(w => w.Trim()).ToList();
}