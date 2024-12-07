namespace common;

public static class Utils
{
    public static Dictionary<Point, char> ReadFile(string filename)
    {
        var lines = File.ReadAllLines(filename);

        var grid = new Dictionary<Point, char>();
        for (int i = 0; i < lines.Length; i++)
        {
            for (var j = 0; j < lines[i].Length; j++)
            {
                grid[new Point(i, j)] = lines[i][j];
            }
        }

        return grid;
    }
}