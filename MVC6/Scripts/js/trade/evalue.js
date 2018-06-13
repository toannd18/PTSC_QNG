$(document).ready(function () {
    $("#spectable >tbody >tr").on("click", function () {
        $(this).addClass("info").siblings().removeClass("info");
    });
    $('#myModal').on("hidden.bs.modal", function (e) {
        window.location.reload();
    });
});
function getdata() {
    var t = $("#spectable >tbody >tr").filter(".info").data("id");
    if (t === undefined) {
        bootbox.alert("Bạn chưa chọn dữ liệu");
        return false;
    }
    if (parseInt(t) > 0) {
        $.ajax({
            url: '/Commerce/Evaluations/LoadSpec',
            data: { dg_kt: t, dx: $("#dxid").val() },
            type: 'Get',
            success: function (data) {
                $('#myModal').modal('show');
                $('#modalbody').html(data);
                $('#DeXuatId').val($("#dxid").val());
                $('#DG_KT_Id').val(t);
                var form = $('#dgkttable').closest('form');
                form.removeData('validator');
                form.removeData('unobtrusiveValidation');
                $.validator.unobtrusive.parse(form);
                $('#DG_NCC_Id').change(function () {
                    if ($(this).val() > 0) {
                        detaildata($(this).val());
                    }
                });
            },
            error:function(err){
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
                $("#idTM").hide();
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
        url: '/Commerce/Evaluations/DetailSpec',
        data: { NCC: ma, dg_kt: $('#DG_KT_Id').val() },
        type: 'Get',
        success: function (data) {
            Alert(true, true, "");
            if (data.status === false) {
                $('#Id').val(0);
                $('#Ten').val("");
                $('#Mo_Ta').val("");
                $('#Ghi_Chu').val("");
                $('#DG').val("False");
                return false;
            }
                $('#Id').val(data.Id);
                $('#DeXuatId').val(data.DeXuatId);
                $('#DG_KT_Id').val(data.DG_KT_Id);
                $('#Ten').val(data.Ten);
                $('#Mo_Ta').val(data.Mo_Ta);
                $('#Ghi_Chu').val(data.Ghi_Chu);
                $('#DG').val(data.DG?"True":"False");
            
        },
        error: function (err) {
            $('#myModal').modal('hide');
            bootbox.alert("Lỗi kết nối");
            
        }
    });
}
function savedata() {
    var form = $('#dgkttable').closest('form');
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);
    if (!form.valid()) {
        return false;
    }
    $.ajax({
        url: '/Commerce/Evaluations/SaveSpec',
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
        erro: function (err) {
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
            $('#modalbody').html(data);
            var form = $('#ncckttable').closest('form');
            form.removeData('validator');
            form.removeData('unobtrusiveValidation');
            $.validator.unobtrusive.parse(form);
            $("#idTM").hide();
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

// Thông báo
function Alert(id, hiden, note) {
    var alert = $("#Alert");
    if (hiden === true) {
        alert.hide();
        return false;
    }
    alert.show();
    if (id === true) {
        alert.html(note);
        alert.removeClass("alert-danger").addClass("alert-success");
    }
    else {
        alert.html(note);
        alert.removeClass("alert-success").addClass("alert-danger");
    }
}