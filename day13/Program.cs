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
    problem.Prize = new Prize(problem.Prize.X + 10000000000000, problem.Prize.Y + 10000000000000);
    var (stepsA, stepsB) = SolveProblem(problem);
    
    // now we are just checking that stepsA and stepsB aren't fractional and are positive.
    if (stepsA > 0 && stepsB > 0 && stepsA == Math.Floor(stepsA) && stepsB == Math.Floor(stepsB))
    {
        mlongokenSpent += Convert.ToInt64(stepsA) * 3 + Convert.ToInt64(stepsB);
    }
}

Console.WriteLine(mlongokenSpent);


// trivial long running thing. Was able to solve part1 with it, but not part 2.
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

/*
 * Basically the problem is equals to this one:
 * X*A1+Y*B1=C1
 * X*A2+Y*B2=C2
 *
 * Where X = number to press button A
 * Where Y = number to press button B
 *
 * From this equation this is the result for X, Y coefficients.
 * 
 * X = (C1*B2 - C2*B1)/(A1*B2 - A2*B1)
 * Y = (C1*A2 - C2*A1)/(A2*B1 - B2*A1)
 *
 */
(double stepsButtonA, double stepsButtonB) SolveProblem(Problem problem)
{
    double x = ((double)(problem.Prize.X * problem.ButtonB.Y - problem.Prize.Y * problem.ButtonB.X)) / (problem.ButtonA.X * problem.ButtonB.Y - problem.ButtonA.Y * problem.ButtonB.X);
    double y = ((double)(problem.Prize.X * problem.ButtonA.Y - problem.Prize.Y * problem.ButtonA.X)) / (problem.ButtonA.Y * problem.ButtonB.X - problem.ButtonB.Y * problem.ButtonA.X);

    return (x, y);
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