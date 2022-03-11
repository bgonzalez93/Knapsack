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

            UpdateItemPosition(item, position);

            foreach (var rotationType in RotationTypes.Instances) // We're going to rotate the item in all six
            {                                                            // directions to see if we can make it fit...
                item.RotationType = rotationType;

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
                    UpdateItemPosition(item, validItemPosition);
                }

                return fit;
            }

            if (!fit)
            {
                UpdateItemPosition(item, validItemPosition);
            }

            return fit;
        }

        public override string ToString() =>
            $"{Name}({Width}x{Length}x{Height}) vol({Volume})";

        private void UpdateItemPosition(Item item, Position itemPosition)
        {
            item.Position = itemPosition;

            //item.CoordX = itemPosition.Y;
            //item.CoordY = itemPosition.Z;
            //item.CoordZ = itemPosition.X;

            //SetDimensionalData(item, itemPosition);
        }

        private static void SetDimensionalData(Item item, Position itemPosition)
        {
            switch (item.RotationType)
            {
                case RotationType.WidthHeightDepth:
                    item.PackDimX = item.Width;
                    item.PackDimY = item.Height;
                    item.PackDimZ = item.Length;
                    
                    return;

                case RotationType.HeightWidthDepth:
                    item.PackDimX = item.Height;
                    item.PackDimY = item.Width;
                    item.PackDimZ = item.Length;

                    return;

                case RotationType.HeightDepthWidth:
                    item.PackDimX = item.Height;
                    item.PackDimY = item.Length;
                    item.PackDimZ = item.Width;

                    return;

                case RotationType.DepthHeightWidth:
                    item.PackDimX = item.Length;
                    item.PackDimY = item.Height;
                    item.PackDimZ = item.Width;

                    return;

                case RotationType.DepthWidthHeight:
                    item.PackDimX = item.Length;
                    item.PackDimY = item.Width;
                    item.PackDimZ = item.Height;

                    return;

                case RotationType.WidthDepthHeight:
                    item.PackDimX = item.Width;
                    item.PackDimY = item.Length;
                    item.PackDimZ = item.Height;

                    return;

                default:
                    item.PackDimX = 0;
                    item.PackDimY = 0;
                    item.PackDimZ = 0;

                    return;
            }
        }
    }
}