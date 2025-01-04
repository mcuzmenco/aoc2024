// The raw input block.
// (You could also read this from a file or stdin if needed)
string input = File.ReadAllText("real.txt");
var (parts, stringsToVerify) = ParseInput(input);

var counter = 0;
long totalCount = 0;
var cache = new Dictionary<string, long>(); 
foreach (var str in stringsToVerify)
{
    var c = CalcPossibilities(str, parts.ToHashSet(), cache);
    totalCount += c;
    if (c > 0)
    {
        counter++;
    }
}

Console.WriteLine($"Total string which can be formed: {counter}. Total variations: {totalCount}");


(string[] parts, string[] stringsToVerify) ParseInput(string input)
{
    var lines = input
        .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
        .Select(line => line.Trim())
        .ToArray();

    var availableParts = lines[0]
        .Split(',')
        .Select(part => part.Trim())
        .ToArray();

    var stringsToVerify = lines.Skip(1).ToArray();
    return (availableParts, stringsToVerify);
}

long CalcPossibilities(string s, HashSet<string> parts, Dictionary<string, long> combinationsCache)
{
    if (combinationsCache.TryGetValue(s, out var i))
    {
        return i;
    }
    
    var firstParts = parts.Where(s.StartsWith).ToList();
    if (firstParts.Count == 0)
    {
        combinationsCache[s] = 0;
        return 0;
    }

    var c = 0L;
    foreach (var firstPart in firstParts)
    {
        if (firstPart == s)
        {
            c++;
        }
        else
        {
            var reminderPart = s.Substring(firstPart.Length);
            c += CalcPossibilities(reminderPart, parts, combinationsCache);
        }
    }

    combinationsCache[s] = c;
    return c;
}
