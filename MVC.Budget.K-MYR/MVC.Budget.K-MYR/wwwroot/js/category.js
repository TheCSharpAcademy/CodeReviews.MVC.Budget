const transactionsAPI = "https://localhost:7246/api/Transactions";
const menu = document.getElementById('menu-container');

Chart.defaults.color = '#ffffff';


document.addEventListener("DOMContentLoaded", () => {
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
            $("#add-transaction-modal").modal('show');
            $("#add-transaction-modal").find("#CategoryId").val(id);
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
            $("#add-transaction-modal").modal('hide');
            await addTransaction(new FormData(this));
        }
    });    

    $('.transaction').on("click", function (event) {
        if (menu.dataset.transaction != 0) {
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

    document.getElementById('add-menu').onclick = function () {
        var id = menu.dataset.category;
        $("#add-transaction-modal").modal('show');
        $("#add-transaction-modal").find("#CategoryId").val(id);
    };
});

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