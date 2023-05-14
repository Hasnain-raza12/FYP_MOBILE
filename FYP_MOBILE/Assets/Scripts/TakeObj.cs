using UnityEngine;

public class TakeObj : MonoBehaviour
{
	public Transform RaySpot;

	public GameObject Gun;

	public GameObject Door;

	public GameObject Doorfance;

	public GameObject C4Pos;

	public GameObject C4;

	public GameObject Keycard;

	public AudioClip[] Audios;

	public LayerMask Mask;

	public GameObject PcWork;

	public bool TxTShow;

	private void Start()
	{
		Gun = GameObject.FindWithTag("Gun");
		PlayerPrefs.SetInt("c4", 0);
		PlayerPrefs.SetInt("Keycard", 0);
		PlayerPrefs.SetInt("PcWork", 0);
	}

	private void Update()
	{
		TxTShow = false;
		C4Pos = null;
		Door = null;
		Doorfance = null;
		Keycard = null;
		PcWork = null;
		if (Physics.Raycast(new Ray(RaySpot.position, RaySpot.forward), out var hitInfo, 5f, Mask))
		{
			if (hitInfo.collider.tag == "DoorFance")
			{
				TxTShow = true;
				Doorfance = hitInfo.collider.gameObject;
			}
			else if (hitInfo.collider.tag == "Door")
			{
				TxTShow = true;
				Door = hitInfo.collider.gameObject;
			}
			else if (hitInfo.collider.tag == "C4Pos")
			{
				TxTShow = true;
				C4Pos = hitInfo.collider.gameObject;
			}
			else if (hitInfo.collider.tag == "Keycard")
			{
				TxTShow = true;
				Keycard = hitInfo.collider.gameObject;
			}
			else if (hitInfo.collider.tag == "PcWork")
			{
				TxTShow = true;
				PcWork = hitInfo.collider.gameObject;
			}
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			Applay();
		}
		_ = TxTShow;
	}

	public void Applay()
	{
		if ((bool)Door)
		{
			if (Door.GetComponent<Door>().locked)
			{
				if (PlayerPrefs.GetInt("Keycard") == 1)
				{
					Door.GetComponent<Door>().doo = true;
					Door.GetComponent<Door>().locked = false;
				}
			}
			else
			{
				Door.GetComponent<Door>().doo = true;
			}
		}
		else if ((bool)Doorfance)
		{
			Doorfance.GetComponent<DoorFance>().doo = true;
		}
		else if ((bool)Keycard)
		{
			Object.Destroy(Keycard);
			PlayerPrefs.SetInt("Keycard", 1);
		}
		else if ((bool)PcWork)
		{
			PlayerPrefs.SetInt("PcWork", 1);
			PcWork.GetComponentInChildren<PcWorkScript>().doo = true;
			PcWork.tag = "Untagged";
		}
		else if ((bool)C4Pos && GetComponentInChildren<WeaponMnager>().Guns.Contains("C4"))
		{
			Object.Instantiate(C4, C4Pos.transform.position, C4Pos.transform.rotation);
			PlayerPrefs.SetInt("c4", PlayerPrefs.GetInt("c4") + 1);
			Object.Destroy(C4Pos, 0.2f);
		}
	}
}
