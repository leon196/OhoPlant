using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Root {

	//
	public Vector3 position;
	public Vector3 direction;
	//
	public List<Root> children;
	// 
	private Vector3 torsade;

	public Root (Vector3 position_) 
	{
		position = position_;
		direction = Manager.GetRandomRootDirection();
		torsade = new Vector3();

		children = new List<Root>();
	}
	
	public void Grow () 
	{
		float angleTorsade = Mathf.PI * Mathf.Cos(Time.time * 10f) * 0.5f;
		torsade.x = Mathf.Cos(angleTorsade);
		torsade.y = Mathf.Sin(angleTorsade);

		position = position + (direction + torsade + Manager.Instance.GetMoonDirection()).normalized;
	}

	public void GrowWithDelay (float delay)
	{

	}
}
