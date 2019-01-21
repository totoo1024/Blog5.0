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

1. 网站使用的sqlsugar ORM开源框架，相关文档请查看官网[sqlsugar框架](http://www.codeisbug.com/)，数据库使用的是mysql，ORM支持5中数据库，所以可以随意切换，具体请看sqlsugar官网文档
2. 数据库设计以及备份和SQL文件放在AppSoft网站下的db目录下
3. 写代码都有详细注释，这里就不一一介绍，没有复杂的设计（ :grin: 能力太菜）
4.创建数据库后记得修改appsetting.json文件中的数据库连接字符串，以及Configs文件夹下的nlog.config的数据库连接字符串

#### 码云特技

1. 使用 Readme\_XXX.md 来支持不同的语言，例如 Readme\_en.md, Readme\_zh.md
2. 码云官方博客 [blog.gitee.com](https://blog.gitee.com)
3. 你可以 [https://gitee.com/explore](https://gitee.com/explore) 这个地址来了解码云上的优秀开源项目
4. [GVP](https://gitee.com/gvp) 全称是码云最有价值开源项目，是码云综合评定出的优秀开源项目
5. 码云官方提供的使用手册 [https://gitee.com/help](https://gitee.com/help)
6. 码云封面人物是一档用来展示码云会员风采的栏目 [https://gitee.com/gitee-stars/](https://gitee.com/gitee-stars/)