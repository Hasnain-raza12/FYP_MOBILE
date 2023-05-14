using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

namespace CodeStage.AntiCheat.Examples
{
	public class ActTesterGui : MonoBehaviour
	{
		public string regularString;

		public int regularInt;

		public float regularFloat;

		public Vector3 regularVector3;

		public ObscuredString obscuredString;

		public ObscuredInt obscuredInt;

		public ObscuredFloat obscuredFloat;

		public ObscuredVector3 obscuredVector3;

		public ObscuredBool obscuredBool;

		public ObscuredLong obscuredLong;

		public ObscuredDouble obscuredDouble;

		public ObscuredVector2 obscuredVector2;

		public string prefsEncryptionKey;
	}
}
