$(function () {
    // 详情数量减少
    $('.details_con .minus,.cart_count .minus').click(function () {
        var _index = $(this).parent().parent().index() - 1;
        var _count = $(this).parent().find('.count');
        var _val = _count.val();
        if (_val > 1) {
            _count.val(_val - 1);
            $('.details_con .selected span').eq(_index).text(_val - 1);

        }
        if (_val <= 2) {
            $(this).addClass('disabled');
        }

    });

    // 详情数量添加
    $('.details_con .add,.cart_count .add').click(function () {
        var _index = $(this).parent().parent().index() - 1;
        var _count = $(this).parent().find('.count');
        var _val = _count.val();
        $(this).parent().find('.minus').removeClass('disabled');
        _count.val(_val - 0 + 1);
        $('.details_con .selected span').eq(_index).text(_val - 0 + 1);

    });

   
});