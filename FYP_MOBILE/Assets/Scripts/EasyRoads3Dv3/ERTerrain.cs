using System.Collections.Generic;
using UnityEngine;

namespace EasyRoads3Dv3
{
	public class ERTerrain : MonoBehaviour
	{
		public List<Vector3> surfacevecs;

		public float[] tdataFloat;

		public TerrainData terrainData;

		public int xStart;

		public int zStart;

		public GameObject roadSurface;

		public Mesh surfaceMesh;

		public MeshCollider surfaceCollider;

		public List<ERTerrainData> terrainDataStored;

		public List<ERTree> terrainTrees;

		public List<tPoint> detailInstances;

		public List<int> detailInstanceStarts;

		public List<GameObject> surfaceObjects;

		public List<Vector3> terrainTestPoints;

		public List<ERSplatmap> splatData;

		public List<ERTreeInstance> addedTrees;

		public bool terrainDone;
	}
}
