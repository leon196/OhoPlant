using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Plant : MonoBehaviour 
{
	private float energy;
	private float resource;

	private List<Branch> branches;
	private List<Root> roots;

	void Start () 
	{
		energy = 0f;
		resource = 5f;

		roots = new List<Root>();
		roots.Add(new Root());

		branches = new List<Branch>();
		branches.Add(new Branch(Vector3.zero));
	}
	
	void Update () 
	{
		for (int r = 0; r < roots.Count; ++r) 
		{
			Root root = roots[r];
			root.Grow();
		}
		for (int b = 0; b < branches.Count; ++b) 
		{
			Branch branch = branches[b];
			branch.Grow();

			Vector3 position = branch.CurrentPosition;
			Manager.Instance.SetScreenBounds(position);
		}
	}
}
