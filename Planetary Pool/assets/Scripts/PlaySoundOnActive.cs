using UnityEngine;
using System.Collections;

/// <summary>
/// Script to play a sound when the game object is activated.
/// </summary>
public class PlaySoundOnActive : MonoBehaviour
{
	public AudioClip sound;	// Sound to be played.

	AudioSource source;

	void Awake()
	{
		source = AudioHelper.CreateAudioSource(gameObject, sound);
	}

	void OnEnable()
	{
		source.Play();
	}
}
