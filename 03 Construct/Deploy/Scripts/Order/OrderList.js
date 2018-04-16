$(function () {
    var status = GetQueryString('status');
    $('.order-panel a').removeClass('focus');
    if (status !=null && !isNaN(status )) {
        $('.order-panel a').eq(status / 1 + 1).addClass('focus');
    }
    else {
        $('.order-panel a').eq(0).addClass('focus');
    }
})


function deleteOrder(obj, OrderId) {
    if (confirm('确定要删除该订单么？')) {
        var _this = $(obj);


        $.ajax({
            url: "deleteOrder",
            data: {
                "orderId": OrderId
            },
            type: 'post',
            async: false, //默认为true 异步   
            dataType: 'json',
            error: function (data) {
                alert('订单删除失败');
            },
            success: function (retData) {
                _this.parent().parent().parent('.order-unpaid').hide('slow');
            },
            complete: function () {
                $('.add-address').removeAttr("disabled"); //设置变灰按钮  
            }
        });
    }
}

function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}