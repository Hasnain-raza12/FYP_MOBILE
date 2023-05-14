using UnityEngine;

namespace EasyRoads3Dv3
{
	public class InterSectionMarkerScriptV3 : MonoBehaviour
	{
		public int intersectionType;

		public int intersectionPriority;

		public string[] intersectionPriorityStrings;

		public int[] startDistance;

		public int[] endDistance;

		public bool rounding;

		public float roundingDistance;

		public float roDiameter;

		public int roundingSegments;

		public float roundingIndent;

		public bool roundingMaterialFlag;

		public Material roundingMaterial;

		public string[] intersectionCrossWalkStrings;

		public int intersectionCrossWalkInt;

		public Material crossWalkMaterial;

		public Vector3 crosspointcenter;

		public GameObject[] roadObjects;

		public OCDQDOODOC scr;

		public Vector3[] storedVecs;

		public int[] storedInts;

		public float[] storedFloats;

		public Vector2[] storedUVs;

		public bool[] storedBools;

		public GameObject[] storedObjs;

		public int[] storedController;

		public bool init;
	}
}
