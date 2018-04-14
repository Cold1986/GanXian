$(function () {
    $('.main a').removeClass('focus');
    $('.main a').eq(2).addClass('focus');

    var status = GetQueryString('status');
    if (status != null) {
        $('#orderStatus').val(status);
    }

    $('.btn-search').click(function () {
        var orderNo = $('#orderNo').val().trim();
        var orderStatus = $('#orderStatus').val().trim()
        var createDate = $('#createDate').val().trim()
        var receiver = $('#receiver').val().trim()
        var expressNo = $('#expressNo').val().trim()

        var parameters = '?t=a';
        if (orderNo != '') {
            parameters += '&orderNo=' + orderNo;
        }
        if (orderStatus != '') {
            parameters += '&status=' + orderStatus;
        }
        if (createDate != '') {
            parameters += '&createDate=' + createDate;
        }
        if (receiver != '') {
            parameters += '&receiver=' + receiver;
        }
        if (expressNo != '') {
            parameters += '&expressNo=' + expressNo;
        }

        if (parameters != '?t=a') {
            window.location.href = "order" + parameters;
        }


    });

    $('.btn-clean').click(function () {
        $('#orderNo').val('');
        $('#orderStatus').val('all');
        $('#createDate').val('');
        $('#receiver').val('');
        $('#expressNo').val('');
    });
})

function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

function changeAmount(salesNo, obj) {
    if (obj.previousElementSibling.value.trim() != '') {
        change('changeAmount', salesNo, obj.previousElementSibling.value.trim());
    }

}

function changeExpress(salesNo, obj) {
    if (obj.previousElementSibling.value.trim() != '') {
        change('changeExpress', salesNo, obj.previousElementSibling.value.trim());
    }
}

function change(type, salesNo, amount) {
    $.ajax({
        url: "ChangePrice",
        data: {
            "type": type,
            "salesNo": salesNo,
            "price": amount
        },
        type: 'post',
        async: false, //默认为true 异步   
        dataType: 'json',
        error: function (data) {
            alert('修改失败，请联系管理员查看');
        },
        success: function (retData) {
            if (retData == "success") {
                alert('修改成功，请尽快联系买家付款');
            } else {
                alert('修改失败，请联系管理员查看');
            }
        },
    });
}

function returnProd(salesNo) {
    if (confirm("确定退货操作么？")) {
        $.ajax({
            url: "ChangeStatusReturn",
            data: {
                "salesNo": salesNo
            },
            type: 'post',
            async: false, //默认为true 异步   
            dataType: 'json',
            error: function (data) {
                alert('修改失败，请联系管理员查看');
            },
            success: function (retData) {
                if (retData == "success") {

                } else {
                    alert('修改失败，请联系管理员查看');
                }
            },
        });
    }
}

//clickPopup(".ion-trash-a", ".delete-alert");
//clickPopup(".ion-logistics", ".delivery-alert");
//clickPopup(".ion-ios-undo", ".return-alert");
$(".order-unpaid").next().find(".edit-price p").show();
var addShipping = '<p class="blue"><input type="text" placeholder="请输入运单号"></p>';
$(".add-shipping").click(function () {
    $(".blue:last").after(addShipping);
})