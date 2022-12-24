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

        //private DispatcherTimer _timer;
        //private bool _isPlay = false;

        public MainWindow()
        {
            _compositeRoot = new CompositeRoot();
            //_timer = new DispatcherTimer();
            //_timer.Interval = new TimeSpan(0, 1, 05);

            InitializeComponent();
        }

        private void Load()
        {
            _compositeRoot.ErrorOccurred += OnClosed;
            _compositeRoot.ChangedTime += OnSetTime;

            _compositeRoot.LoadData();

            dgTodoList.ItemsSource = _compositeRoot.ToDoList;
        }

        private void OnClosed(bool value, string message)
        {
            if (value == false)
            {
                MessageBox.Show(message);
                CloseApplication();
            }
        }

        private void CloseApplication()
        {
            _compositeRoot.ErrorOccurred -= OnClosed;
            _compositeRoot.ChangedTime -= OnSetTime;

            _compositeRoot.Close();

            Close();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e) => Load();

        private void WindowClosed(object sender, EventArgs e) => CloseApplication();

        private void ButtonPlayClick(object sender, RoutedEventArgs e)
        {
            //Прописать логику запуска таймера
            //iconButtonPlay.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
            //WorkTimer();
        }

        private void ButtonDeleteClick(object sender, RoutedEventArgs e) => _compositeRoot.RemoveData(dgTodoList.SelectedIndex);

        private void DgTodoListSelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e) { }

        private void OnSetTime(string time) => timerView.Text = time;

        private async void WorkTimer(string time)
        {
            timerView.Text = time;

            //if (_isPlay == false)
            //{
            //    iconButtonPlay.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
            //    _isPlay = true;
            //}
            //else
            //{
            //    iconButtonPlay.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
            //    _isPlay = false;
            //}




            //while (_timer.Interval.TotalSeconds > 0 && _isPlay == true)
            //{
            //    await Task.Delay(1000);
            //    try
            //    {
            //        _timer.Interval -= TimeSpan.FromSeconds(1);
            //        timerView.Text = _timer.Interval.Hours + ":" + _timer.Interval.Minutes + ":" + _timer.Interval.Seconds;
            //    }
            //    catch (Exception exception)
            //    {
            //        MessageBox.Show(exception.Message);
            //        return;
            //    }
            //}

        }
    }
}
