namespace NeuralNetworks.Tests
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


			var topology = new Topology(4, 1, 0.1, 15);
			var neuralNetwork = new NeuralNetwork(topology);
			var difference = neuralNetwork.Learn(outputs, inputs, 100000);

			var results = new List<double>();
			for (int i = 0; i < outputs.Length; i++)
			{
				var row = NeuralNetwork.GetRow(inputs, i);
				var res = neuralNetwork.Predict(row).Output;
				results.Add(res);
			}

			for (int i = 0; i < results.Count; i++)
			{
				var expected = Math.Round(outputs[i], 3);
				var actual = Math.Round(results[i], 3);

				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod()]
		public void DataSetTest()
		{
			var outputs = new List<double>();
			var inputs = new List<double[]>();
			using var sr = new StreamReader("heart.csv");
			var header = sr.ReadLine();

			while (!sr.EndOfStream)
			{
				var row = sr.ReadLine();
				var values = row.Split(',').Select(x => Convert.ToDouble(x.Replace(".", ","))).ToList();

				var output = values.Last();
				var input = values.Take(values.Count - 1).ToArray();

				outputs.Add(output);
				inputs.Add(input);
			}

			var inputSignals = new double[inputs.Count, inputs[0].Length];
			for (int i = 0; i < inputSignals.GetLength(0); i++)
			{
				for (int j = 0; j < inputSignals.GetLength(1); j++)
				{
					inputSignals[i, j] = inputs[i][j];
				}
			}

			var topology = new Topology(outputs.Count, 1, 0.1, outputs.Count / 2);
			var neuralNetwork = new NeuralNetwork(topology);
			var difference = neuralNetwork.Learn(outputs.ToArray(),
				inputSignals, 1000);


			var results = new List<double>();
			for (int i = 0; i < outputs.Count; i++)
			{
				var res = neuralNetwork.Predict(inputs[i]).Output;
				results.Add(res);
			}

			for (int i = 0; i < results.Count; i++)
			{
				var expected = Math.Round(outputs[i], 2);
				var actual = Math.Round(results[i], 2);

				Assert.AreEqual(expected, actual);
			}
		}

		[TestMethod()]
		public void RecognizeImages()
		{
			var parasitizedPath = @"C:\Users\suoza\Desktop\cell_images\Parasitized\";
			var uninfectedPath = @"C:\Users\suoza\Desktop\cell_images\Uninfected\";

			var converte = new PictureConvertor();
			var testParasitizedImageInput = converte.Convert(@"C:\Users\suoza\source\repos\Dzirt089\NeuralNetworks\NeuralNetworksTests\Images\Parasitized.png");

			var testUninfectedImageInput = converte.Convert(@"C:\Users\suoza\source\repos\Dzirt089\NeuralNetworks\NeuralNetworksTests\Images\Uninfected.png");

			var topology = new Topology(testParasitizedImageInput.Length, 1, 0.1, testParasitizedImageInput.Length / 2);
			var neuralNetwork = new NeuralNetwork(topology);
			int size;
			double[,] parasitizedInputs;
			GetData(parasitizedPath, converte, testParasitizedImageInput, out size, out parasitizedInputs);
			neuralNetwork.Learn([1], parasitizedInputs, 10);

			
			double[,] uninfectedInputs;
			GetData(uninfectedPath, converte, testUninfectedImageInput, out size, out uninfectedInputs);
			neuralNetwork.Learn([1], uninfectedInputs, 10);

			var par = neuralNetwork.Predict(testParasitizedImageInput.Select(x => (double)x).ToArray());
			var unpar = neuralNetwork.Predict(testUninfectedImageInput.Select(x => (double)x).ToArray());

			Assert.AreEqual(1, Math.Round(par.Output, 2));
			Assert.AreEqual(0, Math.Round(unpar.Output, 2));
		}

		private static void GetData(string parasitizedPath, PictureConvertor converte, double[] testParasitizedImageInput, out int size, out double[,] result)
		{
			var images = Directory.GetFiles(parasitizedPath);
			size = 10000;
			result = new double[size, testParasitizedImageInput.Length];
			for (int i = 0; i < size; i++)
			{
				var image = converte.Convert(images[i]);
				for (int j = 0; j < image.Length; j++)
				{
					result[i,j] = image[j];
				}
			}
		}
	}
}