using UnityEngine;

public class aim : MonoBehaviour
{
	public static bool applymode;

	public bool scopeDoo;

	public bool scoped;

	public float smoothtime = 10f;

	public GameObject AimGun;

	public GameObject Scope;

	public Vector3 endMarker;

	public Vector3 startMarker;

	public Camera Aimcamera;

	public Camera Fpscamera;

	public Texture scope;

	private void Start()
	{
		applymode = false;
	}
	public void ScopeButton()
	{
        if (scoped)
        {
            Scope.SetActive(value: true);
        }
        else
        {
            Scope.SetActive(value: false);
        }
        if (applymode = !applymode )
        {
            Apply();

            Aimcamera.enabled = true;
            Scope.SetActive(value: false);
            Fpscamera.enabled = false;
            scopeDoo = true;
        }
        else
        {
            ApplyOFF();
            Aimcamera.enabled = false;
            Scope.SetActive(value: true);
            Fpscamera.enabled = true;
            scopeDoo = false;

        }
        //if (scoped)
        //{
        //    Scope.SetActive(value: true);
        //}
        //else
        //{
        //    Scope.SetActive(value: false);
        //}
        //if (applymode = !applymode)
        //{
        //    Apply();
        //    if (scoped)
        //    {
        //        Aimcamera.enabled = true;
        //        Scope.SetActive(value: false);
        //        Fpscamera.enabled = false;
        //        scopeDoo = true;
        //    }
        //}
        //else
        //{
        //    ApplyOFF();
        //    if (scoped)
        //    {
        //        Aimcamera.enabled = false;
        //        Scope.SetActive(value: true);
        //        Fpscamera.enabled = true;
        //        scopeDoo = false;
        //    }
        //}


    }
	private void Update()
    {
        //if (scoped)
        //{
        //    Scope.SetActive(value: true);
        //}
        //else
        //{
        //    Scope.SetActive(value: false);
        //}
        //if (CFInput.GetButtonDown("Fire2"))
        //{
        //    applymode = !applymode;
        //    Apply();
        //}
        //if (applymode)
        //{
        //    Apply();
        //    if (scoped)
        //    {
        //        Aimcamera.enabled = true;
        //        Scope.SetActive(value: false);
        //        Fpscamera.enabled = false;
        //        scopeDoo = true;
        //    }
        //}
        //else
        //{
        //    ApplyOFF();
        //    if (scoped)
        //    {
        //        Aimcamera.enabled = false;
        //        Scope.SetActive(value: true);
        //        Fpscamera.enabled = true;
        //        scopeDoo = false;
        //    }
        //}
    }

	private void Apply()
	{
		Vector3 localPosition = Vector3.Lerp(AimGun.transform.localPosition, endMarker, Time.deltaTime * smoothtime);
		AimGun.transform.localPosition = localPosition;
		GetComponentInParent<WeaponMnager>().CanChangeGun = false;
		Fpscamera.fieldOfView = 50f;
	}

	private void ApplyOFF()
	{
		Vector3 localPosition = Vector3.Lerp(AimGun.transform.localPosition, startMarker, Time.deltaTime * smoothtime);
		AimGun.transform.localPosition = localPosition;
		GetComponentInParent<WeaponMnager>().CanChangeGun = true;
		Fpscamera.fieldOfView = 65f;
	}

	private void OnGUI()
	{
		if (scopeDoo && scoped)
		{
			GUI.color = Color.black;
			GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), scope, ScaleMode.StretchToFill, alphaBlend: true, 0f);
		}
	}
}
