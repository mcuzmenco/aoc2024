var inputNumbers = File.ReadAllText("real.txt").Split(" ").Select(long.Parse).ToList();

Part1(inputNumbers);

Part2(inputNumbers);


void Part1(List<long> sequence)
{
    var totalCount = GetStoneSequence(sequence, 0, 25).Count();
    Console.WriteLine(totalCount);
}

void Part2(List<long> sequence)
{
    var dictionary25Step = new Dictionary<long, Dictionary<long, long>>();
    foreach (var number in sequence)
    {
        var twentyFiveStepsProjection = GetStoneSequence([number], 0, 25)
            .GroupBy(x => x).ToDictionary(x => x.Key, x => (long)x.Count());

        dictionary25Step[number] = twentyFiveStepsProjection;
    }

    // populating dictionary for all new discovered keys on  50th and 75th
    var i = 2;
    while (i > 0)
    {
        var allKeys = dictionary25Step
            .SelectMany(x => x.Value.Keys)
            .Distinct()
            .ToList();

        var missingKeys = allKeys
            .Where(k => !dictionary25Step.ContainsKey(k))
            .ToList();

        foreach (var number in missingKeys)
        {
            var projection = GetStoneSequence([number], 0, 25)
                .GroupBy(x => x)
                .ToDictionary(g => g.Key, g => (long)g.Count());

            dictionary25Step[number] = projection;
        }

        i--;
    }

    long total =
        (from number in inputNumbers
            let level1Numbers = dictionary25Step[number]
            from level1 in level1Numbers
            let level2Numbers = dictionary25Step[level1.Key]
            from level2 in level2Numbers
            let level3Numbers = dictionary25Step[level2.Key]
            select level3Numbers.Values.Sum() * level2.Value * level1.Value)
        .Sum();
    
    Console.WriteLine(total);
}


IEnumerable<long> GetStoneSequence(IEnumerable<long> numbers, int depth, int maxDepth)
{
    if (depth == maxDepth)
    {
        foreach (var n in numbers)
        {
            yield return n;
        }

        yield break;
    }

    foreach (var n in numbers)
    {
        var newStones = ProduceStones(n);
        foreach (var newStone in GetStoneSequence(newStones, depth + 1, maxDepth))
        {
            yield return newStone;
        }
    }
}

IEnumerable<long> ProduceStones(long number)
{
    if (number == 0)
    {
        yield return 1;
    }
    else
    {
        var numberAsString = number.ToString();
        if (numberAsString.Length % 2 == 0)
        {
            yield return GetTrimmedNumber(numberAsString.Substring(0, numberAsString.Length / 2));
            yield return GetTrimmedNumber(
                numberAsString.Substring(numberAsString.Length / 2, numberAsString.Length / 2));
        }
        else
        {
            yield return number * 2024;
        }
    }
}

long GetTrimmedNumber(string number)
{
    if (number.Length == 1)
    {
        return long.Parse(number);
    }

    var trimmedNumber = number.TrimStart('0');
    return long.Parse(trimmedNumber == string.Empty ? "0" : trimmedNumber);
}