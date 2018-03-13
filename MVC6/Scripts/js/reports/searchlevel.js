$(document).ready(function () {
    table = $('#searchtbl').DataTable({
        columns: [
            { data: null, orderable: false, width: '3%' },
            { data: 'FullName' },
            { data: 'Ten_To' },
            { data: 'Level_1' },
            { data: 'Level_2' },
            { data: 'Level_3' },
            { data: 'Level_4' },
            { data: 'Level_5' }
        ],
        order: [1, 'asc']
    });
    table.on('order.dt search.dt', function () {
        table.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
});
function search() {
    var FromTime = $('#FromTime').val();
    var ToTime = $('#ToTime').val();
    var MaTo = $('#MaTo').val();
    if (FromTime === "") {
        FromTime = "1990-01-01";
    }
    if (ToTime === "") {
        ToTime = "2200-01-01";
    }  
    var url = '/Report/Reports/SearchLoad?FromTime=' + FromTime + '&ToTime=' + ToTime +'&Ma_TO='+   MaTo;
    
    table.ajax.url(url).load();
}