using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;

namespace wpco
{
    internal class Program
    {
        static string[] parameters = { "-f", "--force-still-image" };
        static bool forceStillImage = false;
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                convertCurrentDirectory();
            } 
            else
            {
                foreach (string arg in args)
                {
                    // Check if the argument is a parameter
                    if (parameters.Contains(arg)) continue;
                    // Check if the argument is a parameter option (not a file)
                    if (!File.Exists(arg)) continue;

                    process(arg);
                }
            }
            Console.WriteLine("Process completed.");
            return;
        }

        private static void process(string inputPath, string outputExtension = "png")
        {
            try {
                using (Image<Rgba32> image = Image.Load<Rgba32>(inputPath))
                {
                    Console.WriteLine($"Processing {inputPath}...");
                    int frameCount = image.Frames.Count;

                    if (frameCount == 1 || forceStillImage)
                    {
                        string outputPath = Path.ChangeExtension(inputPath, outputExtension);
                        image.Save(outputPath);
                        Console.WriteLine($"Saved to {outputPath}.");
                    }
                    else
                    {
                        string outputPath = Path.ChangeExtension(inputPath, "gif");
                        Console.WriteLine("Detected an animated WebP, converting to GIF...");
                        image.SaveAsGif(outputPath);
                        Console.WriteLine($"Saved to {outputPath}.");
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine($"An error occured: {ex.Message}");
            }
        }

        static void convertCurrentDirectory()
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
                process(file);
            }
        }
    }
}
