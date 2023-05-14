using UnityEngine;

public class Carsound : MonoBehaviour
{
	public AudioClip CarEngine;

	private void Update()
	{
		if (!GetComponent<AudioSource>().isPlaying)
		{
			GetComponent<AudioSource>().PlayOneShot(CarEngine);
		}
	}
}
