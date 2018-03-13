$(document).ready(function () {
    table = $("#dailytbl").DataTable({
        ajax: {
            url: '/Daily/Dailys/Load',
            type: 'GET',
            dateType: 'json',
            data: function (d) { d.Ma_TO = $("#MaTo").val(); d.Ma_BP = $("#MaPhong").val();}
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
                data: 'FullName_Autho1', render: function (data, type, row) {
                    return data === null ? (row.Status_Autho1 === false ? '<span class="label label-danger">Chưa gửi</span>' : '<span class="label label-warning">Đang chờ đánh giá</span>') : data;
                }
            },
            {
                data: 'FullName_Autho2', render: function (data, type, row) {
                    return data === null ? (row.Status_Autho2 === false ? '<span class="label label-danger">Chưa gửi</span>' : '<span class="label label-warning">Đang chờ đánh giá</span>') : data;
                }
            },
            {
                data: 'FullName_Autho3', render: function (data, type, row) {
                    return data === null ? (row.Status_Autho3 === false ? '<span class="label label-danger">Chưa gửi</span>' : '<span class="label label-warning">Đang chờ đánh giá</span>') : data;
                }
            },
            {
                data: 'Id', render: function (data) {
                    return "<a href= '/Daily/Dailys/Detail?ma=" + data + "' class='btn btn-info' ><span class='fa fa-info-circle'></span></a >";
                }, orderable: false, width: '5%'
            }
        ]
    });
    $("#MaTo").change(function () {
        table.ajax.reload();
    });
    $("#MaPhong").change(function () {
        var value = $(this).val();
        $.ajax({
            url: '/Daily/Dailys/LoadTeam',
            data: { Ma_BP: value },
            type: 'POST',
            success: function (data) {
                html = '<option value>-- Tất cả --</option>';
                $.each(data, function (i, item) {
                    html += '<option value="' + item.Ma_TO + '">' + item.Ten_TO + '</option>';
                });
                $("#MaTo").html(html);
                table.ajax.reload();
            }
        });
    });
});