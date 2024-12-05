var input = File.ReadAllText("real.txt");
var parts = input.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);

var pairs = parts[0]
    .Split("\r\n", StringSplitOptions.RemoveEmptyEntries)
    .Select(line =>
    {
        var numbers = line.Split('|').Select(int.Parse).ToArray();
        return new SortNumberPair(numbers[0], numbers[1]);
    })
    .ToList();

var rows = parts[1]
    .Split("\r\n", StringSplitOptions.RemoveEmptyEntries)
    .Select(line => line.Split(',').Select(int.Parse).ToArray())
    .ToList();

var numbersShouldGoBeforeByNumber = pairs
    .GroupBy(x => x.Second)
    .ToDictionary(x => x.Key, x => x.Select(x => x.First).ToHashSet());

Part1(rows, numbersShouldGoBeforeByNumber);
Part2(rows, numbersShouldGoBeforeByNumber);

void Part1(List<int[]> updates, Dictionary<int, HashSet<int>> dictionary)
{
    ProcessUpdates(updates, dictionary, false);
}

void Part2(List<int[]> updates, Dictionary<int, HashSet<int>> dictionary)
{
    ProcessUpdates(updates, dictionary, true);
}

void ProcessUpdates(List<int[]> updates, Dictionary<int, HashSet<int>> dictionary, bool part2)
{
    var sumMidNumbers = 0;

    foreach (var row in updates)
    {
        var numbersGone = new HashSet<int>();
        bool success = true;

        foreach (var number in row)
        {
            var numbersShouldGoBefore = dictionary.GetValueOrDefault(number);
            if (numbersShouldGoBefore == null && numbersGone.Count > 0)
            {
                success = false;
                break;
            }
        
            if (numbersShouldGoBefore != null && numbersGone.Any(x => !numbersShouldGoBefore.Contains(x)))
            {
                success = false;
                break;
            }
        
            numbersGone.Add(number);
        }

        if (!success && part2)
        {
            Array.Sort(row, new CustomOrderComparer(dictionary));
            sumMidNumbers += row[row.Length / 2];
            Console.WriteLine($"Mid Number: {row[row.Length / 2]}");
        }

        if (success && !part2)
        {
            sumMidNumbers += row[row.Length / 2];
            Console.WriteLine($"Mid Number: {row[row.Length / 2]}");
        }
    }

    Console.WriteLine($"Result {(part2 ? "Part2" : "Part1")} : {sumMidNumbers}");
}

public class CustomOrderComparer(Dictionary<int, HashSet<int>> dictionary) : IComparer<int>
{
    public int Compare(int x, int y)
    {
        if (x == y)
        {
            return 0;
        }

        var dependencies = dictionary.GetValueOrDefault(x);
        if (dependencies != default && dependencies.Contains(y))
        {
            return 1;
        }
        
        return -1;
    }
}

public record SortNumberPair(int First, int Second);