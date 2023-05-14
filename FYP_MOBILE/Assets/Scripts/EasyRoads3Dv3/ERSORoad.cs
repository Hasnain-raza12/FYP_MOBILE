using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyRoads3Dv3
{
	[Serializable]
	public class ERSORoad
	{
		public SideObject sideObject;

		public double id;

		public bool active;

		public List<Vector3> vecPositions;

		public ERSORoad(SideObject so)
		{
		}
	}
}
