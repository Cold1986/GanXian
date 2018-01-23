$(function () {
    $('.btn-buy').click(function () {
        var receiver = $("#receiver").html();
        var phone = $('#Phone').html();
        var province = $('#province').html();
        var city = $('#city').html();
        var county = $('#county').html();
        var detailAddress = $('#detailAddress').html();
        var orderId = GetQueryString('orderId');

        if (receiver.trim() == "" || phone.trim() == ""
            || province.trim() == "" || city.trim == ""
            || detailAddress.trim() == "" || orderId.trim == "") {
            alert('请先填写收货地址');
            return false;
        }

        $.ajax({
            url: "PayOrder",
            data: {
                "receiver": receiver,
                "rPhone": phone,
                "province": province,
                "city": city,
                "county": county,
                "detailAddress": detailAddress,
                "orderId": orderId
            },
            type: 'post',
            async: false, //默认为true 异步   
            dataType: 'json',
            error: function (data) {
            },
            success: function (retData) {
                alert(retData);
                window.location.href = "../Order/OrderList";
            },
            complete: function () {
                $('.add-address').removeAttr("disabled"); //设置变灰按钮  
            }
        });
    })
})


function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}