import { importChartDefaults,  importBootstrapModals} from './asyncComponents';

const fiscalPlanApi = "https://localhost:7246/api/FiscalPlan";

const chartDefaultsTask = importChartDefaults();
const modals = importBootstrapModals().then((modalsArray) => {
    let modal = modalsArray.find(m => m._element.id == "addFiscalPlan-modal");
    document.getElementById("addFiscalPlan-card").addEventListener('click', function () {
        modal.show();
    });
    document.getElementById("addFiscalPlan-form").addEventListener('submit', async function (event) {
        event.preventDefault();
        if ($(this).valid()) {
            modal.hide();
            await addFiscalPlan(new FormData(this));
        }
    })
 });

$('.fiscalPlan-card').on('click', function (event) {
    window.location.href = `https://localhost:7246/FiscalPlan/${this.dataset.id}`;
});

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
