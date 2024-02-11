﻿const uri = "https://localhost:7246/api/Categories";
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


    document.getElementById("sidebar-caret").addEventListener("click", () => {
        sidebar.classList.toggle('collapsed');
    });

    $(".accordion-head").on("click", function (event) {
        if (event.target.matches("img.add-icon.ms-auto")) {
            var id = $(this).closest('.accordion').data("id");
            $("#add-category-modal").modal('show');
            $("#add-category-modal #GroupId").val(id);
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

    /*
    $('.category').on("click", function (event) {
        var id = this.dataset.id;
        if (event.target.matches('img')) {
            var token = this.querySelector('input').value;
            deleteCategory(id, token);
        } else {
            window.location.href = "Category/" + id;
        }
    });
    */
    $('.category').on("click", function (event) {
        menu.style.left = `${this.style.left + event.pageX - 100}px`;
        menu.style.top = `${event.pageY - 100}px`;   
        menu.classList.add('active');     
        menu.dataset.category = this.dataset.id;
    });

    document.getElementById('close-menu').onclick = function () {
        menu.classList.remove('active');
    };

    document.getElementById('delete-menu').onclick = function () {
        var token = menu.querySelector('input').value;
        var id = menu.dataset.category;
        if (deleteCategory(id, token)) {
            menu.classList.remove('active');
        }
    };

    document.getElementById('add-menu').onclick = function () {
        var token = menu.querySelector('input').value;
        var id = menu.dataset.category;        
    };

    document.getElementById('edit-menu').onclick = function () {
        var token = menu.querySelector('input').value;
        var id = menu.dataset.category;        
    };

    document.getElementById('details-menu').onclick = function () {
        var token = menu.querySelector('input').value;
        var id = menu.dataset.category;      
    };

});

async function addCategory(data) {
    try {   
        var response = await fetch(`${uri}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": data.get('__RequestVerificationToken')
            },
            body: JSON.stringify({
                Name: data.get("Name"),
                Budget: data.get("Budget"),
                GroupId: data.get("GroupId")
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
        var response = await fetch(`${uri}/${id}`, {
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