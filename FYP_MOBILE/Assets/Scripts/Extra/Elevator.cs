using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
	public float Smooth = 2f;

	public GameObject Door1;

	public GameObject Door2;

	public Transform ElevatorObj;

	public Vector3 endMarker;

	public Vector3 startMarker;

	public AudioClip Audios;

	public bool Open;

	public bool doo;

	private void Start()
	{
	}

	private void Update()
	{
		if (doo)
		{
			StartCoroutine("Wfc");
			GetComponent<AudioSource>().PlayOneShot(Audios);
			doo = false;
		}
		if (Open)
		{
			Vector3 localPosition = Vector3.Lerp(ElevatorObj.localPosition, endMarker, Time.deltaTime * Smooth);
			ElevatorObj.localPosition = localPosition;
		}
		if (!Open)
		{
			Vector3 localPosition2 = Vector3.Lerp(ElevatorObj.localPosition, startMarker, Time.deltaTime * Smooth);
			ElevatorObj.localPosition = localPosition2;
		}
	}

	private IEnumerator Wfc()
	{
		Door1.GetComponent<Elevatordoor>().doo = true;
		Door2.GetComponent<Elevatordoor>().doo = true;
		yield return new WaitForSeconds(1f);
		Open = !Open;
	}

	private void OnTriggerEnter(Collider o)
	{
		if (o.tag == "Enemy")
		{
			Open = true;
		}
	}
}
