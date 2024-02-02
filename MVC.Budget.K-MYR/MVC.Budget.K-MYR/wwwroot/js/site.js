const uri = "https://localhost:7246/api/Categories";

categoriesContainer = document.getElementById("categories-container");

document.addEventListener("DOMContentLoaded", () => {
    document.getElementById("sidebar-caret").addEventListener("click", () => {
        const sidebar = document.getElementById("sidebar");
        if (sidebar.classList.contains("collapsed")) {
            sidebar.classList.remove("collapsed");
        }
        else {
            sidebar.classList.add("collapsed")
        }
    });

    document.getElementById("accordionIncome").addEventListener("click", function(event) {
        if (event.target.matches("img.add-icon.ms-2")) {
            $("#add-category-modal").modal('show');
            $("#add-category-modal #IncomeId").val(this.dataset.id);                    
        }
    });

    $('#add-category-form').on("submit", async function (event) {
        event.preventDefault();
        if ($(this).valid()) {
            $("#add-category-modal").modal('hide');
            addCategory(new FormData(this));            
        }
    });
})

async function addCategory(data) {
    try {
        const response = await fetch(`${uri}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": data.get('__RequestVerificationToken')
            },
            body: JSON.stringify({
                Name: data.get("Name"),
                Budget: data.get("Budget"),
                IncomeId: data.get("IncomeId")
            })
        });
        
        if (response.ok) {
            createCategoryElement(await response.json());
        }
        else {
            console.error(`HTTP Post Error: ${response.status}`);
        }

    } catch (error) {
        console.error(error);
    };
}

function createCategoryElement(category) {        
    categoriesContainer.innerHTML += `
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