﻿$(document).ready(function () {
    table = $('#checktbl').DataTable({
        ajax: {
            url: '/Requests/LoadCheck',
            data: { id: $('#Id').val() },
            type: 'GET',
            dataType: 'JSON'
        },

        columns: [
            { data: null, orderable: false },
            { data: 'DeXuat' },
            { data: 'HopDong' },
            { data: 'Ten_NCC' },
            { data: 'Ten_TB' },
            { data: 'YC_KT' },
            { data: 'TT_KT' },
            { data: 'YC_SL' },
            { data: 'TT_SL' },
            { data: 'DonVi' },
            {
                data: 'CO', render: function (data) {
                    if (data === true)
                        return "Có";
                    else if (data === false)
                        return "Không Có";
                    else return data;
                }
            },
            {
                data: 'CQ', render: function (data) {
                    if (data === true)
                        return "Có";
                    else if (data === false)
                        return "Không Có";
                    else return data;
                }
            },
            {
                data: 'MTR', render: function (data) {
                    if (data === true)
                        return "Có";
                    else if (data === false)
                        return "Không Có";
                    else return data;
                }
            },
            {
                data: 'SN', render: function (data) {
                    if (data === true)
                        return "Có";
                    else if (data === false)
                        return "Không Có";
                    else return data;
                }
            },
            {
                data: 'PN', render: function (data) {
                    if (data === true)
                        return "Có";
                    else if (data === false)
                        return "Không Có";
                    else return data;
                }
            },
            {
                data: 'Other', render: function (data, type, row) {
                    if (data === true)
                        return "<span onclick='viewnote(" + row.Id + ")'>Có</span>";
                    else if (data === false)
                        return "Không Có";
                    else return data;
                }
            },
           
            {
                 data: 'Result', render: function (data,type,row) {
                    if (data === true)
                        return "Đạt";
                    else if (data === false)
                        return "<span onclick='viewreason("+row.Id+")'>Không Đạt</span>";
                    else return data;
                 }
            },
           
            {
                data: 'Id', render: function (data) {
                    return "<button class='btn-sm btn-success' onclick='detail(" + data + ")' ><span class='fa fa-pencil-square-o'></span></button> | <button class='btn btn-danger' onclick='deletedata(" + data + ")' ><span class='fa fa-trash-o'></span></button>";

                }, orderable: false
            }
        ],
        scrollX: true,
        order: [[1, 'asc']]
    });
    table.on('order.dt search.dt', function () {
        table.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
    $("#name").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/Requests/ListEmail',
                dataType: 'JSON',
                data: { a: request.term },
                success: function (data) {
                    response(data);
                    //response($.map(data, function (item) {

                    //    return { lable: item.FullName, value: item.UserName };

                    //}));
                }
            });
        },
        focus: function (event, ui) {
            $("#name").val(ui.item.FullName);
            return false;
        },
        select: function (event, ui) {
            $("#name").val(ui.item.FullName);
            $("#txtemail").val(ui.item.Email);
            $("#txtuser").val(ui.item.UserName);
            return false;
        },
        appendTo:"#modalemail"
    }).autocomplete("instance")._renderItem = function (ul, item) {
        return $("<li>")
            .append("<div>Tên: " + item.FullName + "<br>Email: " + item.Email + "</div>")
            .appendTo(ul);
    };
    $("#formemail").validate({
        rules: {
            name: "required"
        },
        messages: {
            name: "Yêu Cầu Nhập Tên"
        }
    });
});

function detail(ma) {
    $.ajax({
        url: '/Requests/DetailCheck',
        type: 'GET',
        data: { ma: ma, id: $('#Id').val() },
        success: function (data) {
            if (data === "Error") {
            return bootbox.alert("Bạn không thể thực hiện tác vụ này");
            }
            $('#myModal').modal('show');
            $('.modal-body').html(data);
            var form = $('#checkform').closest('form');
            form.removeData('validator');
            form.removeData('unobtrusiveValidation');
            $.validator.unobtrusive.parse(form);
        },
        error: function (ex) {
            $.notify(ex, "error");
        }
    });
}
function save() {
    var form = $('#checkform').closest('form');
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);
    if (!$('#checkform').valid()) return false;
    $.ajax({
        url: '/Requests/SaveCheck',
        type: 'POST',
        data: $('#checkform').serialize(),
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
                var form = $('#checkform').closest('form');
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
                    url: '/Requests/DeleteCheck',
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
                            $.notify("Lỗi Không Thể Xóa", "error");
                        }
                    },
                    error: function (ex) {
                        console.log(ex);
                    }
                });
            }
        });
}
function sendemail() {
    
    if (!$('#formemail').valid()) {
        $(".error").css("padding",0);
        return false;
    }
    $('#modalemail').modal('hide');
    var dialog = bootbox.dialog({
        message: '<p><i class="fa fa-spin fa-spinner"></i>Làm ơn chờ để xử lý...</p>',
        closeButton: false
    });
    var formdata = new FormData();
    var file = $('#filename').get(0).files;
    formdata.append("FullName", $('#name').val());
    formdata.append("name", $('#txtuser').val() );
    formdata.append("email", $('#txtemail').val());
    formdata.append("Id", $('#Id').val());
    formdata.append("file", file[0]);
    $.ajax({
        url: '/Requests/SendEmail',
        type:'POST',
        data: formdata,
        contentType: false,
        processData: false,
        success: function (data) {
            dialog.modal('hide');
            if (data.status) {
                $.notify("Gửi Email Thành Công", "success");
            }
            else if (data === "Error") {
                return bootbox.alert("Bạn không thể thực hiện tác vụ này");
            }
            else
                $.notify("Gửi Email Bị Lỗi", "error");
        },
        error: function (ex) {
            dialog.modal('hide');
            $.notify(ex, "error");
        }

    });
 }
function viewreason(ma) {
    $.ajax({
        url: '/Approvals/LoadReason',
        data: { ma: ma },
        type: 'GET',
        success: function (data) {

            if (data.reason !== '') {
                bootbox.dialog({
                    message: data.reason
                });
            }
            else {
                bootbox.dialog({
                    message: 'Chưa Nhập Nguyên Nhân'
                });
            }
        },
        error: function (ex) {
            bootbox.dialog({
                message: 'Lỗi không hiển thị'
            });
        }
    });
}
function viewnote(ma) {
    $.ajax({
        url: '/Approvals/LoadNote',
        data: { ma: ma },
        type: 'GET',
        success: function (data) {
            if (data.reason !== '') {
                bootbox.dialog({
                    message: data.reason
                });
            }
            else {
                bootbox.dialog({
                    message: 'Chưa Nhập Ghi Chú'
                });
            }
        },
        error: function (ex) {
            bootbox.dialog({
                message: 'Lỗi không hiển thị'
            });
        }
    });
}