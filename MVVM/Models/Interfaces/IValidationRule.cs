namespace Furia_FanHub.MVVM.Models.Interfaces
{
    public interface IValidationRule<T>
    {
        string ValidationMessage { get; set; }
        bool Check(T value);
    }
}
