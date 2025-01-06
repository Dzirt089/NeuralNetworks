namespace NeuralNetworks.Tests
{
	[TestClass()]
	public class NeuralNetworkTests
	{
		[TestMethod()]
		public void FeedForwardTest()
		{
			var dataset = new List<Tuple<double, double[]>>
			{
				//Результат - Пациент болен - 1
				//			  Пациент здоров - 0
				//Температура ниже или выше нормы человека (ср.: 36,6) (норма - 0, иначе - 1) // - T
				//Молодой возраст	- 1, иначе - 0	// - A
				//Курит	- 1, нет - 0 // - S
				//Правильно питается - 1, нет - 0 // F
				//								  T,A,S,F
				new(0, [0,0,0,0]),
				new(0, [0,0,0,1]),
				new(1, [0,0,1,0]),
				new(0, [0,0,1,1]),
				new(0, [0,1,0,0]),
				new(0, [0,1,0,1]),
				new(1, [0,1,1,0]),
				new(0, [0,1,1,1]),
				new(1, [1,0,0,0]),
				new(1, [1,0,0,1]),
				new(1, [1,0,1,0]),
				new(1, [1,0,1,1]),
				new(1, [1,1,0,0]),
				new(0, [1,1,0,1]),
				new(1, [1,1,1,0]),
				new(1, [1,1,1,1])
			};


			var topology = new Topology(4, 1, 0.1, 2);
			var neuralNetwork = new NeuralNetwork(topology);
			var difference = neuralNetwork.Learn(dataset, 100000);

			var results = new List<double>();
			foreach(var data in dataset)
			{
				var res = neuralNetwork.FeedForward(data.Item2).Output;
				results.Add(res);
			}

			for(int i = 0; i < results.Count; i++)
			{
				var expected = Math.Round(dataset[i].Item1, 3);
				var actual = Math.Round(results[i], 3);

				Assert.AreEqual(expected, actual);
			}
		}
	}
}