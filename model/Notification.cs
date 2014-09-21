using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.model
{
    public class Notification
    {
        public long Id { get; set; }
        public long CharacterId { get; set; }
        public virtual Character Character { get; set; }
        public string NotificationType { get; set; }
        public string NotificationText { get; set; }
        public DateTime? LastNotifiedDate { get; set; }
    }
}
