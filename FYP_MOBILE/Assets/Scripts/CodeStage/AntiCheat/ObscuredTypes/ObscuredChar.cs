using System;
using System.Runtime.InteropServices;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct ObscuredChar
	{
		private ObscuredChar(char value)
		{
			this = default(ObscuredChar);
		}
	}
}
