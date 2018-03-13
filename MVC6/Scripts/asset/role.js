$(document).ready(function () {
    table = $('#roletbl').DataTable({
        ajax: {
            url: '/Roles/Load',
            type: 'GET',
            dataType: 'json'
        },
        lengthChange:false,
        columns: [
            { data: 'Role' },
            { data: 'Description' },
            {
                data: 'Id', render: function (data) {
                    return "<button class='btn-sm btn-success' onclick='detail(\"" + data + "\")' >Sửa</button> | <button class='btn btn-danger' onclick='deletedata(\"" + data + "\")' >Xóa</button>";

                }, orderable: false
            }
        ]
    });
});
function detail(ma) {
    $.ajax({
        url: '/Roles/Detail',
        type: 'GET',
        data: { ma: ma },
        success: function (data) {
            $('#myModal').modal('show');
            $('#modalbody').html(data);
            if (ma > 0) {
                $('#Role').prop('readonly', true);
            }
            else
                $('#Role').prop('readonly', false);
            var form = $('#roleform').closest('form');
            form.removeData('validator');
            form.removeData('unobtrusiveValidation');
            $.validator.unobtrusive.parse(form);
        },
        error: function (ex)
        { console.log(ex); }
    });
}
function savedata() {
    var form = $('#roleform').closest('form');
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);
   
    if (!$('#roleform').valid()) return false;
    $.ajax({
        url: '/Roles/Save',
        data: $('#roleform').serialize(),
        type: 'POST',
        success: function (data) {
            if (data.status) {
                $('#myModal').modal('hide');
                $.notify("Lưu thành công", "success");
                table.ajax.reload();
            }
        },
        error: function (ex) {
            console.log(ex);
        }
    });
}
function deletedata(ma) {
    bootbox.confirm('Bạn muốn xóa cái này', function (data) {
        if (data) {
            $.ajax({
                url: '/Roles/Delete',
                type: 'POST',
                data: { ma: ma },
                success: function (data) {
                    if (data.status) {
                        $.notify("Xóa thành công", "success");
                        table.ajax.reload();
                    }
                },
                error: function (ex) {
                    console.log(ex);
                }
            });
        }
    });
}