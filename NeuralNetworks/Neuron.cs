namespace NeuralNetworks
{
	public class Neuron
	{
		/// <summary>
		/// Вес нейрона
		/// </summary>
		public List<double> Weights { get; }

		/// <summary>
		/// Храним список всех входных сигналов
		/// </summary>
		public List<double> Inputs { get; }
		/// <summary>
		/// Тип нейрона
		/// </summary>
		public NeuronType NeuronType { get; }

		/// <summary>
		/// Сохраняем состояние
		/// </summary>
		public double Output { get; private set; }

		/// <summary>
		/// Сохраняем значение делты для нейрона
		/// </summary>
		public double Delta { get; private set; }

		/// <summary>
		/// Конструктор нейрона
		/// </summary>
		/// <param name="inputNeuronCount">Кол-во сигналов\связей, которые поступает в нейрон</param>
		/// <param name="type">Тип нейрона</param>
		public Neuron(int inputNeuronCount, NeuronType type = NeuronType.Normal)
		{
			NeuronType = type;
			Weights = new List<double>();
			Inputs = new List<double>();
			InitWeightsRandomValue(inputNeuronCount);
		}

		private void InitWeightsRandomValue(int inputNeuronCount)
		{
			var rnd = new Random();
			for (int i = 0; i < inputNeuronCount; i++)
			{
				if (NeuronType == NeuronType.Input)
					Weights.Add(1);
				else
					Weights.Add(rnd.NextDouble());

				Inputs.Add(0);
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

			for (int i = 0; i < Inputs.Count; i++)
			{
				Inputs[i] = inputs[i];
			}

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

		private double SigmoidDx(double x)
		{
			var sigmoid = Sigmoid(x);
			var result = sigmoid / (1 - sigmoid);
			return result;
		}


		/// <summary>
		/// Метод для вычесления\балансирования\обучения нейронных весов
		/// </summary>
		/// <param name="error">Разница между ожидаемым результатом и фактически полученным результатом выходного нейрона</param>
		/// <param name="learningRate">Значение для обучения. Чем больше значение, то тем дольше и точнее обучение</param>
		public void Learn(double error, double learningRate)
		{
			if (NeuronType == NeuronType.Input) return;

			Delta = error * SigmoidDx(Output);

			for (int i = 0; i < Weights.Count; i++)
			{
				var weight = Weights[i];
				var input = Inputs[i];

				var newWeight = weight - input * Delta * learningRate;
				Weights[i] = newWeight;
			}
		}

		public override string ToString()
		{
			return Output.ToString();
		}
	}
}
