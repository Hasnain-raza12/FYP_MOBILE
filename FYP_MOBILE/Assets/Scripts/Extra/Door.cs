using UnityEngine;

public class Door : MonoBehaviour
{
	public float Smooth = 2f;

	public float DoorOpenAngle = 90f;

	public float DoorCloseAngle;

	public AudioClip Audios;

	public bool Open;

	public bool doo;

	public bool locked;

	private void Start()
	{
		Open = false;
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
			Quaternion b = Quaternion.Euler(0f, DoorOpenAngle, 0f);
			base.transform.localRotation = Quaternion.Slerp(base.transform.localRotation, b, Time.deltaTime * Smooth);
		}
		else
		{
			Quaternion b2 = Quaternion.Euler(0f, DoorCloseAngle, 0f);
			base.transform.localRotation = Quaternion.Slerp(base.transform.localRotation, b2, Time.deltaTime * Smooth);
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
