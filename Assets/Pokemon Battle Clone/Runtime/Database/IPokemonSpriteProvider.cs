using System.Threading.Tasks;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Database
{
    public interface IPokemonSpriteProvider
    {
        Task<Sprite> GetBackSprite(uint id);
        Task<Sprite> GetFrontSprite(uint id);
        Task<Sprite> GetIconSprite(uint id);
        void Dispose();
    }
}