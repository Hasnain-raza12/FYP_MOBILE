using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SniperAI : MonoBehaviour
{
	public enum AI
	{
		Idle,
		Attack,
		Dead,
		//Walk,
		Search
	}

	public AI Aienemy;

	private bool CanShoot;

	public bool HeadShot;

	public bool CanSee;

	public float time;

	//public float WalkSpeed;

	public float Dist;

	public float distance = float.PositiveInfinity;

	public float FollowRange;

	public float EscapeRange;

	//public GameObject[] CoverPos;

	public int Cpoint;

	public float accuracy = 80f;

	private float currentAccuracy;

	public float accuracyDropPerShot = 1f;

	public float accuracyRecoverRate = 0.1f;

	public Transform raycastStartSpot;

	public float range = 9999f;

	public GameObject muzzle;

	public GameObject Enemy;

	public GameObject kit;

	public AudioClip SSound;

	public float timer;

	public float TimerSpeed;

	public float Health;

	public LayerMask Mask;

	public Transform MuzzlePos;

	public AudioClip[] EnemySpeak;

	private NavMeshAgent nav;

	private Transform Player;

	private Animation anim;

	private void Start()
	{
		//Creatpoint();
		Enemy = base.gameObject;
		CanShoot = true;
		nav = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animation>();
		Player = GameObject.FindGameObjectWithTag("Player").transform;
		raycastStartSpot = MuzzlePos;
	}

	private void Update()
	{
		Dist = Vector3.Distance(base.transform.position, Player.position);
		currentAccuracy = Mathf.Lerp(currentAccuracy, accuracy, accuracyRecoverRate * Time.deltaTime);
		if (CFInput.GetButton("Fire1") && Dist < EscapeRange && Aienemy != AI.Attack)
		{
			Aienemy = AI.Attack;
		}
		if (Health <= 0f)
		{
			Aienemy = AI.Dead;
		}
		switch (Aienemy)
		{
		case AI.Idle:
			Idle();
			break;
		case AI.Attack:
			Attack();
			break;
		case AI.Dead:
			StartCoroutine("Dead");
			break;
		//case AI.Walk:
		//	StartCoroutine("Walk");
		//	break;
		case AI.Search:
			StartCoroutine("Search");
			break;
		}
	}

	private void Idle()
	{
		Enemy.GetComponent<Animation>().CrossFade("Idle");
		nav.Stop();
	}

	private void Attack()
	{
		nav.Stop();
		LookAt();
		Enemy.GetComponent<Animation>().CrossFade("Attack");
		if (!(Player.GetComponent<Player>().Health > 0f))
		{
			return;
		}
		if (CanSee)
		{
			if (CanShoot)
			{
				StartCoroutine("EnemyShoot");
			}
		}
		else
		{
			Aienemy = AI.Search;
		}
	}

	private IEnumerator Dead()
	{
		nav.Stop();
		CanShoot = false;
		if ((bool)kit)
		{
			Object.Instantiate(kit, MuzzlePos.position, MuzzlePos.rotation).name = kit.name;
			kit = null;
		}
		if (HeadShot)
		{
			Enemy.GetComponent<Animation>().CrossFade("Dead3");
			yield return new WaitForSeconds(anim["Dead3"].length - 0.1f);
		}
		else
		{
			Enemy.GetComponent<Animation>().CrossFade("Dead1");
			yield return new WaitForSeconds(anim["Dead1"].length - 0.1f);
		}
		Object.Destroy(base.gameObject);
	}

	//private IEnumerator Walk()
	//{
	//	Enemy.GetComponent<Animation>().CrossFade("Walk");
	//	base.transform.GetComponent<NavMeshAgent>().destination = CoverPos[Cpoint].transform.position;
	//	if (base.transform.GetComponent<NavMeshAgent>().remainingDistance < 0.4f)
	//	{
	//		Aienemy = AI.Idle;
	//		yield return new WaitForSeconds(2f);
	//		base.transform.GetComponent<NavMeshAgent>().destination = CoverPos[Cpoint].transform.position;
	//		Creatpoint();
	//		Aienemy = AI.Walk;
	//	}
	//	nav.speed = WalkSpeed;
	//	nav.Resume();
	//}

	private IEnumerator Search()
	{
		//Enemy.GetComponent<Animation>().CrossFade("Run");
		//base.transform.GetComponent<NavMeshAgent>().destination = CoverPos[Cpoint].transform.position;
		if (base.transform.GetComponent<NavMeshAgent>().remainingDistance < 0.4f)
		{
			Aienemy = AI.Idle;
			yield return new WaitForSeconds(0.1f);
			//base.transform.GetComponent<NavMeshAgent>().destination = CoverPos[Cpoint].transform.position;
		//	Creatpoint();
			Aienemy = AI.Search;
			if (CanSee)
			{
				Aienemy = AI.Attack;
			}
		}
		nav.speed = 3f;
		nav.Resume();
	}

	private IEnumerator EnemyShoot()
	{
		CanShoot = false;
		time = Random.Range(0.9f, 2f);
		yield return new WaitForSeconds(time);
		base.gameObject.GetComponent<AudioSource>().PlayOneShot(SSound);
		Object.Instantiate(muzzle, MuzzlePos.position, MuzzlePos.rotation);
		MuzzlePos.Rotate(0f, 0f, Random.Range(0, 45));
		float num = (100f - currentAccuracy) / 1000f;
		Vector3 forward = raycastStartSpot.forward;
		forward.x += Random.Range(0f - num, num);
		forward.y += Random.Range(0f - num, num);
		forward.z += Random.Range(0f - num, num);
		currentAccuracy -= accuracyDropPerShot;
		if (currentAccuracy <= 0f)
		{
			currentAccuracy = 0f;
		}
		Ray ray = new Ray(raycastStartSpot.position, forward);
		Debug.DrawRay(raycastStartSpot.position, raycastStartSpot.forward * 1000f, Color.green);
		if (Physics.Raycast(ray, out var hitInfo, range) && hitInfo.collider.tag == "Player")
		{
			hitInfo.collider.SendMessage("PlayerDamage", Random.Range(10, 30));
		}
		CanShoot = true;
	}

	//private void Creatpoint()
	//{
	//	if (CoverPos.Length != 0)
	//	{
	//		Cpoint = Random.Range(0, CoverPos.Length);
	//	}
	//}

	private void LookAt()
	{
		base.transform.LookAt(Player);
		Vector3 eulerAngles = base.transform.rotation.eulerAngles;
		eulerAngles.x = 0f;
		eulerAngles.z = 0f;
		base.transform.rotation = Quaternion.Euler(eulerAngles);
		MuzzlePos.transform.LookAt(Player);
	}

	private void ApplyDamage(float Damage)
	{
		Health -= Damage;
	}
}
