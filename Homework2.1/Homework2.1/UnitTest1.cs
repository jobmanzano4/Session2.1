using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

[assembly: Parallelize(Workers = 10, Scope = ExecutionScope.MethodLevel)]
namespace Homework2._1
{
    [TestClass]
    public class UnitTest1
    {
        private static HttpClient httpClient;

        private static readonly string BaseURL = "https://petstore.swagger.io/v2/";

        private static readonly string UsersEndpoint = "pet";

        private static string GetURL(string enpoint) => $"{BaseURL}{enpoint}";

        private static Uri GetURI(string endpoint) => new Uri(GetURL(endpoint));

        private readonly List<PetModel> cleanUpList = new List<PetModel>();

        [TestInitialize]
        public void TestInitialize()
        {
            httpClient = new HttpClient();
        }

        [TestCleanup]
        public async Task TestCleanUp()
        {
            foreach (var data in cleanUpList)
            {
                var httpResponse = await httpClient.DeleteAsync(GetURL($"{UsersEndpoint}/{data.id}"));
            }
        }

        [TestMethod]
        public async Task PutMethod()
        {
            #region create data

            PetModel petData = new PetModel();
            petData.id = 123123;
            petData.category = new Category { id = 0, name = "string" };
            petData.name = "Whitey";
            petData.photoUrls = new string[] { "test1", "test2" };
            petData.tags = new Tag[] { new Tag() { id = 0, name = "string" } };
            petData.status = "available";

            //PetModel petData = new PetModel()
            //{
            //    id = 123123,
            //    category = new Category()
            //    {
            //        id = 0,
            //        name = "Dog"
            //    },
            //    name = "Doggy",
            //    photoUrls = new string[]
            //    {
            //        "test1",
            //        "test2"
            //    },
            //    status = "available",
            //    tags = new Tag[]
            //    {
            //        new Tag()
            //        {
            //           id = 0,
            //           name = "pet"
            //        }
            //    }
            //};
            // Serialize Content
            var request = JsonConvert.SerializeObject(petData);
            var postRequest = new StringContent(request, Encoding.UTF8, "application/json");

            // Send Post Request
            //var x = await httpClient.PostAsync(GetURL(UsersEndpoint), postRequest);
            //var a = x; //for debugging await output
            #endregion

            #region get created Pet

            // Get Request
            var getResponse = await httpClient.GetAsync(GetURI($"{UsersEndpoint}/{petData.id}"));

            // Deserialize Content
            var listPetData = JsonConvert.DeserializeObject<PetModel>(getResponse.Content.ReadAsStringAsync().Result);

            // filter created data
            var createdPetData = listPetData.name;

            #endregion

            #region send put request to update data

            // Update value of userData
            //petData = new PetModel()
            //{
            //    id = 123123,
            //    category = new Category()
            //    {
            //        id = 0,
            //        name = "Dog"
            //    },
            //    name = "DoggyUpdated",
            //    photoUrls = new string[]
            //    {
            //        "test11",
            //        "test22"
            //    },
            //    status = "not available",
            //    tags = new Tag[]
            //    {
            //        new Tag()
            //        {
            //           id = 0,
            //           name = "pet updated"
            //        }
            //    }
            //};
            
            petData.id = 123123;
            petData.category = new Category { id = 0, name = "string" };
            petData.name = "Whitey";
            petData.photoUrls = new string[] { "test1", "test2" };
            petData.tags = new Tag[] { new Tag() { id = 0, name = "string" } };
            petData.status = "available";


            // Serialize Content
            request = JsonConvert.SerializeObject(petData);
            postRequest = new StringContent(request, Encoding.UTF8, "application/json");

            // Send Put Request
            var httpResponse = await httpClient.PutAsync(GetURL($"{UsersEndpoint}/{createdPetData}"), postRequest);

            var x = httpResponse;

            // Get Status Code
            var statusCode = httpResponse.StatusCode;

            #endregion

            #region get updated data

            // Get Request
            getResponse = await httpClient.GetAsync(GetURI($"{UsersEndpoint}/{petData.id}"));

            // Deserialize Content
            listPetData = JsonConvert.DeserializeObject<PetModel>(getResponse.Content.ReadAsStringAsync().Result);

            // filter created data
            createdPetData = listPetData.name;

            #endregion

            #region cleanup data

            // Add data to cleanup list
            cleanUpList.Add(listPetData);

            #endregion

            #region assertion

            // Assertion
            Assert.AreEqual(HttpStatusCode.OK, statusCode, "Status code is not equal to 201");
            Assert.AreEqual(petData.name, createdPetData, "Pet Name not matching");

            #endregion

        }
    }
}