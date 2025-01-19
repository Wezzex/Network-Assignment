using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;

public class WaveCountUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentWaveCountText;
    public int currentWaveCount;


    private void Start()
    {
        GameManager.Instance.iWaveCount.OnValueChanged += UpdateWaveCount;

        UpdateWaveCount(0, GameManager.Instance.iWaveCount.Value);
    }

    private void Update()
    {

        if (GameManager.Instance.IsServer)
        {
            GameManager.Instance.WaveCount();
        }
    }

    private void UpdateWaveCount(int oldValue, int newValue)
    {
        currentWaveCountText.text = "Wave: " + GameManager.Instance.iWaveCount;

    }

    private void OnDestroy()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.iWaveCount.OnValueChanged -= UpdateWaveCount;
        }
    }

}
