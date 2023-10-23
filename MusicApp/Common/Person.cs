using System;
using System.ComponentModel;

namespace Common
{
    public class Person : INotifyPropertyChanged
    {
        private string _firstName;

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("First name must not be blank");
                if (value != _firstName)
                {
                    PropertyChanged?.Invoke(this,
                        new PropertyChangedEventArgs(nameof(FirstName)));

                    // Old:
                    //if (PropertyChanged != null)
                    //    PropertyChanged(this, new PropertyChangedEventArgs(nameof(FirstName)));
                }
                _firstName = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        // remaining implementation removed from listing
    }
}
