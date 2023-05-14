using UnityEngine;

public class Elevatordoor : MonoBehaviour
{
	public float Smooth = 2f;

	public Transform objtoMove;

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
			Vector3 localPosition = Vector3.Lerp(objtoMove.localPosition, endMarker, Time.deltaTime * Smooth);
			objtoMove.localPosition = localPosition;
		}
		if (!Open)
		{
			Vector3 localPosition2 = Vector3.Lerp(objtoMove.localPosition, startMarker, Time.deltaTime * Smooth);
			objtoMove.localPosition = localPosition2;
		}
	}

	private void OnTriggerEnter(Collider o)
	{
		if (o.tag == "Enemy")
		{
			Open = true;
		}
	}
}
