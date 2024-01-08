using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public  class TextEffect : MonoBehaviour
{
    public static TextEffect Instance = null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    private IEnumerator  textFloat(TMP_Text tmp_text, textCallBack tCallBack,float interval)  //��ʾ��i���ַ���ͬʱ���յ�i���ַ������͵ȴ�Interval��ʱ��
    {
        textCallBack textCallBack = tCallBack;
        float realInterval = interval;
        int i = 0;
        int visibleCount = 0;
        bool richText = false;
        int length = tmp_text.text.Length;
        while (i < length)
        {
            realInterval = Interval(tmp_text.text[i], interval, ref richText,ref visibleCount);
            while (richText == true)
            {
                i++;
                realInterval = Interval(tmp_text.text[i], interval, ref richText,ref visibleCount);
            }
            tmp_text.maxVisibleCharacters = visibleCount;
            Debug.Log( tmp_text.text[i] + "  " + visibleCount+" " +tmp_text.textInfo.characterCount + "  " + tmp_text.text.Length);
            i++;
            if (realInterval > 0)
            { yield return new WaitForSeconds(realInterval); }
        }
        textCallBack();
    }

    private float Interval(char letter, float interval,ref bool richText,ref int visibleCount)
    {
        if(letter == '<')
        {
            richText = true;
            return 0;
        } 
        else if(letter == '>')
        {
            richText = false;
            return 0;
        }
        else if (letter != '>' && richText == true) 
        {
            return 0;
        }

        //����if�ж�textmeshpro��rich Text��ǩ,�������rich Text���ǿɼ��ַ������з����벹��,\nҲ��һ���ɼ��ַ�
        visibleCount++;
        if (letter == '��')
        {
            return interval * 2f;
        }
        else if(letter == '��')
        {
            return interval * 2.5f;
        }
        else if (letter == ' ' || letter == ' ' || letter == '\n')
        {
            return 0;
        }
        else//��ͨ����
        {
            return interval;
        }
    }
    public void StartTextFloat(TMP_Text tmp_text, textCallBack tCallBack, float interval = 0.1f) {
        StartCoroutine(textFloat(tmp_text, tCallBack,interval));
    }
    public void StartTextFloat(TMP_Text tmp_text, float interval = 0.1f)
    {

        StartCoroutine(textFloat(tmp_text, () => { return 0; },interval));//��Ҫ�󷵻�ֵ�Ļ�����һ���յĺ���
    }

    private IEnumerator textAppear(TMP_Text tmp_text, textCallBack tCallBack, float duration = 1f)
    {
        Color color = tmp_text.color;
        color.a = 0f;
        tmp_text.color = color;
        while (tmp_text.color.a < 1f)
        {
            color.a +=  0.1f;
            tmp_text.color = color;
            yield return new WaitForSeconds(duration * 0.1f);
        }
        color.a = 1f;
        tmp_text.color = color;
        tCallBack();
    }

    public void StartTextAppear(TMP_Text tmp_text, float duration = 1f)
    {

        StartCoroutine(textAppear(tmp_text, () => { return 0; }, duration));

    }
    public void StartTextAppear(TMP_Text tmp_text, textCallBack tCallBack, float duration = 1f)
    {

        StartCoroutine(textAppear(tmp_text, tCallBack, duration));

    }
    public delegate int textCallBack();
}
