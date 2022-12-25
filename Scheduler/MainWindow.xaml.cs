using Scheduler.ViewModels;
using System;
using System.Windows;

namespace Scheduler
{
    public partial class MainWindow : Window
    {
        private CompositeRoot _compositeRoot;

        public MainWindow()
        {
            _compositeRoot = new CompositeRoot();

            InitializeComponent();
        }

        private void Load()
        {
            _compositeRoot.ErrorOccurred += OnClosed;
            _compositeRoot.ChangedTime += OnSetTime;
            _compositeRoot.ChangedStatusTimer += ChangeButtonPlayIcon;
            _compositeRoot.TimerIsOver += ShowMessage;

            _compositeRoot.LoadData();

            dgTaskList.ItemsSource = _compositeRoot.Tasks;
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
            _compositeRoot.ErrorOccurred -= OnClosed;
            _compositeRoot.ChangedTime -= OnSetTime;
            _compositeRoot.ChangedStatusTimer -= ChangeButtonPlayIcon;
            _compositeRoot.TimerIsOver -= ShowMessage;

            _compositeRoot.Close();

            Close();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e) => Load();

        private void WindowClosed(object sender, EventArgs e) => CloseApplication(); //дополнительное сохранение данных при закрытии

        private void ButtonPlayClick(object sender, RoutedEventArgs e) => _compositeRoot.RunTimer(dgTaskList.SelectedIndex);

        private void ButtonDeleteClick(object sender, RoutedEventArgs e) => _compositeRoot.RemoveData(dgTaskList.SelectedIndex);

        private void DgTodoListSelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e) { } // отображение цветом выбронной зоны

        private void OnSetTime(string time) => timerView.Text = time;

        private void ChangeButtonPlayIcon(bool value)
        {
            if (value == false)
                iconButtonPlay.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
            else
                iconButtonPlay.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
        }

        private void ShowMessage(string message) => MessageBox.Show(message);
    }
}
