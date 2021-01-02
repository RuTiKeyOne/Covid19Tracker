﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Covid19TrackerLibrary.Model.Windows
{
    public class DisplayRootRegistry
    {
        Dictionary<Type, Type> ViewModelWindowsMapping;
        Dictionary<object, Window> OpenWindows;
        public DisplayRootRegistry()
        {
            ViewModelWindowsMapping = new Dictionary<Type, Type>();
            OpenWindows = new Dictionary<object, Window>();
        }

        public void RegistryWindowType<ViewModel, Win>() where ViewModel : class where Win : Window, new()
        {
            Type ViewModelType = typeof(ViewModel);
            if (ViewModelType.IsInterface)
                throw new Exception("Cannot registry interfaces");
            if (ViewModelWindowsMapping.ContainsKey(ViewModelType))
                throw new Exception($"Type is already registred");
            ViewModelWindowsMapping[ViewModelType] = typeof(Win);
        }

        public Window CreateWindowInstanceWithViewModel(object viewModel)
        {
            if (viewModel == null)
                throw new Exception("View model is null");
            Type WindowType = null;
            Type ViewModelType = viewModel.GetType();
            while(ViewModelType != null && !ViewModelWindowsMapping.TryGetValue(ViewModelType, out WindowType))
                ViewModelType = ViewModelType.BaseType;
            if (WindowType == null)
                throw new Exception($"No registred window type for argument type {viewModel.GetType().FullName}");
            Window Window = (Activator.CreateInstance(WindowType) as Window);
            Window.DataContext = viewModel;
            return Window;
        }

        public async Task ShowModalPresendation(object viewModel)
        {
            Window Window = CreateWindowInstanceWithViewModel(viewModel);
            await Window.Dispatcher.InvokeAsync(() => Window.ShowDialog());
        }

        public void ShowPresentation(object viewModel)
        {
            if (viewModel == null)
                throw new Exception("View model is null");
            if (OpenWindows.ContainsKey(viewModel))
                throw new Exception("Ui is already open");
            Window Window = CreateWindowInstanceWithViewModel(viewModel);
            Window.Show();
            OpenWindows[viewModel] = Window;
        }
    }
}
