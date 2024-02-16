const transactionsAPI = "https://localhost:7246/api/Transactions";

Chart.defaults.color = '#ffffff';


document.addEventListener("DOMContentLoaded", () => {
    const chart1 = document.getElementById('chart');
    const chart2 = document.getElementById('chart2');

    new Chart(chart1, {
        type: 'doughnut',
        data: {
            labels: [
                'Happy',
                'Unhappy',
                'No Data'
            ],
            datasets: [{
                label: 'My First Dataset',
                data: [300, 50, 100],
                backgroundColor: [
                    'rgb(255, 99, 132)',
                    'rgb(54, 162, 235)',
                    'rgb(255, 205, 86)'
                ],
                hoverOffset: 4
            }]
        }
    });

    new Chart(chart2, {
        type: 'doughnut',
        data: {
            labels: [
                'Necessary',
                'Unecessary',
                'No Data'
            ],
            datasets: [{
                label: 'My First Dataset',
                data: [300, 50, 100],
                backgroundColor: [
                    'rgb(255, 99, 132)',
                    'rgb(54, 162, 235)',
                    'rgb(255, 205, 86)'
                ],
                hoverOffset: 4
            }]
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
        if (event.target.matches('img')) {
            var token = this.querySelector('input').value;
            deleteTransaction(this.dataset.id, token);
        } 
    });
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