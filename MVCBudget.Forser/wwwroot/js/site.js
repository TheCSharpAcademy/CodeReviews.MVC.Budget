showPopupCategory = (url, title) => {
    if ($('#ListCategoriesModal').hasClass('show')) {
        $('#ListCategoriesModal').modal('hide');
    }

    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#edit-category').html(res.html);
            $('#form-modal .modal-title').html(title);
            $('#form-modal').modal('show');
            $.validator.unobtrusive.parse($("#update-form"));
        },
        error: function (err) {
            console.log(err);
        }
    });
}

jQueryUpdateCategoryPost = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#edit-categories').html(res.html);
                    $('#form-modal .modal-body').html('');
                    $('#form-modal .modal-title').html('');
                    $('#form-modal').modal('hide');
                    location.reload();
                }
                else {
                    $('#edit-categories').html(res.html);
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
        $.validator.unobtrusive.parse($("#update-form"));
        return false;
    }
    catch (ex) {
        console.log(ex);
    }
};

jQueryDeleteCategory = form => {
    if (confirm('Are you sure you want to delete this Category?')) {
        try {
            if ($('#ListCategoriesModal').hasClass('show')) {
                $('#ListCategoriesModal').modal('hide');
            }

            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    $('#view-categories').html(res.html);
                    $('#ListCategoriesModal').modal('show');
                },
                error: function (err) {
                    console.log(err);
                }
            });
        }
        catch {
            console.log(ex);
        }
    }
    return false;
}