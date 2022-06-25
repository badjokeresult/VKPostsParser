using Newtonsoft.Json;
using VkParser.Models;

namespace VkParser.Controllers
{
    public class FileWorker
    {
        static readonly string FileType = ".json";
        object locker = new object();

        public void SavePosts(List<VkPost> posts)
        {
            lock (locker)
            {
                for (int i = 0; i < posts.Count; i++)
                {

                    CreateFile(i + 1);
                    WriteToFile(i + 1, posts[i]);
                }
            }
        }

        private static void CreateFile(int FileNumber)
        {
            if (File.Exists(FileNumber.ToString() + FileType))
                File.Delete(FileNumber.ToString() + FileType);
            File.Create(FileNumber.ToString() + FileType);
        }

        private static void WriteToFile(int FileNumber, VkPost post)
        {
            Task writeFileTask = Task.Factory.StartNew(() =>
            {
                string serializedPost = JsonConvert.SerializeObject(post);
                File.WriteAllText(FileNumber.ToString() + FileType, serializedPost);
            });
            writeFileTask.Wait();
        }
        
        public List<VkPost> ReadPosts()
        {
            List<VkPost> posts = new List<VkPost>();
            int amountOfPosts = Directory.GetFiles(Directory.GetCurrentDirectory()).Length;
            lock (locker)
            {
                for (int i = 0; i < amountOfPosts; i++)
                {
                    if (File.Exists((i + 1).ToString() + FileType))
                    {
                        VkPost post = ReadFile(i + 1);
                        posts.Add(post);
                    }
                }
            }
            return posts;
        }

        private static VkPost ReadFile(int FileNumber)
        {
            Task<VkPost?> readFileTask = Task.Factory.StartNew(() =>
            {
                string serializedPost = File.ReadAllText(FileNumber.ToString() + FileType);
                VkPost post = JsonConvert.DeserializeObject<VkPost>(serializedPost);
                return post;
            });
            readFileTask.Wait();
            return readFileTask.Result;
        }
    }
}
