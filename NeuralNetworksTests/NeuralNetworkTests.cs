﻿namespace NeuralNetworks.Tests
{
	[TestClass()]
	public class NeuralNetworkTests
	{
		[TestMethod()]
		public void FeedForwardTest()
		{
			double[] outputs = [0, 0, 1, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1];
			double[,] inputs = new double[,]
			{
				{0, 0, 0, 0},
				{0, 0, 0, 1},
				{0, 0, 1, 0},
				{0, 0, 1, 1},
				{0, 1, 0, 0},
				{0, 1, 0, 1},
				{0, 1, 1, 0},
				{0, 1, 1, 1},
				{1, 0, 0, 0},
				{1, 0, 0, 1},
				{1, 0, 1, 0},
				{1, 0, 1, 1},
				{1, 1, 0, 0},
				{1, 1, 0, 1},
				{1, 1, 1, 0},
				{1, 1, 1, 1}
			};


			var topology = new Topology(4, 1, 0.1, 2);
			var neuralNetwork = new NeuralNetwork(topology);
			var difference = neuralNetwork.Learn(outputs,inputs, 100000);

			var results = new List<double>();
			
			for(int i = 0; i < outputs.Length; i++)
			{
				var row = NeuralNetwork.GetRow(inputs,i);
				var res = neuralNetwork.FeedForward(row).Output;
				results.Add(res);
			}

			for (int i = 0; i < results.Count; i++)
			{
				var expected = Math.Round(outputs[i], 3);
				var actual = Math.Round(results[i], 3);

				Assert.AreEqual(expected, actual);
			}
		}
	}
}