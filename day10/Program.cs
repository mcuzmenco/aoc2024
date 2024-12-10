
using common;

var lines = File.ReadAllLines("real.txt");

var grid = new Dictionary<Point, int>();
for (int i = 0; i < lines.Length; i++)
{
    for (var j = 0; j < lines[i].Length; j++)
    {
        if (lines[i][j] != '.')
        {
            grid[new Point(i, j)] = int.Parse(lines[i][j].ToString());
        }
    }
}

var startingPositions = grid.Where(x => x.Value == 0).ToList();

var r1 = 0;
var r2 = 0;
foreach (var tailHead in startingPositions)
{
    var rootNode = new TreeNode<Point>(tailHead.Key);
    BuildTree(rootNode);
    var highPoints = GetHighPoints(rootNode).ToList();
    r1 += highPoints.Distinct().Count();
    r2 += highPoints.Count();
}

Console.WriteLine(r1);
Console.WriteLine(r2);

void BuildTree(TreeNode<Point> rootNode)
{
    foreach (var nextPosition in GetPositionsAtNextHeight(rootNode.Value, grid[rootNode.Value]))
    {
        var childNode = new TreeNode<Point>(nextPosition);
        rootNode.AddChild(childNode);
        BuildTree(childNode);
    }
}

IEnumerable<Point> GetHighPoints(TreeNode<Point> rootNode)
{
    if (grid[rootNode.Value] == 9)
    {
        yield return rootNode.Value;
    }

    foreach (var p in rootNode.Children.SelectMany(GetHighPoints))
    {
        yield return p;
    }
}

List<Point> GetPositionsAtNextHeight(Point position, int height)
{
    var result = new List<Point>();
    var positionsAround = GetPointsAround(position).ToList();
    foreach (var point in positionsAround)
    {
        if (grid.TryGetValue(point, out var value) && value == height + 1)
        {
            result.Add(point);
        }
    }
    
    return result;
}

IEnumerable<Point> GetPointsAround(Point point)
{
    yield return point with { Row = point.Row + 1 };
    yield return point with { Row = point.Row - 1 };
    yield return point with { Col = point.Col + 1 };
    yield return point with { Col = point.Col - 1 };
}

