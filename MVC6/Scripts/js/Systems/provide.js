$(document).ready(function () {
    table = $('#providetbl').DataTable({
        ajax: {
            url: '/Providers/Load',
            type: 'GET',
            dataType: 'JSON'
        },
        columns: [
            { data: 'Ma_NCC',width:'5%' },
            { data: 'Ten_NCC' },
            { data: 'Dia_Chi' },
            { data: 'Tel', render: function (data, type, row) {
                return (data == null ? "" : data) + '<br>(' + (row.Fax == null ? "" : row.Fax)+')';
                }
            },
            { data: 'Attn' },
            { data: 'Email' },
            {
                data: 'Ma_NCC', render: function (data) {
                    return "<button class='btn-sm btn-success' onclick='detail(\"" + data + "\")' data-toggle='tooltip' data-placement='bottom' title='Sửa' ><span class='fa fa-pencil-square-o'></span></button> | <button class='btn btn-danger' onclick='deletedata(\"" + data + "\")'data-toggle='tooltip' data-placement='bottom' title='Xóa' ><span class='fa fa-trash-o'></button>";

                }, orderable: false,width:'10%'
            }
        ]
    });
});
function detail(ma) {
    $.ajax({
        url: '/Providers/Detail',
        data: { ma: ma },
        type: 'GET',
        success: function (data) {
            $('#myModal').modal('show');
            $('.modal-body').html(data);
            if (ma !== '') {
                $('#Ma_NCC').prop('readonly', true);
            }
            else
                $('#Ma_NCC').prop('readonly', false);
            var form = $('#providerform').closest('form');
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
    var form = $('#providerform').closest('form');
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);
    if (!$('#providerform').valid()) return false;
    $.ajax({
        url: '/Providers/Save',
        type: 'POST',
        data: $('#providerform').serialize(),
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
                var form = $('#providerform').closest('form');
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
                url: '/Providers/Delete',
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
