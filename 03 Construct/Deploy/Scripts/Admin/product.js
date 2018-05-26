function editItem(productId) {
    $(".edit-item p").html("编辑产品");
    $("#r-productId").html(productId);
    $("#h-productId").val(productId);

    $.ajax({
        url: "GetProdInfo",
        data: {
            "productId": productId
        },
        type: 'post',
        async: false, //默认为true 异步   
        dataType: 'json',
        error: function (data) {

        },
        success: function (retData) {
            $('#r-product').val(retData.productName);
            $('#r-size').val(retData.specs);
            $('#r-price').val(retData.originalPrice);
            $('#r-oPrice').val(retData.discountedPrice);
            $('#r-cost').val(retData.cost);
            $('#r-APN').val(retData.origin);
            $('#r-weight').val(retData.nw);
            $('#r-condition').val(retData.storageCondition);
            $('#r-remark').val(retData.remark);

            $('.popup-inner input').prop('checked', false);
            $('#' + retData.showPic).prop('checked', true);
        },

    });

    $.ajax({
        url: "GetProd2tabs",
        data: {
            "productId": productId
        },
        type: 'post',
        async: false, //默认为true 异步   
        dataType: 'json',
        error: function (data) {

        },
        success: function (retData) {
            var tabsId = retData.split(',');

            for (var i = 0; i < tabsId.length; i++) {
                $('.popup input[type=checkbox]').each(function () {
                    if ($(this).val() == tabsId[i]) {
                        $(this).prop('checked', true);
                    }
                });
            }

        },

    });



    $(".edit-item").show();
}

function delItem(productId) {
    $('#delProductId').val(productId);
    $(".delete-alert").show();
}


function recoverItem(productId) {
    updateProductStatus(productId, 1);
}

function updateProductStatus(productId, status) {
    $.ajax({
        url: "updateProduct",
        data: {
            "productId": productId,
            "status": status
        },
        type: 'post',
        async: false, //默认为true 异步   
        dataType: 'json',
        error: function (data) {
            alert('产品状态修改失败');
            $(this).parents(".popup").hide();
        },
        success: function (retData) {
            alert("修改成功");
            window.location.reload();
        },

    });
}

function clearInput() {
    $('#r-productId').html('');
    $('.popup input[type=text]').val('');
    $('.popup input[type=hidden]').val('');
    $('.popup input[type=radio]').removeAttr('checked');
    $('.popup input[type=checkbox]').attr('checked', false);
}

$(function () {
    $('.main a').removeClass('focus');
    $('.main a').eq(0).addClass('focus');
    //clickPopup(".ion-trash-a", ".delete-alert");
    clickPopup(".add", ".add-item", addItem);
    function addItem() {
        clearInput();
        $(".add-item p").html("新增产品");
    }


    /* 获取焦点时变色 */
    $("input[type=text]").focus(function () {
        $(this).parent("td").addClass("highlight");
    })
    $("input[type=text]").blur(function () {
        $(this).parent("td").removeClass("highlight");
    })
    /* 设为主图时判断有没有图片 */
    $(".radio-box").click(function () {
        if (!$(this).prev(".r-pic").val()) {
            $(this).parents("li").find(".error-msg").show();
        }
    })
    $(".r-pic").change(function () {

        if ($(this).val()) {
            $(this).next(".radio-box").children("input[type='radio']").removeAttr("disabled");
            $(this).parents("li").find(".error-msg").hide();
        }
    });

    /* close popup */
    $(".popup-bg").on('click', function () {
        $(this).parents(".popup").hide();
    });

    $(".popup-cancel").on('click', function () {
        $(this).parents(".popup").hide();
        return false;
    });


    $(".delete-alert-confirm").on('click', function () {
        updateProductStatus($('#delProductId').val(), 0);
    })

    var options = {
        //target: '#output',          //把服务器返回的内容放入id为output的元素中    
        beforeSubmit: showRequest,  //提交前的回调函数
        success: showResponse,      //提交后的回调函数
        //url: url,                 //默认是form的action， 如果申明，则会覆盖
        //type: type,               //默认是form的method（get or post），如果申明，则会覆盖
        //dataType: null,           //html(默认), xml, script, json...接受服务端返回的类型
        clearForm: true,          //成功提交后，清除所有表单元素的值
        //resetForm: true,          //成功提交后，重置所有表单元素的值
        timeout: 3000               //限制请求的时间，当请求大于3秒后，跳出请求
    }

    function showRequest(formData, jqForm, options) {
        //formData: 数组对象，提交表单时，Form插件会以Ajax方式自动提交这些数据，格式如：[{name:user,value:val },{name:pwd,value:pwd}]
        //jqForm:   jQuery对象，封装了表单的元素   
        //options:  options对象
        var queryString = $.param(formData);   //name=1&address=2
        var formElement = jqForm[0];              //将jqForm转换为DOM对象
        console.log(formElement);
        var address = formElement.r_productId.value;  //访问jqForm的DOM元素

        var boolRadioCheck = false;
        $('.popup input[type=radio]').each(function () {
            if ($(this).prop('checked')) {
                boolRadioCheck = true;
            }
        });

        var r_product = formElement.r_product.value.trim();
        if (r_product == "") {
            alert('请填写产品名称');
            return false;
        }

        var r_price = formElement.r_price.value.trim();
        if (r_price == "") {
            alert('请填写原价');
            return false;
        }

        var r_oPrice = formElement.r_oPrice.value.trim();
        if (r_oPrice == "") {
            alert('请填写折后价');
            return false;
        }


        if (!boolRadioCheck) {
            alert('请选择一张图片做为显示图片');
            return false;
        }
        return true;  //只要不返回false，表单都会提交,在这里可以对表单元素进行验证
    };

    function showResponse(responseText, statusText) {
        alert(responseText._msg);
        window.location.href = window.location.href;
    };

    $('.add-item-confirm').on('click', function () {
        $('#formUpload').ajaxForm(options);
    })


    /* popup */
    function clickPopup(clickObj, popupWindow, fn) {
        fn = fn || function () { };
        $(clickObj).click(function () {
            $(popupWindow).show();
            fn();
        })
    }
})




