const transactionsAPI = "https://localhost:7246/api/Transactions";
const menu = document.getElementById('menu-container');
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


Chart.defaults.color = '#ffffff';



$("#country").countrySelect();

const chart1 = document.getElementById('chart');
const chart2 = document.getElementById('chart2');

new Chart(chart1, {
    type: 'doughnut',
    data: {
        labels: [
            'Happy',
            'Unhappy'
        ],
        datasets: [{
            label: 'Total Amount',
            data: [chart1.dataset.happy, chart1.dataset.unhappy],
            backgroundColor: [
                'rgb(25,135,84)',
                'rgb(220,53,69)'
            ],
            hoverOffset: 4
        }]
    },
    options: {
        responsive: true,
        maintainAspectRatio: false
    }
});

new Chart(chart2, {
    type: 'doughnut',
    data: {
        labels: [
            'Necessary',
            'Unnecessary'
        ],
        datasets: [{
            label: 'Total Amount',
            data: [chart2.dataset.necessary, chart2.dataset.unnecessary],
            backgroundColor: [
                'rgb(25,135,84)',
                'rgb(220,53,69)'
            ],
            hoverOffset: 4
        }]
    },
    options: {
        responsive: true,
        maintainAspectRatio: false
    }
});

$(".accordion-head").on("click", function (event) {
    if (event.target.matches("img.add-icon.ms-auto")) {
        var id = $(this).closest('.accordion').data("id");
        addTransactionModal.modal('show');
        addTransactionModal.find("#CategoryId").val(id);
    }
    else {
        $(this).next().collapse('toggle');
        var caret = $('.accordion-caret', this)[0];
        if (caret.classList.contains("rotate")) {
            caret.classList.remove("rotate");
        } else {
            caret.classList.add("rotate");
        }
    }
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

$('.transaction').on("click", function (event) {
    if (menu.dataset.transaction != 0) {
        console.log(`transaction_${menu.dataset.transaction}`);
        var borderBox = document.getElementById(`transaction_${menu.dataset.transaction}`).querySelector('.border-animation');
        borderBox.classList.remove('border-rotate');
    }

    menu.dataset.transaction = this.dataset.id;
    menu.style.left = `${this.style.left + event.pageX - 100}px`;
    menu.style.top = `${event.pageY - 100}px`;
    menu.classList.add('active');

    this.querySelector('.border-animation').classList.add('border-rotate');
});

document.getElementById('close-menu').onclick = function () {
    menu.classList.remove('active');
    var id = menu.dataset.transaction;
    var borderBox = document.getElementById(`transaction_${id}`).querySelector('.border-animation');
    borderBox.classList.remove('border-rotate');
    menu.dataset.transaction = 0;
};

document.getElementById('delete-menu').onclick = function () {
    var token = menu.querySelector('input').value;
    var id = menu.dataset.transaction;
    if (deleteTransaction(id, token)) {
        menu.classList.remove('active');
        menu.dataset.transaction = 0;
    }
};

document.getElementById('edit-menu').onclick = function () {
    var transaction = document.getElementById(`transaction_${menu.dataset.transaction}`);

    transactionModalLabel.text(`Edit ${transaction.dataset.title}`);
    transactionModalId.val(transaction.dataset.id);
    transactionModalCategoryId.val(transaction.dataset.categoryid);
    transactionModalTitle.val(transaction.dataset.title);
    transactionModalAmount.val(transaction.dataset.amount);
    transactionModalDateTime.val(transaction.dataset.date);
    transactionModalEvaluated.val(transaction.dataset.evaluated);
    transactionModalEvaluatedIsHappy.val(transaction.dataset.evaluatedishappy);
    transactionModalEvaluatedIsNecessary.val(transaction.dataset.evaluatedisnecessary);
    updateTransactionModal.find(`#updateTransaction_isHappy${transaction.dataset.ishappy}`).prop("checked", true);
    updateTransactionModal.find(`#updateTransaction_isNecessary${transaction.dataset.isnecessary}`).prop("checked", true);

    updateTransactionModal.modal('show');
};


async function addTransaction(data) {
    try {
        var response = await fetch(`${transactionsAPI}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": data.get('__RequestVerificationToken')
            },
            body: JSON.stringify({
                Title: data.get("Title"),
                DateTime: data.get("DateTime"),
                Amount: parseFloat(data.get("Amount")),
                IsHappy: data.get("IsHappy") === "true" ? true : false,
                IsNecessary: data.get("IsNecessary") === "true" ? true : false,
                CategoryId: parseInt(data.get("CategoryId"))
            })
        });

        if (response.ok) {
            //document.querySelector(`#transactions .accordion-body`).innerHTML += createCategoryElement(await response.json());
        } else {
            console.error(`HTTP Post Error: ${response.status}`);
        }

    } catch (error) {
        console.error(error);
    };
}

async function updateTransaction(data) {
    try {
        var id = parseInt(data.get("Id"));
        var response = await fetch(`${transactionsAPI}/${id}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": data.get('__RequestVerificationToken')
            },
            body: JSON.stringify({
                Id: id,
                Title: data.get("Title"),
                DateTime: data.get("DateTime"),
                Amount: parseFloat(data.get("Amount")),
                IsHappy: data.get("IsHappy") === "true" ? true : false,
                IsNecessary: data.get("IsNecessary") === "true" ? true : false,
                CategoryId: parseInt(data.get("CategoryId")),
                Evaluated: data.get("Evaluated") === "true" ? true : false,
                EvaluatedIsHappy: data.get("EvaluatedIsHappy") === "true" ? true : false,
                evaluatedIsNecessary: data.get("EvaluatedIsNecessary") === "true" ? true : false,
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

async function deleteTransaction(id, token) {
    try {
        var response = await fetch(`${transactionsAPI}/${id}`, {
            method: "DELETE",
            headers: {
                "RequestVerificationToken": token
            }
        });

        if (response.ok) {
            document.getElementById(`transaction_${id}`).remove();
        } else {
            console.error(`HTTP Delete Error: ${response.status}`);
        }

    } catch (error) {
        console.error(error);
    };
}