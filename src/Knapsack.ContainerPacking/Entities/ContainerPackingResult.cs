using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Knapsack.ContainerPacking.Entities
{
	/// <summary>
	/// The container packing result.
	/// </summary>
	[DataContract]
	public class ContainerPackingResult
	{
		#region Constructors

		public ContainerPackingResult()
		{
			this.AlgorithmPackingResults = new List<AlgorithmPackingResult>();
		}

		#endregion Constructors

		#region Public Properties

		/// <summary>
		/// Gets or sets the container ID.
		/// </summary>
		/// <value>
		/// The container ID.
		/// </value>
		[DataMember]
		public string ContainerID { get; set; }

		[DataMember]
		public List<AlgorithmPackingResult> AlgorithmPackingResults { get; set; }

		#endregion Public Properties
	}
}