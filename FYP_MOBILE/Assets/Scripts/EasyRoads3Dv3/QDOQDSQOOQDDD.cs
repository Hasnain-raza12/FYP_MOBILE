using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyRoads3Dv3
{
	[Serializable]
	public class QDOQDSQOOQDDD
	{
		public int crossingElementLeftIndex;

		public int crossingElementRightIndex;

		public Vector3 centerHandleV3;

		public Vector3 centerHandleV3_2;

		public Vector3 leftHandleV3;

		public Vector3 rightHandleV3;

		public bool renderFlag;

		public bool leftConnectionHandle;

		public bool rightConnectionHandle;

		public float sidewalkWidth1;

		public float sidewalkWidth2;

		public float curbHeight;

		public float curbDepth;

		public bool beveledCurb;

		public float beveledHeight;

		public float beveledDepth;

		public bool outerCurb;

		public bool roadSideCurbUVControl;

		public bool outerSideCurbUVControl;

		public Material sidewalkMaterial;

		public List<float> sidewalkUVs;

		public List<float> curbUVs;

		public bool lockUVs;

		public float cornerRadius;

		public int cornerSegments;

		public float innerSegmentDistance;

		public float startAngle;

		public QDOQDSQOOQDDD(ERModularBase scr)
		{
		}
	}
}
