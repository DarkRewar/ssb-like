using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadzone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerBehaviour>().Die();
            int rand = UnityEngine.Random.Range(0, SpawnPoints.All.Count);
            //Debug.Log(SpawnPoints.All[rand]);
            other.transform.position = SpawnPoints.All[rand].transform.position;
        }
    }
}
