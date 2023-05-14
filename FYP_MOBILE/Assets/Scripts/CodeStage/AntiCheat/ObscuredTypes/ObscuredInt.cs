using System;
using UnityEngine;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	[Serializable]
	public struct ObscuredInt
	{
		[SerializeField]
		private int currentCryptoKey;

		[SerializeField]
		private int hiddenValue;

		[SerializeField]
		private int fakeValue;

		[SerializeField]
		private bool inited;

		private ObscuredInt(int value)
		{
			this = default(ObscuredInt);
		}
	}
}
