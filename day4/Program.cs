var input = File.ReadLines("real.txt").Select(s => s.ToCharArray()).ToArray();

var totalCount = GetTotalCountPart1(input);

Console.WriteLine($"Total count: {totalCount}");

var totalCount2 = GetTotalCountPart2(input);

Console.WriteLine($"Total count part 2: {totalCount2}");

int GetTotalCountPart2(char[][] chars)
{
    var count2 = 0;
    for (int i = 0; i < chars.Length; i++)
    {
        for (int j = 0; j < chars[i].Length; j++)
        {
            count2 += GetXMasWordCountPart2(i, j, chars);
        }
    }

    return count2;
}

int GetTotalCountPart1(char[][] chars)
{
    var count1 = 0;
    for (int i = 0; i < chars.Length; i++)
    {
        for (int j = 0; j < chars[i].Length; j++)
        {
            count1 += GetXMasWordCountPart1(i, j, chars);
        }
    }

    return count1;
}

static IEnumerable<List<(int x, int y)>> GetPossiblePositionsPart1(int x, int y)
{
    yield return [(x + 1, y), (x + 2, y), (x + 3, y)];
    yield return [(x - 1, y), (x - 2, y), (x - 3, y)];
    yield return [(x, y + 1), (x, y + 2), (x, y + 3)];
    yield return [(x, y - 1), (x, y - 2), (x, y - 3)];

    yield return [(x + 1, y + 1), (x + 2, y + 2), (x + 3, y + 3)];
    yield return [(x + 1, y - 1), (x + 2, y - 2), (x + 3, y - 3)];
    yield return [(x - 1, y + 1), (x - 2, y + 2), (x - 3, y + 3)];
    yield return [(x - 1, y - 1), (x - 2, y - 2), (x - 3, y - 3)];
}

int GetXMasWordCountPart1(int x, int y, char[][] input)
{
    var character = input[x][y];
    if (character != 'X')
    {
        return 0;
    }

    var count = 0;
    foreach (var otherCharCoordinates in GetPossiblePositionsPart1(x, y))
    {
        var s = otherCharCoordinates.Aggregate("", (current, c) => current + GetSafe(input, c.x, c.y));
        if (s == "MAS")
        {
            count++;
        }
    }

    return count;
}


int GetXMasWordCountPart2(int x, int y, char[][] input)
{
    var character = input[x][y];
    if (character != 'A')
    {
        return 0;
    }

    var count = 0;
    foreach (var charCoordinates in GetPossiblePositionsPart2(x, y))
    {
        var s = charCoordinates.Aggregate("", (current, c) => current + GetSafe(input, c.x, c.y));

        if (s is "MAS" or "SAM")
        {
            count++;
        }
    }

    return count == 2 ? 1 : 0;
}

static IEnumerable<List<(int x, int y)>> GetPossiblePositionsPart2(int x, int y)
{
    yield return [(x - 1, y + 1), (x, y), (x + 1, y - 1)];
    yield return [(x - 1, y - 1), (x, y), (x + 1, y + 1)];
}

static char GetSafe(char[][] input, int x, int y)
{
    if (x < 0 || y < 0 || x >= input.Length || y >= input.Length || x >= input[0].Length || y >= input[0].Length)
    {
        return '.';
    }

    return input[x][y];
}