using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;
public class SoundManager : MonoBehaviour
{
    public Sprite onMusic;
    public Sprite offMusic;
    public Image MusicButton;

    public bool MusicOn;
    public AudioSource MusicSource;
    public AudioSource SoundSource;
    public AudioClip GoodSnd;
    public AudioClip ErrorSnd;
    public AudioClip LoseSnd;
    public AudioClip WinSnd;
    private Saver _saver;


    private void Awake() {
        _saver = GetComponent<Saver>();
        MusicOn = YandexGame.savesData.MusicOn;
    }
    private void Start() {
        if(MusicOn)
        {
            MusicButton.GetComponent<Image>().sprite = onMusic;
        }
        else if (!MusicOn)
        {
            MusicButton.GetComponent<Image>().sprite = offMusic;
        }
    }

    private void Update() {
        if(MusicOn)
        {
            MusicSource.enabled = true;
        }
        else if (!MusicOn)
        {
            MusicSource.enabled = false;
        }
    }

    public void offSound()
    {
        if (!MusicOn)
        {
            MusicButton.GetComponent<Image>().sprite = onMusic;
            MusicOn = !MusicOn;
        }
        else if (MusicOn)
        {
            MusicButton.GetComponent<Image>().sprite = offMusic;
            MusicOn = !MusicOn;
        }
        _saver.Save();
    }

    public void GoodSound()
    {
        if (MusicOn)
        {
            SoundSource.PlayOneShot(GoodSnd);
        }
    }
    public void ErrorSound()
    {
        if (MusicOn)
        {
            SoundSource.PlayOneShot(ErrorSnd);
        }
    }
    public void WinSound()
    {
        if (MusicOn)
        {
            SoundSource.PlayOneShot(WinSnd);
        }
    }
    public void LoseSound()
    {
        if (MusicOn)
        {
            SoundSource.PlayOneShot(LoseSnd);
        }
    }

    public void StopMusic()
    {
        MusicSource.Stop();
    }

    public void DisableMusicButton()
    {
        MusicButton.enabled = false;
    }

    public void PauseMusic()
    {
        MusicSource.Pause();
    }

    public void PlayMusic()
    {
        if (MusicOn)
        {
            MusicSource.Play();
        }
    }
}
