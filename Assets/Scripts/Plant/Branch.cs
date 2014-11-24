using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Branch 
{
	//
	public Vector3 position;
	public Vector3 direction;
	public float distance;
	private List<Vector3> points;
	private Vector3 lastPosition;
	// 
	private Vector3 torsade;

	public Branch (Vector3 position_) 
	{
		position = position_;
		direction = Manager.GetRandomBranchDirection();
		torsade = new Vector3();
		distance = 0f;

		points = new List<Vector3>();
		points.Add(position);
		points.Add(position);

		lastPosition = position;
	}
	
	public void Grow () 
	{
		float angleTorsade = Mathf.PI * Mathf.Cos(Time.time * 10f);
		torsade.x = Mathf.Cos(angleTorsade);
		torsade.y = Mathf.Sin(angleTorsade);

		position = position + (direction + Manager.Instance.GetSunDirection()).normalized;

		if ((int)position.x != (int)lastPosition.x || (int)position.y != (int)lastPosition.y) 
		{
			points.Add(position);
			++distance;
		}
	}

	public void GrowWithDelay (float delay)
	{

	}
}
