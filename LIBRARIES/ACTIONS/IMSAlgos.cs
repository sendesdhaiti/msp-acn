namespace ACTIONS
{
    public interface IMSAlgos
    {
        /// <summary>
        /// Returns the password sent and the hashed password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        (string, string) HashPassword(string password);
        bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt);
    }
}