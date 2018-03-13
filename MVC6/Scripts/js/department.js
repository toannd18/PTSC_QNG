$(document).ready(function () {
    table = $('#depttbl').DataTable({
        ajax: {
            url: '/Departments/Load',
            type: 'GET',
            dataType: 'JSON'
        },
        columns: [
            { data: 'Ma_BP' },
            { data: 'Ten_BP' },
            {
                data: 'Ma_BP', render: function (data) {
                    return "<button class='btn-sm btn-success' onclick='detail(\"" + data + "\")' >Sửa</button> | <button class='btn btn-danger' onclick='deletedata(\"" + data + "\")' >Xóa</button>";

                }, orderable: false
            }
        ]
    });
});
function detail(ma) {
    $.ajax({
        url: '/Departments/Detail',
        data: { ma: ma },
        type: 'GET',
        success: function (data) {
            $('#myModal').modal('show');
            $('.modal-body').html(data);
            if (ma !== '') {
                $('#Ma_BP').prop('readonly', true);
            }
            else
                $('#Ma_BP').prop('readonly', false);
            var form = $('#deptform').closest('form');
            form.removeData('validator');
            form.removeData('unobtrusiveValidation');
            $.validator.unobtrusive.parse(form);
        },
        error: function (ex) {
            console.log(ex);
        }
    });
}
function save() {
    var form = $('#deptform').closest('form');
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);
    if (!$('#deptform').valid()) return false;
    $.ajax({
        url: '/Departments/Save',
        type: 'POST',
        data: $('#deptform').serialize(),
        success: function (data) {
            if (data.status) {
                $('#myModal').modal('hide');
                $.notify("Lưu thành công", "success");
                table.ajax.reload();
            }
            else if (!data.status) {
                $('#myModal').modal('hide');
                $.notify("Lỗi Kết Nối", "danger");
            }
            else {
                $('.modal-body').html(data);
                var form = $('#deptform').closest('form');
                form.removeData('validator');
                form.removeData('unobtrusiveValidation');
                $.validator.unobtrusive.parse(form);
            }

        },
        error: function (ex) {
            console.log(ex);
        }
    });
}
function deletedata(ma) {
    bootbox.confirm("Bạn Muốn Xóa Cái Này", function (result) {
        if (result) {
            $.ajax({
                url: '/Departments/Delete',
                type: 'POST',
                data: { ma: ma },
                success: function (data) {
                    if (data.status) {
                        $('#myModal').modal('hide');
                        $.notify("Xóa thành công", "success");
                        table.ajax.reload();
                    }
                    else {
                        $('#myModal').modal('hide');
                        $.notify("Lỗi Kết Nối", "danger");
                    }
                },
                error: function (ex) {
                    console.log(ex);
                }
            });
        }
    });
}