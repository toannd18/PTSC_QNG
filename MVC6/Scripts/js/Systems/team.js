$(document).ready(function () {
    table = $('#teamtbl').DataTable({
        ajax: {
            url: '/Teams/Load',
            type: 'GET',
            dataType: 'json'
        },
        lengthChange: false,
        columns: [
            { data: 'Ma_TO' },
            { data: 'Ten_TO' },
            { data: 'Ten_BP' },
            {
                data: 'Ma_TO', render: function (data) {
                    return "<button class='btn-sm btn-success' onclick='detail(\"" + data + "\")' >Sửa</button> | <button class='btn btn-danger' onclick='deletedata(\"" + data + "\")' >Xóa</button>";

                }, orderable: false, width: '200'
            }
        ]
    });
});
function detail(ma) {
    $.ajax({
        url: '/Teams/Detail',
        data: { ma: ma },
        type: 'GET',
        success: function (data) {
            if (!data) {
                bootbox.alert("Ban Khong co quyen de lam cai nay");
            }
            else {
                $('#myModal').modal('show');
                $('#modalbody').html(data);
                if (ma === '') {
                    $('#Ma_TO').prop('readonly', false);
                }
                else {
                    $('#Ma_TO').prop('readonly', true);
                }
                var form = $('#teamform').closest('form');
                form.removeData('validator');
                form.removeData('unobtrusiveValidation');
                $.validator.unobtrusive.parse(form);
            }
        },
        error: function (ex) {
            $.notify(ex, "error");
        }
    });
}
function savedata() {
    var form = $('#teamform').closest('form');
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);
    if (!$('#teamform').valid()) return false;
    $.ajax({
        url: '/Teams/Save',
        type: 'POST',
        data: $('#teamform').serialize(),
        success: function (data) {
            if (data.status) {
                $('#myModal').modal('hide');
                $.notify("Lưu thành công", "success");
                table.ajax.reload();
            }
            else if (!data.status) {
                $('#myModal').modal('hide');
                $.notify("Lỗi Kết Nối", "error");
            }
            else {
                $('#modalbody').html(data);
                var form = $('#teamform').closest('form');
                form.removeData('validator');
                form.removeData('unobtrusiveValidation');
                $.validator.unobtrusive.parse(form);
            }

        },
        error: function (ex) {
            $.notify(ex, "error");
        }
    });
}
function deletedata(ma) {
    bootbox.confirm("Bạn Muốn Xóa Cái Này", function (result) {
        if (result) {
            $.ajax({
                url: '/Teams/Delete',
                type: 'POST',
                data: { ma: ma },
                success: function (data) {
                    if (!data) {
                        bootbox.alert("Ban Khong co quyen de lam cai nay");
                    }
                    else {
                        if (data.status) {
                            $('#myModal').modal('hide');
                            $.notify("Xóa thành công", "success");
                            table.ajax.reload();
                        }
                        else {
                            $('#myModal').modal('hide');
                            $.notify("Lỗi Không Thể Xóa", "error");
                        }
                    }
                },
                error: function (ex) {
                    console.log(ex);
                }
            });
        }
    });
}