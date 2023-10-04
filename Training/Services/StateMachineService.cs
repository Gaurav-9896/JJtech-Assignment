using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Api.Models.States;
using commercetools.Base.Client;
using commercetools.Sdk.Api.Extensions;
using System.Reflection.Metadata.Ecma335;
using commercetools.Sdk.Api.Models.Common;

namespace Training.Services
{
    public class StateMachineService
    {
        private readonly IClient _client;
        private readonly string _projectKey;

        public StateMachineService(IClient client, string projectKey)
        {
            _client = client;
            _projectKey = projectKey;
        }

        /// <summary>
        /// GET a state by key
        /// </summary>
        /// <param name="stateKey"></param>
        /// <returns></returns>
        public async Task<IState> GetStateByKey(string stateKey)
        {
            return await _client.WithApi().WithProjectKey(Settings.ProjectKey)
                .States()
                .WithKey(stateKey)
                .Get()
                .ExecuteAsync();
        }


        /// <summary>
        /// Creates a workflow state
        /// </summary>
        /// <param name="stateDraft"></param>
        /// <returns></returns>
        public async Task<IState> CreateState(IStateDraft stateDraft)
        {
            try
            {
                return await _client.WithApi()
              .WithProjectKey(Settings.ProjectKey)
              .States()
              .Post(stateDraft)
              .ExecuteAsync();
            }
            catch (Exception ex)
            {

                throw;
            }




        }

        /// <summary>
        /// POST a set transition update for the state
        /// </summary>
        /// <param name="stateKey"></param>
        /// <param name="transitionStateKeys"></param>
        /// <returns></returns>
        public async Task<IState> AddTransition(string stateKey, List<string> transitionStateKeys)
        {
            try
            {
                var stateUpdate = await _client
                               .WithApi()
                               .WithProjectKey(Settings.ProjectKey)
                               .States()
                               .WithKey(stateKey)
                               .Get()
                               .ExecuteAsync();

                var actions = new List<IStateUpdateAction>();

                var listofIdentifiers = new List<IStateResourceIdentifier>();

                foreach (var item in transitionStateKeys)
                {

                    listofIdentifiers.Add(new StateResourceIdentifier()
                    {
                        Key = item,
                       
                      
                    });
                }

                var Action = new StateSetTransitionsAction()
                {
                    Action = "setTransitions",
                    Transitions = listofIdentifiers

                };
                actions.Add(Action);    

                var transitionUpdate = new StateUpdate()
                {
                    Actions = actions,
                    Version = stateUpdate.Version

                };


                return await _client.WithApi()
                    .WithProjectKey(Settings.ProjectKey)
                    .States()
                    .WithKey(stateKey)
                    .Post(transitionUpdate)
                    .ExecuteAsync();

            
            }
            catch (Exception ex)
            {

                throw;
            }


        }
    }
}