using System;

namespace EasyRoads3Dv3
{
	[Serializable]
	public struct ERSplatmap
	{
		public int x;

		public int y;

		public int index;

		public float value;

		public ERModularRoad script;

		public float tValue1;

		public float tValue2;

		public float tValue3;

		public float tValue4;

		public ERSplatmap(int m_x, int m_y, int m_index, float m_value, ERModularRoad scr, float tv1, float tv2, float tv3, float tv4)
		{
			this = default(ERSplatmap);
		}
	}
}
