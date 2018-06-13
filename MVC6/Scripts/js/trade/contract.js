$(document).ready(function () {
    table = $("#contractable").DataTable({
        ajax: {
            url: '/Commerce/Contractions/Load',
            type: 'Get',
            dataType: 'Json'
        },
        columnDefs: [{
            type: 'date-euro', targets: 0
        }],
        columns: [
            {
                data: 'Date', render: function (data) {
                    return moment(data).format("DD/MM/YYYY");
                }
            },
            { data: 'Ten_HD' },
            { data: 'TenDeXuat' },
            { data: 'Ten_NCC' },
            { data: 'Diem' }

        ],
        select: {
            style: 'single'
        }
    });
    $('[data-toggle="tooltip"]').tooltip();
});

//Lấy dữ liệu
function getdata() {
    var ma = table.row('.selected').data();
    if (ma === undefined) {
        bootbox.alert("Bạn chưa chọn dữ liệu");
        return false;
    }
    detaildata(ma.Id);
}

function detaildata(id) {
    $.ajax({
        url: '/Commerce/Contractions/Detail',
        data: { Id: id },
        type: 'Get',
        success: function (data) {

            $('#myModal').modal('show');
            $('#modalbody').html(data);
            
            auto_complete($("#Ten_Dx"), $("#DeXuatId"));
            $(".datepicker").datepicker({
                dateFormat: "dd/mm/yy"
            });  
            $.validator.setDefaults({
                ignore: []
                // any other default options and/or rules
            });
          
            $("#Ten_Dx").change(function () {
                var ma = $("#DeXuatId").val();
                $.ajax({
                    url: '/Commerce/Contractions/SelectSupplier',
                    data: { dx: ma },
                    success: function (data) {
                        if (data.DG === 1) {
                            $('#Ten_NCC').val(data.Ten_NCC);
                            $('#Ma_NCC').val(data.Ma_NCC);
                        }
                        else {
                            alert("Bạn chưa đánh giá nhà cung cấp");
                            $('#Ten_NCC').val("");
                            $('#Ma_NCC').val("");
                            $("#Ten_Dx").val("");
                            $("#DeXuatId").val("");
                            $("#Ten_Dx").focus();
                            $('#Nguoi_TH').val("");
                        }
                        var form = $('#contractform').closest('form');
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
                        form.valid();
                    }
                });
            });
            
            TongDiem();
            GetHd();
        }
    });
}


// Lưu dữ liêu
function savedata() {
    var form = $('#contractform').closest('form');
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);
    $.validator.setDefaults({
        ignore: []
        // any other default options and/or rules
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
    if (!form.valid()) {
        return false;
    }
    $.ajax({
        url: '/Commerce/Contractions/Save',
        data: form.serialize(),
        type: 'Post',
        success: function (res) {
            if (res === true) {
                $('#myModal').modal('hide');
                $.notify("Lưu thành công", "success");
                table.ajax.reload();
            }
            else if (res === false) {
                $('#myModal').modal('hide');
                $.notify("Lỗi Kết Nối", "error");
            }
        }
    });
}
// Xóa dữ liệu
function deletedata() {
    var ma = table.row('.selected').data();
    var data = table.cells('.selected', '').render('display');
    if (ma === undefined) {
        bootbox.alert("Bạn chưa chọn dữ liệu");
        return false;
    }
    bootbox.confirm("Ban muốn xóa đề xuất " + data[1] + " này", function (result) {
        if (result) {
            $.ajax({
                url: '/Commerce/Contractions/Delete',
                type: 'Post',
                data: { Id: ma.Id },
                success: function (data) {
                    if (data === true) {
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

// Other Functions
function auto_complete(id, sub) {
    id.autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/Commerce/Contractions/LoadDeXuat',
                dataType: 'JSON',
                data: { ma: request.term },
                success: function (data) {
                    response(data);
                    //response($.map(data, function (item) {
                    //    return { lable: item.FullName, value: item.UserName };

                    //}));
                }
            });
        },
        focus: function (event, ui) {
            id.val(ui.item.Ma);
            $('#Nguoi_TH').val(ui.item.FullName_TH);
            sub.val(ui.item.Ma);
            return false;
        },
        select: function (event, ui) {
            id.val(ui.item.Ma);
            $('#Nguoi_TH').val(ui.item.FullName_TH);
            sub.val(ui.item.Id);
            var kieu = $("#Ten_HD").val();
            if (kieu.length >= 19) {
                kieu = kieu.substr(0, kieu.length - 2);
            }
            if (ui.item.Kieu) {
                kieu = kieu + "HH";
            }
            else {
                kieu = kieu + "DV";
            }
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
            .append("<div>Tên: " + item.Ma + "<br>Người thực hiện: " + item.FullName_TH + "</div>")
            .appendTo(ul);
    };
}

function TongDiem() {
    $(".Tong").keyup(function () {
        var tong = parseInt($("#Chat_luong").val()) * 3 + parseInt($("#Tien_Do").val()) * 3
        + parseInt($("#Gia_Ca").val()) * 2 + parseInt($("#Thai_Do").val()) * 2;
        $("#Tong_Diem").val(tong);
    });
}

function GetHd() {
    $("#Date").change(function () {
        var date = moment($.datepicker.parseDate('dd/mm/yy', $("#Date").val())).format("YYYY-MM-DD");
        var year = moment(date).year();
        $.ajax({
            url: '/Commerce/Contractions/GetHD',
            data: { date: date },
            success: function (data) {
                $("#So_HD").val(data);
                var Ten_Hd = $("#Ten_HD").val();
                if (!$("#Ten_HD").prop("readonly")) {
                    if (Ten_Hd.length >= 19) {
                        $("#Ten_HD").val(data + "-" + year + Ten_Hd.substr(-13));
                    }
                    else {
                        $("#Ten_HD").val(data + "-" + year + "-PTSC-QNG");
                    }
                }
                
            }
        });
    });
}