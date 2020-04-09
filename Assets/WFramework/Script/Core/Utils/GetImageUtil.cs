/************************************************************
    文件: GetImageUtil.cs
    作者: kuangheng_jiaxiaocheng
    功能: 获取目录下的一个图片
*************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetImageUtil : SingletonUnity<GetImageUtil>
{

    private Dictionary<string, object> spritesDic = new Dictionary<string, object>();
    private Texture2D[] sprites; 
    /// <summary>
    /// resources 下的一个目录
    /// </summary>
    /// <param name="path"></param>
    public void LoadAllSprites(string path)
    {
        sprites = Resources.LoadAll<Texture2D>(path);
        for (int i = 0; i < sprites.Length; i++)
        {
            //Debug.Log("sprites " + i + sprites[i]);
            spritesDic.Add(sprites[i].name, sprites[i]);
        }
    }

    public Sprite GetSpriteByName(string name)
    {
        Texture2D texture = null;
        foreach(KeyValuePair<string, object> pair in spritesDic)
        {
            if(pair.Key.ToString() == name)
            {
                texture = pair.Value as Texture2D;
            }
        }

        //创建Sprite
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        sprite.name = name;
        return sprite;
    }

}
