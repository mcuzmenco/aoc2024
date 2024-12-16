using common;

public class Game
{
    private readonly Dictionary<Point, char> _grid;
    private readonly Queue<Command> _commands;

    const char CharacterChar = '@';
    const char WallChar = '#';
    const char BoxChar = 'O';
    const char FreeSpace = '.';

    public Game(Dictionary<Point, char> grid, Queue<Command> commands)
    {
        _grid = grid;
        _commands = commands;
    }

    public void RunPart1()
    {
        var currentPosition = _grid.Single(x => x.Value == CharacterChar).Key;
        while (_commands.TryDequeue(out var command))
        {
            var pointToMoveTo = GetPointToMoveTo(currentPosition, command);
            if (Move(currentPosition, pointToMoveTo, command))
            {
                _grid[pointToMoveTo] = CharacterChar;
                _grid[currentPosition] = '.';
                currentPosition = pointToMoveTo;
            }
            // GridPrinter.PrintGrid(_grid);
        }

        var total = 0;
        foreach (var cell in _grid)
        {
            if (cell.Value == BoxChar)
            {
                total += cell.Key.Row * 100 + cell.Key.Col;
            }
        }

        Console.WriteLine(total);
    }

    public void RunPart2()
    {
        var currentPosition = _grid.Single(x => x.Value == CharacterChar).Key;
        while (_commands.TryDequeue(out var command))
        {
            // Console.WriteLine($"Grid Before Command: {command}");
            //GridPrinter.PrintGrid(_grid);
            var pointToMoveTo = GetPointToMoveTo(currentPosition, command);
            if (Move2(currentPosition, command))
            {
                _grid[pointToMoveTo] = CharacterChar;
                _grid[currentPosition] = '.';
                currentPosition = pointToMoveTo;
            }
        }
        
        GridPrinter.PrintGrid(_grid);

        var total = 0;
        foreach (var cell in _grid)
        {
            if (cell.Value == '[')
            {
                total += cell.Key.Row * 100 + cell.Key.Col;
            }
        }

        Console.WriteLine(total);
    }

    bool Move(Point fromPosition, Point toPosition, Command command)
    {
        if (_grid[toPosition] == WallChar)
        {
            return false;
        }

        if (_grid[toPosition] == FreeSpace)
        {
            _grid[toPosition] = _grid[fromPosition];
            return true;
        }

        // box char
        var nextToPosition = GetPointToMoveTo(toPosition, command);
        if (Move(toPosition, nextToPosition, command))
        {
            _grid[toPosition] = _grid[fromPosition];
            return true;
        }

        return false;
    }

    bool Move2(Point position, Command command)
    {
        if (CanMove(position, command))
        {
            var nextPosition = GetPointToMoveTo(position, command);
            if (IsBox(nextPosition))
            {
                var box = GetBox(nextPosition);
                MoveBox(box, command);
            }
            
            return true;
        }

        return false;
    }

    bool IsBox(Point position)
    {
        return _grid[position] == '[' || _grid[position] == ']';
    }

    void MoveBox((Point part1, Point part2) box, Command command)
    {
        var nextPositionForPart1 = GetPointToMoveTo(box.part1, command);
        var nextPositionForPart2 = GetPointToMoveTo(box.part2, command);

        if (_grid[nextPositionForPart1] == WallChar || _grid[nextPositionForPart2] == WallChar)
        {
            return;
        }

        if (command == Command.MoveLeft && _grid[nextPositionForPart1] == FreeSpace)
        {
            _grid[nextPositionForPart1] = '[';
            _grid[nextPositionForPart2] = ']';
            _grid[box.part2] = '.';
            return;
        }

        if (command == Command.MoveRight && _grid[nextPositionForPart2] == FreeSpace)
        {
            _grid[nextPositionForPart1] = '[';
            _grid[nextPositionForPart2] = ']';
            _grid[box.part1] = '.';
            return;
        }

        if (_grid[nextPositionForPart1] == FreeSpace && _grid[nextPositionForPart2] == FreeSpace)
        {
            _grid[nextPositionForPart1] = '[';
            _grid[nextPositionForPart2] = ']';
            _grid[box.part1] = '.';
            _grid[box.part2] = '.';
            return;
        }

        if (command == Command.MoveLeft)
        {
            var (left, right) = GetBox(nextPositionForPart1);
            MoveBox((left, right), command);
            MoveBox(box, command);
        } else if (command == Command.MoveRight)
        {
            var (left, right) = GetBox(nextPositionForPart2);
            MoveBox((left, right), command);
            MoveBox(box, command);
        }
        else
        {
            Point[] nextPositions = [nextPositionForPart1, nextPositionForPart2];
            foreach (var nexPositionForPart in nextPositions)
            {
                if(IsBox(nexPositionForPart))
                {
                    var (leftPartBox, rightPartBox) = GetBox(nexPositionForPart);
                    MoveBox((leftPartBox, rightPartBox), command);
                }
            }
            
            MoveBox(box, command);
        }
    }

    bool CanMove(Point p, Command command)
    {
        var nextPosition = GetPointToMoveTo(p, command);

        if (_grid[nextPosition] == WallChar)
        {
            return false;
        }

        if (_grid[nextPosition] == FreeSpace)
        {
            return true;
        }

        var (part1, part2) = GetBox(nextPosition);
        if (command == Command.MoveLeft)
        {
            var partOnTheFarLeft = _grid[part1] == '[' ? part1 : part2;
            return CanMove(partOnTheFarLeft, command);
        }

        if (command == Command.MoveRight)
        {
            var partOnTheFarRight = _grid[part1] == ']' ? part1 : part2;
            return CanMove(partOnTheFarRight, command);
        }

        return CanMove(part1, command) && CanMove(part2, command);
    }

    (Point leftPart, Point rightPart) GetBox(Point p)
    {
        Point firstBoxHalf = p;
        Point otherBoxHalf = _grid[p] == '['
            ? p with { Col = p.Col + 1 }
            : p with { Col = p.Col - 1 };

        var partOnTheFarLeft = _grid[firstBoxHalf] == '[' ? firstBoxHalf : otherBoxHalf;
        var partOnTheFarRight = _grid[firstBoxHalf] == ']' ? firstBoxHalf : otherBoxHalf;

        return (partOnTheFarLeft, partOnTheFarRight);
    }

    Point GetPointToMoveTo(Point position, Command command)
    {
        if (command == Command.MoveDown)
        {
            return position.Bottom();
        }

        if (command == Command.MoveUp)
        {
            return position.Top();
        }

        if (command == Command.MoveLeft)
        {
            return position.Left();
        }

        //  Command.MoveRight
        return position.Right();
    }
}