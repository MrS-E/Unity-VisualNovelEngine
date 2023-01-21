using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSideImage : MonoBehaviour
{

    public float FadeInSpeed = 0.5f;
    public GameObject gObject;
    public Vector2 size;
    public string position = "center";
    private Image thisImage;
    private int screenHeight = Screen.height;
    private int screenWidth = Screen.width;

    void Start()
    {

        size = new Vector2(size.x * (int)Math.Floor(screenHeight / size.y), size.y * (int)Math.Floor(screenHeight / size.y));
        gObject = this.gameObject;
        thisImage = this.gameObject.GetComponent<Image>();
        Color c = thisImage.color;
        thisImage.rectTransform.sizeDelta = size;
        if (position == "center") 
        {
            thisImage.rectTransform.anchoredPosition = new Vector2(0, (screenHeight - size.y) / -2);
        }
        else if(position == "right")
        {
            thisImage.rectTransform.anchoredPosition = new Vector2((screenWidth - size.x) / 2, (screenHeight - size.y) / -2);
        }
        else if(position == "left")
        {
            thisImage.rectTransform.anchoredPosition = new Vector2((screenWidth - size.x) / -2, (screenHeight - size.y) / -2);
        }
        else
        {
            thisImage.rectTransform.anchoredPosition = new Vector2(0, (screenHeight - size.y) / -2);
        }
        c.a = 0;
        thisImage.color = c;
        StartCoroutine(FadeInRoutine(FadeInSpeed));
    }

    void Update()
    {
        if(screenHeight != Screen.height || screenWidth != Screen.width)
        {
            screenHeight = Screen.height; screenWidth = Screen.width;
            size = new Vector2(size.x * (int)Math.Floor(screenHeight / size.y), size.y * (int)Math.Floor(screenHeight / size.y));
            thisImage.rectTransform.sizeDelta = size;
            if (position == "center")
            {
                thisImage.rectTransform.anchoredPosition = new Vector2(0, (screenHeight - size.y) / -2);
            }
            else if (position == "right")
            {
                thisImage.rectTransform.anchoredPosition = new Vector2((screenWidth - size.x) / 2, (screenHeight - size.y) / -2);
            }
            else if (position == "left")
            {
                thisImage.rectTransform.anchoredPosition = new Vector2((screenWidth - size.x) / -2, (screenHeight - size.y) / -2);
            }
            else
            {
                thisImage.rectTransform.anchoredPosition = new Vector2(0, (screenHeight - size.y) / -2);
            }
        }
    }

    IEnumerator FadeInRoutine(float speed)
    {
        Color c = thisImage.color;
        for (float alpha = 0f; alpha <= 1f; alpha += 0.1f)
        {
            c.a = alpha;
            thisImage.color = c;
            yield return new WaitForSeconds(speed / 20);
        }
        c.a = 1.0f;
        thisImage.color = c;
    }

    IEnumerator FadeOutRoutine(float speed)
    {
        Color c = thisImage.color;
        for (float alpha = 1f; alpha >= 0f; alpha -= 0.1f)
        {
            c.a = alpha;
            thisImage.color = c;
            yield return new WaitForSeconds(speed / 20);
        }
        Destroy(gObject);
    }

    public void KillMe()
    {
        StartCoroutine(FadeOutRoutine(FadeInSpeed));
    }
}
