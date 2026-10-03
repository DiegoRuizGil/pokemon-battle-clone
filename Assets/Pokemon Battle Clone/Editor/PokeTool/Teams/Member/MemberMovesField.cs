using System;
using System.Collections.Generic;
using Pokemon_Battle_Clone.Runtime.Database;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pokemon_Battle_Clone.Editor.PokeTool.Teams.Member
{
    public class MemberMovesField : VisualElement
    {
        public event Action<int, MoveConfig> OnPicked;

        private readonly List<TextField> _fields = new();
        private readonly List<SearchBinding<MoveConfig>> _searches = new();

        public MemberMovesField(PanelHost host, Func<List<MoveConfig>> getItems)
        {
            var picker = new ConfigPicker<MoveConfig>(
                getName: m => m.moveName,
                makeRow: () => new Label(),
                bindRow: (row, move) => ((Label)row).text = move.moveName,
                rowHeight: 28);

            for (int i = 0; i < TeamMember.MaxMoves; i++)
            {
                var slot = i;
                var field = new TextField();
                _fields.Add(field);
                _searches.Add(new SearchBinding<MoveConfig>(
                    field, picker, host, getItems, move => HandlePicked(slot, move)));
                this.Add(field);
            }
        }

        public void SetMoves(List<MoveConfig> moves)
        {
            for (var i = 0; i < _fields.Count; i++)
            {
                var move = i < moves.Count ? moves[i] : null;
                _fields[i].SetValueWithoutNotify(move != null ? move.moveName : "");
            }
        }

        private void HandlePicked(int slot, MoveConfig move)
        {
            OnPicked?.Invoke(slot, move);
            
            if (slot >= _fields.Count - 1) return;
            var next = _fields[slot + 1];
            next.schedule.Execute(() => next.Focus());
        }
    }
}