using UnityEngine;

public class Map : MonoBehaviour
{
	public bool doo;

	public GameObject MapImg;

	private void Start()
	{
		doo = false;
	}

	private void Update()
	{
	}

	public void MapShow()
	{
		doo = !doo;
		if (doo)
		{
			Time.timeScale = 0f;
			MapImg.SetActive(value: true);
			GameObject.FindWithTag("CF").GetComponent<TouchController>().automaticMode = false;
		}
		else
		{
			Time.timeScale = 1f;
			MapImg.SetActive(value: false);
			GameObject.FindWithTag("CF").GetComponent<TouchController>().automaticMode = true;
		}
	}
}
