layui.use(['form', 'layer', 'jquery'], function () {
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery;
    if (useGeetest) {
        $.ajax({
            url: "register?t=" + (new Date()).getTime(), // 加随机数防止缓存
            type: "get",
            dataType: "json",
            success: function (data) {

                // 调用 initGeetest 进行初始化
                // 参数1：配置参数
                // 参数2：回调，回调的第一个参数验证码对象，之后可以使用它调用相应的接口
                initGeetest({
                    // 以下 4 个配置参数为必须，不能缺少
                    gt: data.gt,
                    challenge: data.challenge,
                    offline: !data.success, // 表示用户后台检测极验服务器是否宕机
                    new_captcha: data.new_captcha, // 用于宕机时表示是新验证码的宕机

                    product: "bind", // 产品形式，包括：float，popup
                    width: "300px",
                    //https: true,
                    //api_server: "apiv6.geetest.com"

                    // 更多配置参数说明请参见：http://docs.geetest.com/install/client/web-front/
                }, function (captchaObj) {
                    captchaObj.onReady(function () {
                        $("#wait").hide();
                    }).onSuccess(function () {
                        var result = captchaObj.getValidate();
                        if (!result) {
                            layer.msg("请完成验证", { icon: 5 });
                            return false;
                        }
                        for (key in result) {
                            $("input[name=" + key + "]").val(result[key]);
                        }
                        login();
                    }).onError(function () {
                        console.log("错误");
                    });
                    //登录按钮
                    form.on("submit(login)",
                        function (data) {
                            captchaObj.verify();
                            return false;
                        });
                });
            }
        });
    } else {
        //登录按钮
        form.on("submit(login)",
            function (data) {
                login();
            });
    }


    //登录请求
    function login() {
        var $btn = $("#btnLogin");
        $btn.text("登录中...").attr("disabled", "disabled").addClass("layui-disabled");
        var parm = {};
        $("form input").each(function () {
            parm[$(this).attr("name")] = $(this).val();
        });
        $.post("/Main/Login/Login",
            parm,
            function (result) {
                if (result.StatusCode === 200) {
                    window.location.href = result.Data;
                } else {
                    $("#imgVerifyCode").attr("src", "/Main/Login/ValidateCode?" + Math.random());
                    $("#code").val("");
                    layer.msg(result.Message, { icon: 5 });
                    $btn.text('登录').removeClass("layui-disabled").removeAttr("disabled");
                }
            });
    }

    //表单输入效果
    $(".loginBody .input-item").click(function (e) {
        e.stopPropagation();
        $(this).addClass("layui-input-focus").find(".layui-input").focus();
    });
    $(".loginBody .layui-form-item .layui-input").focus(function () {
        $(this).parent().addClass("layui-input-focus");
    });
    $(".loginBody .layui-form-item .layui-input").blur(function () {
        $(this).parent().removeClass("layui-input-focus");
        if ($(this).val() != '') {
            $(this).parent().addClass("layui-input-active");
        } else {
            $(this).parent().removeClass("layui-input-active");
        }
    });
})
