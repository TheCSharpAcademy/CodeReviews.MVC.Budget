import { getCountrySelect } from "./asyncComponents";
import { getCountryCookie } from "./api";

const countryChangedEvent = new CustomEvent('countryChanged');
const countrySelectPromise = initializeCountrySelect();
document.getElementById("country-form").onsubmit = (event) => {
    event.preventDefault();
    event.stopPropagation();
} 

async function initializeCountrySelect() {
    var countrySelect = await getCountrySelect("#country");  
    countrySelect.countrySelect("selectCountry", window.userLocale.region.toLowerCase());
    var form = document.getElementById("country-form");
    form.style = "";
    countrySelect.on('change', async function () {
        let iso2Code = countrySelect.countrySelect("getSelectedCountryData").iso2;
        let token = form.querySelector('input[name="__RequestVerificationToken"]').value

        let preferences = await getCountryCookie(iso2Code, token);

        if (preferences) {
            window.userLocale = new Intl.Locale(preferences.locale);
            window.userNumberFormat = new Intl.NumberFormat(preferences.locale, { style: 'currency', currency: preferences.currency });
            window.dispatchEvent(countryChangedEvent);
        }        
    });
}
