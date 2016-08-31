using System;
using System.Data;
using System.Data.Odbc;

namespace ToDoApi.Controllers
{
    public class DBTest
    {
        public string GetCarton()
        {
            OdbcConnection con = new OdbcConnection("DRIVER=iSeries Access ODBC Driver;Database = D10984fb;PKG=QGPL/DEFAULT(IBM),2,0,1,0,512;LANGUAGEID = ENU;DFTPKGLIB = QGPL;DBQ = WMWQ1LOUDB;System = GV1DWH01;Uid=s_dwhodbc;Pwd=p@55thru6rt;");
            con.Open();

                try {
                    using (OdbcCommand command = new OdbcCommand("SELECT * FROM wmwd1loudb.ststyl00 fetch first 10 rows only", con)) {
                        OdbcDataReader reader = command.ExecuteReader();
                    }
                }
                catch {
                    Console.WriteLine("Something went wrong");
                }

            Console.Read();
            return "stuff";
        }
    }
}
