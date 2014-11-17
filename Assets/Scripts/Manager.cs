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

	// Sun
	private GameObject _sun = null;
	public GameObject Sun { 
		get {
			if (_sun == null) _sun = GameObject.Find("Sun");
			return _sun;
		}
	}

	// Moon
	private GameObject _moon = null;
	public GameObject Moon { 
		get {
			if (_moon == null) _moon = GameObject.Find("Moon");
			return _moon;
		}
	}

	// Cloud
	private GameObject _cloud = null;
	public GameObject Cloud { 
		get {
			if (_cloud == null) _cloud = GameObject.Find("Cloud");
			return _cloud;
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

}