using System.IO;

namespace AngleSharpWrappers.Generator
{
    public class Program
    {
        static void Main()
        {
            while (!Directory.GetCurrentDirectory().EndsWith("AngleSharpWrappers.Generator", System.StringComparison.Ordinal))            
            {
                Directory.SetCurrentDirectory("..");
            }
            Directory.SetCurrentDirectory(@"..\AngleSharpWrappers\Generated");

            var _ = new Generator();
        }
    }
}
