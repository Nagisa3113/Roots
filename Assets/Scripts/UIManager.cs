using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Image image;

    protected override void Awake()
    {
        base.Awake();
        FadeIn();
    }

    public void PlayeLevel(int index)
    {
        GameManager.Instance.GameStart(index);
    }

    void FadeIn()
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(.5f);
        float speed = .8f;
        float timer = 1.0f;
        float counter = 0;
        while (counter < timer)
        {
            counter += speed * Time.deltaTime * speed;
            yield return 0;
            image.color = Color.Lerp(Color.black, Color.clear, counter / timer);
        }
    }
}