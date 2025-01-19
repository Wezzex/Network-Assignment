using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;

public class Meteor : NetworkBehaviour
{

    public GameManager gameManager;

    public int iMeteorSize = 3;

    private void Start()
    {
        transform.localScale = 0.5f * iMeteorSize * Vector3.one;

        Rigidbody2D rbMeteor = GetComponent<Rigidbody2D>();
        Vector2 vDirection = new Vector2(UnityEngine.Random.value, UnityEngine.Random.value).normalized;
        float fSpawnSpeed = UnityEngine.Random.Range(4f - iMeteorSize, 5f - iMeteorSize);
        rbMeteor.AddForce(vDirection * fSpawnSpeed, ForceMode2D.Impulse);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))
        {

            
            gameManager.iMeteorCount--;

            Destroy(collision.gameObject);

            if (iMeteorSize > 1)
            {
                for (int i = 0; i < 2; i++)
                {
                    Meteor newMeteor = Instantiate(this, transform.position, Quaternion.identity);
                    newMeteor.iMeteorSize = iMeteorSize - 1;
                    GameManager.Instance.iMeteorCount++;
                    NetworkObject networkObject = newMeteor.GetComponent<NetworkObject>();
                    networkObject.Spawn();

                }
            }

            Destroy(gameObject);

        }
    }
}
