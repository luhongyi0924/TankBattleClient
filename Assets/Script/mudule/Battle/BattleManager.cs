﻿using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    //战场中的坦克
    public static Dictionary<string, BaseTank> idTankPairs =
        new Dictionary<string, BaseTank>();

    static GameObject yourTabPfb = ResManager.LoadPrefab("Prefabs/YourTab");
    static GameObject friendTabPfb = ResManager.LoadPrefab("Prefabs/FriendTab");
    static GameObject enemyTabPfb = ResManager.LoadPrefab("Prefabs/EnemyTab");
    static BaseTank tank;

    //初始化
    public static void Init()
    {
        //添加监听
        NetManager.AddMsgListener("MsgEnterBattle", OnMsgEnterBattle);
        NetManager.AddMsgListener("MsgBattleResult", OnMsgBattleResult);
        NetManager.AddMsgListener("MsgLeaveBattle", OnMsgLeaveBattle);
        NetManager.AddMsgListener("MsgSyncTank", OnMsgSyncTank);
        NetManager.AddMsgListener("MsgFire", OnMsgFire);
        NetManager.AddMsgListener("MsgHit", OnMsgHit);
    }

    //添加坦克
    public static void AddTank(string id, BaseTank tank)
    {
        idTankPairs[id] = tank;
    }

    //删除坦克
    public static void RemoveTank(string id)
    {
        idTankPairs.Remove(id);
    }

    //获取坦克
    public static BaseTank GetTank(string id)
    {
        if (idTankPairs.ContainsKey(id))
        {
            return idTankPairs[id];
        }
        return null;
    }

    //获取玩家控制的坦克
    public static BaseTank GetCtrlTank()
    {
        return GetTank(GameMain.id);
    }

    //重置战场
    public static void Reset()
    {
        //场景
        foreach (BaseTank tank in idTankPairs.Values)
        {
            Destroy(tank.gameObject);
        }
        //列表
        idTankPairs.Clear();
    }

    //开始战斗
    public static void EnterBattle(MsgEnterBattle msg)
    {
        //重置
        Reset();
        //关闭界面
        PanelManager.Close("RoomPanel");//可以放到房间系统的监听中
        PanelManager.Close("ResultPanel");
        PanelManager.Close("KillPanel");
        PanelManager.Close("BattlePanel");
        PanelManager.Close("AimPanel");
        //产生坦克
        for (int i = 0; i < msg.tanks.Length; i++)
        {
            GenerateTank(msg.tanks[i]);
        }
        //打开界面
        PanelManager.Open<BattlePanel>();
        PanelManager.Open<AimPanel>();
    }

    //产生坦克和小地图上的位置坐标
    public static void GenerateTank(TankInfo tankInfo)
    {
        string objName = "Tank_" + tankInfo.id;
        GameObject tankObj = new GameObject(objName);
        if (tankInfo.id == GameMain.id)
        {
            tank = tankObj.AddComponent<CtrlTank>();
            //设置你自己在地图上的标记
            GameObject yourTab = Instantiate(yourTabPfb);
            //标记的位置
            yourTab.transform.position = new Vector3
                (tankInfo.x, 200, tankInfo.z);
            //标记的方向   TUDO：写进游戏场景脚本中的Update方法中
            CameraFollow cf = tankObj.AddComponent<CameraFollow>();
        }
        else
        {
            if (tankInfo.camp == 1)
            {
                //设置盟军在地图上的标记
                GameObject friendTab = Instantiate(friendTabPfb);
                //标记的位置
                friendTab.transform.position = new Vector3
                    (tankInfo.x, 200, tankInfo.z);
            }
            else
            {
                //设置敌军在地图上的标记
                GameObject enemyTab = Instantiate(enemyTabPfb);
                //标记的位置
                enemyTab.transform.position = new Vector3
                    (tankInfo.x, 200, tankInfo.z);
            }
            tank = tankObj.AddComponent<SyncTank>();
        }

        ////camera
        //if (tankInfo.id == GameMain.id)
        //{
        //    CameraFollow cf = tankObj.AddComponent<CameraFollow>();
        //}

        //属性
        tank.camp = tankInfo.camp;
        tank.id = tankInfo.id;
        tank.hp = tankInfo.hp;
        //pos rotation
        Vector3 pos = new Vector3(tankInfo.x, tankInfo.y, tankInfo.z);
        Vector3 rot = new Vector3(tankInfo.ex, tankInfo.ey, tankInfo.ez);
        tank.transform.position = pos;
        tank.transform.eulerAngles = rot;
        //init    阵营      1为己方   2为敌方
        if (tankInfo.camp == 1)
        {
            tank.Init("tankPrefab");
        }
        else
        {
            tank.Init("tankPrefab2");
        }
        //列表
        AddTank(tankInfo.id, tank);
    }

    //收到进入战斗协议
    public static void OnMsgEnterBattle(MsgBase msgBase)
    {
        MsgEnterBattle msg = (MsgEnterBattle)msgBase;
        EnterBattle(msg);
    }

    //收到开火协议
    public static void OnMsgFire(MsgBase msgBase)
    {
        MsgFire msg = (MsgFire)msgBase;
        //不同步自己
        if (msg.id == GameMain.id)
        {
            return;
        }
        //查找坦克
        SyncTank tank = (SyncTank)GetTank(msg.id);
        if (tank == null)
        {
            return;
        }
        //开火
        tank.SyncFire(msg);
    }

    //收到击中协议
    public static void OnMsgHit(MsgBase msgBase)
    {
        MsgHit msg = (MsgHit)msgBase;
        //查找坦克
        BaseTank tank = GetTank(msg.targetId);
        if (tank == null)
        {
            return;
        }
        bool isDie = tank.IsDie();
        //被击中
        tank.Attacked(msg.damage);
        //击杀提示
        if (!isDie && tank.IsDie() && msg.id == GameMain.id)
        {
            PanelManager.Open<KillPanel>();
        }
    }

    //收到战斗结束协议
    public static void OnMsgBattleResult(MsgBase msgBase)
    {
        MsgBattleResult msg = (MsgBattleResult)msgBase;
        //判断显示胜利还是失败
        bool isWin = false;
        BaseTank tank = GetCtrlTank();
        if (tank != null && tank.camp == msg.winCamp)
        {
            isWin = true;
        }
        //显示界面
        PanelManager.Open<ResultPanel>(isWin);
        //关闭界面
        PanelManager.Close("AimPanel");
    }

    //收到玩家退出协议
    public static void OnMsgLeaveBattle(MsgBase msgBase)
    {
        MsgLeaveBattle msg = (MsgLeaveBattle)msgBase;
        //查找坦克
        BaseTank tank = GetTank(msg.id);
        if (tank == null)
        {
            return;
        }
        //删除坦克
        RemoveTank(msg.id);
        Destroy(tank.gameObject);
    }

    //收到同步协议
    public static void OnMsgSyncTank(MsgBase msgBase)
    {
        MsgSyncTank msg = (MsgSyncTank)msgBase;
        //不同步自己
        if (msg.id == GameMain.id)
        {
            return;
        }
        //查找坦克
        SyncTank tank = (SyncTank)GetTank(msg.id);
        if (tank == null)
        {
            return;
        }
        //移动同步
        tank.SyncPos(msg);
    }
}