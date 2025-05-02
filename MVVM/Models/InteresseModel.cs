using System.ComponentModel;

namespace Furia_FanHub.MVVM.Models
{
    public class InteresseModel : INotifyPropertyChanged
    {
        private bool _isSelected;
        public string Name { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
