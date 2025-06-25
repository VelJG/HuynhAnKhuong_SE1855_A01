using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public partial class Employee
    {
        public Employee() { }

        public Employee(int employeeId, string name, string userName, string password, string jobTitle, DateTime birthDate, DateTime hireDate, string address)
        {
            EmployeeId = employeeId;
            Name = name;
            UserName = userName;
            Password = password;
            JobTitle = jobTitle;
            BirthDate = birthDate;
            HireDate = hireDate;
            Address = address;
        }

        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string JobTitle { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime HireDate { get; set; }
        public string Address { get; set; }
    }
}
