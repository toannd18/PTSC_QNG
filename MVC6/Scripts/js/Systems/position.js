$(document).ready(function () {
    table = $('#positiontbl').DataTable({
        ajax: {
            url: '/Positions/Load',
            type: 'GET',
            dataType: 'json'
        },
        lengthChange: false,
        columns: [
            { data: 'Ma_CV' },
            { data: 'Ten_CV' },
            { data: 'Display' },
            {
                data: 'Ma_CV', render: function (data) {
                    return "<button class='btn-sm btn-success' onclick='detail(\"" + data + "\")' >Sửa</button> | <button class='btn btn-danger' onclick='deletedata(\"" + data + "\")' >Xóa</button>";

                }, orderable: false,width:'200'
            }
        ]
    });
});
function detail(ma) {
    $.ajax({
        url: '/Positions/Detail',
        data: { ma: ma },
        type: 'GET',
        success: function (data) {
            if (!data) {
                bootbox.alert("Ban Khong co quyen de lam cai nay");
            }
            else {
                $('#myModal').modal('show');
                $('.modal-body').html(data);
                if (ma === '') {
                    $('#Ma_CV').prop('readonly', false);
                }
                else {
                    $('#Ma_CV').prop('readonly', true);
                }
                var form = $('#positionform').closest('form');
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
    var form = $('#positionform').closest('form');
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);
    if (!$('#positionform').valid()) return false;
    $.ajax({
        url: '/Positions/Save',
        type: 'POST',
        data: $('#positionform').serialize(),
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
                $('.modal-body').html(data);
                var form = $('#positionform').closest('form');
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
                url: '/Positions/Delete',
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