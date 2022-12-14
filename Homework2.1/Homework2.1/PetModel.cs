using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Homework2._1
{
    public class PetModel
    {
        public int id { get; set; }
        public Category category { get; set; }
        public string name { get; set; }
        public string[] photoUrls { get; set; }
        public Tag[] tags { get; set; }
        public string status { get; set; }
    }



    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }
    }



    public class Tag
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
