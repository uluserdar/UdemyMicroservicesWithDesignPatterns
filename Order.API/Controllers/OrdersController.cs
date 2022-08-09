﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.API.DTOs;
using Order.API.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateDto orderCreate)
        {
            var newOrder = new Models.Order
            {
                BuyerId = orderCreate.BuyerId,
                Status = OrderStatus.Suspend,
                Address = new Address
                {
                    Line = orderCreate.Address.Line,
                    Province = orderCreate.Address.Province,
                    District = orderCreate.Address.District
                },
                CreatedDate = DateTime.Now
            };

            orderCreate.OrderItems.ForEach(item =>
            {
                newOrder.Items.Add(new OrderItem()
                {
                    Price = item.Price,
                    ProductId = item.ProductId,
                    Count = item.Count
                });
            });

            await _context.AddAsync(newOrder);
            await _context.SaveChangesAsync();

            var orderCreatedEvent = new OrderCreatedEvent()
            {
                BuyerId = orderCreate.BuyerId,
                OrderId=newOrder.Id,
                Payment=new PaymentMessage
                {
                    //CardName=
                }

            };

            return Ok();
        }
    }
}
