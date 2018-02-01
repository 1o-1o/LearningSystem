﻿using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.WebControl;
using System.Collections.Generic;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;

namespace Song.Site.Manage.Teacher
{
    public partial class Statistics_Details : Extend.CustomPage
    {
        //考试主题的id
        private int examid = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        private string time = WeiSha.Common.Request.QueryString["time"].ToString();
        Song.Entities.Organization org;
        //各场次
        Song.Entities.Examination[] exams = null;
        //学员成绩
        DataTable dtResults = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                dtResults = Business.Do<IExamination>().Result4Theme(examid, -1);
                fill();
                Sts_ID_SelectedIndexChanged(null, null);
            }
        }

        void fill()
        {  
            //学生分组
            Song.Entities.StudentSort[] sort = Business.Do<IExamination>().StudentSort4Theme(examid);
            Sts_ID.DataSource = sort;
            Sts_ID.DataBind();
            Sts_ID.Items.Insert(0, new ListItem("-- 所有学员 --", "0"));
            Sts_ID.Items.Add(new ListItem("-- 未分组学员 --", "-1"));
        }
        /// <summary>
        /// 获取场次的平均分
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected double GetAvg(string id)
        {
            int mid = Convert.ToInt32(id);
            Song.Entities.Examination curr = null;
            if (exams != null)
            {
                foreach (Song.Entities.Examination em in exams)
                {
                    if (mid == em.Exam_ID)
                    {
                        curr = em;
                        break;
                    }
                }
            }
            if (curr == null) return 0;
            double res = Business.Do<IExamination>().Avg4Exam(curr.Exam_ID);
            return Math.Round(Math.Round(res * 10000) / 10000,2,MidpointRounding.AwayFromZero);
           
        }

        protected void Sts_ID_SelectedIndexChanged(object sender, EventArgs e)
        {
            //考试主题的平均成绩
            Song.Entities.Examination theme = Business.Do<IExamination>().ExamSingle(examid);
            if (theme == null) return;
            //各专业的平均成绩
            exams = Business.Do<IExamination>().ExamItem(theme.Exam_UID);
            rptExamItem.DataSource = exams;
            rptExamItem.DataBind();

            //考试主题下的所有参考人员成绩
            int stsid = Convert.ToInt32(Sts_ID.SelectedValue);
            DataTable dt=null;
            if (stsid == -1) dt = dtResults;
            if (dt == null) dt = Business.Do<IExamination>().Result4Theme(examid, stsid);
            gvList.DataSource = dt;
            gvList.DataBind();
        }

        protected void btnOutput_Click(object sender, EventArgs e)
        {
            //考试主题的平均成绩
            Song.Entities.Examination theme = Business.Do<IExamination>().ExamSingle(examid);            

            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            buildExcelSql_1(hssfworkbook);           

            //创建文件
            string name = theme.Exam_Title + "-考试成绩" + ".xls";
            string filePath = Upload.Get["Temp"].Physics + name;
            FileStream file = new FileStream(filePath, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            if (System.IO.File.Exists(filePath))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileInfo.Name));
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.AddHeader("Content-Transfer-Encoding", "binary");
                Response.ContentType = "application/-excel";
                Response.ContentEncoding = System.Text.Encoding.Default;
                Response.WriteFile(fileInfo.FullName);
                Response.Flush();
                Response.End();
                File.Delete(filePath);
            }
        }
        /// <summary>
        /// 导出成绩
        /// </summary>
        /// <param name="hssfworkbook"></param>
        private void buildExcelSql_1(HSSFWorkbook hssfworkbook)
        {
            //考试主题下的所有参考人员（分过组的）成绩
            foreach (ListItem li in Sts_ID.Items)
            {
                int stsid = Convert.ToInt32(li.Value);
                if (stsid < 0) continue;
                DataTable dt = Business.Do<IExamination>().Result4Theme(examid, stsid);
                if (dt.Rows.Count < 1) continue;                
                ISheet sheet = hssfworkbook.CreateSheet(li.Text);   //创建工作簿对象                
                IRow rowHead = sheet.CreateRow(0);          //创建数据行对象                
                for (int i = 0; i < dt.Columns.Count; i++)      //创建表头
                    rowHead.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);                
                ICellStyle style_size = hssfworkbook.CreateCellStyle();     //生成数据行
                style_size.WrapText = true;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow row = sheet.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        row.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());                        
                    }
                }
            }
        }
        /// <summary>
        /// 删除考试成绩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDel_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int acid = 0;   //学员id
            int.TryParse(btn.CommandArgument,out acid);
            //当前考试主题下的所有考试
            Song.Entities.Examination[] exams = Business.Do<IExamination>().ExamItem(examid);
            foreach (Song.Entities.Examination exam in exams)
            {
                Business.Do<IExamination>().ResultDelete(acid, exam.Exam_ID);
            }
            Sts_ID_SelectedIndexChanged(null, null);
        }
    }
}
