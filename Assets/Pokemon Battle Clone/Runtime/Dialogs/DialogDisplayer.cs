using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Pokemon_Battle_Clone.Runtime.Battles.Infrastructure.Dialogs
{
    public class DialogDisplayer : MonoBehaviour, IDialogDisplay
    {
        [SerializeField] private TextMeshProUGUI displayText;
        [SerializeField] private float timeToClear = 2f;
        [SerializeField] private float typingSpeed = 5f;
        
        private TaskCompletionSource<bool> _textTCS;
        
        private Coroutine _typingCoroutine;
        
        private bool _textDisplayed;
        private float _textTimer;

        private void Start()
        {
            Clear();
        }

        private void Update()
        {
            if (_textDisplayed)
            {
                _textTimer += Time.deltaTime;
                if (_textTimer >= timeToClear)
                    CompleteText();
            }
        }

        public async Task DisplayAsync(string text)
        {
            Display(text);
            
            _textTCS = new TaskCompletionSource<bool>();
            await _textTCS.Task;
        }

        public void Display(string text)
        {
            StartTypingText(text);
        }

        public void Close() => Clear();

        private void CompleteText()
        {
            Clear();
            _textTCS.TrySetResult(true);
        }

        private void Clear()
        {
            _textDisplayed = false;
            displayText.text = "";
        }

        private void StartTypingText(string text)
        {
            if (_typingCoroutine != null)
                StopCoroutine(_typingCoroutine);
            _typingCoroutine = StartCoroutine(TypeText(text));
        }

        private IEnumerator TypeText(string text)
        {
            _textDisplayed = false;
            _textTimer = 0f;
            
            displayText.text = text;
            displayText.maxVisibleCharacters = 0;
            
            foreach (var _ in text.ToCharArray())
            {
                displayText.maxVisibleCharacters++;
                yield return new WaitForSeconds(1f / typingSpeed);
            }
            _textDisplayed = true;
        }
    }
}