using UnityEngine;
using System.Collections;

/// <summary>
/// The class that controls gameplay flow.
/// </summary>
public class GameplayManager : MonoBehaviour
{
	public static GameplayManager Instance;
	
	enum GameState
	{
		InGame,		// Player can start shooting with the left mouse button
		LevelFailed, // Player needs to restart the level
		LevelCompleted, // Player can move to the next level
		GameOver,	// Game ended, player input is blocked
	};
	GameState state = GameState.GameOver;
	
	int currentLevel;

	public bool UIBlocked { get; private set; }

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
	{
		Instance = this;
	}

	/// <summary>
	/// Called when this behaviour gets initialized.
	/// See Unity docs for more information.
	/// </summary>
	void Start()
	{
		currentLevel = 1;

		LevelManager.Instance.StartLevel(currentLevel);
		UIManager.Instance.ShowHUD(false);
		UIManager.Instance.ShowScreen("Tutorial");
	}

	/// <summary>
	/// Called once per frame.
	/// See Unity docs for more information.
	/// </summary>
	void Update()
	{
		// Right-click when in-game or when the player has failed the level resets the level
		if (Input.GetMouseButtonUp(1) && (state == GameState.InGame || state == GameState.LevelFailed))
		{
			OnRetryLevel();
		}
	}

	/// <summary>
	/// Logic that runs when we are requested to start gameplay.
	/// </summary>
	public void OnStartGame()
	{
		// Show the HUD
		UIManager.Instance.ShowHUD(true);

		// Hide all UI screens
		UIManager.Instance.ShowScreen("");

		// Invoke calls a function in the future. In this case, we are doing this to prevent
		// the level from receiving the mouse events from dismissing the screen.
		Invoke("StartGame", 0.1F);
	}

	/// <summary>
	/// Sets our game state to in-game so the player can start interacting with the level.
	/// </summary>
	void StartGame()
	{
		state = GameState.InGame;
	}

	/// <summary>
	/// Logic that runs when we are requested to restart the current level.
	/// </summary>
	public void OnRetryLevel()
	{		
		// Reload current level
		LevelManager.Instance.StartLevel(currentLevel);

		// Start gameplay and update UI
		UIManager.Instance.UpdateHUD(currentLevel, 0, Rank.Bronze);
		UIManager.Instance.ShowScreen("");
		state = GameState.InGame;
	}

	/// <summary>
	/// Logic that runs when we are requested to move to the next level.
	/// </summary>
	public void OnNextLevel()
	{
		// Advance to the next level
		currentLevel = (currentLevel == LevelManager.Instance.levels.Length) ? 1 : currentLevel + 1;

		// Re-use retry level logic since it resets the level for us
		OnRetryLevel();
	}

	/// <summary>
	/// Raises the restart event.
	/// </summary>
	public void OnRestart()
	{
		// Reload the current scene
		Application.LoadLevel(Application.loadedLevel);
	}

	/// <summary>
	/// Logic that runs when the level tells us it has been completed.
	/// </summary>
	public void OnLevelCompleted()
	{
		state = GameState.LevelCompleted;

		// Show the Game Complete screen if we have finished all the levels
		if (currentLevel == LevelManager.Instance.levels.Length)
		{
			UIManager.Instance.ShowScreen("Game Complete");
		}
		// Otherwise show the Level Complete Screen
		else
		{
			UIManager.Instance.ShowScreen("Level Complete");
		}
	}

	/// <summary>
	/// Logic that runs when we have failed a level.
	/// </summary>
	public void OnLevelFailed()
	{
		state = GameState.LevelFailed;
		UIManager.Instance.ShowScreen("Game Over");
	}

	/// <summary>
	/// Determines whether the player can launch to start gameplay.
	/// </summary>
	/// <returns><c>true</c> if the player can shoot; otherwise, <c>false</c>.</returns>
	public bool CanShoot()
	{
		// The player can only shoot if we're actually in-game.
		return state == GameState.InGame;
	}

	public void OnLanguageChanged()
	{
		UIManager.Instance.OnLanguageChanged();
	}

	public void BlockInput(bool block)
	{
		UIBlocked = block;
	}
}