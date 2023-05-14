using UnityEngine;

public class PcWorkScript : MonoBehaviour
{
	public GameObject particle;

	public AudioClip Ex;

	public bool doo;

	private void Update()
	{
		if (doo)
		{
			particle.SetActive(value: true);
			GetComponent<AudioSource>().PlayOneShot(Ex);
			Object.Destroy(particle, 3f);
			doo = false;
		}
	}
}
