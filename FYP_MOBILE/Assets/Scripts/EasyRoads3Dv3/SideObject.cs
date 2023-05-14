using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyRoads3Dv3
{
	[Serializable]
	public class SideObject : ScriptableObject
	{
		public string version;

		public new string name;

		public double id;

		public double timestamp;

		public int objectType;

		public string gameobjectGUID;

		public string textureGUID;

		public float m_distance;

		public float uvx;

		public float uvy;

		public int position;

		public float splinePosition;

		public int selectedRotation;

		public List<Vector2> nodeList;

		public List<float> uvDistances;

		public float totalDistance;

		public bool reverseUVs;

		public List<bool> snapList;

		public List<Color> colorList;

		public string gameobjectStartGUID;

		public string gameobjectEndGUID;

		public int align;

		public bool weld;

		public bool combine;

		public bool combineInstantiated;

		public bool markerActive;

		public int uvType;

		public float uv;

		public bool randomObjects;

		public float sidewaysOffset;

		public float density;

		public string goPath;

		public string startPath;

		public string endPath;

		public string texturePath;

		public int terrainTree;

		public float minScale;

		public float maxScale;

		public bool childOrderActive;

		public int childOrder;

		public bool meshBoundsAlignment;

		public float xPosition;

		public int relativeTo;

		public float yPosition;

		public float yRotation;

		public float oldSidwaysDistance;

		public int sidewaysDistanceUpdate;

		public float uvYRound;

		public bool adjustUV;

		public bool collider;

		public bool tangents;

		public GameObject sourceObject;

		public bool flipMesh;

		public GameObject endObject;

		public GameObject connectionObject;

		public Material material;

		public List<ERMesh> meshObjects;

		public bool includeStartSegment;

		public float startSegmentOffset;

		public bool includeStartEdgeTris;

		public bool includeEndSegment;

		public float endSegmentOffset;

		public bool includeEndEdgeTris;

		public bool adjustToRoadWidth;

		public float xOffset;

		public float startOffset;

		public float endOffset;

		public float totalZDistance;

		public float middleZDistance;

		public float startZDistance;

		public float endZDistance;

		public float minStartZ;

		public float maxStartZ;

		public float minMiddleZ;

		public float maxMiddleZ;

		public float minEndZ;

		public float maxEndZ;

		public bool smoothStart;

		public bool smoothMiddle;

		public bool smoothEnd;

		public GameObject targetObject;

		public bool bridgeObject;

		public bool snapToTerrain;

		public int layer;

		public bool deformationObject;

		public bool isStatic;

		public bool doTestmesh;

		public Vector3 testMeshPos;
	}
}
