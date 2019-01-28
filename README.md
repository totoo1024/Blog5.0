# Blog

#### 介绍
个人博客网站

#### 软件架构
网站使用的是.net core 2.2开发，由于前端能力有限，所以没有使用前后端分离，博客基本功能已经全部完成（相关日志已经记录到数据库，没有做展示页面），时间有限，相关优化还没有来得及做，如基本缓存功能还未实现，后期有时间会一一加入进来，做一个整理
[博客地址](http://www.miboxapp.com) http://www.miboxapp.com
[后台地址](http://[输入链接说明](http://www.miboxapp.com/main)) http://www.miboxapp.com/main
后台登录用户名/密码：root/123456
 :grin: 由于博客已经正式上线，所以只开放了预览权限


#### 使用说明

1. 网站使用的sqlsugar ORM开源框架，相关文档请查看官网[sqlsugar框架](http://www.codeisbug.com/)，数据库使用的是mysql，ORM支持5种数据库，所以可以随意切换，具体请看sqlsugar官网文档
2. 数据库设计以及备份和SQL文件放在AppSoft网站下的db目录下,项目中的管理员用户名/密码:admin/admin1024
3. 写代码都有详细注释，这里就不一一介绍，没有复杂的设计（ :grin: 能力太菜）
4. 创建数据库后记得修改appsetting.json文件中的数据库连接字符串，以及Configs文件夹下的nlog.config的数据库连接字符串
1. 在Linux部署注意事项，后台登录验证码使用的是ZKWeb.System.Drawing，所以在Linux上需要安装相关依赖，详细信息请参考作者说明[ZKWeb.System.Drawing](https://github.com/zkweb-framework/zkweb.system.drawing)



前台预览
![前台预览](https://images.gitee.com/uploads/images/2019/0122/094841_7b096768_967952.png "37℃空间-个人博客.png")

后台预览![后台预览](https://images.gitee.com/uploads/images/2019/0122/095015_2d0d64ad_967952.png "后台管理系统.png")

