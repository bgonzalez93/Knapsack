using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Knapsack.ContainerPacking.Algorithms;
using Knapsack.ContainerPacking.Entities;

namespace Knapsack.ContainerPacking
{
    /// <summary>
    /// The container packing service.
    /// </summary>
    public static class PackingService
    {
        /// <summary>
        /// Attempts to pack the specified containers with the specified items using the specified algorithms.
        /// </summary>
        /// <param name="containers">The list of containers to pack.</param>
        /// <param name="itemsToPack">The items to pack.</param>
        /// <param name="algorithmTypeIDs">The list of algorithm type IDs to use for packing.</param>
        /// <returns>A container packing result with lists of the packed and unpacked items.</returns>
        public static List<ContainerPackingResult> Pack(List<Bin> containers, List<Item> itemsToPack, List<int> algorithmTypeIDs)
        {
            var sync = new object { };
            var result = new List<ContainerPackingResult>();

            Parallel.ForEach(containers.OrderBy(x => x.Volume), container =>
            {
                var containerPackingResult = new ContainerPackingResult();
                containerPackingResult.ContainerID = container.ID;

                Parallel.ForEach(algorithmTypeIDs, algorithmTypeID =>
                {
                    var algorithm = GetPackingAlgorithmFromTypeID(algorithmTypeID);

                    // Until I rewrite the algorithm with no side effects, we need to clone the item list
                    // so the parallel updates don't interfere with each other.
                    var items = new List<Item>();

                    itemsToPack.ForEach(item =>
                    {
                        items.Add(new Item(item.ID, item.Length, item.Width, item.Height, item.Quantity));
                    });

                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    var algorithmResult = algorithm.Run(container, items);
                    stopwatch.Stop();

                    algorithmResult.PackTimeInMilliseconds = stopwatch.ElapsedMilliseconds;

                    var containerVolume = container.Length * container.Width * container.Height;
                    var itemVolumePacked = algorithmResult.PackedItems.Sum(i => i.Volume);
                    var itemVolumeUnpacked = algorithmResult.UnpackedItems.Sum(i => i.Volume);

                    algorithmResult.PercentContainerVolumePacked = Math.Round(itemVolumePacked / containerVolume * 100, 2);
                    algorithmResult.PercentItemVolumePacked = Math.Round(itemVolumePacked / (itemVolumePacked + itemVolumeUnpacked) * 100, 2);

                    lock (sync)
                    {
                        containerPackingResult.AlgorithmPackingResults.Add(algorithmResult);
                    }
                });

                containerPackingResult.AlgorithmPackingResults = containerPackingResult.AlgorithmPackingResults.OrderBy(r => r.AlgorithmName).ToList();

                lock (sync)
                {
                    result.Add(containerPackingResult);
                }
            });
			
            return result;
        }

        /// <summary>
        /// Gets the packing algorithm from the specified algorithm type ID.
        /// </summary>
        /// <param name="algorithmTypeID">The algorithm type ID.</param>
        /// <returns>An instance of a packing algorithm implementing AlgorithmBase.</returns>
        /// <exception cref="System.Exception">Invalid algorithm type.</exception>
        public static IPackingAlgorithm GetPackingAlgorithmFromTypeID(int algorithmTypeID)
        {
            switch (algorithmTypeID)
            {
                case (int)AlgorithmType.EB_AFIT:
                    return new EB_AFIT();

                case (int)AlgorithmType.Sharp3dPacking:
                    return new Sharp3DPacking();

                default:
                    throw new Exception("Invalid algorithm type.");
            }
        }
    }
}