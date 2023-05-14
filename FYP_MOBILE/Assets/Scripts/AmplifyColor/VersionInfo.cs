using System;
using UnityEngine;

namespace AmplifyColor
{
	[Serializable]
	public class VersionInfo
	{
		public const byte Major = 1;

		public const byte Minor = 5;

		public const byte Release = 1;

		private static string StageSuffix = "_dev007";

		private static string TrialSuffix = string.Empty;

		[SerializeField]
		private int m_major;

		[SerializeField]
		private int m_minor;

		[SerializeField]
		private int m_release;

		public int Number => m_major * 100 + m_minor * 10 + m_release;

		private VersionInfo()
		{
			m_major = 1;
			m_minor = 5;
			m_release = 1;
		}

		private VersionInfo(byte major, byte minor, byte release)
		{
			m_major = major;
			m_minor = minor;
			m_release = release;
		}

		public static string StaticToString()
		{
			return $"{1}.{5}.{1}" + StageSuffix + TrialSuffix;
		}

		public override string ToString()
		{
			return $"{m_major}.{m_minor}.{m_release}" + StageSuffix + TrialSuffix;
		}

		public static VersionInfo Current()
		{
			return new VersionInfo(1, 5, 1);
		}

		public static bool Matches(VersionInfo version)
		{
			if (version.m_major == 1 && version.m_minor == 5)
			{
				return 1 == version.m_release;
			}
			return false;
		}
	}
}
