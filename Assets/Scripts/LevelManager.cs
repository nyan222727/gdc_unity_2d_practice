using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var player = GameObject.Find("Player").GetComponent<PlayerController>();
        player.transform.position = player.respawnPoint;
    }
}
