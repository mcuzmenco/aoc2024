using common;

var name = "real";

var (grid, commands) = ParseInput(name, false);
GridPrinter.PrintGrid(grid);

var game = new Game(grid, commands);
game.RunPart1();

(grid, _) = ParseInput(name, true);
GridPrinter.PrintGrid(grid);

game = new Game(grid, commands);
game.RunPart2();

(Dictionary<Point, char> grid, Queue<Command> commands) ParseInput(string name, bool part2)
{
    var input = File.ReadAllText($"{name}.txt");
    var parts = input.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries);

    var grid = new Dictionary<Point, char>();
    var lines = parts[0].Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
    for (var i = 0; i < lines.Length; i++)
    {
        if (part2)
        {
            lines[i] = lines[i].Replace("#", "##");
            lines[i] = lines[i].Replace("O", "[]");
            lines[i] = lines[i].Replace(".", "..");
            lines[i] = lines[i].Replace("@", "@.");
        }
        for (int j = 0; j < lines[i].Length; j++)
        {
            grid.Add(new Point(i, j), lines[i][j]);
        }
    }

    Queue<Command> commands = new Queue<Command>();
    var rows = parts[1].Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
    foreach (var row in rows)
    {
        foreach (var c in row)
        {
            if (c == 'v')
            {
                commands.Enqueue(Command.MoveDown);
            }
        
            if (c == '>')
            {
                commands.Enqueue(Command.MoveRight);
            }
        
            if (c == '<')
            {
                commands.Enqueue(Command.MoveLeft);
            }
        
            if (c == '^')
            {
                commands.Enqueue(Command.MoveUp);
            }
        }
    }
    
    return (grid, commands);
}