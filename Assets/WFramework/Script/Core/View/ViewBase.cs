using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ViewBase : MonoBehaviour
{

    /// <summary>
    /// 所有Transform
    /// </summary>
    private List<Transform> transList = new List<Transform>();

    /// <summary>
    /// 主皮肤
    /// </summary>
    private string mainSkinPath;

    /// <summary>
    /// 是否初始化
    /// </summary>
    private bool isInit;

    private GameObject _skin;
    /// <summary>
    /// 皮肤
    /// </summary>
    public GameObject skin
    {
        get
        {
            return _skin;
        }
    }

    public Transform skinTrs {
        get {
            return _skin.transform;
        }
    }

    /// <summary>
    /// 点击按钮
    /// </summary>
    /// <param name="target">点击的目标对象</param>
    protected virtual void OnClick(GameObject target) { }

    /// <summary>
    /// 初始化皮肤前
    /// </summary>
    protected virtual void OnInitSkinFront() { }

    /// <summary>
    /// 初始化皮肤
    /// </summary>
    protected void OnInitSkin()
    {
        if (mainSkinPath != null)
        {
            _skin = LoadSrc(mainSkinPath);
        }
        else
        {
            _skin = new GameObject("Skin");
        }
        skin.transform.SetParent(transform);
        skin.GetComponent<RectTransform>().sizeDelta = GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta;
        skin.transform.localPosition = Vector3.zero;//皮肤的位置不固定--
        skin.transform.localEulerAngles(Vector3.zero).localScale(1);
    }

    /// <summary>
    /// 初始化前
    /// </summary>
    protected void OnInitFront() { transList.Clear(); }

    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Init()
    {
        if (!isInit)
        {
            OnInitFront();
            OnInitSkinFront();
            OnInitSkin();
        }
        Transform[] transforms = this.GetComponentsInChildren<Transform>(true);
        for (int i = 0, max = transforms.Length; i < max; i++)
        {
            Transform transform = transforms[i];
            //如果点按钮没有就是没有初始化 Init()
            if (transform.name.StartsWith("Btn") == true)//以"Btn"开头命名的按钮才会触发OnClick 
            {

                if (transform.GetComponent<Button>())
                {
                    Button listener = transform.GetComponent<Button>();
                    listener.onClick.AddListener(() => { Click(listener.gameObject); });
                }
                else
                {
                    ButtonEx listener = transform.GetOrAddComponent<ButtonEx>();
                    listener.onLeftClick = (go) => { Click(go.gameObject); };
                }
            }
            transList.Add(transform);
        }

        isInit = true;
    }

   


    protected void Click(GameObject target)
    {
        OnClick(target);
    }

    /// <summary>
    /// 设置主skin
    /// </summary>
    /// <param name="path"></param>
    protected void SetMainSkinPath(string path)
    {
        mainSkinPath = path;
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    protected GameObject LoadSrc(string path)
    {
        return ResourceMgr.GetInstance.CreateGameObject(path, false);
    }
}
