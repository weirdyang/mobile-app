using System;
using System.Collections.Generic;
using System.Linq;
using LH.Forcas.Extensions;
using LH.Forcas.Services;
using MvvmCross.Core.ViewModels;

namespace LH.Forcas.ViewModels.Settings
{
    public class SettingsPageViewModel : MvxViewModel
    {
        private readonly IRefDataService refDataService;
        private readonly IUserSettingsService userSettingsService;

        private Tuple<string, string> selectedCountry;
        private Tuple<string, string> selectedCurrency;
        private IList<Tuple<string, string>> countries;
        private IList<Tuple<string, string>> currencies;

        public SettingsPageViewModel(IRefDataService refDataService, IUserSettingsService userSettingsService)
        {
            this.ActivityIndicatorState = new ActivityIndicatorState();

            this.refDataService = refDataService;
            this.userSettingsService = userSettingsService;
        }

        public ActivityIndicatorState ActivityIndicatorState { get; }

        public IList<Tuple<string, string>> Countries
        {
            get { return this.countries; }
            private set { this.SetProperty(ref this.countries, value); }
        }

        public IList<Tuple<string, string>> Currencies
        {
            get { return this.currencies; }
            private set { this.SetProperty(ref this.currencies, value); }
        }

        public Tuple<string, string> SelectedCountry
        {
            get { return this.selectedCountry; }
            set
            {
                if (this.SetProperty(ref this.selectedCountry, value))
                {
                    this.userSettingsService.Settings.DefaultCountryId = value.Item1;
                    this.userSettingsService.Save();
                }
            }
        }

        public Tuple<string, string> SelectedCurrency
        {
            get { return this.selectedCurrency; }
            set
            {
                if (this.SetProperty(ref this.selectedCurrency, value))
                {
                    this.userSettingsService.Settings.DefaultCurrencyId = value.Item1;
                    this.userSettingsService.Save();
                }
            }
        }

        public override void Appearing()
        {
            base.Appearing();
            this.ActivityIndicatorState.RunWithIndicator(this.LoadData);
        }

        private void LoadData()
        {
            this.Currencies = this.refDataService.GetCurrencies()
                .Select(x => new Tuple<string, string>(x.CurrencyId, x.ToCurrencyDisplayName()))
                .ToList();


            this.Countries = this.refDataService.GetCountries()
                                    .Select(x => new Tuple<string, string>(x.CountryId, x.ToCountryDisplayName()))
                                    .ToList();

            this.SelectedCountry = this.Countries.Single(x => x.Item1 == this.userSettingsService.Settings.DefaultCountryId);
            this.SelectedCurrency = this.Currencies.Single(x => x.Item1 == this.userSettingsService.Settings.DefaultCurrencyId);
        }
    }
}