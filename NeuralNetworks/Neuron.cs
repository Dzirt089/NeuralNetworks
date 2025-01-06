namespace NeuralNetworks
{
	public class Neuron
	{
		/// <summary>
		/// Вес нейрона
		/// </summary>
		public List<double> Weights { get; }

		/// <summary>
		/// Тип нейрона
		/// </summary>
		public NeuronType NeuronType { get; }

		/// <summary>
		/// Сохраняем состояние
		/// </summary>
		public double Output { get; private set; }

		/// <summary>
		/// Конструктор нейрона
		/// </summary>
		/// <param name="inputNeuronCount">Кол-во сигналов\связей, которые поступает в нейрон</param>
		/// <param name="type">Тип нейрона</param>
		public Neuron(int inputNeuronCount, NeuronType type = NeuronType.Normal)
		{
			NeuronType = type;
			Weights = new List<double>();

			for (int i = 0; i < inputNeuronCount; i++)
			{
				Weights.Add(1);
			}
		}
		/// <summary>
		/// Линейное распространение данных слева направо -> получили результат
		/// </summary>
		/// <param name="inputs">Список входных сигналов, приходящий на один нейрон</param>
		/// <returns></returns>
		public double FeedForward(List<double> inputs)
		{
			ValidationWeightAndCountSignals(inputs);

			double sum = 0;
			for (int i = 0; i < inputs.Count; i++)
			{
				sum += inputs[i] * Weights[i];
			}
			if (NeuronType != NeuronType.Input)
				Output = Sigmoid(sum);
			else Output = sum;

			return Output;
		}

		private void ValidationWeightAndCountSignals(List<double> inputs)
		{
			if (inputs.Count != Weights.Count)
				throw new Exception("Кол-во входных сигналов не соответсвует кол-ву входных нейронов с весами");
		}

		/// <summary>
		/// Экспонента в шарпе)))
		/// Сигмоида в машинном обучении (ML) преобразует любое реальное значение в диапазон от 0 до 1
		/// </summary>
		/// <param name="x">Сумма весов всех сигналов</param>
		/// <returns></returns>
		private double Sigmoid(double x)
		{
			//s(x) = 1 \ 1 + e(-x)
			var result = 1.0 / (1.0 + Math.Pow(Math.E, -x));
			return result;
		}

		public void SetWeights(params double[] weights)
		{
			//TODO: delete after test
			ValidationWeightAndCountSignals(weights.ToList());
			for (int i = 0; i < weights.Length; i++)
			{
				Weights[i] = weights[i];
			}
		}


		public override string ToString()
		{
			return Output.ToString();
		}
	}
}
