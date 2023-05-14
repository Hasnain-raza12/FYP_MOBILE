using System;
using System.IO;

namespace SimpleJSON
{
	public class JSONData : JSONNode
	{
		private string m_Data;

		public override string Value
		{
			get
			{
				return m_Data;
			}
			set
			{
				m_Data = value;
				Tag = JSONBinaryTag.Value;
			}
		}

		public JSONData(string aData)
		{
			m_Data = aData;
			Tag = JSONBinaryTag.Value;
		}

		public JSONData(float aData)
		{
			AsFloat = aData;
		}

		public JSONData(double aData)
		{
			AsDouble = aData;
		}

		public JSONData(bool aData)
		{
			AsBool = aData;
		}

		public JSONData(int aData)
		{
			AsInt = aData;
		}

		public override string ToString()
		{
			return "\"" + JSONNode.Escape(m_Data) + "\"";
		}

		public override string ToString(string aPrefix)
		{
			return "\"" + JSONNode.Escape(m_Data) + "\"";
		}

		public override string ToJSON(int prefix)
		{
			switch (Tag)
			{
			case JSONBinaryTag.Value:
				return $"\"{JSONNode.Escape(m_Data)}\"";
			case JSONBinaryTag.IntValue:
			case JSONBinaryTag.DoubleValue:
			case JSONBinaryTag.FloatValue:
				return m_Data;
			default:
				throw new NotSupportedException("This shouldn't be here: " + Tag);
			}
		}

		public override void Serialize(BinaryWriter aWriter)
		{
			JSONData jSONData = new JSONData(string.Empty);
			jSONData.AsInt = AsInt;
			if (jSONData.m_Data == m_Data)
			{
				aWriter.Write(4);
				aWriter.Write(AsInt);
				return;
			}
			jSONData.AsFloat = AsFloat;
			if (jSONData.m_Data == m_Data)
			{
				aWriter.Write(7);
				aWriter.Write(AsFloat);
				return;
			}
			jSONData.AsDouble = AsDouble;
			if (jSONData.m_Data == m_Data)
			{
				aWriter.Write(5);
				aWriter.Write(AsDouble);
				return;
			}
			jSONData.AsBool = AsBool;
			if (jSONData.m_Data == m_Data)
			{
				aWriter.Write(6);
				aWriter.Write(AsBool);
			}
			else
			{
				aWriter.Write(3);
				aWriter.Write(m_Data);
			}
		}
	}
}
