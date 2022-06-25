using VkParser.Views;

namespace VkParser
{
    class Program
    {
        static Menu menu = new Menu();
        
        static void Main()
        {
            Initialize();
            Processing();
            Deinitialize();
        }

        static void Initialize()
        {
            Directory.CreateDirectory("Posts");
            Directory.SetCurrentDirectory("Posts");
        }

        static void Processing()
        {
            menu.Processing();
        }

        static void Deinitialize()
        {
            var files = Directory.GetFiles(Directory.GetCurrentDirectory());
            foreach (var file in files)
                File.Delete(file);
            Directory.SetCurrentDirectory(Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().Length - 6));
            Directory.Delete("Posts");
        }
    }
}