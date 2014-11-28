using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Neuron
{
	//the number of inputs into the neuron
	public int inputCount;

	//the weights for each input
	public List<double> inputWeights; // <double>

	public Neuron (int inputCount_) {
		inputWeights = new List<double>();
		inputCount = inputCount_ + 1;
		//we need an additional weight for the bias hence the +1
		for (int i=0; i < inputCount + 1; ++i) {
			//set up the weights with an initial random value
			inputWeights.Add(Random.Range(-1.0f, 1.0f));
		}
	}	
}
