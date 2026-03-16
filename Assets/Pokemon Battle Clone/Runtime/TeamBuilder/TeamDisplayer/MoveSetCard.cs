using System.Collections.Generic;
using Pokemon_Battle_Clone.Runtime.Moves.Infrastructure;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.TeamBuilder.TeamDisplayer
{
    public class MoveSetCard : MonoBehaviour
    {
        [SerializeField] private List<MoveCard> moveCards;

        public void Display(MoveSetDTO moveSet)
        {
            for (var i = 0; i < moveCards.Count; i++)
            {
                if (moveSet.Moves.Count > i)
                {
                    moveCards[i].gameObject.SetActive(true);
                    moveCards[i].Display(moveSet.Moves[i]);
                }
                else
                {
                    moveCards[i].gameObject.SetActive(false);
                }
            }
        }
    }
}