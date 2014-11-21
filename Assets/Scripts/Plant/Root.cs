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

	public Root () 
	{
		position = new Vector3();
		direction = Manager.GetRandomRootDirection();
		torsade = new Vector3();

		children = new List<Root>();
	}
	
	public void Grow () 
	{
		float angleTorsade = Mathf.PI * Mathf.Cos(Time.time * 10f) * 0.2f + Mathf.PI * Mathf.Cos(Time.time * 5f) * 0.6f;
		torsade.x = Mathf.Cos(angleTorsade);
		torsade.y = Mathf.Sin(angleTorsade);

		position = position + (direction + torsade).normalized * Time.deltaTime * 10f;
	}

	public void GrowWithDelay (float delay)
	{

	}
}
