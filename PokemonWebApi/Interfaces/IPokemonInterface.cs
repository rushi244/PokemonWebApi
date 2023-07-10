using PokemonWebApi.Models;

namespace PokemonWebApi.Interfaces
{
    public interface IPokemonInterface
    {
        ICollection<Pokemon>GetPokemons();
        Pokemon GetPokemon(int pokeId);
        Pokemon GetPokemonByname(string name);
        decimal GetPokemonRating(int pokeId);
        bool PokemonExists(int pokeId);
        bool Createpokemon(int ownerId,int categoryId,Pokemon pokemon);
        bool UpdatePokemon(int ownerId,int categoryId,Pokemon pokemon);
        bool DeletePokemon(Pokemon pokemon);
        bool Save();
    }
}
