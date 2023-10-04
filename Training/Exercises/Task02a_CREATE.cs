using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commercetools.Sdk.Api.Models.Common;
using commercetools.Sdk.Api.Models.Customers;
using commercetools.Base.Client;
using Training.Services;
using commercetools.Sdk.Api.Client;
using commercetools.Sdk.ImportApi.Client;
using commercetools.Sdk.Api.Extensions;

namespace Training
{
    /// <summary>
    /// CREATE a customer
    /// CREATE a email verfification token
    /// Verfify customer
    /// </summary>
    public class Task02A : IExercise
    {
        private readonly IClient _client;
        private readonly CustomerService _customerService;
        private const string _customerKey = "123";

        public Task02A(IEnumerable<IClient> clients)
        {
            _client = clients.FirstOrDefault(c => c.Name.Equals("Client"));
            _customerService = new CustomerService(_client, Settings.ProjectKey);
        }

        public async Task ExecuteAsync()
        {
            // CREATE customer draft
            var customerDraft = new CustomerDraft
            {
                Email = "abc@yopmail.com",
                Password = "password",
                Key = _customerKey,
                FirstName = "",
                LastName = "",
                Addresses = new List<IBaseAddress>{
                        new AddressDraft {
                            Country = "DE",
                            Key = _customerKey +"-home"
                    }
                },
                DefaultShippingAddress = 0,
                DefaultBillingAddress = 0
            };


            // TODO: SIGNUP a customer
           // var customer = await _customerService.CreateCustomer(customerDraft);
           // Console.WriteLine($"Customer Created with Id : {customer.Id} and Key : {customer.Key} and Email Verified: {customer.IsEmailVerified}");


            // TODO: CREATE a email verfification token
          // var token = await _customerService.CreateCustomerToken(customer);
           

            // TODO: CONFIRM CustomerEmail
            //var retrievedCustomer =  await _customerService.ConfirmCustomerEmail(token);
           // Console.WriteLine($"Is Email Verified:{retrievedCustomer.IsEmailVerified}");

            
        }
    }
}