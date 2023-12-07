namespace Puzzles;

public class Day07(string input) : DailyPuzzleBase(input)
{
    public override string GetPuzzleOneSolution()
    {
        return GetBids()
            .OrderBy(b => b.Hand.StrengthOne)
            .ThenByDescending(b => b.Hand.WeightOne)
            .Select((bid, i) => bid.BidValue * (i + 1))
            .Sum()
            .ToString();
    }

    public override string GetPuzzleTwoSolution()
    {
        return GetBids()
            .OrderBy(b => b.Hand.StrengthTwo)
            .ThenByDescending(b => b.Hand.WeightTwo)
            .Select((bid, i) => bid.BidValue * (i + 1))
            .Sum()
            .ToString();
    }

    private List<Bid> GetBids()
    {
        return Input.Split(Environment.NewLine).Select(l =>
        {
            var split = l.Split(' ');
            return new Bid(
                new Hand(split.First(s => !string.IsNullOrEmpty(s)).Trim().Select(c => new Card(c))),
                int.Parse(split.Last(s => !string.IsNullOrEmpty(s)).Trim())
            );
        }).ToList();
    }

    private class Bid(Hand hand, int bidValue)
    {
        public Hand Hand { get; } = hand;
        public int BidValue { get; } = bidValue;
    }

    private class Hand(IEnumerable<Card> cards)
    {
        private List<Card> Cards { get; } = cards.ToList();

        private int? strengthOne;
        public int StrengthOne
        {
            get
            {
                if (strengthOne.HasValue)
                    return strengthOne.Value;

                var cardCounts = Cards.GroupBy(c => c.Label).ToDictionary(g => g.Key, g => g.Count());
                strengthOne = GetStrength(cardCounts);

                return strengthOne.Value;
            }
        }

        private int? strengthTwo;
        public int StrengthTwo
        {
            get
            {
                if (strengthTwo.HasValue)
                    return strengthTwo.Value;

                var cardCounts = Cards.GroupBy(c => c.Label).ToDictionary(g => g.Key, g => g.Count());
                if (cardCounts.TryGetValue('J', out int jokerCount))
                {
                    if (cardCounts.Count == 1)
                    {
                        strengthTwo = 7;
                    }
                    else
                    {
                        cardCounts.Remove('J');
                        cardCounts[cardCounts.MaxBy(cc => cc.Value).Key] += jokerCount;
                        strengthTwo = GetStrength(cardCounts);
                    }
                }
                else
                {
                    return StrengthOne;
                }

                return strengthTwo.Value;
            }
        }

        private static int GetStrength(Dictionary<char, int> cardCounts)
        {
            int strength;
            if (cardCounts.Count == 1)
                strength = 7;
            else if (cardCounts.Count == 2 && cardCounts.Values.Any(cc => cc == 4))
                strength = 6;
            else if (cardCounts.Count == 2 && cardCounts.Values.Any(cc => cc == 3))
                strength = 5;
            else if (cardCounts.Count == 3 && cardCounts.Values.Any(cc => cc == 3))
                strength = 4;
            else if (cardCounts.Count(cc => cc.Value == 2) == 2)
                strength = 3;
            else if (cardCounts.Count == 4)
                strength = 2;
            else if (cardCounts.Count == 5)
                strength = 1;
            else
                throw new Exception("!");

            return strength;
        }

        public string WeightOne => string.Join("", Cards.Select(c => c.WeightOne).ToList());
        public string WeightTwo => string.Join("", Cards.Select(c => c.WeightTwo).ToList());
    }

    private class Card(char label)
    {
        private static readonly Dictionary<char, char> LabelWeightPairsOne = new() { {'A', 'A'}, {'K', 'B'}, {'Q', 'C'}, {'J', 'D'}, {'T', 'E'}, {'9', 'F'}, {'8', 'G'}, {'7', 'H'}, {'6', 'I'}, {'5', 'J'}, {'4', 'K'}, {'3', 'L'}, {'2', 'M'} };
        private static readonly Dictionary<char, char> LabelWeightPairsTwo = new() { {'A', 'A'}, {'K', 'B'}, {'Q', 'C'}, {'T', 'D'}, {'9', 'E'}, {'8', 'F'}, {'7', 'G'}, {'6', 'H'}, {'5', 'I'}, {'4', 'J'}, {'3', 'K'}, {'2', 'L'}, {'J', 'M'} };

        public char Label { get; } = label;
        public char WeightOne { get; } = LabelWeightPairsOne[label];
        public char WeightTwo { get; } = LabelWeightPairsTwo[label];
    }
}