using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerLifeUI : MonoBehaviour
{
    public TMP_Text PlayerLife;
    public TMP_Text PlayerNumber;

    internal void Setup(PlayerBehaviour player, int i)
    {
        PlayerLife.text = "0%";
        PlayerNumber.text = $"Joueur {i}";
    }

    internal void UpdateLife(int newLife)
    {
        PlayerLife.text = $"{newLife}%";
    }
}
