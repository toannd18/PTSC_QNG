$(document).ready(function () {
    $("#treeview").shieldTreeView();
});

function detail(ma) {
    
    $.ajax({
        url: '/Functions/Detail',
        data: { ma: ma },
        type: 'GET',
        success: function (data) {
            $('#myModal').modal('show');
            $('.modal-body').html(data);
            if (ma === '') {
                $('#IsUpdate').val(false);
                $('#Id').prop('readonly', false);
            }
            else {
                $('#IsUpdate').val(true);
                $('#Id').prop('readonly', true);
            }
            var form = $('#functionform').closest('form');
            form.removeData('validator');
            form.removeData('unobtrusiveValidation');
            $.validator.unobtrusive.parse(form);
        }
    });
}
function savedata() {
    var form = $('#functionform').closest('form');
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);
    if (!$('#functionform').valid()) return false;

    $.ajax({
        url: '/Functions/Save',
        data: $('#functionform').serialize(),
        type: 'POST',
        success: function (data) {
            if (data.status) {
                $('#myModal').modal('hide');
                bootbox.alert('Bạn Lưu Thành Công', function () {
                    location.reload();
                });
                
            }
        },
        error: function (ex) {
            console.log(ex);
        }
    });
   
}
function deletedata(ma) {
    bootbox.confirm('Bạn muốn xóa cái này', function (data) {
        if (data) {
            $.ajax({
                url: '/Functions/Delete',
                type: 'POST',
                data: { ma: ma },
                success: function (data) {
                    if (data.status) {
                        bootbox.alert('Bạn Lưu Thành Công', function () {
                            location.reload();
                        });
                       
                    }
                    else {
                        $.notify("Lỗi Không Thể Thực Hiện", "error");
                        
                    }
                },
                error: function (ex) {
                    console.log(ex);
                    
                }
            });
           
        }
    });
    
}
function showpermission(functionid) {
    $.ajax({
        url: '/Functions/LoadPermission',
        data: {
            functionID: functionid
        },
        type: 'GET',
        success: function (data) {
            $('#myModal').modal('show');
            $('.modal-body').html(data);
        },
        error: function (ex) {
            console.log(ex);
        }
    });
}
function savepermission() {
    var form = $('#permissionform').closest('form');
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);
    if (!$('#permissionform').valid()) return false;

    var tbl = $('#permissionform').clone();
    var $checkbox = tbl.find('input[type=checkbox]');
    $checkbox.each(function () {
        if ($(this)[0].checked) {
            $(this).attr('type', 'hidden');
            $(this).val(true);
        } else {
            $(this).attr('type', 'hidden');
            $(this).val(false);
        }
    });
    var data = [];
    $.each(tbl.find('tbody>tr'), function () {
        var obj = {};
        $(this).find('input').each(function () {
            if ($(this).attr('name') !== '') 
                obj[$(this).attr('name')] = $(this).val();
            

        });
        data.push(obj);
    });
    var model = JSON.stringify(data);

    $.ajax({
        url: '/Functions/SavePermission',
        type: 'POST',
        cache:false,
        data: {
            data: model,
            FunctionId:$('#FunctionId').val()
        },
        success: function (res) {
            $('#myModal').modal('hide');
            if (res.status) {

                $.notify("Lưu Thành Công", "success");
            }
            else {
                $.notify("Lỗi Kết Nối", "error");
            }
        },
        error: function (ex) {
            bootbox.alert(ex);
        }
    });
}