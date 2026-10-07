using System.Threading.Tasks;
using PokeApiNet;
using Pokemon_Battle_Clone.Runtime.Core.Domain;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEngine;
using MoveCategory = Pokemon_Battle_Clone.Runtime.Moves.Domain.MoveCategory;

namespace Pokemon_Battle_Clone.Editor.Database.PokeApi
{
    public class MoveApiLoader : IConfigApiLoader<MoveConfig, MoveApiDto>
    {
        private readonly PokeApiClient _pokeClient = new();

        public async Task<MoveApiDto> Fetch(string search)
        {
            var move = await _pokeClient.GetResourceAsync<PokeApiNet.Move>(search.Trim().ToLowerInvariant());
            return ToApiDto(move);
        }
        
        public MoveConfig CreateConfig(MoveApiDto dto)
        {
            var config = ScriptableObject.CreateInstance<MoveConfig>();
            config.id = dto.Id;
            config.moveName = dto.Name;
            config.type = dto.Type;
            config.category = dto.Category;
            config.pp = dto.Pp;
            config.accuracy = dto.Accuracy;
            config.power = dto.Power;
            config.priority = dto.Priority;
            return config;
        }

        public int GetId(MoveApiDto dto) => dto.Id;
        public string GetName(MoveApiDto dto) => dto.Name;

        private MoveApiDto ToApiDto(PokeApiNet.Move move)
        {
            var type = ElementalTypeUtils.GetType(move.Type.Name);
            var category = GetCategory(move.DamageClass.Name);

            return new MoveApiDto(
                move.Id, ToDisplayName(move.Name), type, category,
                move.Pp ?? 0, move.Accuracy ?? 100, move.Power ?? 0, move.Priority);
        }
        
        private static string ToDisplayName(string apiName)
        {
            var words = apiName.Split('-');
            for (var i = 0; i < words.Length; i++)
                words[i] = char.ToUpper(words[i][0]) + words[i][1..];
            return string.Join(" ", words);
        }
        
        private static MoveCategory GetCategory(string categoryName) => categoryName switch
        {
            "physical" => MoveCategory.Physical,
            "special" => MoveCategory.Special,
            "status" => MoveCategory.Status,
            _ => MoveCategory.Physical
        };
    }
}