using UnityEngine;

public class Music : MonoBehaviour
{
	public AudioClip GameMusic;

	private void Update()
	{
		if (!GetComponent<AudioSource>().isPlaying)
		{
			GetComponent<AudioSource>().PlayOneShot(GameMusic);
		}
	}
}
