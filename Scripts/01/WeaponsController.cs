using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsController : MonoBehaviour
{
    public GameObject[] weapons;
    [SerializeField] private int weaponNum;
    
    private void Start()
    {
        foreach(var weapon in weapons)
        {
            weapon.SetActive(false);
        }
        weapons[weaponNum].SetActive(true);
    }

    private void Update()
    {
        SwitchWeapons();
    }

    private void SwitchWeapons()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            weapons[weaponNum].SetActive(false);
            weaponNum--;
            if (weaponNum < 0)
            {
                weaponNum = weapons.Length - 1;
            }
            weapons[weaponNum].SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            weapons[weaponNum].SetActive(false);
            weaponNum++;
            if (weaponNum > weapons.Length - 1)
            {
                weaponNum = 0;
            }
            weapons[weaponNum].SetActive(true);
        }
    }
}
