using day17;

var inputdata = Parse("real");

var computer = new ThreeBitComputer(inputdata.A, inputdata.B, inputdata.C, inputdata.program);
string output = computer.Run();
Console.WriteLine(output.Replace(",", ""));
//Console.WriteLine($"Initial A: {ComputeInitialA(inputdata.program) *8}");

(long A, long B, long C, int[]? program) Parse(string fileName)
{
    string[] lines = File.ReadAllLines($"{fileName}.txt");

    long a = 0, b = 0, c = 0;
    int[]? program = null;

    foreach (string line in lines)
    {
        var trimmed = line.Trim();
        if (trimmed.StartsWith("Register A:"))
        {
            a = long.Parse(trimmed.Substring("Register A:".Length).Trim());
        }
        else if (trimmed.StartsWith("Register B:"))
        {
            b = long.Parse(trimmed.Substring("Register B:".Length).Trim());
        }
        else if (trimmed.StartsWith("Register C:"))
        {
            c = long.Parse(trimmed.Substring("Register C:".Length).Trim());
        }
        else if (trimmed.StartsWith("Program:"))
        {
            string progStr = trimmed.Substring("Program:".Length).Trim();
            program = progStr.Split(',')
                .Select(x => int.Parse(x))
                .ToArray();
        }
    }

    return (a, b, c, program);
}