using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace StakeOut
{
    public class DetailPageViewModel : INotifyPropertyChanged
    {
        public DetailPageViewModel()
        {
            noteText = "HERE";
            ExitCommand = new Command(async () => await Application.Current.MainPage.Navigation.PopAsync());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        string noteText;
        public string NoteText
        {
            get => noteText;
            set
            {
                noteText = "HERE";
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NoteText)));
            }
        }

        public ICommand ExitCommand { get; }
    }
}
