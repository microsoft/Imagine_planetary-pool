using UnityEngine;
using System.Collections;

/// <summary>
/// The behaviour that applies gravity to the object it is attached to.
/// </summary>
public class GravityController : MonoBehaviour
{
	public GameObject quad;

	bool gravityEnabled;
	Rigidbody rigidBodyComp;

	const float maxGravityMagnitude = 5000.0F; // Larger values than this cause extremely large position updates causing objects to miss collisions

	/// <summary>
	/// Enables and disables gravity.
	/// </summary>
	/// <param name="bEnabled">Whether gravity should be enabled or disabled.</param>
	public void SetGravityEnabled(bool bEnabled)
	{
		// Setting a rigidbody to kinematic stops it from using engine physics to move.
		rigidBodyComp.isKinematic = !bEnabled;
		gravityEnabled = bEnabled;
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		rigidBodyComp = GetComponent<Rigidbody>();
		gravityEnabled = false;

		var quadObj = Instantiate<GameObject>(quad);
		quadObj.transform.SetParent(GetComponent<Transform>(), false);
	}

	/// <summary>
	/// FixedUpdate is called at fixed intervals to allow smooth physics simulation.
	/// See Unity docs for more information.
	/// </summary>
	void FixedUpdate()
	{
		// Apply gravity to this object if it's enabled
		if (gravityEnabled)
		{
			Vector3 gravity = CalculateGravity(rigidBodyComp.position, rigidBodyComp.mass);

			if (gravity.magnitude > maxGravityMagnitude)
			{
				GameObject.Destroy(gameObject);
			}
			// Debug visuals
			Debug.DrawRay(rigidBodyComp.position, gravity * 0.05F, Color.green);

			rigidBodyComp.AddForce(gravity);
		}
	}

	/// <summary>
	/// Calculates the gravity that should be applied to this object.
	/// </summary>
	/// <returns>The force of gravity to be applied on this object.</returns>
	/// <param name="position">World position.</param>
	/// <param name="mass">Mass.</param>
	public static Vector3 CalculateGravity(Vector3 position, float mass)
	{
		Vector3 combinedForce = Vector3.zero;

		// *** Add your source code here ***

		combinedForce = Vector3.ClampMagnitude(combinedForce, maxGravityMagnitude - 1);
		return combinedForce;
	}
}
