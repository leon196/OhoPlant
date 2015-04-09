using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Master
{
	// Singleton
	private static Master _instance = null;
	private Master () {}
	public static Master Instance {
		get {
			if (_instance == null) {
				_instance = new Master();
				_instance._foodList = new List<Food>();
			}
			return _instance;
		}
	}

	// Game Parameters
	private float _distanceMinWithFood = 0.05f;
	private float _inputScale = 3f;

	// GUI Parameters
	private float _rootAscensionScale = 0.5f;
	private float _lineRendererStartWidth = 0.3f;
	private float _lineRendererEndWidth = 0.1f;
	private float _lineRendererSegmentLength = 0.2f;

	// View 
	private float _cameraDistanceMinWithPlant = 5f;

	// Logic
	private bool _gameOver = false;
	private bool _isRestarting = false;
	private int _foodCount = 0;
	private List<Food> _foodList;

	// Animation
	private Vector3 _cameraGamePosition;
	private Vector3 _cameraGameRotation;
	private Vector3 _cameraGameOverPosition;
	private float _cameraOrbitRadius = 10f;
	private float _timeStartGameOver = 0f;
	private float _timeDelayGameOverTransition = 5f;
	private float _timeStartRestart = 0f;
	private float _timeDelayRestartTransition = 3f;
	public float AnimationRatio (float start, float delay) { return Mathf.Clamp((Time.time - start) / delay, 0f, 1f); }

	// Logic Getter
	public bool GameOver { 
		get { return this._gameOver; }
		set
		{
			if (this._gameOver != value && value == true)
			{
				this._timeStartGameOver = Time.time;
				//this._cameraOrbitRadius = this.Plant.Height * 0.5f;
				this._cameraGameOverPosition.y = this.Plant.Height + 10f;
			}
			this._gameOver = value;
		} 
	}

	// Main Loop
	public void Update ()
	{
		// Game Overing
		if (this._gameOver)
		{
			float transition = this.AnimationRatio(this._timeStartGameOver, this._timeDelayGameOverTransition);
			float rotationAngle = Time.time * 0.2f;
			this._cameraGameOverPosition.x = Mathf.Cos(rotationAngle) * this._cameraOrbitRadius;
			this._cameraGameOverPosition.z = Mathf.Sin(rotationAngle) * this._cameraOrbitRadius;

			Vector3 position = Vector3.Lerp(this._cameraGamePosition, this._cameraGameOverPosition, transition);
			Vector3 target = Vector3.zero;
			target.y = this.Plant.Height * 0.5f;

			this._cameraHelper.transform.position = this._cameraGameOverPosition;
			this._cameraHelper.transform.LookAt(target);
			Vector3 rotation = Vector3.Lerp(this._cameraGameRotation, this._cameraHelper.transform.rotation.eulerAngles, transition);

			this._mainCamera.transform.position = position;
			this._mainCamera.transform.rotation = Quaternion.Euler(rotation);

			// Restart
			if (Input.GetKeyDown(KeyCode.R) && this._isRestarting == false && transition >= 1f)
			{
				this._isRestarting = true;
				this._timeStartRestart = Time.time;
			}
		}

		// Playing
		else
		{
			// Check Camera
			Vector3 cameraPosition = this.MainCamera.transform.position;
			float y = Mathf.Max(cameraPosition.y, this.Plant.Height + this._cameraDistanceMinWithPlant);
			cameraPosition.y = Mathf.Lerp(cameraPosition.y, y, Time.deltaTime);
			this.MainCamera.transform.position = cameraPosition;

			// Spawn Food
			if (_foodCount == 0)
			{
				++_foodCount;
				Vector3 foodPosition = this.RandomGridPositionRatio(16, 16);
				foodPosition.x = Mathf.Clamp(foodPosition.x, 0.2f, 0.8f);
				foodPosition.y = Mathf.Clamp(foodPosition.y, 0.2f, 0.8f);
				foodPosition = this.FoodCamera.ViewportToWorldPoint(foodPosition);
				Food food = new Food();
				food.Create(foodPosition);
				this._foodList.Add(food);
			}

			// Check each Food
			for (int f = this._foodList.Count - 1; f >= 0; --f) {
				Food food = this._foodList[f];

				// For each Root
				for (int r = this.Plant.Roots.Count - 1; r >= 0; --r) {
					Root root = this.Plant.Roots[r];

					Vector2 rootPosition = this.MainCamera.WorldToViewportPoint(root.Position);
					Vector2 foodPosition = this.FoodCamera.WorldToViewportPoint(food.Position);

					// Distance Test
					float distance = Vector2.Distance(rootPosition, foodPosition);
					if (distance < this._distanceMinWithFood)
					{
						// Clean Food
						this._foodList.Remove(food);
						food.Clean();
						--_foodCount;

						// Spawn Root
						this.Plant.SpawnRootAt(root.Position);

						break;
					}
				}
			}
		}

		// Restarting
		if (this._isRestarting)
		{
			float restartion = this.AnimationRatio(this._timeStartRestart, this._timeDelayRestartTransition);

			Vector3 position = Vector3.Lerp(this._cameraGameOverPosition, this._cameraGamePosition, restartion);

			Vector3 rotation = Vector3.Lerp(this._cameraHelper.transform.rotation.eulerAngles, this._cameraGameRotation, restartion);

			this._mainCamera.transform.position = position;
			this._mainCamera.transform.rotation = Quaternion.Euler(rotation);

			this.Plant.Restarting(restartion);

			if (restartion >= 1f)
			{
				this.Plant.Restart();
				this._gameOver = false;
				this._isRestarting = false;
			}
		}
	}

	// Getters
	public float InputScale { get { return _inputScale; } }

	// GUI Getters
	public float RootAscensionScale { get { return this._rootAscensionScale; } }
	public float LineRendererStartWidth { get { return this._lineRendererStartWidth; } }
	public float LineRendererEndWidth { get { return this._lineRendererEndWidth; } }
	public float LineRendererSegmentLength { get { return this._lineRendererSegmentLength; } }

	// Camera
	private Camera _mainCamera = null;
	private Camera _foodCamera = null;
	private GameObject _cameraHelper = null;
	public Camera MainCamera {
		get {
			if (_mainCamera == null) {
				this._mainCamera = Camera.main;
				this._cameraGamePosition = this._mainCamera.transform.position;
				this._cameraGameRotation = this._mainCamera.transform.rotation.eulerAngles;
				this._cameraGameOverPosition = this._cameraGamePosition;
				this._cameraHelper = new GameObject("Camera Helper");
				this._cameraHelper.transform.position = this._cameraGamePosition;
				//_mainCamera.depthTextureMode = DepthTextureMode.Depth;
			}
			return this._mainCamera;
		}
	}
	public Camera FoodCamera {
		get {
			if (_foodCamera == null) {
				_foodCamera = GameObject.Find("Camera Food").GetComponent<Camera>();
			}
			return _foodCamera;
		}
	}

	// Plant
	private Plant _plant = null;
	public Plant Plant {
		get { 
			if (this._plant == null) {
				this._plant = GameObject.Find("Plant").GetComponent<Plant>();
			}
			return this._plant;
		}
	}

	// Food
	private GameObject _prefabFood = null;
	public GameObject PrefabFood {
		get {
			if (this._prefabFood == null) {
				this._prefabFood = Resources.Load("Food") as GameObject;
			}
			return this._prefabFood;
		}
	}

	// Leaf
	private GameObject _prefabLeaf = null;
	public GameObject PrefabLeaf {
		get {
			if (_prefabLeaf == null) {
				this._prefabLeaf = Resources.Load("Leaf") as GameObject;
			}
			return _prefabLeaf;
		}
	}

	// Root Material
	private Material _materialRoot = null;
	public Material MaterialRoot {
		get {
			if (this._materialRoot == null) {
				this._materialRoot = Resources.Load("Root") as Material;
			}
			return this._materialRoot;
		}
	}

	// Maths
	public float Segment (float value, float segments) { return Mathf.Floor(value * segments) / segments; }

	public Vector3 RandomGridPositionRatio (int gridSizeX, int gridSizeY)
	{
		return new Vector3(
			this.Segment(Random.Range(0f, 1f), gridSizeX), 
			this.Segment(Random.Range(0f, 1f), gridSizeY),
			0f);
	}

	public float RandomAngle ()
	{
		return Random.Range(0f, Mathf.PI * 2f);
	}
}