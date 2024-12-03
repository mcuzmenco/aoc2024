var reports = File.ReadAllLines("real.txt")
    .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse)
        .ToArray())
    .ToArray();

var totalSafeReports = reports.Where(IsSafeReport).Count();
Console.WriteLine($"Total safe reports: {totalSafeReports}");

var totalSafeReportsPart2 = reports.Where(IsSafeReportPart2).Count();
Console.WriteLine($"Total safe reports: {totalSafeReportsPart2}");


static bool IsSafeReport(int[] array)
{
    var increasing = array[0] < array[1];
    for (var i = 1; i < array.Length; i++)
    {
        if (increasing && array[i] < array[i - 1])
        {
            return false;
        }
        
        if (!increasing && array[i] > array[i - 1])
        {
            return false;
        }
        
        var diff = Math.Abs(array[i] - array[i - 1]);
        if (diff < 1 || diff > 3)
        {
            return false;
        }
    }
    
    return true;
}

static IEnumerable<int[]> GetChildArrays(int[] array)
{
    for (int i = 0; i < array.Length; i++)
    {
        if (i == 0)
        {
            yield return array[(i + 1)..];
        } else if (i == array.Length - 1) {
            yield return array[..i];
        }
        else
        {
           yield return array[..i]
                .Concat(array[(i + 1)..])
                .ToArray();
        }
    }
}

static bool IsSafeReportPart2(int[] array)
{
    var safe = IsSafeReport(array);
    if (safe)
    {
        return true;
    }

    var childArrays = GetChildArrays(array);
    return childArrays.Any(IsSafeReport);
}