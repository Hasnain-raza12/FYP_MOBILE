using System;
using System.Runtime.InteropServices;
using CodeStage.AntiCheat.Common;

namespace CodeStage.AntiCheat.ObscuredTypes
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct ObscuredDecimal
	{
		private ObscuredDecimal(ACTkByte16 value)
		{
			this = default(ObscuredDecimal);
		}
	}
}
