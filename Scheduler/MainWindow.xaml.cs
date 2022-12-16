using Scheduler.ViewModels;
using System;
using System.Collections.Generic;
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

        private void ButtonPlayClick(object sender, RoutedEventArgs e) { }

        private void ButtonDeleteClick(object sender, RoutedEventArgs e) => _compositeRoot.RemoveData(dgTodoList.SelectedIndex);
    }
}
