# WFramework-ugui C#版本
unity3d 基于ugui的框架，类似于cocos2dx 场景与层的概念，万能ui框架

1.1版本
增加资源更新进度，解决县城调用问题

1.0版本

支持Assetbundle 与 Resources 模式无缝切换

支持资源热更新

scene (后续加) panel layer 加入切换特效

结构： 

LoginScene

     panel
     tip
 MainScene
 
     panel
     tip
     
     
     
     Resources模式
    protected void SetMainSkinPath(string path)
    {
        this._isAssetbundleMode = false;
        mainSkinPath = path;
    }
    assetbudnle 模式
    protected void SetAssetBundleSkin(string abName, string assetName) 
    {
        this._isAssetbundleMode = true;
        this.abName = abName;
        this.assetName = assetName;
    }
    
    SceneMgr.Instance.SwitchingScene(SceneType.LoginScene); 切换场景
    PanelMgr.Instance.ShowPanel(PanelName.Test2Panel); 切换面板

