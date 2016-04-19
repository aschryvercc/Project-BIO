using ServiceConsumer.CSVExportService;
//using ServiceConsumer.qbExportService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ServiceConsumer
{
    public partial class index : System.Web.UI.Page
    {
        CSVExportServiceClient csves;
        //IqbExportServiceClient qbes;
        string csvToken;
        string qbToken;
        string csvString;

        protected void Page_Load(object sender, EventArgs e)
        {
            string[] csvAuthentication = new string[2];
            string[] qbAuthentication = new string[2];

            csves = new CSVExportServiceClient();
            //qbes = new IqbExportServiceClient();

            csvAuthentication = csves.authenticate("username", "thisisbad");
            //qbAuthentication = qbes.authenticate("username", "thisisbad");

            if (csvAuthentication[1].Equals("nvu")) //||
                //qbAuthentication[1].Equals("nvu"))
            {
                page_wrapper.InnerHtml = "<p>Page not available :(</p> <p>Try Refreshing...</p>";
            }
            else
            {
                csvToken = csvAuthentication[0];
                qbToken = qbAuthentication[0];

                csvString = csves.CSVExport(csvToken);
                CSVPreviewBox.Text = csvString;
            }
        }

        protected void downLoadButton_Click(object sender, EventArgs e)
        {
            string sGenName = "CsvExport.csv";
            string sCsvExport = CSVPreviewBox.Text;

            Byte[] bytes = new byte[sCsvExport.Length * sizeof(char)];
            System.Buffer.BlockCopy(sCsvExport.ToCharArray(), 0, bytes, 0, bytes.Length);
            Response.AddHeader("Content-disposition", "attachment; filename=" + sGenName);
            Response.ContentType = "application/octer-stream";
            Response.BinaryWrite(bytes);
            Response.End();
        }
    }
}