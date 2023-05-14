using System.Collections;
using UnityEngine;

public class DoorFance : MonoBehaviour
{
	public float Smooth = 2f;

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
			Open = !Open;
			GetComponent<AudioSource>().PlayOneShot(Audios);
			doo = false;
		}
		if (Open)
		{
			Vector3 localPosition = Vector3.Lerp(base.transform.localPosition, endMarker, Time.deltaTime * Smooth);
			base.transform.localPosition = localPosition;
			StartCoroutine("Wfc");
		}
		if (!Open)
		{
			Vector3 localPosition2 = Vector3.Lerp(base.transform.localPosition, startMarker, Time.deltaTime * Smooth);
			base.transform.localPosition = localPosition2;
		}
	}

	private IEnumerator Wfc()
	{
		yield return new WaitForSeconds(2f);
		Open = false;
	}

	private void OnTriggerEnter(Collider o)
	{
		if (o.tag == "Enemy")
		{
			Open = true;
		}
	}
}
