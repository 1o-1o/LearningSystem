using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.WebControl;
using System.Drawing;
using System.Reflection;
using System.Xml.Serialization;
using Spring.Globalization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Xml;
using System.Threading;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Song.Site.Manage
{
    public partial class Tester : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            string txt = System.IO.File.ReadAllText(this.MapPath("~/Templates/Web/NetSchool/Course.htm"));

            txt = _replacePath(txt, "body|link|script|img");


        }
        /// <summary>
        /// ��ģ��ҳ�е�·�������ʵ����Ҫ��·��
        /// </summary>
        /// <param name="text">ģ��Html����</param>
        /// <param name="tmFile">ģ���ļ���������ҳ��Ҫ���õ�htmlģ��ҳ</param>
        /// <param name="tags">Ҫ������html��ǩ</param>
        /// <returns></returns>
        public static string _replacePath(string text, string tags)
        {           
            RegexOptions options = RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase;
            //�������Ӵ���Ϊ�����ģ��ҳ��·��
            string linkExpr = @"<(" + tags + @")[^>]+>";
            foreach (Match match in new Regex(linkExpr, options).Matches(text))
            {
                //html��ǩ
                string tagName = match.Groups[1].Value.Trim();
                string tagContent = match.Groups[0].Value.Trim();
                
                string expr = @"(?<=\s+)(?<key>path|href|src|error|action|background[^=""']*)=([""'])?(?<value>[^'"">]*)\1?";
                foreach (Match m in new Regex(expr, options).Matches(tagContent))
                {
                    string val = m.Groups["value"].Value.Trim();
                    string key = m.Groups["key"].Value.Trim();
                    //�������Ӳ�������Http://��ͷ�ĳ�����
                    if (new Regex(@"[a-zA-z]+://[^\s]*", RegexOptions.Singleline).IsMatch(val))
                        continue;
                    //��·����������#�ţ�������
                    if (new Regex(@"^[{\\\/\#]").IsMatch(val))
                        continue;
                    //base64ͼƬ��������data:image��ͷ��������
                    if (new Regex(@"^data:image").IsMatch(val))
                        continue;
                    //base64ͼƬ��������data:image��ͷ��������
                    if (new Regex(@"^javascript:").IsMatch(val))
                        continue;
                    //��������ת��Ϊ���ڵ�ǰģ��ҳ�����·��
                    //link = RelativePath(page, tmPath + link);
                    val = m.Groups[2].Value + "=\"" + val + "\"";
                    val = Regex.Replace(val, @"//", "/");
                    tagContent = tagContent.Replace(m.Value, val);
                }
                text = text.Replace(match.Groups[0].Value.Trim(), tagContent);
            }
            return text;
        }
    }
}
