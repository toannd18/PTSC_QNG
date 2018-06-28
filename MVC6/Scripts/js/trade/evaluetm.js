$(document).ready(function () {
    $("#spectable >tbody >tr").on("click", function () {
        $(this).addClass("info").siblings().removeClass("info");
    });
    tally('td.subtotal');
    $('#myModal').on("hidden.bs.modal", function (e) {
        if (parseInt($('#dem').val()) !== 0) {
            window.location.reload();
        }
        
    });
    $('[data-toggle="tooltip"]').tooltip();
});
//Nhập dữ liệu giá
function getdata() {
    var t = $("#spectable >tbody >tr").filter(".info").data("id");
    if (t === undefined) {
        bootbox.alert("Bạn chưa chọn dữ liệu");
        return false;
    }
    if (parseInt(t) > 0) {
        $.ajax({
            url: '/Commerce/Evaluations/LoadTM',
            data: { dg_kt: t, dx: $("#dxid").val() },
            type: 'Get',
            success: function (data) {
                $('#myModal').modal('show');
                $('#modalbody').html(data);
                $('#DeXuatId').val($("#dxid").val());
                $('#DG_KT_Id').val(t);
                var form = $('#dgtmtble').closest('form');
                form.removeData('validator');
                form.removeData('unobtrusiveValidation');
                $.validator.unobtrusive.parse(form);
                $('#DG_NCC_Id').change(function () {
                    if ($(this).val() > 0) {
                        detaildata($(this).val());
                    }
                });
            },
            error: function (err) {
                bootbox.alert("Lỗi kết nối");
            }
        });
    }
    else {
        $.ajax({
            url: '/Commerce/Evaluations/LoadKt',
            data: { dx: $("#dxid").val() },
            type: 'Get',
            success: function (data) {
                $('#myModal').modal('show');
                $('#modalbody').html(data);
                var form = $('#ncckttable').closest('form');
                form.removeData('validator');
                form.removeData('unobtrusiveValidation');
                $.validator.unobtrusive.parse(form);
                $("#idKT").hide();
                $('#Ma_NCC').on('change', function () {
                    detailncc($(this).val());
                   
                });
            },
            error: function (err) {
                bootbox.alert("Lỗi kết nối");
            }
        });
    }
}
function detaildata(ma) {
    $.ajax({
        url: '/Commerce/Evaluations/DetailTM',
        data: { NCC: ma, dg_kt: $('#DG_KT_Id').val() },
        type: 'Get',
        success: function (data) {
            Alert(true, true, "");
            if (data.status === false) {
                $('#Id').val(0);
                $('#Ten').val("");
                $('#Mo_Ta').val("");
             
                $('#DG').val("");
                return false;
            }
            $('#Id').val(data.Id);
            $('#DeXuatId').val(data.DeXuatId);
            $('#DG_KT_Id').val(data.DG_KT_Id);
            $('#Ten').val(data.Ten);
            $('#Mo_Ta').val(data.Mo_Ta);
            
            $('#Don_Gia').val(data.Don_Gia);

        },
        error: function (err) {
            $('#myModal').modal('hide');
            bootbox.alert("Lỗi kết nối");
        }
    });
}
function savedata() {
    var form = $('#dgtmtble').closest('form');
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);
    if (!form.valid()) {
        return false;
    }
    $.ajax({
        url: '/Commerce/Evaluations/SaveDG',
        data: { ma: $("#Id").val(), dg: $("#Don_Gia").val() },
        type: 'Post',
        success: function (data) {
           
            if (data.status === true) {
                Alert(true, false, "Lưu thành công");
            }
            else {
                Alert(false, false, "Lỗi không thể lưu");
            }
        },
        erro: function (err) {
            bootbox.alert("Lỗi kết nối");
        }

    });
}

//Nhập thông số yếu cầu
function gettm() {
    $.ajax({
        url: '/Commerce/Evaluations/LoadDGTM',
        data: { dx: $("#dxid").val() },
        type: 'Get',
        success: function (data) {
            $('#myModal').modal('show');
            $('#modalbody').html(data);
            $('#DeXuatId').val($("#dxid").val());
            var form = $('#tbltm').closest('form');
            form.removeData('validator');
            form.removeData('unobtrusiveValidation');
            $.validator.unobtrusive.parse(form);
            $('#DG_NCC_Id').change(function () {
                if ($(this).val() > 0) {
                    detailtm($(this).val());
                }
            });
        },
        error: function (err) {
            bootbox.alert("Lỗi kết nối");
        }
    });
}
function detailtm(ma){
    $.ajax({
        url: '/Commerce/Evaluations/DetailDGTM',
        data: { NCC: ma, dx: $('#DeXuatId').val() },
        type: 'Get',
        success: function (data) {
            Alert(true, true, "");
            if (data.status === false) {
                $('#Id').val(0);
                $('#Van_Chuyen').val("True");
                $('#BH').val("");
                $('#Hieu_Luc').val("");
                $('#Thoi_Gian').val("");
                $('#Dieu_Kien').val("");
                $('#Che_Do').val("");
                $('#Ghi_Chu').val("");
                $('#Dia_Diem').val("");
                return false;
            }
            $('#Id').val(data.Id);
            $('#DeXuatId').val(data.DeXuatId);
            $('#Van_Chuyen').val(data.Van_Chuyen?"True":"False");
            $('#BH').val(data.BH);
            $('#Hieu_Luc').val(data.Hieu_Luc);
            $('#Thoi_Gian').val(data.Thoi_Gian);
            $('#Dieu_Kien').val(data.Dieu_Kien);
            $('#Che_Do').val(data.Che_Do);
            $('#Ghi_Chu').val(data.Ghi_Chu);
            $('#Dia_Diem').val(data.Dia_Diem);

        },
        error: function (err) {
            $('#myModal').modal('hide');
            bootbox.alert("Lỗi kết nối");
        }
    });
}
function savetm() {
    var form = $('#tbltm').closest('form');
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);
    if (!form.valid()) {
        return false;
    }
    $.ajax({
        url: '/Commerce/Evaluations/SaveTM',
        data: form.serialize(),
        type: 'Post',
        success: function (data) {
            if (data.status === true) {
                Alert(true, false, "Lưu thành công");
            }
            else {
                Alert(false, false, "Lỗi không thể lưu");
            }
          
        },
        error: function (err) {
            bootbox.alert("Lỗi kết nối");
        }
    });
}

function detailncc(ma) {
    $.ajax({
        url: '/Commerce/Evaluations/Detail',
        data: { dx: $("#dxid").val(), ma_ncc: ma },
        type: 'Get',
        success: function (data) {
            $("#DG_TM").val(data.DG_TM);
            $("#DG_KT").val(data.DG_KT ? "True" : "False");
            Alert(true, true, "");
        },
        error: function (err) {
            $('#myModal').modal('hide');
            bootbox.alert("Lỗi kết nối");
        }
    });
}
function savencc() {
    var form = $('#ncckttable').closest('form');
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);
    if (!form.valid()) {
        return false;
    }
    $.ajax({
        url: '/Commerce/Evaluations/SaveNCCKT',
        data: form.serialize(),
        type: 'Post',
        success: function (data) {
            if (data.status === true) {
                Alert(true, false, "Lưu thành công");
            }
            else {
                Alert(false, false, "Lỗi không thể lưu");
            }
            
        },
        error: function (err) {
            bootbox.alert("Lỗi kết nối");
        }
    });
}
function tally(selector) {
    $(selector).each(function (index, item) {
        var total = 0,
        column = index;
        $(this).parents().prevUntil(':has(' + selector + ')').each(function () {
            total += parseInt($('td.sum:eq(' + column + ')', this).text().replace(/,/g,"")) || 0;
        });
        subtotal = total * 0.1;
        total = subtotal + total;
        $(this).html(format(subtotal.toFixed(2)));
        $('td.total:eq('+column+')').html(format(total.toFixed(2)));
    });
}
function format(x) {
    return isNaN(x) ? "" : x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

// Thông báo
function Alert(id, hiden, note) {
    var alert = $("#Alert");
    if (hiden === true) {
        alert.hide();
        return false;
    }
    alert.show();
    var t = parseInt($('#dem').val());
    $('#dem').val(t + 1);
    if (id === true) {
        alert.html(note);
        alert.removeClass("alert-danger").addClass("alert-success");
    }
    else {
        alert.html(note);
        alert.removeClass("alert-success").addClass("alert-danger");
    }
}