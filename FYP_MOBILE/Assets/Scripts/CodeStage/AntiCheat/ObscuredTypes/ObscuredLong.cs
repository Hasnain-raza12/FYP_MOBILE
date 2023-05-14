using System;
using UnityEngine;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	[Serializable]
	public struct ObscuredLong
	{
		[SerializeField]
		private long currentCryptoKey;

		[SerializeField]
		private long hiddenValue;

		[SerializeField]
		private long fakeValue;

		[SerializeField]
		private bool inited;

		private ObscuredLong(long value)
		{
			this = default(ObscuredLong);
		}
	}
}
