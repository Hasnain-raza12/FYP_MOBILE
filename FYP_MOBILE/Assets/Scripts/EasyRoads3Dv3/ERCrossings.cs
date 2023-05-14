using System.Collections.Generic;
using UnityEngine;

namespace EasyRoads3Dv3
{
	public class ERCrossings : MonoBehaviour
	{
		public List<float> uvArrayFront;

		public List<float> uvArrayBack;

		public List<float> uvArrayLeft;

		public List<float> uvArrayRight;

		public List<int> OOQDCDQOOC;

		public List<int> OQDQDDODDC;

		public List<int> OQCCDQCQCC;

		public List<int> ODQDDDCQCQ;

		public List<int> OQODQQCOQC;

		public List<int> OQDDDDQQOD;

		public List<int> ODOODDQQQD;

		public List<int> OCOOOQDQDD;

		public List<int> OOQDCDQOOCStart;

		public List<int> OQDQDDODDCStart;

		public List<int> OQCCDQCQCCStart;

		public List<int> ODQDDDCQCQStart;

		public List<int> OQODQQCOQCStart;

		public List<int> OQDDDDQQODStart;

		public List<int> ODOODDQQQDStart;

		public List<int> OCOOOQDQDDStart;

		public int leftStartSidewalkCornerInt;

		public int rightStartSidewalkCornerInt;

		public int leftEndSidewalkCornerInt;

		public int rightEndSidewalkCornerInt;

		public int leftLeftSidewalkCornerInt;

		public int rightLeftSidewalkCornerInt;

		public int leftRightSidewalkCornerInt;

		public int rightRightSidewalkCornerInt;

		public Vector3[] sidewalkControlPoints;

		public bool[] sidewalkControlStatus;

		public bool copySettingsFlag;

		public bool generalSettingsFlag;

		public bool connectionSettingsFlag;

		public bool cornerSettingsFlag;

		public bool sidewalkSettingsFlag;

		public string[] QDOOOQOOQQQQD;

		public int selectedConnection;

		public float startAngle;

		public bool roundedCorners;

		public float roundingRadius;

		public int roundingSegments;

		public float innerSegmentDistance;

		public bool tCrossing;

		public bool oldTCrossing;

		public int tCrossingLeftRight;

		public int oldtCrossingLeftRight;

		public int geometryType;

		public float resolution;

		public bool includeSidewalks;

		public bool defaultSidewalkEnabledStatus;

		public bool isSceneObject;

		public int connectionHandling;

		public int frontRoadTypeInt;

		public double frontRoadTypeID;

		public float frontRoadWidth;

		public Material frontMaterial;

		public Material frontRoadMaterial;

		public int backRoadTypeInt;

		public double backRoadTypeID;

		public float backRoadWidth;

		public Material backMaterial;

		public Material backRoadMaterial;

		public int leftRoadTypeInt;

		public double leftRoadTypeID;

		public float leftRoadWidth;

		public Material leftMaterial;

		public Material leftRoadMaterial;

		public int rightRoadTypeInt;

		public double rightRoadTypeID;

		public float rightRoadWidth;

		public Material rightMaterial;

		public Material rightRoadMaterial;

		public int selectedRoadType;

		public bool uniformCornersFlag;

		public int selectedCorner;

		public int selectedCornerPreset;

		public string cornerPresetName;

		public int selectedSidewalkPreset;

		public string sidewalkPresetName;

		public int OQCCODDOOQCorner;

		public Vector3 leftBottom;

		public Vector3 rightBottom;

		public Vector3 leftTop;

		public Vector3 rightTop;

		public Vector3 frontCenter;

		public Vector3 backCenter;

		public Vector3 leftCenter;

		public Vector3 rightCenter;

		public int prefabId;

		public ERCrossingPrefabs prefabScript;

		public QDOODOQQDQODD connectionElement;

		public int crossingOuterElement;

		public string crossingName;

		public bool guiChanged;

		public List<Vector3> debugVecs;
	}
}
