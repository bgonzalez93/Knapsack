using System.Runtime.Serialization;

namespace Algorithms
{
	[DataContract]
	public enum AlgorithmType
	{
		[DataMember]
		EB_AFIT = 1,
        [DataMember]
        Sharp3dPacking = 2
	}
}