﻿namespace NeuralNetworks
{
	/// <summary>
	/// Нейронная сеть представляет из себя коллекцию слоёв
	/// </summary>
	public class NeuralNetwork
	{
		public Topology Topology { get; }
		public List<Layer> Layers { get; }

		public NeuralNetwork(Topology topology)
		{
			Topology = topology;
			Layers = [];

			CreateInputLayer();
			CreateHiddenLayers();
			CreateOutputLayer();
		}

		private void CreateOutputLayer()
		{
			var outputNeurons = new List<Neuron>();
			var lastLayer = Layers.Last();
			for (int i = 0; i < Topology.OutputCount; i++)
			{

				var neuron = new Neuron(lastLayer.NeuroCount, NeuronType.Output);
				outputNeurons.Add(neuron);
			}

			var outputLayer = new Layer(outputNeurons, NeuronType.Output);
			Layers.Add(outputLayer);
		}

		private void CreateHiddenLayers()
		{
			for (int j = 0; j < Topology.HiddenLayers.Count; j++)
			{
				var hiddenNeurons = new List<Neuron>();
				var lastLayer = Layers.Last();
				for (int i = 0; i < Topology.HiddenLayers[j]; i++)
				{

					var neuron = new Neuron(lastLayer.NeuroCount);
					hiddenNeurons.Add(neuron);
				}

				var hiddenLayer = new Layer(hiddenNeurons);
				Layers.Add(hiddenLayer);
			}
		}

		private void CreateInputLayer()
		{
			var inputNeurons = new List<Neuron>();
			for (int i = 0; i < Topology.InputCount; i++)
			{
				//У входных (input) нейронов только один вход, поэтому он равен 1
				var neuron = new Neuron(1, NeuronType.Input);
				inputNeurons.Add(neuron);
			}

			var inputLayer = new Layer(inputNeurons, NeuronType.Input);
			Layers.Add(inputLayer);
		}


		public Neuron Predict(params double[] inputSignals)
		{
			if (inputSignals.Length != Topology.InputCount)
				throw new Exception("Кол-во входных сигналов не соответсвует кол-ву входных нейронов для нашей сети");

			//double[,] inputSignals2D = new double[1, inputSignals.Length];
			//for (int i = 0; i < inputSignals.Length; i++)
			//{
			//	inputSignals2D[0, i] = inputSignals[i];
			//}

			//var normalizedSignals2D = Scalling(inputSignals2D);
			//double[] normalizedSignals = new double[normalizedSignals2D.GetLength(1)];
			//for (int i = 0; i < normalizedSignals2D.GetLength(1); i++)
			//{
			//	normalizedSignals[i] = normalizedSignals2D[0, i];
			//}

			SendSignalsToInputNeurons(inputSignals);
			FeedForwardAllLayersAfterInput();

			if (Topology.OutputCount == 1)
			{
				return Layers.Last().Neurons[0];
			}
			else
			{
				return Layers.Last().Neurons.OrderByDescending(n => n.Output).First();
			}
		}

		/// <summary>
		/// Метод обучения по набору данных
		/// </summary>
		/// <param name="expected">это набор всех ожидаемых результатов. Ввиде одномерного массива </param>
		/// <param name="inputs">это набор данных (входных сигналов). Ввиде двумерного массива</param>
		/// <param name="epoch">кол-во эпох. Одна эпоха - это одно прохождение DataSet - а</param>
		/// <returns>среднее значение ошибки</returns>
		public double Learn(double[] expected, double[,] inputs, int epoch)
		{
			//var normalizedInputs = Scalling(inputs);

			var error = 0.0;
			for (int i = 0; i < epoch; i++)
			{
				for (int j = 0; j < expected.Length; j++)
				{
					var output = expected[j];
					var input = GetRow(inputs, j);

					error += Backpropagation(output, input);
				}
			}

			var result = error / epoch;
			return result;
		}

		public static double[] GetRow(double[,] matrix, int row)
		{
			var columns = matrix.GetLength(1);
			var array = new double[columns];
			for (int i = 0; i < columns; i++)
				array[i] = matrix[row, i];
			return array;
		}

		/// <summary>
		/// Алгоритм масштабирования
		/// </summary>
		/// <param name="inputs">Все данные из DataSet-a в виде двумерного массива</param>
		/// <returns>двумерный массив с масштабированными данными</returns>
		public double[,] Scalling(double[,] inputs)
		{
			var result = new double[inputs.GetLength(0), inputs.GetLength(1)];

			//Первый внешний цикл идет по колонкам DataSet-a
			for (int column = 0; column < inputs.GetLength(1); column++)
			{
				//берем первый элемент для начала сравнения
				var min = inputs[0, column];
				var max = inputs[0, column];
				//Второй внутренний цикл, идёт по строкам вниз, находя минимум и мкскимум для масштабирования
				for (int row = 1; row < inputs.GetLength(0); row++)
				{
					var item = inputs[row, column];
					if (item < min) min = item;
					if (item > max) max = item;
				}

				var divider = (max - min);
				//Проходя ещё раз этот цикл, мы устанавливаем новые данные для сигнала
				for (int row = 1; row < inputs.GetLength(0); row++)
				{
					result[row, column] = (inputs[row, column] - min) / divider;
				}
			}

			return result;
		}

		/// <summary>
		/// Алгоритм нормализации данных DataSet-a для нейронки
		/// </summary>
		/// <param name="inputs">Все данные из DataSet-a в виде двумерного массива</param>
		/// <returns>двумерный массив с нормализированными данными</returns>
		public double[,] Normalization(double[,] inputs)
		{
			int numRows = inputs.GetLength(0);
			int numCols = inputs.GetLength(1);
			var result = new double[numRows, numCols];

			//Первый внешний цикл идет по колонкам DataSet-a
			for (int column = 0; column < numCols; column++)
			{
				//Вычисляем среднее значение сигнала нейрона
				double sum = 0;
				for (int row = 0; row < numRows; row++)
				{
					sum += inputs[row, column];
				}
				var average = numRows > 0 ? sum / numRows : 0;

				//Вычисляем стандартное квадратичное отклонение нейрона 
				double error = 0;
				for (int row = 0; row < numRows; row++)
				{
					// тут находим сумму значения в двумерном массиве, минус среднее значение сигнала нейрона, возведя выражение в квадрат
					error += Math.Pow((inputs[row, column] - average), 2);
				}
				//сумму делим на кол-во данных в колонке, извлекаем корень из выражения.
				var standartError = numRows > 0 ? Math.Sqrt(error / numRows) : 1;


				//Новое значение сигнала нейрона
				for (int row = 0; row < numRows; row++)
				{
					result[row, column] = standartError != 0 ? (inputs[row, column] - average) / standartError : 0;
				}
			}
			return result;
		}

		/// <summary>
		/// метод по обратному распространению ошибки (справа налево)
		/// </summary>
		/// <param name="exprected">Результат, который мы ожидаем, то что должно получиться</param>
		/// <param name="inputs">Набор входных параметров\входные сигналы</param>
		/// <returns></returns>
		private double Backpropagation(double exprected,
			params double[] inputs)
		{
			var actual = Predict(inputs).Output;

			var difference = actual - exprected;
			foreach (var neuron in Layers.Last().Neurons)
			{
				neuron.Learn(difference, Topology.LearningRate);
			}

			for (int j = Layers.Count - 2; j >= 0; j--)
			{
				var layer = Layers[j];

				var previousLayer = Layers[j + 1];

				for (int i = 0; i < layer.NeuroCount; i++)
				{
					var neuron = layer.Neurons[i];

					for (int k = 0; k < previousLayer.NeuroCount; k++)
					{
						var previousNeuron = previousLayer.Neurons[k];

						var error = previousNeuron.Weights[i] * previousNeuron.Delta;

						neuron.Learn(error, Topology.LearningRate);
					}
				}
			}

			var result = difference * difference;
			return result;
		}

		private void FeedForwardAllLayersAfterInput()
		{
			for (int i = 1; i < Layers.Count; i++)
			{
				var layer = Layers[i];
				var previousLayerSignals = Layers[i - 1].GetSignals();

				foreach (var neuron in layer.Neurons)
				{
					neuron.FeedForward(previousLayerSignals);
				}
			}
		}

		private void SendSignalsToInputNeurons(params double[] inputSignals)
		{
			for (int i = 0; i < inputSignals.Length; i++)
			{
				var signal = new List<double>() { inputSignals[i] };
				//берем нулевой слой
				var neuron = Layers[0].Neurons[i];
				//Посылаем сигнал на нейройн (первый, входной нейрон всегда один
				neuron.FeedForward(signal);
			}
		}
	}
}
