//进入战场（服务端推送）
public class MsgEnterBattle : MsgBase
{
    public MsgEnterBattle() { protoName = "MsgEnterBattle"; }
    //服务端回
    public TankInfo[] tanks;
    public int mapId = 1;	//地图，只有一张
}

//战斗结果（服务端推送）
public class MsgBattleResult : MsgBase
{
    public MsgBattleResult() { protoName = "MsgBattleResult"; }
    //服务端回
    public int winCamp = 0;	 //获胜的阵营
}

//玩家退出（服务端推送）
public class MsgLeaveBattle : MsgBase
{
    public MsgLeaveBattle() { protoName = "MsgLeaveBattle"; }
    //服务端回
    public string id = "";	//玩家id
}