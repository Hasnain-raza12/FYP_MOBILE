using UnityEngine;

public class DestroyGO : MonoBehaviour
{
	public GameObject muzzle;

	public GameObject Blood;

	public GameObject Hpaticle;

	public GameObject EnemyDead;

	private void Update()
	{
		Object.Destroy(muzzle, 0.1f);
		Object.Destroy(Hpaticle, 0.1f);
		Object.Destroy(Blood, 0.5f);
		Object.Destroy(EnemyDead, 5f);
	}
}
