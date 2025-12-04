using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public AudioClip BGMClip;
    // Start is called before the first frame update

    // 是否在這個 LevelManager 裡面去設置我每個區域的背景音樂
    void Start()
    {
        var player = GameObject.Find("Player").GetComponent<PlayerController>();
        player.transform.position = player.respawnPoint;

        var bgm = player.transform.Find("BGM").GetComponent<AudioSource>();
        bgm.clip = BGMClip;
        bgm.Play();
    }
}
