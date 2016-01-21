﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Metronome.Annotations;
using Metronome.Commands;
using Metronome.Pages;
using Metronome.Pictures;

namespace Metronome.Windows
{
    public class MainWindowViewModel : DependencyObject, INotifyPropertyChanged
    {
        private readonly bool _initialized;
        public MainWindowViewModel()
        {
            Controller = Controller.Instance;

            TickSoundFiles = Controller.Model.TickSoundFiles;
            SelectedTickSoundFile = Controller.Model.SelectedTickSoundFile;
            DelayMseconds = Controller.Model.DelayMseconds;
            Volume = Controller.Model.Volume;
            PageUri = PagesHelper.GetAboutPageUri();

            StartMetronomeButtonImageUri = PicturesHelper.GetStart();

            _initialized = true;
        }

        internal Controller Controller { get; }

        #region Volume

        public static readonly DependencyProperty VolumeProperty = DependencyProperty.Register(
            "Volume", typeof(double), typeof(MainWindowViewModel),
            new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnChangeVolume));

        private static void OnChangeVolume(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = ((MainWindowViewModel)d);
            viewModel.Controller.ChangeVolume((float)(double)e.NewValue);
        }

        public double Volume
        {
            get { return (double)GetValue(VolumeProperty); }
            set
            {
                SetValue(VolumeProperty, value);
                OnPropertyChanged();
            }
        }

        #endregion

        #region Delay Mseconds

        public static readonly DependencyProperty DelayMsecondsProperty = DependencyProperty.Register(
            "DelayMseconds", typeof (double), typeof (MainWindowViewModel),
            new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnChangeDelayMseconds));

        private static void OnChangeDelayMseconds(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = ((MainWindowViewModel)d);
            viewModel.Controller.ChangeDelay((int)(double)e.NewValue);
        }

        public double DelayMseconds
        {
            get { return (double) GetValue(DelayMsecondsProperty); }
            set
            {
                SetValue(DelayMsecondsProperty, value);
                OnPropertyChanged();
            }
        }

        #endregion

        #region Tick Sound Files

        public static readonly DependencyProperty TickSoundFilesProperty = DependencyProperty.Register(
            "TickSoundFiles", typeof (IEnumerable<string>), typeof (MainWindowViewModel), new PropertyMetadata(default(IEnumerable<string>)));

        public IEnumerable<string> TickSoundFiles
        {
            get { return (IEnumerable<string>) GetValue(TickSoundFilesProperty); }
            set
            {
                SetValue(TickSoundFilesProperty, value);
                OnPropertyChanged();
            }
        }

        #endregion

        #region Selected Tick Sound File

        public static readonly DependencyProperty SelectedTickSoundFileProperty = DependencyProperty.Register(
            "SelectedTickSoundFile", typeof (string), typeof (MainWindowViewModel), 
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectTickSound));

        private static void OnSelectTickSound(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = ((MainWindowViewModel)d);
            if (viewModel._initialized)
            {
                viewModel.Controller.ChangeSelectedTickSoundFile((string)e.NewValue);
                if (viewModel.CheckSettingsChangeCommand.CanExecute(viewModel))
                {
                    viewModel.CheckSettingsChangeCommand.Execute(viewModel);
                }
            }
        }

        public string SelectedTickSoundFile
        {
            get { return (string) GetValue(SelectedTickSoundFileProperty); }
            set
            {
                SetValue(SelectedTickSoundFileProperty, value);
                OnPropertyChanged();
            }
        }

        #endregion

        #region Start Metronome Button Enabled

        public static readonly DependencyProperty StartMetronomeButtonEnabledProperty = DependencyProperty.Register(
            "StartMetronomeButtonEnabled", typeof(bool), typeof(MainWindowViewModel), new PropertyMetadata(true));

        public bool StartMetronomeButtonEnabled
        {
            get { return (bool)GetValue(StartMetronomeButtonEnabledProperty); }
            set
            {
                SetValue(StartMetronomeButtonEnabledProperty, value);
                OnPropertyChanged();
            }
        }

        #endregion

        #region Start Metronome Button Text

        public static readonly DependencyProperty StartMetronomeButtonImageUriProperty = DependencyProperty.Register(
            "StartMetronomeButtonStartMetronomeButtonImageUri", typeof (string), typeof (MainWindowViewModel));

        public string StartMetronomeButtonImageUri
        {
            get { return (string) GetValue(StartMetronomeButtonImageUriProperty); }
            set
            {
                SetValue(StartMetronomeButtonImageUriProperty, value);
                OnPropertyChanged();
            }
        }

        #endregion

        #region Page Navigation

        public static readonly DependencyProperty PageUriProperty = DependencyProperty.Register(
            "PageUri", typeof(string), typeof(MainWindowViewModel));

        public string PageUri
        {
            get { return (string)GetValue(PageUriProperty); }
            set
            {
                SetValue(PageUriProperty, value);
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand CheckSettingsChangeCommand { get; } = new CheckSettingsChangeActionCommand();
        public IStoppableInfiniteCommand ExecuteMetronomeAsyncCommand { get; } = new ExecuteMetronomeAsyncCommand();
        public ICommand CloseApplicationCommand { get; } = new CloseApplicationCommand();
        public ICommand NavigateToAboutPageCommand { get; } = new ViewModelActionCommand<MainWindowViewModel>(
            vm => vm.PageUri = PagesHelper.GetAboutPageUri(),
            vm => true);
        public ICommand NavigateToAudioDevicePageCommand { get; } = new ViewModelActionCommand<MainWindowViewModel>(
            vm => vm.PageUri = PagesHelper.GetAudioDevicePageUri(),
            vm => true);

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
