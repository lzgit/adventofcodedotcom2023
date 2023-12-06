using static System.Char;

namespace Puzzles;

public class Day03(string input) : DailyPuzzleBase(input)
{
    public override string GetPuzzleOneSolution()
    {
        var m = Input.Split(Environment.NewLine).ToArray();
        var h = m.Length;
        var w = m[0].Length;

        bool IsSymbol(int r, int c)
        {
            if (r < 0 || r >= h || c < 0 || c >= w)
                return false;

            return !IsDigit(m[r][c]) && m[r][c] != '.';
        }

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
                        isAdjacent = IsSymbol(row, col + 1) ||
                                     IsSymbol(row, col - 1) ||
                                     IsSymbol(row + 1, col) ||
                                     IsSymbol(row - 1, col) ||
                                     IsSymbol(row + 1, col + 1) ||
                                     IsSymbol(row - 1, col - 1) ||
                                     IsSymbol(row + 1, col - 1) ||
                                     IsSymbol(row - 1, col + 1)
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

    public override string GetPuzzleTwoSolution()
    {
        var m = Input.Split(Environment.NewLine).ToArray();
        var h = m.Length;
        var w = m[0].Length;

        bool IsAsterisk(int r, int c)
        {
            if (r < 0 || r >= h || c < 0 || c >= w)
                return false;

            return m[r][c] == '*';
        }

        var asteriskAdjacentNumbers = new List<int>[h, w];
        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                asteriskAdjacentNumbers[i, j] = new List<int>();
            }
        }

        void RegisterPossibleGears(int numberStringColToCheck, int numberStringRowToCheck, string numberStringToCheck)
        {
            for (int c = numberStringColToCheck - 1; c < numberStringColToCheck + numberStringToCheck.Length + 1; c++)
            {
                if (IsAsterisk(numberStringRowToCheck - 1, c))
                    asteriskAdjacentNumbers[numberStringRowToCheck - 1, c] =
                        asteriskAdjacentNumbers[numberStringRowToCheck - 1, c]
                            .Concat(new[] { int.Parse(numberStringToCheck) })
                            .ToList();
            }

            for (int c = numberStringColToCheck - 1; c < numberStringColToCheck + numberStringToCheck.Length + 1; c++)
            {
                if (IsAsterisk(numberStringRowToCheck + 1, c))
                    asteriskAdjacentNumbers[numberStringRowToCheck + 1, c] =
                        asteriskAdjacentNumbers[numberStringRowToCheck + 1, c]
                            .Concat(new[] { int.Parse(numberStringToCheck) })
                            .ToList();
            }

            if (IsAsterisk(numberStringRowToCheck, numberStringColToCheck - 1))
                asteriskAdjacentNumbers[numberStringRowToCheck, numberStringColToCheck - 1] =
                    asteriskAdjacentNumbers[numberStringRowToCheck, numberStringColToCheck - 1]
                        .Concat(new[] { int.Parse(numberStringToCheck) })
                        .ToList();

            if (IsAsterisk(numberStringRowToCheck, numberStringColToCheck + numberStringToCheck.Length))
                asteriskAdjacentNumbers[numberStringRowToCheck, numberStringColToCheck + numberStringToCheck.Length] =
                    asteriskAdjacentNumbers[numberStringRowToCheck, numberStringColToCheck + numberStringToCheck.Length]
                        .Concat(new[] { int.Parse(numberStringToCheck) })
                        .ToList();
        }

        for (int row = 0; row < h; row++)
        {
            string numberString = string.Empty;
            int numberStringRow = -1;
            int numberStringCol = -1;

            for (int col = 0; col < w; col++)
            {
                if (IsNumber(m[row][col]))
                {
                    if (numberString == string.Empty)
                    {
                        numberStringCol = col;
                        numberStringRow = row;
                    }

                    numberString += m[row][col];
                }
                else
                {
                    RegisterPossibleGears(numberStringCol, numberStringRow, numberString);

                    numberString = string.Empty;
                    numberStringRow = -1;
                    numberStringCol = -1;
                }
            }

            RegisterPossibleGears(numberStringCol, numberStringRow, numberString);
        }

        int sum = 0;
        for (int i = 0; i < h; i++)
        {
            for (int j = 0; j < w; j++)
            {
                if (asteriskAdjacentNumbers[i, j].Count == 2)
                    sum += asteriskAdjacentNumbers[i, j].Aggregate(1, (g1, g2) => g1 * g2);
            }
        }

        return sum.ToString();
    }
}