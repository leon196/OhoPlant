using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class NeuralNet {

	private int inputCount = 3;
	private int outputCount = 2;
	private int layerHiddenCount = 1;
	private int neuronCountPerLayer = 6;
	private List<NeuronLayer> layers;
	private double bias = -1.0;
	private double activationResponse = 1.0;

	public void CreateNetwork(int inputCount_, int outputCount_, int layerHiddenCount_, int neuronCountPerLayer_) {
		inputCount = inputCount_;
		outputCount = outputCount_;
		layerHiddenCount = layerHiddenCount_;
		neuronCountPerLayer = neuronCountPerLayer_;
		layers = new List<NeuronLayer>();
		//create the layers of the network
		if (layerHiddenCount > 0) {
			//create first hidden layer
			layers.Add(new NeuronLayer(neuronCountPerLayer, inputCount));

			for (int i=0; i<layerHiddenCount-1; ++i) {
				layers.Add(new NeuronLayer(neuronCountPerLayer, neuronCountPerLayer));
			}

			//create output layer
			layers.Add(new NeuronLayer(outputCount, neuronCountPerLayer));
		}

		else {
			//create output layer
			layers.Add(new NeuronLayer(outputCount, inputCount));
		}
	}

	public List<double> GetWeights() {
		//this will hold the weights
		List<double> weights = new List<double>();
		
		//for each layer
		for (int i = 0; i < layerHiddenCount + 1; ++i) {
			//for each neuron
			for (int j = 0; j < layers[i].neuronCount; ++j) {
				//for each weight
				for (int k = 0; k < layers[i].neurons[j].inputCount; ++k) {
					weights.Add(layers[i].neurons[j].inputWeights[k]);
				}
			}
		}
		return weights;
	}

	public int GetNumberOfWeights() {
		int weights = 0;
		//for each layer
		for (int i = 0; i < layerHiddenCount + 1; ++i) {
			//for each neuron
			for (int j = 0; j < layers[i].neuronCount; ++j) {
				//for each weight
				for (int k = 0; k < layers[i].neurons[j].inputCount; ++k) {
					weights++;
				}
			}
		}
		return weights;
	}

	public void PutWeights(List<double> weights) {
		int cWeight = 0;
		//for each layer
		for (int i = 0; i < layerHiddenCount + 1; ++i) {
			//for each neuron
			for (int j = 0; j < layers[i].neuronCount; ++j) {
				//for each weight
				for (int k = 0; k < layers[i].neurons[j].inputCount; ++k) {
					layers[i].neurons[j].inputWeights[k] = weights[cWeight++];
				}
			}
		}
	}

	public List<double> Update(List<double> inputs) {
		List<double> outputs = new List<double>();
		int weight = 0;
		if (inputs.Count != inputCount) {
			return outputs;
		}
		for (int i = 0; i < layerHiddenCount + 1; ++i) {
			if (i > 0) {
				inputs = new List<double>(outputs);
			}
			outputs.Clear();
			weight = 0;

			int neuronCount = layers[i].neuronCount;

			for (int j = 0; j < neuronCount; ++j) {
				double netInput = 0;
				int numInputs = layers[i].neurons[j].inputCount;
				for (int k = 0; k < numInputs-1; ++k) {
					netInput += layers[i].neurons[j].inputWeights[k] * inputs[weight++];
				}
				netInput += layers[i].neurons[j].inputWeights[numInputs-1] * bias;
				outputs.Add(Sigmoid(netInput, activationResponse));
				weight = 0;
			}
		}
		return outputs;
	}

	public double Sigmoid(double activation, double response) {
		return ( 1.0 / ( 1.0 + Mathf.Exp((float) (-activation / response))));
	}

}
