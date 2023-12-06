using System.Globalization;

namespace Puzzles;

public class Day04(string input) : DailyPuzzleBase(input)
{
    public override string GetPuzzleOneSolution()
    {
        return GetCards()
            .Select(c => c.Numbers.Count(c.WinningNumbers.Contains))
            .Where(c => c > 0)
            .Select(matchCount => Math.Pow(2, matchCount - 1))
            .Sum()
            .ToString(CultureInfo.InvariantCulture);
    }

    public override string GetPuzzleTwoSolution()
    {
        var cards = GetCards();
        var cardNumbers = cards.Select(c => new CardNumber { Count = 1, Card = c }).ToArray();

        for (int i = 0; i < cards.Count; i++)
        {
            var cardNumber = cardNumbers[i];
            var card = cardNumber.Card;

            int matchesCount = card.Numbers.Count(card.WinningNumbers.Contains);
            for (int j = 0; j < matchesCount; j++)
                if(i + j + 1 < cardNumbers.Length)
                    cardNumbers[i + j + 1].Count += cardNumber.Count;
        }

        return cardNumbers.Sum(cn => cn.Count).ToString();
    }

    private List<Card> GetCards()
    {
        var cards = new List<Card>();

        foreach (var line in Input.Split(Environment.NewLine))
        {
            var numberLists = line.Split(':').Last().Split('|');
            var card = new Card
            {
                WinningNumbers = numberLists.First().Trim().Split(' ').Select(n => n.Trim()).Where(n => !string.IsNullOrEmpty(n)).Select(int.Parse).ToList(),
                Numbers = numberLists.Last().Trim().Split(' ').Select(n => n.Trim()).Where(n => !string.IsNullOrEmpty(n)).Select(int.Parse).ToList()
            };

            cards.Add(card);
        }

        return cards;
    }

    private class Card
    {
        public List<int> Numbers { get; set; }
        public List<int> WinningNumbers { get; set; }
    }

    private class CardNumber
    {
        public int Count { get; set; }
        public Card Card { get; set; }
    }
}