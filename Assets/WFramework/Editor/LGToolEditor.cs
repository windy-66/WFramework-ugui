using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LGToolEditor : EditorWindow
{
    [MenuItem("LG_Tool/宏定义")]
    public static void Settings()
    {
        SettingWindows sw = GetWindow<SettingWindows>();//获取指定类型的窗口.
        sw.titleContent = new GUIContent("宏定义工具");
        sw.Show();
    }

    [MenuItem("LG_Tool/一键图片改精灵 %#F1")]
    public static void EditTexture()
    {
        Transform[] SeleGameObj = Selection.transforms;
        foreach (var item in SeleGameObj)
        {
            if (item.GetComponent<Texture>()==null)
            {
                Debug.LogError(item+" 不是texture！");
                continue;
            }
            string path = AssetDatabase.GetAssetPath(item);
            TextureImporter texture = AssetImporter.GetAtPath(path) as TextureImporter;
            texture.textureType = TextureImporterType.Sprite;
            texture.spritePixelsPerUnit = 1;
            texture.filterMode = FilterMode.Trilinear;
            texture.mipmapEnabled = false;
            AssetDatabase.ImportAsset(path);
            AssetDatabase.Refresh();
        }
    }

    [MenuItem("LG_Tool/一键更换字体 %#F2")]
    public static void Open()
    {
        GetWindow(typeof(LGToolEditor));
    }

    Font toChange;
    static Font toChangeFont;
    FontStyle toFontStyle;
    static FontStyle toChangeFontStyle;

    void OnGUI()
    {
        toChange = (Font)EditorGUILayout.ObjectField(toChange, typeof(Font), true, GUILayout.MinWidth(100f));
        toChangeFont = toChange;
        toFontStyle = (FontStyle)EditorGUILayout.EnumPopup(toFontStyle, GUILayout.MinWidth(100f));
        toChangeFontStyle = toFontStyle;
        if (GUILayout.Button("更换"))
        {
            Change();
        }
    }

    public static void Change()
    {
        Transform canvas = Selection.activeTransform;
        if (!canvas)
        {
            Debug.Log("NO Canvas");
            return;
        }
        Transform[] tArray = canvas.GetComponentsInChildren<Transform>(true);
        Debug.Log(tArray.Length );
        for (int i = 0; i < tArray.Length; i++)
        {
            Text t = tArray[i].GetComponent<Text>();
            if (t)
            {
                Undo.RecordObject(t, t.gameObject.name);
                t.font = toChangeFont;
                t.fontStyle = toChangeFontStyle;
                t.fontSize = 30;
                //相当于让他刷新下 不然unity显示界面还不知道自己的东西被换掉了  还会呆呆的显示之前的东西
                EditorUtility.SetDirty(t);
            }
        }
        Debug.Log("Succed");
    }



    [MenuItem("LG_Tool/一键制作预制物 %#F3")]
    static void AddPrefabs()
    {
        GameObject[] SeleGameObj = Selection.gameObjects;
        GameObject TempObj = null;
        foreach (GameObject item in SeleGameObj)
        {
            if (item.name.Contains("Scene"))
            {
                TempObj = PrefabUtility.SaveAsPrefabAsset(item, "Assets/Resources/" + "Scene/" + item.name + ".prefab");
            }else if(item.name.Contains("Panel")){
                TempObj = PrefabUtility.SaveAsPrefabAsset(item, "Assets/Resources/" + "Panel/" + item.name + ".prefab");
            }
            TempObj.SetActive(true);
        }
    }

    [MenuItem("LG_Tool/一键空物体重置 %#F4")]
    static void SetGameObject()
    {
        GameObject[] SeleGameObj = Selection.gameObjects;
        foreach (GameObject item in SeleGameObj)
        {
            item.transform.localPosition = Vector3.zero;
            item.transform.localEulerAngles = Vector3.zero;
            item.transform.localScale = Vector3.one;
        }
    }
    [MenuItem("LG_Tool/一键添加Box碰撞")]
    static void AddBoxCollider()
    {
        GameObject[] SeleGameObj = Selection.gameObjects;
        foreach (GameObject item in SeleGameObj)
        {
            if (item.GetComponent<BoxCollider>()==null)
            {
                item.AddComponent<BoxCollider>();
                item.GetComponent<BoxCollider>().center = Vector3.zero;
            }
        }
    }
    [MenuItem("LG_Tool/生成子物体包围盒")]
    static void CreateChildObjBox()
    {
        GameObject[] SeleGameObj = Selection.gameObjects;
        GameObject NewGameObj = new GameObject();
        NewGameObj.name = "ParentBox";
        foreach (GameObject item in SeleGameObj)
            item.transform.parent = NewGameObj.transform;
        Transform parent = NewGameObj.transform;
        Vector3 postion = parent.position;
        Quaternion rotation = parent.rotation;
        Vector3 scale = parent.localScale;
        Collider[] colliders = parent.GetComponentsInChildren<Collider>();
        foreach (Collider child in colliders)
            DestroyImmediate(child);
        Vector3 center = Vector3.zero;
        Renderer[] renders = parent.GetComponentsInChildren<Renderer>();
        foreach (Renderer child in renders)
            center += child.bounds.center;
        center /= parent.childCount;
        Bounds bounds = new Bounds(center, Vector3.zero);
        foreach (Renderer child in renders)
            bounds.Encapsulate(child.bounds);
        BoxCollider boxCollider = parent.gameObject.AddComponent<BoxCollider>();
        boxCollider.center = bounds.center - parent.position;
        boxCollider.size = bounds.size;
        parent.position = postion;
        parent.rotation = rotation;
        parent.localScale = scale;
    }
    [MenuItem("LG_Tool/一键设置物体Tag")]
    static void SetGameObjTag()
    {
        GameObject[] SeleGameObj = Selection.gameObjects;
        foreach (GameObject item in SeleGameObj)
        {
            AddTag("",item);
        }
    }
    static void AddTag(string tag,GameObject go) {
        if (!isHasTag(tag))
        {
            SerializedObject tagManager = new SerializedObject(go);
            SerializedProperty it = tagManager.GetIterator();
            while (it.NextVisible(true))
            {
                if (it.name == "m_TagString")
                {
                    it.stringValue = tag;
                    tagManager.ApplyModifiedProperties();
                }
            }
        }
    }
    static bool isHasTag(string tag)
    {
        for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.tags.Length; i++)
        {
            if (UnityEditorInternal.InternalEditorUtility.tags[i].Contains(tag))
                return true;
        }
        return false;
    }

}
