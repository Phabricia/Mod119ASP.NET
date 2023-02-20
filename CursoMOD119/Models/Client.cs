using System.ComponentModel.DataAnnotations;

namespace CursoMOD119.Models
{
    public class Client
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public bool Active { get; set; }

    }
}
