$(function () {
    var area1 = new LArea();
    area1.init({
        'trigger': '#demo1', //触发选择控件的文本框，同时选择完毕后name属性输出到该位置
        'valueTo': '#value1', //选择完毕后id属性输出到该位置
        'keys': {
            id: 'id',
            name: 'name'
        }, //绑定数据源相关字段 id对应valueTo的value属性输出 name对应trigger的value属性输出
        'type': 1, //数据源类型
        'data': LAreaData //数据源
    });
    area1.value = [2, 1, 3];//控制初始位置，注意：该方法并不会影响到input的value

    $('.add-address').click(function () {
        if ($('#receiver').val().trim() == "") {
            alert("收货人不能为空");
            return false;
        }
        if ($('#rPhone').val().trim() == "") {
            alert("联系电话不能为空");
            return false;
        }
        if ($('#value1').val().trim() == "") {
            alert("所在地区不能为空");
            return false;
        }
        if ($('#detailAddress').val().trim() == "") {
            alert("详细地址不能为空");
            return false;
        }
        var pattern1 = /^1[34578]\d{9}$/; //手机
        var pattern2 = /^((0\d{2,3})-)(\d{7,8})(-(\d{3,}))?$/; //固定电话
        if (!pattern1.test($('#rPhone').val())
            && !pattern2.test($('#rPhone').val())) {
            alert("联系电话不正确，请重新输入");
            return false;
        }

        $(this).attr("disabled");
        $.ajax({
            url: "AddressAdd",
            data: {
                "receiver": $('#receiver').val(),
                "rPhone": $('#rPhone').val(),
                "district": $('#value1').val(),
                "detailAddress": $('#detailAddress').val(),
                "setAsDefault": $('#default2').prop('checked')
            },
            type: 'post',
            async: false, //默认为true 异步   
            dataType: 'json',
            error: function (data) {
            },
            success: function (retData) {
                if (retData == 'false') {
                    alert("添加失败，请稍后尝试");
                    return false;
                }
            },
            complete: function () {
                $('.add-address').removeAttr("disabled"); //设置变灰按钮  
            }
        });
    });
})