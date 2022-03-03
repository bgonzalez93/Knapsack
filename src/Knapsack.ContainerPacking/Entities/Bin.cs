using System;
using System.Collections.Generic;

namespace Knapsack.ContainerPacking.Entities
{
    public class Bin : Container
    {
        public string Name { get; }

        public List<Item> Items { get; } = new List<Item>();

        public List<Item> UnfittedItems { get; } = new List<Item>();

        public Bin(string name, decimal width, decimal height, decimal depth) : base(name, depth, width, height)
        {
            Name = name;
        }

        public bool PutItem(Item item, Position position)
        {
            var fit = false;
            var validItemPosition = item.Position;

            item.Position = position;

            foreach (var rotationType in Enum.GetValues(typeof(RotationType)))
            {
                item.RotationType = (RotationType)rotationType;

                item.RotatedDimensions(out var width, out var height, out var depth);

                if (Width < position.X + width || Height < position.Y + height || Length < position.Z + depth)
                {
                    continue;
                }

                fit = true;

                foreach (var currentItemInBin in Items)
                {
                    // Check to see if any items currently in the bin would intersect with the 
                    // item that we're currently looking at.
                    if (currentItemInBin.IntersectsWith(item))
                    {
                        fit = false;

                        break;
                    }
                }

                if (fit)
                {
                    Items.Add(item);
                }

                if (!fit)
                {
                    item.Position = validItemPosition;
                }

                return fit;
            }

            if (!fit)
            {
                item.Position = validItemPosition;
            }

            return fit;
        }

        public override string ToString() =>
            $"{Name}({Width}x{Length}x{Height}) vol({Volume})";
    }
}