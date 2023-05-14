using System;
using CodeStage.AntiCheat.Common;
using UnityEngine;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	[Serializable]
	public struct ObscuredFloat
	{
		[SerializeField]
		private int currentCryptoKey;

		[SerializeField]
		private ACTkByte4 hiddenValue;

		[SerializeField]
		private byte[] hiddenValueOld;

		[SerializeField]
		private float fakeValue;

		[SerializeField]
		private bool inited;

		private ObscuredFloat(ACTkByte4 value)
		{
			this = default(ObscuredFloat);
		}
	}
}
