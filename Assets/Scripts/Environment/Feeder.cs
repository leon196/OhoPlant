using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Feeder : MonoBehaviour 
{
	// Objects
	private List<Food> foods;
	private List<int> recycle;

	// Logic
	private float foodLastTime = 0f;
	private float foodTimeDelay = 1f;
	private int foodCount = 0;
	private int foodMax = 16;
	
	// Texture sent to shader
	private Texture2D foodMap;
	private Color[] clear;
	private Color[] clearHalf;
	private int dimension;

	private Game game;
	private Planter planter;

	void Start ()
	{
		game = GetComponent<Game>();
		planter = GetComponent<Planter>();

		/* Logic Setup */

		foodCount = 0;
		foodLastTime = Time.time;

		/* Texture Setup */

		dimension = Manager.Instance.Game.dimension;
		foodMap = new Texture2D(dimension, dimension, TextureFormat.ARGB32, false);
		foodMap.filterMode = FilterMode.Point;
		foods = new List<Food>();
		recycle = new List<int>();

		renderer.material.SetTexture("_TextureFood", foodMap);
		
		int count = dimension*dimension;
		clear = new Color[count];
		for (int i = 0; i < count; ++i) 
			clear[i] = Color.clear;
		count = dimension*dimension/2;
		clearHalf = new Color[count];
		for (int i = 0; i < count; ++i) 
			clearHalf[i] = Color.clear;

		foodMap.SetPixels(0, 0, dimension, dimension, clear);
	}

	void Update ()
	{

		/* Clear */

		foodMap.SetPixels(0, 0, dimension, dimension, clear);

		/* Spawn */

		if (foodLastTime + foodTimeDelay < Time.time && foodCount < foodMax) 
		{
			foodLastTime = Time.time;

			Vector3 position = Manager.Instance.RandomTopEdgePosition();

			Food newFood;
			if (recycle.Count > 0) {
				int index = recycle[0];
			 	newFood = new Food(index, position);
				foods[index] = newFood;
				recycle.RemoveAt(0);
			} else {
			 	newFood = new Food(foods.Count, position);
				foods.Add(newFood);
			}
			++foodCount;
			Draw(newFood);
		}
		
		/* Update */

		for (int i = 0; i < foods.Count; ++i) 
		{
			// 
			if (foods[i] != null) 
			{
				Vector3 position = foods[i].position;

				bool collision = planter.IsRootAt((int)position.x, (int)position.y);
				bool collisionWithGroundEdges = position.y <= 0 || position.x <= 0 || position.x >= dimension - 1;
				//bool isOutOfVelocity = foods[i].IsOutOfVelocity();

				// Clear
				if (collision)
				{
					//
					planter.AddFood(foods[i]);
					
					foods[i] = null;
					recycle.Add(i);
					--foodCount;
				} 
				// 
				else if (collisionWithGroundEdges)// || isOutOfVelocity)
				{
					//
					Draw(foods[i]);

					//
					foods[i] = null;
					recycle.Add(i);
					--foodCount;
				}
				// Move
				else 
				{
					Draw(foods[i]);

					foods[i].Move(game.worldSpeed);
				}
			}
		}

		//
		foodMap.Apply();
	}

	void Draw (Food food)
	{
		int x = (int)Mathf.Clamp(food.position.x, 0f, dimension-1-food.dimension);
		int y = (int)Mathf.Clamp(food.position.y, 0f, dimension-1-food.dimension);
		foodMap.SetPixels(x, y, food.dimension, food.dimension, food.shape);
	}
}