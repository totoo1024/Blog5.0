var $, tab, dataStr, layer, topMenu, childMenu, toolButton, rowButton;

layui.use(['bodyTab', 'tools'],
    function () {
        var form = layui.form,
            element = layui.element,
            tools = layui.tools;
        $ = layui.$;
        layer = parent.layer === undefined ? layui.layer : top.layer;
        tab = layui.bodyTab({
            openTabNum: "50"
            //,  //最大可打开窗口数量
            //url: "json/navs.json" //获取菜单json地址
        });
        // 判断是否显示锁屏
        if (window.sessionStorage.getItem("lockcms") == "true") {
            lockPage();
        }

        //修改密码
        $("#changepwd").click(function () {
            tools.moduleOpen({
                title: "修改密码",
                url: "/Main/Home/Password",
                width: "450px",
                height: "230px",
                callBack: function (index, layero) {
                    var iframeWindow = window["layui-layer-iframe" + index];
                    iframeWindow.layui.form.on("submit(save)",
                        function (data) {
                            if (data.field.Password.length < 6 || data.field.RePassword.length < 6) {
                                tools.msg("密码长度不能少于6个字符");
                                return false;
                            }
                            tools.submit({
                                url: '/Main/Home/ChangePwd',
                                data: data.field,
                                loading: "正在保存数据...",
                                success: function () {
                                    layer.close(index);
                                }
                            });
                        });
                    layero.find("iframe").contents().find("#btnSave").trigger("click");
                }
            });
        });

        //锁屏
        function lockPage() {
            var $u = $("#userInfo a:eq(0)");
            var img = $u.children("img").attr("src");
            var name = $u.children("cite").text();
            layer.open({
                title: false,
                type: 1,
                content: '<div class="admin-header-lock" id="lock-box">' +
                    '<div class="admin-header-lock-img"><img src="' +
                    img +
                    '" class="userAvatar"/></div>' +
                    '<div class="admin-header-lock-name" id="lockUserName">' +
                    name +
                    '</div>' +
                    '<div class="input_btn">' +
                    '<input type="password" class="admin-header-lock-input layui-input" autocomplete="off" placeholder="请输入密码解锁.." name="lockPwd" id="lockPwd" />' +
                    '<button class="layui-btn" id="unlock">解锁</button>' +
                    '</div>' +
                    '<p>请输入登录密码' +
                    '</div>',
                closeBtn: 0,
                shade: 0.9
            })
            $(".admin-header-lock-input").focus();
        }

        $(".lockcms").on("click",
            function () {
                window.sessionStorage.setItem("lockcms", true);
                lockPage();
            });
        // 解锁
        $("body").on("click",
            "#unlock",
            function () {
                if ($(this).siblings(".admin-header-lock-input").val() == '') {
                    layer.msg("请输入解锁密码！");
                    $(this).siblings(".admin-header-lock-input").focus();
                } else {
                    var loading = layer.load(1);
                    var pwd = $(this).siblings(".admin-header-lock-input").val()
                    $.post("/main/home/lock",
                        { pwd: pwd },
                        function (result) {
                            layer.close(loading);
                            if (result.StatusCode === 200) {
                                window.sessionStorage.setItem("lockcms", false);
                                $(this).siblings(".admin-header-lock-input").val('');
                                layer.closeAll("page");
                            } else {
                                layer.msg(result.Message);
                                $(this).siblings(".admin-header-lock-input").val('').focus();
                            }
                        });
                }
            });
        $(document).on('keydown',
            function (event) {
                var event = event || window.event;
                if (event.keyCode == 13) {
                    $("#unlock").click();
                }
            });

        initMenu();

        function initMenu() {
            var mobile = '', pc = '', defaultMneuId = '';
            tools.get({
                url: "/SystemManage/Module/GetMenuAndButton",
                success: function (data) {
                    topMenu = data.topMenus;
                    childMenu = data.childMenus;
                    toolButton = data.toolButtons;
                    rowButton = data.rowButtons;
                    layui.each(topMenu,
                        function (index) {
                            if (index == 0) {
                                defaultMneuId = this.id;
                            }
                            mobile += '<dd ' +
                                (index == 0 ? 'class="layui-this"' : '') +
                                ' data-menu="' +
                                this.id +
                                '">';
                            mobile += '<a href="javascript:;">';
                            mobile += '<i class="' + this.icon + '" data-icon="' + this.icon + '"></i>';
                            mobile += '<cite>' + this.title + '</cite></a></dd>';

                            pc += '<li class="layui-nav-item ' +
                                (index == 0 ? 'layui-this' : '') +
                                '" data-menu="' +
                                this.id +
                                '" pc>';
                            pc += ' <a href="javascript:;"><i class="' +
                                this.icon +
                                '" data-icon="' +
                                this.icon +
                                '"></i><cite>' +
                                this.title +
                                '</cite></a>';
                            pc += '</li>';
                        });
                    $("#pcMenu").html(pc);
                    $("#mobileMenu").html(mobile);
                    getData(defaultMneuId);
                }
            });
        }

        //通过顶部菜单获取左侧二三级菜单   注：此处只做演示之用，实际开发中通过接口传参的方式获取导航数据
        function getData(encode) {
            if (encode) {
                dataStr = { menus: childMenu[encode], code: encode };
                tab.render();
            }
        }

        //退出登录
        $("#signOut").click(function () {
            tools.submitConfrim({
                prompt: "您确定需要退出登录吗？",
                url: '/Main/Login/SignOut',
                success: function () {
                    location.href = "/main/login/index";
                }
            });
        });

        //页面加载时判断左侧菜单是否显示
        //通过顶部菜单获取左侧菜单
        $(".topLevelMenus,.mobileTopLevelMenus").on("click",
            "li,dd",
            function () {
                if ($(this).parents(".mobileTopLevelMenus").length != "0") {
                    $(".topLevelMenus li").eq($(this).index()).addClass("layui-this").siblings()
                        .removeClass("layui-this");
                } else {
                    $(".mobileTopLevelMenus dd").eq($(this).index()).addClass("layui-this").siblings()
                        .removeClass("layui-this");
                }
                $(".layui-layout-admin").removeClass("showMenu");
                $("body").addClass("site-mobile");
                //var c = $(this).html();
                getData($(this).data("menu"));
                //渲染顶部窗口
                tab.tabMove();
                var currlayid = tab.getSelfLayId();
                var $curr = $("a[data-id='" + currlayid + "']");
                if ($curr.length > 0) {
                    $curr.parent().addClass("layui-this").siblings().removeClass("layui-this");
                }
            });

        //隐藏左侧导航
        $(".hideMenu").click(function () {
            if ($(".topLevelMenus li.layui-this a").data("url")) {
                layer.msg("此栏目状态下左侧菜单不可展开"); //主要为了避免左侧显示的内容与顶部菜单不匹配
                return false;
            }
            var c = $(this).attr("class");
            if (c.indexOf("left") > -1) {
                $(this).attr("class", c.replace("spread-left", "shrink-right"));
            } else {
                $(this).attr("class", c.replace("shrink-right", "spread-left"));
            }
            $(".layui-layout-admin").toggleClass("showMenu");
            //渲染顶部窗口
            tab.tabMove();
        });

        //通过顶部菜单获取左侧二三级菜单   注：此处只做演示之用，实际开发中通过接口传参的方式获取导航数据
        //getData("contentManagement");

        //手机设备的简单适配
        $('.site-tree-mobile').on('click',
            function () {
                $('body').addClass('site-mobile');
            });
        $('.site-mobile-shade').on('click',
            function () {
                $('body').removeClass('site-mobile');
            });

        // 添加新窗口
        $("body").on("click",
            ".layui-nav .layui-nav-item a:not('.mobileTopLevelMenus .layui-nav-item a')",
            function () {
                //如果不存在子级
                if ($(this).siblings().length == 0) {
                    addTab($(this));
                    $('body').removeClass('site-mobile'); //移动端点击菜单关闭菜单层
                }
                $(this).parent("li").siblings().removeClass("layui-nav-itemed");
            });

        //清除缓存
        $(".clearCache").click(function () {
            window.sessionStorage.clear();
            window.localStorage.clear();
            var index = layer.msg('清除缓存中，请稍候', { icon: 16, time: false, shade: 0.8 });
            setTimeout(function () {
                layer.close(index);
                layer.msg("缓存清除成功！");
            },
                1000);
        });

        //刷新后还原打开的窗口
        if (cacheStr == "true") {
            if (window.sessionStorage.getItem("menu") != null) {
                menu = JSON.parse(window.sessionStorage.getItem("menu"));
                curmenu = window.sessionStorage.getItem("curmenu");
                var openTitle = '';
                for (var i = 0; i < menu.length; i++) {
                    openTitle = '';
                    if (menu[i].icon) {
                        openTitle += '<i class="' + menu[i].icon + '"></i>';
                    }
                    openTitle += '<cite>' + menu[i].title + '</cite>';
                    openTitle += '<i class="layui-icon layui-unselect layui-tab-close" data-id="' +
                        menu[i].layId +
                        '">&#x1006;</i>';
                    element.tabAdd("bodyTab",
                        {
                            title: openTitle,
                            content: "<iframe src='" + menu[i].href + "' data-id='" + menu[i].layId + "'></frame>",
                            id: menu[i].layId
                        })
                    //定位到刷新前的窗口
                    if (curmenu != "undefined") {
                        if (curmenu == '' || curmenu == "null") { //定位到后台首页
                            element.tabChange("bodyTab", '');
                        } else if (JSON.parse(curmenu).title == menu[i].title) { //定位到刷新前的页面
                            element.tabChange("bodyTab", menu[i].layId);
                        }
                    } else {
                        element.tabChange("bodyTab", menu[menu.length - 1].layId);
                    }
                }
                //渲染顶部窗口
                tab.tabMove();
            }
        } else {
            window.sessionStorage.removeItem("menu");
            window.sessionStorage.removeItem("curmenu");
        }
    });

//打开新窗口
function addTab(_this) {
    tab.tabAdd(_this);
}

//捐赠弹窗
function donation() {
    layer.tab({
        area: ['260px', '367px'],
        tab: [
            {
                title: "微信",
                content:
                    "<div style='padding:30px;overflow:hidden;background:#d2d0d0;'><img width='100%' src='/images/wxpay.png'></div>"
            }, {
                title: "支付宝",
                content:
                    "<div style='padding:30px;overflow:hidden;background:#d2d0d0;'><img width='100%' src='/images/alipay.jpg'></div>"
            }
        ]
    });
}

//图片管理弹窗
function showImg() {
    $.getJSON('json/images.json', function (json) {
        var res = json;
        layer.photos({
            photos: res,
            anim: 5
        });
    });
}