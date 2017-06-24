using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ChuanglitouP2P.Model;

using ChuangLitouP2P.Models;
using ChuanglitouP2P.Common;
namespace ChuanglitouP2P.DAL.Api
{
    public class ContractManagementDal
    {
        chuangtouEntities ef = new chuangtouEntities();
        public M_Contract_management GetContract(int contractId)
        {
            var sql = "SELECT [contract],[loan_number],[targetid],[lender_name],[lender_username],[lender_registerid],[lender_id_card],[lenders_account_name],[lender_bank_account],[lender_bank],[lender_address],[lenders_telephone],[lenders_email],[lendres_date_contract],[borrower_registerid],[borrower_name],[borrower_username],[borrower_id_card],[borrower_account_name],[borrower_bank_account],[borrower_date_contract],[borrower_bank],[witness_name],[witness_address],[witness_telephone],[witness_agent],[witness_admid],[witness_username],[witness_id_card],[witness_date_contract],[surety_company_name],[guarantee_business_icense],[ensure_address],[guarantee_legal_representative],[guarantor_agent],[guarantor_companyid],[guarantor_agent_usernqme],[guarantor_agent_id_card],[guarantor_agent_idate_contract],[contract_money],[counselling_service],[contract_amount],[suretyship_contract],[mode_payment],[createtime],[contract_type],[contractpath],[bid_records_id] FROM[Contract_management] where Contract=" + contractId.ToString();
            var ds = DbHelper.Query(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return GetEntity(ds.Tables[0].Rows[0]);
            return null;
        }

        public List<M_Contract_management> GetContractListForApp(int targetId, int bidRecordsId = 0, bool orderbyContractAsc = true)
        {
            List<M_Contract_management> result = new List<M_Contract_management>();
            var sql = "SELECT a.[contract],a.[loan_number],a.[targetid],a.[lender_name],a.[lender_username],a.[lender_registerid],a.[lender_id_card],a.[lenders_account_name],a.[lender_bank_account],a.[lender_bank],a.[lender_address],a.[lenders_telephone],a.[lenders_email],a.[lendres_date_contract],a.[borrower_registerid],a.[borrower_name],a.[borrower_username],a.[borrower_id_card],a.[borrower_account_name],a.[borrower_bank_account],a.[borrower_date_contract],a.[borrower_bank],a.[witness_name],a.[witness_address],a.[witness_telephone],a.[witness_agent],a.[witness_admid],a.[witness_username],a.[witness_id_card],a.[witness_date_contract],a.[surety_company_name],a.[guarantee_business_icense],a.[ensure_address],a.[guarantee_legal_representative],a.[guarantor_agent],a.[guarantor_companyid],a.[guarantor_agent_usernqme],a.[guarantor_agent_id_card],a.[guarantor_agent_idate_contract],a.[contract_money],a.[counselling_service],a.[contract_amount],a.[suretyship_contract],a.[mode_payment],a.[createtime],a.[contract_type],a.[contractpath],a.[bid_records_id],b.start_time,b.end_time FROM [Contract_management] a left join hx_borrowing_target b on b.targetid = a.targetid where a.targetid =" + targetId.ToString() + " AND a.bid_records_id=" + bidRecordsId.ToString() + " order by a.contract ";
            if (!orderbyContractAsc)
                sql += " desc ";
            var ds = DbHelper.Query(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    int tid = Convert.ToInt32(row["targetid"]);
                    var target = ef.hx_borrowing_target.Where(a => a.targetid == tid);
                    row["End_Time"] = target.ToList().FirstOrDefault().repayment_date.Value;
                    var entity = GetEntity(row);
                    result.Add(entity);
                }
            }
            return result;
        }

        protected M_Contract_management GetEntity(DataRow row)
        {
            if (row == null) return null;
            M_Contract_management result = new M_Contract_management();
            result.contract = row["contract"] == null ? -1 : (int)row["contract"];
            result.loan_number = row["loan_number"] == null ? -1 : (decimal)row["loan_number"];
            result.targetid = row["targetid"] == null ? -1 : (int)row["targetid"];
            result.lender_name = row["lender_name"] == null ? string.Empty : row["lender_name"].ToString();
            result.lender_username = row["lender_username"] == null ? string.Empty : row["lender_username"].ToString();
            result.lender_registerid = row["lender_registerid"] == null ? -1 : (int)row["lender_registerid"];
            result.lender_id_card = row["lender_id_card"] == null ? string.Empty : row["lender_id_card"].ToString();
            result.lenders_account_name = row["lenders_account_name"] == null ? string.Empty : row["lenders_account_name"].ToString();
            result.lender_bank_account = row["lender_bank_account"] == null ? string.Empty : row["lender_bank_account"].ToString();
            result.lender_bank = row["lender_bank"] == null ? string.Empty : row["lender_bank"].ToString();
            result.lender_address = row["lender_address"] == null ? string.Empty : row["lender_address"].ToString();
            result.lenders_telephone = row["lenders_telephone"] == null ? string.Empty : row["lenders_telephone"].ToString();
            result.lenders_email = row["lenders_email"] == null ? string.Empty : row["lenders_email"].ToString();
            result.lendres_date_contract = row["lendres_date_contract"] == null ? DateTime.MinValue : Convert.ToDateTime(row["lendres_date_contract"]);
            result.borrower_registerid = row["borrower_registerid"] == null ? -1 : (int)row["borrower_registerid"];
            result.borrower_name = row["borrower_name"] == null ? string.Empty : row["borrower_name"].ToString();
            result.borrower_username = row["borrower_username"] == null ? string.Empty : row["borrower_username"].ToString();
            result.borrower_id_card = row["borrower_id_card"] == null ? string.Empty : row["borrower_id_card"].ToString();
            result.borrower_account_name = row["borrower_account_name"] == null ? string.Empty : row["borrower_account_name"].ToString();
            result.borrower_bank_account = row["borrower_bank_account"] == null ? string.Empty : row["borrower_bank_account"].ToString();
            result.borrower_date_contract = row["borrower_date_contract"] == null ? string.Empty : row["borrower_date_contract"].ToString();
            result.borrower_bank = row["borrower_bank"] == null ? string.Empty : row["borrower_bank"].ToString();
            result.witness_name = row["witness_name"] == null ? string.Empty : row["witness_name"].ToString();
            result.witness_address = row["witness_address"] == null ? string.Empty : row["witness_address"].ToString();
            result.witness_telephone = row["witness_telephone"] == null ? string.Empty : row["witness_telephone"].ToString();
            result.witness_agent = row["witness_agent"] == null ? string.Empty : row["witness_agent"].ToString();
            result.witness_admid = row["witness_admid"] == null ? -1 : (int)row["witness_admid"];
            result.witness_username = row["witness_username"] == null ? string.Empty : row["witness_username"].ToString();
            result.witness_id_card = row["witness_id_card"] == null ? string.Empty : row["witness_id_card"].ToString();
            result.witness_date_contract = row["witness_date_contract"] == null ? DateTime.MinValue : Convert.ToDateTime(row["witness_date_contract"]);
            result.surety_company_name = row["surety_company_name"] == null ? string.Empty : row["surety_company_name"].ToString();
            result.guarantee_business_icense = row["guarantee_business_icense"] == null ? string.Empty : row["guarantee_business_icense"].ToString();
            result.ensure_address = row["ensure_address"] == null ? string.Empty : row["ensure_address"].ToString();
            result.guarantee_legal_representative = row["guarantee_legal_representative"] == null ? string.Empty : row["guarantee_legal_representative"].ToString();
            result.guarantor_agent = row["guarantor_agent"] == null ? string.Empty : row["guarantor_agent"].ToString();
            result.guarantor_companyid = row["guarantor_companyid"] == null ? -1 : (int)row["guarantor_companyid"];
            result.guarantor_agent_usernqme = row["guarantor_agent_usernqme"] == null ? string.Empty : row["guarantor_agent_usernqme"].ToString();
            result.guarantor_agent_id_card = row["guarantor_agent_id_card"] == null ? string.Empty : row["guarantor_agent_id_card"].ToString();
            result.guarantor_agent_idate_contract = row["guarantor_agent_idate_contract"] == null ? DateTime.MinValue : Convert.ToDateTime(row["guarantor_agent_idate_contract"]);
            result.contract_money = row["contract_money"] == null ? string.Empty : row["contract_money"].ToString();
            result.counselling_service = row["counselling_service"] == null ? string.Empty : row["counselling_service"].ToString();
            result.contract_amount = row["contract_amount"] == null ? 0.00M : Convert.ToDecimal(row["contract_amount"]);
            result.suretyship_contract = row["suretyship_contract"] == null ? string.Empty : row["suretyship_contract"].ToString();
            result.mode_payment = row["mode_payment"] == null ? string.Empty : row["mode_payment"].ToString();
            result.createtime = row["createtime"] == null ? DateTime.MinValue : Convert.ToDateTime(row["createtime"]);
            result.contract_type = row["contract_type"] == null ? -1 : (int)row["contract_type"];
            result.contractpath = row["contractpath"] == null ? string.Empty : row["contractpath"].ToString();
            result.bid_records_id = row["bid_records_id"] == null ? -1 : (int)row["bid_records_id"];
            result.Start_Time = row["start_time"] == null ? string.Empty : ((DateTime)row["start_time"]).ToString("yyyy-MM-dd HH:mm:ss");
            result.End_Time = row["End_Time"] == null ? string.Empty : ((DateTime)row["End_Time"]).ToString("yyyy-MM-dd HH:mm:ss");
            if (!string.IsNullOrWhiteSpace(result.Start_Time) && !string.IsNullOrWhiteSpace(result.End_Time))
            {
                result.DurationTime = Utils.GetTargetDurationDays(result.Start_Time, result.End_Time).ToString();
                result.Start_Time = result.Start_Time.Split(' ')[0].Replace("/", "-");
                result.End_Time = result.End_Time.Split(' ')[0].Replace("/", "-");
            }
            return result;
        }
    }
}
