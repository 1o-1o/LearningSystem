using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// Ժϵְλ�Ĺ���
    /// </summary>
    public interface ISystemPara : WeiSha.Common.IBusinessInterface, System.Collections.IEnumerable
    {

        /// <summary>
        /// ���
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void Add(SystemPara entity);
        /// <summary>
        /// �޸ģ�������ˢ��ȫ�ֲ���
        /// </summary>
        /// <param name="key">������</param>
        /// <param name="value">����ֵ</param>
        void Save(string key, string value);
        /// <summary>
        /// �޸ģ����Ƿ�ֱ��ˢ��ȫ�ֲ���
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="isRefresh"></param>
        void Save(string key, string value, bool isRefresh);
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="key">������</param>
        /// <param name="value">����ֵ</param>
        /// <param name="unit">�����ĵ�λ</param>
        void Save(string key, string value, string unit);
        /// <summary>
        /// ��ʵ������
        /// </summary>
        /// <param name="entity"></param>
        void Save(SystemPara entity);
        /// <summary>
        /// ��ǰ�����Ƿ���ڣ�ͨ���������жϣ�
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>����Ѿ����ڣ��򷵻�true</returns>
        bool IsExists(SystemPara entity);
        /// <summary>
        /// ˢ��ȫ�ֲ���
        /// </summary>
        List<SystemPara> Refresh();
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void Delete(SystemPara entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void Delete(int identify);
        /// <summary>
        /// ɾ��������ֵ
        /// </summary>
        /// <param name="key"></param>
        void Delete(string key);
        /// <summary>
        /// ���ݼ�����ȡֵ
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetValue(string key);
        /// <summary>
        /// ���ݼ�����ȡֵ
        /// </summary>
        /// <param name="key">��ֵ</param>
        /// <returns></returns>
        WeiSha.Common.Param.Method.ConvertToAnyValue this[string key] { get;}        
        /// <summary>
        /// ��ȡ����ʵ��
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SystemPara GetSingle(int id);
        /// <summary>
        /// ��ȡ����ʵ����ͨ����ֵ��ȡ
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        SystemPara GetSingle(string key);
        /// <summary>
        /// ��ȡ���в���
        /// </summary>
        /// <returns></returns>
        DataTable GetAll();
        /// <summary>
        /// ��ѯ��ȡ����
        /// </summary>
        /// <param name="searKey">����</param>
        /// <param name="searIntro">����˵��</param>
        /// <returns></returns>
        DataTable GetAll(string searKey, string searIntro);
        /// <summary>
        /// ������ˮ��
        /// </summary>
        /// <returns></returns>
        string Serial();
        /// <summary>
        /// �����Ƿ������Ȩ
        /// </summary>
        bool IsLicense();
        /// <summary>
        /// ���ݿ������Բ���
        /// </summary>
        /// <returns>����ȱ�ٵı����ֶ�</returns>
        List<string> DatabaseCompleteTest();
        /// <summary>
        /// ���ݿ����Ӳ���
        /// </summary>
        /// <returns>������ȷΪtrue������Ϊfalse</returns>
        bool DatabaseLinkTest();
        /// <summary>
        /// ִ��sql���,����Ӱ�������
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>����Ӱ�������</returns>
        int ExecuteSql(string sql);
        /// <summary>
        /// ִ��sql��䣬���ص�һ�е�һ�е�����
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>���ص�һ�е�һ�е�����</returns>
        object ScalarSql(string sql);
        /// <summary>
        /// ִ��sql���
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>�������ݼ�</returns>
        List<T> ForSql<T>(string sql) where T : WeiSha.Data.Entity;
        /// <summary>
        /// ����ָ�������ݼ�
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        DataTable ForSql(string sql);

    }
}
