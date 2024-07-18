using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TrainingCenterLib.Entities;
using static System.Collections.Specialized.BitVector32;

namespace TrainingCenterLib.Repository
{
    public static class UserInfo
    {
        public static int GlobalUserID {  get; set; }
        public static void CreateAudit(ActionType actionType, Action action,int ID, MasterEntity masterEntity, string EntityRecord)
        {


            var auditTrail = new AuditTrail
            {
                ActionTypeID = (int)actionType,
                ActionID = (int)action,
                UserID = ID,
                IpAddress = LocalIpAddress(),
                TransactionTime = DateTime.Now,
                EntityID = ((int)masterEntity),
                EntityRecord = EntityRecord
            };

            using (var context = new TrainingCenterLibDbContext())
            {
                context.AuditTrails.Add(auditTrail);
                context.SaveChanges();
            }

        }

        public static string LocalIpAddress() 
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            return host.AddressList.FirstOrDefault(i => i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
        }

        
    }
}
