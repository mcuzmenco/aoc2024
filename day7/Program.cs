var inputData = File.ReadAllText("test.txt")
    .Split('\n', StringSplitOptions.RemoveEmptyEntries)
    .Select(line => line.Split(':', StringSplitOptions.RemoveEmptyEntries))
    .Select(parts => new
    {
        EndResult = long.Parse(parts[0].Trim()),
        Numbers = parts[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse).ToArray()
    })
    .ToList();

Console.WriteLine($"Total Sum Part1: {CalculateSum()}");
Console.WriteLine($"Total Sum Part2: {CalculateSum(true)}");
long CalculateSum(bool includeConcat = false)
{
    var sum = 0L;
    foreach (var row in inputData)
    {
        var expectedResult = row.EndResult;
    
        // reversing because during recursion the tail would be evaluated first
        var newArray = row.Numbers.Reverse().ToArray();
        var allResults = ProduceResults(newArray, includeConcat);
        if (allResults.Any(x => x == expectedResult))
        {
            sum += row.EndResult;
        }
    }

    return sum;
}

static IEnumerable<long> ProduceResults(long[] values, bool includeConcat)
{
    if (values.Length == 1)
    {
        yield return values[0];
    }
    else
    {
        foreach (var tailValue in ProduceResults(values[1..], includeConcat))
        {
            yield return values[0] * tailValue;
            yield return values[0] + tailValue;
            if (includeConcat)
            {
                yield return long.Parse($"{tailValue}{values[0]}");
            }
        }
    }
}
