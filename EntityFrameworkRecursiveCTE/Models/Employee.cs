namespace EntityFrameworkRecursiveCTE.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public Employee Manager { get; set; }
        public int? ManagerId { get; set; }
        public List<Employee> Reports { get; set; }
    }
}
