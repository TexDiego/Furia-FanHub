using Furia_FanHub.MVVM.Models.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Furia_FanHub.MVVM.Helpers
{
    public class ValidatableObject<T> : INotifyPropertyChanged
    {
        private T _value;
        private bool _isValid = true;
        private string _error;

        public ObservableCollection<IValidationRule<T>> Validations { get; } = new();

        public T Value
        {
            get => _value;
            set
            {
                if (Equals(_value, value)) return;
                _value = value;
                OnPropertyChanged(nameof(Value));
                Validate();
            }
        }

        public bool IsValid
        {
            get => _isValid;
            set { _isValid = value; OnPropertyChanged(nameof(IsValid)); }
        }

        public string Error
        {
            get => _error;
            set { _error = value; OnPropertyChanged(nameof(Error)); }
        }

        public bool Validate()
        {
            foreach (var rule in Validations)
            {
                if (!rule.Check(Value))
                {
                    IsValid = false;
                    Error = rule.ValidationMessage;
                    return false;
                }
            }

            IsValid = true;
            Error = string.Empty;
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
