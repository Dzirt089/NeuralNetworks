namespace NeuralNetworks
{
	/// <summary>
	/// Описание набора свойств, которая определяет нейронную сеть
	/// </summary>
	public class Topology
	{
		/// <summary>
		/// Кол-во входов (входных слоёв) в саму нейронную сеть
		/// </summary>
		public int InputCount { get; }

		/// <summary>
		/// Кол-во выходных (слоёв) из нейронной сети
		/// </summary>
		public int OutputCount { get; }

		/// <summary>
		/// Коллекция, которая для каждого слоя будет храниться кол-во нейронов в скрытом слое
		/// </summary>
		public List<int> HiddenLayers { get; }
		public Topology(int inputCount, int outputCount,
			params int[] layers)
		{
			InputCount = inputCount;
			OutputCount = outputCount;
			HiddenLayers = [];
			HiddenLayers.AddRange(layers);
		}
	}
}
