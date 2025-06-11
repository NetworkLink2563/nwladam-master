using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWL_AdAm_DML.ViewModel
{
    public class FetchRespModel
    {
        public const string success = "success";
        public const string error = "error";
        public const string warning = "warning";
        public const string info = "info";
        public const string question = "question";

        public string state { get; set; } = string.Empty;

        private string _title = string.Empty;
        public string title
        {
            get
            {
                if (string.IsNullOrEmpty(_title))
                {
                    switch (state)
                    {
                        case success:
                            return "ดำเนินการสำเร็จ";
                        case error:
                            return "เกิดข้อผิดพลาด";
                        case warning:
                            return "แจ้งเตือน";
                        case info:
                            return "แจ้งเตือน";
                        case question:
                            return "ยืนยันรายการ";
                        default:
                            return "";
                    }
                }
                else
                {
                    return _title;
                }
            }
            set
            {
                _title = value;
            }
        }
        public string message { get; set; } = string.Empty;
    }

    public class FetchRespModel<T> : FetchRespModel
    {
        public T data { get; set; }
    }

    public class FetchRespPagingModel<T> : FetchRespModel
    {
        public T data { get; set; }
        public int pagingTotalPage { get; set; }
    }
}
