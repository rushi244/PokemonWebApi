using PokemonWebApi.Models;

namespace PokemonWebApi.Interfaces
{
    public interface IOwnerInterface
    {
        ICollection<Owner> GetOwners();
        Owner GetOwner(int id);
        ICollection<Owner> GetOwnerOfAPokemon(int pokId);
        ICollection<Pokemon> GetPokemonByOwner(int ownerId);
        bool IsOwnerExists(int ownerId);
        bool CreateOwner(Owner owner);
        bool UpdateOwner(Owner owner);
        bool DeleteOwner(Owner owner);
        bool Save();
    }
}
