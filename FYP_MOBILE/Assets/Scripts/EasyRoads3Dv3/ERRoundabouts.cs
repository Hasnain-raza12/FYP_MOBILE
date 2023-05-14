using System.Collections.Generic;
using UnityEngine;

namespace EasyRoads3Dv3
{
	public class ERRoundabouts : MonoBehaviour
	{
		public float roundAboutRadius;

		public float roundAboutResolution;

		public float rDist;

		public Vector3 raStartPos;

		public float roundaboutWidth;

		public int roadTypeInt;

		public float roadWidth;

		public bool lockLeftRightRoundingRadius;

		public float leftRoundingRadius;

		public float rightRoundingRadius;

		public int roundingSegments;

		public float connectionLength;

		public float maxRoadWidth;

		public float maxRoundingRadius;

		public List<Vector3> meshVecs;

		public List<Vector3> innerPoints;

		public List<Vector3> centerPoints;

		public List<Vector3> outerPoints;

		public List<Vector3> ODQCCQOCOO;

		public List<Vector2> innerPointsUVs;

		public List<Vector2> centerPointsUVs;

		public List<Vector2> outerPointsUVs;

		public List<Vector2> ODQCCQOCOOUVs;

		public List<Vector3> innerRoundaboutSidewalkV3;

		public List<Vector2> innerRoundaboutSidewalUV;

		public List<int> innerRoundaboutSidewalTris;

		public Material innerRoundaboutSidewalkMaterial;

		public List<int> innerRoundaboutSidewalkIntsStart;

		public List<int> innerRoundaboutSidewalkIntsEnd;

		public int innerSidewalkSegments;

		public Vector3 leftPoint;

		public Vector3 leftPoint1;

		public Vector3 rightPoint;

		public Vector3 rightPoint1;

		public Vector3 centerOnLine;

		public Vector3 leftOuterPoint;

		public Vector3 rightOuterPoint;

		public Vector3 pl;

		public Vector3 pr;

		public List<Vector3> edgePoints;

		public int newSegmentInt;

		public List<ERRoundaboutElement> connections;

		public string[] QDOOOQOOQQQQD;

		public int selectedConnection;

		public int activeConnection;

		public int tmpSelectedConnection;

		public int minStartInt;

		public int maxEndInt;

		public int centerInt;

		public int leftOuterInt;

		public int rightOuterInt;

		public List<Vector3> leftOuterSegments;

		public List<Vector3> leftInnerSegments;

		public List<Vector3> rightOuterSegments;

		public List<Vector3> rightInnerSegments;

		public List<Vector2> leftOuterSegmentsUVs;

		public List<Vector2> leftInnerSegmentsUVs;

		public List<Vector2> rightOuterSegmentsUVs;

		public List<Vector2> rightInnerSegmentsUVs;

		public Vector3 outerCenterPoint;

		public bool blendFlag;

		public Material mainRoadMaterial;

		public Material roadMaterial;

		public Material connectionMaterial;

		public Material defaultConnectionMaterial;

		public double roadType;

		public List<Vector3> innerRoundaboutPoints;

		public List<Vector2> innerRoundaboutUVs;

		public float innerSegmentDistance;

		public float innerSidewalkWidth1;

		public float innerSidewalkWidth2;

		public float innerCurbHeight;

		public float innerCurbDepth;

		public bool innerBeveledCurb;

		public float innerBeveledHeight;

		public float innerBeveledDepth;

		public bool innerOuterCurb;

		public bool innerRoadSideCurbUVControl;

		public bool innerOuterSideCurbUVControl;

		public Material innerSidewalkMaterial;

		public List<float> innerSidewalkUVs;

		public List<float> innerCurbUVs;

		public int selectedCorner;

		public int selectedCornerPreset;

		public int selectedSidewalkPreset;

		public string sidewalkPresetName;

		public int innerRoundaboutPreset;

		public bool leftFlag;

		public bool rightFlag;

		public ERCrossingPrefabs prefabScript;

		public QDOODOQQDQODD connectionElement;

		public ERModularBase baseScript;

		public bool isSceneObject;

		public bool guiChanged;

		public string crossingName;

		public bool activeSidewalks;

		public Vector3 testIndentMiddlePoint;
	}
}
