import { getCountrySelect, importChartDefaults,  importBootstrapModals} from './asyncComponents';
import 'jquery-validation';

const chartDefaultsTask = importChartDefaults();
const modals = importBootstrapModals();
const countrySelect = getCountrySelect("#country");


const addFiscalPlanModal = async () => {
    await modals;
    return Modal.GetInstance(document.getElementById("add-fiscalPlan-modal"));
    };
const fiscalPlanApi = "https://localhost:7246/api/FiscalPlan";


document.getElementById("country-form").addEventListener('submit', function (event) {
    event.preventDefault();
});

document.getElementById("add-fiscalPlan-card").addEventListener('click', function (event) {
    addFiscalPlanModal.show();
});

$('.fiscalPlan-card').on('click', function (event) {
    console.log(this);
    window.location.href = `https://localhost:7246/FiscalPlan/${this.dataset.id}`;
});

document.getElementById("add-fiscalPlan-form").addEventListener('submit', async function (event) {
    event.preventDefault();
    if ($(this).valid()) {
        addFiscalPlanModal.hide();
        await addFiscalPlan(new FormData(this));
    }
})

async function addFiscalPlan(data) {
    try {
        var response = await fetch(`${fiscalPlanApi}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": data.get('__RequestVerificationToken')
            },
            body: JSON.stringify({
                Name: data.get("Name"),
            })
        });

        if (response.ok) {
            return true;
        } else {
            console.error(`HTTP Post Error: ${response.status}`);
            return false;
        }

    } catch (error) {
        console.error(error);
        return false;
    }
}
