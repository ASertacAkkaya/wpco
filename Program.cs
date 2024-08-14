using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;

namespace wpco
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string[] extensions = { "*.webp" };
            string[] matchedFiles = extensions.SelectMany(ext => Directory.GetFiles(currentDirectory, ext)).ToArray();

            if (matchedFiles.Length == 0)
            {
                Console.WriteLine("There are no .webp files in the current directory.");
                return;
            }

            foreach (string file in matchedFiles)
            {
                string outputPathWithoutExt = Path.Combine(currentDirectory, Path.GetFileNameWithoutExtension(file));

                try
                {
                    using (Image<Rgba32> image = Image.Load<Rgba32>(file))
                    {
                        Console.WriteLine($"Processing {file}...");
                        int frameCount = image.Frames.Count;

                        if (frameCount > 1)
                        {
                            Console.WriteLine("Detected an animated WebP, converting to GIF...");
                            image.SaveAsGif(outputPathWithoutExt + ".gif");
                            Console.WriteLine($"Saved to {outputPathWithoutExt}.gif");
                        }
                        else
                        {
                            image.SaveAsPng(outputPathWithoutExt + ".png");
                            Console.WriteLine($"Saved to {outputPathWithoutExt}.png");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occured: {ex.Message}");
                }
            }
            Console.WriteLine("Process completed.");
        }
    }
}
