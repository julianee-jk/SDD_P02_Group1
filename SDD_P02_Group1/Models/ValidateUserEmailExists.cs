using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SDD_P02_Group1.DAL;

namespace SDD_P02_Group1.Models
{
    public class ValidateUserEmailExists : ValidationAttribute
    {
        private UserDAL userContext = new UserDAL();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Get the email value to validate
            string email = Convert.ToString(value);

            // Casting the validation context to the "Competitor" model class
            User user = (User)validationContext.ObjectInstance;

            // Get the Judge Id from the judge instance
            int userId = user.UserId;

            if (userContext.IsEmailExist(email, userId))
                // validation failed
                return new ValidationResult("Email address already exists!");

            else
                // validation passed 
                return ValidationResult.Success;
        }
    }
}
