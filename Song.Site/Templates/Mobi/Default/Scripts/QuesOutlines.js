﻿$(function () {
    _clacTax(0,"");
	//获取历史记录的章节的题数
	var loglink=$("a.log");
	var li=$("li[olid="+loglink.attr("olid")+"]");
	var count=li.find("a").attr("count");	
	loglink.attr("href",$().setPara(loglink.attr("href"),"count",count))
});

//计算序号
function _clacTax(pid,prefix){
	$(".outline[pid="+pid+"]").each(function(index, element) {
        var tax=$(this).find(".tax");
		tax.html(prefix+(index+1)+".");
		var olid=$(this).attr("olid");
		_clacTax(olid,tax.text());
    });
}

