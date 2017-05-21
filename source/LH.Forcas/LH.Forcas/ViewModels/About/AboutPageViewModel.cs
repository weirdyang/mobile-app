using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using LH.Forcas.Extensions;
using MvvmCross.Core.ViewModels;
using Newtonsoft.Json;

namespace LH.Forcas.ViewModels.About
{
    public class AboutPageViewModel : MvxViewModel
    {
        private Version appVersion;
        private string author;
        private IList<Dependency> dependencies;

        public AboutPageViewModel()
        {
            this.ActivityIndicatorState = new ActivityIndicatorState();
        }

        public ActivityIndicatorState ActivityIndicatorState { get; }

        public Version AppVersion
        {
            get => this.appVersion;
            set => this.SetProperty(ref this.appVersion, value);
        }

        public string Author
        {
            get => this.author;
            set => this.SetProperty(ref this.author, value);
        }

        public IList<Dependency> Dependencies
        {
            get => this.dependencies;
            set
            {
                if (this.SetProperty(ref this.dependencies, value))
                {
                    // ReSharper disable once ExplicitCallerInfoArgument
                    this.RaisePropertyChanged(nameof(this.DependenciesCount));
                }
            }
        }

        public int DependenciesCount => this.Dependencies?.Count ?? 0;

        public override void Appearing()
        {
            base.Appearing();
            #pragma warning disable 4014
            this.AppearingAsync();
            #pragma warning restore 4014
        }

        public async Task AppearingAsync()
        {
            await this.ActivityIndicatorState.RunWithIndicator(this.LoadAppInfo);
        }

        private void LoadAppInfo()
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;

            this.Author = assembly.GetCustomAttribute<AssemblyCompanyAttribute>().Company;
            this.AppVersion = assembly.GetName().Version;
            this.Dependencies = this.LoadDependencies();
        }

        private IList<Dependency> LoadDependencies()
        {
            var type = this.GetType();
            var assembly = type.GetTypeInfo().Assembly;
            var resourceName = type.GetSiblingResourceName("Dependencies.json");
            
            var json = assembly.GetManifestResourceContentAsText(resourceName);

            return JsonConvert.DeserializeObject<List<Dependency>>(json);
        }
    }
}