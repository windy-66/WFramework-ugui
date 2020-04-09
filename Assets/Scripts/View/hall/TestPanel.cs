using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPanel : PanelBase
{

    protected override void OnInitSkinFront()
    {
        this.SetMainSkinPath("Panel/TestPanel");
    }
    public override void Init()
    {
        base.Init();
        this._type = PanelName.TestPanel;
        this._showStyle = PanelMgr.PanelShowStyle.LeftToSlide;
        this._maskStyle = PanelMgr.PanelMaskSytle.Translucence;
    }
    public override void OnShowing() 
    {
       
    }
    protected override void OnClick(GameObject target)
    {
        switch (target.gameObject.name)
        {
            case "Btn_Close":
                    Close();
                    break;
            case "Btn_Test":
                PanelMgr.GetInstance.ShowPanel(PanelName.Test2Panel);
                break;
        }
    }
}
