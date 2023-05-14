using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GunshootClass : MonoBehaviour
{
	public enum GunType
	{
		Pistol,
		Asault,
		Shotgun
	}

	private RaycastHit Hit;

	public GunType Type;

	public bool Canfire;

	public bool Enemylisten;

	public bool UseAnimation;

	public Animation Animat;

	public GameObject weaponModel;

	public GameObject Hitparticle;

	public GameObject Blood;

	public Text UiAmmo;

	public Text WeaponName;
	public string gunName;
	public Transform BulletPos;

	public AudioClip SSound;

	public AudioClip RSound;

	public AudioClip RSound2;

	public float shotPerRound = 1f;

	public float accuracy = 80f;

	public float currentaccuracy;

	private float currentAccuracy;

	public float accuracyDropPerShot = 1f;

	public float accuracyRecoverRate = 0.1f;

	public float rateOfFire = 10f;

	public float actualROF;

	public float fireTimer;

	public float Damagehit = 50f;

	public bool recoil = true;

	public float recoilKickBackMin = 0.1f;

	public float recoilKickBackMax = 0.3f;

	public float recoilRotationMin = 0.1f;

	public float recoilRotationMax = 0.25f;

	public float recoilRecoveryRate = 0.01f;

	public bool showCrosshair = true;

	public Texture2D crosshairTexture;

	public Texture2D hitCrosshairTexture;

	public int crosshairLength = 10;

	public int crosshairWidth = 4;

	public float startingCrosshairSize = 10f;

	private float currentCrosshairSize;

	public float range = 9999f;

	public int UseAmmo;

	public float reloadTime = 2f;

	public int ClipAmmo;

	public int Clipmag;

	public int AnimationSpeed;

	public LayerMask Mask;

	private bool hited;

	private void Start()
	{
		currentaccuracy = accuracy;
		Animat = base.gameObject.GetComponentInChildren<Animation>();
		Canfire = true;
		currentCrosshairSize = startingCrosshairSize;
		Mask = ~(int)Mask;
		if (rateOfFire != 0f)
		{
			actualROF = 1f / rateOfFire;
		}
		else
		{
			actualROF = 0.01f;
		}
		fireTimer = 0f;
	}
	public void ShootButton()
	{
		if (Canfire  && fireTimer >= actualROF)
		{
			Gunshoot();
		}
	}
	public void ReloadButton()
	{
		if (ClipAmmo < Clipmag && UseAmmo != 0)
		{
			Reload2();
		}
	}
	private void Update()
	{
		Enemylisten = false;
		UiAmmo.text = ClipAmmo + "/" + UseAmmo + " "+ gunName;
		fireTimer += Time.deltaTime;
		weaponModel.transform.position = Vector3.Lerp(weaponModel.transform.position, base.transform.position, recoilRecoveryRate * Time.deltaTime);
		weaponModel.transform.rotation = Quaternion.Lerp(weaponModel.transform.rotation, base.transform.rotation, recoilRecoveryRate * Time.deltaTime);
		currentAccuracy = Mathf.Lerp(currentAccuracy, accuracy, accuracyRecoverRate * Time.deltaTime);
		currentCrosshairSize = startingCrosshairSize + (accuracy - currentAccuracy) * 0.8f;
		switch (Type)
		{
		case GunType.Pistol:
			UseAmmo = GetComponentInParent<AmmoManager>().AmmoP;

			break;
		case GunType.Asault:
			UseAmmo = GetComponentInParent<AmmoManager>().AmmoA;
			break;
		case GunType.Shotgun:
			UseAmmo = GetComponentInParent<AmmoManager>().AmmoS;
			break;
		}
		//if (Canfire && CFInput.GetButton("Fire1") && fireTimer >= actualROF)
		//{
		//	Gunshoot();
		//}
		if (aim.applymode)
		{
			showCrosshair = false;
			accuracy = 100f;
		}
		else
		{
			accuracy = currentaccuracy;
			showCrosshair = true;
		}
		if (ClipAmmo == 0 && UseAmmo != 0)
		{
			Reload();
		}
		if (ClipAmmo == 0 && UseAmmo == 0)
		{
			Canfire = false;
		}
		//if (CFInput.GetKeyDown(KeyCode.R) && ClipAmmo < Clipmag && UseAmmo != 0)
		//{
		//	Reload2();
		//}
	}

	private void Gunshoot()
	{
		Enemylisten = true;
		Canfire = false;
		if (recoil)
		{
			Recoil();
		}
		fireTimer = 0f;
		for (int i = 0; (float)i < shotPerRound; i++)
		{
			float num = (100f - currentAccuracy) / 1000f;
			Vector3 forward = BulletPos.forward;
			forward.y += Random.Range(0f - num, num);
			forward.z += Random.Range(0f - num, num);
			currentAccuracy -= accuracyDropPerShot;
			if (currentAccuracy <= 0f)
			{
				currentAccuracy = 0f;
			}
			Debug.DrawRay(BulletPos.position, BulletPos.forward * 1000f, Color.green);
			if (Physics.Raycast(new Ray(BulletPos.position, forward), out Hit, range, Mask))
			{
				if (Hit.collider.tag == "Untagged")
				{
					Mparticle();
				}
				if (Hit.collider.tag == "Enemy")
				{
					BloodParticle();
					Hit.collider.SendMessageUpwards("ApplyDamage", Damagehit, SendMessageOptions.DontRequireReceiver);
					StartCoroutine("DRHit");
				}
				if (Hit.collider.tag == "EnemyHead")
				{
					BloodParticle();
					Hit.collider.SendMessageUpwards("ApplyDamage", 100, SendMessageOptions.DontRequireReceiver);
					Hit.collider.GetComponentInParent<EnemyAi>().HeadShot = true;
					StartCoroutine("DRHit");
				}
				if (Hit.collider.tag == "SniperEnemyHead")
				{
					BloodParticle();
					Hit.collider.SendMessageUpwards("ApplyDamage", 100, SendMessageOptions.DontRequireReceiver);
					Hit.collider.GetComponentInParent<SniperAI>().HeadShot = true;
					StartCoroutine("DRHit");
				}
			}
		}
		if (UseAnimation)
		{
			weaponModel.GetComponent<Animation>().Play("shoot");
			Animat["shoot"].speed = AnimationSpeed;
		}
		GetComponent<AudioSource>().PlayOneShot(SSound);
		MuzzleSP.doo = true;
		ClipAmmo--;
		Canfire = true;
	}

	private void Reload()
	{
		Canfire = false;
		fireTimer = 0f - reloadTime;
		aim.applymode = false;
		weaponModel.GetComponent<Animation>().Play("reload");
		GetComponent<AudioSource>().PlayOneShot(RSound2);
		if (Clipmag > UseAmmo && Clipmag - ClipAmmo < UseAmmo)
		{
			int num = Clipmag - ClipAmmo;
			UseAmmo -= num;
			ClipAmmo += num;
		}
		else if (Clipmag - ClipAmmo > UseAmmo)
		{
			ClipAmmo += UseAmmo;
			UseAmmo = 0;
		}
		else
		{
			ClipAmmo = Clipmag;
			UseAmmo -= ClipAmmo;
		}
		if (Type == GunType.Pistol)
		{
			GetComponentInParent<AmmoManager>().AmmoP = UseAmmo;
		}
		else if (Type == GunType.Asault)
		{
			GetComponentInParent<AmmoManager>().AmmoA = UseAmmo;
		}
		else
		{
			GetComponentInParent<AmmoManager>().AmmoS = UseAmmo;
		}
		Canfire = true;
	}

	private void Reload2()
	{
		aim.applymode = false;
		Canfire = false;
		fireTimer = 0f - reloadTime;
		weaponModel.GetComponent<Animation>().Play("reload2");
		GetComponent<AudioSource>().PlayOneShot(RSound);
		if (Clipmag > UseAmmo && Clipmag - ClipAmmo < UseAmmo)
		{
			int num = Clipmag - ClipAmmo;
			UseAmmo -= num;
			ClipAmmo += num;
		}
		else if (Clipmag - ClipAmmo > UseAmmo)
		{
			ClipAmmo += UseAmmo;
			UseAmmo = 0;
		}
		else
		{
			int num2 = Clipmag - ClipAmmo;
			ClipAmmo += num2;
			UseAmmo -= num2;
		}
		if (Type == GunType.Pistol)
		{
			GetComponentInParent<AmmoManager>().AmmoP = UseAmmo;
			
		}
		else if (Type == GunType.Asault)
		{
			GetComponentInParent<AmmoManager>().AmmoA = UseAmmo;
		}
		else
		{
			GetComponentInParent<AmmoManager>().AmmoS = UseAmmo;
		}
		Canfire = true;
	}

	private void Mparticle()
	{
		Object.Instantiate(Hitparticle, Hit.point, Quaternion.FromToRotation(Vector3.forward, Hit.normal));
	}

	private void BloodParticle()
	{
		Object.Instantiate(Blood, Hit.point, Quaternion.FromToRotation(Vector3.up, Hit.normal));
	}

	private void Recoil()
	{
		if (weaponModel == null)
		{
			Debug.Log("Weapon Model is null.  Make sure to set the Weapon Model field in the inspector.");
			return;
		}
		float num = Random.Range(recoilKickBackMin, recoilKickBackMax);
		float num2 = Random.Range(recoilRotationMin, recoilRotationMax);
		weaponModel.transform.Translate(new Vector3(0f, 0f, 0f - num), Space.Self);
		weaponModel.transform.Rotate(new Vector3(0f - num2, 0f, 0f), Space.Self);
	}

	private IEnumerator DRHit()
	{
		hited = true;
		yield return new WaitForSeconds(0.2f);
		hited = false;
	}

	private void OnGUI()
	{
		if (hited)
		{
			GUI.DrawTexture(new Rect(Screen.width / 2 - hitCrosshairTexture.width / 2, Screen.height / 2 - hitCrosshairTexture.height / 2, hitCrosshairTexture.width, hitCrosshairTexture.height), hitCrosshairTexture);
		}
		if (showCrosshair)
		{
			Vector2 vector = new Vector2(Screen.width / 2, Screen.height / 2);
			GUI.DrawTexture(new Rect(vector.x - (float)crosshairLength - currentCrosshairSize, vector.y - (float)(crosshairWidth / 2), crosshairLength, crosshairWidth), crosshairTexture, ScaleMode.StretchToFill);
			GUI.DrawTexture(new Rect(vector.x + currentCrosshairSize, vector.y - (float)(crosshairWidth / 2), crosshairLength, crosshairWidth), crosshairTexture, ScaleMode.StretchToFill);
			GUI.DrawTexture(new Rect(vector.x - (float)(crosshairWidth / 2), vector.y - (float)crosshairLength - currentCrosshairSize, crosshairWidth, crosshairLength), crosshairTexture, ScaleMode.StretchToFill);
			GUI.DrawTexture(new Rect(vector.x - (float)(crosshairWidth / 2), vector.y + currentCrosshairSize, crosshairWidth, crosshairLength), crosshairTexture, ScaleMode.StretchToFill);
		}
	}
}
