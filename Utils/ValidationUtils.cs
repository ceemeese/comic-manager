namespace Utils;
using System.Text.RegularExpressions;

public static class ValidationUtils
{

    public static bool IsValidMail(string email)
    { 
        string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";
        return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
    }


    public static bool IsValidPassword(string password)
    { 
        var hasNumber = new Regex(@"[0-9]+");
        var hasUpperChar = new Regex(@"[A-Z]+");
        var hasMinimum8Chars = new Regex(@".{8,}");

        return hasNumber.IsMatch(password) && 
                hasUpperChar.IsMatch(password) && 
                hasMinimum8Chars.IsMatch(password);
    }
    
}