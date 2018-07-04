﻿
/*
下拉刷新事件
说明：第一次加载会先执行上拉事件
*/
mui.init({
    pullRefresh: {
        container: '#pullrefresh',
        up: {
            callback: pullupRefresh,
            contentinit: '上拉显示更多',
            contentdown: '上拉显示更多',
            contentrefresh: '正在加载...',
            contentnomore: '没有更多数据了'
        }
    }
});


var size = 10; //每页取多少条
var index = 1; //索引页
var sumcount = 0; //总记录数

/**
* 上拉加载具体业务，在尾部加载新内容
*/
function pullupRefresh() {
    setTimeout("ajaxLoaddata()", 500);
}
if (mui.os.plus) {
    mui.plusReady(function () {
        setTimeout(function () {
            mui('#pullrefresh').pullRefresh().pullupLoading();
        }, 1000);

    });
} else {
    mui.ready(function () {
        mui('#pullrefresh').pullRefresh().pullupLoading();
    });
}
//异步加载数据
function ajaxLoaddata() {
    index = size * index < sumcount ? ++index : index;
    $.post("Collects.ashx", { size: size, index: index, action: "list" }, function (requestdata) {
        var data = eval("(" + requestdata + ")");
        sumcount = data.sumcount;
        mui('#pullrefresh').pullRefresh().endPullupToRefresh((size * index >= sumcount)); //参数为true代表没有更多数据了。
        var table = document.body.querySelector('#detail-view');
        for (var i = 0; i < data.items.length; i++) {
            var d = data.items[i];
            var li = document.createElement('li');
            li.className = 'note-item mui-table-view-cell';
            li.setAttribute("qid", d.Qus_ID);
            		
            //
            var html = '';
            html += '<div class="mui-slider-right mui-disabled" qid="' + d.Qus_ID + '">';
            html += '<a class="mui-btn mui-btn-red mui-icon mui-icon-trash" action="del"></a>';
			html += "</div>";
            html += '<div class="mui-slider-handle mui-table"><div class="mui-table-cell">';
            html += '<a class="mui-navigate-right" href="#"><span class="iconfont ico">&#xe657;</span>' + d.Qus_Title + '</a>';
            html += '</div></div>';
            li.innerHTML = html;
            table.appendChild(li);
        }
        //弹出详情
        mui('body').off('tap', '.note-item');
        mui('body').on('tap', '.note-item', function () {
           var qid = $(this).find(">div").attr("qid");
           new PageBox("查看试题","Questions.ashx?qid="+qid,100,100,null).Open();
        });
        //删除与编辑按钮的事件
        mui('body').off('tap', '.mui-btn');
        mui('body').on('tap', '.mui-btn', function (event) {
            var elem = this.parentNode.parentNode
            var qid = elem.getAttribute("qid");
            var action = $(this).attr("action");
            if (action == "del") {
                mui.confirm('确认删除该条记录？', '删除', ['取消', '确认'], function (e) {
                    if (e.index == 1) {
                        $.post("Collects.ashx", { action: "delete", qid: qid }, function (data) {
                            if (data == "1") {
                                mui.toast('删除成功！', { duration: 1000, type: 'div' });
								$(".note-item[qid=" + qid + "]").remove();
                            } else {
                                mui.toast('系统错误！', { duration: 1000, type: 'div' });
                            }                            
                        });

                    } else {
                        setTimeout(function () { mui.swipeoutClose(elem); }, 0);
                    }
                });
            }
            //
        });
    });
}
