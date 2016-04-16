using UnityEngine;
using System.Collections;

public class BlackHoleSound : MonoBehaviour {

	public AudioClip blackHoleSound;

	AudioSource blackHoleSource;

	// Use this for initialization
	void Start()
	{
		blackHoleSource = AudioHelper.CreateAudioSource(gameObject, blackHoleSound);
	}
	
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Target"))
		{
			blackHoleSource.Play();
		}
	}
}
