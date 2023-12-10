namespace PizzaApi.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Net.WebSockets;
    using System.Reflection.Metadata;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.CodeAnalysis;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using NUnit.Framework;
    using PizzaApi.Constants;
    using PizzaApi.Models;

    public class IntegrationTests : TestBase
    {

        /* 
         * Please read.
         * 
         * You have been provided with several "Happy path" test methods to implement in the time provided
         * 
         * All test methods are commented with details of the endpoints and what is required.
         * 
         * In some tests you will be required to alter the assertions, in others you must not do so, follow the commented instructions for each.
         * 
         * Points will be awarded even if tests are not completed so even if you are struggling it is worth commenting on what you would do even if you dont have the syntax for it.
         * 
         * At the end of the file, if you have time you are encouraged to think of and attempt to implement your own set of tests, this allows you to demonstrate your ability to devise your own tests
         * as above points will be awarded for sudo coded tests as well as fully implemented tests so the more you can add the better you will do.
         * 
         * Finally you have been provided with a commented out test that shows a very basic example of how to interact with the Rest API "PingServer"
         * it is not meant to be a demonstration of best practice, and you should use your experience of testing RestAPIs rather than this as a guide it is only there to help if you have issues.
         * 
        */


        /// <summary>
        /// HTTP Client configured to send REST requests to the API.
        /// </summary>
        protected override HttpClient Client { get; set; }

        /// <summary>
        /// Entity framework context that can be used to access the API database.
        /// </summary>
        protected override PizzaDbContext DbContext { get; set; }


        /// <summary>
        /// The following test can be uncommented to demonstrate how to send a basic request to the API
        /// </summary>
        [Test]
        public async Task PingServer()
        {
            var result = await this.Client.GetAsync("PizzaOrders/ping");
            result.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Get a count of all the pizza orders with the order status "Completed" using GET PizzaOrders
        /// Ensure that the assertion passes and that 385 orders are returned.
        /// </summary>
        [Test]
        public virtual async Task CountCompletedPizzaOrders()
        {
            // Implement the code to retrieve the completed orders so that the assertion succeeds.
            var completedPizzaOrders = new List<PizzaOrder>();
            var response = await this.Client.GetAsync("PizzaOrders?status=Completed");

            var content = await response.Content.ReadAsStringAsync();
            completedPizzaOrders = JsonConvert.DeserializeObject<List<PizzaOrder>>(content);

            response.EnsureSuccessStatusCode();
            completedPizzaOrders.Should().HaveCount(385);
        }

        /// <summary>
        /// Add a pizza order using POST PizzaOrders
        /// Assert that the order was added successfully by improving on the existing assertion which should be replaced with something more appropriate.
        /// </summary>
        [Test]
        public virtual async Task CreatePizzaOrder()
        {
            PizzaOrder createdPizzaOrder = new PizzaOrder()
            {
                Name = "TestPizzaOrder",
                OrderDate = DateTime.Now,
                Status = PizzaOrderStatus.Pending,
                Pizzas = new List<Pizza>() { new Pizza() { Name = "TestPizza", Price = 10, Size = PizzaSize.Medium } }
            };
            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var serializedContent = JsonConvert.SerializeObject(createdPizzaOrder, options);
            var content = new StringContent(serializedContent.ToString(), Encoding.UTF8, "application/json");

            // Implement the code to Post a pizza order to the system and ensure it was added successfully.
            var response = await this.Client.PostAsync("PizzaOrders", content);
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
        }

        /// <summary>
        /// Delete a Pizza Order from the system using DELETE PizzaOrders/{id}
        /// </summary>
        [Test]
        public virtual async Task DeletePizzaOrder()
        {
            PizzaOrder deletedPizzaOrder = default;
            // Implement the code to Delete a pizza order and ensure that the order status is successfully deleted.
            // You may add your own pizza and delete that one or delete on that is already there.

            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            PizzaOrder createdPizzaOrder = new PizzaOrder()
            {
                Name = "TestPizzaOrder",
                OrderDate = DateTime.Now,
                Status = PizzaOrderStatus.Pending,
                Pizzas = new List<Pizza>() { new Pizza() { Name = "TestPizza", Price = 10, Size = PizzaSize.Medium } }
            };

            var serializedContent = JsonConvert.SerializeObject(createdPizzaOrder, options);
            var content = new StringContent(serializedContent.ToString(), Encoding.UTF8, "application/json");

            // Implement the code to Post a pizza order to the system and ensure it was added successfully.
            var result = await this.Client.PostAsync("PizzaOrders", content);
            var idToDelete = await result.Content.ReadAsStringAsync();
            var response = await this.Client.DeleteAsync("PizzaOrders/" + idToDelete);

            // Replace this assertion with something more appropriate.
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Delete a Pizza Order from the system using PUT PizzaOrders/{id}
        /// </summary>
        [Test]
        public virtual async Task UpdatePizzaOrder()
        {
            PizzaOrder updatePizzaOrder = default;
            // Implement the code to update a pizza order and ensure that the order status is successfully transitioned from pending to in-progress and completed.
            // You may add your own pizza and delete that one or delete on that is already there.

            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            PizzaOrder createdPizzaOrder = new PizzaOrder()
            {
                Name = "TestPizzaOrder",
                OrderDate = DateTime.Now,
                Status = PizzaOrderStatus.Pending,
                Pizzas = new List<Pizza>() { new Pizza() { Name = "TestPizza", Price = 10, Size = PizzaSize.Medium } }
            };

            var serializedContent = JsonConvert.SerializeObject(createdPizzaOrder, options);
            var content = new StringContent(serializedContent.ToString(), Encoding.UTF8, "application/json");

            // Implement the code to Post a pizza order to the system and ensure it was added successfully.
            var result = await this.Client.PostAsync("PizzaOrders", content);
            var idToUpdate = await result.Content.ReadAsStringAsync();
            updatePizzaOrder = new PizzaOrder()
            {
                Id = long.Parse(idToUpdate),
                Name = "TestPizzaOrder",
                OrderDate = DateTime.Now,
                Status = PizzaOrderStatus.InProgress,
                Pizzas = new List<Pizza>() { new Pizza() { Name = "TestPizza", Price = 10, Size = PizzaSize.Medium } }
            };
            var updateSerializedContent = JsonConvert.SerializeObject(updatePizzaOrder, options);
            var updateContent = new StringContent(updateSerializedContent.ToString(), Encoding.UTF8, "application/json");

            var response = await this.Client.PutAsync("PizzaOrders", updateContent);

            // Replace this assertion with something more appropriate.
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// The database contains a list of Historical Pizza orders
        /// Use the GET PizzaOrders endpoint to retrieve the completed orders from last year.
        /// Get the total price of the pizzas, grouped by month.
        /// </summary>
        [Test]
        public virtual async Task GetPizzaTotalPriceGroupedByMonthForLastYear()
        {
            // Implement the code to update this variable to contain the values included in the database such that the assertion passes.
            Dictionary<string, decimal> actualResult = default;

            var expectedResult = new Dictionary<string, decimal>()
            {
                { "January", 696.0m }, { "February", 816.0m }, { "March", 392m }, { "April", 332.0m },
                { "May", 292.0m }, { "June", 696.0m }, { "July", 816.0m }, { "August", 392m },
                { "September", 332.0m }, { "October", 292.0m }, { "November", 696.0m }, { "December", 816.0m }
            };

            var completedPizzaOrders = new List<PizzaOrder>();
            var response = await this.Client.GetAsync("PizzaOrders?status=Completed");

            var content = await response.Content.ReadAsStringAsync();
            completedPizzaOrders = JsonConvert.DeserializeObject<List<PizzaOrder>>(content);
            actualResult = completedPizzaOrders.Where(x => x.OrderDate.Year == DateTime.Now.AddYears(-1).Year)
                .GroupBy(x => new { x.OrderDate.Year, x.OrderDate.Month }, (key, group) =>
                    new
                    {
                        Month = DateTimeFormatInfo.CurrentInfo.GetMonthName(key.Month),
                        TotalCost = group.Sum(order => order.Pizzas.Sum(pizza => pizza.Price))
                    }).ToDictionary(x => x.Month, x => x.TotalCost);


            actualResult.Should().Equal(expectedResult);
        }

        /*
         * For the remainder of the time implement some tests of your own for testing the endpoints.
         * Points will be given for tests that are written, even if they are not implemented provided we can understand what the test is trying to achieve.
         */

        [Test]
        public virtual async Task PostPizzaOrderWithDateTimeInPast()
        {
            PizzaOrder createdPizzaOrder = new PizzaOrder()
            {
                Name = "TestPizzaOrder",
                OrderDate = DateTime.Now.AddMinutes(-5),
                Status = PizzaOrderStatus.Pending,
                Pizzas = new List<Pizza>() { new Pizza() { Name = "TestPizza", Price = 10, Size = PizzaSize.Medium } }
            };

            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var serializedContent = JsonConvert.SerializeObject(createdPizzaOrder, options);
            var content = new StringContent(serializedContent.ToString(), Encoding.UTF8, "application/json");

            // Implement the code to Post a pizza order to the system and ensure it was added successfully.
            var response = await this.Client.PostAsync("PizzaOrders", content);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public virtual async Task PostPizzaOrderWithNoPizzas()
        {
            PizzaOrder createdPizzaOrder = new PizzaOrder()
            {
                Name = "TestPizzaOrder",
                OrderDate = DateTime.Now.AddMinutes(-5),
                Status = PizzaOrderStatus.Pending,
                Pizzas = new List<Pizza>() { }
            };

            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var serializedContent = JsonConvert.SerializeObject(createdPizzaOrder, options);
            var content = new StringContent(serializedContent.ToString(), Encoding.UTF8, "application/json");

            // Implement the code to Post a pizza order to the system and ensure it was added successfully.
            var response = await this.Client.PostAsync("PizzaOrders", content);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public virtual async Task UpdateWithNotExistingId()
        {
            var updatePizzaOrder = new PizzaOrder()
            {
                Id = 12345678,
                Name = "TestPizzaOrder",
                OrderDate = DateTime.Now,
                Status = PizzaOrderStatus.InProgress,
                Pizzas = new List<Pizza>() { new Pizza() { Name = "TestPizza", Price = 10, Size = PizzaSize.Medium } }
            };
            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var updateSerializedContent = JsonConvert.SerializeObject(updatePizzaOrder, options);
            var updateContent = new StringContent(updateSerializedContent.ToString(), Encoding.UTF8, "application/json");

            var response = await this.Client.PutAsync("PizzaOrders", updateContent);

            // Replace this assertion with something more appropriate.
            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }

        [Test]
        public virtual async Task UpdateCompletedPizzaOrder()
        {
            PizzaOrder updatePizzaOrder = default;
            // Implement the code to update a pizza order and ensure that the order status is successfully transitioned from pending to in-progress and completed.
            // You may add your own pizza and delete that one or delete on that is already there.

            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            PizzaOrder createdPizzaOrder = new PizzaOrder()
            {
                Name = "TestPizzaOrder",
                OrderDate = DateTime.Now,
                Status = PizzaOrderStatus.Completed,
                Pizzas = new List<Pizza>() { new Pizza() { Name = "TestPizza", Price = 10, Size = PizzaSize.Medium } }
            };

            var serializedContent = JsonConvert.SerializeObject(createdPizzaOrder, options);
            var content = new StringContent(serializedContent.ToString(), Encoding.UTF8, "application/json");

            // Implement the code to Post a pizza order to the system and ensure it was added successfully.
            var result = await this.Client.PostAsync("PizzaOrders", content);
            var idToUpdate = await result.Content.ReadAsStringAsync();
            updatePizzaOrder = new PizzaOrder()
            {
                Id = long.Parse(idToUpdate),
                Name = "TestPizzaOrder",
                OrderDate = DateTime.Now,
                Status = PizzaOrderStatus.InProgress,
                Pizzas = new List<Pizza>() { new Pizza() { Name = "TestPizza", Price = 10, Size = PizzaSize.Medium } }
            };
            var updateSerializedContent = JsonConvert.SerializeObject(updatePizzaOrder, options);
            var updateContent = new StringContent(updateSerializedContent.ToString(), Encoding.UTF8, "application/json");

            var response = await this.Client.PutAsync("PizzaOrders", updateContent);

            // Replace this assertion with something more appropriate.
            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public virtual async Task UpdatePizzaOrderWithNoPizzas()
        {
            PizzaOrder updatePizzaOrder = default;
            // Implement the code to update a pizza order and ensure that the order status is successfully transitioned from pending to in-progress and completed.
            // You may add your own pizza and delete that one or delete on that is already there.

            var options = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            PizzaOrder createdPizzaOrder = new PizzaOrder()
            {
                Name = "TestPizzaOrder",
                OrderDate = DateTime.Now,
                Status = PizzaOrderStatus.Pending,
                Pizzas = new List<Pizza>() { new Pizza() { Name = "TestPizza", Price = 10, Size = PizzaSize.Medium } }
            };

            var serializedContent = JsonConvert.SerializeObject(createdPizzaOrder, options);
            var content = new StringContent(serializedContent.ToString(), Encoding.UTF8, "application/json");

            // Implement the code to Post a pizza order to the system and ensure it was added successfully.
            var result = await this.Client.PostAsync("PizzaOrders", content);
            var idToUpdate = await result.Content.ReadAsStringAsync();
            updatePizzaOrder = new PizzaOrder()
            {
                Id = long.Parse(idToUpdate),
                Name = "TestPizzaOrder",
                OrderDate = DateTime.Now,
                Status = PizzaOrderStatus.InProgress,
                Pizzas = new List<Pizza>() { }
            };
            var updateSerializedContent = JsonConvert.SerializeObject(updatePizzaOrder, options);
            var updateContent = new StringContent(updateSerializedContent.ToString(), Encoding.UTF8, "application/json");

            var response = await this.Client.PutAsync("PizzaOrders", updateContent);

            // Replace this assertion with something more appropriate.
            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public virtual async Task DeletePizzaOrderWithNonExistingId()
        {
            var response = await this.Client.DeleteAsync("PizzaOrders/123456789");

            // Replace this assertion with something more appropriate.
            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }
    }
}
