using System.Collections.Generic;
using UnityEngine;

namespace EasyRoads3Dv3
{
	public class ERSideObjectInstance : MonoBehaviour
	{
		public SideObject so;

		public double id;

		public ERModularRoad roadScript;

		public List<GameObject> childs;

		public List<Vector3> vecs;
	}
}
