using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NeuronLayer {
	public int neuronCount;
	public List<Neuron> neurons; //<Neuron>

	public NeuronLayer (int neuronCount_, int inputCountPerNeuron_)
	{
		neurons = new List<Neuron>();
		neuronCount = neuronCount_;
		for (int i = 0; i < neuronCount_; ++i) {
			neurons.Add(new Neuron(inputCountPerNeuron_));
		}
	}
}
