using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Plant : MonoBehaviour 
{
	private float energy;
	private float resource;
	private float growthLastTime;
	private float growthDelay;

	private List<Branch> branches;
	private List<Root> roots;

	private Texture2D textureBranches;
	private Texture2D textureRoots;
	private Color[] clear;
	private int dimension;

	private Game game;

	private bool gotResource;
	private bool gotEnergy;
	private bool gotCycleGrowth;

	void Start () 
	{
		game = GetComponent<Game>();
		dimension = Manager.Instance.Game.dimension;

		roots = new List<Root>();
		roots.Add(new Root(Vector3.zero));
		roots.Add(new Root(Vector3.zero));
		roots.Add(new Root(Vector3.zero));

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

		energy = 0f;
		resource = 20f;
		growthDelay = 0.05f / game.worldSpeed;
		growthLastTime = Time.time;
	}
	
	void Update () 
	{
		energy = game.GetEnergy();

		gotEnergy = energy > 0f;
		gotResource = resource > 0f;
		gotCycleGrowth = growthLastTime + growthDelay < Time.time;

		if (gotEnergy && gotResource && gotCycleGrowth)
		{
			for (int r = 0; r < roots.Count; ++r) 
			{
				Root root = roots[r];
				
				// Grow Root
				root.Grow();

				// Set Pixel
				Vector3 position = root.position;
				position.x = Mathf.Max(0f, Mathf.Min(dimension, position.x + dimension / 2f));
				position.y = Mathf.Max(0f, Mathf.Min(dimension, position.y + dimension / 2f));
				textureRoots.SetPixel((int)position.x, (int)position.y, Color.green);

				// 
			}
			for (int b = 0; b < branches.Count; ++b) 
			{
				Branch branch = branches[b];

				if (branch.distance < 20f) 
				{
					branch.Grow();

					Vector3 position = branch.position;
					position.x = Mathf.Max(0f, Mathf.Min(dimension, position.x + dimension / 2f));
					position.y = Mathf.Max(0f, Mathf.Min(dimension, position.y + dimension / 2f));
					textureBranches.SetPixel((int)position.x, (int)position.y, Color.green);	

					/*if (branch.distance >= 20f) {
						branches.Add(new Branch(branch.position));
						branches.Add(new Branch(branch.position));
					}*/
				}
			}

			textureRoots.Apply();
			textureBranches.Apply();

			growthLastTime = Time.time;
			growthDelay = 0.05f / game.worldSpeed;
			--resource;
		}
	}

	public bool IsBranchAt (int x, int y)
	{
		return textureBranches.GetPixel(x, y).g > 0f;
	}

	public bool IsRootAt (int x, int y)
	{
		return textureRoots.GetPixel(x, y).g > 0f;
	}

	public void AddResource (int amount = 1)
	{
		resource += amount;
	}

	public void AddRootAt (int x, int y)
	{
		textureRoots.SetPixel(x, y, Color.green);
	}

	public void AddRootWithFood (Food food)
	{
		int halfSize = food.dimension/2;
		int x = (int)Mathf.Clamp(food.position.x, halfSize, dimension - halfSize - 1);
		int y = (int)Mathf.Clamp(food.position.y, halfSize, dimension - halfSize - 1);
		Color[] colors = new Color[food.dimension * food.dimension];
		for (int i = 0; i < food.shape.Length; ++i) colors[i] = food.shape[i].r > 0f ? Color.green : Color.clear;
		textureRoots.SetPixels(x - halfSize, y - halfSize, food.dimension, food.dimension, colors);
	}
}
