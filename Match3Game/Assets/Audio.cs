using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
	public static Audio instance;
	public AudioSource musicSource;
	public AudioSource sfxSource;
	public AudioClip[] musicTracks;
	public AudioClip[] sfxTracks;
	private Dictionary<string, AudioClip> sfx;

	private int nowTrack = 0;
	private bool playMusic = true;

	private void Awake()
	{
		SingletonFunc();
		StartMusicLoop();
		FillSFXDict();
	}
	private void SingletonFunc()
	{
		if (Audio.instance == null)
		{
			Audio.instance = this;
		}
		else
		{
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad(this);
	}
	private void FillSFXDict()
	{
		sfx = new Dictionary<string, AudioClip>();
		foreach(AudioClip clip in sfxTracks)
		{
			sfx.Add(clip.name, clip);
		}
	}
	private void StartMusicLoop()
	{
		nowTrack = 0;
		playMusic = true;
		StartCoroutine("MusicLoop");
	}
	private IEnumerator MusicLoop()
	{
		while(playMusic)
		{
			musicSource.clip = musicTracks[nowTrack];
			musicSource.Play();
			Debug.Log(musicSource.clip.name);
			yield return new WaitWhile(() => musicSource.isPlaying == true);
			nowTrack++;
			nowTrack %= musicTracks.Length;
			
		}
	}
	public void PlaySFX(string name)
	{
		if(sfxSource.isPlaying == false)
		{
			if(sfx.ContainsKey(name))
			{
				sfxSource.clip = sfx[name];
				sfxSource.Play();
			}
			else
			{
				Debug.LogWarning("SFX with given name does not exist: " + name);
			}
		}
	}
}
