using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Laser : MonoBehaviour
{

    [SerializeField] private float fLaserLifeTime = 5f;

    private void Awake()
    {
        Destroy(gameObject, fLaserLifeTime);
    }
}
