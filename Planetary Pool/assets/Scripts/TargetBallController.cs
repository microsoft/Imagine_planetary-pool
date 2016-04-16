using UnityEngine;
using System.Collections;

/// <summary>
/// Behaviour to control the target object.
/// </summary>
public class TargetBallController : MonoBehaviour
{
	bool activated;

	GravityController gravityController;
	Rigidbody rigidBody;

	public AudioClip targetHitSound;
	
	AudioSource targetHitSource;

	// Use this for initialization
	void Start()
	{
		targetHitSource = AudioHelper.CreateAudioSource(gameObject, targetHitSound);

		gravityController = GetComponent<GravityController>();
		rigidBody = GetComponent<Rigidbody>();

		activated = false;	
	}

	/// <summary>
	/// This is called once per frame.
	/// See Unity docs for more information.
	/// </summary>
	void Update()
	{
		// Update the UI manager with our current position
		UIManager.Instance.ShowTargetIndicator(rigidBody.position);
	}

	/// <summary>
	/// Called when this object collides with another rigidbody.
	/// See Unity docs for more information.
	/// </summary>
	/// <param name="collision">Collision.</param>
	void OnCollisionEnter(Collision collision)
	{
		// If the player hits us and we aren't moving yet, start moving
		if (!activated && collision.gameObject.CompareTag("Player"))
		{

			LevelManager.Instance.OnTargetActivated();
			activated = true;
			gravityController.SetGravityEnabled(true);
			targetHitSource.Play();
		}
		else if (activated)
		{
			// If we're moving and we hit a planet that isn't the goal, stop moving, we lost
			if (collision.gameObject.CompareTag("Planet"))
			{
				LevelManager.Instance.OnTargetStopped();
				GameplayManager.Instance.OnLevelFailed();
				gravityController.SetGravityEnabled(false);
				GameObject.Destroy(gameObject);
			}
			// If the planet was the goal planet, stop moving, but we won
			else if (collision.gameObject.CompareTag("Goal"))
			{
				LevelManager.Instance.OnTargetStopped();
				GameplayManager.Instance.OnLevelCompleted();
				gravityController.SetGravityEnabled(false);
			}
			else if (collision.gameObject.CompareTag("Player"))
			{
				targetHitSource.Play();
			}
		}
	}
}
