using common;

var points = File.ReadAllLines("real.txt")
    .Select(x =>
    {
        var parts = x.Split(',');
        return new Point(int.Parse(parts[1]), int.Parse(parts[0]));
    })
    .ToHashSet();

var maxCol = points.Select(x => x.Col).Max();
var maxRow = points.Select(x => x.Row).Max();


for (int i = 0; i < points.Count; i++)
{
    var newBlockerPoints = points.Take(1024 + i).ToHashSet();
    var newDist = CaculateDistances(newBlockerPoints);
    if (!newDist.ContainsKey(new Point(maxRow, maxCol)))
    {
        var pointToCauseBlockade = newBlockerPoints.Last();
        Console.WriteLine($"Part 2. Blocking Point: {pointToCauseBlockade.Col},{pointToCauseBlockade.Row}");
        break;
    }
}
//
// PrintGrid(points.ToList());
//

points = points.Take(1024).ToHashSet();
var dist = CaculateDistances(points);

Console.WriteLine($"Part1. Distance: {dist[new Point(maxRow, maxCol)]}");


void PrintGrid(List<Point> points)
{
    for (var row = 0; row <= maxRow; row++)
    {
        for (var col = 0; col <= maxCol; col++)
        {
            if (points.Contains(new Point(row, col)))
            {
                Console.Write("#");
            }
            else
            {
                Console.Write(".");
            }
        }

        Console.WriteLine();
    }
}

Dictionary<Point, int> CaculateDistances(HashSet<Point> brokenBytes)
{
    var p = new Point(0, 0);
    var dictionary = new Dictionary<Point, int> { { p, 0 } };
    var pq = new PriorityQueue<Point, int>();
    pq.Enqueue(p, 0);
    while (pq.TryDequeue(out Point current, out int currentDist))
    {
        var adjacent = current.Adjacent(x =>
            x is { Row: >= 0, Col: >= 0 }
            && x.Row <= maxRow
            && x.Col <= maxCol
            && !brokenBytes.Contains(x));

        foreach (var neighbor in adjacent)
        {
            int newDist = dictionary[current] + 1;
            if (!dictionary.ContainsKey(neighbor) || newDist < dictionary[neighbor])
            {
                dictionary[neighbor] = newDist;
                pq.Enqueue(neighbor, newDist);
            }
        }
    }

    return dictionary;
}