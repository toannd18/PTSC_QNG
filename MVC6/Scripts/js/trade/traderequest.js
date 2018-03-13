$(document).ready(function () {
    table = $("#commercetable").DataTable({
        ajax: {
            url: '/Commerce/Requests/Load',
            type: 'GET',
            dataType: 'Json'
        },
        columnDefs: [
            { type: 'date-euro', targets: 0 }
        ],
        columns: [
            { data: 'Ma' },
            { data: 'Tieu_De' },
            {
                data: 'Kieu', render: function (data) {
                    return data === true ? "Hàng hóa" : "Dịch vụ";
                }
            },
            { data: 'Ten_Dx' },
            {
                data: 'Ngay_Tao', render: function (data) {
                    return moment(data).format("DD/MM/YYYY");
                }
            }
        ],
        select: {
            style: 'single'
        }
    });
   $('[data-toggle="tooltip"]').tooltip();
   
});

//chọn dữ liệu
function getdata() {
    var ma = table.row('.selected').data();
    if (ma === undefined) {
        bootbox.alert("Bạn chưa chọn dữ liệu");
        return false;
    }
    getdetail(ma.Id);
}
//Tải dữ liệu
function getdetail(ma) {
    $.ajax({
        url: '/Commerce/Requests/Detail',
        data: { Id: ma },
        type: 'GET',
        success: function (data) {
            $('#myModal').modal('show');
            $('#modalbody').html(data);
            if (ma === 0) {
                $('#Ma').prop('readonly', false);
            }
            else {
                $('#Ma').prop('readonly', true);
            }
            var form = $('#traderequestform').closest('form');
            form.removeData('validator');
            form.removeData('unobtrusiveValidation');
            $.validator.unobtrusive.parse(form);
            auto_complete($("#Ten_Dx"));
            auto_complete($("#Ten_Dx1"));
            auto_complete($("#Ten_Dx2"));
            auto_complete($("#Ten_Dx3"));
            auto_complete($("#Ten_Dx4"));
            auto_complete($("#Ten_Dx5"));
            $(".datepicker").datepicker({
                dateFormat: "dd/mm/yy"
            });
        }
    });
}
//lưu dữ liệu
function savedata() {
    var form = $('#traderequestform').closest('form');
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);
    $.validator.addMethod('date',
   function (value, element) {
       if (this.optional(element)) {
           return true;
       }

       var ok = true;
       try {
           $.datepicker.parseDate('dd/mm/yy', value);
       }
       catch (err) {
           ok = false;
       }
       return ok;
   });
    if (!form.valid()) {
        return false;
    }
    $.ajax({
        url: '/Commerce/Requests/Save',
        data: $("#traderequestform").serialize(),
        type: 'Post',
        success: function (res) {
            if (res.status === true) {
                $('#myModal').modal('hide');
                $.notify("Lưu thành công", "success");
                table.ajax.reload();
            }
            else if (res.status === false) {
                $('#myModal').modal('hide');
                $.notify("Lỗi Kết Nối", "error");
            }

        }

    });
}
//Xóa dữ liệu
function deletedata() {
    var ma = table.row('.selected').data();
    var data = table.cells('.selected', '').render('display');
    if (ma === undefined) {
        bootbox.alert("Bạn chưa chọn dữ liệu");
        return false;
    }
    bootbox.confirm("Ban muốn xóa đề xuất " + data[0] + " này", function (result) {
        if (result) {
            $.ajax({
                url: '/Commerce/Requests/Delete',
                type: 'Post',
                data: { ma: ma.Id},
                success: function (data) {
                    if (data.status===true) {
                        $('#myModal').modal('hide');
                        $.notify("Xóa thành công", "success");
                        table.ajax.reload();
                    }
                    else if (!data) {
                        bootbox.alert("Ban Không có quyền để làm điều này");
                    }
                    else {
                        $('#myModal').modal('hide');
                        $.notify("Lỗi Không Thể Xóa", "error");
                    }
                },
                error: function (ex) {
                    bootbox.alert(ex);
                }
            });
        }
    });
}
//Tới trang chi tiết
function detailweb() {
    var ma = table.row('.selected').data();
    if (ma === undefined) {
        bootbox.alert("Bạn chưa chọn dữ liệu");
        return false;
    }
    var url = "/Commerce/Requests/IndexDetail?ma=" + ma.Id;
    window.location.href =url;
}
function auto_complete(id) {
    id.autocomplete({
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
            id.val(ui.item.UserName);
            return false;
        },
        select: function (event, ui) {
            id.val(ui.item.UserName);
         
            return false;
        },
        change: function (event, ui) {
            if (ui.item) {
                
                return false;
            }
            else {
                alert("Yêu cầu lựa chọn trong danh sách");
                id.val("");
                id.focus();
                return false;
            }
        },
        appendTo: "#modalbody"
    }).autocomplete("instance")._renderItem = function (ul, item) {
        return $("<li>")
            .append("<div>Tên: " + item.FullName + "<br>Email: " + item.Email + "</div>")
            .appendTo(ul);
    };
}