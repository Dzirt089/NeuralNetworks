using Microsoft.VisualStudio.TestTools.UnitTesting;

using NeuralNetworks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.Tests
{
	[TestClass()]
	public class PictureConvertorTests
	{
		[TestMethod()]
		public void ConvertTest()
		{
			var converter = new PictureConvertor();
			var inputs = converter.Convert(@"C:\Users\suoza\source\repos\Dzirt089\NeuralNetworks\NeuralNetworksTests\Images\Parasitized.png");
			converter.Save("D:\\imageNew.png", inputs);
		}
	}
}