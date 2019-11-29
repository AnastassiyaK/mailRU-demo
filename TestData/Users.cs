using Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

namespace EmailTests.TestData
{
    public class Users
    {
        static Users()
        {
            User = JsonConvert.DeserializeObject<User>(GetJSONString());
        }

        public static User User { get; set; }

        public static string GetJSONString()
        {
            return File.ReadAllText($@"{GetExecutingDirectory()}\TestData\Users.json");
        }
        private static string GetExecutingDirectory()
        {
            var location = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase);
            return new FileInfo(location.LocalPath).Directory.FullName;
        }
    }
}
