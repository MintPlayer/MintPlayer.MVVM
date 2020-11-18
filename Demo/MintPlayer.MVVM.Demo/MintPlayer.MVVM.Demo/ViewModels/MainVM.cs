﻿using MintPlayer.MVVM.Demo.Events;
using MintPlayer.MVVM.Platforms.Common;
using MintPlayer.MVVM.Platforms.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MintPlayer.MVVM.Demo.ViewModels
{
    public class MainVM : BaseVM
    {
        private readonly IEventAggregator eventAggregator;
        public MainVM(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            eventAggregator.GetEvent<MenuItemSelectedEvent>().Subscribe(OnMenuItemSelected);
        }

        #region Bindings
        #region SidebarVisible
        private bool isSidebarVisible;
        public bool IsSidebarVisible
        {
            get => isSidebarVisible;
            set => SetProperty(ref isSidebarVisible, value);
        }
        #endregion
        #endregion

        #region Methods
        private void OnMenuItemSelected(object obj)
        {
            IsSidebarVisible = false;
        }

        protected override Task OnNavigatedTo(NavigationParameters parameters)
        {
            System.Diagnostics.Debug.WriteLine($"Navigated to {GetType().Name}");
            return Task.CompletedTask;
        }

        protected override Task OnNavigatedFrom()
        {
            System.Diagnostics.Debug.WriteLine($"Navigated from {GetType().Name}");
            return Task.CompletedTask;
        }

        protected override Task OnAppearing(bool pushed)
        {
            System.Diagnostics.Debug.WriteLine($"{GetType().Name} appearing ({pushed})");
            return Task.CompletedTask;
        }

        protected override Task OnDisappearing(bool popped)
        {
            System.Diagnostics.Debug.WriteLine($"{GetType().Name} disappearing ({popped})");
            return Task.CompletedTask;
        }
        #endregion
    }
}
