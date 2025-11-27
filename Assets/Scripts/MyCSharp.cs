using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Player
{
    //public / protect /private

    // 改成 public 其他才能呼叫
    public string name = "小名";
    // private int level = 1;
    public int level = 1;
    public int hp = 10;
    public int atk = 1;

    public Player(string name)
    {
        this.name = name;
    }

    // public void LevelUp()
    // {
    //     level += 1;
    //     UnityEngine.Debug.Log(level);
    // }

}

public class MyCSharp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Player player1 = new Player("小明");
        //可操作範圍(
        // player1.LevelUp();
        player1.hp -= 5;

        Player player2 = new Player("明小");
        player2.level = 3;
        // 物件導向，資料綁成物件，針對物件操作。
        


        // int age = 18;
        // string n = "123";
        // print(age + int.Parse(n));
        // print("我是" + age.ToString() + "歲");
    }


    // Update is called once per frame
    void Update()
    {

    }
}
