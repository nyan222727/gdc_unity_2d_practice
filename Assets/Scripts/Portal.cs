using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//在每個 Level 都要加上去，這個物件會幫我把角色設置到它的重生位置
public class Portal : MonoBehaviour
{
    // 把我的角色還原到重生點(現在傳送的時候，就會把位置設置過去)
    public string SceneName;
    public Vector2 position;
}
