using System;
using UnityEngine;

namespace EasyRoads3Dv3
{
	[Serializable]
	public class ERTree
	{
		public Color color;

		public float heightScale;

		public Color lightmapColor;

		public Vector3 position;

		public int prototypeIndex;

		public float widthScale;

		public ERTree(TreeInstance instance)
		{
		}
	}
}
