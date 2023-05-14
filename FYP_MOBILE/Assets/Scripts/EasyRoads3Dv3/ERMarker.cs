using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyRoads3Dv3
{
	[Serializable]
	public class ERMarker
	{
		public bool activeSplineNode;

		public float leftIndent;

		public int leftIndentAlignment;

		public float rightIndent;

		public int rightIndentAlignment;

		public float leftSurrounding;

		public float rightSurrounding;

		public bool bridgeObject;

		public float rotation;

		public Vector3 position;

		public int controlType;

		public float circularRadius;

		public float circularAngle;

		public int circularSegments;

		public float splineStrength;

		public Vector3 direction;

		public bool followTerrainContours;

		public int startSplinePoint;

		public float startDistance;

		public float startUVY;

		public float totalDistance;

		public string totalDistanceString;

		public float rotationCenter;

		public List<ERSOMarker> soData;

		public bool attachExit;

		public int exitType;

		public int exitGeomteryType;

		public int startExitInt;

		public int endExitInt;

		public float startExitOffset;

		public float extrusionDistance;

		public int extrusionType;

		public float fixedDistance;

		public float connectionAngle;

		public float connectionRadius;

		public Material exitMaterial;

		public Material connectionMaterial;

		public int exitRoadType;

		public int connectionRoadType;

		public List<Vector3> exitInnerVertices;

		public ERMarker(Vector3 pos, ERModularRoad scr, int element)
		{
		}
	}
}
