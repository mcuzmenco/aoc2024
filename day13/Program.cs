using System.Numerics;
using System.Text.RegularExpressions;

var input = File.ReadAllText("real.txt");
string pattern = @"Button A: X\+(\d+), Y\+(\d+)\r?\nButton B: X\+(\d+), Y\+(\d+)\r?\nPrize: X=(\d+), Y=(\d+)";
var matches = Regex.Matches(input, pattern);

var problems = new List<Problem>();


foreach (Match match in matches)
{
    problems.Add(new Problem(
        new Step(long.Parse(match.Groups[1].Value), long.Parse(match.Groups[2].Value), 3),
        new Step(long.Parse(match.Groups[3].Value), long.Parse(match.Groups[4].Value), 1),
        new Prize(long.Parse(match.Groups[5].Value), long.Parse(match.Groups[6].Value))
    ));
}

var mlongokenSpent = 0L;
foreach (var problem in problems)
{
    // problem.Prize = new Prize(problem.Prize.X + 10000000000000, problem.Prize.Y + 10000000000000);
    var costs = GetTokenSpentNumbers(problem, new HashSet<long>(), new HashSet<(long, long)>()).ToArray();
    if (costs.Any())
    {
        mlongokenSpent += costs.Min();
    }
}

Console.WriteLine(mlongokenSpent);


IEnumerable<long> GetTokenSpentNumbers(Problem problem, HashSet<long> usedPrize, HashSet<(long, long)> usedXY, long currentX = 0, long currentY = 0, long currentTokenCost = 0)
{
    if (!usedXY.Add((currentX, currentY)))
    {
        yield break;
    }
    
    if (currentX == problem.Prize.X && currentY == problem.Prize.Y)
    {
        if (usedPrize.Add(currentTokenCost))
        {
            yield return currentTokenCost;
        }
        yield break;
    }
    
    if (currentX > problem.Prize.X || currentY > problem.Prize.Y)
    {
        yield break;
    }

    foreach (var cost in GetTokenSpentNumbers(
                 problem,
                 usedPrize,
                 usedXY,
                 currentX + problem.ButtonA.X,
                 currentY + problem.ButtonA.Y,
                 currentTokenCost + problem.ButtonA.Cost))
    {
        yield return cost;
    }

    foreach (var cost in GetTokenSpentNumbers(
                 problem,
                 usedPrize,
                 usedXY,
                 currentX + problem.ButtonB.X,
                 currentY + problem.ButtonB.Y,
                 currentTokenCost + problem.ButtonB.Cost))
    {
        yield return cost;
    }
}

class Problem
{
    public Problem(Step buttonA, Step buttonB, Prize prize)
    {
        ButtonA = buttonA;
        ButtonB = buttonB;
        Prize = prize;
    }

    public Step ButtonA { get; set; }
    public Step ButtonB { get; set; }
    public Prize Prize { get; set; }
}

record Step (long X, long Y, long Cost);
record Prize(long X, long Y);