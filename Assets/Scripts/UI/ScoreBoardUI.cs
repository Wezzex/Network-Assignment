using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;

public class ScoreBoardUI : MonoBehaviour
{
    [SerializeField] private GameObject player1ArrowImage;
    [SerializeField] private GameObject player2ArrowImage;


    private void Awake()
    {
        player1ArrowImage.SetActive(false);
        player2ArrowImage.SetActive(false);
    }
}
