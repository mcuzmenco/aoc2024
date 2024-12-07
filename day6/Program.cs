using common;

var grid = Utils.ReadFile("real.txt");
var startingPosition = grid.First(x => x.Value == '^');

Console.WriteLine($"Guard visits are {Part1().Count}");
Console.WriteLine($"Positions causing a loop are {Part2()}");

HashSet<Point> Part1()
{
    var (visited, _) = TraverseRoute(startingPosition.Key, Direction.Up, grid);
    return visited.Select(x => x.Item1).Distinct().ToHashSet();
}

int Part2()
{
    var visitedPositions = Part1().Where(x => !x.Equals(startingPosition.Key));
    var withAdditionalBlock = visitedPositions.Select(visitedPosition =>
    {
        var newGridWithPositionBlocked = new Dictionary<Point, char>(grid)
        {
            [visitedPosition] = '#'
        };
        return newGridWithPositionBlocked;
    });

    var totalBlocked = 0;
    foreach (var possibleGrid in withAdditionalBlock)
    {
        var (_, causedLoop) = TraverseRoute(startingPosition.Key, Direction.Up, possibleGrid);
        if (causedLoop)
        {
            totalBlocked++;
        }
    }
    
    return totalBlocked;
}

(HashSet<(Point, Direction)> visited, bool causedLoop) TraverseRoute(Point position, Direction direction, Dictionary<Point, char> inputGrid)
{   
    var visited = new HashSet<(Point, Direction)> { (position, direction) };
    while (true)
    {
        var newPosition = GetNewPosition(direction, position);
        if (visited.Contains((newPosition, direction)))
        {
            return (visited, true);
        }
        
        var c = inputGrid.GetValueOrDefault(newPosition);
        if (c == default)
        {
            break;
        }

        if (c == '#')
        {
            direction = GetNewDirection(direction);
            continue;
        }

        position = newPosition;
        visited.Add((position, direction));
    }
    
    return (visited, false);
}

Direction GetNewDirection(Direction oldDirection)
{
    return oldDirection switch
    {
        Direction.Up => Direction.Right,
        Direction.Right => Direction.Down,
        Direction.Down => Direction.Left,
        Direction.Left => Direction.Up,
        _ => throw new ArgumentOutOfRangeException()
    };
}

Point GetNewPosition(Direction direction, Point point)
{
    return direction switch
    {
        Direction.Up =>  point with { Row = point.Row - 1 },
        Direction.Down => point with { Row = point.Row + 1 },
        Direction.Left => point with { Col = point.Col - 1 },
        Direction.Right => point with { Col = point.Col + 1 },
        _ => throw new ArgumentOutOfRangeException()
    };
}

enum Direction
{
    Up,
    Down,
    Left,
    Right
};