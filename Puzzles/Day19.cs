namespace Puzzles;

public class Day19(string input) : DailyPuzzleBase(input)
{
    public override string GetPuzzleOneSolution()
    {
        var parts = GetParts();
        var workflows = GetWorkflows();

        var finalWorkflowNamesWithPartWeights = parts
            .Select(p => new
            {
                FinalWorkflowName = GetFinalWorkflowName(workflows, workflows.Single(wf => wf.Name == "in"), p),
                Weight = p.Attributes.Sum(a => a.Value)
            })
            .ToList();

        return finalWorkflowNamesWithPartWeights.Where(w => w.FinalWorkflowName == "A").Sum(w => w.Weight).ToString();
    }

    public override string GetPuzzleTwoSolution()
    {
        var workflows = GetWorkflows();

        var initialRanges = new Dictionary<char, Range>
        {
            { 'x', new Range(1,4000 )},
            { 'm', new Range(1,4000 ) },
            { 'a', new Range(1,4000 ) },
            { 's', new Range(1,4000 ) },
        };

        return GetAcceptedAttributeRanges(workflows, "in", initialRanges)
            .Sum(ranges => ranges.Values.Select(r => r.Length).Aggregate((long)1, (r1, r2) => r1 * r2))
            .ToString();
    }

    public List<Dictionary<char, Range>> GetAcceptedAttributeRanges(List<Workflow> workflows, string currentWorkflowName, Dictionary<char, Range> initialRanges)
    {
        var currentWorkflow = workflows.SingleOrDefault(wf => wf.Name == currentWorkflowName);
        if (currentWorkflow == null)
        {
            if (currentWorkflowName == "A")
                return [initialRanges];

            return [];
        }

        var currentRanges = initialRanges;
        
        var nextSteps = new List<(string, Dictionary<char, Range>)>();
        foreach (var r in currentWorkflow.Rules)
        {
            if (r.IsDefaultRule)
            {
                nextSteps.Add((r.TargetWorkflowName, currentRanges));
            }
            else
            {
                nextSteps.Add((r.TargetWorkflowName, Restrict(currentRanges, r)));
                currentRanges = Restrict(currentRanges, r, true);
            }
        }

        return nextSteps
            .Where(step => step.Item2.All(ar => ar.Value.IsValid))
            .SelectMany(s => GetAcceptedAttributeRanges(workflows, s.Item1, s.Item2))
            .ToList();
    }

    public Dictionary<char, Range> Restrict(Dictionary<char, Range> ranges, Rule rule, bool useInverseRule = false)
    {
        var restricted = Clone(ranges);

        var attributeRange = restricted[rule.TargetAttribute];

        if (useInverseRule)
        {
            if (rule.IsLowerLimit)
                attributeRange.Max = Math.Min(attributeRange.Max, rule.Limit);
            else
                attributeRange.Min = Math.Max(attributeRange.Min, rule.Limit);
        }
        else
        {
            if (rule.IsLowerLimit)
                attributeRange.Min = Math.Max(attributeRange.Min, rule.Limit + 1);
            else
                attributeRange.Max = Math.Min(attributeRange.Max, rule.Limit - 1);
        }

        return restricted;
    }

    public string GetFinalWorkflowName(List<Workflow> workflows, Workflow currentWorkflow, Part part)
    {
        var nextWorkflowName = currentWorkflow.GetNextWorkflowName(part);
        var nextWorkflow = workflows.SingleOrDefault(wf => wf.Name == nextWorkflowName);

        return nextWorkflow == null ? nextWorkflowName : GetFinalWorkflowName(workflows, nextWorkflow, part);
    }

    private List<Workflow> GetWorkflows()
    {
        var lines = Input.Split(Environment.NewLine).ToList();
        var indexOfEmptyLine = lines.FindIndex(l => string.IsNullOrEmpty(l) || string.IsNullOrWhiteSpace(l));

        return lines
            .Take(indexOfEmptyLine)
            .Select(ws =>
            {
                var wsParts = ws.Split('{');
                var name = wsParts.First().Trim();
                var ruleStrings = wsParts.Last().Trim('}').Split(',').Select(rs => rs.Trim());

                var rules = ruleStrings.Select(rs =>
                {
                    var rule = new Rule();
                    if (!rs.Contains(':'))
                    {
                        rule.IsDefaultRule = true;
                        rule.TargetWorkflowName = rs;
                    }
                    else
                    {
                        var ruleParts = rs.Split(':');
                        rule.TargetWorkflowName = ruleParts.Last().Trim();
                        rule.IsDefaultRule = false;

                        if (ruleParts.First().Contains('>'))
                        {
                            var conditionParts = ruleParts.First().Split('>');
                            rule.IsLowerLimit = true;
                            rule.TargetAttribute = conditionParts.First().Trim().Single();
                            rule.Limit = int.Parse(conditionParts.Last().Trim());
                        }
                        else
                        {
                            var conditionParts = ruleParts.First().Split('<');
                            rule.IsLowerLimit = false;
                            rule.TargetAttribute = conditionParts.First().Trim().Single();
                            rule.Limit = int.Parse(conditionParts.Last().Trim());
                        }
                    }

                    return rule;
                }).ToList();

                return new Workflow(name, rules);
            })
            .ToList();
    }

    private List<Part> GetParts()
    {
        var lines = Input.Split(Environment.NewLine).ToList();
        var indexOfEmptyLine = lines.FindIndex(l => string.IsNullOrEmpty(l) || string.IsNullOrWhiteSpace(l));

        return lines
            .Skip(indexOfEmptyLine + 1)
            .Select(ps => new Part(ps.Trim('{').Trim('}').Split(',')
                .Select(a => a.Trim().Split('='))
                .ToDictionary(a => a.First().Trim().Single(), a => int.Parse(a.Last().Trim())))
            )
            .ToList();
    }

    public class Part(Dictionary<char, int> attributes)
    {
        public Dictionary<char, int> Attributes { get; set; } = attributes;
    }

    public class Workflow(string name, List<Rule> rules)
    {
        public string Name { get; set; } = name;
        public List<Rule> Rules { get; set; } = rules;

        public string GetNextWorkflowName(Part p) => Rules.First(r => r.Evaluate(p)).TargetWorkflowName;
    }

    public class Rule
    {
        public bool IsDefaultRule { get; set; }
        public string TargetWorkflowName { get; set; }

        public char TargetAttribute { get; set; }
        public int Limit { get; set; }

        public bool IsLowerLimit { get; set; }

        public bool Evaluate(Part p)
        {
            if (IsDefaultRule)
                return true;

            if (IsLowerLimit)
                return p.Attributes[TargetAttribute] > Limit;

            return p.Attributes[TargetAttribute] < Limit;
        }
    }

    private Dictionary<char, Range> Clone(Dictionary<char, Range> ranges) => ranges.ToDictionary(kv => kv.Key, kv => new Range(kv.Value.Min, kv.Value.Max));

    public class Range(int min, int max)
    {
        public int Min { get; set; } = min;
        public int Max { get; set; } = max;

        public long Length => Max - Min + 1;
        public bool IsValid => Min <= Max;
    }
}