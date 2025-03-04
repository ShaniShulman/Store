using System;

namespace DTO
{
    public record GetCategoryDTO(int Id, string CategoryName);
    public record CategoryDTO(string CategoryName);
}