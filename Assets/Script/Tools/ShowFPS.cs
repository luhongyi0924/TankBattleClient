using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowFPS : MonoBehaviour
{
    private int frame;//帧数

    private float timer = 0;//计时器
    public float updateTimeval;//更新帧数的时间间隔

    private void Update()
    {
        if (timer >= updateTimeval)
        {
            frame = (int)(Time.timeScale / Time.deltaTime);

            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.black;
        GUI.Label(new Rect(Screen.width / 2, 100, 0, 0), "FPS：" + frame, style);

    }
}