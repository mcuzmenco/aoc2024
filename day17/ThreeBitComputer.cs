namespace day17;

public class ThreeBitComputer
{
    // Registers
    private long A { get; set; }
    private long B { get; set; }
    private long C { get; set; }
    
    readonly int[] _program;

    // Instruction pointer
    int _currentInstruction;

    List<long> _outputBuffer = new List<long>();

    public ThreeBitComputer(long initialA, long initialB, long initialC, int[] program)
    {
        A = initialA;
        B = initialB;
        C = initialC;
        _program = program;
        _currentInstruction = 0;
    }

    public string Run()
    {
        while (true)
        {
            if (_currentInstruction < 0 || _currentInstruction >= _program.Length)
            {
                break;
            }

            int opcode = _program[_currentInstruction];
            if (_currentInstruction + 1 >= _program.Length)
            {
                break;
            }
            int operand = _program[_currentInstruction + 1];

            switch (opcode)
            {
                case 0: // adv: A = floor(A / (2^(combo)))
                    {
                        long denom = (long)Math.Pow(2, ResolveComboOperand(operand));
                        A = A / denom;
                        _currentInstruction += 2;
                        break;
                    }
                case 1: // bxl: B = B XOR operand (literal)
                    {
                        long val = operand; // literal operand
                        B = B ^ val;
                        _currentInstruction += 2;
                        break;
                    }
                case 2: // bst: B = (combo_operand_value % 8)
                    {
                        long val = ResolveComboOperand(operand) % 8;
                        B = val;
                        _currentInstruction += 2;
                        break;
                    }
                case 3: // jnz: if A != 0 then ip = literal_operand else ip += 2
                    {
                        long val = operand; // literal operand
                        if (A != 0)
                        {
                            _currentInstruction = (int)val; // jump
                        }
                        else
                        {
                            _currentInstruction += 2;
                        }
                        break;
                    }
                case 4: // bxc: B = B XOR C (operand ignored)
                    {
                        B = B ^ C;
                        _currentInstruction += 2;
                        break;
                    }
                case 5: // out: output (combo_operand_value % 8)
                    {
                        Console.WriteLine($"A: {A}");
                        Console.WriteLine($"ip: {_currentInstruction}");
                        long val = ResolveComboOperand(operand) % 8;
                        Console.WriteLine($"operand: {operand}");
                        Console.WriteLine($"resolveOperand: {ResolveComboOperand(operand)}");
                        Console.WriteLine($"val: {val}");
                        _outputBuffer.Add(val);
                        _currentInstruction += 2;
                        break;
                    }
                case 6: // bdv: B = floor(A / (2^(combo)))
                    {
                        long denom = (long)Math.Pow(2, ResolveComboOperand(operand));
                        B = A / denom;
                        _currentInstruction += 2;
                        break;
                    }
                case 7: // cdv: C = floor(A / (2^(combo)))
                    {
                        long denom = (long)Math.Pow(2, ResolveComboOperand(operand));
                        C = A / denom;
                        _currentInstruction += 2;
                        break;
                    }
                default:
                    // Invalid opcode, halt
                    return string.Join(",", _outputBuffer);
            }
        }

        return string.Join(",", _outputBuffer);
    }

    private long ResolveComboOperand(int operand)
    {
        // combo operands 0-3 are literal values 0-3
        // combo operand 4 = A
        // combo operand 5 = B
        // combo operand 6 = C
        switch (operand)
        {
            case 0: return 0;
            case 1: return 1;
            case 2: return 2;
            case 3: return 3;
            case 4: return A;
            case 5: return B;
            case 6: return C;
            default:
                throw new InvalidOperationException();
        }
    }
}