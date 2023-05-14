using System;
using UnityEngine;

[ExecuteInEditMode]
public class ManualCameraClipping : MonoBehaviour
{
	[Serializable]
	public class LayerClipDistance
	{
		[ReadOnlyInInspector]
		public string layerName;

		public float clipDistance;
	}

	[Tooltip("Set this as the default distance you want clipping to occur across the majority of layers.")]
	public float defaultClipDistance;

	internal float oldDefaultClipDistance;

	internal float furthestClipDistance;

	[Tooltip("In order to work properly, this must always have 32 elements, each corrisponding to the index of your layers. Set each index to set clipping distance of a layer. For example: set Element 0 to 1000 and all objects on the default layer will clip at 1000 units from this camera. The layer names should automatically be appropriately assigned for you though, so you don't have to look them up.")]
	public LayerClipDistance[] layerClipDistances = new LayerClipDistance[32];

	private void Awake()
	{
		if (Application.isPlaying || !Application.isEditor)
		{
			return;
		}
		oldDefaultClipDistance = defaultClipDistance;
		for (int i = 0; i < 32; i++)
		{
			if (layerClipDistances[i] == null)
			{
				layerClipDistances[i] = new LayerClipDistance();
			}
			if (LayerMask.LayerToName(i) != string.Empty)
			{
				layerClipDistances[i].layerName = LayerMask.LayerToName(i);
			}
			else
			{
				layerClipDistances[i].layerName = "Layer Not Defined";
			}
		}
	}

	private void Start()
	{
		if (Application.isPlaying)
		{
			float[] array = new float[32];
			for (int i = 0; i < 32; i++)
			{
				array[i] = layerClipDistances[i].clipDistance;
			}
			GetComponent<Camera>().layerCullDistances = array;
		}
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			return;
		}
		if (layerClipDistances.Length != 32)
		{
			LayerClipDistance[] array = new LayerClipDistance[32];
			for (int i = 0; i < layerClipDistances.Length; i++)
			{
				array[i].clipDistance = layerClipDistances[i].clipDistance;
			}
			for (int j = layerClipDistances.Length; j < 32; j++)
			{
				array[j].clipDistance = defaultClipDistance;
			}
			layerClipDistances = array;
		}
		else
		{
			for (int k = 0; k < layerClipDistances.Length; k++)
			{
				if (layerClipDistances[k].clipDistance == oldDefaultClipDistance)
				{
					layerClipDistances[k].clipDistance = defaultClipDistance;
				}
			}
			furthestClipDistance = layerClipDistances[0].clipDistance;
			for (int l = 0; l < layerClipDistances.Length; l++)
			{
				if (layerClipDistances[l].clipDistance > furthestClipDistance)
				{
					furthestClipDistance = layerClipDistances[l].clipDistance;
				}
			}
			if (furthestClipDistance < GetComponent<Camera>().nearClipPlane || furthestClipDistance == 0f)
			{
				furthestClipDistance = GetComponent<Camera>().farClipPlane;
				defaultClipDistance = furthestClipDistance;
			}
			GetComponent<Camera>().farClipPlane = furthestClipDistance;
			for (int m = 0; m < layerClipDistances.Length; m++)
			{
				if (layerClipDistances[m].clipDistance == 0f)
				{
					layerClipDistances[m].clipDistance = defaultClipDistance;
				}
			}
			oldDefaultClipDistance = defaultClipDistance;
		}
		for (int n = 0; n < 32; n++)
		{
			if (LayerMask.LayerToName(n) != string.Empty)
			{
				layerClipDistances[n].layerName = LayerMask.LayerToName(n);
			}
			else
			{
				layerClipDistances[n].layerName = "Layer Not Defined";
			}
		}
	}

	public void ResetDefaults()
	{
		LayerClipDistance[] array = layerClipDistances;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].clipDistance = defaultClipDistance;
		}
	}
}
