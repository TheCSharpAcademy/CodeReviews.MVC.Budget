import { Modal } from 'bootstrap';
import 'country-select-js';
import 'jquery-validation';

const addFiscalPlanModal = new Modal(document.getElementById("add-fiscalPlan-modal"));


$("#country").countrySelect({
    defaultCountry: window.userLocale.region.toLowerCase(),
    preferredCountries: ["at", "us"],
    responsiveDropdown: true
});
document.getElementById("country-form").addEventListener('submit', function (event) {
    event.preventDefault();
});

document.getElementById('add-fiscalPlan-card').addEventListener('click', function (event) {
    addFiscalPlanModal.show();
});
