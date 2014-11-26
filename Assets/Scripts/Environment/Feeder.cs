using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Feeder : MonoBehaviour 
{
	private Texture2D foodMap;
	private List<Food> foods;
	private List<int> recycle;
	private Color[] clear;
	private int dimension;
	private float foodLastTime = 0f;
	private float foodTimeDelay = 0.5f;
	private int foodCount = 0;
	private int foodMax = 16;

	private Game game;
	//private Controls controls;
	//private Shaders shaders;
	private Plant plant;

	void Start ()
	{
		game = GetComponent<Game>();
		//controls = GetComponent<Controls>();	
		//shaders = GetComponent<Shaders>();
		plant = GetComponent<Plant>();

		dimension = Manager.Instance.Game.dimension;
		foodMap = new Texture2D(dimension, dimension, TextureFormat.ARGB32, false);
		foodMap.filterMode = FilterMode.Point;
		foods = new List<Food>();
		recycle = new List<int>();

		renderer.material.SetTexture("_TextureFood", foodMap);
		
		clear = new Color[dimension*dimension];
		for (int i = 0; i < dimension*dimension; ++i) 
			clear[i] = Color.clear;

		foodMap.SetPixels(0, 0, dimension, dimension, clear);

		foodCount = 0;
		foodLastTime = Time.time;
	}

	void Update ()
	{

		// Clear
		foodMap.SetPixels(0, 0, dimension, dimension, clear);

		// Spawn
		if (foodLastTime + foodTimeDelay < Time.time && foodCount < foodMax) {
			foodLastTime = Time.time;

			Vector3 position = new Vector3();

			position.x = Random.Range(0, dimension / 2);
			position.y = Random.Range(0, dimension / 2);

			Food newFood;
			float speed = 1f;//Random.Range(0.5f, 1f);
			if (recycle.Count > 0) {
				int index = recycle[0];
			 	newFood = new Food(index, speed, position);
				foods[index] = newFood;
				recycle.RemoveAt(0);
			} else {
			 	newFood = new Food(foods.Count, speed, position);
				foods.Add(newFood);
			}
			++foodCount;
			foodMap.SetPixels((int)position.x, (int)position.y, newFood.dimension, newFood.dimension, newFood.shape);
		}
		
		// Update
		for (int i = 0; i < foods.Count; ++i) 
		{
			// 
			if (foods[i] != null) 
			{
				// Move
				foods[i].Move(game.worldSpeed);
				Vector3 position = foods[i].position;

				bool collision = plant.IsRootAt((int)position.x, (int)position.y);
				/*int size = foods[i].dimension;
				int count = foods[i].dimension * foods[i].dimension;
				for (int c = 0; c < count; ++c) {
					if (foods[i].shape[c].r > 0f) {
						int x = (int)(position.x + (c % size));
						int y = (int)(position.y + Mathf.Floor(c / size));
						if (plant.IsRootAt(x, y)) {
							collision = true;
							break;
						}
					}
				}*/
				//bool outOfGround = position.y > dimension / 2 || position.y < 0 || position.x < 0 || position.x >= dimension;

				// Clear
				if (collision)// || outOfGround)
				{
					//
					plant.AddResource(foods[i].amount);
					//plant.AddRootWithFood(foods[i]);
					
					foods[i] = null;
					recycle.Add(i);
					--foodCount;
				} 
				// Draw
				else 
				{
					Food food = foods[i];
					int x = (int)Mathf.Clamp(position.x - food.dimension, 0f, dimension-1);
					int y = (int)Mathf.Clamp(position.y - food.dimension, 0f, dimension-1);
					foodMap.SetPixels(x, y, food.dimension, food.dimension, food.shape);
				}
			}
		}
		foodMap.Apply();
	}
}