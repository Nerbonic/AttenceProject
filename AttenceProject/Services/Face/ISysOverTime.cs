using AttenceProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttenceProject.Services.Face
{
    public interface ISysOverTime
    {
        /// <summary>
        /// 获取备选项信息的通用方法
        /// </summary>
        /// <param name="groupname">分组名称</param>
        /// <returns></returns>
        string GetAlternativesData(string groupname);

        /// <summary>
        /// 获取申请时间点
        /// </summary>
        /// <param name="groupname"></param>
        /// <returns></returns>
        string GetTimePoint(string groupname);

        /// <summary>
        /// 保存申请信息和第一个审批信息
        /// </summary>
        /// <param name="cook"></param>
        /// <param name="OverTimeReason"></param>
        /// <param name="OverTimeType"></param>
        /// <param name="Account_Method"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="SendFor"></param>
        /// <param name="CopyFor"></param>
        /// <returns></returns>
        int SaveAdd(HttpCookie cook,string OverTimeReason, string OverTimeType, string Account_Method, string StartTime, string EndTime, string SendFor, string CopyFor);

        IList<SysOverTime> GetList(string userid);

        IList<SysOverTime> GetCopyList(string userid);

        string GetApprove(HttpCookie cook);

        string GetOverTimeInfo(int id);

        //int SaveApprove(string applyrate, string applystatus, string applyid);
    }
}