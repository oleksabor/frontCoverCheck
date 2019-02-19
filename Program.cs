using Mono.Options;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using TagLib;

namespace mp3Pictures
{
	class Program
	{
		static int Main(string[] args)
		{
			var directory = Environment.CurrentDirectory;
			var mask = "*.mp3";
			var maxPixels = 0;

			var options = new OptionSet {
				{ "d|dir=", "directory to check files, current dir by default", _ => directory = _ },
				{ "m|mask=", "file mask", _ => mask = _ },
				{ "p|maxPixel=", "max image size in pixels", (int _) => maxPixels = _ },
			};

			try
			{
				var extra = options.Parse(args);
			}
			catch (OptionException oe)
			{
				Console.Write($"error: {oe.Message}");
				Console.WriteLine("Try `mp3Pictures --help' for more information.");
				return -1;
			}

			if (!Directory.Exists(directory))
			{
				Console.WriteLine($"no directory was found {directory}");
				return -2;
			}

			var files = Directory.EnumerateFiles(directory, mask, SearchOption.AllDirectories);

			var report = new Action<string, string>((_, m) => Console.WriteLine("{0} {1}", _.Substring(directory.Length - 1), m));

			foreach (var f in files)
			{
				var file = TagLib.File.Create(f);

				if (file.Tag.Pictures?.Length == 0)
					report(f, null);
				else
					if (maxPixels > 0)
				{
					var pict = file.Tag.Pictures.FirstOrDefault(_ => _.Type == TagLib.PictureType.FrontCover);
					using (var ms = new MemoryStream(pict.Data.Data))
					using (var image = new Bitmap(ms))
					{
						if (image.Height > maxPixels || image.Width > maxPixels)
							report(f, "frontCover too large");
					}
				}

			}

			Console.WriteLine("{0} files were processed from {1}", files.Count(), directory);

			return 0;
		}
	}
}
