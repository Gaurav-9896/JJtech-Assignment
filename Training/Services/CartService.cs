using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commercetools.Sdk.Api.Models.Carts;
using commercetools.Sdk.Api.Models.Channels;
using commercetools.Sdk.Api.Models.Customers;
using commercetools.Sdk.Api.Models.Payments;
using commercetools.Sdk.Api.Models.ShippingMethods;
using commercetools.Base.Client;
using commercetools.Sdk.Api.Extensions;
using Training.Extensions;
using commercetools.Sdk.Api.Models.Common;
using commercetools.Sdk.Api.Models.DiscountCodes;
using System.Security.AccessControl;

namespace Training.Services
{
    public class CartService
    {
        private readonly IClient _client;
        private readonly string _projectKey;
        
        public CartService(IClient client, string projectKey)
        {
            _client = client;
            _projectKey = projectKey;
        }

        /// <summary>
        /// GET a cart by Key
        /// </summary>
        /// <param name="cartKey"></param>
        /// <returns></returns>
        public async Task<ICart> GetCartByKey(string cartKey)
        {
            return await _client.WithApi().WithProjectKey(Settings.ProjectKey)
                .Carts()
                .WithKey(cartKey)
                .Get()
                .ExecuteAsync();
        }

        /// <summary>
        /// GET a cart by id
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        public async Task<ICart> GetCartById(string cartId)
        {
            return await _client.WithApi().WithProjectKey(Settings.ProjectKey)
                .Carts()
                .WithId(cartId)
                .Get()
                .ExecuteAsync();
        }

        /// <summary>
        /// Create a new cart for a customer with default shipping address
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<ICart> CreateCart(ICustomer customer)
        {

            try
            {
                var cartDraft = new CartDraft()
                {
                    Key = "cart27",
                    ShippingAddress = customer.GetDefaultShippingAddress(),
                    Currency = "EUR",

                    
                };
                return await _client
                    .WithApi()
                    .WithProjectKey(Settings.ProjectKey)
                    .Carts()
                    .Post(cartDraft)
                    .ExecuteAsync();

            }
            catch (Exception ex)
             {

                throw;
            }


        }

        /// <summary>
        /// Create an anonymous cart
        /// </summary>
        /// <param name="anonymousId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<ICart> CreateAnonymousCart(string anonymousId = null)
        {
            return await _client.WithApi().WithProjectKey(Settings.ProjectKey)
                .Carts()
                .Post(
                    new CartDraft
                    {
                        AnonymousId = anonymousId,
                        Currency = "EUR",
                        Country = "DE",
                        DeleteDaysAfterLastModification = 30
                    }
                )
                .ExecuteAsync();
        }

        /// <summary>
        /// Add Product to Cart by SKU and Supply Channel
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="channel"></param>
        /// <param name="skus"></param>
        /// <returns></returns>
        public async Task<ICart> AddProductsToCartBySkusAndChannel(ICart cart, string channelKey,
            params string[] skus)
        {
            try
            {
                var actions = new List<ICartUpdateAction>();
                foreach (var sku in skus)
                {
                    var lineItem = new CartAddLineItemAction()
                    {
                        Action = "addLineItem",
                        
                        Sku = sku,
                        SupplyChannel = new ChannelResourceIdentifier () { Key = channelKey },
                    };
                    actions.Add(lineItem);
                }

                var cartUpdate = new CartUpdate()
                {
                    Version = cart.Version,
                    Actions = actions
                };
                return await _client.WithApi()
                    .WithProjectKey(Settings.ProjectKey)
                    .Carts()
                    .WithId(cart.Id)
                    .Post(cartUpdate)
                    .ExecuteAsync();

            }
            catch (Exception ex)
            {

                throw;
            }
            
          
        }

        /// <summary>
        /// POST Add Discount Code update to the Cart
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<ICart> AddDiscountToCart(ICart cart, string code)
        {
            try
            {
            var actions = new List<ICartUpdateAction>();
            var discount = new CartAddDiscountCodeAction()
            {   Action = "addDiscountCode",
                Code = code,
            };

           actions .Add(discount);
            var cartUpdate = new CartUpdate()
            {
                Version = 4,
                Actions = actions
            };

            return await _client
                .WithApi()
                .WithProjectKey(Settings.ProjectKey)
                .Carts()
                .WithId(cart.Id)    
                .Post(cartUpdate)
                .ExecuteAsync();

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        //Recalculate a cart
        /// <summary>
        /// POST Recalculate update for the cart
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        public async Task<ICart> Recalculate(ICart cart)
        {
            try
            {

            var actions = new List<ICartUpdateAction>();
            var recalculate = new CartRecalculateAction()
            {
                Action = "recalculate",
                UpdateProductData = true,
            };
            actions .Add(recalculate);
            var cartUpdate = new CartUpdate()
            {
                Actions = actions,
                Version = 7,
            };

            return await _client
                .WithApi()
                .WithProjectKey(Settings.ProjectKey)
                .Carts()
                .WithId(cart.Id)
                .Post(cartUpdate)
                .ExecuteAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// POST Set ShippingMethod update for the cart
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        public async Task<ICart> SetShipping(ICart cart)
        {
            try
            {

            var actions = new List<ICartUpdateAction>();
            var shippingMethod = new CartSetShippingMethodAction()
            {
               Action= "setShippingMethod"
             
               
            };
            actions.Add(shippingMethod);
            var cartUpdate = new CartUpdate()
            {
                Actions = actions,
                Version = 8,
            };

            return await _client
                .WithApi()
                .WithProjectKey(Settings.ProjectKey)
                .Carts()
                .WithId(cart.Id)
                .Post(cartUpdate)
                .ExecuteAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// POST Add Payment to a cart
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="payment"></param>
        /// <returns></returns>
        public async Task<ICart> AddPaymentToCart(ICart cart, IPayment payment)
        {
            try
            {
            var actions = new List<ICartUpdateAction>();
            var addpayment = new CartAddPaymentAction()
            {
                Action = "addPayment",
               

            };
            actions.Add(addpayment);
            var cartUpdate = new CartUpdate()
            {
                Actions = actions,
                Version = cart.Version,
            };

            return await _client
                .WithApi()
                .WithProjectKey(Settings.ProjectKey)
                .Carts()
                .WithId(cart.Id)
                .Post(cartUpdate)
                .ExecuteAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
        
    }
