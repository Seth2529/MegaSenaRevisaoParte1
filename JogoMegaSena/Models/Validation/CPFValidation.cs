using System;
using System.ComponentModel.DataAnnotations;
namespace JogoMegaSena.Models.Validation
{
    public class CPFValidation:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {

            if (value == null)
            {
                return new ValidationResult("O CPF deve conter 11 dígitos numéricos."); // Nulo é inválido
            }

            string cpf = value.ToString().Replace(".", "").Replace("-", "");  // Remove pontos e traços

            if (!IsDigitsOnly(cpf) || cpf.Length != 11)
            {
                return new ValidationResult("O CPF deve conter exatamente 11 dígitos numéricos.");
            }

            // Verificar dígitos repetidos
            if (IsRepeatingDigits(cpf))
            {
                return new ValidationResult("O CPF não pode haver todos os dígitos iguais.");
            }

            // Cálculo dos dígitos verificadores
            int sum = 0;
            for(int i = 0;i<9;i++)
            {
                sum += int.Parse(cpf[i].ToString()) * (10-i);
            }

            int remainder = sum % 11;
            int fistVerifier = remainder < 2 ? 0:11 - remainder;

            if (int.Parse(cpf[9].ToString()) != fistVerifier)
            {
                return new ValidationResult("CPF Invalido");
            }

            sum = 0;
            for(int i = 0; i < 10; i++)
            {
                sum += int.Parse(cpf[i].ToString()) * (11 - i);
            }

            remainder = sum % 11;
            int secondVerifier = remainder < 2 ? 0 : 11 - remainder;

            if (int.Parse(cpf[10].ToString())!= secondVerifier)
            {
                return new ValidationResult("CPF Invalido");
            }

            return ValidationResult.Success;

        }
        #region Métodos
        private bool IsDigitsOnly(string value)
        {
            foreach (char c in value)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsRepeatingDigits(string value)
        {
            char FirstChar = value[0];
            foreach (char c in value) 
            {
                if (c != FirstChar)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
    }
}
