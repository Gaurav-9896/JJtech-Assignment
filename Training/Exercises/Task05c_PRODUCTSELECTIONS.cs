using System.Threading.Tasks;
using commercetools.Sdk.Api.Models.Common;
using commercetools.Base.Client;
using commercetools.Sdk.Api.Extensions;
using Training.Services;
using commercetools.Sdk.Api.Models.Products;

namespace Training
{
    /// <summary>
    /// Create a Product Selection
    /// GET a Product Selection
    /// ADD/REMOVE products to/from the Product Selections
    /// ADD/REMOVE Product Selections to/from the stores
    /// </summary>
    public class Task05C : IExercise
    {
        private readonly IClient _client;
        private const string _storekey = "key1";
        private const string _productSelectionKey = "sample";
        private readonly ProductSelectionService _productSelectionService;
        private readonly StoreService _storeService;


        public Task05C(IClient client)
        {
            this._client = client;
            this._productSelectionService = new ProductSelectionService(_client,Settings.ProjectKey);
            this._storeService = new StoreService(_client,Settings.ProjectKey);
        }

        public async Task ExecuteAsync()
        {

            // TODO: CREATE a product selection
            var name = new LocalizedString { { "en", "sample" } };
            var productSelection = await _productSelectionService.CreateProductSelection(_productSelectionKey,name);

            System.Console.WriteLine($"Product selection: {productSelection.Id} with {productSelection.ProductCount} products");

            // TODO: ADD a product to the product selection
            var productKey = "";
            var updatedProductSelection = await _productSelectionService.AddProductToProductSelection(_productSelectionKey, productKey);
            System.Console.WriteLine($"Berlin Product selection: {updatedProductSelection.Id} with {updatedProductSelection.ProductCount} products");

            // TODO: set product selection for the store
            var store = await _storeService.AddProductSelectionToStore(_storekey ,_productSelectionKey);
            System.Console.WriteLine($"Updated store {store.Key} with selection {store.ProductSelections?.Count}");

            // System.Console.WriteLine($"Updated store {store.Key} with selection {updatedStore.ProductSelections?.Count}");
          
            /**
            var productSelectionProducts = await _productSelectionService.GetProductSelectionProductByKey(_productSelectionKey);

            System.Console.WriteLine($"Products in the product selection: {productSelectionProducts.Results.Count}");

            foreach (var product in productSelectionProducts.Results)
            {
                System.Console.WriteLine(product.Product.Obj.Key);
            }

            var productsInStore = await _storeService.GetProductsInStore(_storekey);

            System.Console.WriteLine($"Products in the store through product selections: {productsInStore.Results.Count}");

            foreach (var product in productsInStore.Results)
            {
                System.Console.WriteLine($"{product.Product.Id} through {product.ProductSelection.Id}");
            }
          

        }
    }
}