using UnityEngine;

public class Camps : MonoBehaviour
{
	public static bool Alert;

	public GameObject Enemy;

	public GameObject[] pos;

	private int Rand;

	private GameObject player;

	private void Start()
	{
		Rand = Random.Range(1, 3);
		player = GameObject.FindWithTag("Player");
		Alert = false;
	}

	private void Update()
	{
		float num = Vector3.Distance(base.transform.position, player.transform.position);
		if (Alert && (bool)Enemy && num < 60f)
		{
			Enemy.GetComponent<EnemyAi>().CoverPos = pos;
			Enemy.GetComponent<EnemyAi>().Aienemy = EnemyAi.AI.Run;
			for (int i = 0; i < Rand; i++)
			{
				Object.Instantiate(Enemy, base.transform.position, base.transform.rotation);
			}
			Object.Destroy(base.gameObject);
		}
	}
}
