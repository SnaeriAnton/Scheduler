using Scheduler.ViewModels;
using System;
using System.Windows;

namespace Scheduler
{
    public partial class MainWindow : Window
    {
        private CompositeRoot _compositeRoot;
        private ViewTimer _viewTimer;

        public MainWindow()
        {
            _compositeRoot = new CompositeRoot();

            InitializeComponent();
        }

        private void Load()
        {
            buttonDelete.IsEnabled = false;
            buttonStartTimer.IsEnabled = false;
            _compositeRoot.ErrorOccurred += OnClosed;
            _compositeRoot.ChangedTime += OnSetTime;
            _compositeRoot.ChangedStatusTimer += OnChangeButtonPlayIcon;
            _compositeRoot.TimerIsOver += OnShowMessage;
            _compositeRoot.ReminderIsOver += OnReminderIsOver;
            _compositeRoot.ChangedStatus += OnChangedStatus;

            _compositeRoot.LoadData();

            dgTaskList.ItemsSource = _compositeRoot.Tasks;

            OpenWindowViewTimer();
        }

        private void OnClosed(bool value, string message)
        {
            if (value == true)
                return;

            MessageBox.Show(message);
            CloseApplication();
        }

        private void CloseApplication()
        {
            if (_viewTimer != null)
                _viewTimer.Close();

            _compositeRoot.ErrorOccurred -= OnClosed;
            _compositeRoot.ChangedTime -= OnSetTime;
            _compositeRoot.ChangedStatusTimer -= OnChangeButtonPlayIcon;
            _compositeRoot.TimerIsOver -= OnShowMessage;
            _compositeRoot.ReminderIsOver -= OnReminderIsOver;
            _compositeRoot.ChangedStatus -= OnChangedStatus;

            _compositeRoot.Close();
            Close();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e) => Load();

        private void ClosedWindow(object sender, EventArgs e) => CloseApplication();

        private void ButtonPlayClick(object sender, RoutedEventArgs e)
        {
            _compositeRoot.RunTimer(dgTaskList.SelectedIndex);

            if (_viewTimer != null)
                _viewTimer.ShowTask(_compositeRoot.GetTaskText());
        }

        private void ButtonDeleteClick(object sender, RoutedEventArgs e)
        {
            _compositeRoot.RemoveData(dgTaskList.SelectedIndex);
            buttonDelete.IsEnabled = false;
        }

        private void OnSetTime(string time)
        {
            if (_viewTimer != null)
                _viewTimer.ShowTime(time);

            timerView.Text = time;
        }

        private void OnChangeButtonPlayIcon(bool value)
        {
            if (value == false)
                iconButtonPlay.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
            else
                iconButtonPlay.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
        }

        private void OnShowMessage(string message) => MessageBox.Show(message);

        private void OnReminderIsOver(string message)
        {
            Application.Current.MainWindow.Topmost = true;
            MessageBoxResult result = MessageBox.Show(message);

            if (result == MessageBoxResult.OK)
            {
                Application.Current.MainWindow.Topmost = false;
                _compositeRoot.RestartRemonder();
            }
        }

        private void ButtonShowViewTimerWindowClick(object sender, RoutedEventArgs e) => OpenWindowViewTimer();

        private void OpenWindowViewTimer()
        {
            if (_viewTimer != null)
                return;

            _viewTimer = new ViewTimer();
            buttonViewTimerWindow.IsEnabled = false;
            _viewTimer.ShowTask(_compositeRoot.GetTaskText());
            _viewTimer.Closing += OnClosedViewTimerWindow;
            _viewTimer.Show();
        }

        private void OnClosedViewTimerWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _viewTimer.Closing -= OnClosedViewTimerWindow;
            _viewTimer = null;
            buttonViewTimerWindow.IsEnabled = true;
        }

        private void SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            buttonDelete.IsEnabled = true;
            if (_compositeRoot.CheсkStatusTask(dgTaskList.SelectedIndex) == true)
                buttonStartTimer.IsEnabled = true;
            else
                buttonStartTimer.IsEnabled = false;
        }

        private void OnChangedStatus(bool value)
        {
            if (value == true)
                buttonStartTimer.IsEnabled = false;
            else
                buttonStartTimer.IsEnabled = true;
        }
    }
}
