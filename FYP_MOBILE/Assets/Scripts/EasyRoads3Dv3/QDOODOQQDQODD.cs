using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyRoads3Dv3
{
	[Serializable]
	public class QDOODOQQDQODD
	{
		public Vector3 centerPoint;

		public Vector3 tmpCenterPoint;

		public Vector3 stageCenterPoint;

		public Vector3 tmpStageCenterPoint;

		public List<ERBlendVecs> blendData;

		public Vector3 controlPointV3;

		public Vector2 controlPoint;

		public float blendDistance;

		public float extendBounds;

		public List<Vector3> blendCornerPoints;

		public List<int> blendCornerPointInts;

		public List<float> blendCornerPointWeights;

		public List<Vector3> blendCornerPointsTransformed;

		public float blendRatio;

		public float curveStrength;

		public List<Vector2> roadShapeVecs;

		public string roadShapeVecsString;

		public int roadShapeMatchCount;

		public List<float> roadShapeUVY;

		public List<int> roadShapeMaterialInts;

		public List<Vector2> sidewalkLeftVecs;

		public List<float> sidewalkLeftUVY;

		public List<int> sidewalkLeftMaterialInts;

		public List<Vector2> sidewalkRightVecs;

		public List<float> sidewalkRightUVY;

		public List<int> sidewalkRightMaterialInts;

		public List<ERConnectionVecs> connectionVecs;

		public List<int> connectionVecInts;

		public List<int> fullConnectionVecInts;

		public List<int> sidewalkLeftConnectionVecInts;

		public List<int> sidewalkRightConnectionVecInts;

		public List<int> outerVecInts;

		public bool rotationPriority;

		public float centerPointAngle;

		public ERModularRoad connectedRoad;

		public int connectedMarker;

		public GameObject connectedRoadGO;

		public bool includeLeftSidewalk;

		public bool includeRightSidewalk;

		public Material roadMaterial;

		public Material[] roadMaterials;

		public float centerPointPercentage;

		public int leftIndent;

		public int rightIndent;

		public int leftSurrounding;

		public int rightSurrounding;

		public Vector3 leftIndentV3;

		public Vector3 leftSurroundingV3;

		public Vector3 rightIndentV3;

		public Vector3 rightSurroundingV3;

		public int leftIndentInt;

		public int rightIndentInt;

		public Vector3 alignmentHandleVec;

		public float additionalIndentDistance;

		public float connectionAngle;

		public Vector3 alignmentHandleVecRotationGizmo;

		public double roadType;
	}
}
