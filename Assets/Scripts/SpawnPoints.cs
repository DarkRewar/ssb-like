using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPoints : MonoBehaviour
{
    public static List<SpawnPoints> All = new List<SpawnPoints>();

    private void Awake()
    {
        All.Add(this);
    }
}
