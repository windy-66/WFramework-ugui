using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SceneMgr
{

    #region 初始化
    protected static SceneMgr mInstance;
    public static bool hasInstance
    {
        get
        {
            return mInstance != null;
        }
    }

    public static SceneMgr GetInstance
    {
        get
        {
            if (!hasInstance)
            {
                mInstance = new SceneMgr();
            }
            return mInstance;
        }

    }

    #endregion


    public delegate void OnSwitchingScene(SceneType type);

    /// <summary>
    /// 当更换场景时委派
    /// </summary>
    public OnSwitchingScene OnSwitchingSceneHandler;
    /// <summary>
    /// 存储当前已经实例化的场景
    /// </summary>
    public Dictionary<SceneType, SceneBase> scenes;
    /// <summary>
    /// 当前场景
    /// </summary>
    private SceneBase currentScene;
    /// <summary>
    /// 记录切换数据
    /// </summary>
    private List<SwitchRecorder> switchRecoders;
    /// <summary>
    /// 主场景
    /// </summary>
    private const SceneType mainSceneType = SceneType.MainScene;

    private SceneMgr()
    {
        scenes = new Dictionary<SceneType, SceneBase>();
        switchRecoders = new List<SwitchRecorder>();
    }

    public void Destroy()
    {
        OnSwitchingSceneHandler = null;

        switchRecoders.Clear();
        switchRecoders = null;

        scenes.Clear();
        scenes = null;
    }

    /// <summary>
    /// 场景切换, 上一个场景不销毁
    /// </summary>
    /// <param name="sceneType"></param>
    /// <param name="sceneArgs">场景参数</param>
    public void SwitchingScene(SceneType sceneType, params object[] sceneArgs)
    {
        if (currentScene != null)
        {
            if (sceneType == currentScene.type)
            {
                Debug.LogWarning("试图切换场景至当前场景：" + sceneType.ToString());
                return;
            }
        }
        if (sceneType == SceneType.MainScene) switchRecoders.Clear();
        switchRecoders.Add(new SwitchRecorder(sceneType, sceneArgs));//切换记录
         HideCurrentScene();
        ShowScene(sceneType,sceneArgs);
        if (OnSwitchingSceneHandler != null)
        {
            OnSwitchingSceneHandler(sceneType);
        }
    }

    /// <summary>
    /// 切换至上一个场景
    /// </summary>
    public void SwitchingToPrevScene()
    {
        if (switchRecoders.Count < 2)
        {
            Debug.LogWarning("切换至上一个场景时，没有上一个场景记录！请检查逻辑!");
            return;
        }
        SwitchRecorder sr = switchRecoders[switchRecoders.Count - 2];
        switchRecoders.RemoveRange(switchRecoders.Count - 2, 2);//切换至上一个场景后，记录请除最后一个场景（即当前场景）和上一场景
        SwitchingScene(sr.sceneType,sr.sceneArgs);
    }

    /// <summary>
    /// 打开指定场景
    /// </summary>
    /// <param name="sceneType"></param>
    /// <param name="sceneArgs">场景参数</param>
    private void ShowScene(SceneType sceneType, params object[] sceneArgs)
    {
        if (scenes.ContainsKey(sceneType))
        {
            currentScene = scenes[sceneType];
            currentScene.OnShowing();
            currentScene.OnResetArgs(sceneArgs);
            currentScene.gameObject.SetActive(true);
            currentScene.OnShowed();
        }
        else
        {
            if (sceneType == SceneType.None)
            {
                currentScene = null;
                return;
            }
            GameObject go = new GameObject(sceneType.ToString());
            Type mType = Type.GetType(sceneType.ToString());
            currentScene = go.AddComponent(mType) as SceneBase; //sceneType.tostring等于该场景的classname
            currentScene.OnInit(sceneArgs);
            scenes.Add(currentScene.type, currentScene);
            currentScene.OnShowing();
           // LayerMgr.GetInstance.SetLayer(currentScene.gameObject, LayerType.Scene);
            ViewMgr.GetInstance.SetScene(currentScene.gameObject, sceneType);
            go.transform.localPosition(Vector3.zero).localRotation(Quaternion.identity).localScale(1);
            currentScene.OnShowed();
        }
    }

    /// <summary>
    /// 关闭当前场景
    /// </summary>
    private void HideCurrentScene()
    {
        if (currentScene != null)
        {
            currentScene.OnHiding();
          
            if (!currentScene.cache)
            {
                scenes.Remove(currentScene.type);
                GameObject.Destroy(currentScene.gameObject);
            }
        }
    }

    /// <summary>
    /// 关闭当前场景
    /// </summary>
    private void HideCurrentScene(SceneBase temp)
    {
        if (temp != null)
        {
            temp.OnHiding();

            if (!temp.cache)
            {
                scenes.Remove(temp.type);
                GameObject.Destroy(temp.gameObject);
            }
        }
    }
    public SceneBase getCurrentScene()
    {
        return currentScene;
    }

    /// <summary>
    /// 记录
    /// </summary>
    internal struct SwitchRecorder
    {
        internal SceneType sceneType;
        internal object[]  sceneArgs;

        internal SwitchRecorder(SceneType sceneType, params object[] sceneArgs)
        {
            this.sceneType = sceneType;
            this.sceneArgs = sceneArgs;
        }
    }
}