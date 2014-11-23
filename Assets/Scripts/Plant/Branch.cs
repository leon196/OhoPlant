using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Branch 
{
	//
	public Vector3 position;
	public Vector3 direction;
	public List<Branch> children;
	// 
	private GameObject gameObject;
	private LineRenderer line;
	private List<Vector3> points;
	private int currentPoint;
	private int lastPoint;
	private float startWidth;
	// 
	private Vector3 torsade;

	public Branch (Vector3 position_) 
	{
		position = position_;
		direction = Manager.GetRandomBranchDirection();
		torsade = new Vector3();

		children = new List<Branch>();
/*
		gameObject = new GameObject("Branch");
		line = gameObject.AddComponent<LineRenderer>();
		line.material = Manager.Instance.MaterialBranch;
		line.material.shader = Shader.Find("Custom/Branch");
		startWidth = 0.2f;
		line.SetWidth(startWidth, 0.1f);
		line.SetVertexCount(2);
		line.SetPosition(0, position);
		line.SetPosition(1, position);*/

		points = new List<Vector3>();
		points.Add(position);
		points.Add(position);
		lastPoint = 0;
		currentPoint = 1;
	}
	
	public void Grow () 
	{
		float angleTorsade = Mathf.PI * Mathf.Cos(Time.time * 10f);
		torsade.x = Mathf.Cos(angleTorsade);
		torsade.y = Mathf.Sin(angleTorsade);

		//direction = Manager.Instance.GetSunDirection();

		position = position + (direction + Manager.Instance.GetSunDirection() + torsade).normalized;
/*
		points[currentPoint] = position;
		float distance = Vector3.Distance(points[lastPoint], position);
		if (distance > 1f) 
		{
			//line.SetVertexCount(currentPoint + 2);
			points.Add(position);
			++currentPoint;
			++lastPoint;
		}
*/
		//
		//line.SetPosition(currentPoint, position);

		//
		//startWidth = 0.09f * Camera.main.orthographicSize;
		//line.SetWidth(startWidth, 0.1f);
	}

	public Vector3 CurrentPosition {
		get {
			return points[currentPoint];
		}
	}

	public void GrowWithDelay (float delay)
	{

	}
}
