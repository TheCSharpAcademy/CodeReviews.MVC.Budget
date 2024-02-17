const categoriesAPI = "https://localhost:7246/api/Categories";
const transactionsAPI = "https://localhost:7246/api/Transactions";
const menu = document.getElementById('menu-container');
const sidebar = document.getElementById("sidebar");
Chart.defaults.color = '#ffffff';

document.addEventListener("DOMContentLoaded", () => {
    var chart1 = document.getElementById('chart');
    var chart2 = document.getElementById('chart2');

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

    document.getElementById("sidebar-caret").addEventListener("click", () => {
        sidebar.classList.toggle('collapsed');
    });

    $(".accordion-head").on("click", function (event) {
        if (event.target.matches("img.add-icon.ms-auto")) {
            var id = $(this).closest('.accordion').data("id");
            $("#add-category-modal").modal('show');
            $("#add-category-modal").find("#GroupId").val(id);
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

    $('#add-category-form').on("submit", async function (event) {
        event.preventDefault();
        if ($(this).valid()) {
            $("#add-category-modal").modal('hide');
            await addCategory(new FormData(this));
        }
    });

    $('#add-transaction-form').on("submit", async function (event) {
        event.preventDefault();
        if ($(this).valid()) {
            $("#add-transaction-modal").modal('hide');
            await addTransaction(new FormData(this));
        }
    });

    $('.category').on("click", function (event) { 
        if (menu.dataset.category != 0) {
            var borderBox = document.getElementById(`category_${menu.dataset.category}`).querySelector('.border-animation');
            borderBox.classList.remove('border-rotate');
        }

        menu.dataset.category = this.dataset.id;           
        menu.style.left = `${this.style.left + event.pageX - 100}px`;
        menu.style.top = `${event.pageY - 100}px`;   
        menu.classList.add('active');     
        
        this.querySelector('.border-animation').classList.add('border-rotate');
    });

    document.getElementById('close-menu').onclick = function () {
        menu.classList.remove('active');
        var id = menu.dataset.category;
        var borderBox = document.getElementById(`category_${id}`).querySelector('.border-animation');
        borderBox.classList.remove('border-rotate');
        menu.dataset.category = 0;
    };

    document.getElementById('delete-menu').onclick = function () {
        var token = menu.querySelector('input').value;
        var id = menu.dataset.category;
        if (deleteCategory(id, token)) {
            menu.classList.remove('active');
            menu.dataset.category = 0;
        }
    };

    document.getElementById('add-menu').onclick = function () {
        var id = menu.dataset.category;        
        $("#add-transaction-modal").modal('show');
        $("#add-transaction-modal").find("#CategoryId").val(id);     
    };

    document.getElementById('edit-menu').onclick = function () {
        var token = menu.querySelector('input').value;
        var id = menu.dataset.category;        
    };

    document.getElementById('details-menu').onclick = function () {
        var id = menu.dataset.category;    
        window.location.href = "Category/" + id;
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

async function addCategory(data) {
    try {   
        var response = await fetch(`${categoriesAPI}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": data.get('__RequestVerificationToken')
            },
            body: JSON.stringify({
                Name: data.get("Name"),
                Budget: parseFloat(data.get("Budget")),
                GroupId: parseInt(data.get("GroupId"))
            })
        });
        
        if (response.ok) {
            document.querySelector(`#group_${data.get("GroupId")} .accordion-body`).innerHTML += createCategoryElement(await response.json());
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

async function deleteCategory(id, token) {
    try {
        var response = await fetch(`${categoriesAPI}/${id}`, {
            method: "DELETE",
            headers: {                
                "RequestVerificationToken": token
            }
        });

        if (response.ok) {
            document.getElementById(`category_${id}`).remove();
        } else {
            console.error(`HTTP Delete Error: ${response.status}`);
        }

    } catch (error) {
        console.error(error);
    }  
}

function createCategoryElement(category) {        
    return  `
    <div class="category border p-2">
        <div class="d-flex">
            <div>${category.name}</div>
            <div class="ms-auto">Balance: 700 / 700</div>
        </div>
        <div class="progress">
            <div class="progress-bar progress-bar-striped progress-bar-animated bg-success" role="progressbar" style="width: 100%" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100"></div>
        </div>
    </div>`;   
}