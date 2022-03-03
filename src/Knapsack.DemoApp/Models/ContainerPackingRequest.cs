using System.Collections.Generic;
using Entities;

namespace Knapsack.DemoApp.Models
{
	public class ContainerPackingRequest
	{
		public List<Bin> Containers { get; set; }

		public List<Item> ItemsToPack { get; set; }

		public List<int> AlgorithmTypeIDs { get; set; }
	}
}