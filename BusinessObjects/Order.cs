using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public partial class Order
    {
        public Order() { }

        public Order(int orderId, int customerId, int employeeId, DateTime orderDate)
        {
            OrderId = orderId;
            CustomerId = customerId;
            EmployeeId = employeeId;
            OrderDate = orderDate;
        }

        public int OrderId {  get; set; }   
        public int CustomerId {  get; set; }
        public int EmployeeId { get; set; }
        public DateTime OrderDate { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
