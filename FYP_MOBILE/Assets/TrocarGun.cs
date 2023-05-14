using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrocarGun : MonoBehaviour
{
    public GameObject ObjectGun, ObjectGun2;
    public bool trocar, pressionado;

    public void TrocarGuns()
    {
        trocar = !trocar;
        if (pressionado == trocar)
        {
            TrocarGun1();
        }
        else
        {
            TrocarGun2();
        }
    }

    void TrocarGun1()
    {
        ObjectGun.SetActive(true);
        ObjectGun2.SetActive(false);
    }

    void TrocarGun2()
    {
        ObjectGun.SetActive(false);
        ObjectGun2.SetActive(true);
    }
}