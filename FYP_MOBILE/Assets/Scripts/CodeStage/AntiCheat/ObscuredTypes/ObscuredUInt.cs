using System;
using System.Runtime.InteropServices;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct ObscuredUInt
	{
		private ObscuredUInt(uint value)
		{
			this = default(ObscuredUInt);
		}
	}
}
