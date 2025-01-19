using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Netcode;
using Unity.VisualScripting;

public class GameManager : NetworkBehaviour
{

    public static GameManager Instance { get; private set; }

    [SerializeField] private Meteor meteorPrefab;
    public int iMeteorCount = 0;

    public event EventHandler OnGameStarted;

    private int iLevel = 0;
    public NetworkVariable<int> iWaveCount = new NetworkVariable<int>(0);


    public List<PlayerController> players = new List<PlayerController>();



    private void Start()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }

    }

    private void Update()
    {
        if (!IsHost)
            return;

        if (iMeteorCount == 0)
        {

            iLevel++;

            int iNumberOfMeteors = 2 + (2 * iLevel);
            for (int i = 0; i < iNumberOfMeteors; i++)
            {
                SpawnMeteorsRpc();
            }
            iMeteorCount = iNumberOfMeteors;
        }


        if (GameManager.Instance.IsServer)
        {
            GameManager.Instance.WaveCount();
        }

    }

    public void WaveCount()
    {
        if(IsServer)
        {
            iWaveCount.Value = iLevel;
        }
    }


    [ServerRpc(RequireOwnership =false)]
    public void OnPlayerShootServerRpc(ulong OwnerClientId)
    {
        //Debug.Log("Player: " + GameObject.FindGameObjectWithTag("Player"));

        // NetworkManager.Singleton.ConnectedClients
        OnPlayerShootClientRpc(OwnerClientId);
    }

    [ClientRpc]
    public void OnPlayerShootClientRpc(ulong OwnerClientId)
    {
        foreach(PlayerController player in players)
        {
            ulong id = player.transform.GetComponent<NetworkObject>().OwnerClientId;

            if (id == OwnerClientId)
            { 
                player.OnPlayerShoot(OwnerClientId);
            }
        }
    }

    private void NetworManager_OnClientConnectedCallback(ulong obj)
    {
        if (NetworkManager.Singleton.ConnectedClientsList.Count == 2)
        {
            OnGameStarted?.Invoke(this, EventArgs.Empty);
        }
    }

    [Rpc(SendTo.Server)]
    private void SpawnMeteorsRpc()
    {

        float fOffset = UnityEngine.Random.Range(0f, 1f);
        Vector2 vViewportSpanwPosition = Vector2.zero;

        int iEdge = UnityEngine.Random.Range(0, 4);
        if (iEdge == 0)
        {
            vViewportSpanwPosition = new Vector2(fOffset, 0f);
        }
        else if(iEdge == 1)
        {
            vViewportSpanwPosition = new Vector2(fOffset, 1);
        }
        else if (iEdge == 2)
        {
            vViewportSpanwPosition = new Vector2(0, fOffset);
        }

        else if (iEdge == 3)
        {
            vViewportSpanwPosition = new Vector2(1, fOffset);
        }

        Vector2 vWorldSpawnPosition = Camera.main.ViewportToWorldPoint(vViewportSpanwPosition);

        Meteor meteor = Instantiate(meteorPrefab, vWorldSpawnPosition, Quaternion.identity);
        NetworkObject networkObject = meteor.GetComponent<NetworkObject>();
        networkObject.Spawn();
        meteor.gameManager = this;
    }

}
