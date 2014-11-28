using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Planter : MonoBehaviour 
{
	// Logic
	private Game game;
	private float energy;
	private float resource;
	private float growthLastTime;
	private float growthDelay;
	private bool gotResource;
	private bool gotEnergy;
	private bool gotCycleGrowth;

	public bool autoRestart = true;
	private bool autoRestarted = false;
	private float restartTimeStart = 0f;
	private float restartTimeDelay = 1f;

	// Pseudo L-System
	private int depth;
	private float branchAngle;
	private float branchLength;

	// Neural Network
	private NeuralNet brain;
	private int inputs = 2; // Energy Leaf + Resource
	private int outputs = 0;
	private int layers = 2;
	private int neurons = 9;
	private int outputPerRoot = 2; // Translation + Rotation

	// For a later algo gen
	private float genParamRootGrowth = 2.0f;
	private float genParamRootRotation = 30.0f;

	// Elements
	private List<Branch> branches;
	private List<int> branchesRecycle;
	private int branchesCount;
	private List<Root> roots;
	private List<int> rootsRecycle;
	private List<GameObject> flowers;

	// Draw Stuff
	private Texture2D textureBranches;
	private Texture2D textureRoots;
	private Color[] clear;
	private int dimension;


	void Start () 
	{

		/* Logic */

		game = GetComponent<Game>();
		dimension = Manager.Instance.Game.dimension;
		energy = 0f;
		resource = 20f;
		growthDelay = 0.05f / game.worldSpeed;
		growthLastTime = Time.time;

		/* Pseudo L-System */

		depth = game.plantDepth + Random.Range(1, 3);
		branchAngle = Mathf.PI / 2f;
		branchLength = (dimension / 2) / (depth + 2);

		/* Neural Network */
		
		outputs += outputPerRoot * 3;

		brain = new NeuralNet();
		brain.CreateNetwork(inputs, outputs, layers, neurons);

		/* Elements */

		roots = new List<Root>();
		rootsRecycle = new List<int>();
		SpawnRoot(Vector3.zero);
		SpawnRoot(Vector3.zero);
		SpawnRoot(Vector3.zero);

		branches = new List<Branch>();
		branchesRecycle = new List<int>();
		branchesCount = 0;
		SpawnBranch(Vector3.zero, Vector3.up, 0);

		flowers = new List<GameObject>();

		/* Draw Stuff */

		textureBranches = new Texture2D(dimension, dimension, TextureFormat.ARGB32, false);
		textureBranches.filterMode = FilterMode.Point;
		textureRoots = new Texture2D(dimension, dimension, TextureFormat.ARGB32, false);
		textureRoots.filterMode = FilterMode.Point;
		clear = new Color[dimension*dimension];
		for (int i = 0; i < dimension*dimension; ++i) clear[i] = Color.clear;
		textureBranches.SetPixels(0, 0, dimension, dimension, clear);
		textureBranches.Apply();
		textureRoots.SetPixels(0, 0, dimension, dimension, clear);
		textureRoots.Apply();
		renderer.material.SetTexture("_TextureBranches", textureBranches);
		renderer.material.SetTexture("_TextureRoots", textureRoots);
	}
	
	void Update () 
	{
		energy = game.GetEnergy();

		gotEnergy = energy > 0f;
		gotResource = resource > 0f;
		gotCycleGrowth = growthLastTime + growthDelay < Time.time;

		if (branchesCount == 0 && autoRestart)
		{
			if (!autoRestarted) {
				autoRestarted = true;
				restartTimeStart = Time.time;
			} else {
				if (restartTimeStart + restartTimeDelay < Time.time) {
					Reset();
					autoRestarted = false;
				}
			}
		}

		if (gotEnergy && gotCycleGrowth)
		{

			/* BRANCHES */

			float branchesEnergy = 0f;

			for (int b = 0; b < branches.Count; ++b) 
			{
				Branch branch = branches[b];

				if (branch != null)
				{
					// Grow
					branch.Grow(energy, Manager.Instance.GetSunAngle());
					branchesEnergy += branch.photosynthesize;

					// Check for change
					if (branch.ChangePosition()) 
					{
						// Stop Growing
						if (branch.Length() >= branchLength)
						{
							if (branch.depth < depth)
							{

								// Split
								Vector3 position = branch.position;
								Vector3 direction = branch.direction;
								int count = 2;
								float angle = branchAngle / (count - 1);
								for (int i = 0; i < count; ++i) 
								{
									float rand = Random.Range(Mathf.PI * -0.5f, Mathf.PI * 0.5f);
									float x = Mathf.Cos(angle * i + branchAngle/2 + rand);
									float y = Mathf.Sin(angle * i + branchAngle/2 + rand);
									Vector3 newDirection = direction + new Vector3(x, y, 0); 
									int newDepth = branch.depth + 1;

									// Branch
									SpawnBranch(position, newDirection.normalized, newDepth);

								}

								// Root
								SpawnRoot(Vector3.zero);
							} 
							else 
							{
								// Grow Flower
								GameObject flower = GameObject.Instantiate(Manager.Instance.Flower, Manager.Instance.GetTransformPosition(branch.position) + Vector3.back * 0.1f, Quaternion.identity) as GameObject;
								flowers.Add(flower);
							}

							//
							branch.DetachLeaf();

							//
							branches[b] = null;
							branchesRecycle.Add(b);
							--branchesCount;
						} 
						// Draw
						else {
							DrawBranch(branch);
							branch.UpdateLastPosition();
						}
					}
				}
			}

			/* ROOTS */

			// Get Inputs
			List<double> listInputs = new List<double>();
			listInputs.Add(branchesEnergy/branchesCount);
			listInputs.Add(energy); 

			// Get Outputs
			List<double> ouputs = brain.Update(listInputs);

			// Indexes
			int rootTranslation = 0;
			int rootRotation = 1;

			for (int r = 0; r < roots.Count; ++r) 
			{
				Root root = roots[r];
				if (root != null)
				{
					// Update Root
					root.Grow(	ouputs[rootTranslation] * genParamRootGrowth,
								ouputs[rootRotation] * genParamRootRotation);

					rootTranslation += outputPerRoot;
					rootRotation += outputPerRoot;

					bool isOutOfVelocity = root.IsOutOfVelocity();


					// Optimized Draw
					if (root.ChangePosition()) 
					{
						//
						bool outOfScreen = root.position.x <= -dimension/2 || root.position.x >= dimension/2 - 1 || root.position.y <= -dimension/2 || root.position.y >= dimension/2 - 1;
						if (outOfScreen) {
							//
							roots[r] = null;
							rootsRecycle.Add(r);
						} 
						// Draw
						else {
							DrawRoot(root);
							root.UpdateLastPosition();
						}
					}
					// Stop Growing
					else if (isOutOfVelocity)
					{
						//
						roots[r] = null;
						rootsRecycle.Add(r);
						
						outputs -= outputPerRoot;
						brain.CreateNetwork(inputs, outputs, layers, neurons);
					}
				}
			}

			textureRoots.Apply();
			textureBranches.Apply();

			growthLastTime = Time.time;
			growthDelay = 0.05f / game.worldSpeed;
			--resource;
		}
	}

	void DrawBranch (Branch branch)
	{
		Vector3 position = branch.position;
		position.x = Mathf.Max(0f, Mathf.Min(dimension, position.x + dimension / 2f));
		position.y = Mathf.Max(0f, Mathf.Min(dimension, position.y + dimension / 2f));
		textureBranches.SetPixel((int)position.x, (int)position.y, Color.green);
	}

	void DrawRoot (Root root)
	{
		int x = (int)Mathf.Max(0f, Mathf.Min(dimension, root.position.x + dimension / 2f));
		int y = (int)Mathf.Max(0f, Mathf.Min(dimension, root.position.y + dimension / 2f));
		textureRoots.SetPixel(x, y, Color.green);
	}

	public void SpawnBranch (Vector3 position_, Vector3 direction_, int depth_)
	{
		Branch newBranch = new Branch(position_, direction_, depth_);
		if (branchesRecycle.Count > 0) 
		{
			int index = branchesRecycle[0];
			branches[index] = newBranch;
			branchesRecycle.RemoveAt(0);
		} 
		else 
		{
			branches.Add(newBranch);
		}

		++branchesCount;
	}

	public void SpawnRoot (Vector3 position_)
	{
		//Debug.Log(position_);
		Root newRoot = new Root(position_);
		if (rootsRecycle.Count > 0) 
		{
			int index = rootsRecycle[0];
			roots[index] = newRoot;
			rootsRecycle.RemoveAt(0);
		} 
		else 
		{
			roots.Add(newRoot);
		}

		outputs += outputPerRoot;
		brain.CreateNetwork(inputs, outputs, layers, neurons);
	}

	public bool IsBranchAt (int x, int y)
	{
		return textureBranches.GetPixel(x, y).g > 0f;
	}

	public bool IsRootAt (int x, int y)
	{
		return textureRoots.GetPixel(x, y).g > 0f;
	}

	public void AddResource ()
	{
		++resource;
	}

	public void AddRootAt (int x, int y)
	{
		textureRoots.SetPixel(x, y, Color.green);
	}

	public void AddRootWithFood (Food food)
	{
		int x = (int)Mathf.Clamp(food.position.x, 0, dimension - 1);
		int y = (int)Mathf.Clamp(food.position.y, 0, dimension - 1);
		textureRoots.SetPixel(x, y, Color.red);
	}

	public void AddFood (Food food)
	{
		SpawnRoot(food.position - new Vector3(dimension/2, dimension/2, 0));
	}

	public void Reset ()
	{
		foreach (Branch branch in branches) 
			if (branch != null)
				branch.DetachLeaf();
		foreach (GameObject flower in flowers) Destroy(flower);

		Leaf.s_zIndex = 0f;

		Start();
	}
}
