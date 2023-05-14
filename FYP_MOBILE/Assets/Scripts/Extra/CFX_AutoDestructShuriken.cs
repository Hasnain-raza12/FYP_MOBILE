using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class CFX_AutoDestructShuriken : MonoBehaviour
{
	public bool OnlyDeactivate;

	private void OnEnable()
	{
		StartCoroutine("CheckIfAlive");
	}

	private IEnumerator CheckIfAlive()
	{
		do
		{
			yield return new WaitForSeconds(0.5f);
		}
		while (GetComponent<ParticleSystem>().IsAlive(withChildren: true));
		if (OnlyDeactivate)
		{
			base.gameObject.SetActive(value: false);
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}
}
