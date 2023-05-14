using System;
using CodeStage.AntiCheat.Common;
using UnityEngine;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	[Serializable]
	public struct ObscuredDouble
	{
		[SerializeField]
		private long currentCryptoKey;

		[SerializeField]
		private byte[] hiddenValueOld;

		[SerializeField]
		private ACTkByte8 hiddenValue;

		[SerializeField]
		private double fakeValue;

		[SerializeField]
		private bool inited;

		private ObscuredDouble(ACTkByte8 value)
		{
			this = default(ObscuredDouble);
		}
	}
}
