using System.Collections.Generic;
using UnityEngine;

namespace EasyRoads3Dv3
{
	public class ERBend : MonoBehaviour
	{
		public float roundAboutRadius;

		public float roundAboutResolution;

		public float rDist;

		public Vector3 raStartPos;

		public float roundaboutWidth;

		public float bendAngle;

		public bool meshInstance;

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

		public List<Vector3> splinePoints;

		public List<Vector2> innerPointsUVs;

		public List<Vector2> centerPointsUVs;

		public List<Vector2> outerPointsUVs;

		public List<Vector2> ODQCCQOCOOUVs;

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

		public int tmpSelectedConnection;

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

		public Material roadMaterial;

		public List<Vector3> innerRoundaboutPoints;

		public List<Vector2> innerRoundaboutUVs;

		public float innerSegmentDistance;

		public bool leftFlag;

		public bool rightFlag;
	}
}
