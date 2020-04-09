using UnityEngine;
public class Singleton<T> where T : new()
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }

}

public class SingletonUnity<T> : MonoBehaviour where T : MonoBehaviour
{

    private static string UnitySingletionName = "UnitySingletionRoot";
    private static GameObject UnitySingletionRoot;
    private static T instance;

    public static T Instance
    {
        get
        {
            if (UnitySingletionRoot == null)//如果是第一次调用单例类型就查找所有单例类的总结点
            {
                UnitySingletionRoot = GameObject.Find(UnitySingletionName);
                if (UnitySingletionRoot == null)//如果没有找到则创建一个所有继承MonoBehaviour单例类的节点
                {
                    UnitySingletionRoot = new GameObject();
                    UnitySingletionRoot.name = UnitySingletionName;
                    DontDestroyOnLoad(UnitySingletionRoot);//防止被销毁
                }
            }
            if (instance == null)//为空表示第一次获取当前单例类
            {
                instance = UnitySingletionRoot.GetComponent<T>();
                if (instance == null)//如果当前要调用的单例类不存在则添加一个
                {
                    instance = UnitySingletionRoot.AddComponent<T>();
                }
            }
            return instance;
        }
    }

}

