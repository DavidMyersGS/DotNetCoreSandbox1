using System;
using System.Data;
using System.Data.Odbc;
using GameStop.SupplyChain.DataContracts.ThinkGeek.SKUUpsert;

namespace WMISKUWeb.Models
{
    public class DataLayer : IDataLayer
    {
        private readonly string _dbConnectionString = "DRIVER=iSeries Access ODBC Driver;Database = D10984fb;PKG=QGPL/DEFAULT(IBM),2,0,1,0,512;LANGUAGEID = ENU;DFTPKGLIB = QGPL;DBQ = WMWQ1LOUDB;System = GV1DWH01;Uid=s_dwhodbc;Pwd=p955thru6rt;";
        private readonly string _sqlToExecute = "CALL WMWD1LOUDC.GS_I5INPT00_INSERT(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

        public DataLayer()
        {

        }

        public int InsertSKU(string json)
        {
            SKUUpsertMessageContract obj = new SKUUpsertMessageContract();

            SKUUpsertMessageContract skuUpsert = (SKUUpsertMessageContract)JSONHelper.JSONToObject(obj, json);

            OdbcConnection con = new OdbcConnection(_dbConnectionString);
            OdbcCommand cmd = new OdbcCommand();
            cmd.Connection = con;
            cmd.CommandText = _sqlToExecute;
            cmd.CommandType = CommandType.StoredProcedure;
            
            cmd.Parameters.AddWithValue("P_ERROR_SEQ", 0);
            cmd.Parameters.AddWithValue("P_DATEPROC", 0);
            cmd.Parameters.AddWithValue("P_TIMEPROC", 0);
            cmd.Parameters.AddWithValue("P_BATCH_CTL_NUM", "1");
            cmd.Parameters.AddWithValue("P_DATECREATED", DateTime.Now.ToString("yyyyMMdd"));
            cmd.Parameters.AddWithValue("P_TIMECREATED", DateTime.Now.ToString("hhmmss"));
            cmd.Parameters.AddWithValue("P_USERID","WMISKU");
            cmd.Parameters.AddWithValue("P_COMPANY",skuUpsert.MessageMetadata.Company);
            cmd.Parameters.AddWithValue("P_DIVISION",skuUpsert.MessageMetadata.Brand);
            cmd.Parameters.AddWithValue("P_SKU",skuUpsert.SKU);
            cmd.Parameters.AddWithValue("P_SEQNUM",1);
            cmd.Parameters.AddWithValue("P_CAT",skuUpsert.ProductCategory.Substring(0,3));
            cmd.Parameters.AddWithValue("P_TITLE",skuUpsert.ProductName.ToString());
            cmd.Parameters.AddWithValue("P_PRIMARY_UPC",skuUpsert.identifiers.warehoue_barcode);
            cmd.Parameters.AddWithValue("P_PRICE",1.00);
            cmd.Parameters.AddWithValue("P_COST",1.00);
            cmd.Parameters.AddWithValue("P_INNERPACK",1);
            cmd.Parameters.AddWithValue("P_BOXQTY",1);
            cmd.Parameters.AddWithValue("P_ACTIVATION_DATE", 0);
            cmd.Parameters.AddWithValue("P_TARIFF_CODE","");
            cmd.Parameters.AddWithValue("P_COO","USA");
            cmd.Parameters.AddWithValue("P_MULTI_COO","N");
            cmd.Parameters.AddWithValue("P_TOP_SHELF","N");
            cmd.Parameters.AddWithValue("P_VENDOR","");
            cmd.Parameters.AddWithValue("P_USED_FLAG","N");
            cmd.Parameters.AddWithValue("P_STATUS","10");
            cmd.Parameters.AddWithValue("P_DB_FUNCTION","2");
            cmd.Parameters.AddWithValue("P_PRODUCER","N");
            cmd.Parameters.AddWithValue("P_NET_COST_VALID","N");
            cmd.Parameters.AddWithValue("P_LOT_CONTROL_USED","N");
            cmd.Parameters.AddWithValue("P_STATUS_DESC","");
            cmd.Parameters.AddWithValue("P_NMFC_CODE","");
            cmd.Parameters.AddWithValue("P_TRK_SKU_ATTR","N");
            cmd.Parameters.AddWithValue("P_RETURN_FLAG","R");
            cmd.Parameters.AddWithValue("P_STSC2","N");
            cmd.Parameters.AddWithValue("P_STCREX","");
            cmd.Parameters.AddWithValue("P_STSC3","N");
            cmd.Parameters.AddWithValue("P_STSC4","");

            int affectedRows = 0;
            try
            {
                con.Open();
                affectedRows = cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

            return affectedRows;
        }
    }
}
