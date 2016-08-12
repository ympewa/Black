using UnityEngine;
using System.Collections;

public enum List
{
    A,
    B
}

public class WeaponsList : MonoBehaviour {

    public GameObject[] weapons;
    public List Weapon;
    public  GameObject currentWeapon;
    public  GameObject previousWeapon;

    void Start()
    {
        if (weapons.Length > 0)
        {
            currentWeapon = weapons[0];
        }
        else
        {
            Debug.Log("WEAPON LIST EMPTY!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && currentWeapon != weapons[0])
        {
            Change(weapons[0]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && currentWeapon != weapons[1])
        {
            Change(weapons[1]);
        }
    }

    public void Change(GameObject ActivWeapon)
    {
        previousWeapon = currentWeapon;
        currentWeapon = ActivWeapon;
        if(previousWeapon != null)
        {
            previousWeapon.SetActive(false);
            currentWeapon.SetActive(true);
        }
    }
}
