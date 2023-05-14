using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyRoads3Dv3
{
	[Serializable]
	public class QDQDOOQQDQODD
	{
		public string roadTypeName;

		public double id;

		public double timestamp;

		public float roadWidth;

		public float outerIndent;

		public List<Vector2> roadShape;

		public List<float> roadShapeUVs;

		public bool sidewalks;

		public float sidewalkHeight;

		public float sidewalkWidth;

		public Material roadMaterial;

		public Material connectionMaterial;

		public bool isSideObject;

		public List<ERSORoad> soData;

		public List<ERSORoadLog> soDataLog;

		public int layer;

		public bool splatMapActive;

		public int splatIndex;

		public int expandLevel;

		public int smoothLevel;

		public float splatOpacity;

		public QDQDOOQQDQODD(int count)
		{
		}
	}
}
