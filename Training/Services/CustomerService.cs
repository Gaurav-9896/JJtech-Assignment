using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Api.Models.CustomerGroups;
using commercetools.Sdk.Api.Models.Customers;
using commercetools.Base.Client;
using commercetools.Sdk.Api.Extensions;
using System.Linq;
using commercetools.Sdk.Api.Models.Common;

namespace Training.Services
{
    public class CustomerService
    {
        private readonly IClient _client;
        private readonly string _projectKey;
        
        public CustomerService(IClient client, string projectKey)
        {
            _client = client;
            _projectKey = projectKey;
        }
        /// <summary>
        /// GET Customer by key
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<ICustomer> GetCustomerByKey(string customerKey)
        {
            try
            {
            var customer =await _client.WithApi().WithProjectKey(Settings.ProjectKey)
               .Customers()
               .Get()
               .WithWhere($"key={customerKey}")
               .ExecuteAsync()
               ;
            return customer.Results.FirstOrDefault();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// POST a Customer sign-up
        /// </summary>
        /// <param name="customerDraft"></param>
        /// <returns></returns>
        public async Task<ICustomer> CreateCustomer(ICustomerDraft customerDraft)
        {
            
            {
                var customer = await _client.WithApi().WithProjectKey(Settings.ProjectKey)
              .Customers()
              .Post(customerDraft)
              .ExecuteAsync();

                return customer.Customer;
            }
          

        }

        /// <summary>
        /// Create a Customer Token
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<ICustomerToken> CreateCustomerToken(ICustomer customer)
        {
            var tokenDraft = new CustomerCreateEmailToken()
            {
                Id = $"{customer.Id}",
                TtlMinutes = 4320,
            };

            return await _client
                .WithApi()
                .WithProjectKey(Settings.ProjectKey)
                .Customers()
                .EmailToken()
                .Post(tokenDraft)
                .ExecuteAsync();

        }

        /// <summary>
        /// Confirm a Customer Email
        /// </summary>
        /// <param name="customerToken"></param>
        /// <returns></returns>
        public async Task<ICustomer> ConfirmCustomerEmail(ICustomerToken customerToken)
        {
            var token = new CustomerEmailVerify()
            {
                TokenValue = customerToken.Value
            };

            return await _client.WithApi().WithProjectKey(Settings.ProjectKey)
               .Customers().EmailConfirm().Post(token)
               .ExecuteAsync()
               ;
        }

        /// <summary>
        /// GET Customer Group by key
        /// </summary>
        /// <param name="customerGroupKey"></param>
        /// <returns></returns>
        public async Task<ICustomerGroup> GetCustomerGroupByKey(string customerGroupKey)
        {
            return await _client.WithApi().WithProjectKey(Settings.ProjectKey)
                .CustomerGroups()
                .WithKey(customerGroupKey)
                .Get()
                .ExecuteAsync();
        }

        /// <summary>
        /// POST Set Customer Group update for the customer
        /// </summary>
        /// <param name="customerKey"></param>
        /// <param name="customerGroupKey"></param>
        /// <returns></returns>
        public async Task<ICustomer> AssignCustomerToCustomerGroup(string customerKey, string customerGroupKey)
        {
            var customer = await _client.WithApi().WithProjectKey(Settings.ProjectKey)
                .Customers()
                .WithKey(customerKey).
                Get().
                ExecuteAsync();

            var actions = new List<ICustomerUpdateAction>();

            var action = new CustomerSetCustomerGroupAction()
            {
                Action = "setCustomerGroup",
                CustomerGroup = new CustomerGroupResourceIdentifier()
                {
                    Key = customerGroupKey,
                    TypeId = IReferenceTypeId.CustomerGroup
                }
            };

            actions.Add(action);

            var customerUpdate = new CustomerUpdate()
            {
                Version = customer.Version,
                Actions = actions,
            };

            return await _client.WithApi().WithProjectKey(Settings.ProjectKey)
                .Customers()
                .WithKey(customerKey)
                .Post(customerUpdate) .ExecuteAsync();


    }
        
    }
}