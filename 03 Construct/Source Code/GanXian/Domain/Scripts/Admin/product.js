function editItem(productId) {
    $(".edit-item p").html("编辑产品");
    $("#r-productId").html(productId);
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

$(function () {
    //clickPopup(".ion-trash-a", ".delete-alert");
    clickPopup(".add", ".add-item", addItem);
    function addItem() {
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
    });


    $(".delete-alert-confirm").on('click', function () {
        updateProductStatus($('#delProductId').val(), 0);
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




