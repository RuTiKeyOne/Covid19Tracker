﻿using Covid19Tracker.ViewModel.Base;
using Covid19TrackerLibrary.Model.Commands;
using Covid19TrackerLibrary.Model.Commands.Interfaces;
using Covid19TrackerLibrary.Model.Windows;
using GalaSoft.MvvmLight.Command;
using System;
using System.Windows;
using System.Windows.Input;

namespace Covid19Tracker.ViewModel
{
    public class TheLatestDataByCountryViewModel : BaseViewModel, ICloseCommand, IBackCommand
    {
        private DisplayRootRegistry DisplayRootRegistry;

        #region Close command
        public RelayCommand<Window> Close { get; set; }

        public void CloseWindow(Window window)
        {
            if (window != null)
                window.Close();
        }
        #endregion

        #region Back command
        public ICommand Back { get; set; }
        public bool CanBackExecute(object sender) => true;
        public void BackExecute(object sender)
        {
            DisplayRootRegistry.RegistryWindowType<MainViewModel, MainWindow>();
            DisplayRootRegistry.ShowPresentation(new MainViewModel());
            if (sender is Window)
                (sender as Window).Close();
        }
        #endregion

        public TheLatestDataByCountryViewModel()
        {
            DisplayRootRegistry = new DisplayRootRegistry();
            Close = new RelayCommand<Window>(CloseWindow);
            Back = new ActionCommand(BackExecute, CanBackExecute);
        }
    }
}
