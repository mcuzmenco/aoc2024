using common;

var originalGrid = Utils.ReadFile("real.txt");

var startingPoint = originalGrid.Single(x => x.Value == 'S');
var endPoint = originalGrid.Single(x => x.Value == 'E');
var maxCol = originalGrid.Select(x => x.Key.Col).Max();
var maxRow = originalGrid.Select(x => x.Key.Row).Max();


var part1 = Process(2, 100);
Console.WriteLine($"Part 1: {part1}");

var part2 = Process(20, 100);
Console.WriteLine($"Part 2: {part2}");

int Process(int cheatsAvailable, int saveAtLeast)
{
    var (distancesFromStart, predessor) = Traverse(originalGrid, startingPoint.Key);
    var (distancesFromEnd, _) = Traverse(originalGrid, endPoint.Key);
    var originalCost = distancesFromStart[endPoint.Key];
    var pointsAlongTheRoute = GetPointsAlongThePath(predessor, endPoint.Key);
    var distances = new Dictionary<(Point startCheat, Point endCheat), int>();
    foreach (var point in pointsAlongTheRoute)
    {
        var manhattanDistancePoints = GetPointsWithinManhattanDistance(originalGrid, point, cheatsAvailable);
    
        foreach (var manhattanDistancePoint in manhattanDistancePoints)
        {
            if (distances.ContainsKey((point, manhattanDistancePoint.candidate)))
            {
                continue;
            }
        
            var distanceFromStart = distancesFromStart[point];
            if (distancesFromEnd.TryGetValue(manhattanDistancePoint.candidate, out var distanceToEnd))
            {
                var totalCost = distanceFromStart + manhattanDistancePoint.distance + distanceToEnd;
                if (totalCost < originalCost)
                {
                    distances[(point, manhattanDistancePoint.candidate)] = totalCost;
                }
            }
        }
    }
    
    return distances.Count(x => x.Value <= originalCost - saveAtLeast);
}

(Dictionary<Point, int> costs, Dictionary<Point, Point> predecessor) Traverse(Dictionary<Point, char> grid, Point from)
{
    var predecessor = new Dictionary<Point, Point>();
    var costs = new Dictionary<Point, int>()
    {
        { from, 0 }
    };
    var pq = new PriorityQueue<Point, int>();
    pq.Enqueue(from, 0);

    while (pq.Count > 0)
    {
        var current = pq.Dequeue();
        var adjacent = current.Adjacent(x =>
            x is { Row: >= 0, Col: >= 0 }
            && x.Row <= maxRow
            && x.Col <= maxCol
            && grid[x] != '#');

        foreach (var neighbor in adjacent)
        {
            var newDist = costs[current] + 1;
            if (!costs.ContainsKey(neighbor) || newDist < costs[neighbor])
            {
                predecessor[neighbor] = current;
                costs[neighbor] = newDist;
                pq.Enqueue(neighbor, newDist);
            }
        }
    }

    return (costs, predecessor);
}

HashSet<Point> GetPointsAlongThePath(Dictionary<Point, Point> predecessor, Point from)
{
    var visited = new HashSet<Point>() { from };
    var curr = from;
    while (predecessor.TryGetValue(curr, out var prev))
    {
        visited.Add(curr);
        curr = prev;
    }
    
    visited.Add(curr);
    
    return visited;
}

static IEnumerable<(Point candidate, int distance)> GetPointsWithinManhattanDistance(
    Dictionary<Point, char> grid,
    Point center,
    int maxDistance = 20)
{
    var minRow = grid.Keys.Min(k => k.Row);
    var maxRow = grid.Keys.Max(k => k.Row);
    var minCol = grid.Keys.Min(k => k.Col);
    var maxCol = grid.Keys.Max(k => k.Col);

    var rowStart = Math.Max(minRow, center.Row - maxDistance);
    var rowEnd = Math.Min(maxRow, center.Row + maxDistance);
    var colStart = Math.Max(minCol, center.Col - maxDistance);
    var colEnd = Math.Min(maxCol, center.Col + maxDistance);

    for (var r = rowStart; r <= rowEnd; r++)
    {
        for (var c = colStart; c <= colEnd; c++)
        {
            // Compute Manhattan distance
            var distance = Math.Abs(r - center.Row) + Math.Abs(c - center.Col);

            if (distance <= maxDistance)
            {
                var candidate = new Point(r, c);
                if (grid.TryGetValue(candidate, out var value) && value != '#')
                {
                    yield return (candidate, distance);
                }
            }
        }
    }
}