using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// User interface manager.
/// </summary>
public class UIManager : MonoBehaviour
{
	public static UIManager Instance;

	// Fields to set in the Unity editor
	public GameObject[] screens;
	public GameObject hud;
	public Text levelText;
	public Text scoreText;
	public Text rankText;
	public Texture pathTexture;
	public Image playerIndicator;
	public Image targetIndicator;
	
	public AudioClip buttonClickSound;
	AudioSource buttonClickSource;

	// Information to draw expected path on launch
	Vector3[] pathPositions;
	public const int maxPositionDraws = 15;

	// Positions of moving objects in the level
	Vector3 playerPosition;
	Vector3 targetPosition;
	
	void Awake()
	{
		Instance = this;
		pathPositions = new Vector3[0];
		playerPosition = new Vector3();
		targetPosition = new Vector3();
	}

	void Start()
	{
		buttonClickSource = AudioHelper.CreateAudioSource(gameObject, buttonClickSound);
	}

	/// <summary>
	/// Show a screen.
	/// </summary>
	/// <param name="name">The name of the screen.</param>
	public void ShowScreen(string name)
	{
		// Show the screen with the given name and hide everything else
		foreach (GameObject screen in screens)
		{
			screen.SetActive(screen.name == name);
		}
	}

	/// <summary>
	/// Shows/hides the HUD.
	/// </summary>
	/// <param name="show">Do we show the HUD?</param>
	public void ShowHUD(bool show)
	{
		hud.SetActive(show);
	}

	/// <summary>
	/// Updates the HUD elements.
	/// </summary>
	/// <param name="level">The current level.</param>
	/// <param name="score">The player's score.</param>
	/// <param name="rank">The player's rank.</param>
	public void UpdateHUD(int level, int score, Rank rank)
	{
		ShowLevel(level);
		ShowScore(score);
		ShowRank(rank);
	}

	/// <summary>
	/// Updates the level text on the HUD.
	/// </summary>
	/// <param name="level">The level number.</param>
	void ShowLevel(int level)
	{
		levelText.text = string.Format(LocalizationManager.Instance.GetString("HUD Level"), level);
	}

	/// <summary>
	/// Updates the score text on the HUD.
	/// </summary>
	/// <param name="score">Score.</param>
	void ShowScore(int score)
	{
		scoreText.text = string.Format(LocalizationManager.Instance.GetString("HUD Score"), score);
	}

	/// <summary>
	/// Updates the rank text on the HUD.
	/// </summary>
	/// <param name="rank">Rank.</param>
	void ShowRank(Rank rank)
	{
		string rankStr = null;
		switch(rank)
		{
		case Rank.Bronze:
			rankStr = LocalizationManager.Instance.GetString("Rank Bronze");
			break;
		case Rank.Silver:
			rankStr = LocalizationManager.Instance.GetString("Rank Silver");
			break;
		case Rank.Gold:
			rankStr = LocalizationManager.Instance.GetString("Rank Gold");
			break;
		}

		rankText.text = string.Format(LocalizationManager.Instance.GetString("HUD Rank"), rankStr);
	}

	/// <summary>
	/// Sets the expected path positions for drawing on GUI updates.
	/// </summary>
	/// <param name="positions">Expected positions of the player object when launched.</param>
	public void SetExpectedPath(Vector3[] positions)
	{
		pathPositions = positions;
	}

	/// <summary>
	/// OnGUI is called for rendering and handling GUI events.
	/// See Unity docs for more information.
	/// </summary>
	void OnGUI()
	{
		// Cache the current color for restoring later
		Color guiColor = GUI.color;

		// *** Add your source code here ***

		// Move our offscreen indicators to the appropriate positions
		SetIndicatorPosition(playerIndicator, playerPosition);
		SetIndicatorPosition(targetIndicator, targetPosition);

		// Restore the original GUI color
		GUI.color = guiColor;
	}

	/// <summary>
	/// Sets the position and visibility for an offscreen indicator for one of the moving objects.
	/// </summary>
	/// <param name="indicator">The indicator image.</param>
	/// <param name="objectPos">Object position.</param>
	void SetIndicatorPosition(Image indicator, Vector3 objectPos)
	{
		// Convert world position to screen position
		Vector3 objectScreenPos = Camera.main.WorldToScreenPoint(objectPos);

		// Clamp the obhject position to screen boundaries
		Vector3 indicatorPos = new Vector3();
		indicatorPos.x = Mathf.Clamp(objectScreenPos.x, 0, Screen.width);
		indicatorPos.y = Mathf.Clamp(objectScreenPos.y, 0, Screen.height);
		indicatorPos.z = objectScreenPos.z;

		// If the object is offscreen, we need to draw the indicator.
		if (objectScreenPos != indicatorPos)
		{
			// Adjust indicator position to be fully on-screen, instead of halfway off-screen.
			indicatorPos.x = Mathf.Clamp(objectScreenPos.x, indicator.rectTransform.rect.width, Screen.width - indicator.rectTransform.rect.width);
			indicatorPos.y = Mathf.Clamp(objectScreenPos.y, indicator.rectTransform.rect.height, Screen.height - indicator.rectTransform.rect.height);
			indicatorPos.z = 0.0F;

			// Point the indicator in the direction of the off-screen object,
			// and move it to the correct location.
			Vector3 indicatorDir = objectScreenPos - indicatorPos;
			indicator.transform.up = indicatorDir.normalized;
			indicator.transform.position = indicatorPos;

			// Enable the indicator object to get it to show
			indicator.gameObject.SetActive(true);
		}
		else
		{
			// Disable the indicator object to get it to hide
			indicator.gameObject.SetActive(false);
		}
	}

	/// <summary>
	/// Caches the player object's position to draw an indicator on GUI updates.
	/// </summary>
	/// <param name="position">Position.</param>
	public void ShowPlayerIndicator(Vector3 position)
	{
		playerPosition = position;
	}

	/// <summary>
	/// Caches the target object's position to draw an indicator on GUI updates.
	/// </summary>
	/// <param name="position">Position.</param>
	public void ShowTargetIndicator(Vector3 position)
	{
		targetPosition = position;
	}

	public void OnButton()
	{
		buttonClickSource.Play();
	}

	public void OnLanguageChanged()
	{
		foreach (GameObject o in screens)
		{
			var staticText = o.GetComponent<StaticTextManager>();
			if (staticText)
			{
				staticText.OnLanguageChanged();
			}
		}
	}
}