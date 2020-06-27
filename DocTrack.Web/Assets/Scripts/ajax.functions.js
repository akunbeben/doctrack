// Initial Set Modal
$('#modal-form').hide();
$('#modal-loading').show();

// disable default event form submit
$('#input-form').submit(function (e) {
    e.preventDefault();
})

// Show Add Modal
function onClickAdd(title, url, urlSave) {
    $('#formModal').modal('show');
    $('form').trigger('reset');
    $('.modal-title').html(title);
    $('form').validate();

    $('.btn-submit').html('Save');
    $('.btn-submit').attr('onclick', 'onSave("' + urlSave + '")');
    $('.btn-submit').attr('disabled', false);

    ajaxFormRequest(url);
}

// Add New Data
function onSave(urlSave) {
    var data = $('form').serialize(),
        type = 'POST';

    if ($('form').valid()) {
        ajaxPostRequest(urlSave, data, type);
    }
}

// Show Edit Modal
function onClickEdit(title, url, urlUpdate) {
    $('#formModal').modal('show');
    $('.modal-title').html(title);

    $('.btn-submit').html('Edit');
    $('.btn-submit').attr('onclick', 'onUpdate("' + urlUpdate + '")');
    $('.btn-submit').attr('disabled', false);

    ajaxFormRequest(url);
}

// Update Data
function onUpdate(urlUpdate) {
    type = 'POST',
    data = $('form').serialize();

    ajaxPostRequest(urlUpdate, data, type);
}

// Delete Function
function onClickDelete(urlDelete) {
    Swal.fire({
        title: 'Delete Confirmation',
        text: 'Are You Sure To Delete This Data ?',
        type: 'warning',
        showCancelButton: true
    }).then((result) => {
        if (result.value) {
            var data = null;
            var type = 'POST';

            ajaxPostRequest(urlDelete, data, type);
        }
    });
}

// AJAX Add, Edit, & Delete
function ajaxPostRequest(url, data, type) {
    $.ajax({
        url: url,
        data: data,
        type: type,
        dataType: 'json',
        beforeSend: function () {
            $('.btn-submit').attr('disabled', true);
        },
        success: function (data) {
            $('.btn-submit').attr('disabled', false);

            if (data.Status === 'Success') {
                $('#formModal').modal('hide');
                table.ajax.reload();

                succesSave(data);
            } else if (data.Status === 'Failed') {

                failedSave(data);
            }
        },
        error: function (error) {
            $('.btn-submit').attr('disabled', false);
            failedSave(error);
        }
    });
}

// Success Save Function
function succesSave(data) {
    Swal.fire({
        toast: true,
        timer: 5000,
        position: 'top-end',
        type: 'success',
        title: data.Message,
        showConfirmButton: false,
    });
};

// Failed Save Function
function failedSave(data) {
    Swal.fire({
        toast: true,
        timer: 5000,
        position: 'top-end',
        type: 'error',
        title: data.Message,
        showConfirmButton: false,
    });
};