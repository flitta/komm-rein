using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.model
{
    public static class ContextItemHelper
    {
        public static ContextItem AddCreatedInfo(this ContextItem item, string sid)
        {
            item.CreatedDate = DateTime.Now;
            item.OwnerSid = sid;
            item.CreatedBySid = sid;

            return item;
        }
    }
}
