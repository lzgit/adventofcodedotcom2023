using static System.Char;

namespace Puzzles;

public class Day03(string input) : DailyPuzzleBase(input)
{
    public override string GetPuzzleOneSolution()
    {
        var m = Input.Split(Environment.NewLine).ToArray();
        var h = m.Length;
        var w = m[0].Length;

        bool IsAdjacentSymbol(int r, int c)
        {
            if (r < 0 || r >= h || c < 0 || c >= w)
                return false;

            return IsSymbol(m[r][c]);
        };

        var sum = 0;
        for (int row = 0; row < h; row++)
        {
            bool isAdjacent = false;
            string numberString = string.Empty;
            for (int col = 0; col < w; col++)
            {
                if (IsDigit(m[row][col]))
                {
                    numberString += m[row][col];
                    if (!isAdjacent)
                    {
                        isAdjacent = IsAdjacentSymbol(row, col + 1) ||
                                     IsAdjacentSymbol(row, col - 1) ||
                                     IsAdjacentSymbol(row + 1, col) ||
                                     IsAdjacentSymbol(row - 1, col) ||
                                     IsAdjacentSymbol(row + 1, col + 1) ||
                                     IsAdjacentSymbol(row - 1, col - 1) ||
                                     IsAdjacentSymbol(row + 1, col - 1) ||
                                     IsAdjacentSymbol(row - 1, col + 1)
                            ;
                    }
                }
                else
                {
                    if (isAdjacent)
                        sum += int.Parse(numberString);

                    isAdjacent = false;
                    numberString = string.Empty;
                }
            }

            if (isAdjacent)
                sum += int.Parse(numberString);
        }

        return sum.ToString();
    }


    private bool IsSymbol(char ch) => !IsDigit(ch) && ch != '.';

    public override string GetPuzzleTwoSolution()
    {
        return "";
    }
}