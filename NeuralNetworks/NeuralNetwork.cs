
namespace NeuralNetworks
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


		public double FeedForward(List<double> inputSignals)
		{
			if (inputSignals.Count != Topology.InputCount)
				throw new Exception("Кол-во входных сигналов не соответсвует кол-ву входных нейронов для нашей сети");
			SendSignalsToInputNeurons(inputSignals);
		}

		private void SendSignalsToInputNeurons(List<double> inputSignals)
		{
			for (int i = 0; i < inputSignals.Count; i++)
			{
				var signal = new List<double>() { inputSignals[i] };
				var neuron = Layers[0].Neurons[i];

				neuron.FeedForward(signal);
			}
		}
	}
}
