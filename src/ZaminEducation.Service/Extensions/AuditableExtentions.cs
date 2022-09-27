using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZaminEducation.Domain.Commons;
using ZaminEducation.Domain.Enums;
using ZaminEducation.Service.Helpers;

namespace ZaminEducation.Service.Extensions
{
    public static class AuditableExtentions
    {
        public static void Create(this Auditable auditable)
        {
            auditable.State = ItemState.Created;
            auditable.CreatedBy = HttpContextHelper.UserId;
            auditable.CreatedAt = DateTime.UtcNow;
        }

        public static void Update(this Auditable auditable)
        {
            auditable.State = ItemState.Updated;
            auditable.UpdatedBy = HttpContextHelper.UserId;
            auditable.UpdatedAt = DateTime.UtcNow;
        }

        public static void Delete(this Auditable auditable)
        {
            auditable.State = ItemState.Deleted;
            auditable.UpdatedBy = HttpContextHelper.UserId;
            auditable.UpdatedAt = DateTime.UtcNow;
        }
    }
}
