namespace Entities
{
    public class Position
    {
        public static readonly Position StartingPosition = new Position(0, 0, 0);

        public decimal X { get; }
        public decimal Y { get; }
        public decimal Z { get; }

        public Position(decimal x, decimal y, decimal z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString() => $"x:{X},y:{Y},z:{Z}";
    }
}