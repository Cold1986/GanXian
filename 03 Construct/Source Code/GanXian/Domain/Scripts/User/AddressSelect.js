function SelAddress(receiver, phone, province, city, county, detailAddress, fromURL) {
    console.log(receiver);
    console.log(phone);
    console.log(province);
    console.log(city);
    console.log(county);
    console.log(detailAddress);
    console.log(fromURL);
    var orderId = fromURL.split('=')[1];
    console.log(orderId);

    $.ajax({
        url: "SelAddress",
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
            alert("系统维护中，请稍后再试")
        },
        success: function (retData) {
            window.location.href = fromURL;
        },
        complete: function () {
           
        }
    });
}