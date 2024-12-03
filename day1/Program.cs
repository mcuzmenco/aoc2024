var lines = File.ReadAllLines("real.txt");

List<long> distancesleft = new List<long>();
List<long> distancesright = new List<long>();
foreach (var line in lines)
{
    var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    distancesleft.Add(long.Parse(parts[0]));
    distancesright.Add(long.Parse(parts[1]));
}

long resultPart1 = CalculateTotalDistance(distancesleft, distancesright);
Console.WriteLine("Total Distance: " + resultPart1);

long resultPart2 = CalculateSimilarityScore(distancesleft, distancesright);
Console.WriteLine("Total Similarity Score: " + resultPart2);


static long CalculateSimilarityScore(List<long> leftList, List<long> rightList)
{
    var refDictionary = rightList.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
    long totalSimilarity = 0;
    foreach (var left in leftList)
    {
        if (refDictionary.TryGetValue(left, out var value))
        {
            totalSimilarity += left * value;
        }
    }
    
    return totalSimilarity;
}

static long CalculateTotalDistance(List<long>  leftList, List<long>  rightList)
{
    var leftSorted = leftList.OrderBy(x => x).ToArray();
    var rightSorted = rightList.OrderBy(x => x).ToArray();
    
    long totalDistance = 0;
    for (int i = 0; i < leftSorted.Length; i++)
    {
        totalDistance += Math.Abs(leftSorted[i] - rightSorted[i]);
    }

    return totalDistance;
}