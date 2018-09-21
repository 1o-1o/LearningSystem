﻿$(function () {
    //按钮导航
    mui('body').on('tap', '.mm-item', function () {
        if ($(this).attr("href") != "")
            document.location.href = $(this).attr("href");
        return false;
    });
    //
    //计算章节序号（树形排序）
    (function claxtax(pid, prefix) {
        $(".outline[pid=" + pid + "]").each(function (index, element) {
            var tax = $(this).find(".tax");
            tax.html(prefix + (index + 1) + ".");
            var olid = $(this).attr("olid");
            claxtax(olid, tax.text());
        });
    })(0, "");
    //
    //计算总题数
    (function () {
        var sum = 0;
        $("li[pid=0]").each(function (index, element) {
            sum += Number($(this).find("a").attr("count"));
        });
        $(".sum").text(sum);
    })();
    //
    clac();
    clac_succper();
});

//计算练习的题数
function clac() {
    var couid = $().getPara("couid");
    //记录总共练习多少道
    var count = 0;
    var last = null; //最后一次练习的记录
    $(".outline").each(function (index) {
        var olid = $(this).attr("olid");
        var keyname = state.name.get("QuesExercises", couid, olid);
        var data = state.read(keyname);
        if (data.current != null) {
            if (last == null) last = data.current;
            if (last.last < data.current.last) last = data.current;
        }
        //计算答题信息
        var ret = state.clac(data.items);
        var sum = Number($(this).find(".num").text());
        $(this).find(".ansnum").text(ret.ansnum > sum ? sum : ret.ansnum);   //已经做了多少题
        //正确率（除以整体数量）
        var per = ret.sum > 0 ? Math.floor(ret.correct / ret.sum * 1000) / 10 : 0;
        per = per > 0 ? per : 0;
        $(this).find("b").attr("per", per).addClass("per" + Math.floor(per/10));
        //$(this).find("b").addClass("per" + index);
        count += ret.ansnum;
    });
    //已经练习的题数
    $(".ansSum").text(count);
    //"继续练习"按钮的设置
    $(".log").attr("href", function () {
        if (last == null) return "#";
        var href = $(this).attr("href");
        href = $().setPara(href, "couid", last.info.couid);
        href = $().setPara(href, "olid", last.info.olid);
        href = $().setPara(href, "qid", last.qid);
        var li = $("li[olid=" + last.info.olid + "]");
        href = $().setPara(href, "count", li.find("a").attr("count"));
        return href;
    });
}
//计算总的通过率
function clac_succper() {
    //var outline = { olid: 0, pid: 0, per: 0, level: 0, islast: true, childs: null };  

    var tree = build_tree($(".outline"), null, 1);
    //生成章节的树形结构
    function build_tree(olitem, parent, level) {
        var arr = new Array();
        var pid = 0;
        if (parent != null) pid = parent.olid;
        var items = olitem.filter("[pid=" + pid + "]");
        items.each(function (index) {
            var obj = { olid: Number($(this).attr("olid")), name: $(this).find("a").text(),
                pid: Number($(this).attr("pid")),
                per: Number($(this).find("b:first").attr("per")), islast: false,
                level: level, isclac: false, childs: null, parent: parent
            };
            var childs = olitem.filter("[pid=" + obj.olid + "]");
            obj.islast = childs.size() <= 0;
            if (childs.size() > 0) obj.childs = build_tree(olitem, obj, level + 1);
            $(".outline[olid=" + obj.olid + "]").attr({ "islast": obj.islast, "level": level });
            arr.push(obj);
        });
        return arr;
    }
    //取最大层深
    //var level = getMaxlevel();    
    //取最大层深
    function getMaxlevel() {
        var level = 0;
        $(".outline").each(function () {
            var lv = Number($(this).attr("level"));
            level = lv > level ? lv : level;
        });
        return level;
    }
    for (var i = getMaxlevel(); i > 0; i--) {
        $(".outline[level=" + i + "]").each(function () {
            var pid = Number($(this).attr("pid"));
            //父对象
            var parent = $(".outline[olid=" + pid + "]");
            if (parent.size() < 1) return true;
            if (parent.attr("isclac") == "true") return true;
            //同级同父对象
            var samelevel = $(".outline[pid=" + pid + "]");
            var per = clac_samelevel($(".outline[pid=" + pid + "]"));
            var parentPer = Number(parent.find("b:first").attr("per"));
            parentPer = parentPer < per ? per : parentPer;
            parent.find("b:first").attr("per", parentPer).attr("isclac", "true");
        });
    }
    //计算同级
    function clac_samelevel(rows) {
        if (rows.size() < 1) return 0;
        var sum = 0;
        rows.each(function () {
            var per = Number($(this).find("b:first").attr("per"));
            sum += per;
        });
        return sum / rows.size();
    }
    var average = clac_samelevel($(".outline[pid=0]"));
    $(".cou-rate").attr("rate", average).text(Math.floor(average*10)/10+"%");
}
