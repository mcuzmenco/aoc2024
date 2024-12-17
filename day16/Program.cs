using common;

var grid = Utils.ReadFile("real.txt");

var startingPoint = grid.Single(x => x.Value == 'S');
var endPoint = grid.Single(x => x.Value == 'E');

var costs = new Dictionary<(Point point, Direction direction), int>
{
    [(startingPoint.Key, Direction.Right)] = 0
};
var predecessor2 = TraversePart2(costs);
List<int> endPointCosts =
[
    costs[(endPoint.Key, Direction.Right)],
    costs[(endPoint.Key, Direction.Up)],
    costs[(endPoint.Key, Direction.Left)],
    costs[(endPoint.Key, Direction.Down)],
];
Console.WriteLine($"Part1: {endPointCosts.Min()}");
Process();

Dictionary<(Point point, Direction direction), HashSet<(Point, Direction)>> TraversePart2(Dictionary<(Point point, Direction direction), int> cost)
{
    var predecessor = 
        cost.Select(x => x.Key)
        .ToDictionary(x => x, x => new HashSet<(Point, Direction)>());
    var pq = new PriorityQueue<(Point point, Direction direction), int>();
    pq.Enqueue((startingPoint.Key, Direction.Right), 0);
    while (pq.Count > 0)
    {
        var current = pq.Dequeue();
        var neighbours = GetPointsAround(current.point)
            .Where(x => grid.ContainsKey(current.point))
            .Where(x => grid[current.point] != '#')
            .ToArray();
        var possibleRotationsOfTheCurrentNode = neighbours
            .Select(x => x.Item2)
            .Where(x => x != current.direction)
            .Select(x => (current.point, x))
            .ToList();
        var directNeighbour = neighbours.SingleOrDefault(x => x.Item2 == current.direction);
        if (directNeighbour != default)
        {
            possibleRotationsOfTheCurrentNode.Add(directNeighbour);
        }
        
        foreach (var (neighbour, dir) in possibleRotationsOfTheCurrentNode)
        {
            var weight = dir == current.direction ? 1 : 1000;

            var newWeight = cost[current] + weight;
            if (!cost.ContainsKey((neighbour, dir)))
            {
                cost[(neighbour, dir)] = Int32.MaxValue;
                predecessor[(neighbour, dir)] = new HashSet<(Point, Direction)>();
            }
            
            if (newWeight < cost[(neighbour, dir)])
            {
                predecessor[(neighbour, dir)].Clear();
                predecessor[(neighbour, dir)].Add(current);
                cost[(neighbour, dir)] = newWeight;
                pq.Enqueue((neighbour, dir), cost[(neighbour, dir)]);
            }
            else if (newWeight == cost[(neighbour, dir)])
            {
                predecessor[(neighbour, dir)].Add(current);
                pq.Enqueue((neighbour, dir), cost[(neighbour, dir)]);
            }
        }
    }

    return predecessor;
}

void Process()
{
    var visitedSet = new HashSet<(Point, Direction)>();
    Loop((endPoint.Key, Direction.Down));
    Loop((endPoint.Key, Direction.Up));
    Loop((endPoint.Key, Direction.Left));
    Loop((endPoint.Key, Direction.Right));

    void Loop((Point, Direction) current)
    {
        if (visitedSet.Contains(current))
        {
            return;
        }
        
        if (predecessor2.TryGetValue(current, out var next))
        {
            visitedSet.Add(current);
            foreach (var n in next)
            {
                Loop(n);
            }
        }
    }
    
    var maxRow = grid.Keys.Max(x => x.Row);
    var maxCol = grid.Keys.Max(x => x.Col);

    var uniqueVisited = visitedSet.Select(x => x.Item1).Distinct().ToHashSet();
    for (int i = 0; i <= maxRow; i++)
    {
        for (int j = 0; j <= maxCol; j++)
        {
            if (uniqueVisited.Contains(new Point(i, j)))
            {
                Console.Write("O");
            }
            else
            {
                Console.Write(grid[new Point(i, j)]);
            }
        }
        
        Console.Write("\r\n");
    }
    
    Console.WriteLine($"Part2: {uniqueVisited.Count}");
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