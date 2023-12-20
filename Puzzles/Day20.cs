namespace Puzzles;

public class Day20(string input) : DailyPuzzleBase(input)
{
    public override string GetPuzzleOneSolution()
    {
        var moduleRegistry = GetModuleRegistry();

        long totalLowIntensityPulseCount = 0;
        long totalHighIntensityPulseCount = 0;
        for (int i = 0; i < 1000; i++)
        {
            var buttonPressPulseResults = EvaluateButtonPress(moduleRegistry).ToList();
            totalLowIntensityPulseCount += buttonPressPulseResults.Count(p => p.Intensity == Intensity.Low);
            totalHighIntensityPulseCount += buttonPressPulseResults.Count(p => p.Intensity == Intensity.High);
        }

        return (totalLowIntensityPulseCount * totalHighIntensityPulseCount).ToString();
    }

    public override string GetPuzzleTwoSolution()
    {
        return GetModuleRegistry()
            .Values
            .Single(m => m.OutputModuleNames.Contains("rx"))
            .InputModuleNames
            .Select(GetLoopLength)
            .Aggregate(1L, (l1, l2) => l1 * l2)
            .ToString();
    }

    private int GetLoopLength(string output)
    {
        var moduleRegistry = GetModuleRegistry();

        var length = 1;
        while (true)
        {
            if (EvaluateButtonPress(moduleRegistry).Any(p => p.Target?.Name == output && p.Intensity == Intensity.Low))
                return length;

            length++;
        }
    }

    private List<Pulse> EvaluateButtonPress(Dictionary<string, Module> state)
    {
        var resultPulses = new List<Pulse>();

        var pulseQueue = new Queue<Pulse>();
        pulseQueue.Enqueue(new Pulse(null, state.Values.Single(m => !m.InputModuleNames.Any()), Intensity.Low));
        while (pulseQueue.TryDequeue(out var pulse))
        {
            resultPulses.Add(pulse);
            pulse.Target?.Process(state, pulse).ForEach(pulseQueue.Enqueue);
        }

        return resultPulses;
    }

    private Dictionary<string, Module> GetModuleRegistry()
    {
        var moduleRegistry = Input
            .Split(Environment.NewLine)
            .Select(l =>
            {
                var moduleParts = l.Split("->");

                string type;
                string name;
                if (moduleParts.First().First() == 'b')
                {
                    name = type = moduleParts.First().Trim();
                }
                else
                {
                    name = moduleParts.First().Substring(1, moduleParts.First().Length - 1).Trim();
                    type = moduleParts.First().First().ToString();
                }
                var outputModuleNames = moduleParts.Last().Split(",").Select(m => m.Trim()).ToList();

                return new Module(type, name, outputModuleNames);
            })
            .ToDictionary(m => m.Name, m => m);

        foreach (var module in moduleRegistry.Values)
            module.InputModuleNames = moduleRegistry.Values.Where(mm => mm.OutputModuleNames.Contains(module.Name)).Select(mm => mm.Name).ToList();

        return moduleRegistry;
    }

    public enum Intensity { Low, High }

    public class Pulse(Module source, Module target, Intensity intensity)
    {
        public Module Source { get; set; } = source;
        public Module? Target { get; set; } = target;
        public Intensity Intensity { get; set; } = intensity;

        public override string ToString() => $"{Source?.Name ?? "button"} -{Intensity}-> {Target?.Name}";
    }

    public class Module(string type, string name, List<string> outputModuleNames)
    {
        public const string Broadcaster = "broadcaster";
        public const string FlipFlop = "%";
        public const string Conjunction = "&";

        public string Type { get; set; } = type;
        public string Name { get; set; } = name;

        public List<string> OutputModuleNames { get; set; } = outputModuleNames;
        public List<string> InputModuleNames { get; set; } = new();

        public Dictionary<string, Intensity> PulseIntensityMemory = new();
        public bool IsFlipFlopOn;
        public List<Pulse> Process(Dictionary<string, Module> modules, Pulse pulse)
        {
            var outputModules = OutputModuleNames.Select(modules.GetValueOrDefault);

            if (Type == Broadcaster)
                return outputModules.Select(m => new Pulse(this, m, pulse.Intensity)).ToList();

            if (Type == FlipFlop)
            {
                if (pulse.Intensity == Intensity.High)
                    return [];

                IsFlipFlopOn = !IsFlipFlopOn;
                return outputModules.Select(m => new Pulse(this, m, IsFlipFlopOn ? Intensity.High : Intensity.Low)).ToList();
            }

            if (Type == Conjunction)
            {
                if (!PulseIntensityMemory.Any())
                    InputModuleNames.ForEach(m => PulseIntensityMemory[m] = Intensity.Low);

                PulseIntensityMemory[pulse.Source.Name] = pulse.Intensity;

                return outputModules.Select(m => new Pulse(this, m, PulseIntensityMemory.Values.All(i => i == Intensity.High) ? Intensity.Low : Intensity.High)).ToList();
            }

            throw new Exception("!");
        }

        public override string ToString() => $"[{string.Join(", ", InputModuleNames.Select(m => m))}] -> {(Type != Broadcaster ? Type : "")}{Name} -> [{string.Join(", ", OutputModuleNames)}]";
    }
}