using System;

namespace VacuumShaders.TerrainToMesh
{
	[Serializable]
	public class TerrainConvertInfo
	{
		public int chunkCountHorizontal;

		public int chunkCountVertical;

		public int vertexCountHorizontal;

		public int vertexCountVertical;

		public bool generateUV;

		public bool generateUV2;

		public bool generateNormal;

		public bool generateTangent;
	}
}
