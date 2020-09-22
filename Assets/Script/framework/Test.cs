using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgehogTeam.EasyTouch;

public class Test : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        Debug.Log(ETCInput.GetAxis("MoveHorizontal") != 0);
    }
}
