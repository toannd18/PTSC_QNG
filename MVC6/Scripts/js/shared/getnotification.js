$(document).ready(function () {

    var hub = $.connection.notificationHub;

    hub.client.displayStatus = function () {

        getnotification();
        pushNotification($("#count").text());
    };
    $.connection.hub.start();
    if (Notification.permission === "default") {
        Notification.requestPermission();
    };
    getnotification();
    function getnotification() {
        var divNotification = $("#Notification");
        $.ajax({
            url: '/Home/GetNotification',
            type: 'Get',
            success: function (data) {

                divNotification.empty();
                $("#count").text("");
                if (data.length > 0) {
                    divNotification.append('<li><div class="notification_header" >' +
                        '<h3 >Bạn có ' + data.length + ' thông báo</h3></div ></li>');
                    $("#count").text(data.length);
                    for (var i = 0; i < data.length; i++) {
                        var notification = ' <li">' +
                            '<a href= "' + data[i].Url + '" >' +
                            '<div class="notification_desc">' +
                            '<p>' + data[i].Notification + '</p>' +
                            '<p><span>' + moment(data[i].Date).format("DD/MM/YYYY HH:mm") + '</span></p>' +
                            '</div>' +
                            '<div class="clearfix"></div></a></li>';
                        divNotification.append(notification);
                        if (i === 2) {

                            break;
                        }
                    };

                } else {
                    var notification = ' <li">' +
                        '<a href= "#" >' +
                        '<div class="notification_desc">' +
                        '<p> Chưa có tinh nhắn mới</p>' +

                        '</div>' +
                        '<div class="clearfix"></div></a></li>';
                    divNotification.append(notification);
                }
                var notification = '<li>' +
                    '<div class="notification_bottom">' +
                    '<a href="/Home/Notification">Xem tất cả tin nhắn</a>'
                '</div>' +
                    '</li>';
                divNotification.append(notification);
            }
        })
    }
});
function pushNotification(data) {
    if (Notification.permission === "granted") {
        if (parseInt(data) > 0) {
            var notifi = new Notification("Thông báo", {

                body: $("#Notification p:first").text() + " Ngày gửi " + $("#Notification span:first").text(),
                icon: "/images/Icon.jpg",
            });
            notifi.onclick = function () {

                window.open("/Home/Notification");
            };
        }
    }

}