using common;

var grid = Utils.ReadFile("real.txt");

var startingPoint = grid.Single(x => x.Value == 'S');
var endPoint = grid.Single(x => x.Value == 'E');

Dictionary<Point, int> InitCosts()
{
    var result = new Dictionary<Point, int>();
    foreach (var p in grid.Where(x => x.Value != '#'))
    {
        result.Add(p.Key, Int32.MaxValue);
    }
    
    return result;
}

var costs = InitCosts();
costs[startingPoint.Key] = 0;
var predecessor = Traverse(startingPoint.Key, costs);
PrintPath(predecessor, endPoint.Key, grid);

Console.WriteLine(costs[endPoint.Key]);

Dictionary<Point, (Point, Direction)> Traverse(Point startingPoint, Dictionary<Point, int> cost)
{
    var predecessor = new Dictionary<Point, (Point, Direction)>();
    var pq = new PriorityQueue<(Point point, Direction direction), int>();
    pq.Enqueue((startingPoint, Direction.Right), 0);
    while (pq.Count > 0)
    {
        var current = pq.Dequeue();
        foreach (var (neighbour, dir) in GetPointsAround(current.point).Where(x => cost.ContainsKey(x.Item1)))
        {
            var weight = 1;
            if (dir != current.direction)
            {
                weight += 1000;
            }

            if (cost[current.point] != Int32.MaxValue && cost[current.point] + weight < cost[neighbour])
            {
                predecessor[neighbour] = (current.point, dir);
                cost[neighbour] = cost[current.point] + weight;
                pq.Enqueue((neighbour, dir), cost[neighbour]);
            }
        }
    }

    return predecessor;
}

void PrintPath(Dictionary<Point, (Point, Direction)> predecessor, Point toPoint, Dictionary<Point, char> grid)
{
    var path = new Dictionary<Point, char>();
    var current = toPoint;
    while(predecessor.TryGetValue(current, out var prev))
    {
        var c = prev.Item2 switch
        {
            Direction.Right => '>',
            Direction.Down => 'v',
            Direction.Up => '^',
            Direction.Left => '<',
        };
        path.Add(prev.Item1, c);
        current = prev.Item1;
    }
    
    var maxRow = grid.Keys.Max(x => x.Row);
    var maxCol = grid.Keys.Max(x => x.Col);

    for (int i = 0; i <= maxRow; i++)
    {
        for (int j = 0; j <= maxCol; j++)
        {
            if (path.ContainsKey(new Point(i, j)))
            {
                Console.Write(path[new Point(i, j)]);
            }
            else
            {
                Console.Write(grid[new Point(i, j)]);
            }
        }
        
        Console.Write("\r\n");
    }
}

IEnumerable<(Point, Direction)> GetPointsAround(Point point)
{
    yield return (point with { Row = point.Row + 1 }, Direction.Down);
    yield return (point with { Row = point.Row - 1 }, Direction.Up);
    yield return (point with { Col = point.Col + 1 }, Direction.Right);
    yield return (point with { Col = point.Col - 1 }, Direction.Left);
}
enum Direction
{
    Up,
    Down,
    Left,
    Right
};