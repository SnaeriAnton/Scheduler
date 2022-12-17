using Scheduler.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Scheduler
{
    public partial class MainWindow : Window
    {
        private CompositeRoot _compositeRoot;
        private DispatcherTimer _timer;

        public MainWindow()
        {
            _compositeRoot = new CompositeRoot();

            InitializeComponent();
        }

        private void Load()
        {
            _compositeRoot.LoadedData += OnClosed;
            _compositeRoot.SavedDat += OnClosed;

            _compositeRoot.LoadData();

            dgTodoList.ItemsSource = _compositeRoot.ToDoList;
        }

        private void OnClosed(bool value)
        {
            if (value == false)
                CloseApplication();
        }

        private void CloseApplication()
        {
            _compositeRoot.LoadedData -= OnClosed;
            _compositeRoot.SavedDat -= OnClosed;

            _compositeRoot.Close();

            Close();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e) => Load();

        private void WindowClosed(object sender, EventArgs e) => CloseApplication();

        private void ButtonPlayClick(object sender, RoutedEventArgs e) 
        {
            


            WorkTimer();
        }

        private void ButtonDeleteClick(object sender, RoutedEventArgs e) => _compositeRoot.RemoveData(dgTodoList.SelectedIndex);

        private void DgTodoListSelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {

        }

        private async void WorkTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 1, 05);


            while (_timer.Interval.TotalSeconds > 0)
            {
                await Task.Delay(1000);
                try
                {
                    _timer.Interval -= TimeSpan.FromSeconds(1);
                    timerView.Text = _timer.Interval.Hours + ":" + _timer.Interval.Minutes + ":" + _timer.Interval.Seconds;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                    return;
                }
            }

        }
    }
}
