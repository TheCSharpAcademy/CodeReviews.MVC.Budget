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
});