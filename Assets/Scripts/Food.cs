using UnityEngine;
using System.Collections;

public class Food
{
	GameObject foodObject;
	Vector3 scale;

	public GameObject Create (Vector3 position)
	{
		this.foodObject = GameObject.Instantiate(Master.Instance.PrefabFood) as GameObject;
		Vector3 foodPosition = this.foodObject.transform.position;
		foodPosition.x = position.x;
		foodPosition.z = position.z;
		this.foodObject.transform.position = foodPosition;

		this.scale = Vector3.one;

		return this.foodObject;
	}

	public void Update () 
	{
		float time = Time.time * 8f;
		this.scale.x = 1f + 0.1f * Mathf.Cos(time);
		this.scale.z = 1f + 0.1f * Mathf.Sin(time);
		this.foodObject.transform.localScale = this.scale;
	}

	public void Restarting (float ratio)
	{
		float time = Time.time * 8f;
		this.scale.x = 1f + 0.1f * Mathf.Cos(time);
		this.scale.z = 1f + 0.1f * Mathf.Sin(time);
		this.foodObject.transform.localScale = this.scale * (1f - ratio);
	}

	public Vector3 Position { get { return this.foodObject.transform.position; } }

	public void Clean ()
	{
		GameObject.Destroy(this.foodObject);
	}
}
