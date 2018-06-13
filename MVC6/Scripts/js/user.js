$(document).ready(function () {
    table = $('#usertbl').DataTable({
        ajax: {
            url: '/Users/LoadData',
            type: 'GET',
            dataType:'json'
        },
        order: [[ 0, "asc" ]],
        columnDefs: [
            { className: 'dt-body-center', targets: '_all' }
        ],
        columns: [
                       { data: 'UserName' },
            { data: 'FullName' },
            { data: 'Email' },
            { data: 'Ten_To' },
            { data: 'Ten_BP' },
            {
                data: 'Avatar', render: function (data) {
                    return "<img src='" + data + "' height='50' width='50'/>";
                }
            },
            {
                data: 'Id', render: function (data) {
                    return "<button class='btn-sm btn-success' onclick='detail(" + data + ")' ><span class='fa fa-pencil-square'></span></button> | <button class='btn btn-danger' onclick='deletedata(" + data + ")' ><span class='fa fa-trash-o'></span></button> | <button class='btn btn-info' onclick='detailrole(" + data + ")' ><span class='fa fa-lock'></span></button>";
                }
            }
        ]
    });
   
   
});
function detail(ma) {
    $.ajax({
        url: '/Users/Detail',
        type: 'GET',
        data: { ma: ma },
        success: function (data) {
                $('#myModal').modal('show');
                $('.modal-body').html(data);
                var form = $('#userform').closest('form');
                form.removeData('validator');
                form.removeData('unobtrusiveValidation');
                $.validator.unobtrusive.parse(form);
                if (ma > 0) {
                    $('#UserName').prop('readonly', true);
                }
                else
                    $('#UserName').prop('readonly', false);
           
        },
        error: function (ex) {
            $.notify(ex, "error");
        }
    });
}
function ShowPreviewImage(imageUpload, previewImage) {
    if (imageUpload.files && imageUpload.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImage).attr('src', e.target.result);
        };
        reader.readAsDataURL(imageUpload.files[0]);
    }
}
function save()
{
    var form = $('#userform').closest('form');
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);
    if ($('#Id').val() > 0) {
        $('#UserName').rules("remove");
    }
    if (!$('#userform').valid()) return false;
    var formdata = new FormData();
    var file = $('#ImageUpload').get(0).files;
    var parmas = $('#userform').serializeArray();
    $(parmas).each(function (i, index) {
        formdata.append(index.name, index.value);
    });
    formdata.append("uploadanh", file[0]);
    $.ajax({
        url: '/Users/Save',
        type: 'POST',
        data: formdata,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.status) {
                $('#myModal').modal('hide');
                $.notify("Lưu thành công", "success");
                table.ajax.reload();
            }
            else
                $.notify("Lỗi Không Thể Thuc Thi", "error");
        },
        error: function (ex) {
            $.notify(ex, "error");
        }
    });
}
function deletedata(ma) {
    bootbox.confirm('Bạn muốn xóa cái này', function (data) {
        if (data) {
            $.ajax({
                url: '/Users/Delete',
                type: 'POST',
                data: { ma: ma },
                success: function (data) {
                    if (data.status) {
                        $.notify("Xóa thành công", "success");
                        table.ajax.reload();
                    }
                    else
                        $.notify("Lỗi Không Thể Xóa", "error");
                },
                error: function (ex) {
                    $.notify(ex, "error");
                }
            });
        }
    });
}
function detailrole(ma) {
    $.ajax({
        url: '/Users/DetailRoleUser',
        type: 'Get',
        data: { ma: ma },
        success: function (data) {
            $('#myModal').modal('show');
            $('.modal-body').html(data);
        },
        error: function (ex) {
            $('#myModal').modal('hide');
            bootbox.alert(ex);
        }
    });
}
function saverole() {
    var form = new FormData();
    form.append("Id", $("#Id").val());
    form.append("UserId", $("#UserId").val());
    form.append("RoleId", $("#RoleId").val());
    $.ajax({
        url: '/Users/SaveRole',
        type: 'Post',
        data: form,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.status) {
                $('#myModal').modal('hide');
                $.notify("Lưu thành công", "success");
            }
            else {
                $('#myModal').modal('hide');
                $.notify("Lỗi Kết Nối", "error");
            }
        },
        error: function (ex) {
            bootbox.alert(ex);
        }
    });
}