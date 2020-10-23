using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform weaponPoint;
    public GameObject boomerangPrefab;

    public void Shoot()
    {
        if(GameObject.FindWithTag("Boomy") == null ) {
            Instantiate(boomerangPrefab, weaponPoint.position, weaponPoint.rotation);
        }
    }


}
