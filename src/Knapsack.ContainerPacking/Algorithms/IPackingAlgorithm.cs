using System.Collections.Generic;
using Knapsack.ContainerPacking.Entities;

namespace Knapsack.ContainerPacking.Algorithms
{
	/// <summary>
	/// Interface for the packing algorithms in this project.
	/// </summary>
	public interface IPackingAlgorithm
	{
		/// <summary>
		/// Runs the algorithm on the specified container and items.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="items">The items to pack.</param>
		/// <returns>The algorithm packing result.</returns>
		AlgorithmPackingResult Run(Bin container, List<Item> items, bool shouldSortBothDirections = true);
    }
}