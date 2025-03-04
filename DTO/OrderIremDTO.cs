using System;
using System.Collections.Generic;

namespace DTO
{
    public record OrderItemDTO(int ProductId, int? Quantity);
}