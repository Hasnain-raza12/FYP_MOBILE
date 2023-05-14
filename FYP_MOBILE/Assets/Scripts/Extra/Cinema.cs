using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cinema : MonoBehaviour
{
	public GameObject Player;

	public GameObject canvas;

	public GameObject Cinemaobj;

	public GameObject car;

	public float Secend;

	private void Start()
	{
	}

	private void Update()
	{
		if (PlayerPrefs.GetInt("cinema") == 1)
		{
			Player.SetActive(value: false);
			canvas.SetActive(value: false);
			car.SetActive(value: false);
			Cinemaobj.SetActive(value: true);
			Cinemaobj.GetComponent<Animation>().CrossFade("cin");
			Secend = 7.3f;
			StartCoroutine("wfs");
		}
		if (PlayerPrefs.GetInt("cinema") == 2)
		{
			Player.SetActive(value: false);
			canvas.SetActive(value: false);
			Cinemaobj.SetActive(value: true);
			Cinemaobj.GetComponent<Animation>().CrossFade("cin2");
			Secend = 3f;
			StartCoroutine("wfs");
		}
		if (PlayerPrefs.GetInt("cinema") == 3)
		{
			canvas.SetActive(value: false);
			Secend = 1f;
			StartCoroutine("wfs");
		}
	}

	private IEnumerator wfs()
	{
		yield return new WaitForSeconds(Secend);
		SceneManager.LoadScene("MainMenu");
		Time.timeScale = 0f;
	}
}
