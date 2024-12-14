using System.Threading.Tasks.Dataflow;
using common;

var grid = Utils.ReadFile("real.txt");
List<GardenPatch> gardenPatches = new List<GardenPatch>();

foreach (var plant in grid)
{
    var plantsInTheSamePatch = GetPlantsAround(plant.Key).Where(x => grid[x] == plant.Value).ToArray();
    var gardenPatch = gardenPatches.FirstOrDefault(x => x.Points.Any(y => plantsInTheSamePatch.Contains(y)));
    if (plantsInTheSamePatch.Any())
    {
        var fenceLength = 4 - plantsInTheSamePatch.Length;
        if (gardenPatch != null)
        {
            gardenPatch.Points.Add(plant.Key);
            gardenPatch.FenceLength += 4 - plantsInTheSamePatch.Length;
        }
        else
        {
            gardenPatch = new GardenPatch(plant.Key, grid[plant.Key], fenceLength);
            gardenPatches.Add(gardenPatch);
        }
    }
    else
    {
        gardenPatch = new GardenPatch(plant.Key, grid[plant.Key], 4);
        gardenPatches.Add(gardenPatch);
    }
            
    // ugly... but whatever, bored
    var otherGardenPatch = gardenPatches.FirstOrDefault(x => x.Intersects(gardenPatch) && x != gardenPatch);
    if (otherGardenPatch != null)
    {
        foreach (var otherPoint in otherGardenPatch.Points)
        {
            gardenPatch.Points.Add(otherPoint);
        }
        
        
        gardenPatch.FenceLength += otherGardenPatch.FenceLength;
        gardenPatches.Remove(otherGardenPatch);
    }
}

// Part1
var totalPrice = 0;
foreach (var gardenPatch in gardenPatches)
{
    var plantName = grid[gardenPatch.Points.First()];
    Console.WriteLine($"A region of '{plantName}' Patch. Plants Count : {gardenPatch.Points.Count}. Fence Length : {gardenPatch.FenceLength}");
    totalPrice += gardenPatch.FenceLength * gardenPatch.Points.Count;
}

Console.WriteLine(totalPrice);

// Part2
totalPrice = 0;
foreach (var gardenPatch in gardenPatches)
{
    var plantName = grid[gardenPatch.Points.First()];
    Console.WriteLine($"A region of '{plantName}' Patch. Plants Count : {gardenPatch.Points.Count}. Sides number: {gardenPatch.NumberOfAngles()}");
    totalPrice += gardenPatch.NumberOfAngles() * gardenPatch.Points.Count;
}

Console.WriteLine(totalPrice);


IEnumerable<Point> GetPlantsAround(Point position)
{
    var positionsAround = GetPointsAround(position).ToList();
    foreach (var point in positionsAround)
    {
        if (grid.TryGetValue(point, out var value))
        {
            yield return point;
        }
    }
}

static IEnumerable<Point> GetPointsAround(Point point)
{
    yield return point with { Row = point.Row + 1 };
    yield return point with { Row = point.Row - 1 };
    yield return point with { Col = point.Col + 1 };
    yield return point with { Col = point.Col - 1 };
}

class GardenPatch
{
    public GardenPatch(Point p, char c, int fenceLength)
    {
        Points.Add(p);
        Name = c;
        FenceLength = fenceLength;
    }
    
    public HashSet<Point> Points { get; set; } = new HashSet<Point>();
    public int FenceLength { get; set; } = 0;
    public char Name { get; }

    public bool Intersects(GardenPatch otherPatch)
    {
        var t = from p in Points
            from op in otherPatch.Points
            where (p.Row == op.Row && p.Col == op.Col + 1)
                  || (p.Row == op.Row && p.Col == op.Col - 1)
                  || (p.Row == op.Row + 1 && p.Col == op.Col)
                  || (p.Row == op.Row - 1 && p.Col == op.Col)
                  select p;
        return Name == otherPatch.Name && t.Any();
    }
    
    // should be equal to the number of sides?

    public int NumberOfAngles()
    {
        var total = 0;
        foreach (var p in Points)
        {
            // normal angles
            if (!Points.Contains(p.Top()) && !Points.Contains(p.Right()))
            {
                total++;
            }
            
            if (!Points.Contains(p.Top()) && !Points.Contains(p.Left()))
            {
                total++;
            }
            
            if (!Points.Contains(p.Bottom()) && !Points.Contains(p.Left()))
            {
                total++;
            }
            
            if (!Points.Contains(p.Bottom()) && !Points.Contains(p.Right()))
            {
                total++;
            }
            
            // obtuse angles
            if (Points.Contains(p.Top()) && Points.Contains(p.Right()) && !Points.Contains(p.UpRight()))
            {
                total++;
            }
            
            if (Points.Contains(p.Top()) && Points.Contains(p.Left()) && !Points.Contains(p.UpLeft()))
            {
                total++;
            }
            
            if (Points.Contains(p.Bottom()) && Points.Contains(p.Right()) && !Points.Contains(p.BottomRight()))
            {
                total++;
            }
            
            if (Points.Contains(p.Bottom()) && Points.Contains(p.Left()) && !Points.Contains(p.BottomLeft()))
            {
                total++;
            }
        }
        
        return total;
    }

    public int GetNumberOfSides()
    {
        // grouped by row
        var totalSides = 4;
        var orderedPoints = Points
            .GroupBy(x => x.Row)
            .OrderBy(y => y.Key).ToList();
        if (orderedPoints.Count == 1)
        {
            return totalSides;
        }

        
        var minColumn = orderedPoints[0].Min(x => x.Col);
        var maxColumn = orderedPoints[0].Max(x => x.Col);
        // move by row
        for (int i = 1; i < orderedPoints.Count; i++)
        {
            var orderedRowPoints = orderedPoints[i].OrderBy(x => x.Col).ToList();
            var prevPoint = orderedRowPoints[0];
            var newMinColumn = prevPoint.Col;
            var newMaxColumn = prevPoint.Col;
            for (int j = 1; j <orderedRowPoints.Count; j++)
            {
                if (!orderedRowPoints[j].IsAdjacentHorizontally(prevPoint))
                {
                    totalSides += 2;
                }
                
                newMaxColumn = orderedRowPoints[j].Col;
                prevPoint = orderedRowPoints[j];
            }
            
            if (newMinColumn != minColumn)
            {
                totalSides += 2;
                minColumn = newMinColumn;
            }
            
            if (newMaxColumn != maxColumn)
            {
                totalSides += 2;
                maxColumn = newMaxColumn;
            }
        }
        
        return totalSides;
    }

    public int GetPart2Price()
    {
        return GetNumberOfSides() * Points.Count;
    }
}