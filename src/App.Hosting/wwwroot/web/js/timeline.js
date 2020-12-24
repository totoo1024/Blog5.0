layui.use(['form', 'laytpl', 'flow'], function () {
    var $ = layui.jquery, flow = layui.flow, laytpl = layui.laytpl;
    $(function () {
        flow.load({
            elem: '#timeline' //指定列表容器
            , done: function (page, next) { //到达临界点（默认滚动触发），触发下一页
                var lis = [];
                $.get('/home/line?page=' + page, function (res) {
                    if (res.code === 0) {
                        layui.each(res.data, function (index, item) {
                            var tpl;
                            var $y = $("#y-" + item.Year);
                            //年份元素不存在直接追加
                            if ($y.length == 0) {
                                tpl = lineview.innerHTML;
                                laytpl(tpl).render(item, function (html) {
                                    lis.push(html);
                                });
                            } else {
                                //存在就选择追加
                                for (var key in item.Items) {
                                    var $m = $("#m-" + item.Year + key);
                                    //月份元素不存在
                                    if ($m.length == 0) {
                                        tpl = monthview.innerHTML;
                                        var data = { Year: item.Year, Month: key, Items: item.Items[key] };
                                        laytpl(tpl).render(data, function (html) {
                                            $("#y-" + item.Year).append(html);
                                        });
                                    } else {
                                        tpl = dayview.innerHTML;
                                        laytpl(tpl).render(item.Items[key], function (html) {
                                            $("#m-" + item.Year + key + " ul").append(html);
                                        });
                                    }

                                }
                            }
                        });
                        next(lis.join(''), page < res.count);
                        if (page >= res.count) {
                            $(".layui-flow-more").remove();
                            $("#timeline").append('<h1 style="padding-top:4px;padding-bottom:2px;margin-top:40px;"><i class="fa fa-hourglass-end"></i>THE END</h1>');
                        }
                    }
                });
            }
        });
        $("#timeline").on("click", ".monthToggle", function () {
            $(this).parent('h3').siblings('ul').slideToggle('slow');
            $(this).siblings('i').toggleClass('fa-caret-down fa-caret-right');
        });
        $("#timeline").on("click", ".yearToggle", function () {
            $(this).parent('h2').siblings('.timeline-month').slideToggle('slow');
            $(this).siblings('i').toggleClass('fa-caret-down fa-caret-right');
        });

    })
})