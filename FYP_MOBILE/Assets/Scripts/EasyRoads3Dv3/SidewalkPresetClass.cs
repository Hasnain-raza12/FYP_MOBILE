using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyRoads3Dv3
{
	[Serializable]
	public class SidewalkPresetClass
	{
		public string presetName;

		public double id;

		public double timestamp;

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

		public SidewalkPresetClass(QDOQDSQOOQDDD corner, string name)
		{
		}
	}
}
