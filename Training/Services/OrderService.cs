using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Api.Models.Carts;
using commercetools.Sdk.Api.Models.Orders;
using commercetools.Sdk.Api.Models.States;
using commercetools.Base.Client;
using commercetools.Sdk.Api.Extensions;
using System.Net.Http.Headers;
using commercetools.Sdk.ImportApi.Models.Orders;
using commercetools.Sdk.Api.Models.Messages;
using IOrderState = commercetools.Sdk.Api.Models.Orders.IOrderState;
using commercetools.Sdk.Api.Models.Stores;

namespace Training.Services
{
    public class OrderService
    {
        private readonly IClient _client;
        private readonly string _projectKey;
        
        public OrderService(IClient client, string projectKey)
        {
            _client = client;
            _projectKey = projectKey;
        }

        /// <summary>
        /// Get an Order with order number
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        public async Task<IOrder> GetOrderByOrderNumber(string orderNumber)
        {
            return await _client.WithApi().WithProjectKey(Settings.ProjectKey)
                .Orders()
                .WithOrderNumber(orderNumber)
                .Get()
                .ExecuteAsync();
        }

        /// <summary>
        /// Create an Order From the cart
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        public async Task<IOrder> CreateOrder(ICart cart)
        {
            var orderDraft = new OrderFromCartDraft()
            { 
                Version = cart.Version,
                Cart =
                {
                    Id = cart.Id,
                }
            };

            return await _client
                .WithApi()
                .WithProjectKey(Settings.ProjectKey)
                .Orders()
                .Post(orderDraft)
                .ExecuteAsync();  
        }


        /// <summary>
        /// Change Order State
        /// </summary>
        /// <param name="order"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public async Task<IOrder> ChangeOrderState(IOrder order, IOrderState state)
        {
            var actions = new List<IOrderUpdateAction>();
            var changeState = new OrderChangeOrderStateAction
            {
                Action= "changeOrderState",
                OrderState = state

            };
            actions.Add(changeState);

            var stateUpdate = new OrderUpdate()
            {
                Actions = actions,
                Version = order.Version 
            };
           return await _client.WithApi()
                .WithProjectKey (Settings.ProjectKey)
                .Orders()
                .WithId(order.Id)   
                .Post(stateUpdate)
                .ExecuteAsync(); 
        }
        /// <summary>
        /// Change Order Workflow State
        /// </summary>
        /// <param name="order"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public async Task<IOrder> ChangeWorkflowState(IOrder order, string stateKey)

        {
            var actions = new List<IOrderUpdateAction>();
            var customField = new OrderSetCustomFieldAction()
            {
            Action = "setCustomField",
            Name = "Order Delivered",
            };

            actions.Add(customField);

            var stateUpdate = new OrderUpdate()
            {
                Actions = actions,
                Version= order.Version
            };

            return await _client
                .WithApi()
                .WithProjectKey(Settings.ProjectKey)
                .Orders()
                .WithId(order.Id)
                .Post(stateUpdate)
                .ExecuteAsync();

        }

    }
}