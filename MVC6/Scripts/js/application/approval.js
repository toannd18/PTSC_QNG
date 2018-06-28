$(document).ready(function () {
    table = $('#approtbl').DataTable({
        ajax: {
            url: '/Approvals/Load',
            type: 'Get',
            dataType: 'JSON'
        },
        columns: [
            {
                data: 'LateId', render: function (data) {
                    return data;
                }
            },
            { data: 'Ten_BP' },
            { data: 'Hang_Muc' },
            { data: 'Dia_Diem' },
            {
                data: 'Date', render: function (data) {
                    return moment(data).format("DD/MM/YYYY");
                }
            },
            { data: 'FullName' },
            {
                data: 'Date_Autho', render: function (data) {
                    return moment(data).format("DD/MM/YYYY");
                }
            },
            { data: 'FullName_1' },
            {
                data: 'Status_Autho', render: function (data) {
                    if (data === "A") {
                        return "<span class='fa fa-check-circle' style='color:forestgreen'>Đạt</span>";
                    }
                    else if (data === "R") {
                        return "<span class='fa fa-times-circle' style='color:red'>Không đạt</span>";
                    }
                    else if (data === "W") {
                        return "<span class='fa fa-check-circle' style='color:blue'>Đạt một phần</span>";
                    }
                    else {
                        return "<span class='fa fa-spinner' style='color:SlateBlue'>Đang Chờ</span>";
                    }
                }, orderable: false
            },
            {
                data: 'Id', render: function (data) {
                    return "<a href= '/Approvals/Detail?ma=" + data + "' class='btn btn-info' data-toggle='tooltip' title='Chi tiết'><span class='fa fa-info-circle'></span></a >";
                }, orderable: false, width: '5%'
            }
        ]
    });
    table.on('order.dt search.dt', function () {
        $('[data-toggle="tooltip"]').tooltip();
    }).draw();
});
