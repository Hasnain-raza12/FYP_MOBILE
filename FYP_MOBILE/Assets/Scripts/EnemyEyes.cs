using UnityEngine;

public class EnemyEyes : MonoBehaviour
{
	private Transform Player;

	public bool inseerange;

	public bool see;

	public LayerMask Mask;

	private RaycastHit hit;

	private void OnTriggerStay(Collider other)
	{
		if (inseerange && other.tag == "Player")
		{
			see = true;
		}
	}

	private void Start()
	{
		Player = GameObject.FindGameObjectWithTag("PlayerHead").transform;
	}

	private void Update()
	{
		Vector3 normalized = (Player.position - base.transform.position).normalized;
		Debug.DrawRay(base.transform.position, normalized * 100f, Color.red);
		if (Physics.Raycast(new Ray(base.transform.position, normalized), out hit, 100f, Mask))
		{
			if (hit.collider.tag == "PlayerHead")
			{
				base.gameObject.GetComponentInParent<EnemyAi>().CanSee = true;
				inseerange = true;
			}
			else
			{
				base.gameObject.GetComponentInParent<EnemyAi>().CanSee = false;
				inseerange = false;
			}
		}
	}
}
