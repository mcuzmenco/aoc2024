using common;

public static class GridPrinter
{
    public static void PrintGrid(Dictionary<Point, char> gridToPrint)
    {
        var maxRow = gridToPrint.Keys.Max(x => x.Row);
        var maxCol = gridToPrint.Keys.Max(x => x.Col);

        for (int i = 0; i <= maxRow; i++)
        {
            for (int j = 0; j <= maxCol; j++)
            {
                Console.Write(gridToPrint[new Point(i, j)]);
            }
        
            Console.Write("\r\n");
        }
    }
}