using System.Collections;
using System.Collections.Generic;

namespace MergeBoard.Data
{
	public enum BoardType
	{
		None = 0,
		
		Merge,
	}

	public enum ConsumeType
	{
		None = 0,

		Gold,
		Energy,
	}

	public enum RewardType
	{
		None = 0,
		
		Exp,
		Energy,
		Gold,
	}

	public enum PopType
	{
		None = 0,

		FromPopItem,
		FromRandomBox,
	}
}