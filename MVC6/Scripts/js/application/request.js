$(document).ready(function () {
    table = $('#requiretbl').DataTable({
        ajax: {
            url: '/Requests/LoadRequire',
            type: 'Get',
            dataType: 'JSON'
        },
        columns: [
            {
                data: 'LateId', render: function (data) {
                    return data;
                }
            },
            { data: 'Ten_BP' },
            { data: 'Dia_Diem' },
            {
                data: 'Date', render: function (data) {
                    return moment(data).format("DD/MM/YYYY");
                }
            },
            {data:'FullName'},
            {
                data: 'Status_Autho', render: function (data) {
                    if (data === "A") {
                        return "<span class='fa fa-check-circle' style='color:forestgreen'>Đã Duyệt</span>";
                    }
                    else if (data === "R") {
                        return "<span class='fa fa-times-circle' style='color:red'>Từ Chối</span>";
                    }
                    else if (data === "W") {
                        return "<span class='fa fa-times-circle' style='color:blue'>Đã Kiểm Tra Thực Tế</span>";
                    }
                    else {
                        return "<span class='fa fa-spinner' style='color:yellow'>Đang Chờ</span>";
                    }
                }, orderable: false
            },
            {
                data: 'Id', render: function (data) {
                    return "<button class='btn-sm btn-success' onclick='detail(" + data + ")' ><span class='fa fa-pencil-square-o'></span></button> | <button class='btn btn-danger' onclick='deletedata(" + data + ")' ><span class='fa fa-trash-o'></span></button> | <a href= '/Requests/CheckList?ma="+data+"' class='btn btn-info' ><span class='fa fa-info-circle'></span></a >";
                }, orderable: false, width:'15%'
            }
        ]
    });
});
$(document).ajaxComplete(function () {
    
    $('#Other').change(function () {
        if ($(this).is(":checked")) {
            $('#Note').prop("readonly", false);
        }
        else {
            $('#Note').prop("readonly", true);
        }
    });
});
function detail(ma) {
    $.ajax({
        url: '/Requests/DetailRequire',
        type: 'GET',
        data: { ma: ma },
        success: function (data) {
            if (!data) {
                bootbox.alert("Bạn không có quyền làm điều này");
            }
            else if (data === "Error") {
                bootbox.alert("Bạn không thể thực hiện tác vụ này");
            }
            else {
                $('#myModal').modal('show');
                $('.modal-body').html(data);
                var form = $('#requireform').closest('form');
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
function save() {
    var form = $('#requireform').closest('form');
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);
    if (!$('#requireform').valid()) return false;
    $.ajax({
        url: '/Requests/SaveRequire',
        type: 'POST',
        data: $('#requireform').serialize(),
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
                var form = $('#requireform').closest('form');
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
                url: '/Requests/DeleteRequire',
                type: 'POST',
                data: { ma: ma },
                success: function (data) {
                    if (!data) {
                        bootbox.alert("Bạn không có quyền làm điều này");
                    }
                    else if (data === "Error") {
                        bootbox.alert("Bạn không thể thực hiện tác vụ này");
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

