const uri = "https://localhost:7246/api/Categories";

document.addEventListener("DOMContentLoaded", () => {
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

    $('.delete-icon').on("click", async function (event) {
        var id = $(this).closest('.category').data('id');
        var token = this.querySelector('input').value
        deleteCategory(id, token);
    })
})

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
    };
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
    };    
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