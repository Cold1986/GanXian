$(document).ready(function () {
    //返回顶部
    $(window).scroll(function () {
        if ($(this).scrollTop() > 300) {
            $(".fanhui_cou").fadeIn(1500);

        } else {
            $(".fanhui_cou").fadeOut(1500);

        }
    });
    $(".fanhui_cou").click(function () {
        $("body,html").animate({ scrollTop: 0 }, 200);
        return false;
    });

    //勾选
    $('.checkLabel').on('click', function () {
        var flag = $(this).prev().is(':checked');
        if (flag) {
            $(this).prev().prop("checked", false);

            $("#buy-sele-all").prop("checked", false); //将全选勾除
            $("#buy-sele-all").val(0);

        } else {

            $(this).prev().prop("checked", true);

            //如果全部都选中了，将全选勾选
            var groupUL = $(".container").find("ul[class='list-group']").get();
            var checkUL = $(".container").find("div[class='icheck pull-left mr5'] :checkbox:checked").get();
            if (groupUL.length == checkUL.length) {
                $("#buy-sele-all").prop("checked", true);
                $("#buy-sele-all").val(1);
            }
        }

        //计算总价
        calculateTotal();
    });

    // 全选，全不选
    $("#buy-sele-all").on('click', function () {
        var flag = $(this).val();

        if (flag == 1) {
            $(this).val(0);
            $(".ids").prop("checked", false);
        } else {
            $(this).val(1);
            $(".ids").prop("checked", true);
        }

        //计算总价
        calculateTotal();
    });

    //计算总价
    calculateTotal();

    $(".rowcar .num-edit i").on('click', function () {
        $(this).parents("li").addClass("editnow");
    })
    //$(".rowcar .num-edit span").on('click', function () {
    //    $(this).parents("li").removeClass("editnow");
    //    var num = $(this).parent().siblings('.btn-group').children('.gary2').val();
    //    $(this).parent().siblings('.num-show').text('x ' + num);
    //})
});

function updateProductNum(obj, productId) {
    var _this = $(obj);
    _this.parents("li").removeClass("editnow");
    var num = _this.parent().siblings('.btn-group').children('.gary2').val();
    _this.parent().siblings('.num-show').text('x ' + num);
    if (!isNaN(num) && num > 0) {
        $.ajax({
            url: "UpdateShopcartById",
            data: { "productId": productId, "num": num },
            type: 'post',
            async: true, //默认为true 异步   
            dataType: 'json',
            error: function (data) {
            },
            success: function (data) {
                if (data == "success") {
                    alert("更新成功");
                    return;
                } else {
                    alert("更新失败");
                    return false;
                }
            }
        });
    }
}

//相加
function increase(obj) {
    var _this = $(obj);
    var _count_obj = _this.prev();
    var count = Number($(_count_obj).val());
    var basket_id = $(_count_obj).prop("itemkey");
    var prod_id = $(_count_obj).prop("prodId");
    var sku_id = $(_count_obj).prop("skuId");

    var _num = parseInt(count) + 1;
    var re = /^[1-9]+[0-9]*]*$/;
    if (isNaN(_num) || !re.test(_num)) {
        return;
    } else if (_num == 9999) {
        return;
    }

    $(_count_obj).val(count * 1 + 1);
    var cash = $(_this).parent().parent().prev().find("em[class='price']").html().substring(1);//单价
    var total_cash = $(_this).parent().prev().find("em[class='red productTotalPrice']").html().substring(1);//商品小计

    var e_cash = Math.round((Number(total_cash) + Number(cash)) * 100) / 100;
    var pos_decimal = e_cash.toString().indexOf('.');
    if (pos_decimal < 0) {
        e_cash += '.00';
    }
    $(_this).parent().prev().find("em").html("¥" + e_cash);

    //计算总价
    calculateTotal();


}

//减
function disDe(obj) {
    var _this = $(obj);
    var _count_obj = _this.next();
    var count = Number($(_count_obj).val());
    var basket_id = $(_count_obj).prop("itemkey");
    var prod_id = $(_count_obj).prop("prodId");
    var sku_id = $(_count_obj).prop("skuId");
    var _num = parseInt(count) - 1;

    var re = /^[1-9]+[0-9]*]*$/;
    if (isNaN(_num) || !re.test(_num)) {
        return;
    } else if (_num == 0) {
        return;
    }

    $(_count_obj).val(count * 1 - 1);
    var cash = $(_this).parent().parent().prev().find("em[class='price']").html().substring(1);//单价
    var total_cash = $(_this).parent().prev().find("em[class='red productTotalPrice']").html().substring(1);//商品小计		
    var e_cash = Math.round((Number(total_cash) - Number(cash)) * 100) / 100;
    var pos_decimal = e_cash.toString().indexOf('.');
    if (pos_decimal < 0) {
        e_cash += '.00';
    }
    $(_this).parent().prev().find("em").html("¥" + e_cash);

    //计算总价
    calculateTotal();

}
//更新购物车商品数量
function changeShopCartNumber(_basketId, _num, _prodId, _skuId, type) {
    var config = false;
    $.ajax({
        url: contextPath + "/changeShopCartNum",
        data: { "num": _num, "basketId": _basketId, "prodId": _prodId, "skuId": _skuId, "type": type },
        type: 'post',
        async: false, //默认为true 异步   
        dataType: 'json',
        error: function (data) {
        },
        success: function (result) {
            if (result.status == "OK") {
                config = true;
            } else if (result.fail == "RESTRICTION") {
                floatNotify.simple("超出购买限制");
            } else if (result.fail == "ERR") {
                floatNotify.simple("更新失败");
            } else if (result.fail == "PROD_RESTRICTION") {
                floatNotify.simple("商品超出购买限制");
            } else if (result.fail == "PARAM_ERR") {
                floatNotify.simple("参数有误");
            }
        }
    });
    return config;
}

//计算总价
function calculateTotal() {
    var allCash = 0;
    var list = $(".container").find("ul[class='list-group']").get();
    for (var i = 0; i < list.length; i++) {
        var selected = $(list[i]).find("div[class='icheck pull-left mr5']>:checkbox").is(":checked");
        if (selected) {
            var cash = $(list[i]).find("em[class='price']").html().substring(1);//取单价
            var count = $(list[i]).find("input[class='btn gary2 Amount']").val();//取数量
            allCash += Number(cash) * Number(count);
        }
    }

    allCash = Math.round(Number(allCash) * 100) / 100;
    var pos_decimal = allCash.toString().indexOf('.');
    if (pos_decimal < 0) {
        allCash += '.00';
    }
    $("#totalPrice").html(allCash);
}

//删除购物车商品
function deleteShopCart(productId, productName) {
    if (confirm("删除后不可恢复, 确定要删除'" + productName + "'吗？")) {
        $.ajax({
            url: "DelShopcartById",
            data: { "productId": productId },
            type: 'post',
            async: true, //默认为true 异步   
            dataType: 'json',
            error: function (data) {
            },
            success: function (data) {
                if (data == "success") {
                    window.location.href = window.location.href;
                    return;
                } else {
                    alert("删除失败");
                    return false;
                }
            }
        });
    }
}


function submitShopCart() {

    var array = $(".ids:checked").get();
    if (array.length == 0) {
        floatNotify.simple("请选择要结算的商品");
        return;
    }

    var shopCartStr = "";
    for (var i in array) {
        if (i != 0) {
            shopCartStr = shopCartStr + ",";
        }
        var basket_id = $(array[i]).prop("itemkey");
        shopCartStr = shopCartStr + basket_id;
    }

    //调用方法  
    abstractForm(contextPath + '/p/orderDetails', shopCartStr);
}

function abstractForm(URL, shopCartIds) {
    var temp = document.createElement("form");
    temp.action = URL;
    temp.method = "post";
    temp.style.display = "none";
    var opt = document.createElement("textarea");
    opt.name = 'shopCartItems';
    opt.value = shopCartIds;
    temp.appendChild(opt);
    document.body.appendChild(temp);
    temp.submit();
    return temp;
}