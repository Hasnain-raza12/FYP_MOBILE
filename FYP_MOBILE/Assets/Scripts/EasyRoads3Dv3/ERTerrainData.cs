using System;
using UnityEngine;

namespace EasyRoads3Dv3
{
	[Serializable]
	public class ERTerrainData
	{
		public int terrainWidth;

		public int terrainHeight;

		public float originalHeight;

		public float flattenedHeight;

		public float outerHeightDifference;

		public bool critical;

		public float perc;

		public Vector3 hitpos;

		public Vector3 outerPos;

		public ERTerrainData(int m_terrainWidth, int m_terrainHeight, float m_originalHeight, float m_flattenedHeight, bool m_critical, float m_perc, float m_outerHeight, Vector3 m_hitPoint, Vector3 m_outerPoint)
		{
		}
	}
}
