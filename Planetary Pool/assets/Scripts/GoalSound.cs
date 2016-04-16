using UnityEngine;
using System.Collections;

public class GoalSound : MonoBehaviour
{
	public AudioClip goalHitSound;
	
	AudioSource goalHitSource;
	
	// Use this for initialization
	void Start()
	{
		goalHitSource = AudioHelper.CreateAudioSource(gameObject, goalHitSound);
	}
	
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Target"))
		{
			goalHitSource.Play();
		}
	}
}
