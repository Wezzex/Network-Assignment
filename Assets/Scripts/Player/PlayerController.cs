using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Matchmaker.Models;

public class PlayerController : NetworkBehaviour
{
    [Header("Player parameters ")]
    [SerializeField] private float fShipThrust = 10;
    [SerializeField] private float fShipDrag = 5;
    [SerializeField] private float fShipMaxThrust = 10;
    [SerializeField] private float fShipRotation = 100f;
    [SerializeField] private float fLaserSpeed = 8;

    [Header("Object references")]
    [SerializeField] private Transform tLaserSpawn;
    [SerializeField] private Rigidbody2D rbLaser;

    private Rigidbody2D rbShip;
    private bool bIsAlive = true;
    private bool bIsAccelerating = false;

    private void Start()
    {
        rbShip = GetComponent<Rigidbody2D>();
    }

    public override void OnNetworkSpawn()
    { 
        GameManager.Instance.players.Add(this);
    }

    private void Update()
    {

        if(bIsAlive)
        { 
        HandleShipAcceleration();
        HandleSHipRotation();
        HandleBlasting();
        }


       
    }


    private void HandleShipAcceleration()
    {
        bIsAccelerating = Input.GetKey(KeyCode.W);
    }

    private void HandleSHipRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(fShipRotation * Time.deltaTime * transform.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-fShipRotation * Time.deltaTime * transform.forward);
        }
    }


    private void HandleBlasting()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsOwner)
        {
            Debug.Log("Shoot");
            GameManager.Instance.OnPlayerShootServerRpc(NetworkManager.Singleton.LocalClientId);
        }

    }
   
    public void OnPlayerShoot(ulong OwnerClientId)
    {
        Debug.Log(OwnerClientId);
        Rigidbody2D laser = Instantiate(rbLaser, tLaserSpawn.position, tLaserSpawn.rotation);

        Vector2 vShipVelocity = rbShip.linearVelocity;
        Vector2 vShipDirection = transform.up;
        float fShipForwardSpeed = Vector2.Dot(vShipVelocity, vShipDirection);

        laser.AddForce(fLaserSpeed * transform.up, ForceMode2D.Impulse);
    }


    

    private void FixedUpdate()
    {
        if (bIsAlive)
        {
            if(bIsAccelerating)
            {

            rbShip.AddForce(fShipThrust * transform.up);
            rbShip.linearVelocity = Vector2.ClampMagnitude(rbShip.linearVelocity, fShipMaxThrust);
            }
            else
            {
                rbShip.linearVelocity = Vector2.Lerp(rbShip.linearVelocity, Vector2.zero, fShipDrag * Time.fixedDeltaTime);
            }
        }

    }

}
