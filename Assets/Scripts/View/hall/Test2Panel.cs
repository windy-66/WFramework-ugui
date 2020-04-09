using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2Panel : PanelBase
{
    protected override void OnInitSkinFront()
    {
        this.SetMainSkinPath("Panel/Test2Panel");
    }

    public override void Init()
    {
        base.Init();
        this._type = PanelName.Test2Panel;
        this._showStyle = PanelMgr.PanelShowStyle.LeftToSlide;
        this._maskStyle = PanelMgr.PanelMaskSytle.Translucence;
    }
    protected override void OnClick(GameObject target)
    {
        switch (target.gameObject.name)
        {
            case "Btn_Close":
                Close();
                break;

        }
    }
}
