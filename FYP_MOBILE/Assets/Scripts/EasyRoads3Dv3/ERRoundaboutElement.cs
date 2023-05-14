using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyRoads3Dv3
{
	[Serializable]
	public class ERRoundaboutElement
	{
		public float roadWidth;

		public int roundingSegments;

		public bool lockLeftRightRoundingRadius;

		public float leftRoundingRadius;

		public float rightRoundingRadius;

		public float connectionLength;

		public int centerInt;

		public float positionPercentage;

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

		public List<int> leftSidewalkNormalsStart;

		public List<int> leftSidewalkNormalsEnd;

		public List<int> rightSidewalkNormalsStart;

		public List<int> rightSidewalkNormalsEnd;

		public List<Vector3> leftSidewalkSourceVecs;

		public List<Vector3> rightSidewalkSourceVecs;

		public Vector3 outerCenterPoint;

		public List<Vector3> innerRoundaboutPoints;

		public List<Vector2> innerRoundaboutUVs;

		public bool leftFlag;

		public bool rightFlag;

		public bool blendFlag;

		public Material roadMaterial;

		public Material connectionMaterial;

		public int prefabElement;

		public List<int> connectionVecInts;

		public List<int> fullConnectionVecInts;

		public Vector3 centerPoint;

		public Vector3 controlPointV3;

		public List<Vector2> roadShapeVecs;

		public string roadShapeVecsString;

		public List<float> roadShapeUVY;

		public List<int> blendCornerPointInts;

		public List<float> blendCornerPointWeights;

		public Vector3 sceneSelectionV3;

		public Vector3 sceneSelectionV3Global;

		public List<Vector3> rightIndentvecs;

		public List<Vector3> rightSurroundingvecs;

		public List<Vector3> leftIndentvecs;

		public List<Vector3> leftSurroundingvecs;

		public int rightIndentBorderInt;

		public int leftIndentBorderInt;

		public double roadType;
	}
}
