using System;
using System.Linq;
using UnityEngine;

namespace Pokemon_Battle_Clone.Editor.Database
{
    public class ConfigIdPolicy<T> where T : ScriptableObject
    {
        private readonly ConfigRepository<T> _repository;
        private readonly Func<T, int> _getId;
        private readonly int _reservedRangeStart;

        public ConfigIdPolicy(ConfigRepository<T> repository, Func<T, int> getId, int reservedRangeStart)
        {
            _repository = repository;
            _getId = getId;
            _reservedRangeStart = reservedRangeStart;
        }

        public int GenerateValidId()
        {
            var maxExisting = _repository.FindAll()
                .Select(_getId)
                .DefaultIfEmpty(_reservedRangeStart - 1)
                .Max();
            return Math.Max(maxExisting + 1, _reservedRangeStart);
        }

        public bool IsValidId(int id) => _repository.FindAll().All(c => _getId(c) != id);
    }
}