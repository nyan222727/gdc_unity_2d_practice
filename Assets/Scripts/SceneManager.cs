using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MySceneManager : MonoBehaviour
{
    public GameObject playerObject; //新生成的
    public GameObject uiCanvas;
    public float clipTimer = 0;
    public GameObject Clip;
    // Start is called before the first frame update
    void Start()
    {
        //先判斷有沒有已經存在的角色? 如何搜尋物件？
    var existsPlayerObject = GameObject.Find("Player");

    //如果兩個不同，代表從其他地方來的
    //如果相同，那就沒有外來的 player
    if(existsPlayerObject != playerObject){

        //刪掉新生成的玩家物件
        Destroy(playerObject);
        Destroy(uiCanvas);
    }
    else{
        //不要刪掉
        DontDestroyOnLoad(playerObject);
        DontDestroyOnLoad(uiCanvas);
    }
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

    public void BackToTown()
    {
        SceneManager.LoadScene(2);
    }
}
