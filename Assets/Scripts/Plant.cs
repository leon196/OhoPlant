using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Plant : MonoBehaviour 
{
	List<Root> _rootList;
	public List<Root> Roots { get { return _rootList; } }

	List<Flower> _flowerList;

	float _plantHeight;
	public float Height { get { return _plantHeight; } }

	void Start () 
	{
		this._rootList = new List<Root>();

		Root root = new Root();
		root.Create(new Vector3(), Master.Instance.RandomAngle());
		this._rootList.Add(root);

		this._flowerList = new List<Flower>();

		this._plantHeight = 0f;

		Master.Instance.GameOver = false;
	}

	public void SpawnRootAt (Vector3 position)
	{
		Root root;

		for (int r = this._rootList.Count - 1; r >= 0; --r)
		{
			root = this._rootList[r];
			root.Randomize();
		}

		root = new Root();
		root.Create(position, Master.Instance.RandomAngle());
		this._rootList.Add(root);

		GameObject flowerObject = GameObject.Instantiate(Master.Instance.PrefabFlower) as GameObject;
		Flower flower = flowerObject.GetComponent<Flower>();
		flower.Create(position);
		this._flowerList.Add(flower);
	}
	
	void Update () 
	{
		Master.Instance.Update();

		if (Master.Instance.GameOver == false)
		{
			// Update Roots
			int rootAliveCount = 0;
			for (int i = this._rootList.Count - 1; i >= 0; --i)
			{
				// Check Root
				Root root = this._rootList[i];
				if (root.IsAlive)
				{
					// Update
					++rootAliveCount;
					root.Grow(Time.deltaTime * 0.75f);
					float h = Input.GetAxis("Horizontal");

					if (Arduino.Instance.Enable)
					{
						if (Arduino.Instance.Button(1))
						{
							h += 1f;
						}
						else if (Arduino.Instance.Button(3))
						{
							h -= 1f;
						}
/*
						h -= Arduino.Instance.Spiner(1);
						h += Arduino.Instance.Spiner(2);
						h -= Arduino.Instance.Spiner(3);
						*/
					}

					root.Rotate(h * Time.deltaTime * Master.Instance.InputScale);

					// Out of screen
					Vector3 screenPosition = Master.Instance.MainCamera.WorldToViewportPoint(root.Position);
					if (screenPosition.x < 0f || screenPosition.y < 0f || screenPosition.x > 1f || screenPosition.y > 1f)
					{
						root.IsAlive = false;
					}

					// GUI
					this._plantHeight = Mathf.Max(root.Position.y, this._plantHeight);
					Shader.SetGlobalFloat("_PlantHeight", this._plantHeight);
				}
			}

			// Check Game Over
			if (rootAliveCount == 0)
			{
				Master.Instance.GameOver = true;
			}
		}
	}

	public void Restarting (float ratio)
	{
		for (int i = this._rootList.Count - 1; i >= 0; --i)
		{
			Root root = this._rootList[i];
			root.Restarting(ratio);
		}
		for (int i = this._flowerList.Count - 1; i >= 0; --i)
		{
			Flower flower = this._flowerList[i];
			flower.Restarting(ratio);
		}
	}

	public void Restart ()
	{
		Root root;
		for (int i = this._rootList.Count - 1; i >= 0; --i)
		{
			root = this._rootList[i];
			root.Clean();
		}

		Flower flower;
		for (int i = this._flowerList.Count - 1; i >= 0; --i)
		{
			flower = this._flowerList[i];
			flower.Clean();
		}

		this._rootList.Clear();
		this._flowerList.Clear();

		root = new Root();
		root.Create(new Vector3(), 0f);
		this._rootList.Add(root);

		this._plantHeight = 0f;
	}
}
