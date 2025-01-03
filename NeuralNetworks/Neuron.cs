namespace NeuralNetworks
{
	public class Neuron
	{
		/// <summary>
		/// Вес нейрона
		/// </summary>
		public List<double> Weight { get; }
		/// <summary>
		/// Тип нейрона
		/// </summary>
		public NeuronType Type { get; }

		/// <summary>
		/// Сохраняем состояние
		/// </summary>
		public double Output { get; private set; }
	}
}
