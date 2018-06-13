$(document).ready(function () {
    table = $("#marterialtable").DataTable({
        ajax: {
            url: '/Commerce/Requests/LoadDetail',
            data: { ma: $("#ma").val() },
            type: 'Get',
            dataType: 'Json'
        },
        columns: [
            { data: null, orderable: false, width: '1%' },
            {
                data: 'Ten', render: function (data, type, row) {
                    return data + '<br>' + row.Mo_Ta;
                }
            },
            { data: 'DVT' },
            { data: 'SoLuong' },
            { data: 'Ghi_Chu' }

        ],
        select: {
            style: 'single'
        }
    });
    table.on('order.dt search.dt', function () {
        table.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
    table1 = $("#ncctable").DataTable({
        ajax: {
            url: '/Commerce/Requests/LoadNCC',
            data: { dx: $("#ma").val() },
            type: 'Get',
            dataType: 'Json'
        },
        columns: [
            { data: null, orderable: false, width: '1%' },
            {
                data: 'Ten_NCC'
            },
            {
                data: 'DG_KT', render: function (data) {
                    return data === true ? "Đạt" : "Không đạt";
                }
            },
            { data: 'DG_TM' },
            {
                data: 'DG', render: function (data,type,row) {
                    return "<input type='Number' data-id='" + row.Id + "' value='" + data + "' class='DG'>";
                }
            }

        ],
        select: {
            style: 'single'
        }
    });
    table1.on('order.dt search.dt', function () {
        table1.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
        $(".DG").change(function () {
            var ma = parseInt($(this).data("id"));
            var data =parseInt($(this).val());
            savedg(ma, data);
        });
    }).draw();
    $('[data-toggle="tooltip"]').tooltip();
   
});

//Tiêu chí kỹ thuật
function getdata(url) {
    var ma = table.row('.selected').data();
    if (ma === undefined) {
        bootbox.alert("Bạn chưa chọn dữ liệu");
        return false;
    }
    getdetail(url, ma.Id);
}
function getdetail(url, ma) {
    $.ajax({
        url: url,
        data: { ma: ma },
        type: 'Get',
        success: function (data) {
            if (data === "Error") {
                bootbox.alert("Bạn không có quyền làm điều này");
                return false;
            }
            $('#myModal').modal('show');
            $('#modalbody').html(data);
            $('#DeXuatId').val($('#ma').val());
            var form = $('#dxchitietform').closest('form');
            form.removeData('validator');
            form.removeData('unobtrusiveValidation');
            $.validator.unobtrusive.parse(form);
        },
        error: function (err) {
            $.notify("Lỗi Kết Nối", "error");
        }
    });
}

function savedata(url, nametable, id) {
    console.log(id);
    var data = $(id).parents("form");
    var form = $(data).closest('form');
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);
    if (!form.valid()) {
        return false;
    }
    $.ajax({
        url: url,
        data: data.serialize(),
        type: 'Post',
        success: function (res) {
            if (res.status === true) {
                $('#myModal').modal('hide');
                $.notify("Lưu thành công", "success");
                if (nametable === "table") {
                    table.ajax.reload();
                }
            }
            else if (res.status === false) {
                $('#myModal').modal('hide');
                $.notify("Lỗi Kết Nối", "error");
            }
        }
    });
}
function deletedata(url, nametable) {
    if (nametable === "table") {
        var ma = table.row('.selected').data();
    }
    if (ma === undefined) {
        bootbox.alert("Bạn chưa chọn dữ liệu");
        return false;
    }
    bootbox.confirm("Ban muốn xóa mục " + ma.Ten + " này", function (result) {
        if (result) {
            $.ajax({
                url: url,
                type: 'Post',
                data: { ma: ma.Id },
                success: function (data) {
                    if (data.status === true) {
                        $('#myModal').modal('hide');
                        $.notify("Xóa thành công", "success");
                        if (nametable === "table") {
                            table.ajax.reload();
                        }
                    }
                    else if (data==="Error") {
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

//Lựa chọn nhà cung cấp
function loadncc() {
    $.ajax({
        url: '/Commerce/Requests/SelectNCC',
        type: 'Get',
        success: function (data) {
            if (data === "Error") {
                bootbox.alert("Ban Không có quyền để làm điều này");
                return false;
            }
            $('#myModal_1').modal('show');
            $('#modalbody_1').html(data);
            $('#sel1').val('0');
            tablencc = $('#selectable').DataTable({
                ajax: {
                    url: '/Commerce/Requests/ListNCC',
                    type: 'Get',
                    dataType: 'Json',
                    data: function (d) { d.ma = parseInt($('#sel1').val()); }
                },
                columns: [
                    { data: 'Ten_NCC' },
                    { data: 'Hang_Hoa' },
                    { data: 'Dich_Vu' },
                    { data: 'Diem' }
                ],
                select: {
                    style: 'single'
                }
            });
        }
    });
}
$(document).ajaxComplete(function () {
    $('#sel1').change(function () {
        console.log($(this).val());
        if ($(this).val() === '1') {
            tablencc.column(2).visible(false);
            tablencc.column(1).visible(true);
            tablencc.ajax.reload();
        }
        else if ($(this).val() === '2') {
            tablencc.column(1).visible(false);
            tablencc.column(2).visible(true);
            tablencc.ajax.reload();
        }
        else {
            tablencc.column(1).visible(true);
            tablencc.column(2).visible(true);
            tablencc.ajax.reload();
        }
    });
});
function takedata() {
    var ma = tablencc.row('.selected').data();
    if (ma === undefined) {
        bootbox.alert("Bạn chưa chọn dữ liệu");
        return false;
    }
    var data = {
        Id: 0,
        DeXuatId: $('#ma').val(),
        Ma_NCC: ma.Ma_NCC
    };
    $.ajax({
        url: '/Commerce/Requests/DetailNCC',
        type: 'Post',
        data: data,
        success: function (d) {
            $('#myModal_1').modal('hide');
            if (d) {
                $.notify("Lưu thành công", "success");
                table1.ajax.reload();
            }
            else {
                $.notify("Lỗi Không Thể Xóa", "error");
            }
        }
    });
}
function deletencc() {
    var ma = table1.row('.selected').data();
    if (ma === undefined) {
        bootbox.alert("Bạn chưa chọn dữ liệu");
        return false;
    }
    bootbox.confirm("Ban muốn xóa nhà cung cấp " + ma.Ten_NCC, function (result) {
        if (result) {
            $.ajax({
                url: '/Commerce/Requests/DeleteNCC',
                type: 'Post',
                data: { Id: ma.Id },
                success: function (data) {
                    if (data.status === true) {
                        $('#myModal').modal('hide');
                        $.notify("Xóa thành công", "success");
                        table1.ajax.reload();
                    }
                    else if (data==="Error") {
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

//Tiêu chí thương mại
function loadtm() {
    $.ajax({
        url: '/Commerce/Requests/ThuongMai',
        data: { dx: $("#ma").val() },
        type: 'Get',
        success: function (data) {
            if (data === "Error") {
                bootbox.alert("Bạn không có quyền làm việc này");
                return false;
            }
            $('#myModal').modal('show');
            $('#modalbody').html(data);
            $('#DeXuatId').val($('#ma').val());
            var form = $('#tmform').closest('form');
            form.removeData('validator');
            form.removeData('unobtrusiveValidation');
            $.validator.unobtrusive.parse(form);
        }
    });
}
function savetm() {
    var form = $('#tmform').closest('form');
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);
    if (!form.valid()) {
        return false;
    }
    $.ajax({
        url: '/Commerce/Requests/SaveTM',
        data: form.serialize(),
        type: 'Post',
        success: function (data) {
            $('#myModal').modal('hide');
            if (data === true) {
                $('#TM_1').html($('#Loai_Tien').val());
                $('#TM_2').html($('#Hieu_Luc').val());
                $('#TM_3').html($('#Thoi_Gian').val());
                $('#TM_4').html($('#Dia_Diem').val());
                $('#TM_5').html($('#Dieu_Kien').val());
                $('#TM_6').html($('#BH').val());
                $('#TM_7').html($('#Che_Do').val());
                $('#TM_8').html($('#Ghi_Chu').val());
                $.notify("Lưu thành công", "success");
            }
            else {
                $.notify("Lỗi kết nối", "error");
            }
        },
        error: function (err) {
            bootbox.alert("Lỗi máy chủ");
        }
    });
}

function loadanimate(data) {
    var scrollpos;
    if (data === 'home') {
        scrollpos = 0;
    }
    else {
        scrollpos = $(data).offset().top;
    }
    
    $("html, body").animate({ scrollTop: scrollpos }, "500");
}

function savedg(ma, data) {
    $.ajax({
        url: "/Commerce/Requests/DGChung",
        data: { ma: ma, data: data },
        type: "Post",
        success: function (data) {
            if (data === true) {
                $.notify("Lưu thành công", "success");
            }
            else if (data === "Error") {
                bootbox.alert("Ban Không có quyền để làm điều này");
            }
            else {
                $.notify("Lỗi kết nối", "error");
            }
        },
        error: function (err) {
            $.notify("Lỗi máy chủ", "error");
        }
    });
}