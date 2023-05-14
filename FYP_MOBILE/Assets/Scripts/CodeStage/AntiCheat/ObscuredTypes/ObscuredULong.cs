using System;
using System.Runtime.InteropServices;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct ObscuredULong
	{
		private ObscuredULong(ulong value)
		{
			this = default(ObscuredULong);
		}
	}
}
