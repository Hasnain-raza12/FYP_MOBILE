using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	public GameObject[] Maps;

	public GameObject PlayerCam;

	private bool ActiveMap;

	public float Health;

	public Image HealthBar;

	public Image blood;

	public GameObject Deadreplace;

	public AudioClip[] Sounds;

	private GameObject PlayerG;

	private GameObject Canvas;

	private Color Alfa;

	private void Start()
	{
		Time.timeScale = 1f;
		ActiveMap = false;
		PlayerG = GameObject.FindWithTag("Player");
		Canvas = GameObject.FindWithTag("Canvas");
		blood.GetComponent<Image>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.M))
		{
			if (ActiveMap)
			{
				ActiveMap = false;
				PlayerCam.SetActive(value: true);
				for (int i = 0; i < Maps.Length; i++)
				{
					Maps[i].SetActive(value: false);
				}
			}
			else
			{
				ActiveMap = true;
				PlayerCam.SetActive(value: false);
				for (int j = 0; j < Maps.Length; j++)
				{
					Maps[j].SetActive(value: true);
				}
			}
		}
		Alfa = blood.color;
		Backblood();
		HealthBar.fillAmount = Health / 100f;
		if (Health <= 0f)
		{
			Canvas.SetActive(value: false);
			Object.Instantiate(Deadreplace, base.transform.position, base.transform.rotation).GetComponent<Rigidbody>().AddForce(20f, 10f, -50f);
			PlayerG.SetActive(value: false);
			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = true;
		}
	}

	private void PlayerDamage(float Damage)
	{
		Health -= Damage;
		Alfa.a = 1f;
		blood.color = Alfa;
		StartCoroutine("playsound");
		PlayerPrefs.GetInt("vibre");
		_ = 1;
	}

	private void Backblood()
	{
		if (Alfa.a != 0f)
		{
			Alfa.a -= 1f * Time.deltaTime / 2f;
			blood.color = Alfa;
		}
	}

	private IEnumerator playsound()
	{
		yield return new WaitForSeconds(0.4f);
		GetComponent<AudioSource>().PlayOneShot(Sounds[Random.Range(0, Sounds.Length)]);
	}
}
