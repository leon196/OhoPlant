using UnityEngine;
using System.Collections;

public class Element 
{
	// Basic
	public Vector3 position;
	public Vector3 direction;
	public float speed;

	// Specific
	public int index;
	public float velocity;
	public Vector3 lastPosition;

	// Constructor
	public Element () {}

	//
	public bool ChangePosition ()
	{
		return (int)position.x != (int)lastPosition.x || (int)position.y != (int)lastPosition.y;
	}
	public void UpdateLastPosition ()
	{
		lastPosition = position;
	}
}
