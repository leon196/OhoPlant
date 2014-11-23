using UnityEngine;
using System.Collections;

public class Manager {
	// Singleton
	private static Manager _instance = null;
	private Manager () {}
	public static Manager Instance {
		get {
			if (_instance == null) _instance = new Manager();
			return _instance;
		}
	}

	// Game
	private Game _game = null;
	public Game Game {
		get {
			if (_game == null) _game = GameObject.Find("Game").GetComponent<Game>();
			return _game;
		}
	}

	// Controls
	private Controls _controls = null;
	public Controls Controls {
		get {
			if (_controls == null) _controls = Instance.Game.GetComponent<Controls>();
			return _controls;
		}
	}

	// Environment
	private GameObject _environment = null;
	public GameObject Environment { 
		get {
			if (_environment == null) _environment = GameObject.Find("Environment");
			return _environment;
		}
	}

	// Water
	private GameObject _water = null;
	public GameObject Water { 
		get {
			if (_water == null) _water = GameObject.Find("Water");
			return _water;
		}
	}

	// Leaf
	private GameObject _leaf = null;
	public GameObject Leaf {
		get {
			if (_leaf == null) _leaf = Resources.Load("Leaf") as GameObject;
			return _leaf;
		}
	}

	// Random Root Direction
	public static Vector3 GetRandomRootDirection () { 
		Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 0f), 0f);
		return direction.normalized;
	}

	// Material Branch
	private Material _materialBranch = null;
	public Material MaterialBranch {
		get {
			if (_materialBranch == null) _materialBranch = Resources.Load("MaterialBranch") as Material;
			return _materialBranch;
		}
	}

	// SetScreenBounds
	public void SetScreenBounds (Vector3 position) 
	{
		float size = Mathf.Max(Mathf.Abs(position.x), Mathf.Abs(position.y));
		
		if (Camera.main.orthographicSize < size) 
		{
			SetSizeBounds(size);
		}
	}

	public void SetSizeBounds (float size)
	{
		Camera.main.orthographicSize = size;	
		Shader.SetGlobalFloat("WorldDetails", size);
		Environment.transform.localScale = new Vector3(size * 4f, size * 4f, size * 4f);
	}

}