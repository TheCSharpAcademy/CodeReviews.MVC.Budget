import { importBootstrapModals } from './asyncComponents';
import { addFiscalPlan } from './api'

formatDashboard();
const fiscalPlanApi = "https://localhost:7246/api/FiscalPlan";
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

window.addEventListener('countryChanged', () => {
    formatDashboard();
})

function formatDashboard() {
    var cards = $('.fiscalPlan-card');
    for (let i = 0; i < cards.length; i++) {
        let id = cards[i].dataset.id;
        let incomeText = document.getElementById(`fiscalPlan_${id}_income`);
        incomeText.textContent = `${window.userNumberFormat.format(incomeText.dataset.total)} / ${window.userNumberFormat.format(incomeText.dataset.budget)}`;
        let expensesText = document.getElementById(`fiscalPlan_${id}_expenses`);
        expensesText.textContent = `${window.userNumberFormat.format(expensesText.dataset.total)} / ${window.userNumberFormat.format(expensesText.dataset.budget)}`;
    }
}
