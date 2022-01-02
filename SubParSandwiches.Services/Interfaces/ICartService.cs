﻿using SubParSandwiches.Entities;
using SubParSandwiches.Repositories.Models;
using System;

namespace SubParSandwiches.Services.Interfaces
{
    public interface ICartService
    {
        int GetCartCount(Guid cartId);
        CartModel GetCartDetails(Guid cartId);
        Cart AddItem(int UserId, Guid CartId, int ItemId, decimal UnitPrice, int Quantity);
        int DeleteItem(Guid cartId, int ItemId);
        int UpdateQuantity(Guid cartId, int id, int quantity);
        int UpdateCart(Guid CartId, int UserId);
    }
}
