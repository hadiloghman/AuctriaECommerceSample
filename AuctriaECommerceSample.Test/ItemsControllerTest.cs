using AuctriaECommerceSample.Controllers;
using AuctriaECommerceSample.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;

namespace AuctriaECommerceSample.Test
{

    [TestFixture]
    public class ItemsControllerTests
    {
        private ItemsController _controller;
        private string _baseUrl;
        private HttpClient _httpClient;
        [SetUp]
        public void SetUp()
        {
            var factory = new WebApplicationFactory<Program>();
            _httpClient = factory.CreateClient();

            _controller = new ItemsController();
            _baseUrl = "https://localhost:7220/";
            //_httpClient = new HttpClient();
        }

        [Test, Order(1)]
        public async Task AddItem_ShouldReturnOk()
        {
            var newItem = new Item { Title = "Test Item1", Price = decimal.MaxValue };
            string jsonContent = JsonConvert.SerializeObject(newItem);
            StringContent content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage result = await _httpClient.PostAsync("/api/items/AddItem", content);
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            newItem = new Item { Title = "Test Item2", Price = 1M };
            jsonContent = JsonConvert.SerializeObject(newItem);
            content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            result = await _httpClient.PostAsync("/api/items/AddItem", content);
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));


            newItem = new Item { Title = "Test Item3", Price = 1000M };
            jsonContent = JsonConvert.SerializeObject(newItem);
            content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            result = await _httpClient.PostAsync("/api/items/AddItem", content);
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));

        }

        [Test, Order(2)]
        public async Task AddItem_ShouldReturnError()
        {
            var newItem = new Item { Title = "Test Item1", Price = decimal.MaxValue };
            string jsonContent = JsonConvert.SerializeObject(newItem);
            StringContent content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync("/api/items/AddItem", content);
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            var responseStr = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(responseStr.Contains("duplicated", StringComparison.OrdinalIgnoreCase));

            newItem = new Item { Title = "Test Item2", Price = decimal.MinValue };
            jsonContent = JsonConvert.SerializeObject(newItem);
            content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            response = await _httpClient.PostAsync("/api/items/AddItem", content);
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            responseStr = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(responseStr.Contains("not a positive value", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(responseStr.Contains("duplicated", StringComparison.OrdinalIgnoreCase));


            newItem = new Item { Title = "", Price = 100 };
            jsonContent = JsonConvert.SerializeObject(newItem);
            content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            response = await _httpClient.PostAsync("/api/items/AddItem", content);
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            responseStr = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(responseStr.Contains("Title of the item is not specified", StringComparison.OrdinalIgnoreCase));


            newItem = new Item { Title = "Test Item3", Price = decimal.MinValue };
            jsonContent = JsonConvert.SerializeObject(newItem);
            content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            response = await _httpClient.PostAsync("/api/items/AddItem", content);
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            responseStr = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(responseStr.Contains("not a positive value", StringComparison.OrdinalIgnoreCase));

        }

        [Test, Order(3)]
        public async Task UpdateItem_ShouldReturnOk()
        {
            var existingItem = new Item { Id = 1, Title = "Existing Item", Price = 15M };
            string jsonContent = JsonConvert.SerializeObject(existingItem);
            StringContent content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage result = await _httpClient.PostAsync("/api/items/UpdateItem", content);
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test, Order(3)]
        public async Task UpdateItem_ShouldReturnError()
        {
            var existingItem = new Item { Id = 1, Title = "Test Item2", Price = 15M };
            string jsonContent = JsonConvert.SerializeObject(existingItem);
            StringContent content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync("/api/items/UpdateItem", content);
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            var responseStr = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(responseStr.Contains("duplicated", StringComparison.OrdinalIgnoreCase));


            existingItem = new Item { Id = 1, Title = "Test Item2", Price = decimal.MinValue };
            jsonContent = JsonConvert.SerializeObject(existingItem);
            content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            response = await _httpClient.PostAsync("/api/items/UpdateItem", content);
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            responseStr = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(responseStr.Contains("not a positive value", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(responseStr.Contains("duplicated", StringComparison.OrdinalIgnoreCase));


            existingItem = new Item { Id = 1, Title = "", Price = 100 };
            jsonContent = JsonConvert.SerializeObject(existingItem);
            content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            response = await _httpClient.PostAsync("/api/items/UpdateItem", content);
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            responseStr = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(responseStr.Contains("Title of the item is not specified", StringComparison.OrdinalIgnoreCase));


            existingItem = new Item { Id = 1, Title = "Test Item3", Price = decimal.MinValue };
            jsonContent = JsonConvert.SerializeObject(existingItem);
            content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            response = await _httpClient.PostAsync("/api/items/UpdateItem", content);
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            responseStr = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(responseStr.Contains("not a positive value", StringComparison.OrdinalIgnoreCase));


            existingItem = new Item { Id = 10, Title = "Test Item3", Price = decimal.MinValue };
            jsonContent = JsonConvert.SerializeObject(existingItem);
            content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            response = await _httpClient.PostAsync("/api/items/UpdateItem", content);
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            responseStr = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(responseStr.Contains("not a positive value", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(responseStr.Contains("There is no item with Id", StringComparison.OrdinalIgnoreCase));
        }


        [Test, Order(4)]
        public async Task DeleteItem_ShouldReturnOk()
        {
            var itemId = 3;

            HttpResponseMessage response = await _httpClient.GetAsync($"/api/items/DeleteItem/{itemId}");
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            string itemTitle = "Test Item2";
            response = await _httpClient.GetAsync($"/api/items/DeleteItemByTitle/{itemTitle}");
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test, Order(5)]
        public async Task DeleteItem_ShouldReturnError()
        {
            var itemId = 30;

            HttpResponseMessage response = await _httpClient.GetAsync($"/api/items/DeleteItem/{itemId}");
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            var responseStr = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(responseStr.Contains("There is no item with Id", StringComparison.OrdinalIgnoreCase));

            string itemTitle = "Test Item2";
            response = await _httpClient.GetAsync($"/api/items/DeleteItemByTitle/{itemTitle}");
            Assert.IsNotNull(response);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            responseStr = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(responseStr.Contains("There is no item with Title", StringComparison.OrdinalIgnoreCase));
        }

       
    }

}
