using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using MvvmHelpers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MediaElement.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private const string LOCAL_VIDEO_FILENAME = "SampleVideo.mp4";

        MediaSource _source;
        Aspect _aspect;
        bool _autoPlay = true;
        bool _isLooping = true;
        bool _keepScreenOn = true;
        bool _showsPlaybackControls = true;

        public MainViewModel()
        {
            Aspect = Aspects[0];
            PlayPauseCommand = new AsyncCommand<Xamarin.Forms.MediaElement>(InvokePlayPauseCommandAsync);
            StopCommand = new AsyncCommand<Xamarin.Forms.MediaElement>(InvokeStopCommandAsync);
            PlayLocalVideoCommand = new AsyncCommand(InvokePlayLocalVideoCommandAsync);
            PlayRemoteVideoCommand = new AsyncCommand(InvokePlayRemoteVideoCommandAsync);
        }

        public ICommand PlayPauseCommand { get; }

        public ICommand StopCommand { get; }

        public ICommand PlayLocalVideoCommand { get; }

        public ICommand PlayRemoteVideoCommand { get; }

        public ObservableCollection<Aspect> Aspects { get; } = new ObservableCollection<Aspect> { Aspect.AspectFill, Aspect.AspectFit, Aspect.Fill };

        public MediaSource Source
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }

        public Aspect Aspect
        {
            get => _aspect;
            set => SetProperty(ref _aspect, value);
        }

        public bool AutoPlay
        {
            get => _autoPlay;
            set => SetProperty(ref _autoPlay, value);
        }

        public bool IsLooping
        {
            get => _isLooping;
            set => SetProperty(ref _isLooping, value);
        }

        public bool KeepScreenOn
        {
            get => _keepScreenOn;
            set => SetProperty(ref _keepScreenOn, value);
        }

        public bool ShowsPlaybackControls
        {
            get => _showsPlaybackControls;
            set => SetProperty(ref _showsPlaybackControls, value);
        }

        private Task InvokePlayPauseCommandAsync(Xamarin.Forms.MediaElement mediaElement)
        {
            if (mediaElement.CurrentState == MediaElementState.Playing)
            {
                mediaElement.Pause();
            }
            else
            {
                mediaElement.Play();
            }
            return Task.CompletedTask;
        }

        private Task InvokeStopCommandAsync(Xamarin.Forms.MediaElement mediaElement)
        {
            mediaElement.Stop();
            return Task.CompletedTask;
        }

        private Task InvokePlayRemoteVideoCommandAsync()
        {
            Source = MediaSource.FromUri(new Uri("https://sample-videos.com/video123/mp4/720/big_buck_bunny_720p_1mb.mp4"));
            return Task.CompletedTask;
        }

        private async Task InvokePlayLocalVideoCommandAsync()
        {
            var bundleFileName = LOCAL_VIDEO_FILENAME;
            await CopyVideoIfNotExistsAsync(bundleFileName);
            var localFileName = GetLocalFilePathByName(bundleFileName);
            Source = MediaSource.FromFile(localFileName);
        }

        private async Task CopyVideoIfNotExistsAsync(string filename)
        {
            var targetVideoFile = GetLocalFilePathByName(filename);

            if (!File.Exists(targetVideoFile))
            {
                using (var inputStream = await FileSystem.OpenAppPackageFileAsync(filename))
                {
                    using (var outputStream = File.Create(targetVideoFile))
                    {
                        await inputStream.CopyToAsync(outputStream);
                    }
                }
            }
        }

        private string GetLocalFilePathByName(string filename) => Path.Combine(FileSystem.AppDataDirectory, Path.GetFileName(filename));
    }
}
