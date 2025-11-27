using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class MySceneManager : MonoBehaviour
{

    public float clipTimer = 0;
    public GameObject Clip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        clipTimer += Time.deltaTime;

        if (clipTimer >= 10)
        {
            Instantiate(Clip, new Vector3(1.8f, 0, 0), Quaternion.identity);
            clipTimer = 0;
        }
    }

//寫在哪邊都 OK 
    // public void BackToTown()
    // {
    //     // 切換場景
    //     SceneManager.LoadScene("1");
    // }
}
