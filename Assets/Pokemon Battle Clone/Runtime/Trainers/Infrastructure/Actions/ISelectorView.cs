namespace Pokemon_Battle_Clone.Runtime.Trainers.Infrastructure.Actions
{
    public interface ISelectorView<T>
    {
        void Hide();
        void Show(bool forceSelection, T data);
    }
}