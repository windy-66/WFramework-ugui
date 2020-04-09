using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 设置窗口.
/// </summary>
public class SettingWindows : EditorWindow
{
    string[] Modes = new string[] { "调试模式  DEBUG_MODE" , "运行模式  RUN_MODE", "打印日志  DEBUG_LOG" };

    private List<MacroItem> macroItemLists = new List<MacroItem>();

    private List<MacroItem> customItemLists = new List<MacroItem>();

    private Dictionary<string, bool> dic = new Dictionary<string, bool>();

    private string Macro = null;

    private ArrayList CustomHong;
    /// <summary>
    /// 每打开一次窗口就会执行一次
    /// </summary>
    private void OnEnable()
    {
        Macro = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);//获取宏信息  参数:获取哪个平台下的
        macroItemLists.Clear();//每次打开一次窗口都需要将数据重置.
        macroItemLists.Add(new MacroItem() { Name = "DEBUG_MODE", DisplayName = "调试模式  DEBUG_MODE" });
        macroItemLists.Add(new MacroItem() { Name = "RUN_MODE", DisplayName = "运行模式  RUN_MODE" });
        macroItemLists.Add(new MacroItem() { Name = "DEBUG_LOG", DisplayName = "打印日志  DEBUG_LOG" });
        for (int i = 0; i < Modes.Length; i++)
        {
            //macroItemLists.Add(new MacroItem() { Name = Modes[i].Split(' ')[1], DisplayName = Modes[i] });
            dic[macroItemLists[i].Name] = !string.IsNullOrEmpty(Macro) && Macro.IndexOf(macroItemLists[i].Name) != -1;
        }
        CustomHong = ReadText(Application.dataPath ,"Macro.txt");
        customItemLists.Clear();
        for (int i = 0; i < CustomHong.Count; i++)
        {
            customItemLists.Add(new MacroItem() { Name = CustomHong[i].ToString ().Split(' ')[1], DisplayName = CustomHong[i].ToString()});
            dic[customItemLists[i].Name] = !string.IsNullOrEmpty(Macro) && Macro.IndexOf(customItemLists[i].Name) != -1;
        }
    }

    public ArrayList ReadText(string path, string name = "")
    {
        StreamReader sr;
        try
        {
            string pathstr = name == "" ? path : path + "/" + name;
            if (!File.Exists(pathstr))File.Create(pathstr);
            sr = File.OpenText(pathstr);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            return null;
        }
        string line;
        ArrayList al = new ArrayList();
        while ((line = sr.ReadLine()) != null)
            al.Add(line);
        sr.Dispose();
        return al;
    }

    /// <summary>
    /// 绘制窗口条目.
    /// </summary>
    private void OnGUI()
    {
        for (int i = 0; i < macroItemLists.Count; i++)
        {
            EditorGUILayout.BeginHorizontal("box");//开启一个水平行   备注:必须成对出现
            dic[macroItemLists[i].Name] = GUILayout.Toggle(dic[macroItemLists[i].Name], macroItemLists[i].DisplayName);
            EditorGUILayout.EndHorizontal();//结束这个水平行
        }
        if (CustomHong.Count >0)
        {
            EditorGUILayout.BeginHorizontal();//开启一个水平行   备注:必须成对出现
            GUILayout.Label("自定义宏");
            EditorGUILayout.EndHorizontal();//结束这个水平行
        }
        for (int i = 0; i < customItemLists.Count; i++)
        {
            EditorGUILayout.BeginHorizontal("box");//开启一个水平行   备注:必须成对出现
            dic[customItemLists[i].Name] = GUILayout.Toggle(dic[customItemLists[i].Name], customItemLists[i].DisplayName);
            EditorGUILayout.EndHorizontal();//结束这个水平行
        }
        SaveMacro();
    }
    /// <summary>
    /// 保存宏信息
    /// </summary>
    private void SaveMacro()
    {
        Macro = string.Empty;
        foreach (KeyValuePair<string, bool> item in dic)
        {
            if (item.Value)
            {
                Macro += string.Format("{0};", item.Key);
            }
        }
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, Macro);//将信息保存到宏信息里. 参数1:保存到哪个平台  参数2:要保存的内容.
    }

    /// <summary>
    /// 宏元素.
    /// </summary>
    public class MacroItem
    {
        /// <summary>
        /// 宏名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 窗口上显示的名称
        /// </summary>
        public string DisplayName;

    }
}