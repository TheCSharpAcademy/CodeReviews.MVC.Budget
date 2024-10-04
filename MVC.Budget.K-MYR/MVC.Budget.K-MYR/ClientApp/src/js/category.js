import { importChartDefaults, importBootstrapCollapses} from './asyncComponents';
import { ArcElement, Chart, DoughnutController } from 'chart.js';
Chart.register(DoughnutController, ArcElement);

const chartDefaultsTask = importChartDefaults();

const currentDate = new Date();
const categoryId = document.getElementById('category_Id');

const categoryDashboard = await getCategoryDashboard(categoryId.value, currentDate, JSON.parse(categoryId.dataset.object));

const transactionsAPI = "https://localhost:7246/api/Transactions";
const addTransactionModal = $("#add-transaction-modal");
const updateTransactionModal = $("#updateTransaction-modal");
const transactionModalLabel = updateTransactionModal.find("#updateTransaction-label");
const transactionModalId = updateTransactionModal.find("#updateTransaction_id");
const transactionModalTitle = updateTransactionModal.find("#updateTransaction_title");
const transactionModalAmount = updateTransactionModal.find("#updateTransaction_amount");
const transactionModalCategoryId = updateTransactionModal.find("#updateTransaction_categoryId");
const transactionModalDateTime = updateTransactionModal.find("#updateTransaction_dateTime");
const transactionModalEvaluated = updateTransactionModal.find("#updateTransaction_evaluated");
const transactionModalEvaluatedIsHappy = updateTransactionModal.find("#updateTransaction_evaluatedIsHappy");
const transactionModalEvaluatedIsNecessary = updateTransactionModal.find("#updateTransaction_evaluatedIsNecessary");

const collapses = await importBootstrapCollapses();

const collapsesPromise = importBootstrapCollapses()
    .then(() => {
        $('.accordion-head').on('click', function (event) {
            if (!event.target.classList.contains('add-category-icon')) {
                let collapse = $(this).next();
                if (!collapse[0].classList.contains('collapsing')) {
                    collapse.collapse('toggle');
                    let caret = $('.accordion-caret', this)[0];
                    caret.classList.toggle('rotate');
                }               
            }
        });
    });

$('#add-transaction-form').on("submit", async function (event) {
    event.preventDefault();
    if ($(this).valid()) {
        addTransactionModal.modal('hide');
        const start = performance.now();
        await addTransaction(new FormData(this));
        const end = performance.now();
        console.log(`Execution time: ${end - start} ms`);
    }
});

$('#update-transaction-form').on("submit", async function (event) {
    event.preventDefault();
    if ($(this).valid()) {
        updateTransactionModal.modal('hide');
        await updateTransaction(new FormData(this));
    }
});

async function getCategoryDashboard(id, date, data) {
    try {
        const { default: CategoryDashboard } = await import(/* webpackChunkName: "categoryDashboard"*/ './categoryDashboard');
        await chartDefaultsTask;

        return new CategoryDashboard(id, date, data);

    } catch (error) {
        console.error('Error loading category dashboard:', error);
        throw error;
    }
} 
