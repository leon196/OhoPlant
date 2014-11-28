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
			if (_controls == null) _controls = Game.GetComponent<Controls>();
			return _controls;
		}
	}

	// Shaders
	private Shaders _shaders = null;
	public Shaders Shaders {
		get {
			if (_shaders == null) _shaders = Game.GetComponent<Shaders>();
			return _shaders;
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

	public float GetSunAngle () {
		return Controls.GetSunAngle();
	}

	public Vector3 GetSunDirection () {
		return Controls.GetSunDirection();
	}

	public Vector3 GetMoonDirection () {
		return Controls.GetMoonDirection() * -1f;
	}

	// Random Branch Direction
	public static Vector3 GetRandomBranchDirection () { 
		Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(0.5f, 1f), 0f);
		return direction.normalized;
	}

	//
	public Vector3 RandomGroundPosition () {
		return new Vector3(Random.Range(0, Game.dimension), Random.Range(0, Game.dimension), 0f);
	}

	//
	public Vector3 RandomTopEdgePosition () {
		return new Vector3(Random.Range(0, Game.dimension), Game.dimension, 0f);
	}

	// Random Root Direction
	public static Vector3 GetRandomRootDirection () { 
		return (new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, -0.5f), 0f)).normalized;
	}

	// GetRandomLeafDirection
	public Vector3 GetRandomLeafDirection () {
		return new Vector3(0, 0, Random.Range(0f, 1f) * 90f + 45f);
	}

	public Vector3 GetTransformPosition (Vector3 position_) {
		return (position_ / (float)(Game.dimension * 4) * Shaders.levelOfDetails);
	}

	// Material Branch
	private Material _materialBranch = null;
	public Material MaterialBranch {
		get {
			if (_materialBranch == null) _materialBranch = Resources.Load("MaterialBranch") as Material;
			return _materialBranch;
		}
	}
}