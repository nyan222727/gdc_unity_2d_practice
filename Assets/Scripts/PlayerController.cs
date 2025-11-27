using System.Collections;
using System.Collections.Generic;
// using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 想要引用 UI 文件就要使用這個 namespace

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    // 誰？我不知道。要在編輯器附加。
    // 要 Public。
    // 取得其他 GameObject 實體
    public Rigidbody2D rb; //開一個接口。
    public GameObject rockGameObject;

    public float speed = 3;
    public int jumpCount = 1;
    public GameObject bulletPrefab;
    // 設置子彈數量，吃彈夾恢復子彈。
    // Q1. 假設子彈數量有十個，碰到彈夾的時候就恢復子彈，沒有子彈就不能發射。
    // Q2. 碰到彈夾時子彈會消失。
    // Q3. 每十秒鐘場面上固定位置自動生成彈夾。 => 透過新腳本或 Player 都可以。
    public int bulletCount = 10; // 跳躍，發射的時候先去檢查。

    public int aniVal = 1;

    public Animator animator;

    public GameObject cameraObject;

    public int hp = 10;
    public int maxhp = 10;

    // 如果你想要改顏色
    public Image hpImage;  
    
    void UpdateUI()
    {
        float Percent = (float)hp / (float)maxhp;
        hpImage.transform.localScale = new Vector3(Percent, 1, 1);
    }

    void Start()
    {
        // Rigidbody 物理系統 通過物理幫我做移動，必須得到這個元件，我才能對它做運算。
        // 怎麼做，開一個接口。 1.rb 是剛才從編輯器拉進去的 Player 物件的 Rigidbody2D。 
        // 2. 動態抓取。 掛上我這個腳本的物件 => Player，讓他抓取 Rigidbody2D。
        rb = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        cameraObject = Camera.main.gameObject;

    }
    // 你要監測事件就只少要有其中一方有 Rigidbody
    void OnCollisionEnter2D(Collision2D col)
    {
        print("碰到: " + col.gameObject.name);
        if (col.gameObject.tag == "Ground")
        {
            animator.SetInteger("state", 0);
            jumpCount = 1;
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        print("碰到Trigger:" + col.gameObject.name);

        // 生成很多個，不能依靠名字，而是 Tag。以下註解為錯誤示範。
        // if (col.gameObject.name == "Clip_Object")
        // {
        //     bulletCount += 10;
        //     Destroy(col.gameObject);
        // }

        if (col.gameObject.tag == "Clip")
        {
            bulletCount += 10;
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "Trap")
        {
            // 當我碰到刺的時候
            hp -= 1;
            // 血量變動了 -> 更新 UI 抓到，那我要抓到 UI 的圖片。
            // 1. 自動找
            // 2. 拉進來

            // 你碰到的時候他會一直觸發。 1. 無敵禎 2. 推力
            UpdateUI();
        }

        if(col.gameObject.tag == "Teleport")
        {
            // SceneManager.LoadScene(1);
            // SceneManager.LoadScene(1);
            SceneManager.LoadScene(col.gameObject.name);
            //我想要動態改變重送位置，傳送物件名字拿來用。
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        
    }

    void DamageToPlayer()
    {

    }
    // 你要用

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // 你要在哪生成這個子彈物件？
            if (bulletCount >= 1)
            {
                GameObject newObject = Instantiate(bulletPrefab, this.transform.position, Quaternion.Euler(new Vector3(0, 0, 90)));
                Rigidbody2D newRb = newObject.GetComponent<Rigidbody2D>();
                newRb.AddForce(new Vector2(20, 5), ForceMode2D.Impulse);
                // 發射子彈 -Prefab 預處理物件 => 你的子彈的圖片？有哪些腳本？有哪些元件 => 製作成 Prefab
                // How to recycle bullet. => We use time.
                bulletCount--;
            }

        }

        if (Input.GetKeyDown(KeyCode.W) && jumpCount >= 1)
        {

            rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
            // rockGameObject.transform.position += new Vector3(0, 0.01f, 0);

            jumpCount -= 1;
            print("W");
            // animator.SetTrigger("jump");
            animator.SetInteger("state", 2);
        }
        // 輸入檢測：GetKey 檢查按下的狀態，GetKeyDown 檢查按下的那個瞬間。
        // 如果使用物理系統，使用物理系統移動比較好。
        if (Input.GetKey(KeyCode.D))
        {
            //想操作腳本掛上去的那個 Transform 位置。
            // print("向右走");
            // this.transform.position += new Vector3(0.01f, 0, 0);
            // rb.AddForce(new Vector2(0.1f, 0), ForceMode2D.Impulse);
            rb.velocity = new Vector2(speed, rb.velocity.y); // 我直接調整它速度的值。且 y 不要動。
            // this.transform.Rotate(new Vector3(0, 0, 1), 0.1f);
            // this.transform.localScale += new Vector3(0.1f, 0, 0);
            // animator.SetTrigger("walk"); // 把 walk 這個 Trigger 把他打開。
            animator.SetInteger("state", 1);
        }

        if (Input.GetKey(KeyCode.A))
        {
            // print("向左走");
            // this.transform.position -= new Vector3(0.01f, 0, 0);
            // rb.AddForce(new Vector2(-0.1f, 0), ForceMode2D.Impulse);
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            // this.transform.Rotate(new Vector3(0, 0, 1), -0.1f);
            // this.transform.localScale -= new Vector3(0.1f, 0, 0);
            // animator.SetTrigger("walk"); // 把 walk 這個 Trigger 把他打開。
            animator.SetInteger("state", 1);
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            animator.SetInteger("state", 0);
        }
        //注意 Camera Z 軸，還有不要變更我的攝影機的 Z 軸。
        var newCameraPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.cameraObject.transform.position.z);
        cameraObject.transform.position = newCameraPosition;



    }
    
    void AnimationCallback()
        {
            print("UWU");
        }
}
