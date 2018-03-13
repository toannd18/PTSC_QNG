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
                    return data ;
                }
            },
            { data: 'Ten_BP' },
            { data: 'Dia_Diem' },
            {
                data: 'Date', render: function (data) {
                    return moment(data).format("DD/MM/YYYY");
                }
            },
            { data: 'FullName' },
            {
                data: 'Status_Autho', render: function (data) {
                    if (data === "A") {
                        return "<span class='fa fa-check-circle' style='color:forestgreen'>Đã Duyệt</span>";
                    }
                    else if (data === "R") {
                        return "<span class='fa fa-times-circle' style='color:red'>Từ Chối</span>";
                    }
                    else if (data === "W") {
                        return "<span class='fa fa-check-circle' style='color:blue'>Đã Kiểm Tra Thực Tế</span>";
                    }
                    else {
                        return "<span class='fa fa-spinner' style='color:yellow'>Đang Chờ</span>";
                    }
                }, orderable: false
            },
            {
                data: 'Id', render: function (data) {
                    return "<a href= '/Approvals/Detail?ma=" + data +"' class='btn btn-info' ><span class='fa fa-info-circle'></span></a >";
                }, orderable: false, width: '5%'
            }
        ]
    });
});
