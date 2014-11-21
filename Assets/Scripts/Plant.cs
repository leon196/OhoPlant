using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Plant : MonoBehaviour 
{
	private Texture2D plantMap;
	private int dimension = 256;
	private Color[] clear;

	private float energy;
	private float resource;

	private List<Root> roots;

	void Start () 
	{
		// Plant Texture
		plantMap = new Texture2D(dimension, dimension, TextureFormat.ARGB32, false);

		// Clear
		clear = new Color[dimension*dimension];
		for (int i = 0; i < dimension*dimension; ++i) clear[i] = Color.clear;
		plantMap.SetPixels(0, 0, dimension, dimension, clear);
		plantMap.Apply();

		// Set to material
		gameObject.renderer.material.mainTexture = plantMap;

		energy = 0f;
		resource = 5f;

		roots = new List<Root>();
		roots.Add(new Root());
		roots.Add(new Root());
		roots.Add(new Root());
	}
	
	void Update () 
	{
		if (resource > 0f) 
		{
			for (int r = 0; r < roots.Count; ++r) 
			{
				Root root = roots[r];
				root.Grow();

				int x = (int)Mathf.Max(0f, Mathf.Min(dimension, root.position.x + dimension / 2f));
				int y = (int)Mathf.Max(0f, Mathf.Min(dimension, root.position.y + dimension / 2f));
				plantMap.SetPixel(x, y, Color.blue);
			}
			
			plantMap.Apply();

			resource -= Time.deltaTime;
		}
	}
}
