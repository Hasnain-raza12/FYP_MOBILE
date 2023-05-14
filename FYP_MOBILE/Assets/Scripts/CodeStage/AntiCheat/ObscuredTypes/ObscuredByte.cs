using System;
using System.Runtime.InteropServices;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct ObscuredByte
	{
		private ObscuredByte(byte value)
		{
			this = default(ObscuredByte);
		}
	}
}
