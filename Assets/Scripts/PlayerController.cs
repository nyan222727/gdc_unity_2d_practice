using System.Collections;
using System.Collections.Generic;
// using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 想要引用 UI 文件就要使用這個 namespace
using UnityEngine.InputSystem;

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

    public float invicibleTime = 0;

    public Vector2 respawnPoint;

    public GameObject uiObject;

    public SpriteRenderer spriteRenderer;

    public AudioSource seSource;
    public AudioClip shootAudioClip;
    public AudioClip damageClip;

    public GameObject hitEffectPrefab;

    public float controlMoveVal;
    
    



    
    void UpdateUI()
    {
        float hpPercent = (float)hp / (float)maxhp;
        hpImage.transform.localScale = new Vector3(hpPercent, 1, 1);
    }

    void Start()
    {

        seSource = this.transform.Find("SE").GetComponent<AudioSource>();
        
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        // Rigidbody 物理系統 通過物理幫我做移動，必須得到這個元件，我才能對它做運算。
        // 怎麼做，開一個接口。 1.rb 是剛才從編輯器拉進去的 Player 物件的 Rigidbody2D。 
        // 2. 動態抓取。 掛上我這個腳本的物件 => Player，讓他抓取 Rigidbody2D。
        rb = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        // attempt to get current scene's main camera (may be null if scene has none)
        cameraObject = Camera.main != null ? Camera.main.gameObject : null;
        // keep camera reference up-to-date when scenes change
        SceneManager.sceneLoaded += OnSceneLoaded;

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

    void DamageToPlayer(int damage)
        {

        

        if(invicibleTime > 0)
        {
            invicibleTime -= Time.deltaTime;
            return;
        }
            
            // 我在受傷的時候，要在我這角色的位置，生成受傷特效。   
            Instantiate(hitEffectPrefab, this.transform.position, Quaternion.identity);
            
            hp -= damage;
            invicibleTime = 1; //不會倒數，必須讓他倒數
            UpdateUI();
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

        // void DamageToPlayer(int damage)
        // {
        //     if (invicibleTime > 0)
        //     {
        //         return;
        //     }
        //     hp -= damage;
        //     invicibleTime = 1; //不會倒數，必須讓他倒數
        //     UpdateUI();
        // }



        if (col.gameObject.tag == "Trap")
        {
            // 碰到的時候會一直觸發。 1.無敵貞 2.推力
            // 當我碰到刺的時候
            // hp -= 1;
            // 血量變動了 -> 更新 UI 抓到，那我要抓到 UI 的圖片。
            // 1. 自動找
            // 2. 拉進來

            // 你碰到的時候他會一直觸發。 1. 無敵禎 2. 推力
            // UpdateUI();
            DamageToPlayer(1);
        }

        if(col.gameObject.tag == "Teleport")
        {
            // SceneManager.LoadScene(1);
            //轉換場景 寫死了
            // SceneManager.LoadScene(1);
            // SceneManager.LoadScene(col.gameObject.name);
            // // 技巧:我想要動態改變重送位置，傳送物件名字拿來用。

            //拿這個 Portal 元件的資訊
        var portal = col.gameObject.GetComponent<Portal>();

            //如果先傳送再轉換他會卡一偵
            this.transform.position = portal.position;
            // ?use the portal's transform position (the Portal component may not expose a Position property)

            //創建一個變數，把我接下來要去的地方的位置記錄下來。
            respawnPoint = portal.position;
            SceneManager.LoadScene(portal.SceneName);
        
        }

        if(col.gameObject.tag == "DeadZone")
        {

            DamageToPlayer(5);
            this.transform.position = respawnPoint;
            }
    }

    void OnCollisionStay2D(Collision2D col)
    {
                if (col.gameObject.tag == "Trap")
        {
            // 碰到的時候會一直觸發。 1.無敵貞 2.推力
            // 當我碰到刺的時候
            // hp -= 1;
            // 血量變動了 -> 更新 UI 抓到，那我要抓到 UI 的圖片。
            // 1. 自動找
            // 2. 拉進來

            // 你碰到的時候他會一直觸發。 1. 無敵禎 2. 推力
            // UpdateUI();
            DamageToPlayer(1);
        }

    }

    // removed empty parameterless DamageToPlayer()
    // 你要用

    // Update is called once per frame
    void Update()
    {
        
        // if (Input.GetKeyDown(KeyCode.Z))
        // {
        //     // 你要在哪生成這個子彈物件？
        //     if (bulletCount >= 1)
        //     {
        //         // // 他會暫停再撥放
        //         seSource.clip = shootAudioClip;
        //         seSource.Play();

        //         // 音效比較長，會疊在一起。
        //         // PlayerOneShot可以讓音訊撥放互不影響。
        //         // seSource.PlayOneShot(shootAudioClip);// 先 Stop 再播。
        //         GameObject newObject = Instantiate(bulletPrefab, this.transform.position, Quaternion.Euler(new Vector3(0, 0, 90)));
        //         Rigidbody2D newRb = newObject.GetComponent<Rigidbody2D>();
        //         newRb.AddForce(new Vector2(20, 5), ForceMode2D.Impulse);
        //         // 發射子彈 -Prefab 預處理物件 => 你的子彈的圖片？有哪些腳本？有哪些元件 => 製作成 Prefab
        //         // How to recycle bullet. => We use time.
        //         bulletCount--;
        //     }

        // }

        // if (Input.GetKeyDown(KeyCode.W) && jumpCount >= 1)
        // {

        //     rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        //     // rockGameObject.transform.position += new Vector3(0, 0.01f, 0);

        //     jumpCount -= 1;
        //     print("W");
        //     // animator.SetTrigger("jump");
        //     animator.SetInteger("state", 2);
        // }
        // 輸入檢測：GetKey 檢查按下的狀態，GetKeyDown 檢查按下的那個瞬間。
        // 如果使用物理系統，使用物理系統移動比較好。
        if (controlMoveVal > 0)
        {
            spriteRenderer.flipX = false;
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

        if (controlMoveVal < 0)
        {
            spriteRenderer.flipX = true;
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
            rb.velocity = new Vector2(0, rb.velocity.y);
            animator.SetInteger("state", 0);
        }
        //注意 Camera Z 軸，還有不要變更我的攝影機的 Z 軸。
        
        // follow the current scene camera if available
        if (cameraObject != null)
        {
            var newCameraPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.cameraObject.transform.position.z);
            cameraObject.transform.position = newCameraPosition;
        }

        if (this.hp <= 0) {
            Destroy(this.uiObject);
            Destroy(this.gameObject);
            SceneManager.LoadScene("StartScene");
        }

    }

    




    
    void AnimationCallback()
        {
            // print("UWU");
        }
        

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // After a new scene loads, find the scene's main camera (if any)
        cameraObject = Camera.main != null ? Camera.main.gameObject : null;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    // 新的 InputSystem 觸發的事件
    public void Jump(InputAction.CallbackContext ctx)
    {
        // 他觸發 3 次 他分成三個階段，在符合條件 Performed 時，我再處理。
        // 為啥分三階段，我可以設置 eg 連點 hold 多少秒才觸發，有更多彈性去設置輸入。
        if (ctx.performed)
        {
            print("uwu");
            if(jumpCount >= 1)
            {
                rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
                jumpCount -= 1;
                animator.SetInteger("status", 2);
            }
        }
    }

    public void Shoot(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {

            // 你要在哪生成這個子彈物件？
            if (bulletCount >= 1)
            {
                // // 他會暫停再撥放
                seSource.clip = shootAudioClip;
                seSource.Play();

                // 音效比較長，會疊在一起。
                // PlayerOneShot可以讓音訊撥放互不影響。
                // seSource.PlayOneShot(shootAudioClip);// 先 Stop 再播。
                GameObject newObject = Instantiate(bulletPrefab, this.transform.position, Quaternion.Euler(new Vector3(0, 0, 90)));
                Rigidbody2D newRb = newObject.GetComponent<Rigidbody2D>();
                newRb.AddForce(new Vector2(20, 5), ForceMode2D.Impulse);
                // 發射子彈 -Prefab 預處理物件 => 你的子彈的圖片？有哪些腳本？有哪些元件 => 製作成 Prefab
                // How to recycle bullet. => We use time.
                bulletCount--;
            }

        
        }
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        // 只會觸發一次
        // 處理 用變數紀錄他的狀態
        if (ctx.performed)
        {
            controlMoveVal = ctx.ReadValue<float>();

            // 我要怎麼知道他觸發多少？ 左移動？ 右移動？
            // 
            // var data = ctx.ReadValue<float>();
            // print(data);

            
            // 只會觸發一次
            // 處理 用變數紀錄他的狀態
            // if (data > 0)
            // {
            //     spriteRenderer.flipX = false;
            //     rb.velocity = new Vector2(speed * data, rb.velocity.y);
            //     animator.SetInteger("status", 1);
            //     print(speed * data);
            // }
            // if (data < 0)
            // {
            //     spriteRenderer.flipX = true;
            //     rb.velocity = new Vector2(speed * data, rb.velocity.y);
            //     animator.SetInteger("status", 1);
            //     print(speed * data);
            // }
        
        }
        if (ctx.canceled )
        {
            controlMoveVal = 0;
            rb.velocity = new Vector2(0, rb.velocity.y);
            animator.SetInteger("status", 0);
        }
    }


    



}


