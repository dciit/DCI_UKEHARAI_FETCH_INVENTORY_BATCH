using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DCI_UKEHARAI_FETCH_INVENTORY_BATCH
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int milliseconds = 100;
            OraConnectDB ALPHAPD = new OraConnectDB("ALPHAPD");
            SqlConnectDB SCM = new SqlConnectDB("dbSCM");
            Console.WriteLine("----- Start BATCH ------ ");
            Console.WriteLine("----- For Support Inventory Ukeharai Warning ------ ");
            OracleCommand str = new OracleCommand();
            str.CommandText = @"select model, pltype, count(serial) cnt,to_char(current_date,'YYYY-MM-DD') as currentDate
from fh001 
where comid='DCI' and nwc in ('DCI','SKO')  
  and locacode like '%'
group by model, pltype
order by model";
            DataTable dt = ALPHAPD.Query(str);
            foreach (DataRow dr in dt.Rows)
            {
                string model = dr["MODEL"].ToString().Trim();
                string pltype = dr["PLTYPE"].ToString();
                int cnt = dr["CNT"].ToString() != "" ? int.Parse(dr["CNT"].ToString()) : 0;
                string currentDate = dr["CURRENTDATE"].ToString().Replace("-", "");
                Console.WriteLine($"{model} {pltype} {cnt} {currentDate}");
                SqlCommand InvExist = new SqlCommand();
                InvExist.CommandText = @"SELECT CNT FROM UKE_ALPHA_INVENTORY WHERE MODEL = @MODEL AND PLTYPE = @PLTYPE AND YMD = @YMD";
                InvExist.Parameters.Add(new SqlParameter("@MODEL", model));
                InvExist.Parameters.Add(new SqlParameter("@PLTYPE", pltype));
                InvExist.Parameters.Add(new SqlParameter("@YMD", currentDate));
                DataTable dtInvExist = SCM.Query(InvExist);
                if (dtInvExist.Rows.Count > 0)
                {
                    int existCnt = dtInvExist.Rows[0]["CNT"].ToString() != "" ? int.Parse(dtInvExist.Rows[0]["CNT"].ToString()) : 0;
                    if (cnt != existCnt)
                    {
                        Console.WriteLine("----- Have Model : " + model + " ------");
                        SqlCommand InvUpdate = new SqlCommand();
                        InvUpdate.CommandText = @"UPDATE UKE_ALPHA_INVENTORY SET CNT = @CNT WHERE MODEL = @MODEL AND PLTYPE = @PLTYPE AND YMD = @YMD";
                        InvUpdate.Parameters.Add(new SqlParameter("@MODEL", model));
                        InvUpdate.Parameters.Add(new SqlParameter("@PLTYPE", pltype));
                        InvUpdate.Parameters.Add(new SqlParameter("@CNT", cnt));
                        InvUpdate.Parameters.Add(new SqlParameter("@YMD", currentDate));
                        int update = SCM.ExecuteNonCommand(InvUpdate);
                        Console.WriteLine("----- Update Model : " + model + " Before : " + existCnt + " To " + cnt + " Status : " + update + " ------");
                    }
                }else
                {
                    SqlCommand strInsertInv = new SqlCommand();
                    strInsertInv.CommandText = @"INSERT INTO UKE_ALPHA_INVENTORY (MODEL,PLTYPE,CNT,YMD) VALUES (@MODEL,@PLTYPE,@CNT,@YMD) ";
                    strInsertInv.Parameters.Add(new SqlParameter("@MODEL", model));
                    strInsertInv.Parameters.Add(new SqlParameter("@PLTYPE", pltype));
                    strInsertInv.Parameters.Add(new SqlParameter("@CNT", cnt));
                    strInsertInv.Parameters.Add(new SqlParameter("@YMD", currentDate));
                    int insert = SCM.ExecuteNonCommand(strInsertInv);
                    Console.WriteLine("----- Insert Model : " + model + " Inventory :  " + cnt + " Status : " + insert + " ------");
                }
                Thread.Sleep(milliseconds);
            }
            Console.WriteLine("----- End BATCH ------");
        }
    }
}
