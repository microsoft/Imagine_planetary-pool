using UnityEngine;
using System.Collections;

/// <summary>
/// The behaviour that flags an object as a source of gravity.
/// </summary>
public class PlanetController : MonoBehaviour
{
	// Density of the planet (kg * 10^6 / m^3)
	public float density;

	public GameObject quad;
	public GameObject flashParticle;

	// This value is 10^12 times larger than the actual value of the gravitational constant, G.
	// We compensate for this by assuming planets have densities 10^6 times larger than stated, in units kg / m^3.
	private static float gravitationalConstant = 66.7384f;

	Rigidbody rigidBody;

	// Use this for initialization
	void Start()
	{
		rigidBody = GetComponent<Rigidbody>();
		var trans = GetComponent<Transform>();

		// Calculate the mass based on the object's scale and density.
		rigidBody.mass = density * (4.0F / 3.0F) * Mathf.PI * Mathf.Pow(trans.localScale.x, 3);

		var quadObj = Instantiate<GameObject>(quad);
		quadObj.transform.SetParent(trans, false);
	}

	/// <summary>
	/// Gets the force of gravity this planet should apply.
	/// </summary>
	/// <returns>The force of gravity to apply.</returns>
	/// <param name="position">Position of the object to apply gravity to.</param>
	/// <param name="mass">Mass of the object to apply gravity to.</param>
	public Vector3 GetGravityForce(Vector3 position, float mass)
	{
		Vector3 forceOfGravity = Vector3.zero;

		// *** Add your source code here ***

		return forceOfGravity;
	}

	public void OnCollisionEnter(Collision collision)
	{
		if (CompareTag("Goal"))
		{
			return;
		}

		var flash = GameObject.Instantiate(flashParticle, transform.position, Quaternion.identity);
		Destroy(flash, 1f);
	}
}
