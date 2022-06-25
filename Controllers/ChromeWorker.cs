using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using VkParser.Models;

namespace VkParser.Controllers
{
    public class ChromeWorker
    {
        ChromeOptions chromeOptions;
        ChromeDriver chromeDriver;
        FileWorker fileWorker;

        public ChromeWorker()
        {
            chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument(@"user-data-dir=C:\Users\badjo\AppData\Local\Google\Chrome\User Data");
            fileWorker = new FileWorker();
        }

        public void ParseVkPosts()
        {
            chromeDriver = new ChromeDriver(chromeOptions);
            GoToVkUrl();
            List<VkPost> posts = SelectVkPosts();
            CloseAndQuitDriver();
            fileWorker.SavePosts(posts);
            chromeDriver = null;
        }

        private void GoToVkUrl()
        {
            try
            {
                chromeDriver.Navigate().GoToUrl(@"https://vk.com/feed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private List<VkPost> SelectVkPosts()
        {
            List<IWebElement> webElements = chromeDriver.FindElements(By.ClassName("feed_row")).ToList();
            List<VkPost> posts = new List<VkPost>();

            foreach (var element in webElements)
            {
                try
                {
                    if (!element.Displayed) continue;
                    VkPost post = new VkPost(element);
                    if (!string.IsNullOrEmpty(post.Id))
                    {
                        posts.Add(post);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return posts;
        }

        private void CloseAndQuitDriver()
        {
            chromeDriver.Close();
            chromeDriver.Quit();
        }

        public List<VkPost> GetVkPosts()
        {
            return fileWorker.ReadPosts();
        }
    }
}
