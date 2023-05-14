using UnityEngine;

public class SniperEyes : MonoBehaviour
{
	private Transform Player;

	public bool inseerange;

	private RaycastHit hit;

	public LayerMask Mask;

	private void Start()
	{
		Player = GameObject.FindGameObjectWithTag("PlayerHead").transform;
	}

	private void Update()
	{
		Vector3 normalized = (Player.position - base.transform.position).normalized;
		if (Physics.Raycast(new Ray(base.transform.position, normalized), out hit, 400f, Mask))
		{
			if (hit.collider.tag == "PlayerHead")
			{
				base.gameObject.GetComponentInParent<SniperAI>().CanSee = true;
				inseerange = true;
			}
			else
			{
				base.gameObject.GetComponentInParent<SniperAI>().CanSee = false;
				inseerange = false;
			}
		}
	}
}
