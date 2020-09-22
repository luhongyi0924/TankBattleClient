using System;

//坦克信息
[Serializable]
public class TankInfo
{
    public string id = "";  //玩家id
    public int camp = 0;    //阵营
    public int hp = 0;      //生命值

    //位置（transform.position）
    public float x = 0;     
    public float y = 0;
    public float z = 0;

    //旋转（transform.rotation）
    public float ex = 0;    
    public float ey = 0;
    public float ez = 0;
}
