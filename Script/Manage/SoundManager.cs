using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("#BGM")]
    public AudioClip[] bgmClips;
    public float bgmVolume = 1.0f;
    AudioSource[] bgmPlayers;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume = 1.0f;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Bgm { Main, Stage1, Stage2, Stage3 }
    public enum Sfx { Sword, Setting1, Setting2, Attack, Ice, LvUp }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �� ������Ʈ�� �� ��ȯ �� �ı����� �ʵ��� ����
            Init();
        }
        else if (instance != this)
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� �����ϸ� �� ������Ʈ�� �ı�
        }
    }

    public void Init()
    {
        // ����� �÷��̾� �ʱ�ȭ
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayers = new AudioSource[bgmClips.Length];
        for (int index = 0; index < bgmPlayers.Length; index++)
        {
            AudioSource bgmPlayer = bgmObject.AddComponent<AudioSource>();
            bgmPlayer.playOnAwake = false;
            bgmPlayer.loop = true;
            bgmPlayer.volume = bgmVolume;
            bgmPlayer.clip = bgmClips[index]; // �� bgmPlayer�� �ش��ϴ� ����� Ŭ�� �Ҵ�
            bgmPlayers[index] = bgmPlayer; // bgmPlayers �迭�� �Ҵ�
        }

        // ȿ���� �÷��̾� �ʱ�ȭ
        GameObject sfxObject = new GameObject("sfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++) {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    public void PlayBgm(bool isPlay, Bgm bgm)
    {
        int bgmIndex = (int)bgm;

        if (isPlay)
        {
            bgmPlayers[bgmIndex].Play(); // �ش� bgmPlayer ���
        }
        else
        {
            bgmPlayers[bgmIndex].Stop(); // �ش� bgmPlayer ����
        }
    }


    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0;index < sfxPlayers.Length; index++) {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }

    public void StopAllAudio()
    {
        // ����� ��� ����
        foreach (AudioSource bgmPlayer in bgmPlayers)
        {
            bgmPlayer.Stop();
        }

        // ȿ���� ��� ����
        foreach (AudioSource sfxPlayer in sfxPlayers)
        {
            sfxPlayer.Stop();
        }
    }

    public void SetBgmVolume(float volume)
    {
        bgmVolume = volume;

        foreach (AudioSource bgmPlayer in bgmPlayers)
        {
            bgmPlayer.volume = bgmVolume;
        }
    }

    public void SetSfxVolume(float volume)
    {
        sfxVolume = volume;

        foreach (AudioSource sfxPlayer in sfxPlayers)
        {
            sfxPlayer.volume = sfxVolume;
        }
    }
}
