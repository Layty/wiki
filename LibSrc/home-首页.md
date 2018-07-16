# Wiki文档

<br>

[返回我的Blog](/../)  

![欢迎使用Wiki！](amWiki/images/logo.png "Wiki！")  


[ [点击返回我的博客](/../) ]


## amwikiTool 说明

#### 功能

1. 自动生成导航文件
2. 将本地的md文件转换为amwiki 识别的路径(图片和文件的引用)
3. C##写的,源码也在此

#### 目录结构

```
---amwikiTool.exe放置在此
---library-------网页读取的文件
	--home-首页.md
	--$navigation.md 导航文件
---LibSrc--------本地写md的位置,使用本工具将推送至library
```

#### 程序流程

1. 判断是否存在LibSrc,如果有,进入2,否则copy`library`到`LibSrc`
2. 删除`library`,将`LibSrc`copy至`library`
3. 遍历`library`,生成导航文件,并将md里面的链接转换为以`index.html`为`/`的引用路径

#### 名字规范

1. 文件夹中的`[...]`或者包含`assets`的文件夹内部的内容为引用文件夹,放置在与md同级方便引用

   - typora 软件支持自动将截图和本地拖拽的图片转换为同路径的和文件同名+.assets的文件夹中
2. 兼容作者原先的 `0xx-`的文件名排序,同时支持不加序号的文件名,生成导航


#### node 命令行

```
使用node 查看浏览器
node amWiki/App/bin/main.js -b
使用node更新 (废弃)
node amWiki/App/bin/main.js -u 
```

#### shell 命令行(废弃-不支持目录)

```
##!/bin/sh
####此脚本：生成左侧导航栏目,放在根目录下，即和index.html同级
cd ./library
echo "######## [首页](?file=home-首页)" > '$navigation.md'
ls -d */ | while read CATEGORY
do
echo -e "\n########## $CATEGORY" | sed 's/\// /' >>  '$navigation.md'
grep -R -m 1 '^##'  $CATEGORY |sort -r  | sed '/##/p' | sed 'N;s/\n/](?file=/'  | sed 's/##/@- [/1' | sed 's/.*@//g' | sed 's/##.*//g' | sed 's/.md:/)/g' >> '$navigation.md'
done
cat '$navigation.md'
```
