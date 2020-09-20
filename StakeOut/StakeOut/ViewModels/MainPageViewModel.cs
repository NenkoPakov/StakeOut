using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;

namespace StakeOut
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public MainPageViewModel()
        {
            Points = new ObservableCollection<Point>();

            SavePointCommand = new Command(() =>
            {
                Points.Add(new Point { CoordinateX = this.CoordinateX, CoordinateY = this.CoordinateY, Number = this.Number });
                CoordinateX = string.Empty;
                CoordinateY = string.Empty;
                Number = string.Empty;
            },
            () => !string.IsNullOrEmpty(CoordinateX) && !string.IsNullOrEmpty(CoordinateY) && !string.IsNullOrEmpty(Number));

            ErasePointsCommand = new Command(() => Points.Clear());


            GoToMapCommand = new Command(async () =>
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new MapPage());
                });


            PointSelectedCommand = new Command(async () =>
            {
                if (SelectedPoint is null)
                    return;

                var detailViewModel = new StakeOutViewModel(SelectedPoint);

                await Application.Current.MainPage.Navigation.PushAsync(new StakeOutPage(detailViewModel));

                SelectedPoint = null;
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        string coordinateX;
        public string CoordinateX
        {
            get => coordinateX;
            set
            {
                coordinateX = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CoordinateX)));

                SavePointCommand.ChangeCanExecute();
            }
        }

        string number;
        public string Number
        {
            get => number;
            set
            {
                number = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Number)));

                SavePointCommand.ChangeCanExecute();
            }
        }

        string coordinateY;
        public string CoordinateY
        {
            get => coordinateY;
            set
            {
                coordinateY = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CoordinateY)));

                SavePointCommand.ChangeCanExecute();
            }
        }




        Point selectedPoint;
        public Point SelectedPoint
        {
            get => selectedPoint;
            set
            {
                selectedPoint = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedPoint)));
            }
        }

        public ObservableCollection<Point> Points { get; }

        public Command PointSelectedCommand { get; }
        public Command SavePointCommand { get; }
        public Command GoToMapCommand { get; }
        public Command ErasePointsCommand { get; }
    }
}
