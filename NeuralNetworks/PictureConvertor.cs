using System.Drawing;

namespace NeuralNetworks
{
	public class PictureConvertor
	{
		public int Boundary { get; set; } = 128;
		public int Height { get; set; }
		public int Width { get; set; }

		public double[] Convert(string path)
		{
			if (string.IsNullOrEmpty(path)) return [];

			Bitmap image = new Bitmap(path);
			var resizeImage = new Bitmap(image, new Size(100, 100));
			var size = resizeImage.Width * resizeImage.Height;
			
			Height = resizeImage.Height;
			Width = resizeImage.Width;

			var result = new List<double>(size);

			for (int y = 0; y < resizeImage.Height; y++)
			{
				for (int x = 0; x < resizeImage.Width; x++)
				{
					var pixel = resizeImage.GetPixel(x, y);
					var value = Brightness(pixel);
					result.Add(value);
				}
			}			

			return result.ToArray();
		}

		private int Brightness(Color pixel)
		{
			var result = 0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B;
			return result < Boundary ? 0 : 1;
		}

		public void Save(string path, double[] pixels)
		{
			var image = new Bitmap(Width, Height);
			for (int y = 0; y < image.Height; y++)
			{
				for (int x = 0; x < image.Width; x++)
				{
					var color = pixels[y * Width + x] == 1 ? Color.White : Color.Black;
					image.SetPixel(x, y, color);
				}
			}
			image.Save(path);
		}
	}
}
