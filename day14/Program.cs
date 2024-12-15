string mode = "real";

List<Particle> particles = new List<Particle>();

var input = File.ReadAllText($"{mode}.txt");
// Split the input by new lines to get each row
var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

foreach (var line in lines)
{
    var parts = line.Split(' ');

    string pPart = parts[0].Substring(2); // remove "p="
    string vPart = parts[1].Substring(2); // remove "v="

    var pValues = pPart.Split(',');
    var vValues = vPart.Split(',');

    int px = int.Parse(pValues[0]);
    int py = int.Parse(pValues[1]);

    int vx = int.Parse(vValues[0]);
    int vy = int.Parse(vValues[1]);

    var position = new Position(px, py);
    var velocity = new Velocity(vx, vy);

    // Create the particle instance (or handle them separately if you prefer)
    Particle particle = new Particle(position, velocity);
    particles.Add(particle);
}

int width = 11;
int height = 7;
if (mode != "test")
{
    width = 101;
    height = 103;
}

Part1();

Part2();

void Part1()
{
    var i = 0;
    while (i < 100)
    {
        foreach (var particle in particles)
        {
            particle.Move(width, height);
        }
    
        i++;
    }
    
    Print(particles.ToArray(), width, height);

    var totalCount = particles
        .Where(x => x.GetQuadrant(width, height) != "Q-Middle")
        .GroupBy(x => x.GetQuadrant(width, height)).Aggregate(1, (s, g) => s * g.Count());

    Console.WriteLine(totalCount);
}

void Part2()
{
    var i = 0;
    var maxMiddleQuadrantNumber = 0;
    while (i < 101 * 103)
    {
        foreach (var particle in particles)
        {
            particle.Move(width, height);
        }
        
        var maxNumberOfVerticallyMirroredCells = (from p1 in particles
                from p2 in particles
                where p1.IsMirroredVertically(p2, width, height)
                    select p1).Count();

        if (maxNumberOfVerticallyMirroredCells > maxMiddleQuadrantNumber)
        {
            // just eye-ball printed pics :D
            maxMiddleQuadrantNumber = maxNumberOfVerticallyMirroredCells;
            Console.WriteLine(i + 1);
            Print(particles.ToArray(), width, height);
        }
    
        i++;
    }
    
    Print(particles.ToArray(), width, height);

    var totalCount = particles
        .Where(x => x.GetQuadrant(width, height) != "Q-Middle")
        .GroupBy(x => x.GetQuadrant(width, height)).Aggregate(1, (s, g) => s * g.Count());

    Console.WriteLine(totalCount);
}

void Print(Particle[] particles, int maxWidth, int maxHeight)
{
    Console.Write("\r\n");
    
    for (int j = 0; j < maxHeight; j++)
    {
        for (int i = 0; i < maxWidth; i++)
        {
            var countOfParticles = particles.Count(x => x.Position.X == i && x.Position.Y == j);
            Console.Write(countOfParticles > 0 ? countOfParticles : ".");
        }
        
        Console.Write("\r\n");
        
    }

    Console.Write("\r\n");
}



record Position(int X, int Y);

record Velocity(int X, int Y);

class Particle
{
    public Position Position { get; private set; }
    public Velocity Velocity { get; }

    public Particle(Position position, Velocity velocity)
    {
        Position = position;
        Velocity = velocity;
    }

    public void Move(int maxWidth, int maxHeight)
    {
        var newPositionX = Position.X + Velocity.X;
        if (newPositionX > (maxWidth - 1))
        {
            newPositionX = newPositionX - maxWidth;
        } else if (newPositionX < 0)
        {
            newPositionX = maxWidth + newPositionX;
        }
        
        var newPositionY = Position.Y + Velocity.Y;
        if (newPositionY > (maxHeight - 1))
        {
            newPositionY = newPositionY - maxHeight;
        } else if (newPositionY < 0)
        {
            newPositionY = maxHeight + newPositionY;
        }
        
        Position = new Position(newPositionX, newPositionY);
    }

    public string GetQuadrant(int maxWidth, int maxHeight)
    {
        var midXIndex = maxWidth / 2;
        var midYIndex = maxHeight / 2;
        if (Position.X == midXIndex || Position.Y == midYIndex)
        {
            return "Q-Middle";
        }
        
        if (Position.X < midXIndex && Position.Y < midYIndex)
        {
            return "Q1";
        }
        
        if (Position.X > midXIndex && Position.Y < midYIndex)
        {
            return "Q2";
        }
        
        if (Position.X < midXIndex && Position.Y > midYIndex)
        {
            return "Q3";
        }
        
        if (Position.X > midXIndex && Position.Y > midYIndex)
        {
            return "Q4";
        }

        throw new Exception();
    }
    public bool IsMirroredVertically(Particle otherParticle, int maxWidth, int maxHeight)
    {
        return this.Position.Y == otherParticle.Position.Y && this.Position.X == (maxWidth - otherParticle.Position.X - 1);
    }
}