using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Water : MonoBehaviour 
{
	private Texture2D water;
	private List<Droplet> droplets;
	private List<int> recycle;
	private Color[] clear;
	private int dimension = 256;
	private float waterLastTime = 0f;
	private float waterTimeDelay = 0.05f;

	private Game game;
	private Controls controls;
	private Shaders shaders;
	private Plant plant;

	void Start ()
	{
		game = GetComponent<Game>();
		controls = GetComponent<Controls>();	
		shaders = GetComponent<Shaders>();
		plant = GetComponent<Plant>();

		water = new Texture2D(dimension, dimension, TextureFormat.ARGB32, false);
		water.filterMode = FilterMode.Point;
		droplets = new List<Droplet>();
		recycle = new List<int>();

		renderer.material.SetTexture("_TextureWater", water);
		
		clear = new Color[dimension*dimension];
		for (int i = 0; i < dimension*dimension; ++i) 
			clear[i] = Color.clear;

		water.SetPixels(0, 0, dimension, dimension, clear);
	}

	void Update ()
	{

		// Clear
		water.SetPixels(0, 0, dimension, dimension, clear);

		// Spawn
		if (controls.GetCloudDirection().y > 0f) {
			
			Vector3 cloudPosition = controls.GetCloudDirection() * shaders.CloudDistance * dimension;

			// From world to texture
			cloudPosition.x = Mathf.Max(0f, Mathf.Min(dimension, cloudPosition.x + dimension / 2f));
			cloudPosition.y = Mathf.Max(0f, Mathf.Min(dimension, cloudPosition.y + dimension / 2f));

			int count = (int)(shaders.CloudRadius * dimension);
			if (waterLastTime + waterTimeDelay < Time.time) {
				waterLastTime = Time.time;
				for (int i = 0; i < count; ++i) {
					Vector3 position = new Vector3();
					float ratio = i / (float)count;
					position.x = cloudPosition.x + i - count / 2 + 1;
					position.y = cloudPosition.y + Random.Range(-1, 1);//- Mathf.Floor(i / 4f) - (i % 4);// + Random.Range(-2, 2);
					// - ((Mathf.Sin(ratio * Mathf.PI)) / 2f) * shaders.CloudRadius * dimension * 2;

					if (!IsWaterAt((int)position.x, (int)position.y)) {
						water.SetPixel((int)position.x, (int)position.y, Color.blue);

						Droplet droplet;
						float speed = 50f;
						if (recycle.Count > 0) {
							int index = recycle[0];
						 	droplet = new Droplet(index, speed, position);
							droplets[index] = droplet;
							recycle.RemoveAt(0);
						} else {
						 	droplet = new Droplet(droplets.Count, speed, position);
							droplets.Add(droplet);
						}
					}
				}
			}
		}
		
		// Update
		for (int i = 0; i < droplets.Count; ++i) 
		{
			// 
			if (droplets[i] != null) 
			{
				// Move
				droplets[i].ApplyGravity(game.worldSpeed);
				Vector3 position = droplets[i].position;

				bool collision = plant.IsBranchAt((int)position.x, (int)position.y);
				bool outOfGround = position.y >= dimension || position.y < 0 || position.x < 0 || position.x >= dimension;

				// Clear
				if (collision || outOfGround)
				{
					droplets[i] = null;
					recycle.Add(i);
				} 
				// Draw
				else 
				{
					water.SetPixel((int)position.x, (int)position.y, Color.blue);
				}
			}
		}
		water.Apply();
	}

	public bool IsWaterAt (int x, int y)
	{
		return water.GetPixel(x, y).b > 0f;
	}
}