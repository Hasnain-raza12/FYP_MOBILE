using System.Collections.Generic;
using UnityEngine;

namespace EasyRoads3Dv3
{
	public class ERModularRoad : MonoBehaviour
	{
		public ERModularBase baseScript;

		public string roadName;

		public double roadType;

		public List<ERMarker> markers;

		public List<ERMarker> tmpMarkers;

		public float roadWidth;

		public float faceDistance;

		public bool closedTrack;

		public float minNodeDistance;

		public int nodeWithinRange;

		public int vertsStats;

		public int trisStats;

		public float indent;

		public float surrounding;

		public bool followTerrainContours;

		public float terrainContoursOffset;

		public List<Vector2> roadShape;

		public List<int> roadShapeIntsStart;

		public List<int> roadShapeIntsEnd;

		public string roadShapeString;

		public string roadShapeReversedString;

		public int roadShapeMatchCount;

		public int geoReversed;

		public List<float> nodeDistance;

		public List<float> roadShapeUVs;

		public List<int> roadShapeMaterialInts;

		public int subMeshCount;

		public List<int> roadShapeMaterialIntCounts;

		public List<Vector3> controlPoints;

		public List<Vector3> splinePoints;

		public List<float> distances;

		public List<int> markerInts;

		public List<Vector3> insertSplinePoints;

		public List<Vector3> soSplinePoints;

		public List<Vector3> soSplinePointsLeft;

		public List<Vector3> soSplinePointsRight;

		public List<float> OQOCCOQODQ;

		public List<float> OOCOODQQDQ;

		public List<Vector3> meshVecs;

		public List<Vector2> meshUVs;

		public List<Vector3> surfaceMeshVecs;

		public List<Vector3> vecsBelowTerrain;

		public List<Vector3> treeVecs;

		public List<Vector3> detailVecs;

		public List<int> vegetationTris;

		public float totalDistance;

		public List<int> nodeSplinePoint;

		public string totalDistanceString;

		public ERCrossingPrefabs startPrefabScript;

		public ERCrossingPrefabs endPrefabScript;

		public int startConnectionSegment;

		public int endConnectionSegment;

		public Material roadMaterial;

		public Material[] roadMaterials;

		public Vector3 startDir;

		public Vector3 endDir;

		public float startAngle;

		public float endAngle;

		public int startbendLeftRight;

		public int endbendLeftRight;

		public Vector3 pivotp;

		public Vector3 p1;

		public Vector3 p2;

		public Vector3 p3;

		public Vector3 p4;

		public List<Vector3> segPoints;

		public List<Vector3> testPoints;

		public List<Vector3> testPoints2;

		public Vector3 OCDCQQCOOD;

		public Vector3 ODODOCOODD;

		public Vector3 endLeft;

		public Vector3 endRight;

		public Mesh testmesh;

		public GameObject surfaceMesh;

		public Vector3 sv1;

		public Vector3 sv2;

		public Vector3 prefabIndentLeft;

		public Vector3 prefabIndentRight;

		public Vector3 roadIndent1;

		public Vector3 tmpPerpCP;

		public Vector3 tmpCP;

		public float splinePos;

		public float camHeight;

		public Vector3[] flyOverPoints;

		public Vector3 splinePosV3;

		public List<float> markerDistances;

		public string osmRoadType;

		public List<ERSORoad> soData;

		public string[] sideObjectNames;

		public int selectedSO;

		public bool rebuildSos;

		public bool sosCleared;

		public bool isSideObject;

		public int startOffsetActiveMarker;

		public int endOffsetActiveMarker;

		public float leftToCenterPerc;

		public bool splatMapActive;

		public int splatIndex;

		public int expandLevel;

		public int smoothLevel;

		public float splatOpacity;

		public int layer;

		public bool fadeInFlag;

		public float fadeInDistance;

		public bool fadeOutFlag;

		public float fadeOutDistance;

		public bool doSurroundingSurfaces;

		public bool snapToTerrain;

		public bool isUpdated;
	}
}
