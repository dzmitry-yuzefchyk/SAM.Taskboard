namespace SAM.Taskboard.Logic.Utility
{
    public enum UserServiceResult
    {
        success = 0,
        error = 1,
        userAlreadyExists = 2,
        emailNotSent = 3,
        userNotExist = 4,
        emailAlreadyConfirmed = 5,
        emailNotConfirmed = 6
    }
}
