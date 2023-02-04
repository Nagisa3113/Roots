using UnityEngine;

public class AudioController : Singleton<AudioController>
{
    public AudioSource growth;
    public AudioSource lose;
    public AudioSource win;
    public AudioSource gameplay;
    public AudioSource title;
    public AudioSource change;
    public AudioSource impact;

    public void PlayAudio()
    {
    }

    public void GameStart()
    {
        title.Stop();
        growth.Play();
        gameplay.Play();
    }

    public void Win()
    {
        win.Play();
    }

    public void Lose()
    {
        lose.Play();
        growth.Stop();
        gameplay.Stop();
    }

    public void BackToTitle()
    {
        title.Play();
        growth.Stop();
        gameplay.Stop();
    }

    public void PlayChange()
    {
        change.Play();
    }

    public void PlayImpact()
    {
        impact.Play();
    }
}