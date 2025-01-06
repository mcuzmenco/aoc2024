namespace common;

public record struct Point(int Row, int Col)
{
    public bool IsAdjacentHorizontally(Point otherP)
    {
        return Row == otherP.Row && Math.Abs(Col - otherP.Col) == 1;
    }

    public Point Right()
    {
        return new Point(Row, Col + 1);
    }
    
    public Point Left()
    {
        return new Point(Row, Col - 1);
    }
    
    public Point Top()
    {
        return new Point(Row - 1, Col);
    }
    
    public Point Bottom()
    {
        return new Point(Row + 1, Col);
    }

    public Point UpRight()
    {
        return new Point(Row - 1, Col + 1);
    }
    
    public Point UpLeft()
    {
        return new Point(Row - 1, Col -1);
    }
    
    public Point BottomLeft()
    {
        return new Point(Row + 1, Col - 1);
    }
    
    public Point BottomRight()
    {
        return new Point(Row + 1, Col +1);
    }

    public IEnumerable<Point> Adjacent(Predicate<Point>? predicate = null)
    {
        predicate ??= (_ => true);
        
        var p = Top();
        if (predicate(p))
        {
            yield return p;
        }
        
        p = Left();
        if (predicate(p))
        {
            yield return p;
        }
        
        p = Right();
        if (predicate(p))
        {
            yield return p;
        }
        
        p = Bottom();
        if (predicate(p))
        {
            yield return p;
        }
    }
};