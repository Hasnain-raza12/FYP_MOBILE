using System;
using UnityEngine;

namespace EasyRoads3Dv3
{
	[Serializable]
	public class ERSOMarker
	{
		public SideObject sideObject;

		public double id;

		public bool active;

		public float startOffset;

		public float endOffset;

		public bool splineActive;

		public float sidewaysDistance;

		public Vector3 startOffsetV3;

		public Vector3 endOffsetV3;

		public Vector3 startOffsetDir;

		public Vector3 endOffsetDir;

		public Vector3 startOffsetV3nb;

		public Vector3 endOffsetV3nb;

		public int curStartInt;

		public int curEndInt;

		public bool startOffsetActive;

		public bool endOffsetActive;

		public ERSOMarker(SideObject so)
		{
		}
	}
}
