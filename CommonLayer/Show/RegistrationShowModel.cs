//-----------------------------------------------------------------------
// <copyright file="RegistrationShowModel.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace CommonLayer.Show
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Registration show model 
    /// </summary>
    public class RegistrationShowModel
    {
        [Required(ErrorMessage ="First name is required")]
        [RegularExpression("^([a-zA-Z]{2,20})$", ErrorMessage = "First name should contain atleast 2 or between 20 letters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [RegularExpression("^([a-zA-Z]{2,20})$", ErrorMessage = "Last name should contain atleast 2 or between 20 letters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email Is Required")]
        [RegularExpression("^([a-z0-9](.?[a-z0-9]){5,}@g(oogle)?mail.com)$", ErrorMessage = "Enter valid gmail. eg. abc123@gmail.com")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Mobile Number Is Required")]
        [RegularExpression("^([789][0-9]{9})$", ErrorMessage = "Please enter 10 digit mobile no. starts from 7 or 8 or 9")]
        public string MobileNumber { get; set; }
    }
}
