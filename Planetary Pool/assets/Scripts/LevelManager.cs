using UnityEngine;
using System.Collections;

/// <summary>
/// Our manager for loading levels and tracking level progress.
/// </summary>
public class LevelManager : MonoBehaviour
{
	public static LevelManager Instance;

	// Custom data structure for a level.
	// This allows us to set all of these fields in the Unity editor.
	[System.Serializable]
	public class Level
	{
		public GameObject prefab;
		public int goldRequirement;
		public int silverRequirement;
	};

	public Level[] levels;
	public Transform spawnTransform;

	GameObject currentLevel;
	int currentLevelID = 1;

	Rank lastRank;
	int score = 0;
	bool scoring = false;
	bool targetActive = false;

	void Awake()
	{
		Instance = this;
	}

	/// <summary>
	/// Starts the level.
	/// </summary>
	/// <param name="level">The level index.</param>
	public void StartLevel(int level)
	{
		// Clear objects from previous level first
		if (currentLevel)
		{
			Destroy(currentLevel);
		}

		// Spawn the new level
		int index = Mathf.Clamp(level - 1, 0, levels.Length - 1);
		currentLevel = Instantiate(levels[index].prefab, spawnTransform.position, spawnTransform.rotation) as GameObject;
		currentLevelID = level;

		// Reset level progress information
		score = 0;
		lastRank = Rank.Bronze;
		scoring = false;
		targetActive = false;
		UIManager.Instance.UpdateHUD(currentLevelID, score, lastRank);
	}

	/// <summary>
	/// Logic to run when the player object starts moving.
	/// </summary>
	public void OnPlayerActivated()
	{
		// The level has been started, so we can start scoring now.
		scoring = true;
	}

	/// <summary>
	/// Logic to run when the target object starts moving.
	/// </summary>
	public void OnTargetActivated()
	{
		targetActive = true;
	}

	/// <summary>
	/// Logic to run when the player object has stopped moving (it has collided with a planet).
	/// </summary>
	public void OnPlayerStopped()
	{
		// If the target ball isn't in motion, this attempt at completing the level is over.
		if (scoring && !targetActive)
		{
			GameplayManager.Instance.OnLevelFailed();
			StopScoring();
		}
	}

	/// <summary>
	/// Raises the target stopped event.
	/// </summary>
	/// <param name="success">True if the level was successfully completed.</param>
	public void OnTargetStopped()
	{
		// The target has hit something, so this attempt at completing the level is over.
		StopScoring();
	}

	/// <summary>
	/// Called to request the level manager to stop scoring this level attempt.
	/// </summary>
	public void StopScoring()
	{
		scoring = false;
	}

	public void Update()
	{
		// *** Add your source code here ***
	}

	/// <summary>
	/// Gets the rank of the current level attempt.
	/// </summary>
	/// <returns>The rank of the current level attempt.</returns>
	public Rank GetRank()
	{
		var rank = Rank.Bronze;

		// Cross-reference the level information we have to determine the rank
		if (score >= levels[currentLevelID - 1].goldRequirement)
		{
			rank = Rank.Gold;
		}
		else if (score >= levels[currentLevelID - 1].silverRequirement)
		{
			rank = Rank.Silver;
		}

		return rank;
	}
}