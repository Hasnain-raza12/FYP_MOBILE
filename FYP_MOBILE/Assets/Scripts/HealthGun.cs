using UnityEngine;
using UnityEngine.UI;

public class HealthGun : MonoBehaviour
{
	private GameObject p;

	public Text NumberOfHealthKit;

	private void Start()
	{
		p = GameObject.FindWithTag("Player");
	}

	private void Update()
	{
	}

	public void UseGun()
	{
	}
}
