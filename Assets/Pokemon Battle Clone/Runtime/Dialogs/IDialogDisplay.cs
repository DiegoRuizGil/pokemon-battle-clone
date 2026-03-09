using System.Threading.Tasks;

namespace Pokemon_Battle_Clone.Runtime.Battles.Infrastructure.Dialogs
{
    public interface IDialogDisplay
    {
        Task DisplayAsync(string text);
        
        void Display(string text);
        void Close();
    }
}