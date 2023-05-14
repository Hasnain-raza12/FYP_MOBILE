using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
	public enum AI
	{
		Idle,
		Attack,
		Dead,
		Walk,
		Shak,
		MoveR,
		Run,
		Follow
	}

	public AI Aienemy;

	public Vector3 finalPosition;

	private bool CanShoot;

	public bool HeadShot;

	public bool CanSee;

	public bool Alert;

	public bool Doo;

	public float time;

	public float WalkSpeed;

	public float Dist;

	public float distance = float.PositiveInfinity;

	public float FollowRange;

	public float EscapeRange;

	public GameObject[] CoverPos;

	public int Cpoint;

	public float accuracy = 80f;

	private float currentAccuracy;

	public float accuracyDropPerShot = 1f;

	public float accuracyRecoverRate = 0.1f;

	public Transform raycastStartSpot;

	public float range = 9999f;

	public int Ammo;

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

	private bool canplaysound;

	private GameObject CurrentPos;

	private NavMeshAgent nav;

	private Transform Player;

	private Animation anim;

	private void Start()
	{
		Creatpoint();
		CanShoot = true;
		canplaysound = true;
		nav = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animation>();
		Player = GameObject.FindGameObjectWithTag("Player").transform;
		raycastStartSpot = MuzzlePos;
	}

	private void Update()
	{
		Dist = Vector3.Distance(base.transform.position, Player.position);
		currentAccuracy = Mathf.Lerp(currentAccuracy, accuracy, accuracyRecoverRate * Time.deltaTime);
		if (Player.GetComponentInChildren<GunshootClass>().Enemylisten && CanSee && Aienemy != AI.Attack && Aienemy != AI.Follow && Aienemy != AI.MoveR)
		{
			Aienemy = AI.Attack;
			Camps.Alert = true;
		}
		if (GetComponentInChildren<EnemyEyes>().see && Aienemy != AI.Attack && Aienemy != AI.Follow && Aienemy != AI.MoveR)
		{
			Aienemy = AI.Attack;
			Camps.Alert = true;
		}
		if (Health <= 0f)
		{
			Aienemy = AI.Dead;
			Doo = false;
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
		case AI.Walk:
			StartCoroutine("Walk");
			break;
		case AI.Shak:
			StartCoroutine("Shak");
			break;
		case AI.MoveR:
			MoveR();
			break;
		case AI.Run:
			StartCoroutine("Run");
			break;
		case AI.Follow:
			Follow();
			break;
		}
	}

	private void Idle()
	{
		Enemy.GetComponent<Animation>().CrossFade("Idle");

        nav.isStopped = true;
    }

	private IEnumerator Shak()
	{
		nav.isStopped = true;
		LookAt();
		canplaysound = true;
		Enemy.GetComponent<Animation>().CrossFade("Shak");
		yield return new WaitForSeconds(1f);
		if (canplaysound)
		{
			Enemy.GetComponent<AudioSource>().PlayOneShot(EnemySpeak[0]);
			canplaysound = false;
		}
		Aienemy = AI.Attack;
	}

	private void Attack()
	{
		nav.Stop();
		LookAt();
		Enemy.GetComponent<Animation>().CrossFade("Attack");
		if (Dist > FollowRange)
		{
			Aienemy = AI.Follow;
		}
		if (Player.GetComponent<Player>().Health > 0f && CanSee && CanShoot)
		{
			StartCoroutine("EnemyShoot");
			if (Ammo <= 0)
			{
				Aienemy = AI.MoveR;
				Doo = true;
			}
		}
	}

	private void MoveR()
	{
		Creatpoint2();
		if ((float)Mathf.RoundToInt(base.transform.position.x) == finalPosition.x)
		{
			Ammo = 30;
			Aienemy = AI.Attack;
		}
		Enemy.GetComponent<Animation>().CrossFade("Run");
		nav.speed = 5f;
		nav.Resume();
	}

	private IEnumerator Dead()
	{
		nav.Stop();
		CanShoot = false;
		if ((bool)kit)
		{
			GameObject obj = Object.Instantiate(kit, MuzzlePos.position, MuzzlePos.rotation);
			Enemy.GetComponent<AudioSource>().PlayOneShot(EnemySpeak[1]);
			obj.name = kit.name;
			kit = null;
		}
		if (HeadShot)
		{
			Enemy.GetComponent<Animation>().CrossFade("Dead3");
			yield return new WaitForSeconds(anim["Dead3"].length - 0.2f);
		}
		else
		{
			Enemy.GetComponent<Animation>().CrossFade("Dead1");
			yield return new WaitForSeconds(anim["Dead1"].length - 0.2f);
		}
		Object.Destroy(base.gameObject);
	}

	private IEnumerator Walk()
	{
		Enemy.GetComponent<Animation>().CrossFade("Walk");
		base.transform.GetComponent<NavMeshAgent>().destination = CoverPos[Cpoint].transform.position;
		if (base.transform.GetComponent<NavMeshAgent>().remainingDistance < 0.4f)
		{
			Aienemy = AI.Idle;
			yield return new WaitForSeconds(2f);
			base.transform.GetComponent<NavMeshAgent>().destination = CoverPos[Cpoint].transform.position;
			Creatpoint();
			Aienemy = AI.Walk;
		}
		nav.speed = WalkSpeed;
		nav.Resume();
	}

	private IEnumerator Run()
	{
		Enemy.GetComponent<Animation>().CrossFade("Run");
		base.transform.GetComponent<NavMeshAgent>().destination = CoverPos[Cpoint].transform.position;
		if (base.transform.GetComponent<NavMeshAgent>().remainingDistance < 0.4f)
		{
			Aienemy = AI.Idle;
			yield return new WaitForSeconds(2f);
			base.transform.GetComponent<NavMeshAgent>().destination = CoverPos[Cpoint].transform.position;
			Creatpoint();
			Aienemy = AI.Run;
		}
		nav.speed = 3f;
		nav.Resume();
	}

	private void Follow()
	{
		if (Dist > EscapeRange)
		{
			Aienemy = AI.Walk;
		}
		Enemy.GetComponent<Animation>().CrossFade("Run");
		base.transform.GetComponent<NavMeshAgent>().destination = Player.position;
		if (Dist < 15f)
		{
			Aienemy = AI.Attack;
		}
		nav.speed = 3f;
		nav.Resume();
	}

	private IEnumerator EnemyShoot()
	{
		CanShoot = false;
		time = Random.Range(0.1f, 0.4f);
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
		if (Physics.Raycast(ray, out var hitInfo, range, Mask) && hitInfo.collider.tag == "Player")
		{
			hitInfo.collider.SendMessage("PlayerDamage", Random.Range(10, 20));
		}
		Ammo--;
		CanShoot = true;
	}

	private void Creatpoint()
	{
		if (CoverPos.Length != 0)
		{
			Cpoint = Random.Range(0, CoverPos.Length);
		}
	}

	private void Creatpoint2()
	{
		if (!Doo)
		{
			return;
		}
		for (int i = 0; i < 1; i++)
		{
			if (NavMesh.SamplePosition(Random.insideUnitSphere * 7f + base.transform.position, out var hit, 7f, 1) && CanSee)
			{
				finalPosition.x = Mathf.RoundToInt(hit.position.x);
				finalPosition.y = Mathf.RoundToInt(hit.position.y);
				finalPosition.z = Mathf.RoundToInt(hit.position.z);
				nav.destination = finalPosition;
				Doo = false;
			}
		}
	}

	private void Timer()
	{
		timer -= Time.fixedDeltaTime * TimerSpeed;
	}

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
