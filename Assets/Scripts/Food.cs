using UnityEngine;
using System.Collections;

public class Food
{
	GameObject foodObject;

	public GameObject Create (Vector3 position)
	{
		this.foodObject = GameObject.Instantiate(Master.Instance.PrefabFood);
		Vector3 foodPosition = this.foodObject.transform.position;
		foodPosition.x = position.x;
		foodPosition.z = position.z;
		this.foodObject.transform.position = foodPosition;

		return this.foodObject;
	}

	public Vector3 Position { get { return this.foodObject.transform.position; } }

	public void Clean ()
	{
		GameObject.Destroy(this.foodObject);
	}
}
