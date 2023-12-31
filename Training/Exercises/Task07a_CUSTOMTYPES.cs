using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commercetools.Sdk.Api.Models.Common;
using commercetools.Sdk.Api.Models.Types;
using commercetools.Base.Client;
using commercetools.Sdk.Api.Extensions;
using commercetools.Sdk.Api.Models.Orders;

namespace Training
{
    /// <summary>
    /// 1- Create TypeDraft with Custom fields
    /// 2- Create The Type and assign it to customers (as Resources you want to extend)
    public class Task07A : IExercise
    {
        private readonly IClient _client;

        public Task07A(IEnumerable<IClient> clients)
        {
            _client = clients.FirstOrDefault(c => c.Name.Equals("Client"));
        }

        public async Task ExecuteAsync()
        {
            // TODO: DEFINE fields
            var list = new List<IFieldDefinition>();
            var fieldDefinition = new FieldDefinition()
            {
                Type = new FieldType() { Name = "Boolean" },
                Name = "allowed-to-place-orders",
                Label = new LocalizedString() { { "en", "allowed-to-place-orders" } },
                Required = false
            };
            list.Add(fieldDefinition);

            var resourceId = new List<IResourceTypeId>()
            {
               IResourceTypeId.Order
            };


            // TODO: CREATE custom type with one field 'allowed-to-place-orders'

            var type = await CreateCustomType("kay1", new LocalizedString() { { "en", "allowed-to-place-orders" } }, resourceId, list);

            Console.WriteLine($"New custom type has been created with Id: {type.Id}");
        }



        /// <summary>
        /// Creates a new custom type
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="resourceTypeIds"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        private async Task<IType> CreateCustomType(string key,
            LocalizedString name,
            List<IResourceTypeId> resourceTypeIds,
            List<IFieldDefinition> fields) 
        {
            return await _client.WithApi().WithProjectKey(Settings.ProjectKey)
                .Types()
                .Post(
                    new TypeDraft
                    {
                        Key = key,
                        Name = name,
                        ResourceTypeIds = resourceTypeIds,
                        FieldDefinitions = fields
                    }
                )
                .ExecuteAsync();
        }
    }
}