using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		base.gameObject.GetComponent<Animation>().CrossFade("s");
	}
}
