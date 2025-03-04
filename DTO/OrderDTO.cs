using System;
using System.Collections.Generic;

namespace DTO
{
    public record OrderDTO(DateTime OrderDate, double? OrderSum, int UserId, List<OrderItemDTO> OrderItems);
    public record GetOrderDTO(int Id, DateTime OrderDate, double? OrderSum, string UserUserName, List<OrderItemDTO> OrderItems);
}