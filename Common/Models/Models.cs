using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    public class UserModel
    {
        [Required]
        [MinLength(4, ErrorMessage = "User Name must have at least 4 chars")]
        [MaxLength(40, ErrorMessage = "Nome must have max 16 chars")]
        public string UserName { get; set; }

        [Required]
        [MinLength(4, ErrorMessage = "Department must have at least 4 chars")]
        [MaxLength(40, ErrorMessage = "Department must have max 12 chars")]
        public string Depto { get; set; }

        [Required]
        public string Role { get; set; }

        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        public string Email { get; set; }

        [Editable(true)]
        public int RowId { get; set; }

        public UserModel()
        {
            UserName = "";
            Depto = "";
            Role = "";
            Email = "";
            RowId = 0;
        }
        public UserModel( string userName, string depto, string role, string email, int rowId)
        {
            UserName = userName;
            Depto = depto;
            Role = role;
            Email = email;
            RowId = rowId;
        }
    }    
}