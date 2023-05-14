using UnityEngine;

public class TakeGun : MonoBehaviour
{
	public enum GunTypeE
	{
		Pistol,
		Asault,
		Shotgun
	}

	public GunTypeE TypeE;

	public AudioClip Take;

	private void OnTriggerEnter(Collider o)
	{
		if (o.tag == "Player")
		{
			GetComponent<AudioSource>().PlayOneShot(Take);
			switch (TypeE)
			{
			case GunTypeE.Pistol:
				o.gameObject.GetComponentInChildren<AmmoManager>().AmmoP += 17;
				break;
			case GunTypeE.Asault:
				o.gameObject.GetComponentInChildren<AmmoManager>().AmmoA += 30;
				break;
			case GunTypeE.Shotgun:
				o.gameObject.GetComponentInChildren<AmmoManager>().AmmoS += 6;
				break;
			}
			o.gameObject.GetComponentInChildren<WeaponMnager>().Guns.Add(base.gameObject.name);
			Object.Destroy(base.gameObject, 0.2f);
		}
	}
}
