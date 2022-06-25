using VkParser.Controllers;
using VkParser.Models;

namespace VkParser.Views
{
    public class Menu
    {
        static readonly string[] options =
        {
            "1. Get next 10 VK posts",
            "2. Show 10 saved VK posts",
            "3. Quit the program"
        };
        ChromeWorker worker;

        public Menu()
        {
            this.worker = new ChromeWorker();
        }

        public void Processing()
        {
            while (true)
            {
                ShowMenuOptions();
                int option = GetMenuOption();
                if (option == 3) return;
                switch (option)
                {
                    case 1:
                        ParseVkPosts();
                        break;
                    case 2:
                        GetVkPosts();
                        break;
                }
                PauseAndClear();
            }
        }

        private static void ShowMenuOptions()
        {
            foreach (string option in options)
                Console.WriteLine(option);
        }

        private int GetMenuOption()
        {
            int option = -1;
            while (true)
            {
                try
                {
                    Console.Write("Enter the menu option: ");
                    option = int.Parse(Console.ReadLine());
                    if (option < 1 || option > 3) throw new ArgumentException();
                }
                catch (FormatException)
                {
                    Console.WriteLine("Entered symbols are not digits, please try again");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("The entered number is not connected with options, please try again");
                }
                break;
            }
            return option;
        }

        public void ParseVkPosts()
        {
            worker.ParseVkPosts();
        }

        public void GetVkPosts()
        {
            List<VkPost> vkPosts = worker.GetVkPosts();
            foreach (VkPost post in vkPosts)
                Console.WriteLine(post.ToString());
        }

        private static void PauseAndClear()
        {
            Console.WriteLine("Press any button to continue");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
