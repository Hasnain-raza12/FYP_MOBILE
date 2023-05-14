using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

public class Ladder : MonoBehaviour
{
	public bool climb;

	public int speed;

	private Transform Player;

	public float sss;

	private void Start()
	{
		Player = GameObject.FindWithTag("Player").transform;
	}

	private void Update()
	{
		sss = CrossPlatformInputManager.GetAxis("Horizontal");
		if (climb && CrossPlatformInputManager.GetAxis("Horizontal") != 0f)
		{
			Player.transform.position += Vector3.up / speed;
		}
	}

	private void OnTriggerEnter(Collider o)
	{
		if (o.tag == "Player")
		{
			climb = true;
			Player.GetComponent<FirstPersonController>().m_GravityMultiplier = 0f;
		}
	}

	private void OnTriggerExit(Collider o)
	{
		if (o.tag == "Player")
		{
			climb = false;
			Player.GetComponent<FirstPersonController>().m_GravityMultiplier = 2f;
		}
	}
}
