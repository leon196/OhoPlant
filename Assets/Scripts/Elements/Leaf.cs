using UnityEngine;
using System.Collections;

public class Leaf : Element
{
	public GameObject gameObject;
	private Vector3 scaleInitial;

	// For overlapping issue
	private float zIndex;
	public static float s_zIndex = 0f; 

	public Leaf (Vector3 position_, Vector3 direction_) 
	{
		position = position_;
		direction = direction_;

		gameObject = GameObject.Instantiate(Manager.Instance.Leaf, position, Quaternion.Euler(direction)) as GameObject;
		s_zIndex += 0.01f;
		zIndex = s_zIndex;

		scaleInitial = gameObject.transform.localScale;
	}

	public void MoveTo (Vector3 position_)
	{
		gameObject.transform.position = Manager.Instance.GetTransformPosition(position_) + new Vector3(0, 0, -zIndex);
	}

	public void Animate (float energy)
	{
		//energy = 1f - energy;
		//gameObject.transform.localScale = scaleInitial + new Vector3(Mathf.Cos(Time.time) * 0.01f * energy, (1.5f + Mathf.Sin(Time.time)) * 0.01f * energy, 0f);
	}
}
