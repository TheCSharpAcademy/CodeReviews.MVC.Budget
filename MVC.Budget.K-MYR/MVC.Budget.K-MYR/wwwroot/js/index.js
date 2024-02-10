const uri = "https://localhost:7246/api/Categories";
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


    document.getElementById("sidebar-caret").addEventListener("click", () => {
        var sidebar = document.getElementById("sidebar");
        if (sidebar.classList.contains("collapsed")) {
            sidebar.classList.remove("collapsed");
        }
        else {
            sidebar.classList.add("collapsed")
        }
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

    $('.category').on("click", function (event) {
        var id = this.dataset.id;
        if (event.target.matches('img')) {
            var token = this.querySelector('input').value;
            deleteCategory(id, token);
        } else {
            window.location.href = "Category/" + id;
        }
    });
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
        } else {
            console.error(`HTTP Post Error: ${response.status}`);
        }

    } catch (error) {
        console.error(error);
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
            //document.getElementById(`category_${id}`).remove();
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