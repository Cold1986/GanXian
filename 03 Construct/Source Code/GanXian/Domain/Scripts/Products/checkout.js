$(function () {
    $('.btn-buy').click(function () {
        var receiver = $("#receiver").html();
        var phone = $('#Phone').html();
        var province = $('#province').html();
        var city = $('#city').html();
        var county = $('#county').html();
        var detailAddress = $('#detailAddress').html();
        var orderId = GetQueryString('orderId');
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
                if (retData == 'false') {
                    alert("购买失败，请稍后尝试");
                    return false;
                }
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