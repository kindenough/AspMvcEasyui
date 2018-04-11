using System;
using System.Collections.Generic;
using System.Text;
using Zephyr.Core;

namespace Zephyr.Models
{
    [Module("Mms")]
    public class REMP_StaffService : ServiceBase<REMP_Staff>
    {
       
    }

    public class REMP_Staff : ModelBase
    {
        [Identity]
        [PrimaryKey]   
        public int EMPSID { get; set; }
        public int? DEPTID { get; set; }
        public string UserName { get; set; }
        public string EmployeeName { get; set; }
        public string DisplayName { get; set; }
        public string RealNameSpell { get; set; }
        public int? UserType { get; set; }
        public string EmployeeCode { get; set; }
        public string Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string Nationality { get; set; }
        public string Folk { get; set; }
        public string Native { get; set; }
        public string VoucherName { get; set; }
        public string UserIndentity { get; set; }
        public string Register { get; set; }
        public string OfficeAddress { get; set; }
        public string HomeAddress { get; set; }
        public string HomeZipCode { get; set; }
        public string OfficeFax { get; set; }
        public int? IsStaff { get; set; }
        public DateTime? InDutyDate { get; set; }
        public string TryDutyEndDate { get; set; }
        public string Duty { get; set; }
        public string OutDuty { get; set; }
        public string DutyLevel { get; set; }
        public string Polity { get; set; }
        public string Marry { get; set; }
        public string Diploma { get; set; }
        public string College { get; set; }
        public DateTime? Graduationtime { get; set; }
        public string Major { get; set; }
        public string ProfessionalTitle { get; set; }
        public string ProfessionalTitleNo { get; set; }
        public string ProfessionalTime { get; set; }
        public string WorkCategory { get; set; }
        public string UserWeight { get; set; }
        public string UserHeight { get; set; }
        public string UserLike { get; set; }
        public string Special { get; set; }
        public DateTime? SistartDate { get; set; }
        public string SocialInsurance { get; set; }
        public int? IsPerformance { get; set; }
        public string TalkMan { get; set; }
        public string ParentMan { get; set; }
        public string PointMan { get; set; }
        public string CarNumber { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string Telephone3 { get; set; }
        public string Mobile1 { get; set; }
        public string Mobile2 { get; set; }
        public string Mobile3 { get; set; }
        public string OICQ { get; set; }
        public string Email { get; set; }
        public string NetAddress { get; set; }
        public string SkypeUser { get; set; }
        public string EmergencyMan { get; set; }
        public string EmergencyRelation { get; set; }
        public string EmergencyPhone { get; set; }
        public string EmergencyMobile { get; set; }
        public DateTime? SocialinsuranceOut { get; set; }
        public DateTime? ConstarDate { get; set; }
        public DateTime? ConendDate { get; set; }
        public int? IsWagePay { get; set; }
        public string WageLevel { get; set; }
        public string WageCompany { get; set; }
        public string WagePassword { get; set; }
        public string WageAccounts { get; set; }
        public int? GroupID { get; set; }
        public int? CompanyId { get; set; }
        public int? DepartmentId { get; set; }
        public int? WorkgroupId { get; set; }
        public int? RoleId { get; set; }
        public int? DeletionStateCode { get; set; }
        public int Enabled { get; set; }
        public int? SortCode { get; set; }
        public string Description { get; set; }
        public DateTime? CreateOn { get; set; }
        public int? CreateUserId { get; set; }
        public string CreateBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedUserId { get; set; }
        public string ModifiedBy { get; set; }
    }
}
