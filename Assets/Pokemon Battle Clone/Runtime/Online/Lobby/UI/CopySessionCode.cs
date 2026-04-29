using System.Collections;
using TMPro;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Online.Lobby.UI
{
    public class CopySessionCode : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI sessionCodeText;
        [SerializeField] private string copiedText;
        [SerializeField] private float copiedTextDuration;

        private bool _showingCopiedText;
        
        public void Copy()
        {
            if (_showingCopiedText) return;
            
            GUIUtility.systemCopyBuffer = sessionCodeText.text;
            
            StopAllCoroutines();
            StartCoroutine(ShowCopiedText());
        }

        private IEnumerator ShowCopiedText()
        {
            _showingCopiedText = true;
            var code = sessionCodeText.text;
            sessionCodeText.text = copiedText;
            yield return new WaitForSeconds(copiedTextDuration);
            sessionCodeText.text = code;
            _showingCopiedText = false;
        }
    }
}