using AttenceProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttenceProject.Services.Face
{
    public interface ISysVacation
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
        int SaveAdd(HttpCookie cook, string VacationReason, string VacationType, string Emergency, string StartTime, string EndTime, string SendFor, string CopyFor);

        /// <summary>
        /// 获取申请列表
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        IList<SysVacation> GetList(string userid);

        /// <summary>
        /// 抄送人获取申请列表
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        IList<SysVacation> GetCopyList(string userid);

        /// <summary>
        /// 获取审批信息
        /// </summary>
        /// <param name="cook"></param>
        /// <returns></returns>
        string GetApprove(HttpCookie cook);

        /// <summary>
        /// 获取加班信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetVacationInfo(int id);

        /// <summary>
        /// 保存加班信息
        /// </summary>
        /// <param name="cook"></param>
        /// <param name="applyrate"></param>
        /// <param name="applystatus"></param>
        /// <param name="applyid"></param>
        /// <returns></returns>
        int SaveApprove(HttpCookie cook,string applyrate, string applystatus, string applyid);

        /// <summary>
        /// 获取审批详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetApproveDetail(int id);

        string GetApproveCopy(HttpCookie cook);
    }
}