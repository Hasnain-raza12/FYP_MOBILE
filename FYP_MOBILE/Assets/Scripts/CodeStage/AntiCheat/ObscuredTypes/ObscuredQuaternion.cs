using System;
using UnityEngine;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	[Serializable]
	public struct ObscuredQuaternion
	{
		[Serializable]
		public struct RawEncryptedQuaternion
		{
			public int x;

			public int y;

			public int z;

			public int w;
		}

		[SerializeField]
		private int currentCryptoKey;

		[SerializeField]
		private RawEncryptedQuaternion hiddenValue;

		[SerializeField]
		private Quaternion fakeValue;

		[SerializeField]
		private bool inited;

		private ObscuredQuaternion(RawEncryptedQuaternion value)
		{
			this = default(ObscuredQuaternion);
		}
	}
}
