using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using WeiSha.Data;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// ѧϰ������
    /// </summary>
    public interface ILearningCard : WeiSha.Common.IBusinessInterface
    {
        #region ѧϰ�����ù���
        /// <summary>
        /// ���ѧϰ��������
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void SetAdd(LearningCardSet entity);
        /// <summary>
        /// �޸�ѧϰ��������
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void SetSave(LearningCardSet entity);
        /// <summary>
        /// ɾ��ѧϰ��������
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void SetDelete(LearningCardSet entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void SetDelete(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        LearningCardSet SetSingle(int identify);
        /// <summary>
        /// ��ȡ����������
        /// </summary>
        /// <param name="orgid">���ڻ���id</param>
        /// <param name="isEnable"></param>
        /// <returns></returns>
        LearningCardSet[] SetCount(int orgid, bool? isEnable, int count);
        /// <summary>
        /// ��������������
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <returns></returns>
        int SetOfCount(int orgid, bool? isEnable);
        /// <summary>
        /// ��ҳ��ȡѧϰ��������
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        LearningCardSet[] SetPager(int orgid, bool? isEnable, string searTxt, int size, int index, out int countSum);
        #endregion

        #region �����γ�
        /// <summary>
        /// ��ȡ�����Ŀγ�
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        Course[] CoursesGet(LearningCardSet set);
        Course[] CoursesGet(string xml);
        /// <summary>
        /// ���ù����Ŀγ�
        /// </summary>
        /// <param name="set"></param>
        /// <param name="courses"></param>
        /// <returns>LearningCardSet�����е�Lcs_RelatedCourses����¼������Ϣ</returns>
        LearningCardSet CoursesSet(LearningCardSet set, Course[] courses);
        LearningCardSet CoursesSet(LearningCardSet set, int[] couid);
        /// <summary>
        /// ���ù����Ŀγ�
        /// </summary>
        /// <param name="set"></param>
        /// <param name="couids">�γ�id�����Զ��ŷָ�</param>
        /// <returns></returns>
        LearningCardSet CoursesSet(LearningCardSet set, string  couids);
        #endregion

        #region ѧϰ������
        /// <summary>
        /// ����ѧϰ��
        /// </summary>
        /// <param name="set">ѧϰ����������</param>
        /// <param name="factor">�������</param>
        /// <returns></returns>
        LearningCard CardGenerate(LearningCardSet set, int factor = -1);
        /// <summary>
        /// ��������ѧϰ��
        /// </summary>
        /// <param name="set">ѧϰ����������</param>        
        /// <returns></returns>
        LearningCard[] CardGenerate(LearningCardSet set);
        /// <summary>
        /// <param name="tran">����</param>
        /// </summary>
        /// <param name="set"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        LearningCard[] CardGenerate(LearningCardSet set, DbTrans tran);
        /// <summary>
        /// ���ѧϰ��������
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void CardAdd(LearningCard entity);
        /// <summary>
        /// �޸�ѧϰ��������
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void CardSave(LearningCard entity);
        /// <summary>
        /// ɾ��ѧϰ��������
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void CardDelete(LearningCard entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void CardDelete(int identify);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        LearningCard CardSingle(int identify);
        /// <summary>
        /// У��ѧϰ���Ƿ���ڣ������
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        LearningCard CardCheck(string code);
        /// <summary>
        /// ʹ�ø�ѧϰ��
        /// </summary>
        /// <param name="entity"></param>
        void CardUse(LearningCard entity);
        /// <summary>
        /// ѧϰ��ʹ�ú�Ļع�����ɾ��ѧԱ�Ĺ����γ�
        /// </summary>
        /// <param name="entity"></param>
        void CardRollback(LearningCard entity);
        /// <summary>
        /// ��ȡ����������
        /// </summary>
        /// <param name="orgid">���ڻ���id</param>
        /// <param name="orgid">����id</param>
        /// <param name="lcsid">ѧϰ���������id</param>
        /// <param name="isEnable">�Ƿ�����</param>
        /// <param name="isUsed">�Ƿ��Ѿ�ʹ��</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        LearningCard[] CardCount(int orgid, int lcsid, bool? isEnable, bool? isUsed, int count);
        /// <summary>
        /// ��������������
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="lcsid">�����������id</param>
        /// <param name="isEnable">�Ƿ�����</param>
        /// <param name="isUsed">�Ƿ��Ѿ�ʹ��</param>
        /// <returns></returns>
        int CardOfCount(int orgid, int lcsid, bool? isEnable, bool? isUsed);
        /// <summary>
        /// ����Excel��ʽ��ѧϰ����Ϣ
        /// </summary>
        /// <param name="path">�����ļ���·�����������ˣ�</param>
        /// <param name="orgid">����id</param>
        /// <param name="rsid">ѧϰ���������id</param>
        /// <returns></returns>
        string Card4Excel(string path, int orgid, int rsid);
        /// <summary>
        /// ��ҳ��ȡѧϰ��������
        /// </summary>
        /// <param name="orgid">����id</param>
        /// <param name="lcsid">ѧϰ���������id</param>
        /// <param name="isEnable">�Ƿ�����</param>
        /// <param name="isUsed">�Ƿ��Ѿ�ʹ��</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        LearningCard[] CardPager(int orgid, int lcsid, bool? isEnable, bool? isUsed, int size, int index, out int countSum);
        #endregion
    }
}
