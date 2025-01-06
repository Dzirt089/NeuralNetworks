namespace NeuralNetworks
{
	/// <summary>
	/// Слой представляет собой набор нейронов
	/// </summary>
	public class Layer
	{
		public List<Neuron> Neurons { get; }
		public NeuronType NeuronType { get; }
		public int NeuroCount => Neurons?.Count ?? 0;

		public Layer(List<Neuron> neurons, NeuronType type = NeuronType.Normal, NeuronType neuronType = default)
		{
			if (Validations(neurons, type))
				throw new Exception("Нейроны не соотвутствуют переданному типу");

			Neurons = neurons;
			NeuronType = neuronType;
		}

		/// <summary>
		/// Проверка всех нейронов, на соответсвие к типу.
		/// </summary>
		/// <param name="neurons">Список нейронов</param>
		/// <param name="type">тип этих нейронов</param>
		/// <returns></returns>
		private static bool Validations(List<Neuron> neurons, NeuronType type)
		{
			var cheking = neurons.Any(x => x.NeuronType != type);
			return cheking;
		}

		public List<double> GetSignals()
		{
			var result = new List<double>();
			foreach (var neuron in Neurons)
			{
				result.Add(neuron.Output);
			}
			return result;
		}


		public override string ToString()
		{
			return NeuronType.ToString();
		}
	}
}
