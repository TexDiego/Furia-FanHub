using Furia_FanHub.MVVM.Helpers;

namespace Furia_FanHub.MVVM.Models
{
    public class CPFEntryValidationBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnTextChanged;
        }

        void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Entry entry)
            {
                string value = new string(entry.Text?.Where(char.IsDigit).ToArray() ?? []);

                if (string.IsNullOrEmpty(value) || value.Length < 11)
                {
                    entry.TextColor = Colors.LightCoral;
                    return;
                }

                var validator = new CPFValidationRule();
                entry.TextColor = validator.Check(value) ? Colors.Green : Colors.LightCoral;
            }
        }
    }
}
