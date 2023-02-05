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
    public AudioSource swoosh;

    public void PlayAudio()
    {
    }

    public void GameStart()
    {
        title.Stop();
        growth.PlayDelayed(1.2f);
        gameplay.PlayDelayed(1f);
    }

    public void Win()
    {
        win.Play();
        growth.Stop();
        gameplay.Stop();
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