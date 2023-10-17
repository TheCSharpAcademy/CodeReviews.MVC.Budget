$('#myModal').on('shown.bs.modal', function () {
    $('#myInput').trigger('focus')
})

document.getElementById('searchTransaction').addEventListener('keyup', function (e) {
    const searchTerm = e.target.value.toLowerCase();
    const tableRows = document.querySelectorAll('.table tbody tr');

    tableRows.forEach(row => {
        const name = row.querySelector("td:nth-child(1)").textContent.toLowerCase(); // Assuming transaction name is in the first column (index 0)

        if (name.includes(searchTerm)) {
            row.style.display = ''; // Show row
        } else {
            row.style.display = 'none'; // Hide row
        }
    });
});