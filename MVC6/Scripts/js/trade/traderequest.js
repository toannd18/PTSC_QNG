$(document).ready(function () {
    table = $("#commercetable").DataTable({
        ajax: {
            url: '/Commerce/Requests/Load',
            type: 'GET',
            dataType: 'Json'
        },
        columnDefs: [
            { type: 'date-euro', targets: 5 }
        ],
        columns: [
             {
                 className: 'details-control',
                 orderable: false,
                 data: null,
                 defaultContent: ''
             },
            { data: 'Ma' },
            { data: 'Tieu_De' },
            {
                data: 'Kieu', render: function (data) {
                    return data === true ? "Hàng hóa" : "Dịch vụ";
                }
            },
            { data: 'FullName_Dx' },
            { data: 'FullName_TH' },
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
    $('#commercetable tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var row = table.row(tr);
        console.log(tr);
        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row
            row.child(format(row.data())).show();
            tr.addClass('shown');
            tr.next().find(".Status").val(row.data().Status.toString());
            
        }
        $(".datepicker").datepicker({
            dateFormat: "dd/mm/yy"
        });
        $.validator.addMethod('datepicker',
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
               },"Yêu cầu nhập ngày");
        $("#" + row.data().Id).validate({
            errorClass: "text-danger",
            errorElement: "span",
            highlight: function (element, errorClass, validClass) {
                $(element).addClass(errorClass).removeClass(validClass);
                $(element.form).find("label[for=" + element.id + "]").addClass(errorClass);
            },
            unhighlight: function(element, errorClass, validClass) {
                $(element).removeClass(errorClass).addClass(validClass);
                $(element.form).find("label[for=" + element.id + "]")
                  .removeClass(errorClass);
            },
            rules: {
                Ngay_Ky: { datepicker: true },
                Ngay_TH: { datepicker: true },
                Ngay_THTT: { datepicker: true },
                Ngay_NT: { datepicker: true },
                Ngay_NT_QC: { datepicker: true }
            }
            
        });
        
    });
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
            auto_complete($("#subTen_Dx"), $("#Ten_Dx"));
            auto_complete($("#subTen_Dx1"), $("#Ten_Dx1"));
            auto_complete($("#subTen_Dx2"), $("#Ten_Dx2"));
            auto_complete($("#subTen_Dx3"), $("#Ten_Dx3"));
            auto_complete($("#subTen_Dx4"), $("#Ten_Dx4"));
            auto_complete($("#subTen_Dx5"), $("#Ten_Dx5"));
            auto_complete($("#subTen_Dg"), $("#Ten_Dg"));
            auto_complete($("#subTen_Dg1"), $("#Ten_Dg1"));
            auto_complete($("#subTen_Dg2"), $("#Ten_Dg2"));
            auto_complete($("#subTen_Dg3"), $("#Ten_Dg3"));
            auto_complete($("#subTen_Dg4"), $("#Ten_Dg4"));
            $(".datepicker").datepicker({
                dateFormat: "dd/mm/yy"
            });
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
                data: { ma: ma.Id },
                success: function (data) {
                    if (data.status === true) {
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
    window.location.href = url;
}
function auto_complete(id,sub) {
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
            id.val(ui.item.FullName);
           
            sub.val(ui.item.UserName);
            return false;
        },
        select: function (event, ui) {
            id.val(ui.item.FullName);
            
            sub.val(ui.item.UserName);
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

//Format database
function format(d) {
    // `d` is the original data object for the row
    return '<form id="' + d.Id + '">' +
    '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;" >' +
        '<tr>' +
            '<td>Số ĐĐH/HĐ:</td>' +
            '<td>' + (d.Sohd===null?"":d.Sohd) + '</td>' +
            '<td>Tên NCC:</td>' +
            '<td>' + (d.TenNCC===null?"":d.TenNCC) + '</td>' +
        '</tr>' +
        
        '<tr>' +
            '<td>Ngày ĐĐH/HĐ:</td>' +
            '<td><input type="text" class="form-control datepicker" name="Ngay_Ky" value="' + (d.Ngay_Ky === null ? "" : moment(d.Ngay_Ky).format("DD/MM/YYYY")) + '" /></td>' +
            '<td>Ngày thực hiện:</td>' +
            '<td><input type="text" class="form-control datepicker" name="Ngay_TH" value="' + (d.Ngay_TH === null ? "" : moment(d.Ngay_TH).format("DD/MM/YYYY")) + '" /></td>' +
            '<td>Ngày thực hiện thực tế:</td>' +
            '<td><input type="text" class="form-control datepicker" name="Ngay_THTT" value="' + (d.Ngay_THTT === null ? "" : moment(d.Ngay_THTT).format("DD/MM/YYYY")) + '" /></td>' +
        '</tr>' +
        '<tr>' +
            '<td>Ngày nghiệm thu:</td>' +
            '<td><input type="text" class="form-control datepicker" name="Ngay_NT" value="' + (d.Ngay_NT === null ? "" : moment(d.Ngay_NT).format("DD/MM/YYYY")) + '" /></td>' +
            '<td>Ngày nhận hồ sơ:</td>' +
            '<td><input type="text" class="form-control datepicker" name="Ngay_NT_QC" value="' + (d.Ngay_NT_QC === null ? "" : moment(d.Ngay_NT_QC).format("DD/MM/YYYY")) + '" /></td>' +
            '<td>Tình trạng:</td>' +
            '<td><select class="form-control Status"  name="Status">' +
                '<option value="false">Đang thực hiện</option>'+
                '<option value="true">Hoàn thành</option>' +
                '</select></td>' +
        '</tr>' +
         '<tr>' +
            '<td>Ghi chú:</td>' +
            '<td><input type="text" class="form-control" name="Ghi_Chu" value="' + (d.Ghi_Chu === null ? "" : d.Ghi_Chu) + '" /></td>' +
            '<td><input type="button" class="btn btn-primary" onclick="submitform(' + d.Id + ')" value="Lưu"></td>' +
        '</tr>' +
 
    '</table></form>';
}
function submitform(id) {
    var form = $("#" + id).closest('form');
    if (!form.valid()) {
        return false;
    }
    var formdata = new FormData();
    formdata.append("Id", id);
   
    var data = form.serializeArray();

    $.each(data, function (i, val) {
     
        formdata.append(val.name, val.value);
        
    });
    $.ajax({
        url: '/Commerce/Requests/Update',
        data: formdata,
        contentType: false,
        processData: false,
        type: 'Post',
        success: function (data) {
            if (data.status === true) {
                $.notify("Lưu thành công", "success");
            }
            else {
                $.notify("Lỗi Kết Nối", "error");
            }
        },
        error: function (err) {
            $.notify("Lỗi máy chủ", "error");
        }
    });
    
}