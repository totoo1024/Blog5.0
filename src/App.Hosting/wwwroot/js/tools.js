layui.define(['form', 'layer', 'table', 'jquery'], function (exports) {
    var layer = layui.layer, $ = layui.jquery, form = layui.form, table = layui.table;

    // 添加响应拦截器
    axios.interceptors.response.use(function (result) {
        // 对响应数据进行处理
        if (result.data.StatusCode === 401) {
            top.location.href = "/Main/Login/Index";
        }
        return result;
    }, function (error) {
        return Promise.reject(error);
    });

    //表单验证扩展
    form.verify({
        //用户名验证
        //username: function (value, item) { //value：表单的值、item：表单的DOM对象
        //    if (!new RegExp("^[a-zA-Z0-9_\u4e00-\u9fa5\\s·]+$").test(value)) {
        //        return '用户名不能有特殊字符';
        //    }
        //    if (/(^\_)|(\__)|(\_+$)/.test(value)) {
        //        return '用户名首尾不能出现下划线\'_\'';
        //    }
        //    if (/^\d+\d+\d$/.test(value)) {
        //        return '用户名不能全为数字';
        //    }
        //}
        username: [/^[a-zA-Z][a-zA-Z0-9-_]{4,15}$/, '用户名必须以字母开头，可包含_和数字']
        //密码验证
        , pass: [
            /^[\S]{6,18}$/
            , '密码必须6到18位，且不能出现空格'
        ],
        //小数
        decimal: [/^(-?\d+)(\.\d+)?$/, '数字格式不正确'],
        cn: [/^[\u4e00-\u9fa5]{0,}$/, '中文格式不正确'],
        money: [/^[1-9][0-9]*$/, '金额格式不正确'],
        ip: [/(25[0-5]|2[0-4]\d|[0-1]\d{2}|[1-9]?\d)\.(25[0-5]|2[0-4]\d|[0-1]\d{2}|[1-9]?\d)\.(25[0-5]|2[0-4]\d|[0-1]\d{2}|[1-9]?\d)\.(25[0-5]|2[0-4]\d|[0-1]\d{2}|[1-9]?\d)/, 'IP地址不合法'],
        qq: [/^[1-9]([0-9]{4,11})/, 'QQ号码无效'],
        datetime: [/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$/, '时间格式不正确'],
        en: [/^[a-zA-Z]+$/, '字母格式不正确'],
        idcard: [/(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/, '身份证号码不合法'],
        num: [/^[1-9]\d*|0$/, '数字必须为正整数']
    });

    var tools = {
        msg: function (text) {
            layer.msg(text, { icon: 5 });
        },
        moduleOpen: function (options) {
            var defaults = {
                id: new Date().getTime(),
                title: '系统窗口',
                width: "100px",
                height: "100px",
                resize: false,
                url: '',
                btn: ['确认', '取消'],
                callBack: null,
                cancel: null,
                cancelCallBack: false//是否有回调函数
                //,isDestroy: false//窗口销毁时所作操作
            };
            var options = $.extend(defaults, options);
            layer.open({
                id: options.id,
                type: 2,
                //shade: options.shade,
                title: options.title,
                fix: false,
                area: [options.width, options.height],
                content: options.url,
                btn: options.btn,
                resize: options.resize,
                yes: function (index, layero) {
                    options.callBack(index, layero)
                }, cancel: function (index, layero) {
                    if (options.cancelCallBack) {
                        options.cancel(index, layero);
                    }
                }
            });
        },
        queryString: function (key) {//获取链接参数值
            var reg = new RegExp("(^|&)" + key + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return "";
        },
        post: function (options) {
            var defaults = {
                url: '',
                data: null,
                datatype: "form",
                headers: { 'X-Requested-With': 'XMLHttpRequest' },
                loading: '正在提交请求...',
                success: null,
                isloading: true
            };
            var options = $.extend(defaults, options);
            if (options.datatype == "form") {
                options.headers["Content-Type"] = "application/x-www-form-urlencoded;charset=utf-8";
                options.data = $.param(options.data);
            }
            //加载动画
            var loading = undefined;
            if (options.isloading) {
                loading = layer.msg(options.loading, {
                    icon: 16,
                    shade: 0.4,
                    time: 0
                });
            }
            axios({ method: "post", url: options.url, headers: options.headers, data: options.data }).then(function (result) {
                if (options.isloading) {
                    layer.close(loading);
                }
                callBack(result.data);
            }).catch(function (error) {
                if (options.isloading) {
                    layer.close(loading);
                }
                layer.msg(error.message, { icon: 2 });
            });
        },
        get: function (options) {
            var defaults = {
                url: '',
                loading: '正在加载中...',
                success: null,
                isloading: true
            };
            var options = $.extend(defaults, options);
            //加载动画
            var loading = undefined;
            if (options.isloading) {
                loading = layer.msg(options.loading, {
                    icon: 16,
                    shade: 0.4,
                    time: 0
                });
            }
            axios({ method: "get", url: options.url, headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(function (result) {
                if (options.isloading) {
                    layer.close(loading);
                }
                options.success(result.data);
            }).catch(function (error) {
                if (options.isloading) {
                    layer.close(loading);
                }
                layer.msg(error.message, { icon: 2 });
            });
        },
        requestAll: function (arr, callBack) {
            if (arr == undefined || arr.length != 2) {
                this.msg("请求参数错误");
                return false;
            }
            //加载动画
            let loading = layer.msg("正在加载中...", {
                icon: 16,
                shade: 0.4,
                time: 0
            });
            var a = new Array();
            for (var i = 0; i < arr.length; i++) {
                var item = axios({
                    method: arr[i].method || "get",
                    url: arr[i].url,
                    headers: { 'X-Requested-With': 'XMLHttpRequest' },
                    data: arr[i].data
                });
                a.push(item);
            }
            axios.all(a).then(axios.spread(function (data1, data2) {
                layer.close(loading);
                callBack(data1.data, data2.data);
            })).catch(function (error) {
                layer.close(loading);
                layer.msg(error.message, { icon: 2 });
            });
        }, submit: function (options) {
            var defaults = {
                url: "",
                data: [],
                datatype: "form",
                headers: { 'X-Requested-With': 'XMLHttpRequest' },
                loading: "正在提交数据...",
                success: null,
                close: true
            };
            var options = $.extend(defaults, options);
            if (options.datatype == "form") {
                options.headers["Content-Type"] = "application/x-www-form-urlencoded;charset=utf-8";
                options.data = $.param(options.data);
            }
            //加载动画
            var loading = layer.msg(options.loading, {
                icon: 16,
                shade: 0.4,
                time: 0
            });
            axios({
                method: "post",
                headers: options.headers,
                url: options.url,
                data: options.data
            }).then(function (response) {
                layer.close(loading);
                if (response.data.StatusCode === 200) {
                    //成功
                    options.success(response.data);
                    layer.msg(response.data.Message, { icon: 1 });
                } else {
                    //失败
                    layer.msg(response.data.Message, { icon: 2 });
                }
            }).catch(function (error) {
                layer.close(loading);
                layer.msg(error.message, { icon: 2 });
            });
        }, submitConfrim: function (options) {
            var defaults = {
                prompt: "注：您确定要变更该项数据吗？",
                url: "",
                data: [],
                datatype: "form",
                headers: { 'X-Requested-With': 'XMLHttpRequest' },
                loading: "正在提交数据...",
                success: null,
                cancel: null,
                cancelCallBack: false,
                close: true
            };
            var options = $.extend(defaults, options);
            if (options.datatype == "form") {
                options.headers["Content-Type"] = "application/x-www-form-urlencoded;charset=utf-8";
                options.data = $.param(options.data);
            }
            layer.confirm(options.prompt, {
                title: '系统提示',
                icon: 3,
                btn: ['确认', '取消'] //按钮
            }, function () {//确认
                //加载动画
                let loading = layer.msg(options.loading, {
                    icon: 16,
                    shade: 0.4,
                    time: 0
                });
                axios({
                    method: "post",
                    headers: options.headers,
                    url: options.url,
                    data: options.data
                }).then(function (response) {
                    layer.close(loading);
                    options.success(response.data);
                    if (response.data.StatusCode === 200) {
                        //成功
                        layer.msg(response.data.Message, { icon: 1 });
                    } else {
                        //失败
                        layer.msg(response.data.Message, { icon: 2 });
                    }
                }).catch(function (error) {
                    layer.close(loading);
                    layer.msg(error.message, { icon: 2 });
                });
            }, function (index, layero) {
                //取消
                if (options.cancelCallBack) {
                    options.cancel(index, layero);
                }
            });
        }, condition: function () {
            let arr = new Array();
            $("*[data-op]").each(function () {
                let name = $(this).attr("name"), op = $(this).attr("data-op"), val = $(this).val().trim();
                if (name && op && val) {
                    arr.push({ name: name, op: op, value: val });
                }
            });
            if (arr.length == 0) {
                return null;
            } else {
                return { conditions: arr };
            }
        }, renderSelect: function (options) {
            var defaults = {
                elem: '',//绑定的元素
                text: '',//默认项显示的文本
                value: '',//默认项显示的值
                url: '',//数据源请求链接
                data: undefined
            };
            var options = $.extend(defaults, options);
            var $select = $(options.elem);
            if (options.text) {
                $select.append("<option value=\"" + options.value + "\">" + options.text + "</option>");
            }
            if (options.data) {
                $.each(options.data, function () {
                    $select.append("<option value=\"" + this.value + "\">" + this.text + "</option>");
                });
                form.render();
            } else {
                this.get({
                    url: options.url,
                    success: function (result) {
                        $.each(result, function () {
                            $select.append("<option value=\"" + this.value + "\">" + this.text + "</option>");
                        });
                        form.render();
                    }
                });
            }
        }

    };
    exports('tools', tools);
})