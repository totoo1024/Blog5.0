var layedit;
layui.use(['jquery', 'form', 'layedit', 'flow', 'laytpl'], function () {
    var form = layui.form, laytpl = layui.laytpl;
    var $ = layui.jquery;
    layedit = layui.layedit;
    var flow = layui.flow;

    var w = $("body").width();

    $(document).ready(function () {
        //友情链接
        $.get("/home/link", "", function (result) {
            var getTpl = linkview.innerHTML;
            laytpl(getTpl).render(result, function (html) {
                $("#link").append(html);
            });
        });
        showmsg();
        if ($("#time").length > 0) {
            systemTime();
        }
        if (w <= 450) {
            $("body").on("click", ".layedit-tool-face", function () {
                $(".layui-util-face").css("left", "0");
            });
        }
    });

    //评论和留言的编辑器
    var editIndex = layedit.build('remarkEditor', {
        height: 150,
        tool: ['face', '|', 'left', 'center', 'right', '|', 'link'],
    });
    //评论和留言的编辑器的验证
    layui.form.verify({
        content: function (value) {
            value = $.trim(layedit.getText(editIndex));
            if (value == "") return "至少得有一个字吧";
            layedit.sync(editIndex);
        },
        replyContent: function (value) {
            if ($.trim(value) == "") {
                return "至少得有一个字吧!";
            }
        }
    });
    //评论显示
    function showmsg() {
        $("#messageList").children().remove();
        flow.load({
            elem: '#messageList'//流加载容器
            , done: function (page, next) { //执行下一页的回调
                var lis = [];
                $.get("/home/msg?page=" + page + "&r=" + Math.random(), "", function (result) {
                    if (result.count > 0) {
                        var getTpl = msgview.innerHTML;
                        $.each(result.data, function (index, item) {
                            laytpl(getTpl).render(item, function (html) {
                                lis.push(html);
                            });
                        });
                    }
                    //执行下一页渲染，第二参数为：满足“加载更多”的条件，即后面仍有分页
                    //pages为Ajax返回的总页数，只有当前页小于总页数的情况下，才会继续出现加载更多
                    next(lis.join(''), page < result.count);
                });
            }
        });
    }

    //监听留言提交
    form.on('submit(formLeaveMessage)', function (data) {
        var loading = layer.msg("正在提交留言...", {
            icon: 16,
            time: 0
        });
        var url = '/home/comment';
        $.ajax({
            type: "POST",
            url: url,
            data: data.field,
            success: function (res) {
                layer.close(loading);
                if (res.StatusCode === 200) {
                    showmsg();
                    layedit.sync(editIndex);
                    layedit.setContent(editIndex, "");
                    layer.msg("评论成功", {
                        icon: 1
                    });
                } else if (res.StatusCode === 401) {
                    layer.confirm('您还没有登录，是否前往登录？', {
                        btn: ['是', '否'] //按钮
                    }, function () {
                        var loading = layer.msg("正在通过QQ登录", {
                            icon: 16,
                            time: 0
                        });
                        $.get("/home/login", "", function (result) {
                            layer.close(loading);
                            if (result.StatusCode === 200) {
                                window.location.href = result.Data;
                            } else {
                                layer.msg(result.Message, { icon: 5 });
                            }
                        });
                    });
                } else {
                    layer.msg(res.Message);
                }
            },
            error: function (data) {
                layer.close(loading);
                layer.msg("评论失败！");
            }
        });
        return false;
    });

    //监听留言回复提交
    form.on('submit(formReply)', function (data) {
        data.field["rootid"] = $(this).attr("data-rootid");
        data.field["fromid"] = $(this).attr("data-fromid");
        var loading = layer.msg("正在提交回复...", {
            icon: 16,
            time: 0
        });
        //模拟留言回复
        var url = '/home/reply';
        $.ajax({
            type: "POST",
            url: url,
            data: data.field,
            success: function (res) {
                layer.close(loading);
                if (res.StatusCode === 200) {
                    showmsg();
                    btnReplyClick(this);
                    layer.msg("回复成功", {
                        icon: 1
                    });
                } else if (res.StatusCode === 401) {
                    layer.confirm('您还没有登录，是否前往登录？', {
                        btn: ['是', '否'] //按钮
                    }, function () {
                        var loading = layer.msg("正在通过QQ登录", {
                            icon: 16,
                            time: 0
                        });
                        $.get("/home/login", "", function (result) {
                            layer.close(loading);
                            if (result.Status == 0) {
                                window.location.href = result.Data;
                            } else {
                                layer.msg(result.Message, { icon: 5 });
                            }
                        });
                    });
                } else {
                    layer.msg(res.Message);
                }
            },
            error: function (data) {
                layer.close(loading);
                layer.msg("回复失败！");
            }
        });
        return false;
    });
});
//加载更多回复
function nextpage(elem) {
    var $ = layui.jquery, laytpl = layui.laytpl;
    var $e = $(elem);
    var rootid = $e.attr("data-rootid");
    var page = $e.attr("data-page");
    var loading = layer.msg("正在加载中...", {
        icon: 16,
        time: 0
    });
    $.get("/home/replypage", { "page": page, "rootid": rootid }, function (result) {
        var getTpl = moreview.innerHTML;
        laytpl(getTpl).render(result.data, function (html) {
            $e.parent().before(html);
        });
        if (result.count > page) {
            $e.attr("data-total", result.count);
            $e.attr("data-page", page + 1);
        } else {
            $e.parent().remove();
        }
        layer.close(loading);
    });
}

function btnReplyClick(elem) {
    var $ = layui.jquery;
    $(elem).parent('p').parent('.comment-parent').siblings('.replycontainer').toggleClass('layui-hide');
    if ($(elem).text() == '回复') {
        $(elem).text('收起')
    } else {
        $(elem).text('回复')
    }
}

function systemTime() {
    //获取系统时间。
    var dateTime = new Date();
    var year = dateTime.getFullYear();
    var month = dateTime.getMonth() + 1;
    var day = dateTime.getDate();
    var hh = dateTime.getHours();
    var mm = dateTime.getMinutes();
    var ss = dateTime.getSeconds();
    //分秒时间是一位数字，在数字前补0。
    mm = extra(mm);
    ss = extra(ss);

    //将时间显示到ID为time的位置，时间格式形如：19:18:02
    document.getElementById("time").innerHTML = year + "-" + month + "-" + day + " " + hh + ":" + mm + ":" + ss;
    //每隔1000ms执行方法systemTime()。
    setTimeout("systemTime()", 1000);
}

//补位函数。
function extra(x) {
    //如果传入数字小于10，数字前补一位0。
    if (x < 10) {
        return "0" + x;
    } else {
        return x;
    }
}