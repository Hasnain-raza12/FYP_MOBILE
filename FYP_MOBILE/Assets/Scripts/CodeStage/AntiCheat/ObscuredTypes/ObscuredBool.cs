using System;
using UnityEngine;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	[Serializable]
	public struct ObscuredBool
	{
		[SerializeField]
		private byte currentCryptoKey;

		[SerializeField]
		private int hiddenValue;

		[SerializeField]
		private bool fakeValue;

		[SerializeField]
		private bool fakeValueChanged;

		[SerializeField]
		private bool inited;

		private ObscuredBool(int value)
		{
			this = default(ObscuredBool);
		}
	}
}
