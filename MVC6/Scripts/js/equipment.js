$(document).ready(function () {
    table = $('#equipttbl').DataTable({
        ajax: {
            url: '/Equipments/Load',
            type: 'GET',
            dataType: 'JSON'
        },
        columns: [
            { data: 'Ma_TB', autoWidth: true },
            { data: 'Ten_TB', autoWidth: true },
            { data: 'Ten_NCC', autoWidth: true },
            { data: 'Cong_Suat', autoWidth: true },
            {
                data: 'Ma_TB', render: function (data) {
                    return "<button class='btn btn-primary' onclick='detail(\"" + data + "\")' >Sửa</button> | <button class='btn btn-danger' onclick='deletedata(\"" + data + "\")' >Xóa</button>";

                }, orderable: false, width:'12%'
            }
        ]
    });
});
function detail(ma) {
    $.ajax({
        url: '/Equipments/Detail',
        data: { ma: ma },
        type: 'GET',
        success: function (data) {
            $('#myModal').modal('show');
            $('.modal-body').html(data);
            if (ma !== '') {
                $('#Ma_TB').prop('readonly', true);
            }
            else
                $('#Ma_TB').prop('readonly', false);
            var form = $('#equiptform').closest('form');
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
    var form = $('#equiptform').closest('form');
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);
    if (!$('#equiptform').valid()) return false;
    $.ajax({
        url: '/Equipments/Save',
        type: 'POST',
        data: $('#equiptform').serialize(),
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
                var form = $('#equiptform').closest('form');
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
                url: '/Equipments/Delete',
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
