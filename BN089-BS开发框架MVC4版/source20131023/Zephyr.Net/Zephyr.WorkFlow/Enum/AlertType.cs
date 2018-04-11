using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zephyr.WorkFlow
{
    public enum AlertType
    {
        //@0=不接收@1=短信@2=邮件@3=内部消息@4=QQ消息@5=RTX消息@6=MSN消息 
        none,
        show,
        redirect
    }
}
