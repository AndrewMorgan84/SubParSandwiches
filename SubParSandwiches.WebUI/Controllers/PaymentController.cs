﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SubParSandwiches.Entities;
using SubParSandwiches.Repositories.Models;
using SubParSandwiches.Services.Interfaces;
using SubParSandwiches.Services.Models;
using SubParSandwiches.WebUI.Interfaces;
using SubParSandwiches.WebUI.Models;
using SubParSandwiches.WebUI.Helpers;
using System;

namespace SubParSandwiches.WebUI.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly IOptions<RazorPayConfig> _razorPayConfig;
        private readonly IPaymentService _paymentService;
        //private readonly IOrderService _orderService;
        public IUserAccessor _userAccessor { get; set; }
        public PaymentController(IOptions<RazorPayConfig> razorPayConfig, IPaymentService paymentService,
            IUserAccessor userAccessor) : base(userAccessor)
        {
            _razorPayConfig = razorPayConfig;
            _paymentService = paymentService;
        }

        public IActionResult Index()
        {
            PaymentViewModel payment = new PaymentViewModel();
            CartModel cart = TempData.Peek<CartModel>("Cart");

            if (cart != null)
            {
                payment.Cart = cart;
            }

            payment.GrandTotal = Math.Round(cart.GrandTotal);
            payment.Currency = "GBP";
            string items = "";
            foreach (var item in cart.Items)
            {
                items += item.Name + ",";
            }
            payment.Description = items;
            payment.RazorpayKey = _razorPayConfig.Value.Key;
            payment.Receipt = Guid.NewGuid().ToString();
            payment.OrderId = _paymentService.CreateOrder(payment.GrandTotal * 100, payment.Currency, payment.Receipt);

            return View(payment);
        }

        [HttpPost]
        public IActionResult Status(IFormCollection form)
        {
            try
            {
                if (form.Keys.Count > 0 && !String.IsNullOrWhiteSpace(form["rzp_paymentid"]))
                {
                    string paymentId = form["rzp_paymentid"];
                    string orderId = form["rzp_orderid"];
                    string signature = form["rzp_signature"];
                    string transactionId = form["Receipt"];
                    string currency = form["Currency"];

                    var payment = _paymentService.GetPaymentDetails(paymentId);
                    bool IsSignVerified = _paymentService.VerifySignature(signature, orderId, paymentId);

                    if (IsSignVerified && payment != null)
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
            ViewBag.Message = "Your payment has been failed. You can contact us at support@dotnettricks.com.";
            return View();
        }

        public IActionResult Receipt()
        {
            PaymentDetails model = TempData.Peek<PaymentDetails>("PaymentDetails");
            return View(model);
        }
    }
}
