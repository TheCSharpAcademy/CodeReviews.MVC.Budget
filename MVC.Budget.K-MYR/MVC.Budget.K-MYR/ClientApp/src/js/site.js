import { getCountrySelect } from "./asyncComponents";
import { getCountryCookie } from "./api";

const countrySelectPromise = initializeCountrySelect();

async function initializeCountrySelect() {
    let countrySelect = await getCountrySelect("#country");
    countrySelect.countrySelect("selectCountry", window.userLocale.region.toLowerCase());
    countrySelect.on('change', async function () {
        let iso2Code = countrySelect.countrySelect("getSelectedCountryData").iso2;
        let form = document.getElementById("country-form");
        let token = form.querySelector('input[name="__RequestVerificationToken"]').value

        let preferences = await getCountryCookie(iso2Code, token);

        if (preferences) {
            window.userLocale = new Intl.Locale(preferences.locale);
            window.userNumberFormat = new Intl.NumberFormat(preferences.locale, { style: 'currency', currency: preferences.currency });
        }        
    });
}