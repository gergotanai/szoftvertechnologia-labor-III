namespace UniversityWebAPI.Models
{
    public class Classroom
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public ICollection<Student> Students { get; set; }
        public Teacher Teacher { get; set; }
    }
}
