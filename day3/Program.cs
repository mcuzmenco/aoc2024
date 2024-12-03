using System.Text.RegularExpressions;

var input = File.ReadAllText("real.txt");

Console.WriteLine($"Total sum part 1: {GetPart1(input)}");
Console.WriteLine($"Total sum part 2: {GetPart2(input)}");

static long GetPart1(string input)
{
    string pattern = @"mul\((\d{1,3}),(\d{1,3})\)";

    MatchCollection matches = Regex.Matches(input, pattern);

    long sum = 0L;
    foreach (Match match in matches)
    {
        if (match.Groups.Count == 3)
        {
            int x = int.Parse(match.Groups[1].Value); 
            int y = int.Parse(match.Groups[2].Value);

            sum += x * y;
        }
    }

    return sum;
}

static long GetPart2(string input)
{
    string pattern = @"mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\)";

    MatchCollection matches = Regex.Matches(input, pattern);

    long sum = 0L;
    var doCalculation = true;
    foreach (Match match in matches)
    {
        if (match.Groups[0].Value == "do()")
        {
            doCalculation = true;
            continue;
        }
        
        if (match.Groups[0].Value == "don't()")
        {
            doCalculation = false;
            continue;
        }
        
        if (match.Groups.Count == 3 && doCalculation)
        {
            int x = int.Parse(match.Groups[1].Value); 
            int y = int.Parse(match.Groups[2].Value);

            sum += x * y;
        }
    }

    return sum;
}