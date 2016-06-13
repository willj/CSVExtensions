using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace MBW
{
    public class CsvFileResult<T> : FileResult
    {
        private IEnumerable<string> data;

        public CsvFileResult(IEnumerable<T> data, string fileName)
            : base("text/csv")
        {
            this.data = data.ToCsv();
            this.FileDownloadName = fileName;
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            foreach (var item in data)
            {
                response.Write(item);
            }
        }
    }
}
