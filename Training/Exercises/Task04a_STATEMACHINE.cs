using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commercetools.Sdk.Api.Models.Common;
using commercetools.Sdk.Api.Models.States;
using commercetools.Base.Client;
using Training.Services;

namespace Training
{
    /// <summary>
    /// create 2 states, stateOrderPacked and stateOrderShipped with Transition
    /// </summary>
    public class Task04A : IExercise
    {
        private readonly IClient _client;
        private readonly StateMachineService _stateMachineService;

        public Task04A(IEnumerable<IClient> clients)
        {
            _client = clients.FirstOrDefault(c => c.Name.Equals("Client"));
            _stateMachineService = new StateMachineService(_client, Settings.ProjectKey);
        }

        public async Task ExecuteAsync()
        {
            // TODO: CREATE OrderPacked stateDraft, state
            var transitionKeys = new List<string>();


            var stateOrderPackedDraft = new StateDraft
            {
                Key = "OrderConfirmed",
                Initial = true,
                Name = new LocalizedString { { "en", "Order Confirmed" } },
                Type = IStateTypeEnum.OrderState,
                
               
            };

           var stateOrderPacked = await _stateMachineService.CreateState(stateOrderPackedDraft);


            // TODO: CREATE OrderShipped stateDraft, state
            var stateOrderShippedDraft = new StateDraft
            { 
                Key = "OrderShipped",
                Initial = false,
                Name = new LocalizedString {{"en", "Order Shipped"}},
                Type = IStateTypeEnum.OrderState,
                
            };

            var stateOrderShipped = await _stateMachineService.CreateState(stateOrderShippedDraft);

            var stateOrderDeliveredDraft = new StateDraft
            {
                Key = "OrderDelivered",
                Initial = false,
                Name = new LocalizedString { { "en", "Order Delivered" } },
                Type = IStateTypeEnum.OrderState,
            };

            var stateOrderDelivered = await _stateMachineService.CreateState(stateOrderDeliveredDraft);

            transitionKeys.Add(stateOrderPacked.Key);
            transitionKeys.Add(stateOrderShipped.Key);
            transitionKeys.Add(stateOrderDelivered.Key);


            // TODO: UPDATE packedState to transit to stateShipped
            var updatedStateOrderPacked = await _stateMachineService.AddTransition(stateOrderPacked.Key, transitionKeys);

            Console.WriteLine($"stateOrderShipped Id : {stateOrderShipped.Id}, stateOrderPacked transition to:  {updatedStateOrderPacked.Transitions[0].Id}");
        }
    }
}