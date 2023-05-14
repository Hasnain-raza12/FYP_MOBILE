using System.Collections.Generic;
using UnityEngine;

namespace EasyRoads3Dv3
{
	public class ERCrossingPrefabs : MonoBehaviour
	{
		public List<QDOODOQQDQODD> crossingElements;

		public List<QDOQDSQOOQDDD> sidewalkControlElements;

		public Vector3[] meshVecs;

		public Vector3[] fullMeshVecs;

		public Vector3[] tmpMeshVecs;

		public Vector3[] tmpFullMeshVecs;

		public int[] outerVecInts;

		public List<Vector3> surfaceVecs;

		public List<int> surfaceVecType;

		public List<int> surfaceConnectionInt;

		public List<Vector3> indentVecs;

		public GameObject sourcePrefab;

		public int prefabId;

		public List<int> prioritySegments;

		public float minNodeDistance;

		public int nodeWithinRange;

		public GameObject sourceObject;

		public bool meshInstance;

		public int selectedConnection;

		public string[] QDOOOQOOQQQQD;

		public bool deformTerrain;

		public bool isRoundabout;

		public ERRoundabouts roundaboutScript;

		public bool isCustomPrefab;

		public bool isSceneObject;

		public GameObject surfaceObject;

		public Vector3[] surfaceMeshVecs;

		public int[] surfaceInts;

		public Vector3 leftBottomCorner;

		public Vector3 leftTopCorner;

		public Vector3 rightBottomCorner;

		public Vector3 rightTopCorner;

		public bool tCrossing;

		public int tCrossingLeftRight;

		public Vector3 testVec;

		public List<int> surfaceSurroundingInts;

		public int rotationPriorityElement;

		public Vector3 cornerPos;

		public Vector3 mainCorner;

		public Vector3 connectedCorner;

		public Vector3 mainVecOuter;

		public Vector3 connectionVecOuter;

		public Vector3 indentTopVec;

		public Vector3 indentRightVec;

		public Vector3 mainIndent;

		public Vector3 connectionIndent;

		public int selectedRotationConnection;

		public Vector3 bottomVec;

		public Vector3 rightVec;

		public Vector3 bottomIndent;

		public Vector3 rightIndent;

		public float sAngle;

		public ERModularBase baseScript;

		public Vector3 tp1;

		public Vector3 tp2;

		public bool doTerrainDeformation;

		public bool includeOuterVertices;

		public float surroundingDistance;

		public Mesh surfaceMesh;

		public List<Vector3> debugVecs1;

		public List<Vector3> debugVecs2;
	}
}
