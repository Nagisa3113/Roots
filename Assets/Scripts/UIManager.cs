using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Image image;

    protected override void Awake()
    {
        base.Awake();
        image.enabled = true;
        FadeIn();
    }

    public void PlayeLevel(int index)
    {
        GameManager.Instance.GameStart(index);
    }

    public void FadeIn()
    {
        StartCoroutine(Fade(Color.black, Color.clear));
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(Color.clear, Color.black));
    }

    IEnumerator Fade(Color src, Color des)
    {
        yield return new WaitForSeconds(.5f);
        float speed = .8f;
        float timer = 1.0f;
        float counter = 0;
        while (counter < timer)
        {
            counter += speed * Time.deltaTime * speed;
            yield return 0;
            image.color = Color.Lerp(src, des, counter / timer);
        }
    }
}