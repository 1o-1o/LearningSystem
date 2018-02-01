using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// ��־�Ĺ���
    /// </summary>
    public interface ILogs : WeiSha.Common.IBusinessInterface
    {
        /// <summary>
        /// ���ӵ�¼��־
        /// </summary>
        void AddLoginLogs();
        /// <summary>
        /// ���Ӳ�����־
        /// </summary>
        void AddOperateLogs();
        /// <summary>
        /// ���
        /// </summary>
        /// <param name="entity">ҵ��ʵ��</param>
        void Add(Logs entity);
        /// <summary>
        /// ɾ����������ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        void Delete(int identify);
        /// <summary>
        /// ���ݷ��ࡢ����idɾ��
        /// </summary>
        /// <param name="accId">�û�id</param>
        void Delete4Acc(int accId);
        /// <summary>
        /// ��ȡ��һʵ����󣬰�����ID��
        /// </summary>
        /// <param name="identify">ʵ�������</param>
        /// <returns></returns>
        Logs GetSingle(int identify);
        /// <summary>
        /// ���������֮ǰ����־
        /// </summary>
        /// <param name="day">����</param>
        /// <param name="type">��־����ݷ�Ϊlogin,operate������¼��־��������־</param>
        void Clear(int day, string type);
        /// <summary>
        /// ��ȡĳ�û�������ʵĲ�����
        /// </summary>
        /// <param name="accId">�û�id</param>
        /// <param name="type">��־����ݷ�Ϊlogin,operate������¼��־��������־</param>
        /// <param name="count">���صĸ���</param>
        /// <returns></returns>
        DataSet GetLately(int accId, string type, int count);
        /// <summary>
        /// ��ȡĳ�û�ĳʱ����ڣ����ʴ������Ĳ�����
        /// </summary>
        /// <param name="accId">�û�id</param>
        /// <param name="type">��־����ݷ�Ϊlogin,operate������¼��־��������־</param>
        /// <param name="count">���صĸ���</param>
        /// <returns></returns>
        DataSet GetFrequently(int accId, string type, int count);
        /// <summary>
        /// ��ҳ��ȡ������־��¼
        /// </summary>
        /// <param name="type">��־����ݷ�Ϊlogin,operate������¼��־��������־</param>
        /// <param name="size">ÿҳ��ʾ������¼</param>
        /// <param name="index">��ǰ�ڼ�ҳ</param>
        /// <param name="countSum">��¼����</param>
        /// <returns></returns>
        Logs[] GetPager(string type,int size, int index, out int countSum);
        Logs[] GetPager(string type,DateTime start,DateTime end, int size, int index, out int countSum);
        /// <summary>
        /// ��ҳ��ȡ������־��¼
        /// </summary>
        /// <param name="accId">Ա��id</param>
        /// <param name="type">��־����ݷ�Ϊlogin,operate������¼��־��������־</param>
        /// <param name="size">ÿҳ��ʾ������¼</param>
        /// <param name="index">��ǰ�ڼ�ҳ</param>
        /// <param name="countSum">��¼����</param>
        /// <returns></returns>
        Logs[] GetPager(int accId,string type, int size, int index, out int countSum);
        Logs[] GetPager(int accId,string type, DateTime start, DateTime end, int size, int index, out int countSum);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accId"></param>
        /// <param name="mmSear">�˵�����</param>
        /// <param name="type"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Logs[] GetPager(int accId, string mmSear,string type, DateTime start, DateTime end, int size, int index, out int countSum);
        
    }
}
