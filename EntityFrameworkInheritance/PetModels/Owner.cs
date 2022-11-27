using System.Text;

namespace PetModels
{
    public class Owner
    {
        public int Id { get; set; }
        public ICollection<Pet> Pets { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var p in Pets)
                stringBuilder.AppendLine(p.ToString());

            return stringBuilder.ToString();
        }
    }
}
