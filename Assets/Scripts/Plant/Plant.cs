using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Plant : MonoBehaviour 
{
	private float energy;
	private float resource;

	private List<Branch> branches;
	private List<Root> roots;

	private Texture2D textureBranches;
	private Texture2D textureRoots;
	private int dimension = 256;
	private Color[] clear;

	void Start () 
	{
		energy = 0f;
		resource = 5f;

		roots = new List<Root>();
		roots.Add(new Root());

		branches = new List<Branch>();
		branches.Add(new Branch(Vector3.zero));

		textureBranches = new Texture2D(dimension, dimension, TextureFormat.ARGB32, false);
		textureBranches.filterMode = FilterMode.Point;

		textureRoots = new Texture2D(dimension, dimension, TextureFormat.ARGB32, false);
		textureRoots.filterMode = FilterMode.Point;

		clear = new Color[dimension*dimension];
		for (int i = 0; i < dimension*dimension; ++i) clear[i] = Color.clear;
		textureBranches.SetPixels(0, 0, dimension, dimension, clear);
		textureBranches.Apply();
		textureRoots.SetPixels(0, 0, dimension, dimension, clear);
		textureRoots.Apply();

		renderer.material.SetTexture("_TextureBranches", textureBranches);
		renderer.material.SetTexture("_TextureRoots", textureRoots);
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

			Vector3 position = branch.position;
			position.x = Mathf.Max(0f, Mathf.Min(dimension, position.x + dimension / 2f));
			position.y = Mathf.Max(0f, Mathf.Min(dimension, position.y + dimension / 2f));
			textureBranches.SetPixel((int)position.x, (int)position.y, Color.green);
			//Manager.Instance.SetScreenBounds(position);
		}

		textureBranches.Apply();
	}

	public bool IsBranchAt (int x, int y)
	{
		return textureBranches.GetPixel(x, y).g > 0f;
	}
}
