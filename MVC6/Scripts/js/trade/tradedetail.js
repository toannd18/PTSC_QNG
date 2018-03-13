$(document).ready(function () {
    table = $("#marterialtable").DataTable({
        ajax: {
            url: '/Commerce/Requests/LoadDetail',
            data:{ma:$("#ma").val()},
            type: 'Get',
            dataType:'Json'
        },
        columns: [
            {data: null,orderable:false,width:'1%'},
            {
                data: 'Ten', render: function (data, type, row) {
                    return data + '<br>' + row.Mo_Ta;
                }
            },
            { data:'DVT'},
            { data: 'SoLuong' },
            { data: 'Ghi_Chu' }

        ],
        select: {
            style:'Single'
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
            { data: 'DG_KT' },
            { data: 'DG_TM' },
            { data: 'DG' }

        ],
        select: {
            style: 'Single'
        }
    });
    table1.on('order.dt search.dt', function () {
        table1.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
    $('[data-toggle="tooltip"]').tooltip();
});
function getdata(url) {
    var ma = table.row('.selected').data();
    if (ma === undefined) {
        bootbox.alert("Bạn chưa chọn dữ liệu");
        return false;
    }
    getdetail(url,ma.Id);
}
function getdetail(url,ma) {
    $.ajax({
        url: url,
        data: { ma: ma },
        type: 'Get',
        success: function (data) {

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

function savedata(url, nametable,id) {
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