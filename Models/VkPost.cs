using Newtonsoft.Json;
using OpenQA.Selenium;

namespace VkParser.Models
{
    public class VkPost
    {
        public string Id { get; private set; }
        public string Text { get; private set; }
        public List<string> Images { get; private set; }
        public List<string> Hrefs { get; private set; }

        [JsonConstructor]
        public VkPost() { }

        public VkPost(IWebElement element)
        {
            if (IsPost(element))
            {
                Id = GetPostId(element);
                Text = GetPostText(element);
                Images = GetPostImages(element);
                Hrefs = GetPostHrefs(element);
            }
        }

        private static bool IsPost(IWebElement element)
        {
            if (element.Displayed)
            {
                List<IWebElement> list = element.FindElements(By.ClassName("feed_row")).ToList();
                return list.Count == 0;
            }
            return false;
        }

        private string GetPostId(IWebElement element)
        {
            IWebElement idElem = element.FindElement(By.TagName("div"));
            return idElem.GetAttribute("id");
        }

        private string GetPostText(IWebElement element)
        {
            return element.Text;
        }

        private List<string> GetPostImages(IWebElement element)
        {
            List<IWebElement> imageElements = element.FindElements(By.TagName("img")).ToList();
            List<string> images = new List<string>();
            foreach (var image in imageElements)
            {
                var imageRef = image.GetAttribute("src");
                if (imageRef != null)
                    images.Add(imageRef);
            }

            images = images
                .Where(x => !x.EndsWith(".png"))
                .Distinct()
                .ToList();
            return images;
        }

        private List<string> GetPostHrefs(IWebElement element)
        {
            List<IWebElement> hrefElements = element.FindElements(By.TagName("a")).ToList();
            List<string> hrefs = new List<string>();
            foreach (IWebElement href in hrefElements)
            {
                string hrefRef = href.GetAttribute("href");
                if (hrefRef != null)
                    hrefs.Add(hrefRef);
            }

            hrefs = hrefs
                .Where(x => !x.Contains("reply"))
                .Distinct()
                .ToList();
            return hrefs;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"ID : {Id}\nText : {Text}\nImages :\n");
            foreach (var image in Images)
                builder.Append($"\t{image.ToString()}\n");
            builder.Append("Hrefs :\n");
            foreach (var href in Hrefs)
                builder.Append($"\t{href.ToString()}\n");
            return builder.ToString();
        }
    }
}
