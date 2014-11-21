using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {

	public float minEnergy = 0.5f;

	private Controls controls;
	private Texture2D water;
	private List<Droplet> droplets;
	private List<int> recycle;
	private Color[] clear;
	private int dimension = 128;
	private float waterLastTime = 0f;
	private float waterTimeDelay = 0.1f;

	// Use this for initialization
	void Start () {
		controls = GetComponent<Controls>();	
		
		water = new Texture2D(dimension, dimension, TextureFormat.ARGB32, false);
		droplets = new List<Droplet>();
		recycle = new List<int>();

		Manager.Instance.Water.renderer.material.mainTexture = water;
		
		clear = new Color[dimension*dimension];
		for (int i = 0; i < dimension*dimension; ++i) 
			clear[i] = Color.clear;

		water.SetPixels(0, 0, dimension, dimension, clear);
		
	}
	
	// Update is called once per frame
	void Update () {

		// Clear
		water.SetPixels(0, 0, dimension, dimension, clear);

		// Spawn
		if (controls.GetCloudDirection().y > 0.5) {
			
			Vector3 cloudPosition = controls.GetCloudDirection();
			float cloudRadius = controls.GetCloudRadius();

			//cloudPosition.y *= -1f;
			cloudPosition *= 3f/10f * dimension;

			// From world to texture
			cloudPosition.x = Mathf.Max(0f, Mathf.Min(dimension, cloudPosition.x + dimension / 2f));
			//cloudPosition.y = Mathf.Max(0f, Mathf.Min(dimension, cloudPosition.y + dimension / 2f));


			int count = (int)(cloudRadius/5f * dimension) - 2;
			if (waterLastTime + waterTimeDelay < Time.time) {
				waterLastTime = Time.time;
				for (int i = 0; i < count; ++i) {
					Vector3 position = new Vector3();
					float ratio = i / (float)count;
					position.x = cloudPosition.x + i - count/2;
					position.y = dimension/2;//cloudPosition.y;
					water.SetPixel((int)position.x, (int)position.y, Color.blue);

					Droplet droplet;
					float speed = 10f;
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
		
		// Update
		for (int i = 0; i < droplets.Count; ++i) 
		{
			// 
			Droplet droplet = droplets[i];
			if (droplet != null) 
			{
				// Out of screen
				if (droplet.position.y > dimension) 
				{
					droplets[i] = null;
					recycle.Add(i);
				} 
				else 
				{
					water.SetPixel((int)droplet.position.x, (int)droplet.position.y, Color.blue);
					
					droplets[i].ApplyGravity();
				}
			}
		}
		water.Apply();
	}

	public float GetGlobalLight () {
		return Mathf.Max(minEnergy, (Mathf.Cos(controls.GetSunAngle() - Mathf.PI / 2f) + 1f) * 0.5f);
	}
}
