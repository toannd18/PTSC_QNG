var ptsc = {
    notify: function (messages, value) {
        $.notify(message, value);
    },
    confirm: function (messages, okCallBack) {
        bootbox.confirm({
            message: messages,
            buttons: {
                confirm: {
                    label: 'Có',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'Hủy',
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                if (result === true) {
                    okCallBack();
                }
            }
        });
    },
    datetimeFormat: function (datetime) {
        datetime.datepicker({
            dateFormat: "dd/mm/yy"
        });
    },
    formatNumber: function (number, precision) {
        if (!isFinite(number)) {
            return number.toString();
        }

        var a = number.toFixed(precision).split('.');
        a[0] = a[0].replace(/\d(?=(\d{3})+$)/g, '$&,');
        return a.join('.');
    },

    loadanimate: function (data) {
        var scrollpos;
        if (data === 'home') {
            scrollpos = 0;
        }
        else {
            scrollpos = $(data).offset().top;
        }

        $("html, body").animate({ scrollTop: scrollpos }, "500");
    }
};

//$(document).ajaxSend(function (e, xhr, options) {
//    if (options.type.toUpperCase() == "POST" || options.type.toUpperCase() == "PUT") {
//        var token = $('form').find("input[name='__RequestVerificationToken']").val();
//        xhr.setRequestHeader("RequestVerificationToken", token);
//    }
//});