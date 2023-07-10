﻿using PokemonWebApi.Models;

namespace PokemonWebApi.Interfaces
{
    public interface ICategoryInterface
    {
        ICollection<Category>GetCategories();
        Category GetCategory(int id);
        ICollection<Pokemon> GetPokemonsByCategory(int CategoryId);
        bool IsCategoryExists(int id);
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();
    }
}