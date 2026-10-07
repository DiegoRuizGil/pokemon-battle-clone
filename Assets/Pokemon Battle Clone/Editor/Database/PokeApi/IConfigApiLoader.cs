using System.Threading.Tasks;
using UnityEngine;

namespace Pokemon_Battle_Clone.Editor.Database.PokeApi
{
    public interface IConfigApiLoader<T, TDto> where T : ScriptableObject
    {
        Task<TDto> Fetch(string search);
        T CreateConfig(TDto dto);
        int GetId(TDto dto);
        string GetName(TDto dto);
        Task AfterFetch(TDto dto) => Task.CompletedTask;
    }
}