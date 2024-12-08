using common;

var grid = Utils.ReadFile("real.txt");
var dictionaryByChar = grid
    .Where(x => x.Value != '.')
    .GroupBy(x => x.Value)
    .ToDictionary(x => x.Key, x => x.Select(c => c.Key).ToList());

Part1();
Part2();

void Part1()
{
    var totalAntinodes = new HashSet<Point>();
    foreach (var antennaGroup in dictionaryByChar)
    {
        // get unique pairs of the same antennaes
        var sameAntennaes = antennaGroup.Value;
        var antennaPairs = sameAntennaes.SelectMany(
            (x, i) => sameAntennaes.Skip(i + 1),
            (x, y) => (x, y)
        );

        foreach (var (x, y) in antennaPairs)
        {
            var rowDiff = x.Row - y.Row;
            var colDiff = x.Col - y.Col;
            var firstAntinode = new Point(x.Row + rowDiff, x.Col + colDiff);
            var secondAntinode = new Point(y.Row - rowDiff, y.Col - colDiff);
            if (grid.ContainsKey(firstAntinode))
            {
                totalAntinodes.Add(firstAntinode);
            }
            
            if (grid.ContainsKey(secondAntinode))
            {
                totalAntinodes.Add(secondAntinode);
            }
        }
    }
    
    Console.WriteLine(totalAntinodes.Count);
}

void Part2()
{
    var totalAntinodes = new HashSet<Point>();
    foreach (var antennaGroup in dictionaryByChar)
    {
        // get unique pairs of the same antennaes
        var sameAntennaes = antennaGroup.Value;
        var antennaPairs = sameAntennaes.SelectMany(
            (x, i) => sameAntennaes.Skip(i + 1),
            (x, y) => (x, y)
        );

        foreach (var (x, y) in antennaPairs)
        {
            var rowDiff = x.Row - y.Row;
            var colDiff = x.Col - y.Col;
            totalAntinodes.Add(x);
            totalAntinodes.Add(y);

            foreach (var antinode in GenerateAntinodes(x, rowDiff, colDiff))
            {
                totalAntinodes.Add(antinode);
            }
        }
    }
    
    Console.WriteLine(totalAntinodes.Count);
}

IEnumerable<Point> GenerateAntinodes(Point start, int rowDiff, int colDiff)
{
    var current1 = new Point(start.Row + rowDiff, start.Col + colDiff);
    while (grid.ContainsKey(current1))
    {
        yield return current1;
        current1 = new Point(current1.Row + rowDiff, current1.Col + colDiff);
    }
    
    var current2 = new Point(start.Row - rowDiff, start.Col - colDiff);
    while (grid.ContainsKey(current2))
    {
        yield return current2;
        current2 = new Point(current2.Row - rowDiff, current2.Col - colDiff);
    }
}

