using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyRoads3Dv3
{
	[Serializable]
	public class ERMesh
	{
		public List<int> vecsInt;

		public List<Vector3> vecs;

		public List<Vector2> uv;

		public List<Vector2> uv2;

		public List<Color> colors;

		public List<Vector3> normals;

		public List<Vector4> tangents;

		public List<int> triangles;

		public List<int> startVecsInt;

		public List<Vector3> startVecs;

		public List<Vector2> startUv;

		public List<Vector2> startUv2;

		public List<Color> startColors;

		public List<Vector3> startNormals;

		public List<Vector4> startTangents;

		public List<int> startTriangles;

		public List<int> endVecsInt;

		public List<Vector3> endVecs;

		public List<Vector2> endUv;

		public List<Vector2> endUv2;

		public List<Color> endColors;

		public List<Vector3> endNormals;

		public List<Vector4> endTangents;

		public List<int> endTriangles;

		public List<Material> materials;

		public List<Vector3> sVecs;

		public List<Vector2> sUv;

		public List<Vector2> sUv2;

		public List<Color> sColors;

		public List<Vector3> sNormals;

		public List<Vector4> sTangents;

		public List<int> sTriangles;

		public List<Vector3> sStartVecs;

		public List<Vector2> sStartUv;

		public List<Vector2> sStartUv2;

		public List<Color> sStartColors;

		public List<Vector3> sStartNormals;

		public List<Vector4> sStartTangents;

		public List<int> sStartTriangles;

		public List<Vector3> sEndVecs;

		public List<Vector2> sEndUv;

		public List<Vector2> sEndUv2;

		public List<Color> sEndColors;

		public List<Vector3> sEndNormals;

		public List<Vector4> sEndTangents;

		public List<int> sEndTriangles;

		public int startEndVecCount;

		public int middleStartVecCount;

		public int middleEndVecCount;

		public int endStartVecCount;

		public List<Vector3> middleEndVecs;

		public List<int> startEndInts;

		public List<int> middleStartInts;

		public List<int> middleEndInts;

		public List<int> middleStartStartInts;

		public List<int> middleEndEndInts;

		public List<int> endStartInts;

		public List<int> startEndIntsNC;

		public List<int> middleStartStartIntsNC;

		public List<int> middleStartIntsNC;

		public List<int> middleEndIntsNC;

		public List<int> middleEndEndIntsNC;

		public List<int> endStartIntsNC;

		public int OCDCQQCOODInt;

		public int ODODOCOODDInt;

		public int middleLeftInt;

		public int middleRightInt;

		public int endLeftInt;

		public int endRightInt;

		public List<int> normalArray1;

		public List<int> normalArray2;

		public int vecCount;

		public List<float> zValues;

		public List<ZIndexArray> zValueVecIndexes;

		public List<float> zValuesStart;

		public List<ZIndexArray> zValueVecIndexesStart;

		public List<float> zValuesEnd;

		public List<ZIndexArray> zValueVecIndexesEnd;

		public float minZ;

		public float minMiddleZ;

		public float maxZ;

		public float maxMiddleZ;

		public float totalZDistance;

		public ERMesh(GameObject m_go, SideObject soScript, float minZ)
		{
		}
	}
}
