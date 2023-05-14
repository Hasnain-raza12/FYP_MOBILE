using UnityEngine;

public class MuzzleSP : MonoBehaviour
{
	public static bool doo;

	public GameObject muzzle;

	private void Update()
	{
		if (doo)
		{
			Object.Instantiate(muzzle, base.transform.position, base.transform.rotation);
			base.transform.Rotate(0f, Random.Range(0, 0), 45f);
			doo = false;
		}
	}
}
