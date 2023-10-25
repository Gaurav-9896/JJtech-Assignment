using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commercetools.Sdk.Api.Models.Channels;
using commercetools.Sdk.Api.Models.Orders;
using commercetools.Sdk.Api.Models.States;
using commercetools.Sdk.Api.Models.Subscriptions;
using commercetools.Base.Client;
using commercetools.Sdk.Api.Extensions;

using Training.Services;
using System.Runtime.InteropServices;

namespace Training
{
    /// <summary>
    /// Create a cart for a customer, add a product to it, create an order from the cart and change the order state.
    /// </summary>
    public class Task04B : IExercise
    {
        private readonly IClient _client;
        private const string _channelKey = "store_other";
        private const string _customerKey = "1234";
        private const string _discountCode = "BOGO";
        private const string _customStateKey = "OrderConfirmed";
        private const string _cartId = "";

        private readonly CustomerService _customerService;
        private readonly CartService _cartService;
        private readonly PaymentService _paymentService;
        private readonly OrderService _orderService;


        public Task04B(IEnumerable<IClient> clients)
        {
            _client = clients.FirstOrDefault(c => c.Name.Equals("Client"));
            _customerService = new CustomerService(_client, Settings.ProjectKey);
            _cartService = new CartService(_client, Settings.ProjectKey);
            _paymentService = new PaymentService(_client, Settings.ProjectKey);
            _orderService = new OrderService(_client, Settings.ProjectKey);

        }

        public async Task ExecuteAsync()
        {
            // TODO: GET customer
            var customer = await _customerService.GetCustomerByKey(_customerKey);
            //// TODO: CREATE a cart for the customer
            var cart = await _cartService.CreateCart(customer);
            Console.WriteLine($"Cart {cart.Id} for customer: {cart.CustomerId}");

            // TODO: GET cart, created in previous step
            var getCart = await _cartService.GetCartById(cart.Id);
            // TODO: ADD items to the cart
            string[] skus = { "148096" };
            var addProducts = await _cartService.AddProductsToCartBySkusAndChannel(getCart, _channelKey, skus);
                
            // TODO: ADD discount coupon code to the cart
            var addDiscount = await _cartService.AddDiscountToCart(getCart,_discountCode);
            // TODO: RECALCULATE the cart
            var recalculate = await _cartService.Recalculate(getCart);
            // TODO: ADD default shipping to the cart
            var defaultShipping =await _cartService.SetShipping(getCart);
            // TODO: CREATE a payment 
            string pspname = "wirecard";
            string pspmethod = "creditcard";
            string interfaceid = "visa2";
            var payment =await  _paymentService.CreatePayment(getCart, pspname, pspmethod, interfaceid);
            Console.WriteLine($"Payment Created with Id: {payment.Id}");

            // TODO: ADD transaction to the payment
            string interactionId = "interaction"; 
            var addTransaction = await _paymentService.AddTransactionToPayment(getCart,payment,interactionId);
            // TODO: ADD payment to the cart
            var addPayment = await _cartService.AddPaymentToCart(getCart, payment);
            // TODO: CREATE order
            var order = await _orderService.CreateOrder(getCart);
            Console.WriteLine($"Order Created with order number: {order.OrderNumber}");

            // TODO: UPDATE order state to Confirmed
            var OrderState = await _orderService.ChangeOrderState(order, order.OrderState);
           Console.WriteLine($"Order state changed to: {order.OrderState.Value}");

            // TODO: UPDATE order custom workflow state
          
            var customWorkflow = await _orderService.ChangeWorkflowState(order, _customStateKey);
          Console.WriteLine($"Order Workflow State changed to: {order.State?.Obj?.Name["en"]}");
        }

    }
}