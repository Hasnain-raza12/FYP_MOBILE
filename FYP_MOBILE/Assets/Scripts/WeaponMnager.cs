using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponMnager : MonoBehaviour
{
	public GameObject[] weapons;

	public int startingWeaponIndex;

	public int weaponIndex;

	public bool CanChangeGun = true;

	public List<string> Guns = new List<string>();

	public AudioClip ChangeGun;

	
	private void Start()
	{
		weaponIndex = startingWeaponIndex;
		SetActiveWeapon(weaponIndex);
	}

	private void Update()
	{
		if (Input.anyKeyDown)
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				SetActiveWeapon(0);
			}
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				SetActiveWeapon(1);
			}
			if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				SetActiveWeapon(3);
			}
			if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				SetActiveWeapon(4);
			}
			if (Input.GetKeyDown(KeyCode.Alpha5))
			{
				SetActiveWeapon(5);
			}
			if (Input.GetKeyDown(KeyCode.Alpha6))
			{
				SetActiveWeapon(6);
			}
		}
	}

	public void SetActiveWeapon(int index)
	{
		GetComponent<Animation>().Play("Draw");
		GetComponentInParent<AudioSource>().PlayOneShot(ChangeGun);
		if (index >= weapons.Length || index < 0)
		{
			Debug.LogWarning("Tried to switch to a weapon that does not exist.  Make sure you have all the correct weapons in your weapons array.");
			return;
		}
		weaponIndex = index;
		for (int i = 0; i < weapons.Length; i++)
		{
			weapons[i].SetActive(value: false);
		}
		if (Guns.Contains(weapons[index].gameObject.name))
		{
			weapons[index].SetActive(value: true);
		}
		else
		{
			NextWeapon();
		}
		Guns = Guns.Distinct().ToList();
	}

	public void NextWeapon()
	{
		if (CanChangeGun)
		{
			weaponIndex++;
			if (weaponIndex > weapons.Length - 1)
			{
				weaponIndex = 0;
			}
			SetActiveWeapon(weaponIndex);
		}
	}
}
