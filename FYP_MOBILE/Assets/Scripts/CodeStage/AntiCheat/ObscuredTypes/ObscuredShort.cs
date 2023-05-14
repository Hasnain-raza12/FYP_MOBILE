using System;
using System.Runtime.InteropServices;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct ObscuredShort
	{
		private ObscuredShort(short value)
		{
			this = default(ObscuredShort);
		}
	}
}
