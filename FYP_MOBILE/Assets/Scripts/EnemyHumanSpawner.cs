using UnityEngine;

public class EnemyHumanSpawner : MonoBehaviour
{
	public GameObject[] E;

	private void OnTriggerEnter(Collider O)
	{
		if (O.tag == "Player")
		{
			for (int i = 0; i < E.Length; i++)
			{
				E[i].SetActive(value: true);
			}
			Object.Destroy(base.gameObject, 0.5f);
		}
	}
}
