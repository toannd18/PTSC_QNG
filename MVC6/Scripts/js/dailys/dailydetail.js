$(document).ready(function () {
    table = $("#dailytable").DataTable({
        ajax: {
            url: '/Daily/Reports/LoadDaily',
            data:{ma:$("#Id").val()},
            type: 'GET',
            dateType: 'json'
        },
       
        columns: [
            {
                data: 'FormTime', render: function (data) {
                    return moment(data).format("HH:mm");
                }
            },
            {
                data: 'ToTime', render: function (data) {
                    return moment(data).format("HH:mm");
                }
            },
            {
                data: 'Content_Job', width: '15%'
            },
            {
                data: 'Method', render: function (data,type) {
                    return data !== null ? '<p style="white-space:pre-wrap">' + data + '</p>' : data;
                }, width: '20%'
            },
            {
                data: 'Result',width:'8%'
            },
            {
                data: 'Total_Job', orderable: false,width:'15%'
            },
            {
                data: 'Comment1', render: function (data, type, row) {
                    return (row.Level_1 === null ? "" : "- Mức " + row.Level_1) + (data === null ? "" :(data===""?"": " (" + data + ")"));
                }, width: '10%'
                    
            },
            {
                data: 'Comment2', render: function (data, type, row) {
                    return (row.Level_2 === null ? "" : "- Mức " + row.Level_2) + (data === null ? "" : (data === "" ? "" : " (" + data + ")"));
                },width:'10%'
            },
            {
                data: 'Comment3', render: function (data, type, row) {
                    return (row.Level_3 === null ? "" : "- Mức " + row.Level_3) + (data === null ? "" : (data === "" ? "" : " (" + data + ")"));
                }, width: '10%'
            }
        ],
        rowsGroup: [5],
        select: {
            style:'single'
        }
    });
    $('[data-toggle="tooltip"]').tooltip(); 
    jQuery.validator.addMethod("timeValidator",
        function (value, element, params) {
            var val = new Date('1/1/1991' + ' ' + value);
            var par = new Date('1/1/1991' + ' ' + $(params).val());
            return val > par;
           
        }, 'Thời gian đến phải lớn hơn thời gian bắt đầu');
});

function detail(mad) {
    $.ajax({
        url: '/Daily/Reports/DetailReport',
        data: { mad: mad, ma: $("#Id").val() },
        type: 'GET',
        success: function (data) {
            if (!data) {
                bootbox.alert("Ban Khong co quyen de lam cai nay");
            }
            else {
                $('#myModal').modal('show');
                $('.modal-body').html(data);
                var form = $('#dailyform').closest('form');
                form.removeData('validator');
                form.removeData('unobtrusiveValidation');
                $.validator.unobtrusive.parse(form);
                $("#ToTime").rules('add', { timeValidator: "#FormTime" });
            }
        },
        error: function (ex) {
            $.notify(ex, "error");
        }
    });
}

function save() {
    var form = $('#dailyform').closest('form');
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);
    $("#ToTime").rules('add', { timeValidator: "#FormTime" });
    if (!$('#dailyform').valid()) return false;
    $.ajax({
        url: '/Daily/Reports/SaveDaily',
        type: 'POST',
        data: $('#dailyform').serialize(),
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
                var form = $('#dailyform').closest('form');
                form.removeData('validator');
                form.removeData('unobtrusiveValidation');
                $.validator.unobtrusive.parse(form);
                $("#ToTime").rules('add', { timeValidator: "#FormTime" });
            }

        },
        error: function (ex) {
            $.notify(ex, "error");
        }
    });
}
function getdata() {
    var ma = table.row('.selected').data();
    if (ma===undefined) {
        bootbox.alert("Bạn chưa chọn dữ liệu");
        return false;
    }
    detail(ma.Id);
}
function deletedata() {
    var ma = table.row('.selected').data();
    var data = table.cells('.selected', '').render('display');
    if (ma === undefined) {
        bootbox.alert("Bạn chưa chọn dữ liệu");
        return false;
    }
    bootbox.confirm("Ban muốn xóa từ " + data[0] + " tới " + data[1] + " này", function (result) {
        if (result) {
            $.ajax({
                url: '/Daily/Reports/DeleteDaily',
                type: 'Post',
                data: { ma: ma.Id, mad: ma.DailyId },
                success: function (data) {
                    if (data.status) {
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
function sendrequest() {
    if ($("#permission").val() < 3) {
        bootbox.alert("Bạn không có quyền");
        return false;
    };
    bootbox.confirm("Bạn muốn gửi dữ liệu", function (result) {
        if (result) {
            $.ajax({
                url: '/Daily/Reports/SendRequest',
                data: { ma: $('#Id').val() },
                type: 'POST',
                success: function (data) {
                    if (data.status) {
                        $.notify("Gửi thành công", "success");
                    }
                    else if (data.status===false) {
                        $.notify("Lỗi kết nối", "error");
                    }
                    else if(data===false) bootbox.alert("Phiếu kiểm tra này đã được gửi");
                },
                error: function (ex) {
                    bootbox.alert(ex);
                }
            });
        }
    });
}
function detailcomment() {
    if ($("#permission").val() < 3) {
        bootbox.alert("Bạn không có quyền");
        return false;
    };
    var ma = table.row('.selected').data();
    if (ma === undefined) {
        bootbox.alert("Bạn chưa chọn dữ liệu");
        return false;
    }
    check = $("#permission").val();
        $.ajax({
            url: '/Daily/Dailys/CommentDetail',
            data: { mad: ma.Id, ma: $("#Id").val(), check: check },
            type: 'GET',
            success: function (data) {
                if (!data) {
                    bootbox.alert("Phiếu kiểm tra này đã được gửi");
                } else {
                    $('#myModal').modal('show');
                    $('.modal-body').html(data);
                }
            },
            error: function (ex) {
                bootbox.alert(ex);
            }
        });
}
function savecomment() {
    var check = parseInt($("#permission").val());
    if (check === 3) {
        comment = $("#commentform #Comment1").val();
        level = $("#commentform #Level_1").val();
    }
    else if (check===4) {
        comment = $("#commentform #Comment2").val();
        level = $("#commentform #Level_2").val();
    }
    else {
        comment = $("#commentform #Comment3").val();
        level = $("#commentform #Level_3").val();
    }
    $.ajax({
        url: '/Daily/Dailys/Comment',
        data: { ma: $("#commentform #Id").val(), comment: comment, level: level,check:check },
        type: 'POST',
        success: function (data) {
            $('#myModal').modal('hide');
            if (data) {

                $.notify("Lưu thành công", "success");
                table.ajax.reload();
            }
            else
                $.notify("Lỗi Kết Nối", "error");
        },
        error: function (ex) {
            bootbox.alert(ex);
        }
    });
}
function savecommentall() {
    $.ajax({
        url: '/Daily/Dailys/CommentAll',
        data: { ma: $("#Id").val(), comment: $("#commentform #Comment").val(), level: $("#commentform #Level").val(), check: $("#permission").val() },
        type: 'POST',
        success: function (data) {
            $('#commentmodal').modal('hide');
            if (data.status===true) {

                $.notify("Lưu thành công", "success");
                table.ajax.reload();
            }
            else if (data.status===false) {
                $.notify("Lỗi Kết Nối", "error");
            }  
            else {
                bootbox.alert("Không thể thực hiện tác vụ này");
            }
                
        },
        error: function (ex) {
            bootbox.alert(ex);
        }
    });
}
function detailcommentall() {
    if ($("#permission").val() < 3) {
        bootbox.alert("Bạn không có quyền");
        return false;
    };
    $("#commentmodal").modal("show");
   
    $("#Comment").val("");
    $("#Level").val(3);
}