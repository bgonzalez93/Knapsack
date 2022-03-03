using System;
using System.Collections.Generic;
using System.Linq;
using Entities;

namespace Algorithms
{
	/// <summary>
	/// A 3D bin packing algorithm originally ported from https://github.com/keremdemirer/3dbinpackingjs,
	/// which itself was a JavaScript port of https://github.com/wknechtel/3d-bin-pack/, which is a C reconstruction 
	/// of a novel algorithm developed in a U.S. Air Force master's thesis by Erhan Baltacioglu in 2001.
	/// </summary>
	public class Sharp3DPacking : IPackingAlgorithm
	{
        private Bin _bin;
        private readonly List<Item> _items = new List<Item>();

        public AlgorithmPackingResult Run(Bin container, List<Item> items, bool biggerFirst = false)
        {
            _bin = container;

            SetGlobalItems(items);

            var sortedItems = (biggerFirst ? _items.OrderByDescending(x => x.Volume) : _items.OrderBy(x => x.Volume)).ToList();

            foreach (var item in sortedItems)
            {
                PackToBin(_bin, item);
            }

            return new AlgorithmPackingResult
            {
                PackedItems = _bin.Items,
                UnpackedItems = _bin.UnfittedItems,
                AlgorithmID = (int)AlgorithmType.Sharp3dPacking,
                AlgorithmName = AlgorithmType.Sharp3dPacking.ToString()
            };
        }

        private void SetGlobalItems(List<Item> items)
        {
            var id = items.OrderBy(x => x.ID).FirstOrDefault()?.ID ?? 0;

            foreach (var item in items)
            {
                var qty = 1;

                while (qty <= item.Quantity)
                {
                    _items.Add(new Item(id, item.Length, item.Width, item.Height, item.Quantity));
                    qty++;
                    id++;
                }

                item.Quantity = 1;
            }
        }

        private void PackToBin(Bin bin, Item item)
        {
            var fitted = false;

            if (!bin.Items.Any())
            {
                var wasPut = bin.PutItem(item, Position.StartingPosition);

                if (!wasPut)
                {
                    bin.UnfittedItems.Add(item);
                }

                return;
            }

            foreach (var axis in Enum.GetValues(typeof(Axis)))
            {
                var itemsInBin = bin.Items;

                foreach (var itemInBin in itemsInBin)
                {
                    var pivot = itemInBin.RotatePosition((Axis)axis);

                    if (bin.PutItem(item, pivot))
                    {
                        fitted = true;

                        break;
                    }
                }

                if (fitted)
                {
                    break;
                }
            }

            if (!fitted)
            {
                bin.UnfittedItems.Add(item);
            }
        }
    }
}