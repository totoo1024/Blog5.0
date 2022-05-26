layui.use(['form', 'laytpl', 'flow', 'tools'], function () {
    var form = layui.form, $ = layui.$, laytpl = layui.laytpl, flow = layui.flow, tools = layui.tools;
    $(function () {
        search();

        $.get("/article/module", "", function (result) {
            var tpl = categoryview.innerHTML;
            laytpl(tpl).render(result.category, function (html) {
                $("#category").after(html);
            });

            var getTpl = hotview.innerHTML;
            laytpl(getTpl).render(result.hot, function (html) {
                $("#hot").append(html);
            });
        });
    });

    function search(key) {
        $("#articlelist>div").remove();
        flow.load({
            elem: '#articlelist' //指定列表容器
            , done: function (page, next) { //到达临界点（默认滚动触发），触发下一页
                var lis = [];
                var parm = "";
                var cid = tools.queryString("cid");
                var aid = tools.queryString("tid");
                if (cid) {
                    parm = "&cid=" + cid;
                }
                if (aid) {
                    parm = "&tid=" + aid;
                }
                if (key) {
                    parm += "&key=" + key;
                }
                $.get('/article/views?page=' + page + parm, function (res) {
                    //假设你的列表返回在data集合中
                    layui.each(res.data, function (index, item) {
                        var tpl = newsview.innerHTML;
                        laytpl(tpl).render(item, function (html) {
                            lis.push(html);
                        });
                    });
                    //执行下一页渲染，第二参数为：满足“加载更多”的条件，即后面仍有分页
                    //pages为Ajax返回的总页数，只有当前页小于总页数的情况下，才会继续出现加载更多
                    next(lis.join(''), page < res.count);
                });
            }
        });
    }

    form.on('submit(search)', function (data) {
        console.log("aaa");
        var keywords = $("#keywords").val();
        if (keywords == null || keywords == "") {
            layer.msg('请输入要搜索的关键字');
            return false;
        }
        search(keywords);
        return false; //阻止表单跳转。如果需要表单跳转，去掉这段即可。
    });
});
