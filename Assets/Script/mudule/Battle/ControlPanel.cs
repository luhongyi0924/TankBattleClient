using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : BasePanel
{
    private GameObject controlPanel;
    public override void OnInit()
    {
        skinPath = "ControlCanvas";
        layer = PanelManager.Layer.Panel;
        controlPanel = Instantiate(ResManager.LoadPrefab("ControlCanvas"));
    }

}