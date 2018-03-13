$(document).ready(function () {
   
    table = $("#reporttbl").DataTable({
        ajax: {
            url: '/Daily/Reports/Load',
            type: 'GET',
            dateType: 'json'
        },
        columnDefs: [
         { type: 'date-euro', targets: 0 }
 ],
        order: [[0, "desc"]],
        columns: [
            {
                data: 'Date', render: function (data) {
                    return moment(data).format("DD/MM/YYYY");
                }
            },
            {
                data: 'FullName'
            },
            {
                data: 'Total_Job'
            },
            {
                data: 'FullName_Autho1', render: function (data, type,row) {
                    return data === null ? (row.Status_Autho1 === false ? "<span class='label label-danger'>Chưa gửi</span>" : "<span class='label label-primary'>Đã gửi</span>"):data;
                }
            },
            {
                data: 'FullName_Autho2', render: function (data, type, row) {
                    return data === null ? (row.Status_Autho2 === false ? "<span class='label label-danger'>Chưa gửi</span>" : "<span class='label label-primary'>Đã gửi</span>") : data;
                }
            },
            {
                data: 'FullName_Autho3', render: function (data, type, row) {
                    return data === null ? (row.Status_Autho3 === false ? '<span class="label label-danger">Chưa gửi</span>' : '<span class="label label-warning">Đang chờ đánh giá</span>') : data;
                }
            },
            {
                data: 'Id', render: function (data) {
                    return "<button class='btn-sm btn-success' onclick='detail(" + data + ")' ><span class='fa fa-pencil-square-o'></span></button> | <button class='btn btn-danger' onclick='deletedata(" + data + ")' ><span class='fa fa-trash-o'></span></button> | <a href= '/Daily/Reports/DailyDetail?ma=" + data + "' class='btn btn-info' ><span class='fa fa-info-circle'></span></a >";
                }, orderable: false, width: '15%'
            }
        ]
    });
    var date = moment().format("YYYY-MM-DD");
    $("#FromTime").val(date);
    $("#ToTime").val(date);

});
function detail(ma) {
    $.ajax({
        url: '/Daily/Reports/Detail',
        data: { ma: ma },
        type: 'GET',
        success: function (data) {
            if (!data) {
                bootbox.alert("Ban Khong co quyen de lam cai nay");
            }
            else {
                $('#myModal').modal('show');
                $('#modalbody').html(data);
                var form = $('#reportform').closest('form');
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
    var form = $('#reportform').closest('form');
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);
    if (!$('#reportform').valid()) return false;
    $.ajax({
        url: '/Daily/Reports/Save',
        type: 'POST',
        data: $('#reportform').serialize(),
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
                $('#modalbody').html(data);
                var form = $('#reportform').closest('form');
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
function deletedata(ma) {
    bootbox.confirm("Bạn Muốn Xóa Cái Này", function (result) {
        if (result) {
            $.ajax({
                url: '/Daily/Reports/Delete',
                type: 'POST',
                data: { ma: ma },
                success: function (data) {
                    if (!data) {
                        bootbox.alert("Ban Khong co quyen de lam cai nay");
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
                    $.notify(ex, "error");
                }
            });
        }
    });
}
function ExportExcel() {
    var formtime = $("#FromTime").val();
    var totime = $("#ToTime").val();

    window.location="/Report/Reports/ExportGridDaily?FromTime=" + formtime + "&Totime=" + totime;
};