using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TipsView : MonoBehaviour
{
    public void StartTips(string content)
    {
        TweenPosition tp = transform.GetComponent<TweenPosition>();
        TweenAlpha ta = transform.GetComponent<TweenAlpha>();
        Text label = transform.Find("TipsSprite/Label").GetComponent<Text>();
        RectTransform sprite = transform.Find("TipsSprite").GetComponent<RectTransform>();
        label.text = content;
        int length = CalculateLengthOfText(content,label);
        sprite.GetComponent<RectTransform>().sizeDelta = new Vector2 (length, label.fontSize*2);
        ta.onFinished=()=> { ReturnPlayStop(); };
        tp.Play(true);
        ta.Play(true);
    }

    /// <summary>
    /// 计算字符串在指定text控件中的长度
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    int CalculateLengthOfText(string message, Text tex)
    {
        int totalLength = 0;
        Font myFont = tex.font;  //chatText is my Text component
        myFont.RequestCharactersInTexture(message, tex.fontSize, tex.fontStyle);
        CharacterInfo characterInfo = new CharacterInfo();

        char[] arr = message.ToCharArray();

        foreach (char c in arr)
        {
            myFont.GetCharacterInfo(c, out characterInfo, tex.fontSize);

            totalLength += characterInfo.advance;
        }

        return totalLength/2;
    }



    void ReturnPlayStop()
    {
        Destroy(gameObject);
    }
}
