using System;
using System.Runtime.InteropServices;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct ObscuredUShort
	{
		private ObscuredUShort(ushort value)
		{
			this = default(ObscuredUShort);
		}
	}
}
